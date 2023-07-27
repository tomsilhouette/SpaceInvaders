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
        public float playerXcord { get; set; } = 500.0f;
        public float playerYcord { get; set; } = 1750.0f;

        // player image png for canvas to draw on load
        public string playerImagePath { get; set; } = "player.png";
    }
}
