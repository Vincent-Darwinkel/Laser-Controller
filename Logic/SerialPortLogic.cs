using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Models;

namespace Logic
{
    public class SerialPortLogic
    {
        private readonly SerialPort _serialPort = new SerialPort();

        public SerialPortLogic(Settings settings)
        {
            _serialPort.PortName = settings.ComPort;
            _serialPort.BaudRate = 800000;
        }

        public List<string> GetPortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public void SendCommand(string command)
        {
            try
            {
                _serialPort.Open();
                _serialPort.WriteLine(command);
            }

            catch (Exception e)
            {
                // catch locked exception
            }

            finally
            {
                _serialPort.Close();
            }
        }
    }
}