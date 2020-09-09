using System;
using System.Collections.Generic;
using System.IO.Ports;
using Newtonsoft.Json;

namespace Models
{
    public class SerialPortModel
    {
        private readonly SerialPort _serialPort;

        public SerialPortModel(LaserSettings settings)
        {
            if (string.IsNullOrEmpty(settings.ComPort)) settings.ComPort = "COM4";
            _serialPort = new SerialPort(settings.ComPort, 128000);
            _serialPort.Open();
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
            if (!_serialPort.IsOpen) _serialPort.Open();
            _serialPort.WriteLine(command);
        }
    }
}