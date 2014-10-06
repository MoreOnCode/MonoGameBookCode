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

		// speed constants
		protected const float SPEED_STARS = 20.0f;
		protected const float SPEED_SHIP = 75.0f;

		// reference our spaceship
		protected Texture2D texShip;

		// NEW: our ship's location
		protected Vector2 locShip = Vector2.Zero;

		// NEW: reference our starfield
		protected Texture2D texStars;

		// NEW: our background's location
		protected Vector2 locStars = Vector2.Zero;

		// NEW: screen resolution
		protected Rectangle rectViewBounds = Rectangle.Empty;

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

			// NEW: load our spacey background
			texStars = Content.Load<Texture2D> ("purple");

			// NEW: screen bounds
			rectViewBounds = graphics.GraphicsDevice.Viewport.Bounds;

			// NEW: start ship in center of screen
			locShip.X = rectViewBounds.Width / 2 - texShip.Bounds.Width / 2;
			locShip.Y = rectViewBounds.Height / 2 - texShip.Bounds.Height / 2;

			// NEW: start stars off screen
			locStars.Y = -texStars.Bounds.Height;
        }

        protected override void Update(GameTime gameTime)
        {
			var gamepad1 = GamePadEx.GetState (PlayerIndex.One);
			if (gamepad1.IsButtonDown (Buttons.Back)) {
				this.Exit ();
			} else {
				// NEW: move the ship
				var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
				var dX = 
					gamepad1.ThumbSticks.Left.X +
					gamepad1.ThumbSticks.Right.X;
				var dY = 
					gamepad1.ThumbSticks.Left.Y +
					gamepad1.ThumbSticks.Right.Y;
				locShip.X += dX * SPEED_SHIP * elapsed;
				locShip.Y -= dY * SPEED_SHIP * elapsed;

				// NEW: move the stars
				locStars.Y += SPEED_STARS * elapsed;
				if (locStars.Y >= 0.0f) {
					locStars.Y = -texStars.Bounds.Height;
				}
			}
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
			spriteBatch.Begin ();

			// NEW: draw our space image at current location
			spriteBatch.Draw (texStars, locStars, Color.White);

			// NEW: draw our spaceship image at current location
			spriteBatch.Draw (texShip, locShip, Color.White);

			spriteBatch.End ();
			base.Draw (gameTime);
		}
    }
}
