using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoreOnCode.Screens
{
	public abstract class Screen
	{
		protected Game Parent 
			{ get { return ScreenManager.ParentGame; } }

		protected GamePadState GamePad1 
			{ get { return ScreenManager.GamePad1; } }
		protected GamePadState GamePad2 
			{ get { return ScreenManager.GamePad2; } }
		protected GamePadState GamePad3 
			{ get { return ScreenManager.GamePad3; } }
		protected GamePadState GamePad4 
			{ get { return ScreenManager.GamePad4; } }

		public Screen() { }

		public abstract void Update (GameTime gameTime);
		public abstract void Draw (GameTime gameTime, SpriteBatch batch);

		public virtual void OnShow() { }
		public virtual void OnHide() { }
	}
}

