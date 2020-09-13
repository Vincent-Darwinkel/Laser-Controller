using System;
using System.Collections.Generic;
using System.Diagnostics;
using Enums;
using Interfaces;

namespace Models.LaserPatterns
{
    public class LiquidSkyRandomColors : ILaserPattern
    {
        private readonly Laser _laser;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;
        private readonly LaserAnimationStatus _laserAnimationStatus;

        public LiquidSkyRandomColors(Laser laser, LaserPatternHelper laserPatternHelper, LaserSettings settings, LaserAnimationStatus laserAnimationStatus)
        {
            _laser = laser;
            _laserPatternHelper = laserPatternHelper;
            _settings = settings;
            _laserAnimationStatus = laserAnimationStatus;
        }

        private List<LaserColors> GetRandomColors()
        {
            return new List<LaserColors>
            {
               _laserPatternHelper.GetRandomLaserColors(),
               _laserPatternHelper.GetRandomLaserColors(),
               _laserPatternHelper.GetRandomLaserColors(),
               _laserPatternHelper.GetRandomLaserColors(),
               _laserPatternHelper.GetRandomLaserColors(),
               _laserPatternHelper.GetRandomLaserColors(),
               _laserPatternHelper.GetRandomLaserColors(),
            };
        }

        public void Project(PatternOptions options)
        {
            int xCenter = (_settings.maxLeft + _settings.maxRight) / 2;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            AnimationSpeed animationSpeed = options.AnimationSpeed;
            int iterations = 0;

            List<LaserColors> colors = GetRandomColors();
            int totalLines = colors.Count;
            int lineDistance = (Math.Abs(_settings.maxLeft) + Math.Abs(_settings.maxRight)) / totalLines;

            int previousColorChange = 0;
            int y = _settings.maxHeight;

            while (stopwatch.ElapsedMilliseconds < options.DurationMilliseconds || iterations < options.Total)
            {
                iterations++;

                if (y > _settings.minHeight) y -= (int) animationSpeed / 5 + 2;
                else y = _settings.maxHeight;

                if (options.AnimationSpeed == AnimationSpeed.NotSet) animationSpeed = _laserAnimationStatus.AnimationSpeed;

                if (iterations - previousColorChange > 600 / (int) animationSpeed)
                {
                    previousColorChange = iterations;
                    colors = GetRandomColors();
                }

                for (int i = 0; i < totalLines; i++)
                {
                    _laser.SendTo(_settings.maxLeft + lineDistance * i, y);
                    System.Threading.Thread.SpinWait(16000);

                    _laser.On(colors[i]);
                }

                _laser.Off();
            }

            _laser.Off();
        }
    }
}
