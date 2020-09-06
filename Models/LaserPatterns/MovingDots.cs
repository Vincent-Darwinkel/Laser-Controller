using System;
using System.Collections.Generic;
using Interfaces;

namespace Models.LaserPatterns
{
    public class MovingDots : ILaserPattern
    {
        private AnimationSpeed AnimationSpeed { get; } = AnimationSpeed.Medium;
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;

        public MovingDots(SerialPortModel serialPortModel, LaserPatternHelper laserPatternHelper, LaserSettings settings)
        {
            _serialPortModel = serialPortModel;
            _laserPatternHelper = laserPatternHelper;
            _settings = settings;
        }

        public void Project(int total)
        {
            int totalLines = 4;
            if (totalLines < 2) totalLines = 2;

            var colors = new List<LaserColors>();

            for (int i = 0; i < totalLines; i++)
                colors.Add(_laserPatternHelper.GetRandomLaserColors());

            for (double i = 0; i < 36; i += 0.02)
            {
                for (int line = 0; line < totalLines; line++)
                {
                    for (int l = 0; l < 15; l++)
                    {
                        _serialPortModel.SendCommand(new SerialCommand().Lasers(colors[line]));
                        int x = Convert.ToInt32(Math.Cos(i + line) * Math.Abs(_settings.maxLeft));
                        _serialPortModel.SendCommand(new SerialCommand().Galvo(x, _settings.minHeight));
                    }

                    _serialPortModel.SendCommand(new SerialCommand().LasersOff());
                }
            }
        }

        public AnimationSpeed GetAnimationSpeed()
        {
            return AnimationSpeed;
        }
    }
}
