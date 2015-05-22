namespace NewGameUI.Dialogs
{
    partial class EndGameDialog
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
            this.buttonYes = new System.Windows.Forms.Button();
            this.buttonNo = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblFileName = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lblGameStats = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listPlayers = new System.Windows.Forms.ListView();
            this.Skóre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Jméno = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Smrt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonYes
            // 
            this.buttonYes.Location = new System.Drawing.Point(295, 240);
            this.buttonYes.Name = "buttonYes";
            this.buttonYes.Size = new System.Drawing.Size(84, 23);
            this.buttonYes.TabIndex = 2;
            this.buttonYes.Text = "Ano";
            this.buttonYes.UseVisualStyleBackColor = true;
            this.buttonYes.Click += new System.EventHandler(this.buttonYes_Click);
            // 
            // buttonNo
            // 
            this.buttonNo.Location = new System.Drawing.Point(385, 240);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.Size = new System.Drawing.Size(84, 23);
            this.buttonNo.TabIndex = 3;
            this.buttonNo.Text = "Ne";
            this.buttonNo.UseVisualStyleBackColor = true;
            this.buttonNo.Click += new System.EventHandler(this.buttonNo_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.listPlayers);
            this.panel1.Controls.Add(this.lblFileName);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.txtFileName);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(463, 188);
            this.panel1.TabIndex = 9;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(3, 164);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(79, 13);
            this.lblFileName.TabIndex = 12;
            this.lblFileName.Text = "Název souboru";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(372, 160);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(84, 23);
            this.buttonSave.TabIndex = 11;
            this.buttonSave.Text = "Uložit";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(88, 161);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(278, 20);
            this.txtFileName.TabIndex = 10;
            // 
            // lblGameStats
            // 
            this.lblGameStats.Location = new System.Drawing.Point(12, 316);
            this.lblGameStats.Name = "lblGameStats";
            this.lblGameStats.Size = new System.Drawing.Size(453, 43);
            this.lblGameStats.TabIndex = 9;
            this.lblGameStats.Text = "Výsledek hry";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(200, 245);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Spustit další hru?";
            // 
            // listPlayers
            // 
            this.listPlayers.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listPlayers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listPlayers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.Skóre,
            this.Jméno,
            this.Smrt});
            this.listPlayers.Location = new System.Drawing.Point(-5, -1);
            this.listPlayers.Name = "listPlayers";
            this.listPlayers.Size = new System.Drawing.Size(467, 155);
            this.listPlayers.TabIndex = 13;
            this.listPlayers.UseCompatibleStateImageBehavior = false;
            this.listPlayers.View = System.Windows.Forms.View.Details;
            // 
            // Skóre
            // 
            this.Skóre.Text = "Skóre";
            this.Skóre.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Skóre.Width = 50;
            // 
            // Jméno
            // 
            this.Jméno.Text = "Jméno";
            this.Jméno.Width = 160;
            // 
            // Smrt
            // 
            this.Smrt.Text = "Smrt";
            this.Smrt.Width = 220;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 20;
            // 
            // EndGameDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(487, 272);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonNo);
            this.Controls.Add(this.buttonYes);
            this.Controls.Add(this.lblGameStats);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EndGameDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Konec hry - výsledky";
            this.Load += new System.EventHandler(this.EndGameDialog_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonYes;
        private System.Windows.Forms.Button buttonNo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label lblGameStats;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.ListView listPlayers;
        private System.Windows.Forms.ColumnHeader Skóre;
        private System.Windows.Forms.ColumnHeader Jméno;
        private System.Windows.Forms.ColumnHeader Smrt;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}