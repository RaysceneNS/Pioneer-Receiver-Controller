using System;

namespace ReceiverController
{
	public class ReceivedDataEventArgs : EventArgs
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ReceivedDataEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public ReceivedDataEventArgs(string data)
		{
			this.Data = data;
		}

        public string Data { get; }
    }
}