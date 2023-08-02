using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Enemies
{
    public class Alien
    {
        public int EnemyNumber { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
/*        public float attackXpos { get; set; }
        public float attackYpos { get; set; }*/
        public int ScorePerKill { get; } = 10;

        public Alien(int enemyNumber, int x, int y)
        {
            EnemyNumber = enemyNumber;
            X = x;
            Y = y;
        }
    }
}
