namespace JeuDeCombat
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            choiceButtons = new GroupBox();
            textClass = new Label();
            name = new Label();
            ordiStats = new GroupBox();
            playerStat = new GroupBox();
            SuspendLayout();
            // 
            // choiceButtons
            // 
            choiceButtons.AccessibleName = "";
            choiceButtons.Location = new Point(106, 137);
            choiceButtons.Name = "choiceButtons";
            choiceButtons.Size = new Size(594, 64);
            choiceButtons.TabIndex = 0;
            choiceButtons.TabStop = false;
            choiceButtons.Text = "Classe";
            // 
            // textClass
            // 
            textClass.AutoSize = true;
            textClass.Location = new Point(337, 218);
            textClass.Name = "textClass";
            textClass.Size = new Size(0, 15);
            textClass.TabIndex = 1;
            // 
            // name
            // 
            name.AutoSize = true;
            name.BackColor = SystemColors.Control;
            name.Font = new Font("Segoe UI", 25F);
            name.Location = new Point(97, 41);
            name.Name = "name";
            name.Size = new Size(633, 46);
            name.TabIndex = 2;
            name.Text = "Super Street Combat 74 Deluxe Premium";
            name.TextAlign = ContentAlignment.TopCenter;
            // 
            // ordiStats
            // 
            ordiStats.Location = new Point(655, 276);
            ordiStats.Name = "ordiStats";
            ordiStats.Size = new Size(133, 144);
            ordiStats.TabIndex = 3;
            ordiStats.TabStop = false;
            ordiStats.Text = "Stats Ordinateur";
            ordiStats.Visible = false;
            // 
            // playerStat
            // 
            playerStat.Location = new Point(12, 276);
            playerStat.Name = "playerStat";
            playerStat.Size = new Size(133, 144);
            playerStat.TabIndex = 4;
            playerStat.TabStop = false;
            playerStat.Text = "Stats Joueur";
            playerStat.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(playerStat);
            Controls.Add(ordiStats);
            Controls.Add(name);
            Controls.Add(textClass);
            Controls.Add(choiceButtons);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox choiceButtons;
        private Label textClass;
        private Label name;
        private GroupBox ordiStats;
        private GroupBox playerStat;
    }
}
