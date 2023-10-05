using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    public class Animation
    {
        Texture2D spriteSheet;
        Vector2 position;
        Rectangle[] animationFrames;
        double frameTimer = 100;
        double frameInterval = 100;

        public Animation(Texture2D spriteSheet, int spriteWidth, int spriteHeight, int frames)
        {
            this.spriteSheet = spriteSheet;
            animationFrames = new Rectangle[frames];
            FillAnimationArray(spriteWidth, spriteHeight);
        }

        private void FillAnimationArray(int spriteWidth, int spriteHeight)
        {
            Rectangle spriteRect = new Rectangle(0, 0, spriteWidth, spriteHeight);
            for (int index = 0; index < animationFrames.Length; index++) 
            {
                animationFrames[index] = spriteRect;
                spriteRect.X = index * spriteWidth;
            }

        }
    }
}
