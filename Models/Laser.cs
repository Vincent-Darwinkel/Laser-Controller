namespace Models
{
    public class Laser
    {
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserSettings _settings;
        private int _y;

        public Laser(SerialPortModel serialPortModel, LaserSettings settings)
        {
            _serialPortModel = serialPortModel;
            _settings = settings;
        }

        /// <summary>
        /// Sends the galvos to the specified position, range -2000 / 2000 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SendTo(int x, int y)
        {
            if (x > _settings.maxRight) x = _settings.maxRight;
            if (x < _settings.maxLeft) x = _settings.maxLeft;
            if (y < _settings.minHeight) y = _settings.minHeight;
            if (y > _settings.maxHeight) y = _settings.maxHeight;

            _y = y;

            _serialPortModel.SendCommand(new SerialCommand().Galvo(x, y));
        }

        /// <summary>
        /// Turns the laser on with the specified colors range 0 / 255
        /// </summary>
        /// <param name="colors"></param>
        public void On(LaserColors colors)
        {
            if (colors.Red > 255) colors.Red = 255;
            if (colors.Red < 0) colors.Red = 0;

            if (colors.Green > 255) colors.Green = 255;
            if (colors.Green < 0) colors.Green = 0;

            if (colors.Blue > 255) colors.Blue = 255;
            if (colors.Blue < 0) colors.Blue = 0;

            int audienceScanPower = 80;

            var checkedColors = new LaserColors
            {
               Red = colors.Red,
               Green = colors.Green,
               Blue = colors.Blue
            };

            if (_y < 0 && colors.Red > audienceScanPower + 10)
                checkedColors.Red = audienceScanPower + 10;

            if (_y < 0 && colors.Green > audienceScanPower)
                checkedColors.Green = audienceScanPower;

            if (_y < 0 && colors.Blue > audienceScanPower + 7)
                checkedColors.Blue = audienceScanPower + 7;

            _serialPortModel.SendCommand(new SerialCommand().Lasers(checkedColors));
        }

        /// <summary>
        /// Turns the red green and blue laser off, not the show laser itself
        /// </summary>
        public void Off()
        {
            _serialPortModel.SendCommand(new SerialCommand().LasersOff());
        }
    }
}
