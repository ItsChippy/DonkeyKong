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

        LoadingManager loadingManager;

        //player
        Player player;
        Vector2 playerStartingPos;

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
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 840;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            loadingManager = new LoadingManager(this);

            //storing "map levels" to determine type of tile
            List<string> stringsFromTextFile = new List<string>();
            
            StreamReader sr = new StreamReader("map.txt");
            while(!sr.EndOfStream)
            {
                stringsFromTextFile.Add(sr.ReadLine());
            }
            sr.Close();

            //loads the map and all tiles
            numOfRows = stringsFromTextFile[0].Length;
            numOfCols = stringsFromTextFile.Count;
            tileMap = new Tile[numOfRows, numOfCols];
            loadingManager.LoadMap(tileMap, stringsFromTextFile, numOfRows, numOfCols);

            player = loadingManager.LoadPlayer(Window.ClientBounds.Width);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var keys = Keyboard.GetState();

            player.Move(keys, gameTime, Window.ClientBounds.Width);
            player.UpdateRectanglePos();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            for (int row = 0; row < numOfRows; row++)
            {
                for (int col = 0; col < numOfCols; col++)
                {
                    tileMap[row, col].Draw(_spriteBatch);
                }
            }
            player.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public static TileType CheckTileType(Vector2 tilePosition)
        {
            int tileWidth = tileMap[0, 0].texture.Width;
            int tileHeight = tileMap[0, 0].texture.Height;
            return tileMap[(int)tilePosition.X / tileWidth, (int) tilePosition.Y / tileHeight].thisTileType;
        }
    }
}