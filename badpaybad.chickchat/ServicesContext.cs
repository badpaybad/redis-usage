using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using badpaybad.redis.repository;

namespace badpaybad.chickchat
{
    public static class ServicesContext
    {
        public static string CurrentChannelKeyOwner { get; set; }
        public static string CurrentChannelKeyChatting { get; set; }

        public static ChickChatServices ChickChatServices;

        public static MessagesHanlder MessagesHanlder;

        public static void Boot()
        {
            ChickChatServices = new ChickChatServices();
            MessagesHanlder = new MessagesHanlder();
        }

        public static void Dispose()
        {
            ChickChatServices.RemoveChat(CurrentChannelKeyOwner);
        }
    }

    public class MessagesHanlder
    {
        ConcurrentDictionary<string, List<ChatMessage>> _data = new ConcurrentDictionary<string, List<ChatMessage>>();

        public void Register(string channelKey, List<ChatMessage> msgs)
        {
            _data[channelKey] = msgs;
        }

        public List<ChatMessage> GetAll(string channelKey)
        {
            List<ChatMessage> xxx;
            if (!_data.TryGetValue(channelKey, out xxx))
            {
                return new List<ChatMessage>();
            }

            return xxx ?? new List<ChatMessage>();
        }

        public List<string> ListChannelKeyRegistered()
        {
            return _data.Select(i => i.Key).ToList();
        }
    }
}