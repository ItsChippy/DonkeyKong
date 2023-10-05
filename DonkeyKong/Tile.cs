using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    public class Tile : BaseGameObject
    {
        public bool isEmpty;
        public bool isLadder;
        public Tile(Texture2D texture, Vector2 position, bool isEmpty, bool isLadder) : base(texture, position)
        {
            this.isEmpty = isEmpty;
            this.isLadder = isLadder;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
