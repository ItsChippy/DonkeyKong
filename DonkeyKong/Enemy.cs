using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    public class Enemy : BaseGameObject
    {
        float speed;

        public Enemy(Texture2D texture, Vector2 position) : base (texture, position)
        {
            speed = GenerateRandomSpeed();
        }

        private float GenerateRandomSpeed()
        {
            Random randomSpeed = new Random();
            return (float)randomSpeed.Next(80, 110);
        }
    }
}
