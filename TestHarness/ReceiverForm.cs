using System;
using System.Diagnostics;
using System.Windows.Forms;
using ReceiverController;

namespace TestHarness
{
	public partial class ReceiverForm : Form
	{
		private readonly IReceiver _receiver;
		private readonly TextBoxTraceListener _textBoxListener;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReceiverForm"/> class.
		/// </summary>
		public ReceiverForm()
		{
			this.InitializeComponent();

			//listen for trace messages
			this._textBoxListener = new TextBoxTraceListener(this.logText);
			Trace.Listeners.Add(this._textBoxListener);

			//create an instance of the AV receiver controller
			this._receiver = new Pioneer_SC05();
			this.UpdateUI();
		}

		/// <summary>
		/// Closes the stuff.
		/// </summary>
		private void CloseStuff()
		{
			Trace.WriteLine("Shutting down app");
		    _receiver?.Disconnect();
		}

		/// <summary>
		/// Handles the Click event of the connectButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void ConnectButton_Click(object sender, EventArgs e)
		{
			if (this._receiver == null)
				return;

			try
			{
				if (this._receiver.Connect(this.connectionPort.Text))
				{
					this.UpdateUI();
				}
				else
				{
					MessageBox.Show("Failed to connect, please check your settings");
				}
			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.ToString());
			}

			this._receiver.StatusChanged += Receiver_StatusChanged;
		}

		void Receiver_StatusChanged(object sender, EventArgs e)
		{
			ShowStatus();
		}

		/// <summary>
		/// Handles the Click event of the disconnectButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void DisconnectButton_Click(object sender, EventArgs e)
		{
		    _receiver?.Disconnect();
		    this.UpdateUI();
		}

		/// <summary>
		/// Handles the FormClosing event of the TestApp control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
		private void TestApp_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.CloseStuff();
		}

		private void UpdateUI()
		{
			try
			{
				inputSelector.Items.Clear();
				inputSelector.Items.AddRange(this._receiver.GetInputModes());

				listeningMode.Items.Clear();
				listeningMode.Items.AddRange(this._receiver.GetListeningModes());


				if (this._receiver != null && this._receiver.IsConnected)
				{
					ShowStatus();
				}

			}
			catch (Exception e)
			{

				MessageBox.Show(e.ToString());
			}
		}

		private void ShowStatus()
		{
			var sc = (Pioneer_SC05)this._receiver;
			string status = "Power : " + sc.MasterPowerState + Environment.NewLine;
			status += "Mute : " + sc.Mute + Environment.NewLine;
			status += "Volume : " + sc.Volume + Environment.NewLine;
			status += "Input :" + sc.InputMode + Environment.NewLine;
			status += "Treble :" + sc.Treble + Environment.NewLine;
			status += "Bass :" + sc.Bass + Environment.NewLine;
			status += "Tone :" + sc.ToneStatus + Environment.NewLine;

			this.textBoxStatus.Text = status;
		}
        
		private void ButtonPowerOn_Click(object sender, EventArgs e)
		{
			this._receiver.PowerOn();
		}

		private void ButtonPowerOff_Click(object sender, EventArgs e)
		{
			this._receiver.PowerOff();
		}

		private void ButtonStatusRequest_Click(object sender, EventArgs e)
		{
			foreach (string statusMode in this._receiver.GetStatusModes())
			{
				this._receiver.StatusRequest(statusMode);
			}
		}

		private void ButtonChangeInput_Click(object sender, EventArgs e)
		{
			this._receiver.RequestInputMode(this.inputSelector.SelectedText);
		}
        
		private void ButtonChangeMode_Click(object sender, EventArgs e)
		{
			this._receiver.RequestListeningMode(this.listeningMode.SelectedText);
		}
	}
}