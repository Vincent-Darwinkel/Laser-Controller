using System.Diagnostics;
using System.Threading.Tasks;
using Interfaces;

namespace Models.LaserPatternModels
{
    public class TestPattern : ILaserPattern
    {
        private readonly Settings _settings;
        private readonly AnimationSpeed _animationSpeed = AnimationSpeed.Slow;
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserPatternsHelper _laserPatternsHelper;

        public TestPattern(Settings settings, SerialPortModel serialPortModel, LaserPatternsHelper laserPatternsHelper)
        {
            _settings = settings;
            _serialPortModel = serialPortModel;
            _laserPatternsHelper = laserPatternsHelper;
        }

        public async Task Project(AnimationSpeed animationSpeed)
        {
            LaserColors laserColors = _laserPatternsHelper.GetRandomLaserColor();

            int height = _settings.MaxHeight;

            for (int i = 0; i < 10000; i++)
            {
                _serialPortModel.SendCommand(new SerialCommand().Galvo(_settings.MaxLeft, height));
                _serialPortModel.SendCommand(new SerialCommand().Lasers(laserColors.Red, laserColors.Green, laserColors.Blue));
                _serialPortModel.SendCommand(new SerialCommand().Galvo(_settings.MaxRight, height));
                if (height - 15 > _settings.MinHeight) height -= 15;
            }
        }

        public AnimationSpeed GetAnimationSpeed()
        {
            return _animationSpeed;
        }
    }
}