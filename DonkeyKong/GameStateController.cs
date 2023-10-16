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

        public void PlayingUpdate(KeyboardState keys, GameTime gameTime, Player player, Enemy[] enemies, Pauline pauline, Game1 game1)
        {
            if (CheckWin(game1))
            {
                game1.currentState = GameState.GameOver;
            }

            player.Move(keys, gameTime, game1.playerWalkingAnimation);
            pauline.Update(gameTime);
            game1.playerWalkingAnimation.UpdatePosition(player.position);
            game1.playerClimbingAnimation.UpdatePosition(player.position);

            for (int i = 0; i < game1.springTiles.Count; i++)
            {
                game1.springTiles[i].Update(player);
            }

            for (int index = 0; index < enemies.Length; index++)
            {
                enemies[index].Update(gameTime, game1.enemyAnimations[index]);
                game1.enemyAnimations[index].UpdatePosition(enemies[index].position);
            }
        }

        public void PlayingDraw(SpriteBatch spriteBatch, Tile[,] tileMap, Player player, Enemy[] enemies, Pauline pauline, Game1 game1)
        {
            for (int rows = 0; rows < tileMap.GetLength(0); rows++)
            {
                for (int cols = 0; cols < tileMap.GetLength(1); cols++)
                {
                    tileMap[rows, cols].Draw(spriteBatch);
                }
            }

            for (int i = 0; i < game1.springTiles.Count; i++)
            {
                game1.springTiles[i].Draw(spriteBatch);
            }

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Draw(spriteBatch, game1.enemyAnimations[i]);
            }

            pauline.Draw(spriteBatch);
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

        private bool CheckWin(Game1 game1)
        {
            for (int i = 0; i < game1.springTiles.Count; i++)
            {
                if (!game1.springTiles[i].isHit)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
