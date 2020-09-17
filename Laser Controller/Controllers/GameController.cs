using System.Collections.Generic;
using System.Threading.Tasks;
using Enums;
using Logic;
using Microsoft.AspNetCore.Mvc;

namespace Laser_Controller.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameLogic _gameLogic;

        public GameController(GameLogic gameLogic)
        {
            _gameLogic = gameLogic;
        }

        [HttpGet("all")]
        public List<string> GetGames()
        {
            return _gameLogic.GetGames();
        }

        [HttpPost("start/{gameName}")]
        public void StartGame(string gameName)
        {
            new Task(() => _gameLogic.StartGame(gameName)).Start();
        }

        [HttpPost("stop")]
        public void StopGame()
        {
            _gameLogic.StopGame();
        }

        [HttpPost("restart")]
        public void RestartGame()
        {
            _gameLogic.RestartGame();
        }

        [HttpPost("move/{key}")]
        public void Move(GameKey key)
        {
            _gameLogic.Move(key);
        }

        [HttpPost("score")]
        public int GetScore()
        {
            return _gameLogic.GetScore();
        }
    }
}