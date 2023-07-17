using Plugin.Maui.Audio;

namespace SpaceInvaders.Views.HighScores
{
    public partial class HighScoresPage : ContentPage
    {
        private IAudioManager audioManager;

        public HighScoresPage(IAudioManager audioManager)
        {
            InitializeComponent();

            this.audioManager = audioManager;
        }

        private async void GoBackToMain_Clicked(object sender, EventArgs e)
        {
            //var navigateSound = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Resources/Audio/explosion.wav"));
            //navigateSound.Play();
            await AppShell.Current.GoToAsync("///MainPage");
        }
    }
}