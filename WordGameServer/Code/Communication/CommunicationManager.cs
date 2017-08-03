using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WordGameServer.Code.Communication.Events;
using WordGameServer.Code.Lobby;

namespace WordGameServer.Code.Communication
{
    public class CommunicationManager
    {
        #region Singleton
        public static CommunicationManager _instance = new CommunicationManager();
        public static CommunicationManager Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        internal void NotifyUserInvited(string userName, string userTo, int invitedRoomId)
        {
            SendEventToClient(userTo, new GameEvent { EventAuthor = userName, EventKey = EventOptionTypes.GAME_INVITATION_EVENT,
                EventData = new {
                    roomId = invitedRoomId,
                    inviteAuthor = UsersManager.Instance.GetUserByName(userName) 
                } });
        }

        internal void NotifyRoomDataChanged(string userName, int roomId)
        {
            var room = LobbyManager.Instance.GetRoomById(roomId);
            if (room == null || room.MembersCount == 0)
                return;

            foreach(var user in room.Members)
            {
                if (user != userName)
                {
                    SendEventToClient(user, new GameEvent { EventAuthor = userName, EventData = "", EventKey = EventOptionTypes.ROOM_INFO_CHANGED_EVENT });
                }
            }
        }


        public void SendEventToClient(string userToSend, GameEvent gameEvent)
        {
            EventResponse eventToSend = new EventResponse();
            eventToSend.eventAuthor = gameEvent.EventAuthor;
            eventToSend.eventKey = gameEvent.EventKey;
            var eventJsonData = Newtonsoft.Json.JsonConvert.SerializeObject(gameEvent.EventData);
            eventToSend.eventJsonData = eventJsonData;
            var communicationService = GlobalHost.ConnectionManager.GetHubContext<SignalRService>();
            communicationService.Clients.Client(SignalRService.GetConnectionIdForUser(userToSend)).PostEvent(eventToSend);
        }
    }
}