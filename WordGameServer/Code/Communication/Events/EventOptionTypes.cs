using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WordGameServer.Code.Communication.Events
{
    public class EventOptionTypes
    {
        public static string ROOM_INFO_CHANGED_EVENT = "game.event.room.changed";
        public static string GAME_INVITATION_EVENT = "game.event.invitation.receved";
        public static string GAME_EVENT_PING = "game.event.ping";
        public static string GAME_CREATE_EVENT = "game.created.event";
    }
}