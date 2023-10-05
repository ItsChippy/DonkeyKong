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
            Vector2 newDestination = GetNewDestination(direction);
            TileType tileAtNewDestination = Game1.CheckTileType(newDestination);
            TileType tileBelowPlayer = Game1.CheckTileType(GetNewDestination(new Vector2(0, 1)));

            if (tileBelowPlayer == TileType.Bridge || tileBelowPlayer == TileType.BridgeLadder)
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
            Vector2 newDestination = GetNewDestination(direction);
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
            Vector2 newDestination = GetNewDestination(direction);
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

        public Vector2 GetNewDestination(Vector2 direction)
        {
            int tileWidth = 40;
            return position + direction * tileWidth;
        }

        public void Draw(SpriteBatch spriteBatch, KeyboardState keys)
        {
            SpriteEffects textureDirection = SpriteEffects.None;
            
            if (keys.IsKeyDown(Keys.Left))
            {
                textureDirection = SpriteEffects.FlipHorizontally;
            }
            else if (keys.IsKeyDown(Keys.Right))
            {
                textureDirection = SpriteEffects.None;
            }

            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, 2f, textureDirection, 0);
        }
    }
}
