using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WordGameServer.Code.Communication;

namespace WordGameServer.Controllers
{
    public class TestController : ApiController
    {

        public IEnumerable<string> GetAllProducts()
        {
            CommunicationManager.Instance.NotifyTestToAllUsers();
            return SignalRService.GameUsers.Values;
        }

        public IHttpActionResult GetProduct(string id)
        {
            return Ok(SignalRService.GetConnectionIdForUser(id));
        }
    }
}
