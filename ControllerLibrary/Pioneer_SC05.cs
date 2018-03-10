using System;
using System.Diagnostics;
using System.Threading;

namespace ReceiverController
{
    /// <summary>
    /// The interface for the Pioneer SC05 7 channel receiver
    /// </summary>
    public class Pioneer_SC05 : IReceiver
    {
        private static readonly Mode[] MasterInputModes;
        private static readonly Mode[] ListeningModes;
        private static readonly Mode[] StatusModes;

        private readonly SerialConnection _connection;
        private PowerState _powerState;
        private PowerState _mute;
        private float _volume;
        private Mode _inputMode;
        private int _treble;
        private int _bass;
        private Tone _toneStatus;
        private Mode _listenMode;
        private string _port;

        public event EventHandler StatusChanged;

        #region Static initilization

        static Pioneer_SC05()
        {
            MasterInputModes = new[]
                                    {
                                        new Mode("PHONO", "00"),
                                        new Mode("CD", "01"),
                                        new Mode("TUNER", "02"),
                                        new Mode("CDR", "03"),
                                        new Mode("DVD", "04"),
                                        new Mode("TV", "05"),
                                        new Mode("VIDEO1", "10"),
                                        new Mode("MULTI CHANNEL", "12"),
                                        new Mode("VIDEO2", "14"),
                                        new Mode("DVR1", "15"),
                                        new Mode("XM", "18"),
                                        new Mode("HDMI1", "19"),
                                        new Mode("HDMI2", "20"),
                                        new Mode("HDMI3", "21"),
                                        new Mode("BDP", "25"),
                                        new Mode("HOME MEDIA", "26"),
                                        new Mode("SIRIUS", "27"),
                                        new Mode("HDMI CYCLIC", "31"),
                                        new Mode("VIDEO3", "32")
                                    };

            StatusModes = new[]
                               {
                                   new Mode("VOLUME LEVEL", "V"),
                                   new Mode("POWER STATUS", "P"),
                                   new Mode("MUTE STATUS", "M"),
                                   new Mode("FUNCTION MODE", "F"),
                                   new Mode("LISTENING MODE SETTING", "S"),
                                   new Mode("LISTENING MODE", "L"),
                                   new Mode("TONE STATUS", "TO"),
                                   new Mode("BASS STATUS", "BA"),
                                   new Mode("TREBLE STATUS", "TR"),
                                   new Mode("TUNER PRESET STATUS", "PR"),
                                   new Mode("TUNER FREQ REQUEST", "FR"),
                                   new Mode("MULTI INPUT CH REQUEST", "MI"),
                                   new Mode("ZONE 2 POWER STATUS REQUEST", "AP"),
                                   new Mode("ZONE 3 POWER STATUS REQUEST", "BP"),
                                   new Mode("ZONE 2 FUNCTION STATUS REQUEST", "Z2"),
                                   new Mode("ZONE 3 FUNCTION STATUS REQUEST", "Z3"),
                                   new Mode("ZONE 2 VOLUME STATUS REQUEST", "ZV"),
                                   new Mode("ZONE 3 VOLUME STATUS REQUEST", "YV"),
                                   new Mode("MCACC POSITION REQUEST", "MC"),
                                   new Mode("SBch PROCESSING STATUS REQUEST", "EX"),
                                   new Mode("XM channel REQUEST", "XM"),
                                   new Mode("PHASE CONTROL STATUS REQUEST", "IS"),
                                   new Mode("Sirius channel REQUEST", "SI")
                               };

            ListeningModes = new[]
                                  {
                                      new Mode("DIRECT", "002"),
                                      new Mode("AUTO SURROUND/STREAM DIRECT", "005"),
                                      new Mode("AUTO SURROUND", "006"),
                                      new Mode("NORMAL DIRECT", "007"),
                                      new Mode("PURE DIRECT", "008"),
                                      new Mode("STEREO", "009"),
                                      new Mode("STANDARD SELECTION", "010"),
                                      new Mode("PRO LOGIC", "012"),
                                      new Mode("Neo:6 CINEMA", "016"),
                                      new Mode("Neo:6 MUSIC", "017"),
                                      new Mode("THX SELECTION", "050"),
                                      new Mode("ADVANCED SURROUND SELECTION", "100")
                                  };
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Pioneer_SC05"/> class.
        /// </summary>
        public Pioneer_SC05()
        {
            this._connection = new SerialConnection();
            this._connection.ReceivedData += Connection_ReceivedData;
        }

        /// <summary>
        /// Connects the specified port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public bool Connect(string port)
        {
            this._port = port;
            return this._connection.Connect(port);
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            this._connection.Disconnect();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected
        {
            get { return this._connection.IsConnected; }
        }

        #region Device Specific Properties

        /// <summary>
        /// Volumes the max.
        /// </summary>
        /// <returns></returns>
        public int VolumeMax()
        {
            return 12;
        }

        /// <summary>
        /// Volumes the min.
        /// </summary>
        /// <returns></returns>
        public int VolumeMin()
        {
            return -81;
        }

        /// <summary>
        /// Devices the name.
        /// </summary>
        /// <returns></returns>
        public string DeviceName()
        {
            return "Pioneer Elite SC-05";
        }

        #endregion

        #region Device Functions

        /// <summary>
        /// Statuses the request.
        /// </summary>
        /// <param name="statusMode">The status mode.</param>
        public void StatusRequest(string statusMode)
        {
            foreach (var mode in StatusModes)
            {
                if (mode.Name == statusMode)
                {
                    this.SendCommand('?' + mode.Flag);
                    return;
                }
            }
            throw new Exception("Unknown status request " + statusMode);
        }

        /// <summary>
        /// Requests the input mode i.e. DVD or HDMI1.
        /// </summary>
        /// <param name="mode">The mode.</param>
        public void RequestInputMode(string mode)
        {
            foreach (var t in MasterInputModes)
            {
                if (t.Name == mode)
                {
                    this.SendCommand(t.Flag + "FN");
                }
            }
        }

        /// <summary>
        /// Requests the listening mode.
        /// </summary>
        /// <param name="mode">The mode.</param>
        public void RequestListeningMode(string mode)
        {
            foreach (var t in ListeningModes)
            {
                if (t.Name == mode)
                {
                    this.SendCommand("0" + t.Flag + "SR");
                }
            }
        }

        /// <summary>
        /// Powers the off.
        /// </summary>
        public void PowerOff()
        {
            this.SendCommand("PF");
        }

        /// <summary>
        /// Powers the on.
        /// </summary>
        public void PowerOn()
        {
            //the receiver manual suggest to send this twice to give the processor a chance to wake up
            this.SendCommand("PO");
            Thread.Sleep(100);
            this.SendCommand("PO");
        }

        /// <summary>
        /// Gets the state of the master power.
        /// </summary>
        /// <value>The state of the master power.</value>
        public PowerState MasterPowerState
        {
            get { return this._powerState; }
        }

        /// <summary>
        /// Gets the mute.
        /// </summary>
        /// <value>The mute.</value>
        public PowerState Mute
        {
            get { return _mute; }
        }

        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>The volume.</value>
        public float Volume
        {
            get { return _volume; }
        }

        /// <summary>
        /// Gets the input mode.
        /// </summary>
        /// <value>The input mode.</value>
        public string InputMode
        {
            get { return _inputMode.Name; }
        }

        /// <summary>
        /// Gets the treble.
        /// </summary>
        /// <value>The treble.</value>
        public int Treble
        {
            get { return _treble; }
        }

        /// <summary>
        /// Gets the bass.
        /// </summary>
        /// <value>The bass.</value>
        public int Bass
        {
            get { return _bass; }
        }

        /// <summary>
        /// Gets the tone status.
        /// </summary>
        /// <value>The tone status.</value>
        public Tone ToneStatus
        {
            get { return _toneStatus; }
        }

        /// <summary>
        /// Gets the listen mode.
        /// </summary>
        /// <value>The listen mode.</value>
        public string ListenMode
        {
            get { return _listenMode.Name; }
        }

        #endregion

        /// <summary>
        /// Sends the command.
        /// </summary>
        /// <param name="command">The command.</param>
        private void SendCommand(string command)
        {
            Trace.WriteLine("Sending command " + command);

            //automatically reconnect
            if (!this._connection.IsConnected)
            {
                if (!this._connection.Connect(_port))
                    throw new Exception("Attempt to reconnect port " + _port + "failed.");
            }

            this._connection.Write(command + '\r');
        }

        /// <summary>
        /// Handles the ReceivedData event of the mConnection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ReceivedDataEventArgs"/> instance containing the event data.</param>
        private void Connection_ReceivedData(object sender, ReceivedDataEventArgs e)
        {
            string line = e.Data;
            if (string.IsNullOrEmpty(line))
                return;

            Trace.WriteLine("Got data " + line);

            //parse the received line and update the internal state accordinly...
            if (line.StartsWith("PWR"))
            {
                switch (line[3])
                {
                    case '0':
                        this._powerState = PowerState.On;
                        break;
                    case '1':
                        this._powerState = PowerState.Off;
                        break;
                    default:
                        this._powerState = PowerState.Unknown;
                        break;
                }
            }
            else if (line.StartsWith("VOL"))
            {
                if (int.TryParse(line.Substring(3, 3), out var volume))
                {
                    if (volume == 0) //this is _mute
                        _volume = float.NaN;
                    else
                    {
                        _volume = (volume / 2f) + -80.5f;
                    }
                }
                else
                {
                    throw new Exception("Could not parse '" + line.Substring(3, 3) + "' ");
                }
            }
            else if (line.StartsWith("MUT"))
            {
                switch (line[3])
                {
                    case '0':
                        this._mute = PowerState.On;
                        break;
                    case '1':
                        this._mute = PowerState.Off;
                        break;
                    default:
                        this._mute = PowerState.Unknown;
                        break;
                }
            }
            else if (line.StartsWith("FN"))
            {
                string function = line.Substring(2, 2);
                {
                    foreach (Mode mode in MasterInputModes)
                    {
                        if (mode.Flag == function)
                        {
                            this._inputMode = mode;
                            break;
                        }
                    }
                }
            }
            else if (line.StartsWith("TR"))
            {
                if (!int.TryParse(line.Substring(2, 2), out var treble))
                    throw new Exception("Could not parse " + line);

                _treble = treble;
            }
            else if (line.StartsWith("BA"))
            {
                if (!int.TryParse(line.Substring(2, 2), out var bass))
                    throw new Exception("Could not parse " + line);

                _bass = bass;
            }
            else if (line.StartsWith("TO"))
            {
                if (!int.TryParse(line.Substring(2, 2), out var tone))
                    throw new Exception("Could not parse " + line);

                switch (tone)
                {
                    case 0:
                        _toneStatus = Tone.Bypass;
                        break;
                    case 1:
                        _toneStatus = Tone.On;
                        break;
                    default:
                        _toneStatus = Tone.Unknown;
                        break;
                }
            }
            else if (line.StartsWith("SR0"))
            {
                string listenMode = line.Substring(3, 3);

                foreach (Mode mode in ListeningModes)
                {
                    if (mode.Flag == listenMode)
                    {
                        this._listenMode = mode;
                        break;
                    }
                }
            }

            OnStatusChanged();
        }

        /// <summary>
        /// Gets the input modes.
        /// </summary>
        /// <returns></returns>
        public string[] GetInputModes()
        {
            string[] lModes = new string[MasterInputModes.Length];
            for (int i = 0; i < lModes.Length; i++)
            {
                lModes[i] = MasterInputModes[i].Name;
            }
            return lModes;
        }

        /// <summary>
		/// Gets the input modes.
		/// </summary>
		/// <returns></returns>
		public string[] GetListeningModes()
        {
            string[] lModes = new string[ListeningModes.Length];
            for (int i = 0; i < lModes.Length; i++)
            {
                lModes[i] = ListeningModes[i].Name;
            }
            return lModes;
        }

        /// <summary>
        /// Gets the input modes.
        /// </summary>
        /// <returns></returns>
        public string[] GetStatusModes()
        {
            string[] lModes = new string[StatusModes.Length];
            for (int i = 0; i < lModes.Length; i++)
            {
                lModes[i] = StatusModes[i].Name;
            }
            return lModes;
        }

        /// <summary>
        /// Called when [status changed].
        /// </summary>
        private void OnStatusChanged()
        {
            this.StatusChanged?.Invoke(this, new EventArgs());
        }
    }

    public enum PowerState
    {
        Unknown,
        On,
        Off
    }

    public enum Tone
    {
        Unknown,
        Bypass,
        On
    }

}