using System;
using System.Diagnostics;
using Interfaces;

namespace Models.LaserPatterns
{
    public class RandomDots : ILaserPattern
    {
        private readonly Laser _laser;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;
        private readonly LaserAnimationStatus _laserAnimationStatus;

        public RandomDots(Laser laser, LaserPatternHelper laserPatternHelper, LaserSettings settings, LaserAnimationStatus laserAnimationStatus)
        {
            _laser = laser;
            _laserPatternHelper = laserPatternHelper;
            _settings = settings;
            _laserAnimationStatus = laserAnimationStatus;
        }

        public void Project(PatternOptions options)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < 1000; i++)
            {
                if (stopwatch.ElapsedMilliseconds > options.DurationMilliseconds && options.DurationMilliseconds != 0 || _laserAnimationStatus.AnimationCanceled) break;

                LaserColors colors = _laserPatternHelper.GetRandomLaserColors();

                _laser.SendTo(_laserPatternHelper.GetRandomXPosition(), _laserPatternHelper.GetRandomYPosition());
                _laser.On(colors);
                System.Threading.Thread.SpinWait(20000);
                _laser.Off();
            }
        }
    }
}
