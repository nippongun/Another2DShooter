using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Another2DShooter
{
    class Enemy
    {
        public Texture2D enemyTexture;

        public Vector2 position;
        public bool active;

        public float enemyMoveSpeed;
        public int hp;
        public int damage;

        public float angle;

        public int scorepoints;

        public int width { get { return enemyTexture.Width; } }
        public int height { get{ return enemyTexture.Height; } }

        public void Initialize(Texture2D texture,Vector2 position) {
            enemyTexture = texture;
            this.position = position;
            active = true;
            damage = 10;
            hp = 10;
            enemyMoveSpeed = 8f;
        }

        public void Update(GameTime gameTime) {
            position.X -= enemyMoveSpeed;

            if (position.X <-width || hp <=0)
            {
                active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(enemyTexture, position, null, Color.White, angle, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

    }
}
