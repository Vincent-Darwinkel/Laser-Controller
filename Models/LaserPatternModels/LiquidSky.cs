using Interfaces;

namespace Models.LaserPatternModels
{
    public class LiquidSky : ILaserPattern
    {
        private readonly Settings _settings;
        private readonly AnimationSpeed _animationSpeed = AnimationSpeed.Slow;

        public LiquidSky(Settings settings)
        {
            _settings = settings;
        }

        public void Project(AnimationSpeed animationSpeed)
        {
            
        }

        public AnimationSpeed GetAnimationSpeed()
        {
            return _animationSpeed;
        }
    }
}