using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Another2DShooter
{
    class Player
    {
        //This represents the player
        public Texture2D playerTexture;

        //The player's position relative to 0,0 (upper left corner)
        public Vector2 position;

        public Vector2 rotation;

        public bool active;

        //Player's health points
        public int hp;

        public int width { get { return playerTexture.Width; } }
        public int height { get { return playerTexture.Height; } }

        public float angle = 0f;
        public void Initialize(Texture2D texture, Vector2 position) {
            playerTexture = texture;
            this.position = position;
            active = true;
            hp = 100;
        }

        public void Update() {

        }

        public void Draw(SpriteBatch spriteBatch) {
            //One of the overload; This has the parameters of a standard 2D player
            //spriteBatch.Draw(texture,position,rectangle,color,rotation,origin,scale,effects,layerDepth)
            spriteBatch.Draw(playerTexture, position, null, Color.White, angle, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

    }
}
