using Plugin.Maui.Audio;
using SpaceInvaders.ViewModel;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Linq;
using Microsoft.Maui.Graphics;
using SpaceInvaders.Models;
using System;
using SpaceInvaders.Controls;
using Microsoft.Maui.Graphics.Text;
using Microsoft.Maui;

namespace SpaceInvaders.Views.HighScores
{
    public partial class HighScoresPage : ContentPage
    {
        private IAudioManager audioManager;

        private readonly HighScoresViewModel vm;
        public Database Database { get; set; }

        public int MissingScoreContainers = 0;
        public HighScoresPage(IAudioManager audioManager, Database database)
        {
            Database = database;
            InitializeComponent();

            this.audioManager = audioManager;
            vm = (HighScoresViewModel) BindingContext;
        }

        protected override void OnAppearing()
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
                    textColor = Color.FromHex("#FFD700"); // Gold color
                    fontSize = largestFontSize;
                }
                else if (counter == 1)
                {
                    // Silver color for the second item
                    textColor = Color.FromHex("#C0C0C0"); // Silver color
                    fontSize = largerFontSize;
                }
                else if (counter == 2)
                {
                    // Bronze color for the third item
                    textColor = Color.FromHex("#CD7F32"); // Bronze color
                    fontSize = largeFontSize;
                }
                else
                {
                    // Default color for other items
                    textColor = Color.FromHex("#000000"); // Black color
                    fontSize = 22;
                }

                Label userLabel = new Label()
                {
                    Text = user.Username,
                    FontSize = fontSize,
                    TextColor = textColor,
                    FontAttributes = FontAttributes.Bold,
                };

                Label scoreLabel = new Label()
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
                    Label userLabel = new Label()
                    {
                        Text = "",
                    };

                    Label scoreLabel = new Label()
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
            var navigateSound = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Resources/Audio/explosion.wav"));
            navigateSound.Play();
            await AppShell.Current.GoToAsync("///MainPage");
        }
    }
}