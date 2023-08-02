namespace SpaceInvaders.Enemies
{
    public class Ship
    {
        // Set to come in screen from left to right
        public int X { get; set; } = 0;
        public int Y { get; set; } = 60;
        public string playerImagePath { get; set; } = "enemyShip.png";
        public int ScorePerKill { get; } = 50;

        public Ship() { }
    }
}
