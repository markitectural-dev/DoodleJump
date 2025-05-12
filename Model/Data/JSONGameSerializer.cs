using System;
using System.Collections.Generic;
using System.IO;
using Model.Core;
using Newtonsoft.Json;

namespace Model.Data {
    public class JSONGameSerializer : GameSerializer {
        public override void SaveGame(GameEngine engine, string filePath) {
            try {
                // Create game state from engine
                GameState gameState = new GameState(engine);

                // Serialize to JSON
                string json = JsonConvert.SerializeObject(gameState, Formatting.Indented); // serialize with visual structure

                // Ensure directory exists
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) 
                    Directory.CreateDirectory(directory);
                
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex) {
                Console.WriteLine($"Error saving game: {ex.Message}");
                throw;
            }
        }

        public override GameEngine LoadGame(string filePath, int screenWidth = 480, int screenHeight = 720) {
            try {
                // Read JSON from file
                string json = File.ReadAllText(filePath);

                // Deserialize JSON
                GameState gameState = JsonConvert.DeserializeObject<GameState>(json);

                // Create new game engine
                GameEngine engine = new GameEngine(screenWidth, screenHeight);

                // Create player
                Player player = new Player(gameState.PlayerX, gameState.PlayerY);

                // Create platforms
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

                // Load game state into engine
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