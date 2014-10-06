using System;

namespace Microsoft.Xna.Framework.Input
{
	public struct GamePadCapabilitiesEx
	{
		public bool IsConnected { get; set; }
		public bool HasAButton { get; set; }
		public bool HasBackButton { get; set; }
		public bool HasBButton { get; set; }
		public bool HasDPadDownButton { get; set; }
		public bool HasDPadLeftButton { get; set; }
		public bool HasDPadRightButton { get; set; }
		public bool HasDPadUpButton { get; set; }
		public bool HasLeftShoulderButton { get; set; }
		public bool HasLeftStickButton { get; set; }
		public bool HasRightShoulderButton { get; set; }
		public bool HasRightStickButton { get; set; }
		public bool HasStartButton { get; set; }
		public bool HasXButton { get; set; }
		public bool HasYButton { get; set; }
		public bool HasBigButton { get; set; }
		public bool HasLeftXThumbStick { get; set; }
		public bool HasLeftYThumbStick { get; set; }
		public bool HasRightXThumbStick { get; set; }
		public bool HasRightYThumbStick { get; set; }
		public bool HasLeftTrigger { get; set; }
		public bool HasRightTrigger { get; set; }
		public bool HasLeftVibrationMotor { get; set; }
		public bool HasRightVibrationMotor { get; set; }
		public bool HasVoiceSupport { get; set; }
		public GamePadType GamePadType { get; set; }

		// TODO: add a constructor to copy the values from 
		//       an existing GamePadCapatilities instance
		public GamePadCapabilitiesEx(GamePadCapabilities copy) : this() {
			this.IsConnected = copy.IsConnected;
			this.HasAButton = copy.HasAButton;
			this.HasBackButton = copy.HasBackButton;
			this.HasBButton = copy.HasBButton;
			this.HasDPadDownButton = copy.HasDPadDownButton;
			this.HasDPadLeftButton = copy.HasDPadLeftButton;
			this.HasDPadRightButton = copy.HasDPadRightButton;
			this.HasDPadUpButton = copy.HasDPadUpButton;
			this.HasLeftShoulderButton = copy.HasLeftShoulderButton;
			this.HasLeftStickButton = copy.HasLeftStickButton;
			this.HasRightShoulderButton = copy.HasRightShoulderButton;
			this.HasRightStickButton = copy.HasRightStickButton;
			this.HasStartButton = copy.HasStartButton;
			this.HasXButton = copy.HasXButton;
			this.HasYButton = copy.HasYButton;
			this.HasBigButton = copy.HasBigButton;
			this.HasLeftXThumbStick = copy.HasLeftXThumbStick;
			this.HasLeftYThumbStick = copy.HasLeftYThumbStick;
			this.HasRightXThumbStick = copy.HasRightXThumbStick;
			this.HasRightYThumbStick = copy.HasRightYThumbStick;
			this.HasLeftTrigger = copy.HasLeftTrigger;
			this.HasRightTrigger = copy.HasRightTrigger;
			this.HasLeftVibrationMotor = copy.HasLeftVibrationMotor;
			this.HasRightVibrationMotor = copy.HasRightVibrationMotor;
			this.HasVoiceSupport = copy.HasVoiceSupport;
			this.GamePadType = copy.GamePadType;
		}

		// TODO: add a constructor to populate the values for
		//       the keyboard; use it to create a static instance
		public static readonly GamePadCapabilitiesEx
			KeyboardCapabilities = new GamePadCapabilitiesEx(true);

		private GamePadCapabilitiesEx(bool isKeyboard) : this() {
			this.IsConnected = true;
			this.HasAButton = true;
			this.HasBackButton = true;
			this.HasBButton = true;
			this.HasDPadDownButton = true;
			this.HasDPadLeftButton = true;
			this.HasDPadRightButton = true;
			this.HasDPadUpButton = true;
			this.HasLeftShoulderButton = false;
			this.HasLeftStickButton = false;
			this.HasRightShoulderButton = false;
			this.HasRightStickButton = false;
			this.HasStartButton = true;
			this.HasXButton = true;
			this.HasYButton = true;
			this.HasBigButton = false;
			this.HasLeftXThumbStick = true;
			this.HasLeftYThumbStick = true;
			this.HasRightXThumbStick = true;
			this.HasRightYThumbStick = true;
			this.HasLeftTrigger = true;
			this.HasRightTrigger = true;
			this.HasLeftVibrationMotor = false;
			this.HasRightVibrationMotor = false;
			this.HasVoiceSupport = false;
			this.GamePadType = GamePadType.GamePad;
		}
	}
}