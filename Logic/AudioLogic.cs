using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NAudio.CoreAudioApi;

namespace Logic
{
    public class AudioLogic
    {
        public bool _algorithmCanceled { get; set; }
        private readonly IServiceProvider _serviceProvider;
        private List<ILaserPattern> _patternCollection = new List<ILaserPattern>();
        private List<ILaserPattern> _executedPatternsHistory = new List<ILaserPattern>();
        private List<AnimationSpeed> _animationSpeedHistory = new List<AnimationSpeed>();
        
        public AudioLogic(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var patterns = AppDomain.CurrentDomain.GetAssemblies().SelectMany(e => e.GetTypes())
                .Where(x => typeof(ILaserPattern).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var pattern in patterns) 
                _patternCollection.Add((ILaserPattern)ActivatorUtilities.CreateInstance(_serviceProvider, pattern));
        }

        public List<string> GetAudioDevices()
        {
            var enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToArray();

            return devices.Select(mmDevice => mmDevice.FriendlyName).ToList();
        }

        private MMDevice GetDeviceByName(string name)
        {
            var enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();

            return devices.Find(dev => dev.FriendlyName == name);
        }

        private AnimationSpeed GetAverageAnimationSpeed()
        {
            if (_animationSpeedHistory.Count <= 0) return AnimationSpeed.Slow;

            AnimationSpeed averageAnimationSpeed = _animationSpeedHistory.GroupBy(i => i).OrderByDescending(grp => grp.Count())
                .Select(grp => grp.Key).First();

            Console.WriteLine("AverageSpeed: " + averageAnimationSpeed);
            Console.WriteLine("Total: " + _animationSpeedHistory.Count);
            if (_animationSpeedHistory.Count >= 10) _animationSpeedHistory.Clear();
            return averageAnimationSpeed;
        }

        private AnimationSpeed GetSpeedByPeaksAndVolume(int peaks, double volume)
        {
            AnimationSpeed avarageAnimationSpeed = GetAverageAnimationSpeed();

            if (avarageAnimationSpeed == AnimationSpeed.Slow) volume -= 25;
            else if (avarageAnimationSpeed == AnimationSpeed.Medium) volume -= 50;
            else if (avarageAnimationSpeed == AnimationSpeed.Fast) volume -= 200;
            else if (avarageAnimationSpeed == AnimationSpeed.VeryFast) volume -= 300;

            if (peaks > 0 && peaks < 3 && volume >= 300)
                return AnimationSpeed.Medium;

            if (peaks >= 3 && peaks < 5 && volume >= 600)
                return AnimationSpeed.Fast;

            return peaks >= 5 && volume >= 850 ? AnimationSpeed.VeryFast : AnimationSpeed.Slow;
        }

        public void StartAudioAlgorithm(string deviceName)
        {
            _algorithmCanceled = false;
            MMDevice device = GetDeviceByName(deviceName);

            var volumeCheckStopwatch = new Stopwatch();
            var animationTimeStopwatch = new Stopwatch();

            AnimationSpeed animationSpeed = AnimationSpeed.Slow;
            int peaks = 0;

            double lastVolume = 0;
            long taskDuration = 0;
            volumeCheckStopwatch.Start();

            while (!_algorithmCanceled)
            {
                double volume = Math.Round(device.AudioMeterInformation.MasterPeakValue * 2000);

                if (volume > lastVolume)
                    peaks++;
                
                if (volumeCheckStopwatch.ElapsedMilliseconds >= 350)
                {
                    animationSpeed = GetSpeedByPeaksAndVolume(peaks, volume);
                    peaks = 0;

                    _animationSpeedHistory.Add(animationSpeed);

                    Console.WriteLine(animationSpeed);

                    volumeCheckStopwatch.Reset();
                    volumeCheckStopwatch.Start();
                }

                if (animationTimeStopwatch.ElapsedMilliseconds >= taskDuration && volume > 200)
                {
                    animationTimeStopwatch.Stop();
                    animationTimeStopwatch.Reset();

                    animationTimeStopwatch.Start();
                    taskDuration = (int)animationSpeed;

                    Task.Run(() =>
                    {
                        while (animationTimeStopwatch.ElapsedMilliseconds < (int)animationSpeed)
                            DrawPattern(animationSpeed);
                    });
                }
                
                lastVolume = volume;
            }
        }

        /// <summary>
        /// Selects a pattern based on a random value and will only select a pattern if it was not executed three patterns ago
        /// </summary>
        /// <param name="animationSpeed"></param>
        /// <returns></returns>
        private ILaserPattern SelectRandomPattern(AnimationSpeed animationSpeed)
        {
            var patternsThatMatchSpeed = _patternCollection.FindAll(pattern => pattern.GetAnimationSpeed() == animationSpeed);
            if (_executedPatternsHistory.Count >= 6) _executedPatternsHistory.RemoveRange(0, 3);

            var patternsThatCanBeExecuted = patternsThatMatchSpeed.Except(_executedPatternsHistory).ToList();
            int randomIndex = new Random(DateTime.Now.Millisecond).Next(0, patternsThatCanBeExecuted.Count);
            
            if (patternsThatCanBeExecuted.Count != 0) return patternsThatCanBeExecuted[randomIndex];
            return _patternCollection[0];
        }

        private void DrawPattern(AnimationSpeed animationSpeed)
        {
            ILaserPattern pattern = SelectRandomPattern(animationSpeed);
            pattern.Project(animationSpeed);
        }
    }
}