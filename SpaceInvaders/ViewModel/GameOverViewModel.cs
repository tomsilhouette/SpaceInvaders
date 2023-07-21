using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpaceInvaders.Models;
using SpaceInvaders.Views.GameOver;

namespace SpaceInvaders.ViewModel
{
    public partial class GameOverViewModel : ObservableObject
    {
        public Database Database { get; set; }
        public GameState State { get; set; }


        [ObservableProperty]
        public bool isVisible = false;

        [ObservableProperty]
        public string username;  
        
        [ObservableProperty]
        public string submitBtn = "Submit";

        public GameOverViewModel(Database database, GameState state)
        {
            Database = database;
            State = state;
        }
        public GameOverViewModel()
        {
            
        }

        [RelayCommand]
        public void SetEntryBool()
        {
            IsVisible = true;
        }

        [RelayCommand]
        async Task PostToHighScores()
        {
            Database.PostNewHighScore(Username, State.FinishingScore);
            SubmitBtn = "Score Posted!";
            Username = "";
        }        
        
        [RelayCommand]
        public void CloseKeyboard()
        {
            App.Current.MainPage.Focus();
        }
    }
}
