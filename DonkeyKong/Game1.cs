using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace DonkeyKong
{
    public enum GameState
    {
        StartMenu,
        Playing,
        GameOver
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public LoadingManager loadingManager;
        public StartScreen startScreen;

        public GameOverScreen gameOverScreen;

        public GameState currentState;

        //player
        Player player;
        public int lives;
        public int points;
        public Animation playerWalkingAnimation;
        public Animation playerClimbingAnimation;

        //enemy
        Enemy[] enemies;
        public Animation[] enemyAnimations;
        int numOfEnemies;

        //pauline (objective)
        Pauline pauline;

        //collectibles
        public Umbrella[] umbrellas;

        //donkey kong
        public Animation donkeyKongAnimation;

        //tiles and map (tile array)
        static int numOfRows;
        static int numOfCols;
        static Tile[,] tileMap;

        public List<SpringTile> springTiles;

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
            currentState = GameState.StartMenu;

            //map and storing array dimensions
            numOfRows = loadingManager.stringsFromTextFile[0].Length;
            numOfCols = loadingManager.stringsFromTextFile.Count;

            tileMap = new Tile[numOfRows, numOfCols];

            springTiles = new List<SpringTile>();

            loadingManager.LoadMap(tileMap, springTiles, numOfRows, numOfCols);

            //setting screen size
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.PreferredBackBufferWidth = loadingManager.emptyTileTexture.Width * numOfRows;
            _graphics.ApplyChanges();

            //start screen
            Texture2D startScreenTexture = loadingManager.startScreenTexture;
            Vector2 startScreenTexturePos = new Vector2(Window.ClientBounds.Width / 2 - startScreenTexture.Width / 2,
                                                        Window.ClientBounds.Height / 2 - startScreenTexture.Height / 2);
            startScreen = new StartScreen(startScreenTexture, loadingManager.spriteFont, startScreenTexturePos, Window.ClientBounds.Width);

            //game over screen
            Texture2D gameWinScreenTexture = loadingManager.gameWinScreenTexture;
            Vector2 gameWinScreenTexturePos = new Vector2(Window.ClientBounds.Width / 2 - gameWinScreenTexture.Width / 2,
                                                          Window.ClientBounds.Height / 2 - gameWinScreenTexture.Height / 2);
            
            Texture2D gameLoseScreenTexture = loadingManager.gameLoseScreenTexture;
            Vector2 gameLoseScreenTexturePos = new Vector2(Window.ClientBounds.Width / 2 - gameLoseScreenTexture.Width / 2,
                                                          Window.ClientBounds.Height / 2 - gameLoseScreenTexture.Height / 2);
            gameOverScreen = new GameOverScreen(gameWinScreenTexture, gameWinScreenTexturePos, gameLoseScreenTexture, gameLoseScreenTexturePos, loadingManager.spriteFont, this, Window.ClientBounds.Width);

            //player and player animations
            lives = 3;
            int firstPlatform = numOfCols - 2;
            player = loadingManager.LoadPlayer(tileMap[1, firstPlatform].position);
            playerWalkingAnimation = new Animation(loadingManager.characterSpriteSheet, 17, 17, 3);
            playerClimbingAnimation = new Animation(loadingManager.characterSpriteSheet, 17, 17, 1, 100, 153);

            //pauline
            int lastPlatform = numOfCols - 17;
            pauline = loadingManager.LoadPauline(tileMap[numOfRows / 2, lastPlatform].position);

            //umbrellas
            FillUmbrellaArray();

            //enemy and enemy animations
            numOfEnemies = 4;
            enemies = new Enemy[numOfEnemies];
            enemyAnimations = new Animation[numOfEnemies];
            FillEnemyArray();

            //donkey kong
            donkeyKongAnimation = new Animation(loadingManager.donkeyKongSpriteSheet, 46, 32, 3, 500);
            int donkeyKongPlatform = numOfCols - 15;
            donkeyKongAnimation.UpdateAnimationPosition(tileMap[numOfRows / 2 - 1, donkeyKongPlatform].position);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var keys = Keyboard.GetState();

            switch (currentState)
            {
                case GameState.StartMenu:

                    GameStateController.Instance.StartMenuUpdate(keys, this, gameTime);
                    break;

                case GameState.Playing:

                    GameStateController.Instance.PlayingUpdate(keys, gameTime, player, enemies, pauline, this);
                    break;

                case GameState.GameOver:

                    GameStateController.Instance.GameOverUpdate(keys, this, gameTime, player, pauline);
                    break;
            }
            base.Update(gameTime);
            Debug.WriteLine(currentState.ToString());
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            var keys = Keyboard.GetState();
            
            _spriteBatch.Begin();

            switch(currentState)
            {
                case GameState.StartMenu:

                    GameStateController.Instance.StartMenuDraw(_spriteBatch, tileMap, this);
                    break;

                case GameState.Playing:

                    GameStateController.Instance.PlayingDraw(_spriteBatch, tileMap, player, enemies, pauline, this);
                    break;

                case GameState.GameOver:

                    GameStateController.Instance.GameOverDraw(_spriteBatch, tileMap, player, pauline, this, gameTime);
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
                enemyAnimations[index] = new Animation(loadingManager.enemySpriteSheet, 17, 16, 2, 200);
                platformSpawningDifference += 3;
            }
        }

        protected void FillUmbrellaArray()
        {
            umbrellas = new Umbrella[]
            {
                new Umbrella(loadingManager.umbrellaTexture, tileMap[numOfRows - 4, numOfCols - 2].position, player, loadingManager.pickupSound),
                new Umbrella(loadingManager.umbrellaTexture, tileMap[numOfRows - 7, numOfCols - 5].position, player, loadingManager.pickupSound),
                new Umbrella(loadingManager.umbrellaTexture, tileMap[numOfRows - 14, numOfCols - 8].position, player, loadingManager.pickupSound),
                new Umbrella(loadingManager.umbrellaTexture, tileMap[numOfRows - 13, numOfCols - 11].position, player , loadingManager.pickupSound),
                new Umbrella(loadingManager.umbrellaTexture, tileMap[numOfRows - 19, numOfCols - 14].position, player, loadingManager.pickupSound)
            };
        }

        //returns the type of tile at the position of the Vector parameter
        public static TileType CheckTileType(Vector2 tilePosition)
        {
            int tileWidth = tileMap[0, 0].texture.Width;
            int tileHeight = tileMap[0, 0].texture.Height;
            return tileMap[(int)tilePosition.X / tileWidth, (int) tilePosition.Y / tileHeight].thisTileType;
        }

        public void Restart()
        {
            LoadContent();
        }
    }
}