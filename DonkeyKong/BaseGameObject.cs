﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    internal class BaseGameObject
    {
        public Texture2D texture;
        public Vector2 position;
        public Rectangle rect;

        public BaseGameObject(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;

            rect = new Rectangle((int) position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void UpdateRectanglePos()
        {
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
        }
    }
}
