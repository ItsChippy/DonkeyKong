using Microsoft.VisualBasic.Devices;
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
    internal class Player : BaseGameObject
    {
        const float speed = 4f;

        public Player(Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.texture = texture;
            this.position = position;
        }

        public void Move(KeyboardState keys, int width)
        {
            if (keys.IsKeyDown(Keys.Left) && position.X >= 0)
            {
                position.X -= speed;
            }
            if (keys.IsKeyDown(Keys.Right) &&  position.X + texture.Width <= width)
            {
                position.X += speed;
            }
            if (keys.IsKeyDown(Keys.Up))
            {
                position.Y -= speed;
            }
            if (keys.IsKeyDown(Keys.Down))
            {
                position.Y += speed;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
