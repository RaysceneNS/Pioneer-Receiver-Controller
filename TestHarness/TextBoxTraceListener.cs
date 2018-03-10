using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TestHarness
{
	public class TextBoxTraceListener : TraceListener
	{
		private readonly TextBox _target;
		private readonly StringSendDelegate _invokeWrite;

		public TextBoxTraceListener(TextBox target)
		{
			_target = target;
			_invokeWrite = SendString;
		}

		public override void Write(string message)
		{
			_target.Invoke(_invokeWrite, message);
		}

		public override void WriteLine(string message)
		{
			_target.Invoke(_invokeWrite, message + Environment.NewLine);
		}

		private delegate void StringSendDelegate(string message);
		private void SendString(string message)
		{
			// No need to lock text box as this function will only 
			// ever be executed from the UI thread
			_target.AppendText( message);
		}
	}
}