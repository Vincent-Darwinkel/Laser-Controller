using System.Diagnostics;
using System.Threading.Tasks;
using Interfaces;

namespace Models.LaserPatternModels
{
    public class LiquidSky : ILaserPattern
    {
        private readonly Settings _settings;
        private readonly AnimationSpeed _animationSpeed = AnimationSpeed.Slow;
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserPatternsHelper _laserPatternsHelper;

        public LiquidSky(Settings settings, SerialPortModel serialPortModel, LaserPatternsHelper laserPatternsHelper)
        {
            _settings = settings;
            _serialPortModel = serialPortModel;
            _laserPatternsHelper = laserPatternsHelper;
        }

        public async Task Project(AnimationSpeed animationSpeed)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            LaserColors laserColors = _laserPatternsHelper.GetRandomLaserColor();

            int height = _settings.MaxHeight;

            while (stopwatch.ElapsedMilliseconds < (int) animationSpeed)
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