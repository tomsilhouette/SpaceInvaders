using CommunityToolkit.Mvvm.Input;
using SpaceInvaders.ViewModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace SpaceInvaders.Views.Upgrades;

public partial class UpgradesPage : ContentPage
{
    private UpgradesViewModel viewModel;
    public GameState State { get; set; }

    public UpgradesPage(UpgradesViewModel vm, GameState state)
	{
		InitializeComponent();
        BindingContext = viewModel = vm;
        State = state;
    }

    private async void BackToPage_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine($"STATEEEEEEEEEEEEEEEEEEEEEEEE {State}");
        if (State.GameOver)
        { 
            await AppShell.Current.GoToAsync("///LevelCompletePage");
        }
        else
        {
            await AppShell.Current.GoToAsync("///MainPage");
        }
    }

}