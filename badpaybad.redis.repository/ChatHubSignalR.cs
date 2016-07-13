using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using badpaybad.redis.repository;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System.Web.Routing;
using Microsoft.AspNet.SignalR.Hubs;

[assembly: OwinStartup(typeof (Startup))]
namespace badpaybad.redis.repository
{
    public static class SignalrContext 
    {
        public static ChickChatServices ChickChatServices;

        public static CustomTaskScheduler TaskScheduler;

        public static void Boot()
        {
            ChickChatServices = new ChickChatServices();
            TaskScheduler = new CustomTaskScheduler();

            TaskScheduler.Add("chickchat_refresh_group_list", () => { ChickChatServices.MockChange(); }, 5);
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            // app.MapSignalR("~/signalr", new HubConfiguration());
            app.MapSignalR();
            //var hubConfiguration = new HubConfiguration();
            //hubConfiguration.EnableDetailedErrors = true;
            //hubConfiguration.EnableJavaScriptProxies = true;

            //app.MapSignalR("/signalr", hubConfiguration);
        }
    }

    [HubName("chatHubSignalR")]
    public class ChatHubSignalR : Hub
    {
        public void JoinChat(string channelKey)
        {
            SignalrContext.ChickChatServices.JoinChat(channelKey,
                list => { Clients.Caller.JoinChat(channelKey, list); });

            SignalrContext.ChickChatServices.RefreshListGroupChickChat(
               list =>
               {
                   var filtered = list.Where(i=>i.IsOnline).ToList();
                   Clients.All.ListGroup(channelKey, filtered);
               });
        }

        public void PingKeepAlive(string channelKey)
        {
            SignalrContext.ChickChatServices.PingKeepAlive(channelKey);
        }
    }
}