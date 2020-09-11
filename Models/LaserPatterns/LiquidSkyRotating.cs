using System;
using System.Diagnostics;
using Enums;
using Interfaces;

namespace Models.LaserPatterns
{
    public class LiquidSkyRotating : ILaserPattern
    {
        private readonly Laser _laser;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;
        private readonly LaserAnimationStatus _laserAnimationStatus;

        public LiquidSkyRotating(Laser laser, LaserPatternHelper laserPatternHelper, LaserSettings settings, LaserAnimationStatus laserAnimationStatus)
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
            double iterations = 0;

            while (stopwatch.ElapsedMilliseconds < options.DurationMilliseconds || iterations / 6.3 < options.Total)
            {
                iterations += (double) animationSpeed / 800;
                if (options.AnimationSpeed == AnimationSpeed.NotSet) animationSpeed = _laserAnimationStatus.AnimationSpeed;

                for (int line = 0; line < 2; line++)
                {
                    if (stopwatch.ElapsedMilliseconds > options.DurationMilliseconds && options.DurationMilliseconds != 0 || _laserAnimationStatus.AnimationCanceled) break;

                    for (int j = 0; j < 3; j++)
                    {
                        int x = Convert.ToInt32(Math.Cos(iterations + line * 3) * Math.Abs(_settings.maxLeft));
                        int y = Convert.ToInt32(Math.Sin(iterations + line * 3) * Math.Abs(2000));

                        _laser.SendTo(x, y);
                        System.Threading.Thread.SpinWait(20000);
                        _laser.On(colors);
                    }

                    System.Threading.Thread.SpinWait(2000);
                    _laser.Off();
                }
            }

            _laser.Off();
        }
    }
}