using System.Diagnostics;
using Enums;
using Interfaces;

namespace Models.LaserPatterns
{
    public class LiquidSkyDown : ILaserPattern
    {
        private readonly Laser _laser;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;
        private readonly LaserAnimationStatus _laserAnimationStatus;

        public LiquidSkyDown(Laser laser, LaserPatternHelper laserPatternHelper, LaserSettings settings, LaserAnimationStatus laserAnimationStatus)
        {
            _laser = laser;
            _laserPatternHelper = laserPatternHelper;
            _settings = settings;
            _laserAnimationStatus = laserAnimationStatus;
        }

        public void Project(PatternOptions options)
        {
            LaserColors colors = _laserPatternHelper.GetRandomLaserColors();
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            AnimationSpeed animationSpeed = options.AnimationSpeed;
            int iterations = 0;

            while (stopwatch.ElapsedMilliseconds < options.DurationMilliseconds || iterations < options.Total)
            {
                iterations++;

                for (int i = _settings.maxHeight; i > _settings.minHeight; i -= (int)animationSpeed)
                {
                    if (stopwatch.ElapsedMilliseconds > options.DurationMilliseconds && options.DurationMilliseconds != 0 || _laserAnimationStatus.AnimationCanceled) break;
                    if (options.AnimationSpeed == AnimationSpeed.NotSet) animationSpeed = _laserAnimationStatus.AnimationSpeed;

                    _laser.SendTo(_settings.maxLeft, i);
                    System.Threading.Thread.SpinWait(30000);
                    _laser.On(colors);

                    _laser.SendTo(_settings.maxRight, i);
                    System.Threading.Thread.SpinWait(30000);
                }
            }

            _laser.Off();
        }
    }
}