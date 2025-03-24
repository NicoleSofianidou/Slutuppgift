using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame
{
    public class TestClass
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public bool IsActive;  // Om kulan är aktiv
        private Texture2D texture;

        public TestClass(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            this.texture = texture;
            Position = position;
            Velocity = velocity;
            IsActive = true;
        }

        public void Update()
        {
            
            if (IsActive)
            {
                Position += Velocity;
                

                
                if (Position.Y < 0) 
                    IsActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                spriteBatch.Draw(texture, Position, Color.White);
            }
        }
    }
}

