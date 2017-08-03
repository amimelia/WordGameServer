using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WordGameServer.DbModel;

namespace WordGameServer.Code.Lobby.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string OwnerUserName { get; set; }
        public List<string> Members { get; set; }
        public List<string> InvitedUsers { get; set; }
        public string RoomName { get; set; }
        public List<user> MemberUsers { get; set; }

        public int MembersCount
        {
            get
            {
                return Members.Count;
            }
        }

        public string Accessibility { get; set; }
        public Room()
        {
            RoomId = GetNextRoomId();
            Members = new List<string>();
            InvitedUsers = new List<string>();
        }

        public Room(string ownerUserName, string roomName, string accessibility):this()
        {
            OwnerUserName = ownerUserName;
            RoomName = roomName;
            Accessibility = accessibility;
            Members.Add(OwnerUserName);
            InvitedUsers.Add(OwnerUserName);
        }

        public bool IsPublicRoom()
        {
            return PUBLIC_ROOM.Equals(Accessibility, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool IsPrivateRoom()
        {
            return !IsPublicRoom();
        }


        public static string PUBLIC_ROOM = "public";
        public static string PRIVATE_ROOM = "private";

        private static int _RoomIdCounter = 1;
        private static object _roomIdCounterLock = new object();
        public static int GetNextRoomId()
        {
            lock (_roomIdCounterLock)
            {
                _RoomIdCounter++;
                return _RoomIdCounter;
            }
        }

    }
}