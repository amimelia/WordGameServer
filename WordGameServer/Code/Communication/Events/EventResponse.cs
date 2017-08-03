using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WordGameServer.Code.Communication.Events
{
    public class EventResponse
    {
        public string eventAuthor { get; set; }
        public string eventKey { get; set; }
        public string eventJsonData { get; set; }
    }
}