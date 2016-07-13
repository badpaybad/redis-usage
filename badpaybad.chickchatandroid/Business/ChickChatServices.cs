using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace badpaybad.chickchatandroid.Business
{
    public class ChickChatServices
    {
        private ISubscriber _subscriber;
        private IDatabase _db;
        
        public ChickChatServices()
        {
            RedisConnectionPool.Init(new chickchatandroid.Business.RedisConfig()
            {
                Host = "badpaybad.info",
                Port = 6379,
                Pwd = "badpaybad.info"
            });

            _subscriber = chickchatandroid.Business.RedisConnectionPool.CurrentConnection.GetSubscriber();
            _db = chickchatandroid.Business.RedisConnectionPool.CurrentConnection.GetDatabase();

         
        }

        public void RegisterGroup(string anonymousName, string anonymousTitle, out string channelKey)
        {
            channelKey = Guid.NewGuid().ToString();

            _subscriber.Publish(channelKey, DateTime.Now.Ticks.ToString());

            _db.HashSet(channelKey, "owner", anonymousName);
            _db.HashSet(channelKey, "title", anonymousTitle);
            _db.HashSet("anonymous_list", channelKey, channelKey);

            PublishMessage(channelKey, channelKey, anonymousTitle + "\r\n------------");

            _subscriber.Publish("chickchat_changed", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            _db.KeyExpire(channelKey, DateTime.Now.AddSeconds(30));
            _db.KeyExpire(channelKey+"_msg", DateTime.Now.AddSeconds(30));
        }

        public void RefreshListGroupChickChat(Action<List<ChickChatGroup>> listenerGroups)
        {
            _subscriber.Subscribe("chickchat_changed", (c, v) =>
            {
                var temp = ListGroupChickChat();
                listenerGroups(temp);
            });
        }

        public void JoinChat(string channelKey, Action<List<ChatMessage>> listenerMessages)
        {
            _subscriber.Subscribe(channelKey, (c, v) =>
            {
                var msgs = GetMessageseChat(channelKey);
                listenerMessages(msgs);
            });

            _subscriber.Publish(channelKey, channelKey + DateTime.Now.Ticks.ToString());

        }

        public List<ChatMessage> GetMessageseChat(string channelKey)
        {
            var all = _db.HashGetAll(channelKey + "_msg");
            return all.Select(i => new ChatMessage()
            {
                ChannelKey = channelKey,
                Date = i.Name,
                Message = i.Value
            }).ToList();
        }

        public void PublishMessage(string channelKey, string senderChannelKey, string msg)
        {
            msg = msg.Replace(":", " ");

            var sender = GetChickChat(senderChannelKey);
            var tempMsg = string.Format("{0}:{1}:{2}", sender.ChannelKey, sender.AnonymousName, msg);
            _db.HashSet(channelKey + "_msg", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), tempMsg);

            _subscriber.Publish(channelKey, channelKey+ DateTime.Now.Ticks.ToString());
        }

        public void PingKeepAlive(string channelKey)
        {
            _db.KeyExpire(channelKey,DateTime.Now.AddSeconds(30));
            _db.KeyExpire(channelKey+"_msg",DateTime.Now.AddSeconds(30));
        }

        public void RemoveChat(string channelKey)
        {
            if (string.IsNullOrEmpty(channelKey)) return;

            _db.HashDelete(channelKey, "owner");
            _db.HashDelete(channelKey, "title");
            _db.HashDelete("anonymous_list", channelKey);
            _db.KeyDelete(channelKey);
            var allMsg = _db.HashGetAll(channelKey + "_msg");
            foreach (var he in allMsg)
            {
                _db.HashDelete(channelKey + "_msg", he.Name);
            }
            _db.KeyDelete(channelKey + "_msg");

            _subscriber.Publish("chickchat_changed", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

        }

        public List<ChickChatGroup> ListGroupChickChat()
        {
            var temp = new List<ChickChatGroup>();

            var all = _db.HashGetAll("anonymous_list");

            foreach (var he in all)
            {
                var chickChat = GetChickChat(he.Value);
                temp.Add(chickChat);
            }

            return temp;
        }


        public ChickChatGroup GetChickChat(string channelKey)
        {
            var chickChat = new ChickChatGroup();

            var all = _db.HashGetAll(channelKey);

            if (all.Length <= 0)
            {
                chickChat.IsOnline = false;
                return chickChat;
            }

            var dic = all.ToDictionary(k => k.Name, v => v.Value);
            if (dic == null || dic.Count == 0) return chickChat;

            chickChat.AnonymousName = dic["owner"];
            chickChat.AnonymousTitle = dic["title"];
            chickChat.ChannelKey = channelKey;
            chickChat.IsOnline = true;

            return chickChat;
        }

        public void MockChange()
        {
            _subscriber.Publish("chickchat_changed",  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

        }


        public void CleanAll()
        {
            var xxx = ListGroupChickChat();

            foreach (var cc in xxx)
            {
                RemoveChat(cc.ChannelKey);
            }
        }
    }

    public class ChickChatGroup
    {
        public string ChannelKey { get; set; }
        public string AnonymousName { get; set; }
        public string AnonymousTitle { get; set; }
        public bool IsOnline { get; set; }
    }

    public class ChatMessage
    {
        public string Message { get; set; }
        public string Date { get; set; }
        public string ChannelKey { get; set; }
    }
}