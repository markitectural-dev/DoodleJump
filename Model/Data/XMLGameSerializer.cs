using Model.Core;

namespace Model.Data {

    public class XMLGameSerializer : GameSerializer {

        public override void SaveGame(GameEngine engine, string filePath) {
            try {
                GameState gameState = new GameState(engine);

                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) 
                    Directory.CreateDirectory(directory);
                

                XmlSerializer serializer = new XmlSerializer(typeof(GameState));

                using (var fs = new StreamWriter(filePath, false)) {
                    serializer.Serialize(fs, gameState);
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Error saving game: {ex.Message}");
                throw;
            }
        }

        public override GameEngine LoadGame(string filePath, int screenWidth = 480, int screenHeight = 720) {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(GameState));

                GameState gameState;
                using (var fs = new StreamReader(filePath)) {
                    gameState = (GameState)serializer.Deserialize(fs);
                }

                GameEngine engine = new GameEngine(screenWidth, screenHeight);

                Player player = new Player(gameState.PlayerX, gameState.PlayerY);

                List<IPlatform> platforms = new List<IPlatform>();
                foreach (var platformState in gameState.Platforms) {
                    IPlatform platform;
                    switch (platformState.Type) {
                        case "Normal":
                            platform = new NormalPlatform(platformState.X, platformState.Y);
                            break;
                        case "Breakable":
                            platform = new BreakablePlatform(platformState.X, platformState.Y);
                            break;
                        case "Boost":
                            platform = new BoostPlatform(platformState.X, platformState.Y);
                            break;
                        case "Trick":
                            platform = new TrickPlatform(platformState.X, platformState.Y);
                            break;
                        default:
                            platform = new NormalPlatform(platformState.X, platformState.Y);
                            break;
                    }
                    platforms.Add(platform);
                }

                engine.LoadGame(player, platforms, gameState.Score);

                return engine;
            }
            catch (Exception ex) {
                Console.WriteLine($"Error loading game: {ex.Message}");
                throw;
            }
        }
    }
}