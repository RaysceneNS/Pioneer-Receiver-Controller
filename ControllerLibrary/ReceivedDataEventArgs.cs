using System;

namespace ReceiverController
{
	public class ReceivedDataEventArgs : EventArgs
	{
		private readonly string _data;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReceivedDataEventArgs"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		public ReceivedDataEventArgs(string data)
		{
			this._data = data;
		}

		/// <summary>
		/// Gets the data.
		/// </summary>
		/// <value>The data.</value>
		public string Data
		{
			get { return this._data; }
		}
	}
}