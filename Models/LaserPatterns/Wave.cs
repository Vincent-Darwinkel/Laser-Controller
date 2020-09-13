﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Enums;
using Interfaces;

namespace Models.LaserPatterns
{
    public class Wave : ILaserPattern
    {
        private readonly Laser _laser;
        private readonly LaserPatternHelper _laserPatternHelper;
        private readonly LaserSettings _settings;
        private readonly LaserAnimationStatus _laserAnimationStatus;

        public Wave(Laser laser, LaserPatternHelper laserPatternHelper, LaserSettings settings, LaserAnimationStatus laserAnimationStatus)
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
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var color = _laserPatternHelper.GetRandomLaserColors();

            AnimationSpeed animationSpeed = options.AnimationSpeed;
            double iterations = 0;

            while (stopwatch.ElapsedMilliseconds < options.DurationMilliseconds || iterations < options.Total)
            {
                if (stopwatch.ElapsedMilliseconds > options.DurationMilliseconds && options.DurationMilliseconds != 0 || _laserAnimationStatus.AnimationCanceled) break;
                if (options.AnimationSpeed == AnimationSpeed.NotSet) animationSpeed = _laserAnimationStatus.AnimationSpeed;

                for (int i = _settings.maxLeft; i < _settings.maxRight; i += 15)
                {
                    iterations += 0.025;

                    for (double j = 0; j < (double)animationSpeed; j++)
                        iterations += 0.0005;

                    int y = Convert.ToInt32(Math.Sin(iterations) * Math.Abs(_settings.maxHeight));
                    _laser.SendTo(i, y);

                    if (i == _settings.maxLeft) System.Threading.Thread.SpinWait(22000);
                    _laser.On(color);
                }

                _laser.Off();
            }
        }
    }
}