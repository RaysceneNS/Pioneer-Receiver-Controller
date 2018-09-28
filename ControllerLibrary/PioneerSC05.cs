using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ReceiverController
{
    /// <summary>
    /// The interface for the Pioneer SC05 7 channel receiver
    /// </summary>
    public class PioneerSC05 : IReceiver
    {
        // map status modes to low level commands
        private static readonly Dictionary<StatusRequest, string> StatusModeCommands = new Dictionary<StatusRequest, string>
        {
            {StatusRequest.VolumeLevel, "V"},
            {StatusRequest.PowerStatus, "P"},
            {StatusRequest.MuteStatus, "M"},
            {StatusRequest.FunctionMode, "F"},
            {StatusRequest.ListeningModeSetting, "S"},
            {StatusRequest.ListeningMode, "L"},
            {StatusRequest.ToneStatus, "TO"},
            {StatusRequest.BassStatus, "BA"},
            {StatusRequest.TrebleStatus, "TR"},
            {StatusRequest.TunerPresetStatus, "PR"},
            {StatusRequest.TunerFreqRequest, "FR"},
            {StatusRequest.MultiInputChannel, "MI"},
            {StatusRequest.Zone2PowerStatus, "AP"},
            {StatusRequest.Zone3PowerStatus, "BP"},
            {StatusRequest.Zone2FunctionStatus, "Z2"},
            {StatusRequest.Zone3FunctionStatus, "Z3"},
            {StatusRequest.Zone2VolumeStatus, "ZV"},
            {StatusRequest.Zone3VolumeStatus, "YV"},
            {StatusRequest.MCACCPosition, "MC"},
            {StatusRequest.XmChannel, "XM"},
            {StatusRequest.PhaseControlStatus, "IS"},
            {StatusRequest.SiriusChannel, "SI"},
            {StatusRequest.SBchProcessingStatus, "EX"}
        };

        private static readonly Dictionary<InputMode, string> InputModeCommands = new Dictionary<InputMode, string>
        {
            {InputMode.Phono, "00"},
            {InputMode.Cd, "01"},
            {InputMode.Tuner, "02"},
            {InputMode.Cdr, "03"},
            {InputMode.Dvd, "04"},
            {InputMode.Tv, "05"},
            {InputMode.Video1, "10"},
            {InputMode.MultiChannel, "12"},
            {InputMode.Video2, "14"},
            {InputMode.Dvr1, "15"},
            {InputMode.Xm, "18"},
            {InputMode.Hdmi1, "19"},
            {InputMode.Hdmi2, "20"},
            {InputMode.Hdmi3, "21"},
            {InputMode.Bdp, "25"},
            {InputMode.HomeMedia, "26"},
            {InputMode.Sirius, "27"},
            {InputMode.HdmiCyclic, "31"},
            {InputMode.Video3, "32"}
        };

        private static readonly Dictionary<ListeningMode, string> ListeningModeCommands =
            new Dictionary<ListeningMode, string>
            {
                {ListeningMode.Direct, "002"},
                {ListeningMode.AutoSurroundStreamDirect, "005"},
                {ListeningMode.AutoSurround, "006"},
                {ListeningMode.NormalDirect, "007"},
                {ListeningMode.PureDirect, "008"},
                {ListeningMode.Stereo, "009"},
                {ListeningMode.StandardSelection, "010"},
                {ListeningMode.ProLogic, "012"},
                {ListeningMode.Neo6Cinema, "016"},
                {ListeningMode.Neo6Music, "017"},
                {ListeningMode.ThxSelection, "050"},
                {ListeningMode.AdvancedSurroundSelection, "100"}
            };

        private readonly SerialConnection _connection;
        private string _port;
        public event EventHandler StatusChanged;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PioneerSC05"/> class.
        /// </summary>
        public PioneerSC05()
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

        public int VolumeMax
        {
            get { return 12; }
        }

        public int VolumeMin
        {
            get { return -81; }
        }

        public string DeviceName
        {
            get { return "Pioneer Elite SC-05"; }
        }

        #endregion

        #region Device Functions

        /// <summary>
        /// Issue a status request
        /// </summary>
        /// <param name="request">The status request.</param>
        public void SendStatusRequest(StatusRequest request)
        {
            if (StatusModeCommands.TryGetValue(request, out var command))
            {
                this.SendCommand('?' + command);
                return;
            }
            throw new ArgumentException("Unknown status request " + request);
        }

        /// <summary>
        /// Requests the input mode i.e. DVD or HDMI1.
        /// </summary>
        /// <param name="mode">The mode.</param>
        public void RequestInputMode(InputMode mode)
        {
            if (InputModeCommands.TryGetValue(mode, out var command))
            {
                this.SendCommand(command + "FN");
            }
        }

        /// <summary>
        /// Requests the listening mode.
        /// </summary>
        /// <param name="mode">The mode.</param>
        public void RequestListeningMode(ListeningMode mode)
        {
            if (ListeningModeCommands.TryGetValue(mode, out var command))
            {
                this.SendCommand("0" + command + "SR");
            }
        }

        /// <summary>
        /// Power off.
        /// </summary>
        public void PowerOff()
        {
            this.SendCommand("PF");
        }

        /// <summary>
        /// Power on.
        /// </summary>
        public void PowerOn()
        {
            //the receiver manual suggests that we send this twice to give the processor a chance to wake up
            this.SendCommand("PO");
            Thread.Sleep(100);
            this.SendCommand("PO");
        }

        public PowerState MasterPowerState { get; private set; }
        public PowerState Mute { get; private set; }
        public float Volume { get; private set; }
        public InputMode InputMode { get; private set; }
        public int Treble { get; private set; }
        public int Bass { get; private set; }
        public Tone ToneStatus { get; private set; }
        public ListeningMode ListenMode { get; private set; }

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
            var line = e.Data;
            if (string.IsNullOrEmpty(line))
                return;

            Trace.WriteLine("Got data " + line);

            //parse the received line and update the internal state accordingly...
            if (line.StartsWith("PWR"))
            {
                switch (line[3])
                {
                    case '0':
                        this.MasterPowerState = PowerState.On;
                        break;
                    case '1':
                        this.MasterPowerState = PowerState.Off;
                        break;
                    default:
                        this.MasterPowerState = PowerState.Unknown;
                        break;
                }
            }
            else if (line.StartsWith("VOL"))
            {
                if (int.TryParse(line.Substring(3, 3), out var volume))
                {
                    if (volume == 0) //this is _mute
                        Volume = float.NaN;
                    else
                    {
                        Volume = (volume / 2f) + -80.5f;
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
                        this.Mute = PowerState.On;
                        break;
                    case '1':
                        this.Mute = PowerState.Off;
                        break;
                    default:
                        this.Mute = PowerState.Unknown;
                        break;
                }
            }
            else if (line.StartsWith("FN"))
            {
                var function = line.Substring(2, 2);
                {
                    foreach (var mode in InputModeCommands)
                    {
                        if (mode.Value == function)
                        {
                            this.InputMode = mode.Key;
                            break;
                        }
                    }
                }
            }
            else if (line.StartsWith("TR"))
            {
                if (!int.TryParse(line.Substring(2, 2), out var treble))
                    throw new Exception("Could not parse " + line);

                Treble = treble;
            }
            else if (line.StartsWith("BA"))
            {
                if (!int.TryParse(line.Substring(2, 2), out var bass))
                    throw new Exception("Could not parse " + line);

                Bass = bass;
            }
            else if (line.StartsWith("TO"))
            {
                if (!int.TryParse(line.Substring(2, 2), out var tone))
                    throw new Exception("Could not parse " + line);

                switch (tone)
                {
                    case 0:
                        ToneStatus = Tone.Bypass;
                        break;
                    case 1:
                        ToneStatus = Tone.On;
                        break;
                    default:
                        ToneStatus = Tone.Unknown;
                        break;
                }
            }
            else if (line.StartsWith("SR0"))
            {
                var listenMode = line.Substring(3, 3);
                foreach (var mode in ListeningModeCommands)
                {
                    if (mode.Value == listenMode)
                    {
                        this.ListenMode = mode.Key;
                        break;
                    }
                }
            }

            OnStatusChanged();
        }

        /// <summary>
        /// Called when [status changed].
        /// </summary>
        private void OnStatusChanged()
        {
            this.StatusChanged?.Invoke(this, new EventArgs());
        }
    }
}