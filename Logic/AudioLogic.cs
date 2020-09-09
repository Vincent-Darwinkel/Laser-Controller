using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Enums;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using NAudio.Dsp;
using NAudio.Wave;

namespace Logic
{
    public class AudioLogic
    {
        private IWaveIn waveIn;
        private static int fftLength = 8192;
        private readonly SampleAggregator sampleAggregator = new SampleAggregator(fftLength);

        private static Timer _timer;
        private readonly List<Complex> _averageValues = new List<Complex>();

        private readonly IServiceProvider _serviceProvider;
        private readonly LaserAnimationStatus _laserAnimationStatus;

        private readonly List<ILaserPattern> _patterns = new List<ILaserPattern>();
        private List<ILaserPattern> _previousExecutedPatterns = new List<ILaserPattern>();

        private readonly List<AnimationSpeed> _previousAnimationSpeeds = new List<AnimationSpeed>();
        private AnimationSpeed _averageAnimationSpeed;

        private int _totalTimesOff;
        private Task _animationTask;

        private double _audioCalibrationValue = -200;
        private bool _audioCalibrated;

        public AudioLogic(IServiceProvider serviceProvider, LaserAnimationStatus laserAnimationStatus)
        {
            _serviceProvider = serviceProvider;
            _laserAnimationStatus = laserAnimationStatus;

            SetTimer();
            sampleAggregator.FftCalculated += FftCalculated;
            sampleAggregator.PerformFFT = true;
            waveIn = new WasapiLoopbackCapture();
            waveIn.DataAvailable += OnDataAvailable;

            try
            {
                var patterns = AppDomain.CurrentDomain.GetAssemblies().SelectMany(e => e.GetTypes())
                    .Where(x => typeof(ILaserPattern).IsAssignableFrom(x) && !x.IsInterface);

                foreach (var pattern in patterns)
                    _patterns.Add((ILaserPattern)ActivatorUtilities.CreateInstance(_serviceProvider, pattern));
            }

            catch (Exception e) { /* catch windows forms not found exception */ }
        }

        private void CalibrateAudioVolume()
        {
            var complex = new Complex();
            while (complex.Y == 0 || complex.Y != complex.Y)
            {
                complex = GetAverageBassVolume();
                Console.Beep(125, 1000);
            }

            Console.WriteLine(GetAverageBassVolume().Y + " Calibrated");
            _audioCalibrated = true;
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesRecorded = e.BytesRecorded;
            int bufferIncrement = waveIn.WaveFormat.BlockAlign;

            for (int index = 0; index < bytesRecorded; index += bufferIncrement)
            {
                float sample32 = BitConverter.ToSingle(buffer, index);
                sampleAggregator.Add(sample32);
            }
        }

        private void FftCalculated(object sender, FftEventArgs e)
        {
            var highestValue = new Complex();
            int index = 0;

            foreach (var complex in e.Result)
            {
                if (highestValue.Y == 0 || complex.Y > highestValue.Y && index < 25 && index > 5) highestValue = complex; // get tones between 25 / 145 hz
                index++;
            }

            _averageValues.Add(highestValue);
        }
        
        private void SetTimer()
        {
            _timer = new Timer(400);
            _timer.Elapsed += TimerTick;
            _timer.AutoReset = true;
        }

        private Complex GetAverageBassVolume()
        {
            float totalY = 0;
            float totalX = 0;

            var averageComplex = new Complex();

            foreach (var value in _averageValues)
            {
                totalX += value.X;
                totalY += value.Y;
            }

            averageComplex.Y = totalY / _averageValues.Count;
            averageComplex.X = totalX / _averageValues.Count;

            _averageValues.Clear();

            return averageComplex;
        }

        private AnimationSpeed GetAnimationSpeedByFftData(Complex average)
        {
            if (average.Y != average.Y || average.X != average.X)
                return AnimationSpeed.Off; // check if value is a number if not no music is played

            if (average.Y > 0.010 && average.Y < 0.014) return AnimationSpeed.Slow;
            if (average.Y > 0.014 && average.Y < 0.018) return AnimationSpeed.Medium;
            if (average.Y > 0.018 && average.Y < 0.022) return AnimationSpeed.Fast;
            return average.Y > 0.026 ? AnimationSpeed.VeryFast : AnimationSpeed.Off;
        }

        private bool MusicIsPlaying()
        {
            if (_averageAnimationSpeed != AnimationSpeed.Off)
            {
                _totalTimesOff = 0;
                return true;
            }

            _totalTimesOff++;

            if (_totalTimesOff <= 5) return false;
            _laserAnimationStatus.AnimationCanceled = true;
            _laserAnimationStatus.AnimationSpeed = AnimationSpeed.Off;
            _totalTimesOff = 0;

            return false;
        }

        private void TimerTick(object source, ElapsedEventArgs e)
        {
            Complex averageBassVolume = GetAverageBassVolume();
            _averageAnimationSpeed = GetAverageAnimationSpeed();
            AnimationSpeed currentSpeed = GetAnimationSpeedByFftData(averageBassVolume);

            if (!_audioCalibrated)
            {
                new Task(() => CalibrateAudioVolume()).Start();
                return;
            }

            _previousAnimationSpeeds.Add(currentSpeed);
            if (_previousAnimationSpeeds.Count > 8) _previousAnimationSpeeds.RemoveRange(0, _previousAnimationSpeeds.Count - 3);

            Console.WriteLine(averageBassVolume.Y);
            Console.WriteLine(currentSpeed);

            if (!MusicIsPlaying()) return;
            if (_averageAnimationSpeed != AnimationSpeed.Off) _laserAnimationStatus.AnimationSpeed = _averageAnimationSpeed;

            PlayAnimation(GetRandomPattern(), new PatternOptions
            {
                AnimationSpeed = AnimationSpeed.NotSet,
                DurationMilliseconds = 0,
                Total = new Random(Guid.NewGuid().GetHashCode()).Next(2, 5)
            });
        }

        private ILaserPattern GetRandomPattern()
        {
            if (_previousExecutedPatterns.Count > 2) _previousExecutedPatterns.Clear();

            List<ILaserPattern> patterns = _patterns.Except(_previousExecutedPatterns).ToList();
            return patterns[new Random(Guid.NewGuid().GetHashCode()).Next(0, patterns.Count)];
        }

        private bool AnimationCompleted()
        {
            return _animationTask == null || _animationTask.Status == TaskStatus.Canceled ||
                   _animationTask.Status == TaskStatus.Created || _animationTask.Status == TaskStatus.RanToCompletion;
        }

        private AnimationSpeed GetAverageAnimationSpeed()
        {
            if (_previousAnimationSpeeds.Count == 0) return AnimationSpeed.Off;

            return _previousAnimationSpeeds.GroupBy(i => i).OrderByDescending(grp => grp.Count())
                .Select(grp => grp.Key).First();
        }

        private void PlayAnimation(ILaserPattern pattern, PatternOptions options)
        {
            if (!AnimationCompleted() || options.AnimationSpeed.ToString() == AnimationSpeed.Off.ToString()) return;
            _laserAnimationStatus.AnimationCanceled = false;

            _animationTask = new Task(() => pattern.Project(options), TaskCreationOptions.LongRunning);
            _animationTask.Start();
            _previousExecutedPatterns.Add(pattern);
        }

        public void StartAudioAlgorithm()
        {
            waveIn.StartRecording();
            _timer.Enabled = true;
            _timer.Start();
        }

        public void StopAudioAlgorithm()
        {
            waveIn.StopRecording();
            _timer.Enabled = false;
            _timer.Stop();
        }
    }
}