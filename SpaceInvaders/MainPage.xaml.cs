using Plugin.Maui.Audio;
using SpaceInvaders.Models;
using SpaceInvaders.ViewModel;
using System.Diagnostics;

namespace SpaceInvaders;

public partial class MainPage : ContentPage
{
    // Check for music 
    // private bool isMainMusicPlaying = false;

    // Instance of audioPlayer
    // private IAudioManager audioManager;

    private MainPageViewModel ViewModel;

    // public MainPage(IAudioManager audioManager, MainPageViewModel vm)
    public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();
        // this.audioManager = audioManager;
        BindingContext = ViewModel = vm;
    }

    protected override void OnAppearing()
    {
        _ = Task.Run(ViewModel.Initialise);
        // Check music is not already playing on page loading
        // Otherwise it will play multiple times at once
/*        if (!isMainMusicPlaying)
        {
            Task.Factory.StartNew(async () =>
            {
                var mainMusic = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Resources/Audio/main_music.mpeg"));
                mainMusic.Play();
                isMainMusicPlaying = true;
            });
        }*/
    }

    private async void StartButton_Clicked(object sender, EventArgs e)
    {
/*        var startingSound = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Resources/Audio/shoot.wav"));
        startingSound.Play();*/
        await AppShell.Current.GoToAsync("///GamePage");
    }

    private async void ShowHighScores_Clicked(object sender, EventArgs e)
    {
/*        var highScoreSound = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Resources/Audio/explosion.wav"));
        highScoreSound.Play();*/
        await AppShell.Current.GoToAsync("///HighScoresPage");
    }

    private async void Upgrades_Clicked(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync("///UpgradesPage");
    }
}

