using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    public class Pauline : BaseGameObject
    {
        bool isFlipped;
        const float eventDelay = 5f;
        float remainingTime;

        public Pauline(Texture2D texture, Vector2 position) : base(texture, position)
        {
            isFlipped = false;
            remainingTime = eventDelay;
        }

        public void Update(GameTime gameTime)
        {
            remainingTime -= (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (remainingTime <= 0)
            {
                FlipDirection();
                remainingTime = eventDelay;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffect = SpriteEffects.None;

            if (isFlipped)
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
            }
            else if (!isFlipped)
            {
                spriteEffect = SpriteEffects.None;
            }

            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, 1, spriteEffect, 0);
        }

        private void FlipDirection()
        {
            if (isFlipped)
            {
                isFlipped = false;
            }
            else if (!isFlipped)
            {
                isFlipped = true;
            }
        }
    }
}
