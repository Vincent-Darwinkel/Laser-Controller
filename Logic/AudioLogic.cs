using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NAudio.CoreAudioApi;

namespace Logic
{
    public class AudioLogic
    {
        public bool _algorithmCanceled { get; set; }
        private readonly IServiceProvider _serviceProvider;
        private readonly List<ILaserPattern> _patternCollection;
        private List<ILaserPattern> _executedPatternsHistory;

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

        private double GetCalibrationValue(MMDevice device)
        {
            var stopWatch = new Stopwatch();

            Console.Beep(100, 50);
            stopWatch.Start();

            double testToneVolume = 0;

            while (stopWatch.ElapsedMilliseconds < 40)
                testToneVolume = Math.Round(device.AudioMeterInformation.MasterPeakValue * 2000);
            
            return testToneVolume;
        }

        private AnimationSpeed GetSpeedByPeaks(int peaks)
        {
            if (peaks >= 10 && peaks < 15)
                return AnimationSpeed.Medium;

            if (peaks >= 15 && peaks < 20)
                return AnimationSpeed.Fast;

            return peaks >= 20 ? AnimationSpeed.VeryFast : AnimationSpeed.Slow;
        }

        public void StartAudioAlgorithm(string deviceName)
        {
            _algorithmCanceled = false;
            MMDevice device = GetDeviceByName(deviceName);
            double maxVolume = GetCalibrationValue(device);

            var volumeCheckStopwatch = new Stopwatch();
            var animationTimeStopwatch = new Stopwatch();

            AnimationSpeed animationSpeed = AnimationSpeed.Slow;
            int peaksPer50MiliSeconds = 0;

            double lastVolume = 0;

            volumeCheckStopwatch.Start();

            while (!_algorithmCanceled)
            {
                double volume = Math.Round(device.AudioMeterInformation.MasterPeakValue * 2000);

                if (volume > maxVolume)
                {
                    double difference = volume / maxVolume;
                    double volumeCalibrationValue = difference * volume;
                    volume -= volumeCalibrationValue;
                }

                if (volume > lastVolume)
                    peaksPer50MiliSeconds++;

                if (volumeCheckStopwatch.ElapsedMilliseconds >= 50)
                {
                    peaksPer50MiliSeconds = 0;
                    animationSpeed = GetSpeedByPeaks(peaksPer50MiliSeconds);

                    volumeCheckStopwatch.Stop();
                    volumeCheckStopwatch.Reset();
                }

                animationTimeStopwatch.Start();

                while (animationTimeStopwatch.ElapsedMilliseconds < (int)animationSpeed)
                    DrawPattern(animationSpeed);

                animationTimeStopwatch.Stop();
                animationTimeStopwatch.Reset();

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

            var patternsThatCanBeExecuted = patternsThatMatchSpeed.Except(_executedPatternsHistory).ToList();
            int randomIndex = new Random(DateTime.Now.Millisecond).Next(0, patternsThatCanBeExecuted.Count());

            if (_executedPatternsHistory.Count >= 3) _executedPatternsHistory.Clear();

            return patternsThatCanBeExecuted[randomIndex];
        }

        private void DrawPattern(AnimationSpeed animationSpeed)
        {
            ILaserPattern pattern = SelectRandomPattern(animationSpeed);
            pattern.Project(animationSpeed);
        }
    }
}