using System.Diagnostics;
using System.Timers;

namespace SpaceInvaders
{
    public class GameState
    {
        // Game Set up
        public int numberOfEnemiesPerRow { get; } = 6;
        public int numberOfRowsOfEnemies { get; } = 6;
        public int startingEnemySpeed { get; set; } = 1;
        public int currentLevel { get; set; } = 0;

        // Game in progress? 
        public bool isPlaying { get; set; }
        public bool gameOver { get; set; } = false;
        // Score
        public int finishingScore { get; set; } = 0;
        public int endOfLevelScore { get; set; } = 0;
        public string PlayerName { get; set; }
    }
}
