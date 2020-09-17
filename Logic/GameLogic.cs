using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic
{
    public class GameLogic
    {
        private IGame _game;
        private readonly List<IGame> _games = new List<IGame>();

        private readonly IServiceProvider _serviceProvider;

        public GameLogic(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            try
            {
                var games = AppDomain.CurrentDomain.GetAssemblies().SelectMany(e => e.GetTypes())
                    .Where(x => typeof(IGame).IsAssignableFrom(x) && !x.IsInterface);

                foreach (var game in games)
                    _games.Add((IGame)ActivatorUtilities.CreateInstance(_serviceProvider, game));
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<string> GetGames()
        {
            return _games.Select(game => game.GetType().Name).ToList();
        }

        public void StartGame(string gameName)
        {
            _game = _games.Find(game => game.GetType().Name == gameName);
            _game?.Start();
        }

        public void StopGame()
        {
            _game.Stop();
        }

        public void RestartGame()
        {
            _game.Restart();
        }

        public void Move(GameKey key)
        {
            _game.Move(key);
        }

        public int GetScore()
        {
            return _game.GetScore();
        }
    }
}
