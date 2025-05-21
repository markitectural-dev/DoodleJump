using System;
using System.IO;
using System.Windows.Forms;
using Model.Core;
using Model.Data;
using static Model.Data.GameSerializer;

namespace DoodleJump.Forms {
    public partial class MenuForm : Form {
        private string defaultSavePath => Path.Combine(
            saveFolderPath,
            $"doodle-jump_game-state.{(cboSaveFormat.SelectedIndex == 0 ? "json" : "xml")}"
        );
        private string FormatPathForDisplay(string path, int maxLength = 25) {
            if (string.IsNullOrEmpty(path) || path.Length <= maxLength)
                return path;

            string folderName = Path.GetFileName(path);

            if (folderName.Length >= maxLength - 3)
                return folderName[..(maxLength - 3)] + "...";

            string drive = Path.GetPathRoot(path);
            int remainingChars = maxLength - drive.Length - folderName.Length - 3;

            if (remainingChars < 0)
                return folderName + "...";

            return drive + "..." + folderName;
        }
        public MenuForm() {
            InitializeComponent();

            saveFolderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "DoodleJump");

            if (!string.IsNullOrEmpty(saveFolderPath) && !Directory.Exists(saveFolderPath)) {
                Directory.CreateDirectory(saveFolderPath);
            }

            lnkSaveFolder.Text = FormatPathForDisplay(saveFolderPath);
            toolTip1.SetToolTip(lnkSaveFolder, saveFolderPath);
            toolTip1.SetToolTip(lblSaveFolderTitle, "Click the path to change save location");

            btnContinueGame.Enabled = false;
            cboSaveFormat.SelectedIndex = 0;

            CheckSaveFileExists();
        }

        private void CheckSaveFileExists() {
            bool fileExists = File.Exists(defaultSavePath);
            
            if (fileExists)
            {
                try
                {
                    string fileText = File.ReadAllText(defaultSavePath);
                    if (!string.IsNullOrWhiteSpace(fileText) && fileText.Contains("PlayerX"))
                    {
                        btnContinueGame.Enabled = true;
                        lblFileStatus.Text = "File valid";
                        lblFileStatus.ForeColor = Color.Green;
                        return;
                    }
                    else
                    {
                        btnContinueGame.Enabled = false;
                        lblFileStatus.Text = "Invalid file structure";
                        lblFileStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                catch
                {
                    btnContinueGame.Enabled = false;
                    lblFileStatus.Text = "Error reading save file";
                    lblFileStatus.ForeColor = Color.Red;
                    return;
                }
            }

            btnContinueGame.Enabled = false;
            lblFileStatus.Text = "File not found";
            lblFileStatus.ForeColor = Color.Gray;
        }

        private void cboSaveFormat_SelectedIndexChanged(object sender, EventArgs e) {
            CheckSaveFileExists();
        }

        private void lnkSaveFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            using (var folderDialog = new FolderBrowserDialog()) {
                folderDialog.Description = "Select folder for game state saves";
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK) {
                    saveFolderPath = folderDialog.SelectedPath;
                    lnkSaveFolder.Text = FormatPathForDisplay(saveFolderPath);
                    toolTip1.SetToolTip(lnkSaveFolder, saveFolderPath);

                    if (!Directory.Exists(saveFolderPath)) {
                        Directory.CreateDirectory(saveFolderPath);
                    }

                    CheckSaveFileExists();
                }
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e) {
            try {
                GameEngine gameEngine = new GameEngine();

                StartGame(gameEngine);
            }
            catch (Exception ex) {
                MessageBox.Show($"Error starting new game: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnContinueGame_Click(object sender, EventArgs e) {
            try {
                if (!File.Exists(defaultSavePath)) {
                    MessageBox.Show("No saved game found.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SerializerType type = cboSaveFormat.SelectedIndex == 0
                                        ? SerializerType.JSON : SerializerType.XML;

                GameSerializer serializer = GetSerializer<GameSerializer>(type);

                GameEngine gameEngine = serializer.LoadGame(defaultSavePath);

                StartGame(gameEngine);
            }
            catch (Exception ex) {
                MessageBox.Show($"Error loading saved game: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartGame(GameEngine gameEngine) {
            this.Hide();

            SerializerType type = cboSaveFormat.SelectedIndex == 0
                        ? SerializerType.JSON : SerializerType.XML;

            using (var gameForm = new MainForm(gameEngine, defaultSavePath, type)) {
                gameForm.FormClosed += (s, args) => this.Show();
                gameForm.ShowDialog();
            }

            CheckSaveFileExists();
        }

    }
}