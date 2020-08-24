using System;
using System.Collections.Generic;
using System.IO.Ports;
using Newtonsoft.Json;

namespace Models
{
    public class SerialPortModel
    {
        private readonly SerialPort _serialPort = new SerialPort();

        public SerialPortModel(LaserSettings settings)
        {
            if (string.IsNullOrEmpty(settings.ComPort)) settings.ComPort = "COM4";
            _serialPort.PortName = settings.ComPort;
            _serialPort.BaudRate = 600000;
        }

        public IEnumerable<string> GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        public T SendReadAndConvert<T>(string command)
        {
            try
            {
                if (_serialPort.IsOpen) return default;
                _serialPort.Open();

                string json = "";

                // sometimes the laser doesn't respond fast enough so we try it more times
                int tries = 0;
                while (string.IsNullOrEmpty(json) && tries < 10)
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

            finally
            {
                try
                {
                    _serialPort.Close();
                }
                catch (Exception e)
                {

                }
            }

            return default;
        }

        public void SendCommand(string command)
        {
            try
            {
                if (_serialPort.IsOpen) return;

                _serialPort.Open();
                _serialPort.WriteLine(command);
            }

            catch (Exception e)
            {
                // catch locked exception
            }

            finally
            {
                try
                {
                    _serialPort.Close();
                }
                catch (Exception e)
                {
                    
                }
            }
        }
    }
}