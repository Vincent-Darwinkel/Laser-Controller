using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Timers;
using Newtonsoft.Json;

namespace Models
{
    public class SerialPortModel
    {
        private readonly SerialPort _serialPort;
        private Timer _timer;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private const int _serialPortAutoCloseTime = 2500;

        public SerialPortModel(LaserSettings settings)
        {
            if (string.IsNullOrEmpty(settings.ComPort)) settings.ComPort = "COM4";
            _serialPort = new SerialPort(settings.ComPort, 128000);
            _serialPort.Open();
            SetTimer();
            _stopwatch.Start();
        }

        private void SetTimer()
        {
            _timer = new Timer(5000);
            _timer.Elapsed += TimerTick;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }

        private void TimerTick(object source, ElapsedEventArgs e)
        {
            if (_stopwatch.ElapsedMilliseconds < _serialPortAutoCloseTime) return;
            
            _stopwatch.Restart();

            if (!_serialPort.IsOpen) return;
            _serialPort.Close();
        }

        public IEnumerable<string> GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        public T SendReadAndConvert<T>(string command)
        {
            try
            {
                string json = "";

                // sometimes the laser doesn't respond fast enough so we try it more times
                int tries = 0;
                while (string.IsNullOrEmpty(json) && tries < 1000)
                {
                    _serialPort.WriteLine(command);
                    json = _serialPort.ReadExisting();
                    tries++;
                }

                return json.Contains("error") ? default : JsonConvert.DeserializeObject<T>(json);
            }

            catch (Exception e)
            {
                // catch locked exception
            }

            return default;
        }

        public void SendCommand(string command)
        {
            if (!_serialPort.IsOpen)
                _serialPort.Open();

            _stopwatch.Restart();
            _serialPort.WriteLine(command);
        }
    }
}