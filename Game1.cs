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


        // UFO
        Texture2D ufoTexture;
        Vector2 ufoPosition;
        float ufoSpeed;
       

        //BULLET
        Texture2D bulletTexture;

        //Shooting
        float timeSinceLastShot = 0f;
        float shootCooldown = 0.5f;
        List<Bulletclass> Multiplebullets = new List<Bulletclass>();


        //BACKGROUND
        Texture2D backgroundTexture;
        float backgroundPositionY = 0f;
        float backgroundScrollSpeed = 2f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;


            //Setting the screen size
            _graphics.PreferredBackBufferWidth = 1400; 
            _graphics.PreferredBackBufferHeight = 800;

            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {

            //UFO
            ufoPosition = new Vector2(0, 0);
            ufoSpeed = 2f;


            //SPACESHIP
            spaceshipPosition = new Vector2(0,300);
            spaceshipSpeed = 3f;

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            //Load game textures/sprites
            spaceshipTexture = Content.Load<Texture2D>("SpaceShip");

            ufoTexture = Content.Load<Texture2D>("UFO");

            bulletTexture = Content.Load<Texture2D>("Smallbullet");

            backgroundTexture = Content.Load<Texture2D>("Background");
        }

        protected override void Update(GameTime gameTime)
        {
            try
            {
                KeyboardState kState = Keyboard.GetState();


                // SPACESHIP

                //If left arrow key is pressed, move the spaceship left
                if (kState.IsKeyDown(Keys.Left))
                {
                    spaceshipPosition.X -= spaceshipSpeed;
                }
                //If right arrow key is pressed, move the spaceship right
                if (kState.IsKeyDown(Keys.Right))
                {
                    spaceshipPosition.X += spaceshipSpeed;
                }
                //If spaceship is at the right edge of the screen, it can't move more to the rigth
                if (spaceshipPosition.X + spaceshipTexture.Width > _graphics.PreferredBackBufferWidth)
                {
                    spaceshipPosition.X = _graphics.PreferredBackBufferWidth - spaceshipTexture.Width;
                }
                //If spaceship is at the left edge of the screen, it can't move more to the left
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

                
                // Collision detection between bullets and UFO
                Rectangle ufoRectangle = new Rectangle((int)ufoPosition.X, 10, ufoTexture.Width, ufoTexture.Height); // Create bounding box for UFO

                foreach (var bullet in Multiplebullets)
                {
                    Rectangle bulletRectangle = new Rectangle((int)bullet.bulletPosition.X, (int)bullet.bulletPosition.Y, bulletTexture.Width, bulletTexture.Height); // Create bounding box for bullet

                    if (bulletRectangle.Intersects(ufoRectangle))
                    {
                        // Handle collision and set the ufo to its initial position
                        bullet.IsActive = false; // Mark bullet as inactive
                        ufoPosition.X = 0;
                    }
                }


                Multiplebullets.RemoveAll(b => !b.IsActive); // Remove inactive bullets

                // UFO

                ufoPosition.X += ufoSpeed; // Update ufo position based on its speed

                //When ufo reaches the right edge of the screen, it bounces back and moves left
                if (ufoPosition.X + ufoTexture.Width >= _graphics.PreferredBackBufferWidth)
                {
                    ufoSpeed *= -1;
                }
                //When ufo reaches the left edge of the screen, it bounces back and moves right
                if (ufoPosition.X < 0)
                {
                    ufoSpeed *= -1;
                }

                
                
                timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds; //Update shooting timer

                base.Update(gameTime);

                //BACKGROUND
                backgroundPositionY += backgroundScrollSpeed;
                // Reset the background position when it goes off-screen
                if (backgroundPositionY >= _graphics.PreferredBackBufferHeight)
                {
                    backgroundPositionY = 0;
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();
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

                //UFO
                _spriteBatch.Draw(ufoTexture, new Vector2(ufoPosition.X, 10), Color.White);

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
            Vector2 bulletPosition = new Vector2(spaceshipPosition.X + spaceshipTexture.Width/2 - bulletTexture.Width/2, _graphics.PreferredBackBufferHeight + bulletTexture.Height - spaceshipTexture.Height - bulletTexture.Height); //Sets bullets position to the middle of the spaceship
            Vector2 bulletVelocity = new Vector2(0, -12); //Sets the bullets velocity/speed to move
            Multiplebullets.Add(new Bulletclass(bulletTexture, bulletPosition, bulletVelocity));
        }
  

          
    }

}