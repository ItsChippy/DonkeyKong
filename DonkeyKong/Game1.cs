﻿using Microsoft.Xna.Framework;
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
        Animation playerAnimation;
        Animation playerClimbingAnimation;
        int lives;

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

            numOfRows = loadingManager.stringsFromTextFile[0].Length;
            numOfCols = loadingManager.stringsFromTextFile.Count;

            tileMap = new Tile[numOfRows, numOfCols];
            loadingManager.LoadMap(tileMap, numOfRows, numOfCols);

            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = loadingManager.emptyTileTexture.Width * numOfRows;
            _graphics.ApplyChanges();

            player = loadingManager.LoadPlayer(Window.ClientBounds.Width);
            playerAnimation = new Animation(loadingManager.characterSpriteSheet, 17, 17, 5, 0, 0);
            playerClimbingAnimation = new Animation(loadingManager.characterSpriteSheet, 17, 17, 1, 153, 0);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var keys = Keyboard.GetState();

            player.Move(keys, gameTime, Window.ClientBounds.Width, playerAnimation);
            player.UpdateRectanglePos();
            playerAnimation.UpdatePosition(player.position);
            playerClimbingAnimation.UpdatePosition(player.position);
            
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
            player.Draw(_spriteBatch, playerAnimation, playerClimbingAnimation);

            _spriteBatch.End();
            base.Draw(gameTime);
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