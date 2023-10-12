using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace DonkeyKong
{
    enum GameState
    {
        StartMenu,
        Playing,
        GameOver
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private LoadingManager loadingManager;
        GameState currentState;
        
        //player
        Player player;
        public int lives;
        public Animation playerWalkingAnimation;
        public Animation playerClimbingAnimation;

        //enemy
        Enemy[] enemies;
        public Animation[] enemyAnimations;
        int numOfEnemies;

        //tiles and map (tile array)
        static int numOfRows;
        static int numOfCols;
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
            currentState = GameState.Playing;

            //map and storing array dimensions
            numOfRows = loadingManager.stringsFromTextFile[0].Length;
            numOfCols = loadingManager.stringsFromTextFile.Count;

            tileMap = new Tile[numOfRows, numOfCols];
            loadingManager.LoadMap(tileMap, numOfRows, numOfCols);

            //setting screen size
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = loadingManager.emptyTileTexture.Width * numOfRows;
            _graphics.ApplyChanges();

            //player and player animations
            lives = 3;
            int firstPlatform = numOfCols - 2;
            player = loadingManager.LoadPlayer(tileMap[1, firstPlatform].position);
            playerWalkingAnimation = new Animation(loadingManager.characterSpriteSheet, 17, 17, 5);
            playerClimbingAnimation = new Animation(loadingManager.characterSpriteSheet, 17, 17, 1, 100, 100, 153);

            //enemy and enemy animations
            numOfEnemies = 4;
            enemies = new Enemy[numOfEnemies];
            enemyAnimations = new Animation[numOfEnemies];
            FillEnemyArray();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var keys = Keyboard.GetState();

            if (lives == 0)
            {
                currentState = GameState.GameOver;
            }

            switch (currentState)
            {
                case GameState.StartMenu:

                    break;

                case GameState.Playing:

                    GameStateController.Instance.PlayingUpdate(keys, gameTime, player, enemies, this);
                    break;

                case GameState.GameOver:

                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            var keys = Keyboard.GetState();
            
            _spriteBatch.Begin();

            switch(currentState)
            {
                case GameState.StartMenu:

                    break;

                case GameState.Playing:

                    GameStateController.Instance.PlayingDraw(_spriteBatch, tileMap, player, enemies, this);
                    break;

                case GameState.GameOver:

                    break;
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
                enemyAnimations[index] = new Animation(loadingManager.enemySpriteSheet, 17, 16, 2, 200, 200);
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