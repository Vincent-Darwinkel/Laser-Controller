using Enums;

namespace Interfaces
{
    public interface IGame
    {
        public void Start();
        public void Stop();
        public void Restart();
        public void Move(GameKey key);
        public int GetScore();
    }
}