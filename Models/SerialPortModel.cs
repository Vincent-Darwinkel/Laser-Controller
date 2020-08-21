using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace Models
{
    public class SerialPortModel
    {
        private readonly SerialPort _serialPort = new SerialPort();

        public SerialPortModel(Settings settings)
        {
            if (settings.ComPort == null) return;
            _serialPort.PortName = settings.ComPort;
            _serialPort.BaudRate = 60000000;
        }

        public List<string> GetPortNames()
        {
            return SerialPort.GetPortNames().ToList();
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