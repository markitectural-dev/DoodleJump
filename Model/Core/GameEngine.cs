
namespace Model.Core {

    public partial class GameEngine {

        public Player Player { get; private set; }
        public List<IPlatform> Platforms { get; private set; }
        public int Score { get; private set; } 
        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }
        public bool IsGameOver { get; private set; } = false;
        public float CameraY { get; private set; } 
        private float HighestPoint { get; set; } 
        private float HighestCameraY { get; set; } 

        private Random Random { get; set; } = new Random();

        public GameEngine(int screenWidth = 480, int screenHeight = 720) {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            InitializeNewGame();
        }

        public void InitializeNewGame() {
            IsGameOver = false;
            Score = 0;
            CameraY = 0;
            HighestPoint = 0;

            Platforms = new List<IPlatform>();

            float platformWidth = 60;
            float platformX = ScreenWidth / 2 - platformWidth / 2;
            float platformY = ScreenHeight - 100;

            Platforms.Add(new NormalPlatform(platformX, platformY));

            Player = new Player(platformX + platformWidth / 2 - 20, platformY - 40);

            Player.Jump();

            GeneratePlatformsWithinBounds();
        }

        public void LoadGame(Player player, List<IPlatform> platforms, int score) {
            Player = player;
            Platforms = platforms;
            Score = score;
            IsGameOver = false;
            HighestPoint = score;
            
            HighestCameraY = ScreenHeight - (int)Player.Y - ScreenHeight / 2; 
        }




        public void Update() {
            if (IsGameOver)
                return;

            Player.Update();

            HandlePlayerWrapping();

            CheckPlatformCollisions();

            foreach (var platform in Platforms) {
                platform.Update();
            }

            Platforms.RemoveAll(p => !p.IsActive);

            float currentHeight = ScreenHeight - Player.Y;
            if (currentHeight > HighestPoint) {
                HighestPoint = currentHeight;
                Score = (int)HighestPoint;
            }

            if (Player.Y < ScreenHeight / 2) {
                float newCameraY = Player.Y - ScreenHeight / 2;
        
                if (newCameraY < HighestCameraY) {
                    CameraY = newCameraY;
                    HighestCameraY = newCameraY;
                } else {
                    CameraY = HighestCameraY;
                }
            }

            float topPlatformY = Platforms.Count > 0 ? Platforms.Min(p => p.Y) : 0;
            if (topPlatformY > CameraY) 
                GenerateNewPlatformRowWithinBounds();
            

            if (Player.Y > CameraY + ScreenHeight - Player.Height) 
                IsGameOver = true;
            
        }

        private void CheckPlatformCollisions() {
            foreach (var platform in Platforms) {
                if (Player.IsCollidingWith(platform)) {
                    platform.Bounce(Player);
                    break;
                }
            }
        }


        public void MovePlayerLeft() {
            Player.MoveLeft();
        }

        public void MovePlayerRight() {
            Player.MoveRight();
        }

        public void StopPlayerMovement() {
            Player.StopMoving();
        }
    }
}