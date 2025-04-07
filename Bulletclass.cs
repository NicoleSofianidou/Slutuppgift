using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slutuppgift
{
    public class Bulletclass
    {
        private Texture2D texture;
        public Vector2 bulletPosition;
        private Vector2 velocity;
        public bool IsActive { get; set; }

        public Bulletclass(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            this.texture = texture;
            this.bulletPosition = position;
            this.velocity = velocity;
            IsActive = true;
        }

        public void Update()
        {
            bulletPosition += velocity;
            if (bulletPosition.Y < 0)
            {
                IsActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bulletPosition, Color.White);
        }
    }
}
