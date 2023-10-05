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
    public class Player : BaseGameObject
    {
        const float speed = 100f;
        Vector2 destination;
        Vector2 direction;
        bool isMoving = false;


        public Player(Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.texture = texture;
            this.position = position;
        }

        public void Move(KeyboardState keys, GameTime gameTime, int screenWidth)
        {
            if (!isMoving)
            {
                if (keys.IsKeyDown(Keys.Left) && position.X > 0)
                {
                    ChangeDirectionHorizontal(new Vector2(-1, 0));
                }
                else if (keys.IsKeyDown(Keys.Right) && position.X + texture.Width < screenWidth)
                {
                    ChangeDirectionHorizontal(new Vector2(1, 0));
                }
                else if (keys.IsKeyDown(Keys.Up))
                {
                    ChangeDirectionVertical(new Vector2(0, -1));
                }
                else if (keys.IsKeyDown (Keys.Down))
                {
                    ChangeDirectionVertical(new Vector2(0, 1));
                }

            }
            else
            {
                position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Vector2.Distance(position, destination) < 1)
                {
                    position = destination;
                    isMoving = false;
                }
            }
        }

        public void ChangeDirectionHorizontal(Vector2 inputDirection)
        {
            direction = inputDirection;
            int tileWidth = 40;
            Vector2 newDestination = position + direction * tileWidth;
            if (Game1.CheckIfEmpty(newDestination))
            {
                destination = newDestination;
                isMoving = true;
            }
        }

        public void ChangeDirectionVertical(Vector2 inputDirection) 
        {
            direction = inputDirection;
            int tileWidth = 40;
            Vector2 newDestination = position + direction * tileWidth;

            if (Game1.CheckIfLadder(newDestination))
            {
                destination = newDestination;
                isMoving = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
