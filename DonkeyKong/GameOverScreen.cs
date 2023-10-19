using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    public class GameOverScreen
    {
        Texture2D winTexture;
        Vector2 winScreenPosition;
        Texture2D loseTexture;
        Vector2 loseScreenPosition;
        Game1 game1;

        SpriteFont font;
        string text;
        Vector2 textPosition;

        public GameOverScreen(Texture2D winTexture, Vector2 winScreenPosition, Texture2D loseTexture, Vector2 loseScreenPosition, SpriteFont font, Game1 game1, int screenWidth)
        {
            this.winTexture = winTexture;
            this.winScreenPosition = winScreenPosition;
            this.loseTexture = loseTexture;
            this.loseScreenPosition = loseScreenPosition;
            this.font = font;
            this.game1 = game1;

            textPosition = new Vector2(screenWidth / 2, 100);
        }

        public void DrawLose(SpriteBatch spriteBatch)
        {           
            text = "You died! Press Enter to restart..." +
                   $"\nPoints: {game1.points}";

            Vector2 textMiddlePoint = font.MeasureString(text) / 2;

            spriteBatch.Draw(loseTexture, loseScreenPosition, Color.White);
            spriteBatch.DrawString(font, text, textPosition, Color.White, 0f, textMiddlePoint, 1.5f, SpriteEffects.None, 0f);
        }

        public void DrawWin(SpriteBatch spriteBatch) 
        {
            text = "You saved the princess! Press Enter to play again..." +
                   $"\nPoints: {game1.points}";

            Vector2 textMiddlePoint = font.MeasureString(text) / 2;

            spriteBatch.Draw(winTexture, winScreenPosition, Color.White);
            spriteBatch.DrawString(font, text, textPosition, Color.White, 0f, textMiddlePoint, 1.5f, SpriteEffects.None, 0f);
        }
    }
}
