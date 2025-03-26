using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
        Texture2D smallbulletTexture;
        /*
        Vector2 smallbulletPosition;
        float smallbulletSpeed;
        */
        List<BulletList> Multiplebullets = new List<BulletList>();

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

            alienPosition = new Vector2(0, 0);
            alienSpeed = 3f;

            spaceshipPosition = new Vector2(0,100);
            spaceshipSpeed = 3f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            spaceshipTexture = Content.Load<Texture2D>("SpaceShipSmall");
            alienTexture = Content.Load<Texture2D>("Alien");
            smallbulletTexture = Content.Load<Texture2D>("Smallbullet");

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

                // SHOOT BULLETS
                if (kState.IsKeyDown(Keys.Space))
                {
                    Shoot();
                }

                foreach (var smallbullet in Multiplebullets)
                {
                    smallbullet.Update();
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

                base.Update(gameTime);
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
                GraphicsDevice.Clear(Color.CornflowerBlue);

                // Draw all sprites
                _spriteBatch.Begin();
                _spriteBatch.Draw(spaceshipTexture, new Vector2(spaceshipPosition.X, _graphics.PreferredBackBufferHeight - 20 - spaceshipTexture.Height), Color.White);
                _spriteBatch.Draw(alienTexture, new Vector2(alienPosition.X, 10), Color.White);


                foreach (var smallbullet in Multiplebullets)
                {
                    smallbullet.Draw(_spriteBatch);
                }
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
            Vector2 bulletPosition = new Vector2(spaceshipPosition.X + spaceshipTexture.Width/2 - smallbulletTexture.Width/2, _graphics.PreferredBackBufferHeight - 20 - spaceshipTexture.Height - smallbulletTexture.Height);
            Vector2 bulletVelocity = new Vector2(0, -5);  // Bullets move upwards

            Multiplebullets.Add(new BulletList(smallbulletTexture, bulletPosition, bulletVelocity));
        }
    }

    public class BulletList
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        public bool IsActive { get; private set; }

        public BulletList(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
            IsActive = true;
        }

        public void Update()
        {
            position += velocity;
            if (position.Y < 0)
            {
                IsActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
