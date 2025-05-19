using System;
using System.Collections;
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
        private bool shiftKeyPressed = false;
        private float sprintMeter = 100f;
        private float maxSprintMeter = 100f;
        private float sprintDrainRate = 30f; 
        private float sprintRechargeRate = 15f; 
        private bool canSprint => sprintMeter > 20f; 
        private readonly int gameWidth = 480;
        private readonly int gameHeight = 720;
        private Font smallFont = new Font("Arial", 8);

        private Image player;
        private Image normal_platform;
        private Image break_platform_t1;
        private Image break_platform_t2;
        private Image jump_platform;


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

            player = Image.FromFile("Forms/Image/player.png");
            normal_platform = Image.FromFile("Forms/Image/platform.png");
            break_platform_t1 = Image.FromFile("Forms/Image/breakT1.png");
            break_platform_t2 = Image.FromFile("Forms/Image/break_t2.png");
            jump_platform = Image.FromFile("Forms/Image/jumpP.png");


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

                case Keys.ShiftKey:
                    shiftKeyPressed = true;
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
                
                case Keys.ShiftKey:
                    shiftKeyPressed = false;
                    break;
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e) {
            float sprintMultiplier = 2f;

            if (shiftKeyPressed && canSprint)
                sprintMeter = Math.Max(0, sprintMeter - sprintDrainRate * (gameTimer.Interval / 1000f));
            else 
                sprintMeter = Math.Min(maxSprintMeter, sprintMeter + sprintRechargeRate * (gameTimer.Interval / 1000f));
            

            if (leftKeyPressed) {
                if (shiftKeyPressed && canSprint)
                    gameEngine.Player.MoveLeft(gameEngine.Player.MoveSpeed * sprintMultiplier);
                else
                    gameEngine.Player.MoveLeft();
            }
            else if (rightKeyPressed) {
                if (shiftKeyPressed && canSprint)
                    gameEngine.Player.MoveRight(gameEngine.Player.MoveSpeed * sprintMultiplier); 
                else
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

            if (player != null)
            {
                g.DrawImage(player,
                    gameEngine.Player.X,
                    playerScreenY,
                    40, 40);
            }

            foreach (var platform in gameEngine.Platforms) {
                if (!platform.IsActive)
                    continue;

                float platformScreenY = platform.Y - cameraY;

                if (platformScreenY > gameHeight || platformScreenY + platform.Height < 0)
                    continue;

                Image i;
                if (platform.Color == Color.Green)
                    i = normal_platform;
                else if (platform.Color == Color.Orange)
                    i = break_platform_t1;
                else if (platform.Color == Color.Blue)
                    i = jump_platform;
                else if (platform.Color == Color.Red)
                    i = break_platform_t2;
                else
                    i = null;
                if (i != null)
                {
                    g.DrawImage(i,
                        platform.X,
                        platformScreenY,
                        platform.Width,
                        platform.Height);
                }
            }

            string scoreText = $"Score: {gameEngine.Score}";
            g.DrawString(scoreText, Font, Brushes.White, 10, 10);
        

            float meterWidth = 100;
            float meterHeight = 5;
            RectangleF meterBackground = new RectangleF(5, 50, meterWidth, meterHeight);
            RectangleF meterFill = new RectangleF(5, 50, meterWidth * (sprintMeter / maxSprintMeter), meterHeight);
            
            g.FillRectangle(Brushes.DarkGray, meterBackground);
            g.DrawRectangle(Pens.Black, 5, 50, meterWidth, meterHeight);

            Brush meterBrush;
            if (!canSprint)
                meterBrush = Brushes.Crimson;
            else if (sprintMeter < 50)
                meterBrush = Brushes.Gold;
            else
                meterBrush = Brushes.LimeGreen;

            g.FillRectangle(meterBrush, meterFill);
        }
    }
}