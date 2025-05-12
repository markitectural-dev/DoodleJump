using System;
using System.Collections.Generic;
using System.Drawing;
using Model.Core;
using Newtonsoft.Json;

namespace Model.Data {
    public class GameState {
        // Player state
        public float PlayerX { get; set; }
        public float PlayerY { get; set; }
        public List<PlatformState> Platforms { get; set; } = new List<PlatformState>();
        public int Score { get; set; }

        // Nested class for platform serialization
        public class PlatformState {
            public float X { get; set; }
            public float Y { get; set; }
            public string Type { get; set; } 
        }

        public GameState() { }

        // Create from game engine
        public GameState(GameEngine engine) {
            PlayerX = engine.Player.X;
            PlayerY = engine.Player.Y;
            Score = engine.Score;

            foreach (var platform in engine.Platforms) {
                string type;
                if (platform is NormalPlatform)
                    type = "Normal";
                else if (platform is BreakablePlatform)
                    type = "Breakable";
                else if (platform is BoostPlatform)
                    type = "Boost";
                else if (platform is TrickPlatform)
                    type = "Trick";
                else
                    type = "Normal"; 

                Platforms.Add(new PlatformState {
                    X = platform.X,
                    Y = platform.Y,
                    Type = type
                });
            }
        }
    }
}