using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using badpaybad.redis.repository;

namespace badpaybad.chickchatweb.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string txtUid, string txtTitle)
        {
            string channelKey;
            SignalrContext.ChickChatServices.RegisterGroup(txtUid, txtTitle, out channelKey);
            var chickChatGroup = SignalrContext.ChickChatServices.GetChickChat(channelKey);

            return new JsonResult()
            {
                Data = new
                {
                    ChannelKey = channelKey,
                    IsOk = true,
                    ChickChatGroup = chickChatGroup
                }
            };
        }

        [HttpPost]
        public ActionResult Logout(string channelKey, string senderChannelKey)
        {
        
            SignalrContext.ChickChatServices.RemoveChat(channelKey);

            return new JsonResult()
            {
                Data = new
                {
                    ChannelKey = channelKey,
                    IsOk = true,
                   
                }
            };
        }

        [HttpPost]
        public ActionResult PushMessage(string channelKey, string senderChannelKey, string message)
        {

            SignalrContext.ChickChatServices.PublishMessage(channelKey,senderChannelKey,message);

            return new JsonResult()
            {
                Data = new
                {
                    ChannelKey = channelKey,
                    IsOk = true,

                }
            };
        }

        [HttpPost]
        public ActionResult JoinChat(string channelKey)
        {

           var ccg= SignalrContext.ChickChatServices.GetChickChat(channelKey);

            return new JsonResult()
            {
                Data = new
                {
                    ChannelKey = channelKey,
                    ChickChatGroup= ccg,
                    IsOk = true,

                }
            };
        }
    }
}