using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using SkiaSharp;
using SpaceInvaders.Enemies;
using SpaceInvaders.Weapons;
using System.ComponentModel;
using System.Diagnostics;
using SpaceInvaders.Models;
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
        public int currentScore;  
        
        [ObservableProperty]
        public string currentLevel;

        public Player.Player player = new Player.Player();
        public SKPaint PaintCom { get; set; }

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
            currentLevel = "0" + State.CurrentLevel+1.ToString();
        }        
        public GameViewModel() 
        {

        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new Timer(TimeSpan.FromMilliseconds(32.0f));

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            TickEvent?.Invoke(this, EventArgs.Empty);

            if (!State.IsPlaying)
            {
                aTimer.Stop();

                player.playerXcord = 500;
                player.playerYcord = 1750;

                if (EnemyAlienGrid.Count > 0) 
                {
                    State.FinishingScore += CurrentScore;

                    Shell.Current.GoToAsync("///GameOverPage");
                }

                if (EnemyAlienGrid.Count == 0)
                {
                    State.FinishingScore += CurrentScore;
                    State.EndOfLevelScore = CurrentScore;

                    Shell.Current.GoToAsync("///LevelCompletePage");
                }
            }
        }

        internal void StartGame()
        {
            State.IsPlaying = true;

            // Add player
            using var playerStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{imageSource}{player.playerImagePath}");
            playerBitmap = SKBitmap.Decode(playerStream);

            CreateGameAnimations();

            SetTimer();
        }

        // Draws every tick of the timer
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
                State.IsPlaying = false;
            }
            else {

                List<Alien> aliensToRemove = new List<Alien>();
                List<Bolt> killBoltsToRemove = new List<Bolt>();

                foreach (Alien alien in EnemyAlienGrid)
                {
                    if (alien.Y >= player.playerYcord)
                    {
                        // LOSING CONDITIONS
                        killBoltsToRemove.Clear();
                        BoltsFired.Clear();
                        State.IsPlaying = false;
                    }

                    if (DirectionLeft)
                    {
                        if (alien.X >= 950)
                        {
                            foreach (Alien alien2 in EnemyAlienGrid)
                            {
                                alien2.Y += 100;
                            }
                            DirectionLeft = false;
                        }
                        alien.X += 5;

                    } else {

                        alien.X -= 5;

                        if (alien.X <= 0)
                        {
                            foreach (Alien alien2 in EnemyAlienGrid)
                            {
                                alien2.Y += 100;
                            }
                            DirectionLeft = true;
                        }
                    }

                    var alienPos = mat.Invert().MapPoint(alien.X, alien.Y);
                    gameCanvas.DrawBitmap(enemyAlienBitmap, alienPos, new SKPaint());


                    var alienRect = mat.Invert().MapRect(new SKRect(alien.X, alien.Y, alien.X + 120, alien.Y + 100));
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

        // Runs start of each level
        internal void CreateGameAnimations()
        {
            CurrentScore = 0;

            int currentAlienYCord = 100;
            int currentAlienXCord = 5;

            for (int i = 0; i < State.NumberOfRowsOfEnemies; i++)
            {
                currentAlienXCord = 5;

                for (int j = 0; j < State.NumberOfEnemiesPerRow; j++)
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
