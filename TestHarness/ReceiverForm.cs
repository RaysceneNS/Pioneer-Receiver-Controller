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
			this._receiver = new PioneerSC05();
			this.UpdateUi();
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
					this.UpdateUi();
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
		    this.UpdateUi();
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

		private void UpdateUi()
		{
			try
			{
				inputSelector.Items.Clear();
			    inputSelector.Items.AddRange(Enum.GetNames(typeof(InputMode)));

				listeningMode.Items.Clear();
				listeningMode.Items.AddRange(Enum.GetNames(typeof(ListeningMode)));


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
			var sc = (PioneerSC05)this._receiver;
			var status = "Power : " + sc.MasterPowerState + Environment.NewLine;
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
			foreach (var statusMode in Enum.GetValues(typeof(StatusRequest)))
			{
				this._receiver.SendStatusRequest((StatusRequest)statusMode);
			}
		}

		private void ButtonChangeInput_Click(object sender, EventArgs e)
		{
		    if (this.inputSelector.SelectedItem == null)
		        return;
		    var mode = (InputMode) Enum.Parse(typeof(InputMode), this.inputSelector.SelectedItem.ToString());
            this._receiver.RequestInputMode(mode);
		}

	    private void ButtonChangeMode_Click(object sender, EventArgs e)
	    {
	        if (this.listeningMode.SelectedItem == null)
	            return;
	        var mode = (ListeningMode) Enum.Parse(typeof(ListeningMode), this.listeningMode.SelectedItem.ToString());
	        this._receiver.RequestListeningMode(mode);
	    }
	}
}