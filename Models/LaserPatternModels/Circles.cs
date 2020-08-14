using System;
using Interfaces;

namespace Models.LaserPatternModels
{
    public class Circles : ILaserPattern
    {
        private readonly Settings _settings;
        private readonly AnimationSpeed _animationSpeed = AnimationSpeed.Medium;
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserPatternsHelper _laserPatternsHelper;

        public Circles(Settings settings, SerialPortModel serialPortModel, LaserPatternsHelper laserPatternsHelper)
        {
            _settings = settings;
            _serialPortModel = serialPortModel;
            _laserPatternsHelper = laserPatternsHelper;
        }

        public void Project(AnimationSpeed animationSpeed)
        {
            LaserColors laserColors = _laserPatternsHelper.GetRandomLaserColor();

            int iteration = 0;
            const double PI = 3.1415926535;
            double i, angle, x, y;

            for (int k = 0; k < (int)animationSpeed; k += 40)
            {
                for (i = 0; i < 360; i += 24)
                {
                    int differenceHeight = _settings.MinHeight + _settings.MaxHeight / 2;
                    int differenceWidth = _settings.MaxLeft + _settings.MaxRight / 2;

                    angle = i;
                    x = differenceHeight * Math.Cos(angle * PI / 180);
                    y = differenceWidth * Math.Sin(angle * PI / 180);

                    if (x > _settings.MaxRight && x < _settings.MaxLeft) x = 0;
                    if (y > _settings.MaxHeight && y < _settings.MinHeight) y = 0;
                    
                    _serialPortModel.SendCommand(new SerialCommand().Galvo(Convert.ToInt32(x), Convert.ToInt32(y)));
                }

                _serialPortModel.SendCommand(new SerialCommand().Lasers(laserColors.Red, laserColors.Green, laserColors.Blue));

                iteration += 50;
            }
        }

        public AnimationSpeed GetAnimationSpeed()
        {
            return _animationSpeed;
        }
    }
}