using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
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
        float timer;
        bool isPlayerHit;
        bool hasLost;
        StringBuilder pointDisplay = new StringBuilder();
        protected static GameStateController instance;

        public static GameStateController Instance
        {
            get
            {
                instance ??= new GameStateController();
                return instance;
            }
        }

        public void StartMenuUpdate(KeyboardState keys, Game1 game1, GameTime gameTime)
        {
            if (keys.IsKeyDown(Keys.Space))
            {
                timer = 0;
                game1.currentState = GameState.Playing;
            }
            
            game1.donkeyKongAnimation.UpdateAnimation(gameTime);
        }

        public void PlayingUpdate(KeyboardState keys, GameTime gameTime, Player player, Enemy[] enemies, Pauline pauline, Game1 game1)
        {
            if (CheckWin(game1) || keys.IsKeyDown(Keys.F))
            {
                hasLost = false;
                game1.currentState = GameState.GameOver;
            }
            else if (game1.lives == 0 || keys.IsKeyDown(Keys.G))
            {
                hasLost = true;
                game1.currentState = GameState.GameOver;
            }

            EnemyPatrolAndCollision(game1, gameTime, player, enemies);
            player.Move(keys, gameTime, game1.playerWalkingAnimation);
            game1.playerWalkingAnimation.UpdateAnimationPosition(player.position);
            game1.playerClimbingAnimation.UpdateAnimationPosition(player.position);

            if (player.isMoving)
            {
                game1.loadingManager.playerWalkingSoundInstance.Play();
            }
            else if (!player.isMoving)
            {
                game1.loadingManager.playerWalkingSoundInstance.IsLooped = false;
            }

            for (int i = 0; i < game1.springTiles.Count; i++)
            {
                game1.springTiles[i].Update(player, game1);
            }
            for (int i = 0; i < game1.umbrellas.Length; i++)
            {
                game1.umbrellas[i].Update(game1);
            }

            pauline.Update(gameTime);
            game1.donkeyKongAnimation.UpdateAnimation(gameTime);
            pointDisplay.Clear();
            pointDisplay.Append($"Points: {game1.points}");
        }

        public void GameOverUpdate(KeyboardState keys, Tile[,] tileMap, Game1 game1, GameTime gameTime, Player player, Pauline pauline)
        {
            if(keys.IsKeyDown(Keys.Enter))
            {
                game1.Restart();
                game1.currentState = GameState.StartMenu;
            }
            if (timer >= 3)
            {
                player.position = pauline.position;
                player.position.X -= 50;
            }

        }

        
        //Game state Draw() methods

        public void StartMenuDraw(SpriteBatch spriteBatch, Tile[,] tileMap, Game1 game1)
        {
            for (int rows = 0; rows < tileMap.GetLength(0); rows++)
            {
                for (int cols = 0; cols < tileMap.GetLength(1); cols++)
                {
                    tileMap[rows, cols].Draw(spriteBatch);
                }
            }
            
            game1.startScreen.Draw(spriteBatch);

            game1.donkeyKongAnimation.Draw(spriteBatch, 2.5f, SpriteEffects.None, Color.White);
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

            for (int i = 0; i < game1.umbrellas.Length; i++)
            {
                game1.umbrellas[i].Draw(spriteBatch);
            }

            player.Draw(spriteBatch, game1.playerWalkingAnimation, game1.playerClimbingAnimation, isPlayerHit);
            player.DrawLives(spriteBatch, game1);
            spriteBatch.DrawString(game1.loadingManager.spriteFont, "Lives", Vector2.Zero, Color.White);
            spriteBatch.DrawString(game1.loadingManager.spriteFont, pointDisplay, new Vector2(0, 75), Color.White);
            pauline.Draw(spriteBatch);
            game1.donkeyKongAnimation.Draw(spriteBatch, 2.35f, SpriteEffects.None, Color.White);
        }

        public void GameOverDraw(SpriteBatch spriteBatch, Tile[,] tileMap, Player player, Pauline pauline, Game1 game1, GameTime gameTime)
        {
            
            for (int rows = 0; rows < tileMap.GetLength(0); rows++)
            {
                for (int cols = 0; cols < tileMap.GetLength(1); cols++)
                {
                    tileMap[rows, cols].Draw(spriteBatch);
                }
            }

            if (hasLost)
            {
                game1.gameOverScreen.DrawLose(spriteBatch);
            }
            else
            {
                game1.gameOverScreen.DrawWin(spriteBatch);
                spriteBatch.Draw(player.texture, player.position, null, Color.White, 0, Vector2.Zero, 2.35f, SpriteEffects.None, 0);
                pauline.Draw(spriteBatch);
                game1.donkeyKongAnimation.Draw(spriteBatch, 2.35f, SpriteEffects.FlipVertically, Color.White);
            }

        }



        //explicit methods for the state controller methods

        //Handles enemy patrolling and player collision
        private void EnemyPatrolAndCollision(Game1 game1, GameTime gameTime, Player player, Enemy[] enemies)
        {
            for (int index = 0; index < enemies.Length; index++)
            {
                enemies[index].Update(gameTime, game1.enemyAnimations[index]);
                game1.enemyAnimations[index].UpdateAnimationPosition(enemies[index].position);
                if (CheckCollision(enemies[index].rect, player.rect) && !isPlayerHit)
                {
                    game1.lives--;
                    game1.points -= 25;
                    game1.loadingManager.loseLifeSound.Play(0.2f, 0.0f, 0.0f);
                    isPlayerHit = true;
                }
            }

            //makes sure only one life gets removed if colliding with the enemy with an invulnerability timer of 3 seconds
            if (isPlayerHit)
            {
                collisionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (collisionTimer <= 0)
            {
                isPlayerHit = false;
                collisionTimer = collisionDelay;
            }
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
