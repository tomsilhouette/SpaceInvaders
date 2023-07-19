using CommunityToolkit.Mvvm.ComponentModel;
using SpaceInvaders.Models;
using System.Diagnostics;

namespace SpaceInvaders.ViewModel
{
    public partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private int highscore;

        public Database Database { get; set; }

        public MainPageViewModel(Database database)
        {
            Database = database;
        }

        public MainPageViewModel()
        {

        }

        public async Task Initialise()
        {
            Debug.WriteLine("IIIIIINNNNNNNIIIIIIIIIITTTTTT");
            Debug.WriteLine($"DATATATATATATATTAA {Database.HighScoresList.Count}");
            Debug.WriteLine($"DATATATATATATATTAA {Database.HighScoresList[0]}");
            await Database.GetHighScoreRequest();
            Debug.WriteLine("IIIIIINNNNNNNIIIIIIIIIITTTTTT2222222222222222222222");

        }
    }
}
