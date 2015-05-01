using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoreOnCode.Screens
{
	public static class ScreenManager
	{
		private static Screen _CurrentScreen = null;
		public static Screen CurrentScreen { 
			get { return _CurrentScreen; }
			set {
				if (_CurrentScreen != null) {
					_CurrentScreen.OnHide ();
				}
				_CurrentScreen = value;
				if (_CurrentScreen != null) {
					_CurrentScreen.OnShow ();
				}
			}
		}

		public static Game ParentGame { get; set; }

		public static GamePadState GamePad1 { get; private set; }
		public static GamePadState GamePad2 { get; private set; }
		public static GamePadState GamePad3 { get; private set; }
		public static GamePadState GamePad4 { get; private set; }

		public static void Update(GameTime gameTime) {
			GamePad1 = GamePadEx.GetState (PlayerIndex.One);
			GamePad2 = GamePadEx.GetState (PlayerIndex.Two);
			GamePad3 = GamePadEx.GetState (PlayerIndex.Three);
			GamePad4 = GamePadEx.GetState (PlayerIndex.Four);

			if (CurrentScreen != null) {
				CurrentScreen.Update (gameTime);
			}
		}

		public static void Draw(GameTime gameTime, SpriteBatch batch) {
			if (CurrentScreen != null) {
				CurrentScreen.Draw (gameTime, batch);
			}
		}

	}
}

