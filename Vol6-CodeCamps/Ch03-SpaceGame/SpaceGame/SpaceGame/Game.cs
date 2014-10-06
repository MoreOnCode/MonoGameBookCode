using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace SpaceGame
{
    public class SpaceGame : Microsoft.Xna.Framework.Game
    {
        // Resources for drawing.
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

		// reference our spaceship
		protected Texture2D texShip;

		// NEW: our ship's location
		protected Vector2 locShipStart = Vector2.One * 100.0f;
		protected Vector2 locShipDelta = Vector2.Zero;

        public SpaceGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

			if(PlatformHelper.CurrentPlatform == Platforms.WindowsPhone) 
			{
	            TargetElapsedTime = TimeSpan.FromTicks(333333);
			}

			graphics.IsFullScreen = PlatformHelper.IsMobile;
			this.IsMouseVisible = PlatformHelper.IsDesktop;

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;

            graphics.SupportedOrientations = 
				DisplayOrientation.LandscapeLeft | 
				DisplayOrientation.LandscapeRight;

			GamePadEx.KeyboardPlayerIndex = PlayerIndex.One;
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

			// load our spaceship image
			texShip = Content.Load<Texture2D> ("playerShip1_red");
        }

        protected override void Update(GameTime gameTime)
        {
			var gamepad1 = GamePadEx.GetState (PlayerIndex.One);
			if (gamepad1.IsButtonDown (Buttons.Back)) {
				this.Exit ();
			} else {
				// NEW: move the ship
				var dX = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds);
				locShipDelta.X = 75.0f * dX;
			}
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
			spriteBatch.Begin ();

			// NEW: draw our spaceship image at current location
			spriteBatch.Draw (
				texShip, 
				locShipStart + locShipDelta, 
				Color.White);

			spriteBatch.End ();
			base.Draw (gameTime);
		}
    }
}
