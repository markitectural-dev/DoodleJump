namespace DoodleJump.Forms {
    partial class MenuForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>


        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            btnNewGame = new Button();
            btnContinueGame = new Button();
            lnkSaveFolder = new LinkLabel();
            lblSaveFolderTitle = new Label();
            toolTip1 = new ToolTip(components);
            lblFormat = new Label();
            cboSaveFormat = new ComboBox();
            lblFileStatus = new Label();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.Location = new Point(164, 82);
            lblTitle.Margin = new Padding(6, 0, 6, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(440, 86);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Doodle Jump";
            lblTitle.Click += lblTitle_Click;
            // 
            // btnNewGame
            // 
            btnNewGame.BackColor = Color.LimeGreen;
            btnNewGame.Font = new Font("Segoe UI", 12F);
            btnNewGame.Location = new Point(204, 226);
            btnNewGame.Margin = new Padding(6);
            btnNewGame.Name = "btnNewGame";
            btnNewGame.Size = new Size(334, 85);
            btnNewGame.TabIndex = 1;
            btnNewGame.Text = "New Game";
            btnNewGame.UseVisualStyleBackColor = false;
            btnNewGame.Click += btnNewGame_Click;
            // 
            // btnContinueGame
            // 
            btnContinueGame.BackColor = SystemColors.Highlight;
            btnContinueGame.Font = new Font("Segoe UI", 12F);
            btnContinueGame.Location = new Point(204, 356);
            btnContinueGame.Margin = new Padding(6);
            btnContinueGame.Name = "btnContinueGame";
            btnContinueGame.Size = new Size(334, 85);
            btnContinueGame.TabIndex = 2;
            btnContinueGame.Text = "Continue Game";
            btnContinueGame.UseVisualStyleBackColor = false;
            btnContinueGame.Click += btnContinueGame_Click;
            // 
            // lnkSaveFolder
            // 
            lnkSaveFolder.ActiveLinkColor = Color.DarkBlue;
            lnkSaveFolder.AutoSize = true;
            lnkSaveFolder.Font = new Font("Segoe UI", 9F);
            lnkSaveFolder.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkSaveFolder.LinkColor = Color.RoyalBlue;
            lnkSaveFolder.Location = new Point(376, 507);
            lnkSaveFolder.Name = "lnkSaveFolder";
            lnkSaveFolder.Size = new Size(238, 32);
            lnkSaveFolder.TabIndex = 4;
            lnkSaveFolder.TabStop = true;
            lnkSaveFolder.Text = "Default save location";
            lnkSaveFolder.VisitedLinkColor = Color.RoyalBlue;
            lnkSaveFolder.LinkClicked += lnkSaveFolder_LinkClicked;
            // 
            // lblSaveFolderTitle
            // 
            lblSaveFolderTitle.AutoSize = true;
            lblSaveFolderTitle.Font = new Font("Segoe UI", 9F);
            lblSaveFolderTitle.Location = new Point(204, 507);
            lblSaveFolderTitle.Name = "lblSaveFolderTitle";
            lblSaveFolderTitle.Size = new Size(166, 32);
            lblSaveFolderTitle.TabIndex = 0;
            lblSaveFolderTitle.Text = "Save Location:";
            // 
            // lblFormat
            // 
            lblFormat.AutoSize = true;
            lblFormat.Font = new Font("Segoe UI", 9F);
            lblFormat.Location = new Point(204, 594);
            lblFormat.Name = "lblFormat";
            lblFormat.Size = new Size(151, 32);
            lblFormat.TabIndex = 5;
            lblFormat.Text = "Save Format:";
            // 
            // cboSaveFormat
            // 
            cboSaveFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSaveFormat.Font = new Font("Segoe UI", 9F);
            cboSaveFormat.FormattingEnabled = true;
            cboSaveFormat.Items.AddRange(new object[] { "JSON", "XML" });
            cboSaveFormat.Location = new Point(376, 591);
            cboSaveFormat.Name = "cboSaveFormat";
            cboSaveFormat.Size = new Size(203, 40);
            cboSaveFormat.TabIndex = 5;
            cboSaveFormat.SelectedIndexChanged += cboSaveFormat_SelectedIndexChanged;
            // 
            // lblFileStatus
            // 
            lblFileStatus.AutoSize = true;
            lblFileStatus.Font = new Font("Segoe UI", 9F);
            lblFileStatus.ForeColor = Color.Gray;
            lblFileStatus.Location = new Point(204, 539);
            lblFileStatus.Name = "lblFileStatus";
            lblFileStatus.Size = new Size(171, 32);
            lblFileStatus.TabIndex = 6;
            lblFileStatus.Text = "No save found";
            // 
            // MenuForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(743, 670);
            Controls.Add(lblSaveFolderTitle);
            Controls.Add(lnkSaveFolder);
            Controls.Add(btnContinueGame);
            Controls.Add(btnNewGame);
            Controls.Add(lblTitle);
            Controls.Add(lblFormat);
            Controls.Add(cboSaveFormat);
            Controls.Add(lblFileStatus);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(6);
            MaximizeBox = false;
            Name = "MenuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Doodle Jump";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Label lblTitle;
        private Button btnNewGame;
        private Button btnContinueGame;
        private Label lblSaveFolderTitle;
        private LinkLabel lnkSaveFolder;
        private string saveFolderPath;
        private ToolTip toolTip1;

        private Label lblFormat;
        private ComboBox cboSaveFormat;
        private Label lblFileStatus;
    }
}