using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    public class SpringTile : BaseGameObject
    {
        public bool isHit = false;
        Vector2 positionAboveThisSpring;

        public SpringTile(Texture2D texture, Vector2 position, bool isHit) : base(texture, position)
        {
            this.isHit = isHit;
            positionAboveThisSpring = GetPositionAboveSpring(new Vector2(0, -1));
        }

        public void Update(Player player)
        {
            if (positionAboveThisSpring == player.position)
            {
                isHit = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isHit) 
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
        }

        private Vector2 GetPositionAboveSpring(Vector2 direction)
        {
            int tileWidth = 40;
            return position + direction * tileWidth;
        }
    }
}
