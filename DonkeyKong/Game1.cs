using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DonkeyKong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //player
        Player player;
        Vector2 playerStartingPos;

        //tiles and map (tile array)
        int numOfRows;
        int numOfCols;
        Tile[,] tileMap;

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
            Texture2D bridgeTileTexture = Content.Load<Texture2D>(@"bridge");
            Texture2D bridgeLadderTileTexture = Content.Load<Texture2D>(@"bridgeLadder");
            Texture2D emptyTileTexture = Content.Load<Texture2D>(@"empty");
            Texture2D ladderTileTexture = Content.Load<Texture2D>(@"ladder");
            Texture2D playerTexture = Content.Load<Texture2D>(@"supermariofront");

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
            LoadMap(stringsFromTextFile, bridgeTileTexture, bridgeLadderTileTexture, emptyTileTexture, ladderTileTexture);

            LoadPlayer(playerTexture, Window.ClientBounds.Width);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var keys = Keyboard.GetState();

            player.Move(keys, Window.ClientBounds.Width);

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

        protected void LoadMap(List<string> strings, Texture2D bridgeTileTexture, Texture2D bridgeLadderTileTexture, Texture2D emptyTileTexture, Texture2D ladderTileTexture)
        {
            int tileWidth = bridgeTileTexture.Width;
            int tileHeight = bridgeTileTexture.Height;

            for (int row = 0; row < numOfRows; row++) 
            {
                for (int col = 0; col < numOfCols; col++)
                {
                    if (strings[col][row] == '-')
                    {
                        tileMap[row, col] = new Tile(emptyTileTexture, new Vector2(tileWidth * row, tileHeight * col), true, false);
                    }
                    else if (strings[col][row] == 'X')
                    {
                        tileMap[row, col] = new Tile(bridgeTileTexture, new Vector2(tileWidth * row, tileHeight * col), false, false);
                    }
                    else if (strings[col][row] == 'H')
                    {
                        tileMap[row, col] = new Tile(ladderTileTexture, new Vector2(tileWidth * row, tileHeight * col), false, true);
                    }
                    else if (strings[col][row] == 'M')
                    {
                        tileMap[row, col] = new Tile(bridgeLadderTileTexture, new Vector2(tileWidth * row, tileHeight * col), false, true);
                    }
                }
            }
        }

        protected void LoadPlayer(Texture2D playerTexture, int width)
        {
            Vector2 playerStartingPos;
            int playerStartingHeight = 680;
            playerStartingPos = new Vector2(width / 2 - playerTexture.Width / 2, playerStartingHeight);
            player = new Player(playerTexture, playerStartingPos);
        }
    }
}