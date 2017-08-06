using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WordGameServer.Code.Game;
using WordGameServer.Code.Lobby;

namespace WordGameServer.Controllers
{
    public class GameController : ApiController
    {
        [HttpGet]
        [HttpPost]
        [ActionName("CreateGame")]
        public IHttpActionResult CreateGame(string userName, int roomId)
        {
            try
            {
                GameManager.Instance.CreateGame(LobbyManager.Instance.GetRoomById(roomId));
                return Ok("OK");
            }
            catch (Exception ex)
            {
                return Ok(ex.StackTrace);
            }

        }

        [HttpGet]
        [HttpPost]
        [ActionName("GetGame")]
        public IHttpActionResult GetGame(string userName, int gameId)
        {
            try
            {
                return Ok(GameManager.Instance.GetGameInfo(gameId));
            }
            catch (Exception ex)
            {
                return Ok(ex.StackTrace);
            }

        }

        [HttpGet]
        [HttpPost]
        [ActionName("WordGuessed")]
        public IHttpActionResult WordGuessed(string userName, int gameId, string word, int score)
        {
            try
            {
                GameManager.Instance.WordGuessed(userName, gameId, word, score);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.StackTrace);
            }

        }

        [HttpGet]
        [HttpPost]
        [ActionName("BetPlaced")]
        public IHttpActionResult BetPlaced(string userName, int gameId, int betAmount)
        {
            try
            {
                GameManager.Instance.BetPlaced(userName, gameId, betAmount);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.StackTrace);
            }

        }
    }
}
