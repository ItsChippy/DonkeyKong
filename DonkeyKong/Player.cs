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
                   MoveHorizontally(new Vector2(-1, 0));
                }
                else if (keys.IsKeyDown(Keys.Right) && position.X + texture.Width < screenWidth)
                {
                    MoveHorizontally(new Vector2(1, 0));
                }
                else if (keys.IsKeyDown(Keys.Up))
                {
                    MoveUp(new Vector2(0, -1));
                }
                else if (keys.IsKeyDown (Keys.Down))
                {
                    MoveDown(new Vector2(0, 1));
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

        public void MoveHorizontally(Vector2 inputDirection)
        {
            direction = inputDirection;
            int tileWidth = 40;
            Vector2 newDestination = position + direction * tileWidth;
            TileType tileAtNewDestination = Game1.CheckTileType(newDestination);

            if (Game1.CheckTileType(position) == TileType.Ladder || Game1.CheckTileType(position) == TileType.Empty)
            {
                if (tileAtNewDestination == TileType.Empty || tileAtNewDestination == TileType.Ladder)
                {
                    destination = newDestination;
                    isMoving = true;
                }
            }
        }

        public void MoveUp(Vector2 inputDirection) 
        {
            direction = inputDirection;
            int tileWidth = 40;
            Vector2 newDestination = position + direction * tileWidth;
            TileType tileAtNewDestination = Game1.CheckTileType(newDestination);

            if (Game1.CheckTileType(position) == TileType.Ladder || Game1.CheckTileType(position) == TileType.BridgeLadder)
            {
                if (tileAtNewDestination != TileType.Bridge)
                {
                    destination = newDestination;
                    isMoving = true;
                }
            }
        }

        public void MoveDown(Vector2 inputDirection)
        {
            direction = inputDirection;
            int tileWidth = 40;
            Vector2 newDestination = position + direction * tileWidth;
            TileType tileAtNewDestination = Game1.CheckTileType(newDestination);

            if (Game1.CheckTileType(position) != TileType.Bridge)
            {
                if (tileAtNewDestination == TileType.Ladder || tileAtNewDestination == TileType.BridgeLadder)
                {
                    destination = newDestination;
                    isMoving = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
