using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    internal class TileManager
    {
        int numOfRows;
        int numOfCols;
        Tile[,] tileMap;

        public TileManager(int numOfRows, int numOfCols)
        {
            this.numOfRows = numOfRows;
            this.numOfCols = numOfCols;
            tileMap = new Tile[numOfRows, numOfCols];
        }

        public void fillTiles(List<string> strings)
        {

        }
    }
}
