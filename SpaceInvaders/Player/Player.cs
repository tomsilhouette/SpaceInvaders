using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Plugin.Maui.Audio;
using Microsoft.Maui.Graphics;

namespace SpaceInvaders.Player
{
    public class Player
    {
        public int playerLives { get; set; } = 3;

        public int playerXcord { get; set; } = 500;
        public int playerYcord { get; set; } = 1750;

        // player image png for canvas to draw on load
        public string playerImagePath { get; set; } = "player.png";
    }
}
