using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Decorations
{
    public class Shield
    {
        public int shieldHealth { get; set; } = 10;
        public int X { get; set; }
        public int Y { get; set; }
        public int ShieldNum { get; set; }
        public string playerImagePath { get; set; } = "barrier.png";

        public Shield(int shieldNum, int x, int y)
        {
            ShieldNum = shieldNum;
            X = x;
            Y = y;
        }
    }
}
