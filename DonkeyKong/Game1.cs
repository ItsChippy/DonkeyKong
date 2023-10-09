using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace DonkeyKong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private LoadingManager loadingManager;

        //player
        Player player;
        Animation playerWalkingAnimation;
        Animation playerClimbingAnimation;
        int lives;

        //enemy
        Enemy[] enemies;
        Animation enemyAnimation;
        int numOfEnemies;

        //tiles and map (tile array)
        int numOfRows;
        int numOfCols;
        static Tile[,] tileMap;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            loadingManager = new LoadingManager(this);

            //loading map and storing array dimensions
            numOfRows = loadingManager.stringsFromTextFile[0].Length;
            numOfCols = loadingManager.stringsFromTextFile.Count;

            tileMap = new Tile[numOfRows, numOfCols];
            loadingManager.LoadMap(tileMap, numOfRows, numOfCols);

            //setting screen size
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = loadingManager.emptyTileTexture.Width * numOfRows;
            _graphics.ApplyChanges();

            //loading player and player animations
            player = loadingManager.LoadPlayer(tileMap[numOfRows / 2, numOfCols - 2].position);
            playerWalkingAnimation = new Animation(loadingManager.characterSpriteSheet, 17, 17, 5, 0, 0);
            playerClimbingAnimation = new Animation(loadingManager.characterSpriteSheet, 17, 17, 1, 153, 0);

            //loading enemy and enemy animations
            numOfEnemies = 4;
            enemies = new Enemy[numOfEnemies];
            FillEnemyArray();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var keys = Keyboard.GetState();

            player.Move(keys, gameTime, Window.ClientBounds.Width, playerWalkingAnimation);
            player.UpdateRectanglePos();
            playerWalkingAnimation.UpdatePosition(player.position);
            playerClimbingAnimation.UpdatePosition(player.position);
            
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Update(gameTime, enemyAnimation);
            }
            Debug.WriteLine($"enemy {0}: {enemies[0].position}");
            Debug.WriteLine($"{enemies[0].nextTile} {enemies[0].direction}");
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            var keys = Keyboard.GetState();
            
            _spriteBatch.Begin();

            for (int row = 0; row < numOfRows; row++)
            {
                for (int col = 0; col < numOfCols; col++)
                {
                    tileMap[row, col].Draw(_spriteBatch);
                }
            }
            player.Draw(_spriteBatch, playerWalkingAnimation, playerClimbingAnimation);
            
            for (int i = 0; i < enemies.Length; i++) 
            {
                enemies[i].Draw(_spriteBatch, enemyAnimation);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        protected void FillEnemyArray()
        {
            int platformSpawningDifference = 2;
            Vector2 enemySpawnPos;
            for (int index = 0; index < enemies.Length;index++)
            {
                enemySpawnPos = tileMap[numOfRows / 2, numOfCols - platformSpawningDifference].position;
                enemies[index] = loadingManager.LoadEnemy(enemySpawnPos);
                platformSpawningDifference += 3;
            }
        }

        //returns the type of tile at the position of the Vector parameter
        public static TileType CheckTileType(Vector2 tilePosition)
        {
            int tileWidth = tileMap[0, 0].texture.Width;
            int tileHeight = tileMap[0, 0].texture.Height;
            return tileMap[(int)tilePosition.X / tileWidth, (int) tilePosition.Y / tileHeight].thisTileType;
        }

    }
}