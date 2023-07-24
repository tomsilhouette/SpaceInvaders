using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Weapons
{
    public class EnemyAttack
    {
        public float attackXpos { get; set; }
        public float attackYpos { get; set; }
        public string AlienAttackImagePath { get; set; } = "enemy_attack.png";

        public EnemyAttack(float attackXpos, float attackYpos)
        {
            this.attackXpos = attackXpos;
            this.attackYpos = attackYpos;
        }
    }
}
