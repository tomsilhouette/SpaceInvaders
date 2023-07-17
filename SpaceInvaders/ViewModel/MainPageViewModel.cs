using System.Diagnostics;

namespace SpaceInvaders.ViewModel
{
    public class MainPageViewModel
    {
        public int HighestScore { get; set; } = 3141222;
        public string HighScorePlayerName { get; set; } = "Win-man";

        public MainPageViewModel() { }
    }
}
