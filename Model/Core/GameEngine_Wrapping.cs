using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Model.Core {

    public partial class GameEngine {
        private float GetSafePlatformX(float platformWidth) {
            int leftMargin = 20;
            int rightMargin = 20 + (int)platformWidth;

            float topPlatformY = Platforms.Min(p => p.Y);

            for (int attempts = 0; attempts < 10; attempts++) {
                float x = Random.Next(leftMargin, ScreenWidth - rightMargin);
                
                bool tooClose = false;
                
                var nearbyPlatforms = Platforms.OrderBy(p => p.Y).Take(8);
        
                foreach (var platform in nearbyPlatforms) {
                    if (Math.Abs(platform.Y - topPlatformY) > 50)
                        continue;
                        
                    if (Math.Abs(platform.X - x) < platformWidth * 1.2) {
                        tooClose = true;
                        break;
                    }
                }
                
                if (!tooClose)
                    return x;
            }

            return Random.Next(leftMargin, ScreenWidth - rightMargin);
        }

        private void GeneratePlatformsWithinBounds() {
            int totalPlatforms = 15;
            float lastPlatformY = ScreenHeight - 100;
            float platformWidth = 60;

            for (int i = 0; i < totalPlatforms; i++) {
                float x = GetSafePlatformX(platformWidth);

                float spacing = Random.Next(40, 100);
                lastPlatformY -= spacing;

                AddRandomPlatform(x, lastPlatformY);
            }
        }

        private void AddRandomPlatform(float x, float y) {
            double platformType = Random.NextDouble();

            if (platformType < 0.55)
                Platforms.Add(new NormalPlatform(x, y));

            else if (platformType < 0.8) 
                Platforms.Add(new BreakablePlatform(x, y));

            else if (platformType < 0.95) 
                Platforms.Add(new BoostPlatform(x, y));

            else 
                Platforms.Add(new TrickPlatform(x, y));
        }

        private void GenerateNewPlatformRowWithinBounds() {
            float topPlatformY = Platforms.Min(p => p.Y);
            int platformsToAdd = 5;
            float platformWidth = 60;

            float totalVerticalSpace = 300f; 
            float sectionHeight = totalVerticalSpace / platformsToAdd;

            for (int i = 0; i < platformsToAdd; i++) {
                float x = GetSafePlatformX(platformWidth);

                float y = topPlatformY - (sectionHeight * i) - Random.Next(10, (int)(sectionHeight * 0.8f));

                AddRandomPlatform(x, y);
            }

            Platforms.RemoveAll(p => p.Y > CameraY + ScreenHeight);
        }
    }
}