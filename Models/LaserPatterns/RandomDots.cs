using System;
using Interfaces;

namespace Models.LaserPatterns
{
    public class RandomDots : ILaserPattern
    {
        private AnimationSpeed AnimationSpeed { get; } = AnimationSpeed.Medium;
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;

        public RandomDots(SerialPortModel serialPortModel, LaserPatternHelper laserPatternHelper, LaserSettings settings)
        {
            _serialPortModel = serialPortModel;
            _laserPatternHelper = laserPatternHelper;
            _settings = settings;
        }

        public void Project(int total)
        {
            for (int i = 0; i < 1000; i++)
            {
                LaserColors colors = _laserPatternHelper.GetRandomLaserColors();

                _serialPortModel.SendCommand(new SerialCommand().Galvo(_laserPatternHelper.GetRandomXPosition(), _laserPatternHelper.GetRandomYPosition()));
                
                _serialPortModel.SendCommand(new SerialCommand().Lasers(colors));
                System.Threading.Thread.SpinWait(20000);
                _serialPortModel.SendCommand(new SerialCommand().LasersOff());
            }

            _serialPortModel.SendCommand(new SerialCommand().LasersOff());
        }

        public AnimationSpeed GetAnimationSpeed()
        {
            return AnimationSpeed;
        }
    }
}
