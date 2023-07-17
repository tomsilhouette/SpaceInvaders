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

        FinalScoreContainer.Text = State.finishingScore.ToString();
        State.currentLevel = 1;

        if (State.finishingScore > 100)
        {
            // Highscores
        }
    }

    private async void HomeButton_Clicked(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync("///MainPage");
    }

    private async void NewGameButton_Clicked(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync("///GamePage");

    }
}