using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DonkeyKong
{
    public static class GameStateController
    {
        public static void PlayingUpdate(KeyboardState keys, GameTime gameTime, Player player, Animation playerWalkingAnimation, Animation playerClimbingAnimation)
        {
            player.Move(keys, gameTime, playerWalkingAnimation);
            player.UpdateRectanglePos();
            playerWalkingAnimation.UpdatePosition(player.position);
            playerClimbingAnimation.UpdatePosition(player.position);
        }
    }
}
