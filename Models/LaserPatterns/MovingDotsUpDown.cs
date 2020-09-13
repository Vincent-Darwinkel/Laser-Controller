using System;
using System.Collections.Generic;
using System.Diagnostics;
using Enums;
using Interfaces;

namespace Models.LaserPatterns
{
    public class MovingDotsUpDown : ILaserPattern
    {
        private readonly Laser _laser;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;
        private readonly LaserAnimationStatus _laserAnimationStatus;

        public MovingDotsUpDown(Laser laser, LaserPatternHelper laserPatternHelper, LaserSettings settings, LaserAnimationStatus laserAnimationStatus)
        {
            _laser = laser;
            _laserPatternHelper = laserPatternHelper;
            _settings = settings;
            _laserAnimationStatus = laserAnimationStatus;
        }

        public void Project(PatternOptions options)
        {
            int totalLines = 4;
            if (totalLines < 2) totalLines = 2;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var colors = new List<LaserColors>();

            for (int i = 0; i < totalLines; i++)
                colors.Add(_laserPatternHelper.GetRandomLaserColors());

            AnimationSpeed animationSpeed = options.AnimationSpeed;
            double iterations = 0;

            while (stopwatch.ElapsedMilliseconds < options.DurationMilliseconds || iterations / 6.3 < options.Total)
            {
                iterations += (double) animationSpeed / 700;
                if (stopwatch.ElapsedMilliseconds > options.DurationMilliseconds && options.DurationMilliseconds != 0 || _laserAnimationStatus.AnimationCanceled) break;
                if (options.AnimationSpeed == AnimationSpeed.NotSet) animationSpeed = _laserAnimationStatus.AnimationSpeed;

                for (int line = 0; line < totalLines; line++)
                {
                    int x = Convert.ToInt32(Math.Cos(iterations + line) * Math.Abs(_settings.maxLeft));
                    int y = Convert.ToInt32(Math.Sin(iterations) * Math.Abs(2000));

                    _laser.SendTo(x, y);
                    System.Threading.Thread.SpinWait(40000);

                    _laser.On(colors[line]);

                    System.Threading.Thread.SpinWait(6000);
                    _laser.Off();
                }
            }
        }
    }
}
