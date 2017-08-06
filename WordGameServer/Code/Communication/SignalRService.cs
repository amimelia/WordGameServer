using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using WordGameServer.Code.Communication.Events;

namespace WordGameServer.Code.Communication
{
    public class SignalRService : Hub
    {
        public static ConcurrentDictionary<string, string> GameUsers = new ConcurrentDictionary<string, string>();// <userName, connectionId>
        private string userName = "";

        public override Task OnConnected()
        {
            DoConnect();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            GameUsers.TryRemove(Context.ConnectionId, out userName);
            if (stopCalled) // Client explicitly closed the connection
            {
                string id = Context.ConnectionId;
                
                //TODO user disconnected
            }
            else // Client timed out
            {
                // Do nothing here...
                // FromUsers.TryGetValue(Context.ConnectionId, out userName);            
                // Clients.AllExcept(Context.ConnectionId).broadcastMessage(new ChatMessage() { UserName = userName, Message = "I'm Offline By TimeOut"});                
            }

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            DoConnect();
            //TODO user reconnected
            return base.OnReconnected();
        }

        private void DoConnect()
        {
            userName = Context.Request.Headers["User-Name"];
            if (userName == null || userName.Length == 0)
            {
                userName = Context.QueryString["User-Name"]; // for javascript clients
            }
            GameUsers.TryAdd(Context.ConnectionId, userName);
        }

        [HubMethodName("PostEvent")]
        public void EventReceived(EventResponse response)
        {
            GameEvent gameEvent = new GameEvent();
            gameEvent.EventAuthor = response.eventAuthor;
            gameEvent.EventKey = response.eventKey;
            gameEvent.EventData = Json.Decode(response.eventJsonData);

            CommunicationManager.Instance.GameEventReceived(gameEvent);
        }

        public static string GetConnectionIdForUser(string username)
        {

            var user = GameUsers.FirstOrDefault(pair => pair.Value == username);
            //throw new SystemException("UserName " + username + "connection = " + user.Key);
            return user.Key;
        }

        [HubMethodName("test")]
        public void Test(string testStr)
        {
            if (testStr == "test")
            {
                Clients.All.PostEvent("test");// new ChatMessage() { UserName = "Test username", Message = "Test Msg" });
            }
        }

    }

   
}
