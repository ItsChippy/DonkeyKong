using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    public class GameStateController
    {
        float collisionDelay = 3f;
        float collisionTimer;
        protected static GameStateController instance;

        public static GameStateController Instance
        {
            get
            {
                instance ??= new GameStateController();
                return instance;
            }
        }

        public void PlayingUpdate(KeyboardState keys, GameTime gameTime, Player player, Enemy[] enemies, Game1 game1)
        {
            player.Move(keys, gameTime, game1.playerWalkingAnimation);
            game1.playerWalkingAnimation.UpdatePosition(player.position);
            game1.playerClimbingAnimation.UpdatePosition(player.position);
            
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Update(gameTime, game1.enemyAnimations[i]);
                game1.enemyAnimations[i].UpdatePosition(enemies[i].position);

                if (CheckCollision(player.rect, enemies[i].rect))
                {
                    game1.lives--;
                    collisionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                    if (collisionTimer == collisionDelay)
                    {
                        collisionTimer = 0;
                    }
                }
            }
        }

        public void PlayingDraw(SpriteBatch spriteBatch, Tile[,] tileMap, Player player, Enemy[] enemies, Game1 game1)
        {
            for (int rows = 0; rows < tileMap.GetLength(0); rows++)
            {
                for (int cols = 0; cols < tileMap.GetLength(1); cols++)
                {
                    tileMap[rows, cols].Draw(spriteBatch);
                }
            }

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Draw(spriteBatch, game1.enemyAnimations[i]);
            }

            player.Draw(spriteBatch, game1.playerWalkingAnimation, game1.playerClimbingAnimation);
        }

        private bool CheckCollision(Rectangle rect1,  Rectangle rect2)
        {
            if (rect1.Intersects(rect2))
            {
                return true;
            }
            return false;
        }

    }
}
