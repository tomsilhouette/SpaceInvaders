using System.Diagnostics;

namespace SpaceInvaders.Views.LevelComplete;

public partial class LevelCompletePage : ContentPage
{
    public GameState State { get; set; }

    public LevelCompletePage(GameState state)
	{
        State = state;

		InitializeComponent();

        LevelScoreContainer.Text = State.endOfLevelScore.ToString();
	}

    private async void UpgradesButton_Clicked(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync("///UpgradesPage");
    }

    private async void NextLevelButton_Clicked(object sender, EventArgs e)
    {
        State.currentLevel++;
        await AppShell.Current.GoToAsync("///GamePage");
    }
}