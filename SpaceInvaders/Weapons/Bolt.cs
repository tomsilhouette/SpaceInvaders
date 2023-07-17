using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Weapons
{
    public class Bolt
    {
        public int BoltHeight { get; set; } = 100;
        public int BoltWidth { get; set; } = 10;

        public float BoltXcord { get; set; } = 0;
        public float BoltYcord { get; set; } = 0;

        public Bolt(float boltXcord, float boltYcord)
        {
            BoltXcord = boltXcord;
            BoltYcord = boltYcord;
        }
    }      
}
