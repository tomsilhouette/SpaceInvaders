using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using SkiaSharp;
using SpaceInvaders.Enemies;
using SpaceInvaders.Views.Game;
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

        Random random = new Random();
        public GameState State { get; set; }

        [ObservableProperty]
        public int currentScore;  
        
        [ObservableProperty]
        public string currentLevel;
        
        [ObservableProperty]
        public bool canViewOne = true;    
        
        [ObservableProperty]
        public bool canViewTwo = true; 
        
        [ObservableProperty]
        public bool canViewThree = true;

        public Player.Player player = new Player.Player();
        public SKPaint PaintCom { get; set; }

        private readonly string imageSource = "SpaceInvaders.Resources.Images.";

        // Game Images
        private SKBitmap playerBitmap;
        private SKBitmap enemyAlienBitmap;
        private SKBitmap enemyShipBitmap;
        private SKBitmap enemyAlienAttackBitmap;

        // Game Objects
        public List<Alien> EnemyAlienGrid = new ();
        public List<Alien> AttackingAliens = new ();
        public List<Bolt> BoltsFired = new ();
        public List<Ship> EnemyShips = new ();
        public List<EnemyAttack> EnemyBoltsFired = new ();

        private int enemyAlienCount = 1;
        private int enemyShotTimer = 50;
        private int enemyShipTimer = 200;

        public event EventHandler TickEvent;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool DirectionLeft { get; set; } = true;

        public GameViewModel(GameState state)
        {
            State = state;
            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            CreateGameBitmaps();
        } 
        public GameViewModel() 
        {

        }

        private void CreateGameBitmaps()
        {
            // Add player
            using var playerStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{imageSource}{player.playerImagePath}");
            playerBitmap = SKBitmap.Decode(playerStream);

            // Add enemy ship
            using var shipStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{imageSource}enemyShip.png");
            enemyShipBitmap = SKBitmap.Decode(shipStream);

            // Add enemy attack bolts
            using var enemyAttackStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{imageSource}enemy_attack.png");
            enemyAlienAttackBitmap = SKBitmap.Decode(enemyAttackStream).Resize(new SKImageInfo(200, 200), SKFilterQuality.Low);

            // Add enemy aliens
            using var alienStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{imageSource}enemyAlien.png");
            enemyAlienBitmap = SKBitmap.Decode(alienStream);
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

            enemyShotTimer++;
            enemyShipTimer++;

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

                if (EnemyAlienGrid.Count == 0 && !State.GameOver)
                {
                    State.FinishingScore += CurrentScore;
                    State.EndOfLevelScore = CurrentScore;

                    Shell.Current.GoToAsync("///LevelCompletePage");
                }
            }
        }


        internal void StartGame()
        {
            SetLevelNumber();

            EnemyShips.Clear();
            EnemyAlienGrid.Clear();
            EnemyBoltsFired.Clear();
            BoltsFired.Clear();
            AttackingAliens.Clear();

            enemyShotTimer = 50;
            enemyShipTimer = 200;
            enemyAlienCount = 1;

            State.IsPlaying = true;

            CreateGameAnimations();

            SetTimer();
        }

        // Draws every tick of the timer
        internal void DrawGame(SKCanvas gameCanvas)
        {
           // Draw the image on the canvas
            var mat = SKMatrix.CreateScale(0.2f, 0.2f);
            gameCanvas.SetMatrix(mat);

            float canvasWidth = gameCanvas.DeviceClipBounds.Width;

            // Set player
            var playerPos = mat.Invert().MapPoint(player.playerXcord, player.playerYcord);
            gameCanvas.DrawBitmap(playerBitmap, new SKPoint(playerPos.X, playerPos.Y), new SKPaint());

            var playerRect = mat.Invert().MapRect(new SKRect(player.playerXcord - 5, player.playerYcord, player.playerXcord + 110, player.playerYcord + 100));
            gameCanvas.DrawRect(playerRect, new SKPaint()
            {
                IsStroke = true,
                Color = SKColors.Transparent
            });

            GenerateRandomEnemyAttack();
            GenerateRandomEnemyShip();

            if (EnemyAlienGrid.Count == 0)
            {
                CheckForWinConditions();
            }
            else 
            {
                List<Alien> aliensToRemove = new();
                List<Bolt> killBoltsToRemove = new();
                List<Ship> shipsToRemove = new();
                List<EnemyAttack> alienKillBoltsToRemove = new();


                foreach (Ship ship in EnemyShips)
                {
                    var shipMap = mat.Invert().MapPoint(ship.X, ship.Y);
                    gameCanvas.DrawBitmap(enemyShipBitmap, shipMap, new SKPaint());

                    ship.X += 5;

                    if (ship.X >= canvasWidth)
                    {
                        shipsToRemove.Add(ship);
                    }

                    var shipRect = mat.Invert().MapRect(new SKRect(ship.X, ship.Y, ship.X + 120, ship.Y + 90));

                    gameCanvas.DrawRect(shipRect, new SKPaint()
                    {
                        IsStroke = true,
                        Color = SKColors.Transparent
                    });

                    var shipPos = mat.Invert().MapPoint(ship.X, ship.Y);

                    foreach (Bolt bolt in BoltsFired)
                    {
                        var boltPos = mat.Invert().MapPoint(bolt.BoltXcord, bolt.BoltYcord);
                        if (shipRect.Contains(boltPos))
                        {
                            shipsToRemove.Add(ship);
                            killBoltsToRemove.Add(bolt);
                            CurrentScore += ship.ScorePerKill;
                        }
                    }
                }

                foreach (Alien alien in EnemyAlienGrid)
                {
                    if (alien.Y >= player.playerYcord)
                    {
                        // LOSING CONDITIONS
                        CheckForLoseConditions(killBoltsToRemove, alienKillBoltsToRemove);
                    }

                    if (DirectionLeft)
                    {
                        if (alien.X >= 950)
                        {
                            foreach (Alien alien2 in EnemyAlienGrid)
                            {
                                alien2.Y += 60;
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
                                alien2.Y += 60;
                            }
                            DirectionLeft = true;
                        }
                    }

                    var alienPos = mat.Invert().MapPoint(alien.X, alien.Y);
                    gameCanvas.DrawBitmap(enemyAlienBitmap, alienPos, new SKPaint());


                    var alienRect = mat.Invert().MapRect(new SKRect(alien.X, alien.Y, alien.X + 150, alien.Y + 100));
                    gameCanvas.DrawRect(alienRect, new SKPaint()
                    {
                        IsStroke = true,
                        Color = SKColors.Transparent
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

                // Generate player attack bolts
                GeneratePlayerAttacks(gameCanvas, mat);                
                
                // Generate enemy attack bolts
                GenerateEnemyAttacks(gameCanvas, mat, playerRect, alienKillBoltsToRemove);

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
                
                foreach (EnemyAttack enemyBoltToRemove in alienKillBoltsToRemove)
                {
                    EnemyBoltsFired.Remove(enemyBoltToRemove);
                }

                foreach (Ship ship in shipsToRemove)
                {
                    EnemyShips.Remove(ship);
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
        }

        internal void GeneratePlayerAttacks(SKCanvas gameCanvas, SKMatrix mat)
        {
            if (BoltsFired.Count > 0)
            {
                List<Bolt> boltsToRemove = new ();

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

        internal void GenerateEnemyAttacks(SKCanvas gameCanvas, SKMatrix mat, SKRect playerRect, List <EnemyAttack> alienKillBoltsToRemove)
        {
            foreach (EnemyAttack enemyBolt in EnemyBoltsFired)
            {
                float canvasHeight = gameCanvas.DeviceClipBounds.Height;

                // Add enemy bolt
                int addMe = 10;

                float newY = enemyBolt.attackYpos += addMe;

                var alienAttackPos = mat.Invert().MapPoint(enemyBolt.attackXpos, newY);
                gameCanvas.DrawBitmap(enemyAlienAttackBitmap, alienAttackPos, new SKPaint());

                var alienAttackRect = mat.Invert().MapRect(new SKRect(enemyBolt.attackXpos, newY + 5, enemyBolt.attackXpos + 30, newY + 30));

                gameCanvas.DrawRect(alienAttackRect, new SKPaint()
                {
                    IsStroke = true,
                    Color = SKColors.Transparent
                });

                if (playerRect.Contains(alienAttackPos))
                {
                    State.PlayerLives--;
                    alienKillBoltsToRemove.Add(enemyBolt);
                    SetPlayerLives();
                }

                if (alienAttackPos.Y < 200)
                {
                    alienKillBoltsToRemove.Add(enemyBolt);
                }
            }
        }

        internal void GenerateRandomEnemyAttack()
        {
            if (enemyShotTimer > 60)
            {
                // Find alien in row based on index
                try
                {
                    var alienFound = EnemyAlienGrid[random.Next(0, EnemyAlienGrid.Count - 1)];

                    // Create attack for alien
                    EnemyBoltsFired.Add(new EnemyAttack(alienFound.X, alienFound.Y));

                    // reset spawn timer
                    enemyShotTimer = 0;
                } 
                catch
                {
                    Debug.WriteLine("CATCHCATCHCATCHCATCHCATCHCATCHCATCHCATCHCATCH");
                }

            }
        }   
        
        internal void GenerateRandomEnemyShip()
        {
            if (enemyShipTimer >= 300)
            {
                CreateShipAndAddToCanvas();
                enemyShipTimer = 0;
            }
        }

        internal void CreateShipAndAddToCanvas()
        {
            EnemyShips.Add(new Ship());
        }

        public void SetLevelNumber()
        {
            int levelNum = State.CurrentLevel + 1;
            CurrentLevel = levelNum.ToString().PadLeft(3, '0');
        }

        public void SetPlayerLives()
        {
            if (State.PlayerLives == 3)
            {
                CanViewOne = true;
                CanViewTwo = true;
                CanViewThree = true;
            } else if (State.PlayerLives == 2) {
                CanViewOne = true;
                CanViewTwo = true;
                CanViewThree = false;
            } else if (State.PlayerLives == 1) {
               CanViewOne = true;
               CanViewTwo = false;
               CanViewThree = false;
            } else if (State.PlayerLives == 0) {
               CanViewOne = false;
               CanViewTwo = false;
               CanViewThree = false;
            } else if (State.PlayerLives == -1)
            {
                State.GameOver = true;
                State.IsPlaying = false;
            }
        }     
        
        public void CheckForWinConditions()
        {
            // WIN GAME CONDITIONS

            BoltsFired.Clear();
            EnemyAlienGrid.Clear();
            EnemyShips.Clear();
            EnemyBoltsFired.Clear();
            AttackingAliens.Clear();
            State.IsPlaying = false;
        }   
        
        public void CheckForLoseConditions(List <Bolt> killBoltsToRemove, List <EnemyAttack> alienKillBoltsToRemove)
        {
            // LOSE GAME CONDITIONS
            killBoltsToRemove.Clear();
            BoltsFired.Clear();
            EnemyShips.Clear();
            EnemyBoltsFired.Clear();
            alienKillBoltsToRemove.Clear();
            State.IsPlaying = false;
            State.GameOver = true;
        }

        public void FireWeapon()
        {
            BoltsFired.Add(new Bolt(player.playerXcord + 50, player.playerYcord));
        }    
        
        public void GoLeft()
        {
            player.playerXcord -= 100;
        }   
        
        public void GoRight()
        {
            player.playerXcord += 100;
        }
    }
}
