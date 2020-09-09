using System;
using System.Collections.Generic;
using System.Diagnostics;
using Enums;
using Interfaces;
namespace Models.LaserPatterns
{
    public class MovingDots : ILaserPattern
    {
        private readonly Laser _laser;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;
        private readonly LaserAnimationStatus _laserAnimationStatus;

        public MovingDots(Laser laser, LaserPatternHelper laserPatternHelper, LaserSettings settings, LaserAnimationStatus laserAnimationStatus)
        {
            Process myProcess = Process.GetCurrentProcess();
            myProcess.PriorityClass = ProcessPriorityClass.High;

            _laser = laser;
            _laserPatternHelper = laserPatternHelper;
            _settings = settings;
            _laserAnimationStatus = laserAnimationStatus;
        }

        public void Project(PatternOptions options)
        {
            int totalLines = 4;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var colors = new List<LaserColors>();

            for (int i = 0; i < totalLines; i++)
                colors.Add(_laserPatternHelper.GetRandomLaserColors());

            AnimationSpeed animationSpeed = options.AnimationSpeed;

            for (double i = 0; i < 5 * options.Total; i += (double)animationSpeed / 800)
            {
                if (stopwatch.ElapsedMilliseconds > options.DurationMilliseconds && options.DurationMilliseconds != 0 || _laserAnimationStatus.AnimationCanceled) break;
                if (options.AnimationSpeed == AnimationSpeed.NotSet) animationSpeed = _laserAnimationStatus.AnimationSpeed;
                for (int line = 0; line < totalLines; line++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        int x = Convert.ToInt32(Math.Cos(i + line) * Math.Abs(_settings.maxLeft));
                        _laser.SendTo(x, _settings.maxHeight);
                        System.Threading.Thread.SpinWait(10000);

                        _laser.On(colors[line]);
                    }

                    _laser.Off();
                    System.Threading.Thread.SpinWait(10000);
                }
            }
        }
    }
}
