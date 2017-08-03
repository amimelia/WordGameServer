using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WordGameServer.Code.Communication;
using WordGameServer.Code.Lobby.Models;
using WordGameServer.DbModel;

namespace WordGameServer.Code.Lobby
{
    public class LobbyManager
    {
        #region Singleton 

        private static LobbyManager _instance = new LobbyManager();
        public static LobbyManager Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        private List<Room> _rooms = new List<Room>();

        public int CreateRoom(string userName, string roomName, string roomAccessibility)
        {
            Room room = new Room(userName, roomName, roomAccessibility);
            _rooms.Add(room);
            return room.RoomId;
        }

        public void LeaveRoom(string userName)
        {
            List<Room> roomsToRemove = new List<Room>();
            foreach (var room in _rooms)
            {
                if (room.Members.Contains(userName))
                {
                    room.Members.Remove(userName);
                    if (room.MembersCount == 0)
                    {
                        roomsToRemove.Add(room);
                    }
                    else
                    {
                        if (room.OwnerUserName == userName)
                        {
                            room.OwnerUserName = room.Members.ElementAt(0);
                        }
                    }
                    
                    CommunicationManager.Instance.NotifyRoomDataChanged(userName, room.RoomId);

                }
            }
            roomsToRemove.ForEach(toRemove => _rooms.Remove(toRemove));
        }

        public void JoinRoom(string userName, int roomId)
        {
            var roomToJoin = GetRoomById(roomId);
            if (roomToJoin == null)
            {
                throw new Exception("room.no.exists.anymore");
            }
            if (!roomToJoin.Members.Contains(userName))
                roomToJoin.Members.Add(userName);
            if (roomToJoin.InvitedUsers.Contains(userName))
            {
                roomToJoin.InvitedUsers.Remove(userName);
            }
            CommunicationManager.Instance.NotifyRoomDataChanged(userName, roomId);
        }

        internal void InvitePlayer(string userName, string userTo, int roomId)
        {
            var room = GetRoomById(roomId);
            if (room == null)
                return;
            if (!room.InvitedUsers.Contains(userName))
                room.InvitedUsers.Add(userName);
            CommunicationManager.Instance.NotifyUserInvited(userName, userTo, roomId);
        }

        internal Room GetRoomInfo(int roomId)
        {
            var room = GetRoomById(roomId);
            if (room == null)
                return null;
            List<user> roomUsers = new List<user>();
            foreach(var userName in room.Members)
            {
                roomUsers.Add(UsersManager.Instance.GetUserByName(userName));
            }
            room.MemberUsers = roomUsers;
            return room;
        }

        public Room GetRoomById(int roomId)
        {
            return _rooms.Where(room => roomId == room.RoomId).FirstOrDefault();
        }

        public List<Room> GetAvaliableRooms(string userName)
        {
            return _rooms.Where(room => room.IsPublicRoom() || room.InvitedUsers.Contains(userName)).ToList();
        }

    }
}
