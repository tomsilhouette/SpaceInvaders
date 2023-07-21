using System.Diagnostics;
using System.Timers;

namespace SpaceInvaders
{
    public class GameState
    {
        // Game Set up
        public int NumberOfEnemiesPerRow { get; } = 6;
        public int NumberOfRowsOfEnemies { get; } = 5;
        public int StartingEnemySpeed { get; set; } = 1;
        public int CurrentLevel { get; set; } = 0;
        public int FinishLevel { get; set; } = 0;

        // Game in progress? 
        public bool IsPlaying { get; set; } = false;
        public bool GameOver { get; set; } = false;
        // Score
        public int FinishingScore { get; set; } = 0;
        public int EndOfLevelScore { get; set; } = 0;
        public int ShipTimeInterval { get; set; } = 5;
        public string PlayerName { get; set; }
    }
}
