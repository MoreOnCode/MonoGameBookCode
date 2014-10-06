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
		protected const float SPEED_METEOR = 40.0f;
		protected const float SPEED_LASER = 90.0f;

		// reference our enemy ship
		protected Texture2D texEnemyShip;

		// enemy ships' locations
		protected List<Vector3> locEnemyShips = new List<Vector3>();

		// reference to enemy laser
		protected Texture2D texEnemyLaser;

		// our enemy lasers' locations
		protected List<Vector2> locEnemyLasers = new List<Vector2> ();

		// enemy travel lanes
		protected Vector2 locStartEnemyRight = Vector2.Zero;
		protected Vector2 locStartEnemyLeft = Vector2.Zero;

		// reference our spaceship
		protected Texture2D texShip;

		// our ship's location
		protected Vector2 locShip = Vector2.Zero;

		// references for ship damage sprites
		protected List<Texture2D> texShipDamage = new List<Texture2D> ();

		// ship damaged?
		protected int shipDamageLevel = -1;

		// reference our starfield
		protected Texture2D texStars;

		// our background's location
		protected Vector2 locStars = Vector2.Zero;

		// screen resolution
		protected Rectangle rectViewBounds = Rectangle.Empty;

		// player bounds
		protected Rectangle rectPlayerBounds = Rectangle.Empty;

		// reference to meteors
		protected List<Texture2D> texMeteors = new List<Texture2D> ();

		// our meteors' locations
		protected List<Vector3> locMeteors = new List<Vector3> ();

		// reference to laser
		protected Texture2D texLaser;

		// our lasers' locations
		protected List<Vector2> locLasers = new List<Vector2> ();

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
			texShip = Content.Load<Texture2D> ("ship/playerShip1_red");

			// load our space background
			texStars = Content.Load<Texture2D> ("purple");

			// load our meteors
			texMeteors.Add (Content.Load<Texture2D> ("meteor/meteorBrown_big1"));
			texMeteors.Add (Content.Load<Texture2D> ("meteor/meteorBrown_big2"));
			texMeteors.Add (Content.Load<Texture2D> ("meteor/meteorBrown_big3"));
			texMeteors.Add (Content.Load<Texture2D> ("meteor/meteorBrown_big4"));

			// load our laser
			texLaser = Content.Load<Texture2D> ("ship/laserRed01");

			// load our ship damage sprites
			texShipDamage.Add(Content.Load<Texture2D> ("ship/playerShip1_damage1"));
			texShipDamage.Add(Content.Load<Texture2D> ("ship/playerShip1_damage2"));
			texShipDamage.Add(Content.Load<Texture2D> ("ship/playerShip1_damage3"));

			// load enemy ship
			texEnemyShip = Content.Load<Texture2D> ("enemy/enemyBlack3");

			// load enemy laser
			texEnemyLaser = Content.Load<Texture2D> ("enemy/laserBlue01");

			// screen bounds
			rectViewBounds = graphics.GraphicsDevice.Viewport.Bounds;

			// player bounds
			rectPlayerBounds = rectViewBounds;
			rectPlayerBounds.X = 10;
			rectPlayerBounds.Width = rectViewBounds.Width - 10 * 2;
			rectPlayerBounds.Y = rectViewBounds.Height / 3;
			rectPlayerBounds.Height = rectViewBounds.Height - rectPlayerBounds.Top - 10;

			// start ship at center, bottom of screen
			locShip.X = rectViewBounds.Width / 2 - texShip.Bounds.Width / 2;
			locShip.Y = rectViewBounds.Height - texShip.Bounds.Height - 10.0f;

			// start stars off screen
			locStars.Y = -texStars.Bounds.Height;

			// enemy ship starting locations
			locStartEnemyRight.X = rectViewBounds.Right - 1;
			locStartEnemyRight.Y = 
				rectViewBounds.Height / 9 - 
				texEnemyShip.Bounds.Height / 2;
			locStartEnemyLeft.X = 
				1 - texEnemyShip.Bounds.Width;
			locStartEnemyRight.Y = 
				2 * rectViewBounds.Height / 9 - 
				texEnemyShip.Bounds.Height / 2;
        }

		// time between meteor creation
		protected const float METEOR_DELAY = 3.0f;
		protected float meteorElapsed = METEOR_DELAY;

		// time between enemy creation
		protected const float ENEMY_DELAY = 5.0f;
		protected float enemyElapsed = ENEMY_DELAY;
		protected bool isEnemyLeft = true;

		// random meteor placement
		protected Random rand = new Random ();

		// time between laser shots
		protected const float INIT_LASER_DELAY = 1.0f;
		protected float laserDelay = INIT_LASER_DELAY;
		protected float laserElapsed = INIT_LASER_DELAY;

		// time between enemy shots
		protected const float INIT_ENEMY_LASER_DELAY = 3.25f;

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

				// keep ship in bounds (Horizontal)
				var maxX = rectPlayerBounds.Right - texShip.Bounds.Width;
				if (locShip.X < rectPlayerBounds.X) {
					locShip.X = rectPlayerBounds.X;
				} else if(locShip.X > maxX) {
					locShip.X = maxX;
				}

				// keep ship in bounds (Vertical)
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

				// add a new enemy?
				enemyElapsed += elapsed;
				if (enemyElapsed >= ENEMY_DELAY) {
					var locEnemy = new Vector3(locStartEnemyLeft, 0.0f);
					if (!isEnemyLeft) {
						locEnemy = new Vector3(locStartEnemyRight, 0.0f);
					}
					locEnemyShips.Add (locEnemy);
					enemyElapsed = 0.0f;
					isEnemyLeft = !isEnemyLeft;
				}

				// update existing enemies
				for (int i = 0; i < locEnemyShips.Count; i++) {
					var loc = locEnemyShips [i];
					if (loc.Y == locStartEnemyLeft.Y) {
						loc.X += elapsed * SPEED_SHIP;
					} else {
						loc.X -= elapsed * SPEED_SHIP;
					}
					locEnemyShips [i] = loc;
				}

				// add a new meteor?
				meteorElapsed += elapsed;
				if (meteorElapsed >= METEOR_DELAY) {
					var iMeteor = rand.Next (texMeteors.Count);
					var meteorWidth = texMeteors [iMeteor].Bounds.Width;
					var meteorHeight = texMeteors [iMeteor].Bounds.Height;
					var randX = rand.Next(rectPlayerBounds.Width - meteorWidth);
					var loc = 
						new Vector3 (
							rectPlayerBounds.Left + randX,
							1 - meteorHeight,
							iMeteor);
					locMeteors.Add (loc);
					meteorElapsed = 0.0f;
				}

				// update existing meteors
				for (int i = 0; i < locMeteors.Count; i++) {
					var loc = locMeteors [i];
					loc.Y += elapsed * SPEED_METEOR;
					locMeteors [i] = loc;
				}

				// add a new laser?
				laserElapsed += elapsed;
				if (gamepad1.IsButtonDown (Buttons.A)) {
					if (laserElapsed >= laserDelay) {
						var loc =
							new Vector2 (
								locShip.X + texShip.Width / 2 - texLaser.Width / 2,
								locShip.Y);
						locLasers.Add (loc);
						laserElapsed = 0.0f;
					}
				}

				// update existing lasers
				for (int i = 0; i < locLasers.Count; i++) {
					var loc = locLasers [i];
					loc.Y -= elapsed * SPEED_LASER;
					locLasers [i] = loc;
				}

				// add a new enemy laser?
				for (int i = 0; i < locEnemyShips.Count; i++) {
					var loc = locEnemyShips [i];
					loc.Z += elapsed;
					if (loc.Z >= INIT_ENEMY_LASER_DELAY) {
						var locLaser =
							new Vector2 (
								loc.X + texEnemyShip.Width / 2 - texEnemyLaser.Width / 2,
								loc.Y + texEnemyShip.Height);
						locEnemyLasers.Add (locLaser);
						loc.Z = 0.0f;
					}
					locEnemyShips [i] = loc;
				}

				// update existing enemy lasers
				for (int i = 0; i < locEnemyLasers.Count; i++) {
					var loc = locEnemyLasers [i];
					loc.Y += elapsed * SPEED_LASER;
					locEnemyLasers [i] = loc;
				}

				// check for collisions
				CheckForCollisions ();
				DoHousekeeping ();
			}
            base.Update(gameTime);
        }

		// check for collisions
		protected void CheckForCollisions () {

			// --------------------
			// meteor hit by laser?
			// --------------------

			var rectMeteor = Rectangle.Empty;
			var rectLaser = texLaser.Bounds;

			// check all meteor instances
			for (int iMeteor = 0; iMeteor < locMeteors.Count; iMeteor++) {
				// create a rectangle, current location and size of meteor
				var locMeteor = locMeteors [iMeteor];
				rectMeteor = texMeteors [(int)locMeteor.Z].Bounds;
				rectMeteor.X = (int)locMeteor.X;
				rectMeteor.Y = (int)locMeteor.Y;

				// check all laser instances
				for (int iLaser = 0; iLaser < locLasers.Count; iLaser++) {
					// create a rectangle, current location and size of laser
					var locLaser = locLasers [iLaser];
					rectLaser.X = (int)locLaser.X;
					rectLaser.Y = (int)locLaser.Y;

					// does laser touch meteor?
					if (rectLaser.Intersects (rectMeteor)) {
						locMeteors.RemoveAt (iMeteor);
						iMeteor--;
						locLasers.RemoveAt (iLaser);
						iLaser--;
						break;
					}
				}
			}

			// --------------------
			// ship hit by meteor?
			// --------------------

			rectMeteor = Rectangle.Empty;
			var rectShip = texShip.Bounds;
			rectShip.X = (int)locShip.X;
			rectShip.Y = (int)locShip.Y;

			// check all meteor instances
			for (int iMeteor = 0; iMeteor < locMeteors.Count; iMeteor++) {
				// create a rectangle, current location and size of meteor
				var locMeteor = locMeteors [iMeteor];
				rectMeteor = texMeteors [(int)locMeteor.Z].Bounds;
				rectMeteor.X = (int)locMeteor.X;
				rectMeteor.Y = (int)locMeteor.Y;

				// does meteor touch ship?
				if (rectShip.Intersects (rectMeteor)) {
					locMeteors.RemoveAt (iMeteor);
					iMeteor--;
					shipDamageLevel = 
						Math.Min (texShipDamage.Count - 1, shipDamageLevel + 1);
					break;
				}
			}

			// --------------------
			// ship hit by laser?
			// --------------------

			// check all laser instances
			for (int iLaser = 0; iLaser < locEnemyLasers.Count; iLaser++) {
				// create a rectangle, current location and size of laser
				var locLaser = locEnemyLasers [iLaser];
				rectLaser.X = (int)locLaser.X;
				rectLaser.Y = (int)locLaser.Y;

				// does laser touch ship?
				if (rectLaser.Intersects (rectShip)) {
					locEnemyLasers.RemoveAt (iLaser);
					iLaser--;
					shipDamageLevel = 
						Math.Min (texShipDamage.Count - 1, shipDamageLevel + 1);
					break;
				}
			}

			// --------------------
			// enemy hit by laser?
			// --------------------

			var rectEnemy = Rectangle.Empty;

			// check all meteor instances
			for (int iEnemy = 0; iEnemy < locEnemyShips.Count; iEnemy++) {
				// create a rectangle, current location and size of enemy ship
				var locEnemy = locEnemyShips [iEnemy];
				rectEnemy = texEnemyShip.Bounds;
				rectEnemy.X = (int)locEnemy.X;
				rectEnemy.Y = (int)locEnemy.Y;

				// check all laser instances
				for (int iLaser = 0; iLaser < locLasers.Count; iLaser++) {
					// create a rectangle, current location and size of laser
					var locLaser = locLasers [iLaser];
					rectLaser.X = (int)locLaser.X;
					rectLaser.Y = (int)locLaser.Y;

					// does laser touch enemy ship?
					if (rectLaser.Intersects (rectEnemy)) {
						locEnemyShips.RemoveAt (iEnemy);
						iEnemy--;
						locLasers.RemoveAt (iLaser);
						iLaser--;
						break;
					}
				}
			}
		}

		// NEW: remove unused objects
		protected void DoHousekeeping () {
			// our lasers
			var rect = texLaser.Bounds;
			for (int i = 0; i < locLasers.Count; i++) {
				rect.X = (int)locLasers [i].X;
				rect.Y = (int)locLasers [i].Y;
				if (!rectViewBounds.Intersects (rect)) {
					locLasers.RemoveAt (i);
					i--;
				}
			}

			// enemy lasers
			rect = texEnemyLaser.Bounds;
			for (int i = 0; i < locEnemyLasers.Count; i++) {
				rect.X = (int)locEnemyLasers [i].X;
				rect.Y = (int)locEnemyLasers [i].Y;
				if (!rectViewBounds.Intersects (rect)) {
					locEnemyLasers.RemoveAt (i);
					i--;
				}
			}

			// enemy ships
			rect = texEnemyShip.Bounds;
			for (int i = 0; i < locEnemyShips.Count; i++) {
				rect.X = (int)locEnemyShips [i].X;
				rect.Y = (int)locEnemyShips [i].Y;
				if (!rectViewBounds.Intersects (rect)) {
					locEnemyShips.RemoveAt (i);
					i--;
				}
			}

			// meteors
			for (int i = 0; i < locMeteors.Count; i++) {
				rect = texMeteors[(int)locMeteors[i].Z].Bounds;
				rect.X = (int)locMeteors [i].X;
				rect.Y = (int)locMeteors [i].Y;
				if (!rectViewBounds.Intersects (rect)) {
					locMeteors.RemoveAt (i);
					i--;
				}
			}
		}

        protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
			spriteBatch.Begin ();

			// draw our space image at current location, filling screen
			var loc = locStars;
			while (loc.Y < rectViewBounds.Bottom) {
				loc.X = 0.0f;
				while (loc.X < rectViewBounds.Right) {
					spriteBatch.Draw (texStars, loc, Color.White);
					loc.X += texStars.Bounds.Width;
				}
				loc.Y += texStars.Bounds.Height;
			}

			// draw lasers
			for (int i = 0; i < locLasers.Count; i++) {
				spriteBatch.Draw (texLaser, locLasers [i], Color.White);
			}

			// draw our spaceship image at current location
			spriteBatch.Draw (texShip, locShip, Color.White);

			// draw ship damage, if any
			if (shipDamageLevel >= 0) {
				spriteBatch.Draw (
					texShipDamage[shipDamageLevel], 
					locShip, 
					Color.White);
			}

			// draw meteors
			for (int i = 0; i < locMeteors.Count; i++) {
				var locMeteor = locMeteors [i];
				var iTexture = (int)locMeteor.Z;
				spriteBatch.Draw (
					texMeteors[iTexture], 
					new Vector2(locMeteor.X, locMeteor.Y), 
					Color.White);
			}

			// draw enemy lasers
			for (int i = 0; i < locEnemyLasers.Count; i++) {
				spriteBatch.Draw (texEnemyLaser, locEnemyLasers[i], Color.White);
			}

			// draw enemy ships
			for (int i = 0; i < locEnemyShips.Count; i++) {
				loc = new Vector2 (locEnemyShips [i].X, locEnemyShips [i].Y);
				spriteBatch.Draw (texEnemyShip, loc, Color.White);
			}

			spriteBatch.End ();
			base.Draw (gameTime);
		}
    }
}
