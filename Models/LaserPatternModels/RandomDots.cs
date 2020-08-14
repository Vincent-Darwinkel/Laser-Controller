using System;
using Interfaces;

namespace Models.LaserPatternModels
{
    public class RandomDots : ILaserPattern
    {
        private readonly Settings _settings;
        private readonly AnimationSpeed _animationSpeed = AnimationSpeed.Fast;
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserPatternsHelper _laserPatternsHelper;

        public RandomDots(Settings settings, SerialPortModel serialPortModel, LaserPatternsHelper laserPatternsHelper)
        {
            _settings = settings;
            _serialPortModel = serialPortModel;
            _laserPatternsHelper = laserPatternsHelper;
        }

        public void Project(AnimationSpeed animationSpeed)
        {
            for (int i = 0; i < (int)animationSpeed; i += 25)
            {
                int x = new Random(Guid.NewGuid().GetHashCode()).Next(_settings.MaxLeft, _settings.MaxRight);
                int y = new Random(Guid.NewGuid().GetHashCode()).Next(_settings.MinHeight, _settings.MaxHeight);

                LaserColors laserColors = _laserPatternsHelper.GetRandomLaserColor();

                _serialPortModel.SendCommand(new SerialCommand().Galvo(x, y));
                _serialPortModel.SendCommand(new SerialCommand().Lasers(laserColors.Red, laserColors.Green, laserColors.Blue));
            }
        }

        public AnimationSpeed GetAnimationSpeed()
        {
            return _animationSpeed;
        }
    }
}