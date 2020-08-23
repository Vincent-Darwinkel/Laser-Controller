using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Interfaces;
using Models;
using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Wave;

namespace Logic
{
    public class AudioLogic
    {
        private IWaveIn waveIn;
        private static int fftLength = 8192;
        private SampleAggregator sampleAggregator = new SampleAggregator(fftLength);

        public bool _algorithmEnabled { get; set; }
        private static Timer _timer;
        public List<string> _audioDevices;
        private List<Complex> _avarageValues = new List<Complex>();
        private readonly SerialPortModel _serialPortModel;

        public AudioLogic(SerialPortModel serialPortModel)
        {
            SetTimer();
            sampleAggregator.FftCalculated += FftCalculated;
            sampleAggregator.PerformFFT = true;
            _serialPortModel = serialPortModel;
            waveIn = new WasapiLoopbackCapture();

            waveIn.DataAvailable += OnDataAvailable;

            waveIn.StartRecording();
        }

        void OnDataAvailable(object sender, WaveInEventArgs e)
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

        void FftCalculated(object sender, FftEventArgs e)
        {
            var highestValue = new Complex();
            int index = 0;

            foreach (var complex in e.Result)
            {
                if (highestValue.Y == 0 || complex.Y > highestValue.Y && index < 50 && index > 0) highestValue = complex;
                index++;
            }

            _avarageValues.Add(highestValue);
        }

        public List<string> GetAudioDevices()
        {
            var enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToArray();

            return devices.Select(mmDevice => mmDevice.FriendlyName).ToList();
        }

        private void SetTimer()
        {
            _timer = new Timer(600);
            _timer.Elapsed += TimerTick;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }

        private Complex GetAverageValue()
        {
            float totalY = 0;
            float totalX = 0;

            var avarageComplex = new Complex();

            foreach (var value in _avarageValues)
            {
                totalX += value.X;
                totalY += value.Y;
            }

            avarageComplex.Y = totalY / _avarageValues.Count;
            avarageComplex.X = totalX / _avarageValues.Count;

            _avarageValues.Clear();

            return avarageComplex;
        }


        private void TimerTick(Object source, ElapsedEventArgs e)
        {
            Complex average = GetAverageValue();

            if (average.Y != average.Y || average.X != average.X || average.Y < 0.005)
            {
                _serialPortModel.SendCommand(new SerialCommand().SetAnimationSpeed(AnimationSpeed.Off));
                return;
            }

            var animationSpeed = AnimationSpeed.Slow;
            if (average.Y > 0.014 && average.Y < 0.020) animationSpeed = AnimationSpeed.Medium;
            else if (average.Y > 0.020 && average.Y < 0.026) animationSpeed = AnimationSpeed.Fast;
            else if (average.Y > 0.026) animationSpeed = AnimationSpeed.VeryFast;

            _serialPortModel.SendCommand(new SerialCommand().SetAnimationSpeed(animationSpeed));
        }

        public void StartAudioAlgorithm(string device)
        {

        }
    }
}