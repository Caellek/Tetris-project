using System.Drawing;

namespace Tetris
{
    partial class Tetris
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tetris));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.scoreTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boardPBox = new System.Windows.Forms.PictureBox();
            this.NextFigureBox = new System.Windows.Forms.PictureBox();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.levelLabel = new System.Windows.Forms.Label();
            this.FallingTimer = new System.Windows.Forms.Timer(this.components);
            this.StartStopButton = new System.Windows.Forms.Button();
            this.RestartButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LineLabel = new System.Windows.Forms.Label();
            this.GameOverLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boardPBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextFigureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scoreTableToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // scoreTableToolStripMenuItem
            // 
            this.scoreTableToolStripMenuItem.Name = "scoreTableToolStripMenuItem";
            resources.ApplyResources(this.scoreTableToolStripMenuItem, "scoreTableToolStripMenuItem");
            this.scoreTableToolStripMenuItem.Click += new System.EventHandler(this.scoreTableOpen);
            // 
            // boardPBox
            // 
            this.boardPBox.BackColor = System.Drawing.Color.Gray;
            this.boardPBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.boardPBox, "boardPBox");
            this.boardPBox.Name = "boardPBox";
            this.boardPBox.TabStop = false;
            // 
            // NextFigureBox
            // 
            this.NextFigureBox.BackColor = System.Drawing.Color.Gray;
            this.NextFigureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.NextFigureBox, "NextFigureBox");
            this.NextFigureBox.Name = "NextFigureBox";
            this.NextFigureBox.TabStop = false;
            // 
            // scoreLabel
            // 
            resources.ApplyResources(this.scoreLabel, "scoreLabel");
            this.scoreLabel.Name = "scoreLabel";
            // 
            // levelLabel
            // 
            resources.ApplyResources(this.levelLabel, "levelLabel");
            this.levelLabel.Name = "levelLabel";
            // 
            // FallingTimer
            // 
            this.FallingTimer.Tick += new System.EventHandler(this.FallingTimer_Tick);
            // 
            // StartStopButton
            // 
            resources.ApplyResources(this.StartStopButton, "StartStopButton");
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.TabStop = false;
            this.StartStopButton.UseVisualStyleBackColor = true;
            this.StartStopButton.Click += new System.EventHandler(this.StartStop_game);
            // 
            // RestartButton
            // 
            resources.ApplyResources(this.RestartButton, "RestartButton");
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.UseVisualStyleBackColor = true;
            this.RestartButton.Click += new System.EventHandler(this.Restart_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // LineLabel
            // 
            resources.ApplyResources(this.LineLabel, "LineLabel");
            this.LineLabel.Name = "LineLabel";
            // 
            // GameOverLabel
            // 
            resources.ApplyResources(this.GameOverLabel, "GameOverLabel");
            this.GameOverLabel.BackColor = System.Drawing.Color.Gray;
            this.GameOverLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.GameOverLabel.Name = "GameOverLabel";
            // 
            // Tetris
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GameOverLabel);
            this.Controls.Add(this.LineLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RestartButton);
            this.Controls.Add(this.StartStopButton);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.NextFigureBox);
            this.Controls.Add(this.boardPBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Tetris";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boardPBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextFigureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem scoreTableToolStripMenuItem;
        private System.Windows.Forms.PictureBox boardPBox;
        private System.Windows.Forms.PictureBox NextFigureBox;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Label levelLabel;
        private System.Windows.Forms.Timer FallingTimer;
        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.Button RestartButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label LineLabel;
        private System.Windows.Forms.Label GameOverLabel;
    }
}

