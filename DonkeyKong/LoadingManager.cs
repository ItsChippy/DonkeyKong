﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.IO;

namespace DonkeyKong
{
    public class LoadingManager
    {
       public List<string> stringsFromTextFile;

       public Texture2D bridgeTileTexture;
       public Texture2D bridgeLadderTileTexture;
       public Texture2D emptyTileTexture;
       public Texture2D ladderTileTexture;
       public Texture2D playerTexture;
       public Texture2D characterSpriteSheet;

       public LoadingManager(Game game)
        {
            bridgeTileTexture = game.Content.Load<Texture2D>(@"bridge");
            bridgeLadderTileTexture = game.Content.Load<Texture2D>(@"bridgeLadder");
            emptyTileTexture = game.Content.Load<Texture2D>(@"empty");
            ladderTileTexture = game.Content.Load<Texture2D>(@"ladder");
            playerTexture = game.Content.Load<Texture2D>(@"mario");
            characterSpriteSheet = game.Content.Load<Texture2D>(@"mario-pauline");
            
            FillListFromTextFile();
        }

        public void LoadMap(Tile[,] tileMap, int numOfRows, int numOfCols)
        {
            int tileWidth = bridgeTileTexture.Width;
            int tileHeight = bridgeTileTexture.Height;
            
            for (int row = 0; row < numOfRows; row++)
            {
                for (int col = 0; col < numOfCols; col++)
                {
                    if (stringsFromTextFile[col][row] == '-')
                    {
                        tileMap[row, col] = new Tile(emptyTileTexture, new Vector2(tileWidth * row, tileHeight * col));
                        tileMap[row, col].thisTileType = TileType.Empty;
                    }
                    else if (stringsFromTextFile[col][row] == 'X')
                    {
                        tileMap[row, col] = new Tile(bridgeTileTexture, new Vector2(tileWidth * row, tileHeight * col));
                        tileMap[row, col].thisTileType = TileType.Bridge;
                    }
                    else if (stringsFromTextFile[col][row] == 'H')
                    {
                        tileMap[row, col] = new Tile(ladderTileTexture, new Vector2(tileWidth * row, tileHeight * col));
                        tileMap[row, col].thisTileType = TileType.Ladder;
                    }
                    else if (stringsFromTextFile[col][row] == 'M')
                    {
                        tileMap[row, col] = new Tile(bridgeLadderTileTexture, new Vector2(tileWidth * row, tileHeight * col));
                        tileMap[row, col].thisTileType = TileType.BridgeLadder;
                    }
                }
            }
        }

        public Player LoadPlayer(int width)
        {
            Vector2 playerStartingPos;
            int playerStartingHeight = 680;
            playerStartingPos = new Vector2(width / 2 - playerTexture.Width / 2, playerStartingHeight);
            return new Player(playerTexture, playerStartingPos);
        }

        private void FillListFromTextFile()
        {
            stringsFromTextFile = new List<string>();

            StreamReader sr = new StreamReader("map.txt");
            while (!sr.EndOfStream)
            {
                stringsFromTextFile.Add(sr.ReadLine());
            }
            sr.Close();
        }
    }
}