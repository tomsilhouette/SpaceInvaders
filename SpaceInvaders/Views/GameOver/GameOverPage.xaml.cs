using SpaceInvaders.Views.HighScores;
using System.Diagnostics;
using SpaceInvaders.ViewModel;
using SpaceInvaders.Models;

namespace SpaceInvaders.Views.GameOver;

public partial class GameOverPage : ContentPage
{
    public GameState State { get; set; }
    public Database Database { get; set; }
    private GameOverViewModel Govm { get; set; }


    public GameOverPage(GameState state, Database database, GameOverViewModel govm)
	{
        Database = database;
        State = state;

        InitializeComponent();

        BindingContext = Govm = govm;

        FinalScoreContainer.Text = State.FinishingScore.ToString();
    }

    protected override void OnAppearing()
    {
        // Check if score is in top 10
        if (Database.HighScoresList.Count >= 10)
        {
            foreach (User user in Database.HighScoresList)
            {
                Debug.WriteLine($"USERS11111 {user.Score}");
            }
            int tenthPositionScore = Database.HighScoresList[9].Score; // 10th position has index 9
            Debug.WriteLine($"TENTH POSSSSSSS {tenthPositionScore}");

            if (State.FinishingScore > tenthPositionScore)
            {
                Govm.SetEntryBoolCommand.Execute(this);
            }
        } 
        // If less than 10 people in highscore list
        else if (Database.HighScoresList.Count < 10 && State.FinishingScore > 0)
        {
            Debug.WriteLine($"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Govm.SetEntryBoolCommand.Execute(this);
        }
        else
        {
            // Do nothing - no score for you
        }
    }

    private async void HomeButton_Clicked(object sender, EventArgs e)
    {
        ResetGame();
        await AppShell.Current.GoToAsync("///MainPage");
    }

    private async void NewGameButton_Clicked(object sender, EventArgs e)
    {
        ResetGame();
        await AppShell.Current.GoToAsync("///GamePage");
    }

    private void ResetGame()
    {
        State.CurrentLevel = 0;
    }
}