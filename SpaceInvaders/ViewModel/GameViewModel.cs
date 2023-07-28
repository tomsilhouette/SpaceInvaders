using CommunityToolkit.Mvvm.ComponentModel;
using SkiaSharp;
using SpaceInvaders.Enemies;
using SpaceInvaders.Weapons;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Timers;
using Timer = System.Timers.Timer;
using CommunityToolkit.Mvvm.Input;

namespace SpaceInvaders.ViewModel
{
    public partial class GameViewModel : ObservableObject
    {
        private Timer aTimer;

        Random random = new();
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

        public Player.Player player = new();
        public SKPaint PaintCom { get; set; }

        public SKCanvas gameCanvas;

        private readonly string imageSource = "SpaceInvaders.Resources.Images.";

        // Game Images
        private SKBitmap playerBitmap;
        private SKBitmap enemyAlienBitmap;
        private SKBitmap enemyShipBitmap;
        private SKBitmap enemyAlienAttackBitmap;
        private SKBitmap explosionBitmap;

        // Game Objects
        public List<Alien> EnemyAlienGrid = new();
        public List<Alien> AttackingAliens = new();
        public List<Bolt> BoltsFired = new();
        public List<Ship> EnemyShips = new();
        public List<EnemyAttack> EnemyBoltsFired = new();

        private int enemyAlienCount = 1;

        private int enemyShotTimer = 50;
        private int enemyShipTimer = 200;
        private int playerAttackTimer = 10;

        private int enemyMovementSpeedXCoord = 4;
        private int enemyMovementSpeedYCoord = 40;

        private float deviceCanvasWidth;
        private float deviceCanvasHeight;

        private float smallDeviceCanvasWidth = 1100;
        private float smallDeviceCanvasHeight = 2222;

        public event EventHandler TickEvent;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool DirectionLeft { get; set; } = true;

        public GameViewModel(GameState state)
        {
            State = state;
            State.GameOver = false;

            CheckScreenSize();
            CreateGameBitmaps();
        }
        public GameViewModel()
        {

        }

        private void CheckScreenSize()
        {
            // Get the screen metrics
            var metrics = DeviceDisplay.MainDisplayInfo;

            // Access the screen size properties
            double screenWidth = metrics.Width;
            double screenHeight = metrics.Height;

            if (screenWidth > 1200)
            {
                enemyMovementSpeedXCoord = 10;
                enemyMovementSpeedYCoord = 100;
                State.MediumEnemySpeedXCoord = 14;
                State.MediumEnemySpeedYCoord = 120;
                State.LargeEnemySpeedXCoord = 25;
                State.LargeEnemySpeedYCoord = 140;
            }
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

            // Add explosion if hit
            using var explosionStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{imageSource}enemy_attack.png");
            explosionBitmap = SKBitmap.Decode(explosionStream).Resize(new SKImageInfo(800, 800), SKFilterQuality.Low);

            // Add enemy aliens
            using var alienStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{imageSource}enemyAlien.png");
            enemyAlienBitmap = SKBitmap.Decode(alienStream);
        }

        public void SetDeviceDimensions()
        {
            deviceCanvasWidth = gameCanvas.DeviceClipBounds.Width;
            deviceCanvasHeight = gameCanvas.DeviceClipBounds.Height;
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
        public void SetCanvas(SKCanvas canvas)
        {
            gameCanvas = canvas;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            TickEvent?.Invoke(this, EventArgs.Empty);

            enemyShotTimer++;
            enemyShipTimer++;
            playerAttackTimer--;

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

        internal void SetDeviceSizes()
        {
            if (deviceCanvasWidth > smallDeviceCanvasWidth)
            {
                player.playerYcord = deviceCanvasHeight - (deviceCanvasHeight / 9.0f);
            }
            else
            {
                player.playerYcord = deviceCanvasHeight - (deviceCanvasHeight / 6.0f);
            }
        }

        // Draws every tick of the timer
        internal void DrawGame()
        {
            var mat = SKMatrix.CreateScale(0.2f, 0.2f);
            gameCanvas.SetMatrix(mat);

            SetDeviceSizes();

            // Set player
            var playerPos = mat.Invert().MapPoint(player.playerXcord, player.playerYcord);
            gameCanvas.DrawBitmap(playerBitmap, new SKPoint(playerPos.X, playerPos.Y), new SKPaint());

            var playerRect = mat.Invert().MapRect(new SKRect(player.playerXcord - 15, player.playerYcord, player.playerXcord + 100, player.playerYcord + 100));
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

                    if (ship.X >= deviceCanvasWidth)
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

                // TODO
                foreach (Alien alien in EnemyAlienGrid)
                {
                    if (alien.Y >= deviceCanvasHeight / 3 && alien.Y <= deviceCanvasHeight / 2)
                    {
                        enemyMovementSpeedXCoord = State.MediumEnemySpeedXCoord;
                        enemyMovementSpeedYCoord = State.MediumEnemySpeedYCoord;

                        if (alien.Y >= deviceCanvasHeight / 2)
                        {
                            enemyMovementSpeedXCoord = State.LargeEnemySpeedXCoord;
                            enemyMovementSpeedYCoord = State.LargeEnemySpeedYCoord;
                        }
                    }

                    if (alien.Y >= player.playerYcord)
                    {
                        // LOSING CONDITIONS
                        CheckForLoseConditions(killBoltsToRemove, alienKillBoltsToRemove);
                    }

                    if (DirectionLeft)
                    {
                        if (alien.X >= deviceCanvasWidth - 150)
                        {
                            foreach (Alien alien2 in EnemyAlienGrid)
                            {
                                alien2.Y += enemyMovementSpeedYCoord;
                            }
                            DirectionLeft = false;
                        }
                        alien.X += enemyMovementSpeedXCoord;

                    }
                    else
                    {

                        alien.X -= enemyMovementSpeedXCoord;

                        if (alien.X <= 0)
                        {
                            foreach (Alien alien2 in EnemyAlienGrid)
                            {
                                alien2.Y += enemyMovementSpeedYCoord;
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

                // Remove all items from removal lists
                ManageListRemovals(aliensToRemove, killBoltsToRemove, alienKillBoltsToRemove, shipsToRemove);
            }
        }

        // Remove objects from lists
        private void ManageListRemovals(List<Alien> aliensToRemove, List<Bolt> killBoltsToRemove, List<EnemyAttack> alienKillBoltsToRemove, List<Ship> shipsToRemove)
        {
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
                List<Bolt> boltsToRemove = new();

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

        internal void GenerateEnemyAttacks(SKCanvas gameCanvas, SKMatrix mat, SKRect playerRect, List<EnemyAttack> alienKillBoltsToRemove)
        {
            foreach (EnemyAttack enemyBolt in EnemyBoltsFired)
            {
                float newY = enemyBolt.attackYpos += State.EnemyAttackSpeed;

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
                    var explosionPos = mat.Invert().MapPoint(enemyBolt.attackXpos, newY);
                    gameCanvas.DrawBitmap(explosionBitmap, explosionPos, new SKPaint());

                    State.PlayerLives--;
                    alienKillBoltsToRemove.Add(enemyBolt);
                    SetPlayerLives();

                    gameCanvas.DrawBitmap(explosionBitmap, explosionPos, new SKPaint());
                }

                if (alienAttackPos.Y > deviceCanvasHeight * 5)
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
            if (enemyShipTimer >= 300 && EnemyShips.Count == 0)
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
            }
            else if (State.PlayerLives == 2)
            {
                CanViewOne = true;
                CanViewTwo = true;
                CanViewThree = false;
            }
            else if (State.PlayerLives == 1)
            {
                CanViewOne = true;
                CanViewTwo = false;
                CanViewThree = false;
            }
            else if (State.PlayerLives == 0)
            {
                CanViewOne = false;
                CanViewTwo = false;
                CanViewThree = false;
            }
            else if (State.PlayerLives == -1)
            {
                State.GameOver = true;
                State.IsPlaying = false;
            }
        }

        // WIN GAME CONDITIONS
        public void CheckForWinConditions()
        {
            BoltsFired.Clear();
            EnemyAlienGrid.Clear();
            EnemyShips.Clear();
            EnemyBoltsFired.Clear();
            AttackingAliens.Clear();
            State.IsPlaying = false;
        }

        // LOSE GAME CONDITIONS
        public void CheckForLoseConditions(List<Bolt> killBoltsToRemove, List<EnemyAttack> alienKillBoltsToRemove)
        {
            killBoltsToRemove.Clear();
            alienKillBoltsToRemove.Clear();

            BoltsFired.Clear();
            EnemyShips.Clear();
            EnemyBoltsFired.Clear();

            State.IsPlaying = false;
            State.GameOver = true;
        }


        // Go Left
        [RelayCommand]
        private void MovePlayerLeft()
        {
            player.playerXcord -= 100;
        }

        // Go Right
        [RelayCommand]
        private void MovePlayerRight()
        {
            player.playerXcord += 100;
        }

        // Fire
        [RelayCommand]
        private void FirePlayerWeapon()
        {
            if (playerAttackTimer <= 0)
            {
                BoltsFired.Add(new Bolt(player.playerXcord + 50, player.playerYcord));
            }

            playerAttackTimer = 5;
        }
    }
}
