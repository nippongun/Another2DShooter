﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Another2DShooter
{
    class ParallaxingBackground
    {
        Texture2D texture;

        Vector2[] positions;

        int bgHeight;
        int bgWidth;

        int speed;

        public void Initialize(ContentManager content, String texturePath,int screenWidth,int screenHeight, int speed) {
            bgHeight = screenHeight;
            bgWidth = screenWidth;

            texture = content.Load<Texture2D>(texturePath);

            this.speed = speed;

            positions = new Vector2[screenWidth / texture.Width + 1];

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector2(i * texture.Width, 0);
            }
        }

        public void Update(GameTime gameTime) {
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i].X += speed;

                if (speed <= 0)
                {

                    if (positions[i].X <= -texture.Width)
                    {
                        positions[i].X = texture.Width * (positions.Length - 1);
                    }
                }
                else if(positions[i].X >= texture.Width * (positions.Length -1))
                {
                    positions[i].X -= texture.Width;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < positions.Length; i++)
            {
                Rectangle rectBg = new Rectangle((int)positions[i].X, (int)positions[i].Y, bgWidth, bgHeight);
                spriteBatch.Draw(texture, rectBg, Color.White);
            }
        }


    }
}