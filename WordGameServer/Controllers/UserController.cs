using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WordGameServer.Code;
using WordGameServer.DbModel;

namespace WordGameServer.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        [ActionName("GetUser")]
        public IHttpActionResult GetUser(string userUid)
        {
            var curUser = UsersManager.Instance.GetUserByUId(userUid);
            if (curUser == user.USER_NOT_FOUND)
            {
                return Ok(new {messageId = 0,  userId = 0, username = "not.found" });
            }
            else
            {
                return Ok(new { messageId = 1, userId = curUser.Id, username = curUser.UserName });
            }

        }
        [HttpGet]
        [ActionName("AddUserName")]
        public IHttpActionResult AddUserName(string useruid, string username)
        {
            int userId = UsersManager.Instance.AddUserName(useruid, username);
            if (userId == -1)
            {
                return Ok(new { messageId = -1, userId = 0, username = "user.already.exists" });
            }
            else
            {
                return GetUser(useruid);
            }
        }
    }
}
