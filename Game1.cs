using System;
using MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Slutuppgift;

namespace MonoGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // SPACESHIP
        Texture2D spaceshipTexture;
        Vector2 spaceshipPosition;
        float spaceshipSpeed;

        // ALIEN
        Texture2D alienTexture;
        Vector2 alienPosition;
        float alienSpeed;
       
        //BULLET
        Texture2D bulletTexture;
    

        //SHOOTING TIMER
        float timeSinceLastShot = 0f;
        float shootCooldown = 0.5f;
        
        List<BulletList> Multiplebullets = new List<BulletList>();

        //BACKGROUND
        Texture2D backgroundTexture;
        float backgroundPositionY = 0f;
        float backgroundScrollSpeed = 2f;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set the screen size
            _graphics.PreferredBackBufferWidth = 1200;  // Width of the window
            _graphics.PreferredBackBufferHeight = 700; // Height of the window
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Console.WriteLine("Initialized called");

            //ALIEN
            alienPosition = new Vector2(0, 0);
            alienSpeed = 2f;

            //SPACESHIP
            spaceshipPosition = new Vector2(0,300);
            spaceshipSpeed = 3f;

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            spaceshipTexture = Content.Load<Texture2D>("SpaceShip");
            alienTexture = Content.Load<Texture2D>("Alien");
            bulletTexture = Content.Load<Texture2D>("Smallbullet");
            backgroundTexture = Content.Load<Texture2D>("Background");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            try
            {
                KeyboardState kState = Keyboard.GetState();

                // SPACESHIP
                if (kState.IsKeyDown(Keys.Left))
                {
                    spaceshipPosition.X -= spaceshipSpeed;
                }
                if (kState.IsKeyDown(Keys.Right))
                {
                    spaceshipPosition.X += spaceshipSpeed;
                }
                if (spaceshipPosition.X + spaceshipTexture.Width > _graphics.PreferredBackBufferWidth)
                {
                    spaceshipPosition.X = _graphics.PreferredBackBufferWidth - spaceshipTexture.Width;
                }
                if (spaceshipPosition.X < 0)
                {
                    spaceshipPosition.X = 0;
                }
               
                //BULLETS
                if (kState.IsKeyDown(Keys.Space) && timeSinceLastShot > shootCooldown)
                {
                    Shoot();
                    timeSinceLastShot = 0;
                }

                foreach (var bullet in Multiplebullets)
                {
                    bullet.Update();
                }

                Multiplebullets.RemoveAll(b => !b.IsActive); // Remove inactive bullets

                // ALIEN
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                if (alienPosition.X + alienTexture.Width >= _graphics.PreferredBackBufferWidth)
                {
                    alienSpeed *= -1;
                }
                if (alienPosition.X < 0)
                {
                    alienSpeed *= -1;
                }

                alienPosition.X += alienSpeed; // Update alien position based on its speed

                //Update shooting timer
                timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

                base.Update(gameTime);

                //BACKGROUND
                backgroundPositionY += backgroundScrollSpeed;
                if (backgroundPositionY >= _graphics.PreferredBackBufferHeight)
                {
                    backgroundPositionY = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in Update: {ex.Message}");
                throw;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            try
            {
                GraphicsDevice.Clear(Color.DarkBlue);

                // Draw all sprites
                _spriteBatch.Begin();

                //BACKGROUND
                Rectangle backgroundRectangle = new Rectangle(0, (int)backgroundPositionY, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

                Rectangle backgroundRectangleAbove = new Rectangle(0, (int)backgroundPositionY - _graphics.PreferredBackBufferHeight, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

                _spriteBatch.Draw(backgroundTexture, backgroundRectangle,Color.White);

                _spriteBatch.Draw(backgroundTexture, backgroundRectangleAbove, Color.White);

                //ALIEN
                _spriteBatch.Draw(alienTexture, new Vector2(alienPosition.X, 10), Color.White);

                //BULLET
                foreach (var bullet in Multiplebullets)
                {
                    bullet.Draw(_spriteBatch);
                }

                //SPACESHIP
                _spriteBatch.Draw(spaceshipTexture, new Vector2(spaceshipPosition.X, _graphics.PreferredBackBufferHeight - 1 - spaceshipTexture.Height), Color.White);
              
    
                _spriteBatch.End();


                base.Draw(gameTime);
            }

            
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in Draw: {ex.Message}");
                throw;
            }
        }

        void Shoot()
        {
            Vector2 bulletPosition = new Vector2(spaceshipPosition.X + spaceshipTexture.Width/2 - bulletTexture.Width/2, _graphics.PreferredBackBufferHeight + bulletTexture.Height - spaceshipTexture.Height - bulletTexture.Height);
            Vector2 bulletVelocity = new Vector2(0, -8);  
            Multiplebullets.Add(new BulletList(bulletTexture, bulletPosition, bulletVelocity));
        }
  

          
    }

}