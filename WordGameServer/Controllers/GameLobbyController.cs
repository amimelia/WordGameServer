using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WordGameServer.Code.Lobby;

namespace WordGameServer.Controllers
{
    public class GameLobbyController : ApiController
    {

        [HttpGet]
        [HttpPost]
        [ActionName("GetAvaliableRooms")]
        public IHttpActionResult GetAvaliableRooms(string userName)
        {
            try
            {
                return Ok(LobbyManager.Instance.GetAvaliableRooms(userName));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [HttpPost]
        [ActionName("LeaveRoom")]
        public IHttpActionResult LeaveRoom(string userName)
        {
            try
            {
                LobbyManager.Instance.LeaveRoom(userName);
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [HttpPost]
        [ActionName("JoinRoom")]
        public IHttpActionResult JoinRoom(string userName, int roomId)
        {
            try
            {
                LobbyManager.Instance.JoinRoom(userName, roomId);
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [HttpPost]
        [ActionName("SendGameInvitiation")]
        public IHttpActionResult SendGameInvitiation(string userName, string userTo, int roomId)
        {
            try
            {
                LobbyManager.Instance.InvitePlayer(userName, userTo, roomId);
                return Ok("OK");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        [HttpGet]
        [HttpPost]
        [ActionName("GetRoom")]
        public IHttpActionResult GetRoom(string userName, int roomId)
        {
            try
            {
                var room = LobbyManager.Instance.GetRoomInfo(roomId);
                return Ok(room);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "error.getting.room");
            }

        }

        [HttpGet]
        [HttpPost]
        [ActionName("CreateRoom")]
        public IHttpActionResult CreateRoom(string userName, string roomName, string roomAccessibility)
        {
            try
            {
                return Ok(new { roomId = LobbyManager.Instance.CreateRoom(userName, roomName, roomAccessibility) });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
       
    }
}
