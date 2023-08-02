using SpaceInvaders.ViewModel;
using SpaceInvaders.Models;
using System.Diagnostics;

namespace SpaceInvaders.Views.HighScores
{
    public partial class HighScoresPage : ContentPage
    {
        // private IAudioManager AudioManager;

        private readonly HighScoresViewModel vm;
        public Database Database { get; set; }
        public Color SmallFontColour { get; set; }


        public int MissingScoreContainers = 0;
       // public HighScoresPage(IAudioManager audioManager, Database database)
        public HighScoresPage(Database database)
        {
            Database = database;
            InitializeComponent();

            // AudioManager = audioManager;
            vm = (HighScoresViewModel) BindingContext;
        }

        protected override void OnAppearing()
        {
            CheckTheme();
            SetUserHighScores();
        }

        private void CheckTheme()
        {
            // Get the current theme
            var currentTheme = App.Current.RequestedTheme;

            // Check if the current theme is dark or not
            if (currentTheme == AppTheme.Light)
            {
                SmallFontColour = Color.FromRgba(0, 0, 0, 255);
            }
            else if (currentTheme == AppTheme.Dark)
            {
                SmallFontColour = Color.FromRgba(255, 255, 255, 255);
            }
        }

        private void SetUserHighScores()
        {
            var counter = 0;
            MissingScoreContainers = (10 - Database.HighScoresList.Count);

            foreach (User user in Database.HighScoresList.Take(10))
            {
                Color textColor;
                int largestFontSize = 32;
                int largerFontSize = 28;
                int largeFontSize = 24;
                int fontSize;

                if (counter == 0)
                {
                    // Gold color for the first item
                    textColor = Color.FromRgba(255, 215, 0, 255); // Gold color
                    fontSize = largestFontSize;
                }
                else if (counter == 1)
                {
                    // Silver color for the second item
                    textColor = Color.FromRgba(192, 192, 192, 255); // Silver color
                    fontSize = largerFontSize;
                }
                else if (counter == 2)
                {
                    // Bronze color for the third item
                    textColor = Color.FromRgba(205, 127, 50, 255); // Bronze color
                    fontSize = largeFontSize;
                }
                else
                {
                    textColor = SmallFontColour;
                    fontSize = 22;
                }

                Label userLabel = new()
                {
                    Text = user.Username,
                    FontSize = fontSize,
                    TextColor = textColor,
                    FontAttributes = FontAttributes.Bold,
                };

                Label scoreLabel = new()
                {
                    Text = user.Score.ToString(),
                    FontSize = fontSize,
                    TextColor = textColor,
                    FontAttributes = FontAttributes.Bold,
                };

                HighScoreGrid.Add(userLabel, 0, counter);
                HighScoreGrid.Add(scoreLabel, 1, counter);

                counter++;
            }

            if (MissingScoreContainers > 0)
            {
                for (int i = 0; i < MissingScoreContainers; i++)
                {
                    Label userLabel = new()
                    {
                        Text = "",
                    };

                    Label scoreLabel = new()
                    {
                        Text = "",
                    };

                    HighScoreGrid.Add(userLabel, 0, counter);
                    HighScoreGrid.Add(scoreLabel, 1, counter);

                    counter++;
                }
            }
        }

        private async void GoBackToMain_Clicked(object sender, EventArgs e)
        {
            /*var navigateSound = AudioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Resources/Audio/explosion.wav"));
            navigateSound.Play();*/
            await AppShell.Current.GoToAsync("///MainPage");
        }
    }
}