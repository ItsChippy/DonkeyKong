using System;
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
        public Texture2D springTileTexture;
        public Texture2D playerTexture;
        public Texture2D enemyTexture;
        public Texture2D characterSpriteSheet;
        public Texture2D enemySpriteSheet;

       public LoadingManager(Game game)
        {
            bridgeTileTexture = game.Content.Load<Texture2D>(@"bridge");
            bridgeLadderTileTexture = game.Content.Load<Texture2D>(@"bridgeLadder");
            emptyTileTexture = game.Content.Load<Texture2D>(@"empty");
            ladderTileTexture = game.Content.Load<Texture2D>(@"ladder");
            springTileTexture = game.Content.Load<Texture2D>(@"spring");
            playerTexture = game.Content.Load<Texture2D>(@"mario");
            characterSpriteSheet = game.Content.Load<Texture2D>(@"mario-pauline");
            enemyTexture = game.Content.Load<Texture2D>(@"enemy");
            enemySpriteSheet = game.Content.Load<Texture2D>(@"enemy_spritesheet");
            FillListFromTextFile();
        }

        public void LoadMap(Tile[,] tileMap, int numOfRows, int numOfCols)
        {
            int tileWidth = emptyTileTexture.Width;
            int tileHeight = emptyTileTexture.Height;
            
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
                    else if (stringsFromTextFile[col][row] == 'Z')
                    {
                        tileMap[row, col] = new Tile(springTileTexture, new Vector2(tileWidth * row, tileHeight * col));
                        tileMap[row, col].thisTileType = TileType.Spring;
                    }
                    else if (stringsFromTextFile[col][row] == 'W')
                    {
                        tileMap[row, col] = new Tile(emptyTileTexture, new Vector2(tileWidth * row, tileHeight * col));
                        tileMap[row, col].thisTileType = TileType.Wall;
                    }
                }
            }
        }

        public Player LoadPlayer(Vector2 startPos)
        {
            return new Player(playerTexture, startPos);
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

        public Enemy LoadEnemy(Vector2 startPos)
        {
            return new Enemy(enemyTexture, startPos);
        }
    }
}
