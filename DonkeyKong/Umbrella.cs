using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    public class Umbrella : BaseGameObject
    {
        public bool isHit;
        Player player;
        SoundEffect sound;

        public Umbrella(Texture2D texture, Vector2 position, Player player, SoundEffect sound) :base(texture, position)
        {
            isHit = false;
            this.player = player;
            this.sound = sound;
        }

        public void Update(Game1 game1)
        {
            if (rect.Intersects(player.rect))
            {
                isHit = true;
                rect.Offset(1000, 1000);
                game1.points += 75;
                sound.Play();
            }
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            if (!isHit)
            {
                spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 2.35f, SpriteEffects.None, 0f);
            }

        }
    }
}
