using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Weapons
{
    public class EnergyShield 
    {
        public int EnergyShieldHeight { get; set; } = 5;
        public int EnergyShieldWidth { get; set; } = 500;

        public float EnergyShieldXcord { get; set; } = 0;
        public float EnergyShieldYcord { get; set; } = 0;
        public int EnergyShieldDurationInSeconds { get; set; } = 5;


        public EnergyShield(float energyShieldXcord, float energyShieldYcord)
        {
            EnergyShieldXcord = energyShieldXcord;
            EnergyShieldYcord = energyShieldYcord;
        }
    }
}
