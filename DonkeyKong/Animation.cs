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
        double frameTimer, frameInterval;
        int currentFrame;
        int totalFrames;
        int spriteWidth;

        public Animation(Texture2D spriteSheet, int spriteWidth, int spriteHeight, int totalFrames, double frameTimer = 100, double frameInterval = 100, int startingPosX = 0, int startingPosY = 0)
        {
            this.spriteSheet = spriteSheet;
            this.totalFrames = totalFrames;
            this.spriteWidth = spriteWidth;
            this.frameTimer = frameTimer;
            this.frameInterval = frameInterval;
            currentSpriteRect = new Rectangle(startingPosX, startingPosY, spriteWidth, spriteHeight);
        }

        //Put in the main Update method
        public void UpdatePosition(Vector2 position)
        {
            this.position = position;
        }

        //Put in the classes Update method
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

        //Put in the classes Draw method along with the specific conditions
        public void Draw(SpriteBatch spriteBatch, float scale, SpriteEffects effects)
        {
            spriteBatch.Draw(spriteSheet, position, currentSpriteRect, Color.White, 0, Vector2.Zero, scale, effects, 0);
        }
    }
}
