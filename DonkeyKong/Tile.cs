using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    public enum TileType
    {
        Bridge,
        BridgeLadder,
        Empty,
        Ladder
    }
    public class Tile : BaseGameObject
    {
        public TileType thisTileType;
        public Tile(Texture2D texture, Vector2 position) : base(texture, position)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
