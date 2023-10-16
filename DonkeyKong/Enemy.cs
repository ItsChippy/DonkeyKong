using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    public class Enemy : BaseGameObject
    {
        float speed;
        Vector2 direction;
        TileType nextTile;

        bool isMovingRight;
        public Enemy(Texture2D texture, Vector2 position) : base (texture, position)
        {
            speed = GenerateRandomSpeed();
            direction = new Vector2(GenerateRandomDirection(), 0);
        }

        public void Update(GameTime gameTime, Animation animation)
        {
            nextTile = Game1.CheckTileType(GetNextTile(direction));
            if (nextTile == TileType.Wall)
            {
                direction.X *= -1;
                
                if(direction.X == 1)
                {
                    isMovingRight = true;
                }
                else
                {
                    isMovingRight = false;
                }
            }

            animation.Update(gameTime);
            UpdateRectanglePos();
            position.X += direction.X * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void Draw(SpriteBatch spriteBatch, Animation animation)
        {
            if (isMovingRight)
            {
                animation.Draw(spriteBatch, 2.35f, SpriteEffects.FlipHorizontally);
            }
            else
            {
                animation.Draw(spriteBatch, 2.35f, SpriteEffects.None);
            }
        }

        private Vector2 GetNextTile(Vector2 direction)
        {
            int tileWidth = 40;
            return position + direction * tileWidth;
        }
        private float GenerateRandomSpeed()
        {
            Random randomSpeed = new Random();
            return randomSpeed.Next(90, 120);
        }

        private float GenerateRandomDirection()
        {
            Random randomDirection = new Random();
            return randomDirection.Next(2) * 2 - 1;
        }
    }
}
