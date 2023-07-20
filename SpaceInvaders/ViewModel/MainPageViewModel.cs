using CommunityToolkit.Mvvm.ComponentModel;
using SpaceInvaders.Models;
using System.Diagnostics;

namespace SpaceInvaders.ViewModel
{
    public partial class MainPageViewModel : ObservableObject
    {

        [ObservableProperty]
        private string highscoreUsername;

        [ObservableProperty]
        private int highscoreValue;

        public Database Database { get; set; }

        public MainPageViewModel(Database database)
        {
            Database = database;
        }

        public async Task Initialise()
        {
            await Database.GetHighScoreRequest();

            var winner = Database.HighScoresList.MaxBy(user => user.Score);
            HighscoreValue = winner.Score;
            HighscoreUsername = winner.Username;
        }
    }
}
