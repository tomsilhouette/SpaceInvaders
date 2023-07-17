using System.Diagnostics;

namespace SpaceInvaders.Views.Upgrades;

public partial class UpgradesPage : ContentPage
{
	public UpgradesPage()
	{
		InitializeComponent();
	}

    private void BuyLaserButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("BUUUUUY");
    }

    private void BuyLifeButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("BUUUUUY");
    }

    private async void BackToPage_Clicked(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync("///LevelCompletePage");

    }
}