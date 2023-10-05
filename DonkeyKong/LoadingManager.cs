using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace DonkeyKong
{
    public class LoadingManager
    {
       public Texture2D bridgeTileTexture;
       public Texture2D bridgeLadderTileTexture;
       public Texture2D emptyTileTexture;
       public Texture2D ladderTileTexture;
       public Texture2D playerTexture;

       public LoadingManager(Game game)
        {
            bridgeTileTexture = game.Content.Load<Texture2D>(@"bridge");
            bridgeLadderTileTexture = game.Content.Load<Texture2D>(@"bridgeLadder");
            emptyTileTexture = game.Content.Load<Texture2D>(@"empty");
            ladderTileTexture = game.Content.Load<Texture2D>(@"ladder");
            playerTexture = game.Content.Load<Texture2D>(@"supermariofront");
        }

        public void LoadMap(Tile[,] tileMap, List<string> strings, int numOfRows, int numOfCols)
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

        public Player LoadPlayer(int width)
        {
            Vector2 playerStartingPos;
            int playerStartingHeight = 680;
            playerStartingPos = new Vector2(width / 2 - playerTexture.Width / 2, playerStartingHeight);
            return new Player(playerTexture, playerStartingPos);
        }

    }
}
