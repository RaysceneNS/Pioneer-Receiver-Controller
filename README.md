# Pioneer-Receiver-Controller

This controller library is used to control the Pioneer SC-05 7 channel HDMI receiver via the serial connection on the back of the unit. 

This allows bi-directional communication with the reciever. 

## Supported Functions

The library allows the PC to command these functions on the receiver:

### Power State (On/Off)

PowerOn() and PowerOff() functions command the master power state on the receiver.

### Input Mode Selection

RequestInputMode() Select the input from the available inputs:
- Phono
- Cd
- Tuner
- Cdr
- Dvd
- Tv
- Video1
- MultiChannel
- Video2
- Dvr1
- Xm
- Hdmi1
- Hdmi2
- Hdmi3
- Bdp
- HomeMedia
- Sirius
- HdmiCyclic
- Video3

### Listening Mode Selection

RequestListeningMode() from the available listening modes:
- Direct
- AutoSurroundStreamDirect
- AutoSurround
- NormalDirect
- PureDirect
- Stereo
- StandardSelection
- ProLogic
- Neo6Cinema
- Neo6Music
- ThxSelection
- AdvancedSurroundSelection

### Status Query 

Issue the status query command to retrieve a state as set on the receiver. 
The StatusChanged event is raised to notify when a status query has completed.

The following properties hold the state:
- MasterPowerState
- Mute
- Volume
- InputMode
- Treble
- Basss
- ToneStatus
- ListenMode


