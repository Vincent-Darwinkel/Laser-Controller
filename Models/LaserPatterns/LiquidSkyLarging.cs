using System;
using System.Diagnostics;
using Enums;
using Interfaces;

namespace Models.LaserPatterns
{
    public class LiquidSkyLarging : ILaserPattern
    {
        private readonly Laser _laser;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;
        private readonly LaserAnimationStatus _laserAnimationStatus;

        public LiquidSkyLarging(Laser laser, LaserPatternHelper laserPatternHelper, LaserSettings settings, LaserAnimationStatus laserAnimationStatus)
        {
            _laser = laser;
            _laserPatternHelper = laserPatternHelper;
            _settings = settings;
            _laserAnimationStatus = laserAnimationStatus;
        }

        public void Project(PatternOptions options)
        {
            LaserColors colors = _laserPatternHelper.GetRandomLaserColors();
            int xCenter = (_settings.maxLeft + _settings.maxRight) / 2;
            
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            AnimationSpeed animationSpeed = options.AnimationSpeed;

            for (int i = 0; i < options.Total; i++)
            {
                if (options.AnimationSpeed == AnimationSpeed.NotSet) animationSpeed = _laserAnimationStatus.AnimationSpeed;

                int left = xCenter - 100;
                int right = xCenter + 100;

                int y = new Random(Guid.NewGuid().GetHashCode()).Next(_settings.minHeight, _settings.maxHeight);

                while (left > _settings.maxLeft || right < _settings.maxRight)
                {
                    if (stopwatch.ElapsedMilliseconds > options.DurationMilliseconds && options.DurationMilliseconds != 0 || _laserAnimationStatus.AnimationCanceled) break;

                    _laser.SendTo(left -= (int)animationSpeed, y);
                    System.Threading.Thread.SpinWait(30000);
                    _laser.On(colors);

                    _laser.SendTo(right += (int)animationSpeed, y);
                    System.Threading.Thread.SpinWait(30000);
                    _laser.Off();
                    System.Threading.Thread.SpinWait(100);
                }
            }

            _laser.Off();
        }
    }
}