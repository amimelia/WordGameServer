using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WordGameServer.Code.Communication.Events
{
    public class GameEvent
    {
        public string EventKey { get; set; }
        public string EventAuthor { get; set; }
        public dynamic EventData { get; set; }


        public bool IsSameEvent(string eventKey)
        {
            return eventKey.Equals(EventKey);
        }
    }
}