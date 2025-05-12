using System;
using System.IO;
using System.Windows.Forms;
using Model.Core;
using Model.Data;

namespace DoodleJump.Forms {
    public partial class MenuForm : Form {
        private string defaultSavePath => Path.Combine(
            saveFolderPath,
            $"save.{(cboSaveFormat.SelectedIndex == 0 ? "json" : "xml")}"
        );
        private string FormatPathForDisplay(string path, int maxLength = 25) {
            if (string.IsNullOrEmpty(path) || path.Length <= maxLength)
                return path;

            // Get just the folder name for the beginning
            string folderName = Path.GetFileName(path);

            // If folder name itself is too long, truncate it
            if (folderName.Length >= maxLength - 3)
                return folderName[..(maxLength - 3)] + "...";

            // Otherwise show the drive and folder name with ... in between
            string drive = Path.GetPathRoot(path);
            int remainingChars = maxLength - drive.Length - folderName.Length - 3;

            if (remainingChars < 0)
                return folderName + "...";

            return drive + "..." + folderName;
        }
        public MenuForm() {
            InitializeComponent();

            // Ensure save directory exists
            saveFolderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "DoodleJump");

            if (!string.IsNullOrEmpty(saveFolderPath) && !Directory.Exists(saveFolderPath)) {
                Directory.CreateDirectory(saveFolderPath);
            }

            lnkSaveFolder.Text = FormatPathForDisplay(saveFolderPath);
            toolTip1.SetToolTip(lnkSaveFolder, saveFolderPath);
            toolTip1.SetToolTip(lblSaveFolderTitle, "Click the path to change save location");

            // Disable Continue button by default
            btnContinueGame.Enabled = false;
            cboSaveFormat.SelectedIndex = 0;

            // Then check if a valid save file exists
            CheckSaveFileExists();
        }

        private void CheckSaveFileExists() {
            bool fileExists = File.Exists(defaultSavePath);
            if (fileExists) {
                try {
                    // Try to read the file to make sure it's valid
                    string fileText = File.ReadAllText(defaultSavePath);
                    if (!string.IsNullOrWhiteSpace(fileText) && fileText.Contains("PlayerX")) {
                        btnContinueGame.Enabled = true;
                        lblFileStatus.Text = "File valid";
                        lblFileStatus.ForeColor = Color.Green;
                        return;
                    }
                    else {
                        // File exists but doesn't have required content --> invalid
                        btnContinueGame.Enabled = false;
                        lblFileStatus.Text = "Invalid file structure";
                        lblFileStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                catch {
                    // If there's any other error reading the file, keep button disabled
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
            // Recheck save file validity when format changes
            CheckSaveFileExists();
        }

        private void lnkSaveFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog()) {
                folderDialog.Description = "Select folder for game saves";
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK) {
                    saveFolderPath = folderDialog.SelectedPath;
                    lnkSaveFolder.Text = FormatPathForDisplay(saveFolderPath);
                    toolTip1.SetToolTip(lnkSaveFolder, saveFolderPath);

                    if (!Directory.Exists(saveFolderPath)) {
                        Directory.CreateDirectory(saveFolderPath);
                    }

                    // Check if save exists in the new location
                    CheckSaveFileExists();
                }
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e) {
            try {
                // Create a new game engine
                GameEngine gameEngine = new GameEngine();

                // Start a new game session
                StartGame(gameEngine);
            }
            catch (Exception ex) {
                MessageBox.Show($"Error starting new game: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnContinueGame_Click(object sender, EventArgs e) {
            try {
                // Check if save file exists
                if (!File.Exists(defaultSavePath)) {
                    MessageBox.Show("No saved game found.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Create JSON serializer
                SerializerType type = cboSaveFormat.SelectedIndex == 0
                                        ? SerializerType.JSON : SerializerType.XML;

                GameSerializer serializer = GameSerializer.GetSerializer(type);

                // Load the game
                GameEngine gameEngine = serializer.LoadGame(defaultSavePath);

                // Start the game with loaded state
                StartGame(gameEngine);
            }
            catch (Exception ex) {
                MessageBox.Show($"Error loading saved game: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartGame(GameEngine gameEngine) {
            // Hide this form
            this.Hide();

            SerializerType type = cboSaveFormat.SelectedIndex == 0
                        ? SerializerType.JSON : SerializerType.XML;

            // Create and show the game form
            using (var gameForm = new MainForm(gameEngine, defaultSavePath, type)) {
                gameForm.FormClosed += (s, args) => this.Show();
                gameForm.ShowDialog();
            }

            // Refresh save file button state when returning to menu
            CheckSaveFileExists();
        }

        private void lblTitle_Click(object sender, EventArgs e) {

        }
    }
}