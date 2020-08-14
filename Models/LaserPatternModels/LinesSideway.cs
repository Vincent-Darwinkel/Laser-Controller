using Interfaces;

namespace Models.LaserPatternModels
{
    public class LinesSideway : ILaserPattern
    {
        private readonly Settings _settings;
        private readonly AnimationSpeed _animationSpeed = AnimationSpeed.Medium;
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserPatternsHelper _laserPatternsHelper;

        public LinesSideway(Settings settings, SerialPortModel serialPortModel, LaserPatternsHelper laserPatternsHelper)
        {
            _settings = settings;
            _serialPortModel = serialPortModel;
            _laserPatternsHelper = laserPatternsHelper;
        }

        public void Project(AnimationSpeed animationSpeed)
        {
            LaserColors laserColors = _laserPatternsHelper.GetRandomLaserColor();

            int xAxisCenter = (_settings.MaxRight + _settings.MaxLeft) / 2;
            int yAxisCenter = (_settings.MaxHeight + _settings.MinHeight) / 2;

            int iteration = 0;

            for (int i = 0; i < (int)animationSpeed; i += 30)
            {
                _serialPortModel.SendCommand(new SerialCommand().Galvo(xAxisCenter, yAxisCenter));
                _serialPortModel.SendCommand(new SerialCommand().Lasers(laserColors.Red, laserColors.Green, laserColors.Blue));

                int xMinus = xAxisCenter - iteration - 300;
                int xPlus = xAxisCenter + iteration + 300;

                if (xMinus > _settings.MaxLeft) _serialPortModel.SendCommand(new SerialCommand().Galvo(xMinus, yAxisCenter));
                else iteration = 0;

                _serialPortModel.SendCommand(new SerialCommand().Galvo(xAxisCenter, yAxisCenter));
                if (xPlus < _settings.MaxRight) _serialPortModel.SendCommand(new SerialCommand().Galvo(xPlus, yAxisCenter));
                else iteration = 0;

                iteration += 50;
            }
        }

        public AnimationSpeed GetAnimationSpeed()
        {
            return _animationSpeed;
        }
    }
}