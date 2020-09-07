using System;
using System.Collections.Generic;
using Interfaces;

namespace Models.LaserPatterns
{
    public class MovingDotsUpDown : ILaserPattern
    {
        private AnimationSpeed AnimationSpeed { get; } = AnimationSpeed.Medium;
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;

        public MovingDotsUpDown(SerialPortModel serialPortModel, LaserPatternHelper laserPatternHelper, LaserSettings settings)
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

            for (double i = 0; i < 8 * total; i += 0.010)
            {
                for (int line = 0; line < totalLines; line++)
                {
                    int x = Convert.ToInt32(Math.Cos(i + line) * Math.Abs(_settings.maxLeft));
                    int y = Convert.ToInt32(Math.Sin(i) * Math.Abs(2000));

                    _serialPortModel.SendCommand(new SerialCommand().Galvo(x, y));
                    _serialPortModel.SendCommand(new SerialCommand().Lasers(colors[line]));
                    System.Threading.Thread.SpinWait(40000);

                    _serialPortModel.SendCommand(new SerialCommand().LasersOff());
                }
            }

            _serialPortModel.Close();
        }

        public AnimationSpeed GetAnimationSpeed()
        {
            return AnimationSpeed;
        }
    }
}
