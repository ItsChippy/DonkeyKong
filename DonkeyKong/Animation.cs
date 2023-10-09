using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    public class Animation
    {
        Texture2D spriteSheet;
        Vector2 position;
        Rectangle currentSpriteRect;
        double frameTimer = 100;
        double frameInterval = 100;
        int currentFrame;
        int totalFrames;
        int spriteWidth;

        public Animation(Texture2D spriteSheet, int spriteWidth, int spriteHeight, int totalFrames, int startingPosX, int startingPosY)
        {
            this.spriteSheet = spriteSheet;
            this.totalFrames = totalFrames;
            this.spriteWidth = spriteWidth;
            currentSpriteRect = new Rectangle(startingPosX, startingPosY, spriteWidth, spriteHeight);
        }

        public void UpdatePosition(Vector2 position)
        {
            this.position = position;
        }

        public void Update(GameTime gameTime)
        {

            frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (frameTimer <= 0)
            {
                frameTimer = frameInterval;
                currentFrame++;

                if (currentFrame >= totalFrames)
                {
                    currentFrame = 0;
                }
                currentSpriteRect.X = currentFrame * spriteWidth;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch, float scale, SpriteEffects effects)
        {
            spriteBatch.Draw(spriteSheet, position, currentSpriteRect, Color.White, 0, Vector2.Zero, scale, effects, 0);
        }
    }
}
