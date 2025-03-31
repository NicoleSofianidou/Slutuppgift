using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slutuppgift
{
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
