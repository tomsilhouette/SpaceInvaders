using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using SkiaSharp;
using SpaceInvaders.Enemies;
using SpaceInvaders.Weapons;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SpaceInvaders.ViewModel
{
    public partial class GameViewModel : ObservableObject
    {
        private Timer aTimer;       
        public GameState State { get; set; }

        [ObservableProperty]
        private int currentScore;

        // Init player
        public Player.Player player = new Player.Player();
        public SKPaint paintCom { get; set; }

        private readonly string imageSource = "SpaceInvaders.Resources.Images.";

        private SKBitmap playerBitmap;
        private SKBitmap enemyAlienBitmap;

        public List<Alien> EnemyAlienGrid = new List<Alien>();
        public List<Bolt> BoltsFired = new List<Bolt>();

        private int enemyAlienCount = 1;

        public event EventHandler TickEvent;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool DirectionLeft { get; set; } = true;

        public GameViewModel(GameState state) 
        {
            State = state;
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new Timer(TimeSpan.FromMilliseconds(16.0f));

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            TickEvent?.Invoke(this, EventArgs.Empty);

            if (!State.isPlaying)
            {
                aTimer.Stop();

                // Set final score and reset
                State.endOfLevelScore = CurrentScore;
                State.finishingScore += State.endOfLevelScore;

                CurrentScore = 0;

                player.playerXcord = 500;
                player.playerYcord = 1750;

                if (EnemyAlienGrid.Count > 0) 
                {               
                    Shell.Current.GoToAsync("///GameOverPage");
                }

                if (EnemyAlienGrid.Count == 0)
                {
                    Shell.Current.GoToAsync("///LevelCompletePage");
                }
            }
        }

        internal void StartGame()
        {
            State.isPlaying = true;

            // Add player
            using var playerStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{imageSource}{player.playerImagePath}");
            playerBitmap = SKBitmap.Decode(playerStream);

            CreateGameAnimations();

            SetTimer();
        }

        internal void DrawGame(SKCanvas gameCanvas)
        {
            // Draw the image on the canvas
            var mat = SKMatrix.CreateScale(0.2f, 0.2f);
            gameCanvas.SetMatrix(mat);

            // Set player
            var playerPos = mat.Invert().MapPoint(player.playerXcord, player.playerYcord);
            gameCanvas.DrawBitmap(playerBitmap, new SKPoint(playerPos.X, playerPos.Y), new SKPaint());

            if (EnemyAlienGrid.Count == 0)
            {
                // WIN GAME CONDITIONS
                BoltsFired.Clear();
                
                StopGame();

            } else {

                List<Alien> aliensToRemove = new List<Alien>();
                List<Bolt> killBoltsToRemove = new List<Bolt>();

                foreach (Alien alien in EnemyAlienGrid)
                {
                    if (alien.Y == player.playerYcord)
                    {
                        // LOSING CONDITIONS
                        killBoltsToRemove.Clear();
                        StopGame();
                    }

                    if (DirectionLeft)
                    {
                        if (alien.X >= 950)
                        {
                            foreach (Alien alien2 in EnemyAlienGrid)
                            {
                                alien2.Y += 10;
                            }
                            DirectionLeft = false;
                        }
                        alien.X += 5;
                    }

                    if (!DirectionLeft)
                    {
                        alien.X -= 5;

                        if (alien.X <= 0)
                        {
                            foreach (Alien alien2 in EnemyAlienGrid)
                            {
                                alien2.Y += 10;
                            }
                            DirectionLeft = true;
                        }
                    }

                    var alienPos = mat.Invert().MapPoint(alien.X, alien.Y);
                    gameCanvas.DrawBitmap(enemyAlienBitmap, alienPos, new SKPaint());


                    var alienRect = mat.Invert().MapRect(new SKRect(alien.X, alien.Y, alien.X + 100, alien.Y + 100));
                    gameCanvas.DrawRect(alienRect, new SKPaint()
                    {
                        IsStroke = true,
                        Color = SKColors.AliceBlue
                    });

                    foreach (Bolt bolt in BoltsFired)
                    {
                        var boltPos = mat.Invert().MapPoint(bolt.BoltXcord, bolt.BoltYcord);
                        if (alienRect.Contains(boltPos))
                        {
                            aliensToRemove.Add(alien);
                            killBoltsToRemove.Add(bolt);
                        }
                    }

                }

                // Remove the bolts outside the foreach loop
                foreach (Alien alienToRemove in aliensToRemove)
                {
                    EnemyAlienGrid.Remove(alienToRemove);
                    CurrentScore += alienToRemove.ScorePerKill;
                }

                foreach (Bolt boltToRemove in killBoltsToRemove)
                {
                    BoltsFired.Remove(boltToRemove);
                }

                if (BoltsFired.Count > 0)
                {
                    List<Bolt> boltsToRemove = new List<Bolt>();

                    foreach (Bolt bolt in BoltsFired)
                    {
                        if (bolt.BoltYcord <= 0)
                        {
                            boltsToRemove.Add(bolt);
                        }

                        bolt.BoltYcord -= 40.0f;

                        // Create the SKRect object
                        SKRect rect = mat.Invert().MapRect(new SKRect(bolt.BoltXcord, bolt.BoltYcord, bolt.BoltXcord + bolt.BoltWidth, bolt.BoltYcord + bolt.BoltHeight));

                        // Draw the rectangle
                        gameCanvas.DrawRect(rect, new SKPaint()
                        {
                            Color = SKColors.AliceBlue
                        });
                    }

                    // Remove the bolts outside the foreach loop
                    foreach (Bolt boltToRemove in boltsToRemove)
                    {
                        BoltsFired.Remove(boltToRemove);
                    }
                }
            }
        }

        internal void StopGame()
        {
            State.isPlaying = false;
        }

        internal void CreateGameAnimations()
        {
            int currentAlienYCord = 100;
            int currentAlienXCord = 5;

            for (int i = 0; i < State.numberOfRowsOfEnemies; i++)
            {
                currentAlienXCord = 1;

                for (int j = 0; j < State.numberOfEnemiesPerRow; j++)
                {
                    EnemyAlienGrid.Add(new Alien(enemyAlienCount, currentAlienXCord, currentAlienYCord));
                    enemyAlienCount++;
                    currentAlienXCord += 150;
                }

                currentAlienYCord += 100;
            }
           
            // Get enenmy images
            using var alienStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{imageSource}enemyAlien.png");
            enemyAlienBitmap = SKBitmap.Decode(alienStream);
        }
    }
}
