using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Weapons
{
    public class Laser
    {
        public int LaserHeight { get; set; } = 1000;
        public int LaserWidth { get; set; } = 20;

        public float LaserXcord { get; set; } = 0;
        public float LaserYcord { get; set; } = 0;

        public Laser(float laserXcord, float laserYcord)
        {
            LaserXcord = laserXcord;
            LaserYcord = laserYcord;
        }
    }
}
