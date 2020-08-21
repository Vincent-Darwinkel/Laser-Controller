using System;
using System.Threading.Tasks;
using Interfaces;

namespace Models.LaserPatternModels
{
    public class LinesToSide : ILaserPattern
    {
        private readonly Settings _settings;
        private readonly AnimationSpeed _animationSpeed = AnimationSpeed.Fast;
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserPatternsHelper _laserPatternsHelper;

        public LinesToSide(Settings settings, SerialPortModel serialPortModel, LaserPatternsHelper laserPatternsHelper)
        {
            _settings = settings;
            _serialPortModel = serialPortModel;
            _laserPatternsHelper = laserPatternsHelper;
        }

        public async Task Project(AnimationSpeed animationSpeed)
        {
            int y = new Random(Guid.NewGuid().GetHashCode()).Next(_settings.MinHeight, _settings.MaxHeight);
            int increaseValue = 0;

            for (int i = 0; i < (int)animationSpeed; i += 25)
            {
                LaserColors laserColors = _laserPatternsHelper.GetRandomLaserColor();

                int left = Convert.ToInt32( _settings.MaxLeft + increaseValue * 8);
                int right = Convert.ToInt32(_settings.MaxRight - increaseValue * 8);

                if (left > _settings.MaxRight || right < _settings.MaxLeft)
                {
                    left = _settings.MaxLeft;
                    right = _settings.MaxRight;

                    increaseValue = 0;
                }

                int difference = left + right / 2;

                if (difference > 50)
                {
                    _serialPortModel.SendCommand(new SerialCommand().Galvo(left, y));
                    _serialPortModel.SendCommand(new SerialCommand().Lasers(laserColors.Red, laserColors.Green, laserColors.Blue));
                    _serialPortModel.SendCommand(new SerialCommand().Lasers(0, 0, 0));

                    _serialPortModel.SendCommand(new SerialCommand().Galvo(right, y));
                    _serialPortModel.SendCommand(new SerialCommand().Lasers(laserColors.Red, laserColors.Green, laserColors.Blue));
                    _serialPortModel.SendCommand(new SerialCommand().Lasers(0, 0, 0));
                }

                increaseValue += 50;
            }
        }

        public AnimationSpeed GetAnimationSpeed()
        {
            return _animationSpeed;
        }
    }
}
