using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    public class StartScreen
    {
        Texture2D texture;
        Vector2 position;

        SpriteFont font;
        string text;
        Vector2 textPosition;

        public StartScreen(Texture2D texture, SpriteFont font, Vector2 position, int screenWidth)
        {
            this.texture = texture;
            this.font = font;
            this.position = position;
            text = "Press Space to play...";

            textPosition = new Vector2(screenWidth / 2, 550);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 textMiddlePoint = font.MeasureString(text) / 2;

            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.DrawString(font, text, textPosition, Color.White, 0, textMiddlePoint, 1.5f, SpriteEffects.None, 0f);
        }
    }
}
