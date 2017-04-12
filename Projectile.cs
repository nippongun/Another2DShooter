using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Another2DShooter
{
    abstract class Projectile
    {
        public Texture2D laserTexture;
        public Vector2 position;

        private float projectileMoveSpeed;
        public int damage;

        public bool active;

        int range;

        public int width { get{ return laserTexture.Width; } }
        public int height { get{ return laserTexture.Height; } }

        abstract public void Intialize(Texture2D texture, Vector2 position);

        abstract public void Update(GameTime gameTime);

        abstract public void Draw(SpriteBatch spriteBatch);
    }
}
