using System;

namespace ReceiverController
{
	public interface IReceiver 
	{
		bool Connect(string port);
		void Disconnect();
		bool IsConnected { get; }
		int VolumeMax();
		int VolumeMin();
		string DeviceName();
        
		void PowerOn();
		void PowerOff();

		PowerState MasterPowerState { get; }

		void StatusRequest(string statusMode);

		void RequestInputMode(string mode);
		void RequestListeningMode(string mode);

		event EventHandler StatusChanged;

		string[] GetStatusModes();
		string[] GetInputModes();
		string[] GetListeningModes();
	}
}