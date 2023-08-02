using System.Diagnostics;

namespace SpaceInvaders.Views.LevelComplete;

public partial class LevelCompletePage : ContentPage
{
    public GameState State { get; set; }

    public LevelCompletePage(GameState state)
	{
        State = state;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        LevelScoreContainer.Text = State.EndOfLevelScore.ToString();
        LevelTotalScoreContainer.Text = State.FinishingScore.ToString();
    }

    private async void NextLevelButton_Clicked(object sender, EventArgs e)
    {
        State.EndOfLevelScore = 0;
        State.CurrentLevel++;
        await AppShell.Current.GoToAsync("///GamePage");
    }

    private async void UpgradesPageButton_Clicked(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync("///UpgradesPage");
    }
}