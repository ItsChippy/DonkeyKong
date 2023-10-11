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
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public class Player : BaseGameObject
    {
        const float speed = 120f;
        Vector2 destination;
        Vector2 direction;
        public bool isMoving = false;
        Direction currentDirection;

        public Player(Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.texture = texture;
            this.position = position;
        }

        public void Move(KeyboardState keys, GameTime gameTime, Animation animation)
        {
            if (!isMoving)
            {
                UpdateDirectionInput(keys);
                if (keys.IsKeyDown(Keys.Left))
                {
                   MoveHorizontally(new Vector2(-1, 0));
                }
                else if (keys.IsKeyDown(Keys.Right))
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
                animation.Update(gameTime);
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
                    currentDirection = Direction.Up;
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
                    currentDirection = Direction.Down;
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

        public void UpdateDirectionInput(KeyboardState keys)
        {
            if (keys.IsKeyDown(Keys.Left))
            {
                currentDirection = Direction.Left;
            }
            else if (keys.IsKeyDown(Keys.Right))
            {
                currentDirection = Direction.Right;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Animation walkingAnimation, Animation climbingAnimation)
        {
            switch (currentDirection)
            {
                case Direction.Left:
                    walkingAnimation.Draw(spriteBatch, 2.35f, SpriteEffects.FlipHorizontally);
                    break;

                case Direction.Right:
                    walkingAnimation.Draw(spriteBatch, 2.35f, SpriteEffects.None);
                    break;

                case Direction.Up:
                    climbingAnimation.Draw(spriteBatch, 2.35f, SpriteEffects.None);
                    break;

                case Direction.Down:
                    climbingAnimation.Draw(spriteBatch, 2.35f, SpriteEffects.None);
                    break;
            }
        }
    }
}
