using System;
using Interfaces;

namespace Models.LaserPatterns
{
    public class LiquidSkyDown : ILaserPattern
    {
        private AnimationSpeed AnimationSpeed { get; } = AnimationSpeed.Medium;
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;

        public LiquidSkyDown(SerialPortModel serialPortModel, LaserPatternHelper laserPatternHelper, LaserSettings settings)
        {
            _serialPortModel = serialPortModel;
            _laserPatternHelper = laserPatternHelper;
            _settings = settings;
        }

        public void Project(int total)
        {
            LaserColors colors = _laserPatternHelper.GetRandomLaserColors();

            for (int k = 0; k < total; k++)
            {
                for (int i = _settings.maxHeight; i > _settings.minHeight; i -= (int)AnimationSpeed)
                {
                    _serialPortModel.SendCommand(new SerialCommand().Galvo(_settings.maxLeft, i));
                    System.Threading.Thread.SpinWait(30000);

                    _serialPortModel.SendCommand(new SerialCommand().Lasers(colors));

                    _serialPortModel.SendCommand(new SerialCommand().Galvo(_settings.maxRight, i));
                    System.Threading.Thread.SpinWait(30000);
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