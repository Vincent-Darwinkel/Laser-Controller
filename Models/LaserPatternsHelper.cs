using System;

namespace Models
{
    public class LaserPatternsHelper
    {
        private readonly Settings _settings;
        private readonly SerialPortModel _serialPortModel;

        public LaserPatternsHelper(Settings settings, SerialPortModel serialPortModel)
        {
            _settings = settings;
            _serialPortModel = serialPortModel;
        }

        public LaserColors GetRandomLaserColor()
        {
            Random random = new Random(DateTime.Now.Millisecond);
           
            int redPower = random.Next(114, _settings.RedPower);
            int greenPower = random.Next(82, _settings.GreenPower);
            int bluePower = random.Next(87, _settings.BluePower);

            return new LaserColors
            {
                Red = redPower,
                Green = greenPower,
                Blue = bluePower
            };
        }
    }
}