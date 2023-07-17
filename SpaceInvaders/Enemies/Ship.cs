using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Enemies
{
    class Ship : IEnemy
    {
        // Set to come in screen from left to right
        public int X { get; set; } = 0;
        public int Y { get; set; } = 10;
        public string playerImagePath { get; set; } = "enemyShip.png";
        public int ScorePerKill { get; } = 50;

    }
}
