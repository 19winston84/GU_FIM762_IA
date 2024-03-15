
namespace AutocompleteApplication
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
            this.loadDataSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TokenizeButton = new System.Windows.Forms.ToolStripButton();
            this.GenerateUnigramsButton = new System.Windows.Forms.ToolStripButton();
            this.progressListBox = new System.Windows.Forms.ListBox();
            this.GenerateBigramsButton = new System.Windows.Forms.ToolStripButton();
            this.GenerateTrigramButton = new System.Windows.Forms.ToolStripButton();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.OptionBox = new System.Windows.Forms.ListBox();
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
            this.menuStrip1.Size = new System.Drawing.Size(1600, 40);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDataSetToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(71, 36);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadDataSetToolStripMenuItem
            // 
            this.loadDataSetToolStripMenuItem.Name = "loadDataSetToolStripMenuItem";
            this.loadDataSetToolStripMenuItem.Size = new System.Drawing.Size(289, 44);
            this.loadDataSetToolStripMenuItem.Text = "Load data set";
            this.loadDataSetToolStripMenuItem.Click += new System.EventHandler(this.loadDataSetToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(289, 44);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TokenizeButton,
            this.GenerateUnigramsButton,
            this.GenerateBigramsButton,
            this.GenerateTrigramButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 40);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1600, 42);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // TokenizeButton
            // 
            this.TokenizeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TokenizeButton.Image = ((System.Drawing.Image)(resources.GetObject("TokenizeButton.Image")));
            this.TokenizeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TokenizeButton.Name = "TokenizeButton";
            this.TokenizeButton.Size = new System.Drawing.Size(112, 36);
            this.TokenizeButton.Text = "Tokenize";
            this.TokenizeButton.Click += new System.EventHandler(this.TokenizeButton_Click);
            // 
            // GenerateUnigramsButton
            // 
            this.GenerateUnigramsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.GenerateUnigramsButton.Image = ((System.Drawing.Image)(resources.GetObject("GenerateUnigramsButton.Image")));
            this.GenerateUnigramsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GenerateUnigramsButton.Name = "GenerateUnigramsButton";
            this.GenerateUnigramsButton.Size = new System.Drawing.Size(223, 36);
            this.GenerateUnigramsButton.Text = "Generate Unigrams";
            this.GenerateUnigramsButton.Click += new System.EventHandler(this.GenerateUnigramsButton_Click);
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
            this.progressListBox.Location = new System.Drawing.Point(0, 82);
            this.progressListBox.Margin = new System.Windows.Forms.Padding(6);
            this.progressListBox.Name = "progressListBox";
            this.progressListBox.Size = new System.Drawing.Size(1600, 783);
            this.progressListBox.TabIndex = 5;
            this.progressListBox.TabStop = false;
            // 
            // GenerateBigramsButton
            // 
            this.GenerateBigramsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.GenerateBigramsButton.Image = ((System.Drawing.Image)(resources.GetObject("GenerateBigramsButton.Image")));
            this.GenerateBigramsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GenerateBigramsButton.Name = "GenerateBigramsButton";
            this.GenerateBigramsButton.Size = new System.Drawing.Size(207, 36);
            this.GenerateBigramsButton.Text = "Generate Bigrams";
            this.GenerateBigramsButton.Click += new System.EventHandler(this.GenerateBigramsButton_Click);
            // 
            // GenerateTrigramButton
            // 
            this.GenerateTrigramButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.GenerateTrigramButton.Image = ((System.Drawing.Image)(resources.GetObject("GenerateTrigramButton.Image")));
            this.GenerateTrigramButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GenerateTrigramButton.Name = "GenerateTrigramButton";
            this.GenerateTrigramButton.Size = new System.Drawing.Size(202, 36);
            this.GenerateTrigramButton.Text = "Generate Trigram";
            this.GenerateTrigramButton.Click += new System.EventHandler(this.GenerateTrigramButton_Click);
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(24, 601);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(695, 31);
            this.InputBox.TabIndex = 6;
            this.InputBox.TabStop = false;
            this.InputBox.TextChanged += new System.EventHandler(this.InputBox_TextChanged);
            this.InputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputBox_KeyDown);
            // 
            // OptionBox
            // 
            this.OptionBox.FormattingEnabled = true;
            this.OptionBox.ItemHeight = 25;
            this.OptionBox.Location = new System.Drawing.Point(24, 655);
            this.OptionBox.Name = "OptionBox";
            this.OptionBox.Size = new System.Drawing.Size(695, 179);
            this.OptionBox.TabIndex = 7;
            this.OptionBox.TabStop = false;
            this.OptionBox.SelectedIndexChanged += new System.EventHandler(this.OptionBox_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1600, 865);
            this.Controls.Add(this.OptionBox);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.progressListBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Autocompletion";
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
        private System.Windows.Forms.ToolStripMenuItem loadDataSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ListBox progressListBox;
        private System.Windows.Forms.ToolStripButton GenerateUnigramsButton;
        private System.Windows.Forms.ToolStripButton TokenizeButton;
        private System.Windows.Forms.ToolStripButton GenerateBigramsButton;
        private System.Windows.Forms.ToolStripButton GenerateTrigramButton;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.ListBox OptionBox;
    }
}

