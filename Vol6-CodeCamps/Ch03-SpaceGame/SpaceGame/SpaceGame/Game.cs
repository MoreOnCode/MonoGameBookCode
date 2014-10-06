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

		// our ship's location
		protected Vector2 locShip = Vector2.Zero;

		// reference our starfield
		protected Texture2D texStars;

		// our background's location
		protected Vector2 locStars = Vector2.Zero;

		// screen resolution
		protected Rectangle rectViewBounds = Rectangle.Empty;

		// NEW: player bounds
		protected Rectangle rectPlayerBounds = Rectangle.Empty;

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

			// load our space background
			texStars = Content.Load<Texture2D> ("purple");

			// screen bounds
			rectViewBounds = graphics.GraphicsDevice.Viewport.Bounds;

			// NEW: player bounds
			rectPlayerBounds = rectViewBounds;
			rectPlayerBounds.X = 10;
			rectPlayerBounds.Width = rectViewBounds.Width - 10 * 2;
			rectPlayerBounds.Y = rectViewBounds.Height / 3;
			rectPlayerBounds.Height = rectViewBounds.Height - rectPlayerBounds.Top - 10;

			// NEW: start ship at center, bottom of screen
			locShip.X = rectViewBounds.Width / 2 - texShip.Bounds.Width / 2;
			locShip.Y = rectViewBounds.Height - texShip.Bounds.Height - 10.0f;

			// start stars off screen
			locStars.Y = -texStars.Bounds.Height;
        }

        protected override void Update(GameTime gameTime)
        {
			var gamepad1 = GamePadEx.GetState (PlayerIndex.One);
			if (gamepad1.IsButtonDown (Buttons.Back)) {
				this.Exit ();
			} else {
				// move the ship
				var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
				var dX = 
					gamepad1.ThumbSticks.Left.X +
					gamepad1.ThumbSticks.Right.X;
				var dY = 
					gamepad1.ThumbSticks.Left.Y +
					gamepad1.ThumbSticks.Right.Y;
				locShip.X += dX * SPEED_SHIP * elapsed;
				locShip.Y -= dY * SPEED_SHIP * elapsed;

				// NEW: keep ship in bounds (Horizontal)
				var maxX = rectPlayerBounds.Right - texShip.Bounds.Width;
				if (locShip.X < rectPlayerBounds.X) {
					locShip.X = rectPlayerBounds.X;
				} else if(locShip.X > maxX) {
					locShip.X = maxX;
				}

				// NEW: keep ship in bounds (Vertical)
				var maxY = rectPlayerBounds.Bottom - texShip.Bounds.Height;
				if (locShip.Y < rectPlayerBounds.Y) {
					locShip.Y = rectPlayerBounds.Y;
				} else if(locShip.Y > maxY) {
					locShip.Y = maxY;
				}

				// move the stars
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

			// NEW: draw our space image at current location, filling screen
			var loc = locStars;
			while (loc.Y < rectViewBounds.Bottom) {
				loc.X = 0.0f;
				while (loc.X < rectViewBounds.Right) {
					spriteBatch.Draw (texStars, loc, Color.White);
					loc.X += texStars.Bounds.Width;
				}
				loc.Y += texStars.Bounds.Height;
			}

			// draw our spaceship image at current location
			spriteBatch.Draw (texShip, locShip, Color.White);

			spriteBatch.End ();
			base.Draw (gameTime);
		}
    }
}
