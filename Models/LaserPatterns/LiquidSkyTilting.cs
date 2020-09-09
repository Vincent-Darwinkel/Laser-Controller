using System;
using System.Collections.Generic;
using System.Diagnostics;
using Enums;
using Interfaces;

namespace Models.LaserPatterns
{
    public class LiquidSkyTilting : ILaserPattern
    {
        private readonly Laser _laser;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;
        private readonly LaserAnimationStatus _laserAnimationStatus;

        public LiquidSkyTilting(Laser laser, LaserPatternHelper laserPatternHelper, LaserSettings settings, LaserAnimationStatus laserAnimationStatus)
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
            var xPos = new List<int> { _settings.maxLeft, _settings.maxRight };

            for (double i = 0; i < 6.5 * options.Total; i += (double)animationSpeed / 1400)
            {
                if (options.AnimationSpeed == AnimationSpeed.NotSet) animationSpeed = _laserAnimationStatus.AnimationSpeed;

                for (int line = 0; line < 2; line++)
                {
                    if (stopwatch.ElapsedMilliseconds > options.DurationMilliseconds && options.DurationMilliseconds != 0 || _laserAnimationStatus.AnimationCanceled) break;

                    for (int j = 0; j < 3; j++)
                    {
                        int y = Convert.ToInt32(Math.Sin(i + line) * Math.Abs(2000));

                        _laser.SendTo(xPos[line], y);
                        System.Threading.Thread.SpinWait(15000);
                        _laser.On(colors);
                    }
                }
            }

            _laser.Off();
        }
    }
}