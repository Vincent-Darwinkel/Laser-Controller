using Interfaces;

namespace Models.LaserPatterns
{
    public class TraingleSwitching : ILaserPattern
    {
        private AnimationSpeed AnimationSpeed { get; } = AnimationSpeed.Medium;
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;

        public TraingleSwitching(SerialPortModel serialPortModel, LaserPatternHelper laserPatternHelper, LaserSettings settings)
        {
            _serialPortModel = serialPortModel;
            _laserPatternHelper = laserPatternHelper;
            _settings = settings;
        }

        public void Project(int total)
        {
            LaserColors colors = _laserPatternHelper.GetRandomLaserColors();
            int xCenter = (_settings.maxRight + _settings.maxLeft) / 2;

            for (int k = 0; k < total * 3; k++)
            {
                for (int i = 0; i < 80; i += (int) AnimationSpeed / 2)
                {
                    _serialPortModel.SendCommand(new SerialCommand().Galvo(_settings.maxLeft, _settings.minHeight));
                    System.Threading.Thread.SpinWait(40000);
                    _serialPortModel.SendCommand(new SerialCommand().Lasers(colors));

                    _serialPortModel.SendCommand(new SerialCommand().Galvo(xCenter, _settings.maxHeight));
                    System.Threading.Thread.SpinWait(40000);
                }

                for (int i = 0; i < 80; i += (int)AnimationSpeed / 2)
                {
                    _serialPortModel.SendCommand(new SerialCommand().Galvo(_settings.maxRight, _settings.minHeight));
                    System.Threading.Thread.SpinWait(40000);
                    _serialPortModel.SendCommand(new SerialCommand().Lasers(colors));

                    _serialPortModel.SendCommand(new SerialCommand().Galvo(xCenter, _settings.maxHeight));
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
