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
            await Database.GetHighScoreRequest();
        }
    }
}
