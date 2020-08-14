using System;
using Interfaces;

namespace Models.LaserPatternModels
{
    public class RandomLine : ILaserPattern
    {
        private readonly Settings _settings;
        private readonly AnimationSpeed _animationSpeed = AnimationSpeed.Fast;
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserPatternsHelper _laserPatternsHelper;

        public RandomLine(Settings settings, SerialPortModel serialPortModel, LaserPatternsHelper laserPatternsHelper)
        {
            _settings = settings;
            _serialPortModel = serialPortModel;
            _laserPatternsHelper = laserPatternsHelper;
        }

        public void Project(AnimationSpeed animationSpeed)
        {
            LaserColors laserColors = _laserPatternsHelper.GetRandomLaserColor();

            for (int i = 0; i < (int)animationSpeed; i += 10)
            {
                int x = new Random(Guid.NewGuid().GetHashCode()).Next(_settings.MaxLeft, _settings.MaxRight);
                int y = new Random(Guid.NewGuid().GetHashCode()).Next(_settings.MinHeight, _settings.MaxHeight);

                if (x + 200 > _settings.MaxRight && x + 200 < _settings.MaxLeft) x = 0;
                if (y > _settings.MaxHeight && y < _settings.MinHeight) y = 0;

                _serialPortModel.SendCommand(new SerialCommand().Galvo(x + 100, y));
                _serialPortModel.SendCommand(new SerialCommand().Galvo(x - 100, y));
                _serialPortModel.SendCommand(new SerialCommand().Lasers(laserColors.Red, laserColors.Green, laserColors.Blue));
            }
        }

        public AnimationSpeed GetAnimationSpeed()
        {
            return _animationSpeed;
        }
    }
}