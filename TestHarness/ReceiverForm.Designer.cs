using System.ComponentModel;
using System.Windows.Forms;

namespace TestHarness
{
	partial class ReceiverForm
	{
		private IContainer components;
		private Button connectButton;
		private TextBox connectionPort;
		private Button disconnectButton;
		private GroupBox groupBox2;
		private GroupBox groupBox7;
		private ComboBox inputSelector;
		private GroupBox inputSelectorBox;
		private Label label6;
		private ComboBox listeningMode;
		private GroupBox listeningModeBox;
    

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

	
		private void InitializeComponent()
		{
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.logText = new System.Windows.Forms.TextBox();
            this.inputSelectorBox = new System.Windows.Forms.GroupBox();
            this.buttonChangeInput = new System.Windows.Forms.Button();
            this.inputSelector = new System.Windows.Forms.ComboBox();
            this.listeningModeBox = new System.Windows.Forms.GroupBox();
            this.buttonChangeMode = new System.Windows.Forms.Button();
            this.listeningMode = new System.Windows.Forms.ComboBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.connectionPort = new System.Windows.Forms.TextBox();
            this.buttonPowerOn = new System.Windows.Forms.Button();
            this.buttonPowerOff = new System.Windows.Forms.Button();
            this.buttonStatusRequest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.inputSelectorBox.SuspendLayout();
            this.listeningModeBox.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.logText);
            this.groupBox2.Location = new System.Drawing.Point(6, 261);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(754, 133);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log output";
            // 
            // logText
            // 
            this.logText.Location = new System.Drawing.Point(6, 19);
            this.logText.Multiline = true;
            this.logText.Name = "logText";
            this.logText.Size = new System.Drawing.Size(742, 102);
            this.logText.TabIndex = 0;
            // 
            // inputSelectorBox
            // 
            this.inputSelectorBox.Controls.Add(this.buttonChangeInput);
            this.inputSelectorBox.Controls.Add(this.inputSelector);
            this.inputSelectorBox.Location = new System.Drawing.Point(160, 27);
            this.inputSelectorBox.Name = "inputSelectorBox";
            this.inputSelectorBox.Size = new System.Drawing.Size(270, 85);
            this.inputSelectorBox.TabIndex = 12;
            this.inputSelectorBox.TabStop = false;
            this.inputSelectorBox.Text = "Input selector";
            // 
            // buttonChangeInput
            // 
            this.buttonChangeInput.Location = new System.Drawing.Point(6, 49);
            this.buttonChangeInput.Name = "buttonChangeInput";
            this.buttonChangeInput.Size = new System.Drawing.Size(132, 23);
            this.buttonChangeInput.TabIndex = 21;
            this.buttonChangeInput.Text = "Change Input";
            this.buttonChangeInput.UseVisualStyleBackColor = true;
            this.buttonChangeInput.Click += new System.EventHandler(this.ButtonChangeInput_Click);
            // 
            // inputSelector
            // 
            this.inputSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputSelector.FormattingEnabled = true;
            this.inputSelector.Location = new System.Drawing.Point(6, 19);
            this.inputSelector.Name = "inputSelector";
            this.inputSelector.Size = new System.Drawing.Size(153, 21);
            this.inputSelector.TabIndex = 0;
            // 
            // listeningModeBox
            // 
            this.listeningModeBox.Controls.Add(this.buttonChangeMode);
            this.listeningModeBox.Controls.Add(this.listeningMode);
            this.listeningModeBox.Location = new System.Drawing.Point(160, 135);
            this.listeningModeBox.Name = "listeningModeBox";
            this.listeningModeBox.Size = new System.Drawing.Size(270, 98);
            this.listeningModeBox.TabIndex = 13;
            this.listeningModeBox.TabStop = false;
            this.listeningModeBox.Text = "Listening mode";
            // 
            // buttonChangeMode
            // 
            this.buttonChangeMode.Location = new System.Drawing.Point(6, 46);
            this.buttonChangeMode.Name = "buttonChangeMode";
            this.buttonChangeMode.Size = new System.Drawing.Size(132, 23);
            this.buttonChangeMode.TabIndex = 22;
            this.buttonChangeMode.Text = "Change Mode";
            this.buttonChangeMode.UseVisualStyleBackColor = true;
            this.buttonChangeMode.Click += new System.EventHandler(this.ButtonChangeMode_Click);
            // 
            // listeningMode
            // 
            this.listeningMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.listeningMode.FormattingEnabled = true;
            this.listeningMode.Location = new System.Drawing.Point(6, 19);
            this.listeningMode.Name = "listeningMode";
            this.listeningMode.Size = new System.Drawing.Size(153, 21);
            this.listeningMode.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.disconnectButton);
            this.groupBox7.Controls.Add(this.connectButton);
            this.groupBox7.Controls.Add(this.label6);
            this.groupBox7.Controls.Add(this.connectionPort);
            this.groupBox7.Location = new System.Drawing.Point(13, 27);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(126, 114);
            this.groupBox7.TabIndex = 17;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Receiver protocol";
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(57, 76);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(69, 23);
            this.disconnectButton.TabIndex = 4;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(0, 76);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(55, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Port:";
            // 
            // connectionPort
            // 
            this.connectionPort.Location = new System.Drawing.Point(45, 46);
            this.connectionPort.Name = "connectionPort";
            this.connectionPort.Size = new System.Drawing.Size(73, 20);
            this.connectionPort.TabIndex = 2;
            this.connectionPort.Text = "COM1";
            // 
            // buttonPowerOn
            // 
            this.buttonPowerOn.Location = new System.Drawing.Point(13, 148);
            this.buttonPowerOn.Name = "buttonPowerOn";
            this.buttonPowerOn.Size = new System.Drawing.Size(75, 23);
            this.buttonPowerOn.TabIndex = 18;
            this.buttonPowerOn.Text = "Power On";
            this.buttonPowerOn.UseVisualStyleBackColor = true;
            this.buttonPowerOn.Click += new System.EventHandler(this.ButtonPowerOn_Click);
            // 
            // buttonPowerOff
            // 
            this.buttonPowerOff.Location = new System.Drawing.Point(13, 178);
            this.buttonPowerOff.Name = "buttonPowerOff";
            this.buttonPowerOff.Size = new System.Drawing.Size(75, 23);
            this.buttonPowerOff.TabIndex = 19;
            this.buttonPowerOff.Text = "Power Off";
            this.buttonPowerOff.UseVisualStyleBackColor = true;
            this.buttonPowerOff.Click += new System.EventHandler(this.ButtonPowerOff_Click);
            // 
            // buttonStatusRequest
            // 
            this.buttonStatusRequest.Location = new System.Drawing.Point(481, 27);
            this.buttonStatusRequest.Name = "buttonStatusRequest";
            this.buttonStatusRequest.Size = new System.Drawing.Size(132, 23);
            this.buttonStatusRequest.TabIndex = 20;
            this.buttonStatusRequest.Text = "Status Request";
            this.buttonStatusRequest.UseVisualStyleBackColor = true;
            this.buttonStatusRequest.Click += new System.EventHandler(this.ButtonStatusRequest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxStatus);
            this.groupBox1.Location = new System.Drawing.Point(481, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 199);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Location = new System.Drawing.Point(7, 20);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.Size = new System.Drawing.Size(190, 173);
            this.textBoxStatus.TabIndex = 0;
            // 
            // ReceiverForm
            // 
            this.ClientSize = new System.Drawing.Size(772, 406);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonStatusRequest);
            this.Controls.Add(this.buttonPowerOff);
            this.Controls.Add(this.buttonPowerOn);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.listeningModeBox);
            this.Controls.Add(this.inputSelectorBox);
            this.Controls.Add(this.groupBox2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReceiverForm";
            this.Text = "Receiver Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TestApp_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.inputSelectorBox.ResumeLayout(false);
            this.listeningModeBox.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private TextBox logText;
		private Button buttonPowerOn;
		private Button buttonPowerOff;
		private Button buttonStatusRequest;
		private GroupBox groupBox1;
		private TextBox textBoxStatus;
		private Button buttonChangeInput;
		private Button buttonChangeMode;
	}
}