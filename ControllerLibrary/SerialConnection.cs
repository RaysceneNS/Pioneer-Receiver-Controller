using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace ReceiverController
{
    public class SerialConnection
    {
        public event EventHandler<ReceivedDataEventArgs> ReceivedData;
        private SerialPort _serialPort;
        private Thread _rcvThread;
        private bool _closeRequested;

        /// <summary>
        /// Connects the specified port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public bool Connect(string port)
        {
            if (this._serialPort != null)
            {
                this._serialPort.Close();
                this._serialPort = null;
            }

            this._serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One)
            {
                WriteTimeout = 500,
                ReadTimeout = 500
            };
            try
            {
                this._serialPort.Open();
            }
            catch (IOException)
            {
                return false;
            }

            //start the listener thread
            this._rcvThread = new Thread(ReceiveDataThread);
            this._rcvThread.Start();

            return true;
        }

        /// <summary>
        /// Poll on the serial port 
        /// </summary>
        private void ReceiveDataThread()
        {
            while (!_closeRequested)
            {
                try
                {
                    var line = this._serialPort.ReadLine();
                    this.ReceivedData?.Invoke(this, new ReceivedDataEventArgs(line));
                }
                catch (TimeoutException)
                {
                    //the read timeout has elapsed, cycle
                }
                catch (IOException)
                {
                }
            }
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            this._closeRequested = true;

            if (this.IsConnected)
            {
                this._serialPort.Close();
                this._serialPort = null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected
        {
            get
            {
                return this._serialPort != null && this._serialPort.IsOpen;
            }
        }

        /// <summary>
        /// Writes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Write(string command)
        {
            if (this.IsConnected)
                this._serialPort.Write(command);
        }
    }
}