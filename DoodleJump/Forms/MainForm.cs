using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Model.Core;
using Model.Data;
using static Model.Data.GameSerializer;

namespace DoodleJump.Forms {
    public partial class MainForm : Form {
        private readonly GameEngine gameEngine;
        private readonly string savePath;
        private readonly SerializerType serializerType;
        private readonly System.Windows.Forms.Timer gameTimer;

        private bool leftKeyPressed = false;
        private bool rightKeyPressed = false;

        private readonly int gameWidth = 480;
        private readonly int gameHeight = 720;

        public MainForm(GameEngine engine, string savePath, SerializerType serializerType) {
            InitializeComponent();

            this.gameEngine = engine;
            this.savePath = savePath;
            this.serializerType = serializerType;

            this.ClientSize = new Size(gameWidth, gameHeight);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "Doodle Jump";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 16;
            gameTimer.Tick += GameTimer_Tick;
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            gameTimer.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            gameTimer.Stop();

            try {
                if (!gameEngine.IsGameOver) {
                    GameSerializer serializer = GameSerializer.GetSerializer<GameSerializer>(serializerType);
                    serializer.SaveGame(gameEngine, savePath);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Error saving game: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            base.OnFormClosing(e);
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);

            switch (e.KeyCode) {
                case Keys.Left:
                case Keys.A:
                    leftKeyPressed = true;
                    break;

                case Keys.Right:
                case Keys.D:
                    rightKeyPressed = true;
                    break;

                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);

            switch (e.KeyCode) {
                case Keys.Left:
                case Keys.A:
                    leftKeyPressed = false;
                    break;

                case Keys.Right:
                case Keys.D:
                    rightKeyPressed = false;
                    break;
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e) {
            if (leftKeyPressed) {
                gameEngine.Player.MoveLeft();
            }
            else if (rightKeyPressed) {
                gameEngine.Player.MoveRight();
            }
            else {
                gameEngine.Player.StopMoving();
            }

            gameEngine.Update();

            if (gameEngine.IsGameOver) {
                gameTimer.Stop();
                MessageBox.Show($"Game Over! Your score: {gameEngine.Score}", "Game Over",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            g.Clear(Color.White);

            float cameraY = gameEngine.CameraY;

            float playerScreenY = gameEngine.Player.Y - cameraY;
            g.FillRectangle(Brushes.Blue,
                gameEngine.Player.X,
                playerScreenY,
                40, 40);

            foreach (var platform in gameEngine.Platforms) {
                if (!platform.IsActive)
                    continue;

                float platformScreenY = platform.Y - cameraY;

                if (platformScreenY > gameHeight || platformScreenY + platform.Height < 0)
                    continue;

                Brush brush;
                if (platform.Color == Color.Green)
                    brush = Brushes.Green;
                else if (platform.Color == Color.Orange)
                    brush = Brushes.Orange;
                else if (platform.Color == Color.Blue)
                    brush = Brushes.Blue;
                else if (platform.Color == Color.Red)
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;  

                g.FillRectangle(brush,
                    platform.X,
                    platformScreenY,
                    platform.Width,
                    platform.Height);
            }

            string scoreText = $"Score: {gameEngine.Score}";
            g.DrawString(scoreText, Font, Brushes.Black, 10, 10);
        }
    }
}