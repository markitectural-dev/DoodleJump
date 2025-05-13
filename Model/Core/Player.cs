using System;
using System.Drawing;

namespace Model.Core {
    public class Player {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }
        public float InitialJumpVelocity { get; private set; } = -15.0f;
        public float Gravity { get; private set; } = 0.5f;
        public float MaxVelocityY { get; private set; } = 1000000.0f;
        public float MoveSpeed { get; private set; } = 7.0f;

        public Player(float x, float y, float width = 40, float height = 40) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            VelocityY = 0;
            VelocityX = 0;

            InitialJumpVelocity = -13.0f;
            Gravity = 0.4f;
            MaxVelocityY = 12.0f;
            MoveSpeed = 6.0f;
        }

        public void MoveLeft() {
            VelocityX = -MoveSpeed;
        }
        public void MoveLeft(float customSpeed) {
            VelocityX = -customSpeed;
        }
    
        public void MoveRight() {
            VelocityX = MoveSpeed;
        }
        public void MoveRight(float customSpeed) {
            VelocityX = customSpeed;
        }
        
        public void StopMoving() {
            VelocityX = 0;
        }

        public void Jump() {
            VelocityY = InitialJumpVelocity;
        }
        public void Jump(float boostMultiplier) {
            VelocityY = InitialJumpVelocity * boostMultiplier;
        }

        public void Update() {
            VelocityY += Gravity;

            if (VelocityY > MaxVelocityY)
                VelocityY = MaxVelocityY;

            if (VelocityY > MaxVelocityY * 0.7f) {
                float fallMultiplier = 1.0f + VelocityY / MaxVelocityY * 0.3f;
                VelocityY += Gravity * fallMultiplier;
            }

            if (Math.Abs(VelocityX) > 0.1f) 
                VelocityX *= 0.95f;
            else 
                VelocityX = 0;
            
            X += VelocityX;
            Y += VelocityY;
        }

        public void WrapAroundScreen(int screenWidth) {
            if (X < -Width)
                X = screenWidth;
            else if (X > screenWidth)
                X = -Width;
        }

        public bool IsCollidingWith(IPlatform platform) {
            if (VelocityY <= 0)
                return false;

            float playerFeetY = Y + Height;

            bool horizontalOverlap = X + Width > platform.X - 3 && X < platform.X + platform.Width + 3;

            bool verticalOverlap = playerFeetY >= platform.Y - 10 && playerFeetY <= platform.Y + 5;

            return horizontalOverlap && verticalOverlap;
        }
    }
}