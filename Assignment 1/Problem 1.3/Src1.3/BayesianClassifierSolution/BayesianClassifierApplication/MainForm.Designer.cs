﻿
namespace BayesianClassifierApplication
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTrainingSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTestSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tokenizeButton = new System.Windows.Forms.ToolStripButton();
            this.progressListBox = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1600, 44);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadTrainingSetToolStripMenuItem,
            this.loadTestSetToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(71, 36);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadTrainingSetToolStripMenuItem
            // 
            this.loadTrainingSetToolStripMenuItem.Name = "loadTrainingSetToolStripMenuItem";
            this.loadTrainingSetToolStripMenuItem.Size = new System.Drawing.Size(325, 44);
            this.loadTrainingSetToolStripMenuItem.Text = "Load training set";
            this.loadTrainingSetToolStripMenuItem.Click += new System.EventHandler(this.loadTrainingSetToolStripMenuItem_Click);
            // 
            // loadTestSetToolStripMenuItem
            // 
            this.loadTestSetToolStripMenuItem.Name = "loadTestSetToolStripMenuItem";
            this.loadTestSetToolStripMenuItem.Size = new System.Drawing.Size(325, 44);
            this.loadTestSetToolStripMenuItem.Text = "Load test set";
            this.loadTestSetToolStripMenuItem.Click += new System.EventHandler(this.loadTestSetToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(325, 44);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tokenizeButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 44);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1600, 42);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tokenizeButton
            // 
            this.tokenizeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tokenizeButton.Enabled = false;
            this.tokenizeButton.Image = ((System.Drawing.Image)(resources.GetObject("tokenizeButton.Image")));
            this.tokenizeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tokenizeButton.Name = "tokenizeButton";
            this.tokenizeButton.Size = new System.Drawing.Size(112, 36);
            this.tokenizeButton.Text = "Tokenize";
            this.tokenizeButton.Click += new System.EventHandler(this.tokenizeButton_Click);
            // 
            // progressListBox
            // 
            this.progressListBox.BackColor = System.Drawing.Color.Black;
            this.progressListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.progressListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressListBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressListBox.ForeColor = System.Drawing.Color.Lime;
            this.progressListBox.FormattingEnabled = true;
            this.progressListBox.ItemHeight = 22;
            this.progressListBox.Location = new System.Drawing.Point(0, 86);
            this.progressListBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.progressListBox.Name = "progressListBox";
            this.progressListBox.Size = new System.Drawing.Size(1600, 779);
            this.progressListBox.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(129, 44);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(249, 42);
            this.button1.TabIndex = 5;
            this.button1.Text = "Initialize Classifier";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1600, 865);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressListBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bayesian classifier";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTrainingSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTestSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tokenizeButton;
        private System.Windows.Forms.ListBox progressListBox;
        private System.Windows.Forms.Button button1;
    }
}

