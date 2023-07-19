using SpaceInvaders.Views.HighScores;
using System.Diagnostics;

namespace SpaceInvaders.Views.GameOver;

public partial class GameOverPage : ContentPage
{
    public GameState State { get; set; }

    public GameOverPage(GameState state)
	{
        State = state;

        InitializeComponent();

        FinalScoreContainer.Text = State.FinishingScore.ToString();

        if (State.FinishingScore > 100)
        {
            // Highscores
        }
    }

    private async void HomeButton_Clicked(object sender, EventArgs e)
    {
        ResetGame();
        await AppShell.Current.GoToAsync("///MainPage");
    }

    private async void NewGameButton_Clicked(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync("///GamePage");

    }

    private void ResetGame()
    {
        State.CurrentLevel = 0;
    }
}