using System;

namespace Microsoft.Xna.Framework.Input
{
	public static class GamePadEx
	{
		// extreme values for the analog GamePad buttons 
		public const float TRIGGER_MAX = 1.00f;
		public const float TRIGGER_MIN = 0.00f;
		public const float THUMBSTICK_MAX = 1.00f;
		public const float THUMBSTICK_MIN = -1.00f;

		// set the emulated controller index
		public static PlayerIndex? KeyboardPlayerIndex { get; set; }

		// is the specified playerIndex the one associated with the keyboard?
		private static bool IsKeyboardPlayerIndex(PlayerIndex playerIndex) 
		{
			return 
				KeyboardPlayerIndex.HasValue &&
				KeyboardPlayerIndex.Value == playerIndex;
		}

		public static GamePadCapabilitiesEx GetCapabilities(PlayerIndex playerIndex)
		{
			if(GamePadEx.IsKeyboardPlayerIndex(playerIndex))
			{
				return GamePadCapabilitiesEx.KeyboardCapabilities;
			} else {
				return
					new GamePadCapabilitiesEx(GamePad.GetCapabilities(playerIndex));
			}
		}

		public static GamePadState GetState(PlayerIndex playerIndex)
		{
			return GamePadEx.GetState(
				playerIndex, 
				GamePadDeadZone.IndependentAxes);
		}

		public static GamePadState GetState(
			PlayerIndex playerIndex, 
			GamePadDeadZone deadZoneMode)
		{
			if(GamePadEx.IsKeyboardPlayerIndex(playerIndex))
			{
				return GamePadEx.EmulateGamePadState();
			} else {
				return GamePad.GetState (playerIndex, deadZoneMode);
			}
		}

		// TODO: populate the GamePadState from KeyboardState data
		private static GamePadState EmulateGamePadState()
		{
			var keyState = Keyboard.GetState ();

			var leftTrigger = keyState.IsKeyDown(Keys.LeftAlt) ?
				TRIGGER_MAX : TRIGGER_MIN;
			var rightTrigger = keyState.IsKeyDown(Keys.RightAlt) ?
				TRIGGER_MAX : TRIGGER_MIN;

			var dPadUp = keyState.IsKeyDown(Keys.W) ?
				ButtonState.Pressed : ButtonState.Released;
			var dPadDown = keyState.IsKeyDown(Keys.S) ?
				ButtonState.Pressed : ButtonState.Released;
			var dPadLeft = keyState.IsKeyDown(Keys.A) ?
				ButtonState.Pressed : ButtonState.Released;
			var dPadRight = keyState.IsKeyDown(Keys.D) ?
				ButtonState.Pressed : ButtonState.Released;

			// mimic DPad
			var leftThumbstick = Vector2.Zero;
			if (dPadUp == ButtonState.Pressed) {
				leftThumbstick.Y = THUMBSTICK_MAX;
				dPadDown = ButtonState.Released;
			} else if (dPadDown == ButtonState.Pressed) {
				leftThumbstick.Y = THUMBSTICK_MIN;
				dPadUp = ButtonState.Released;
			}
			if (dPadLeft == ButtonState.Pressed) {
				leftThumbstick.X = THUMBSTICK_MIN;
				dPadRight = ButtonState.Released;
			} else if (dPadRight == ButtonState.Pressed) {
				leftThumbstick.X = THUMBSTICK_MAX;
				dPadLeft = ButtonState.Released;
			}

			var rightThumbstick = Vector2.Zero;
			if (keyState.IsKeyDown(Keys.Down)) {
				rightThumbstick.Y = THUMBSTICK_MIN;
			} else if (keyState.IsKeyDown(Keys.Up)) {
				rightThumbstick.Y = THUMBSTICK_MAX;
			}
			if (keyState.IsKeyDown(Keys.Left)) {
				rightThumbstick.X = THUMBSTICK_MIN;
			} else if (keyState.IsKeyDown(Keys.Right)) {
				rightThumbstick.X = THUMBSTICK_MAX;
			}


			Buttons buttons = 0;
			if (keyState.IsKeyDown (Keys.Space)) 
				{ buttons |= Buttons.A; }
			if (keyState.IsKeyDown (Keys.RightControl)) 
				{ buttons |= Buttons.B; }
			if (keyState.IsKeyDown (Keys.PageDown)) 
				{ buttons |= Buttons.X; }
			if (keyState.IsKeyDown (Keys.PageUp)) 
				{ buttons |= Buttons.Y; }
			if (keyState.IsKeyDown (Keys.Escape)) 
				{ buttons |= Buttons.Back; }
			if (keyState.IsKeyDown (Keys.Enter)) 
				{ buttons |= Buttons.Start; }

			return new GamePadState(
				new GamePadThumbSticks(leftThumbstick, rightThumbstick), 
				new GamePadTriggers(leftTrigger, rightTrigger), 
				new GamePadButtons(buttons),
				new GamePadDPad(dPadUp, dPadDown, dPadLeft, dPadRight));
		}

		public static bool SetVibration(
			PlayerIndex playerIndex, 
			float leftMotor, 
			float rightMotor)
		{
			if(GamePadEx.IsKeyboardPlayerIndex(playerIndex))
			{
				return false;
			} else {
				return GamePad.SetVibration (
					playerIndex, 
					leftMotor, 
					rightMotor);
			}
		}
	}
}