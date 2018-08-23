using System;

namespace ReceiverController
{
	public interface IReceiver 
	{
		bool Connect(string port);
		void Disconnect();
		bool IsConnected { get; }
		int VolumeMax { get; }
        int VolumeMin { get; }
	    string DeviceName { get; }

	    void PowerOn();
		void PowerOff();

		PowerState MasterPowerState { get; }

		void SendStatusRequest(StatusRequest request);

		void RequestInputMode(InputMode mode);
		void RequestListeningMode(ListeningMode mode);

		event EventHandler StatusChanged;
	}
}