using System;
using Interfaces;

namespace Models.LaserPatterns
{
    public class LiquidSkyLarging : ILaserPattern
    {
        private AnimationSpeed AnimationSpeed { get; } = AnimationSpeed.Medium;
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;

        public LiquidSkyLarging(SerialPortModel serialPortModel, LaserPatternHelper laserPatternHelper, LaserSettings settings)
        {
            _serialPortModel = serialPortModel;
            _laserPatternHelper = laserPatternHelper;
            _settings = settings;
        }

        public void Project(int total)
        {
            LaserColors colors = _laserPatternHelper.GetRandomLaserColors();
            int xCenter = (_settings.maxLeft + _settings.maxRight) / 2;
            
            for (int i = 0; i < total; i++)
            {
                int left = xCenter - 50;
                int right = xCenter + 50;

                int y = new Random(Guid.NewGuid().GetHashCode()).Next(_settings.minHeight, _settings.maxHeight);

                while (left > _settings.maxLeft || right < _settings.maxRight)
                {
                    _serialPortModel.SendCommand(new SerialCommand().Galvo(left -= Math.Abs((int) AnimationSpeed / 2), y));
                    System.Threading.Thread.SpinWait(40000);

                    _serialPortModel.SendCommand(new SerialCommand().Galvo(right += Math.Abs((int) AnimationSpeed / 2), y));
                    System.Threading.Thread.SpinWait(40000);
                }
            }

            _serialPortModel.SendCommand(new SerialCommand().LasersOff());
        }

        public AnimationSpeed GetAnimationSpeed()
        {
            return AnimationSpeed;
        }
    }
}