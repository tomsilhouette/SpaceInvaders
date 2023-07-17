using Plugin.Maui.Audio;
using SpaceInvaders.ViewModel;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Linq;

namespace SpaceInvaders.Views.HighScores
{
    public partial class HighScoresPage : ContentPage
    {
        private IAudioManager audioManager;
        private readonly HighScoresViewModel vm;

        public HighScoresPage(IAudioManager audioManager)
        {
            InitializeComponent();

            this.audioManager = audioManager;
            vm = (HighScoresViewModel) BindingContext;

            foreach (HighScoresViewModel.HighScoreRecord record in vm.HighScoresList)
            {
                Debug.WriteLine($"RECORD {record.Score}");
                Label label = new Label()
                {
                   Text = record.Name + " " + record.Score.ToString(),
                   FontSize = 22,
                };
                HighScoreListView.Children.Add(label);
            }
        }

        private async void GoBackToMain_Clicked(object sender, EventArgs e)
        {
            var navigateSound = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Resources/Audio/explosion.wav"));
            navigateSound.Play();
            await AppShell.Current.GoToAsync("///MainPage");
        }
    }
}