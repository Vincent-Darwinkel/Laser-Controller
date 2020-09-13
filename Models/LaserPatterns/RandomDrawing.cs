using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Enums;
using Interfaces;

namespace Models.LaserPatterns
{
    public class RandomDrawing : ILaserPattern
    {
        private readonly Laser _laser;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;
        private readonly LaserAnimationStatus _laserAnimationStatus;

        public RandomDrawing(Laser laser, LaserPatternHelper laserPatternHelper, LaserSettings settings, LaserAnimationStatus laserAnimationStatus)
        {
            _laser = laser;
            _laserPatternHelper = laserPatternHelper;
            _settings = settings;
            _laserAnimationStatus = laserAnimationStatus;
        }

        public List<LaserPositionAndColors> GetPredifinedLaserPositionAndColors()
        {
            return new List<LaserPositionAndColors>
            {
                new LaserPositionAndColors
                {
                    X = _settings.maxLeft, Y = (_settings.minHeight + _settings.maxHeight) / 2,
                    LaserColors = _laserPatternHelper.GetRandomLaserColors()
                },

                new LaserPositionAndColors
                {
                    X = _settings.maxLeft + 20, Y = (_settings.minHeight + _settings.maxHeight) / 2,
                    LaserColors = _laserPatternHelper.GetRandomLaserColors()
                }
            };
        }

        public void Project(PatternOptions options)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            AnimationSpeed animationSpeed = options.AnimationSpeed;
            int iterations = 0;

            var random = new Random(Guid.NewGuid().GetHashCode());
            List<LaserPositionAndColors> laserPosAndColors = GetPredifinedLaserPositionAndColors();

            int previousColorChange = 0;

            bool xToLeft = false;
            bool yUp = false;

            while (stopwatch.ElapsedMilliseconds < options.DurationMilliseconds || iterations < options.Total * 100)
            {
                iterations++;

                LaserPositionAndColors lastLaserPositionAndColors = laserPosAndColors.Last();
                int x = lastLaserPositionAndColors.X;
                int y = lastLaserPositionAndColors.Y;
                
                if (xToLeft) x -= random.Next(-10 / (int) animationSpeed, (int)animationSpeed * 3 + 15);
                else x += random.Next(-10 / (int)animationSpeed, (int)animationSpeed * 3 + 15);

                if (yUp) y += random.Next(-10 / (int)animationSpeed, (int)animationSpeed * 3 + 15);
                else y -= random.Next(-10 / (int)animationSpeed, (int)animationSpeed * 3 + 15);

                if (x >= _settings.maxRight && !xToLeft) xToLeft = true;
                else if (x <= _settings.maxLeft && xToLeft) xToLeft = false;

                if (y >= _settings.maxHeight && yUp) yUp = false;
                else if (y <= _settings.minHeight && !yUp) yUp = true;

                var color = lastLaserPositionAndColors.LaserColors;
                if (iterations - previousColorChange > 25)
                {
                    previousColorChange = iterations;
                    color = _laserPatternHelper.GetRandomLaserColors();
                }

                laserPosAndColors.Add(new LaserPositionAndColors
                {
                    LaserColors = color,
                    Y = y,
                    X = x
                });

                foreach (var laserposAndColor in laserPosAndColors)
                {
                    _laser.SendTo(laserposAndColor.X, laserposAndColor.Y);
                    _laser.On(laserposAndColor.LaserColors);
                }

                if (laserPosAndColors.Count > 100 - (int) animationSpeed)
                    laserPosAndColors.RemoveRange(0, 1);
            }

            _laser.Off();
        }
    }
}