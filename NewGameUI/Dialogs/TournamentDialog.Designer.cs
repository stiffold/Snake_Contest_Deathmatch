namespace NewGameUI.Dialogs
{
    partial class TournamentDialog
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
            this.components = new System.ComponentModel.Container();
            this.btStartGames = new System.Windows.Forms.Button();
            this.listPlayers = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Skóre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Jméno = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.lblGameTotalCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblToRun = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.lblFinished = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblRunning = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.upDownThreads = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.upDownGamesCount = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.upDownThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownGamesCount)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btStartGames
            // 
            this.btStartGames.Location = new System.Drawing.Point(148, 74);
            this.btStartGames.Name = "btStartGames";
            this.btStartGames.Size = new System.Drawing.Size(75, 23);
            this.btStartGames.TabIndex = 0;
            this.btStartGames.Text = "GO!";
            this.btStartGames.UseVisualStyleBackColor = true;
            this.btStartGames.Click += new System.EventHandler(this.btStartGames_Click);
            // 
            // listPlayers
            // 
            this.listPlayers.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listPlayers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listPlayers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.Skóre,
            this.Jméno});
            this.listPlayers.Location = new System.Drawing.Point(8, 130);
            this.listPlayers.Name = "listPlayers";
            this.listPlayers.Size = new System.Drawing.Size(478, 223);
            this.listPlayers.TabIndex = 14;
            this.listPlayers.UseCompatibleStateImageBehavior = false;
            this.listPlayers.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 20;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Počet her";
            // 
            // lblGameTotalCount
            // 
            this.lblGameTotalCount.Location = new System.Drawing.Point(81, 25);
            this.lblGameTotalCount.Name = "lblGameTotalCount";
            this.lblGameTotalCount.Size = new System.Drawing.Size(44, 13);
            this.lblGameTotalCount.TabIndex = 16;
            this.lblGameTotalCount.Text = "0000";
            this.lblGameTotalCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Zbývá";
            // 
            // lblToRun
            // 
            this.lblToRun.Location = new System.Drawing.Point(81, 48);
            this.lblToRun.Name = "lblToRun";
            this.lblToRun.Size = new System.Drawing.Size(44, 13);
            this.lblToRun.TabIndex = 18;
            this.lblToRun.Text = "0000";
            this.lblToRun.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Odehráno";
            // 
            // lblFinished
            // 
            this.lblFinished.Location = new System.Drawing.Point(81, 100);
            this.lblFinished.Name = "lblFinished";
            this.lblFinished.Size = new System.Drawing.Size(44, 13);
            this.lblFinished.TabIndex = 20;
            this.lblFinished.Text = "0000";
            this.lblFinished.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(399, 491);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 25);
            this.button2.TabIndex = 21;
            this.button2.Text = "Zavřít";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Hraje se";
            // 
            // lblRunning
            // 
            this.lblRunning.Location = new System.Drawing.Point(81, 75);
            this.lblRunning.Name = "lblRunning";
            this.lblRunning.Size = new System.Drawing.Size(44, 13);
            this.lblRunning.TabIndex = 23;
            this.lblRunning.Text = "0000";
            this.lblRunning.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Počet souběžných her (vláken)";
            // 
            // upDownThreads
            // 
            this.upDownThreads.Location = new System.Drawing.Point(172, 48);
            this.upDownThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownThreads.Name = "upDownThreads";
            this.upDownThreads.Size = new System.Drawing.Size(51, 20);
            this.upDownThreads.TabIndex = 26;
            this.upDownThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.upDownThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Počet her";
            // 
            // upDownGamesCount
            // 
            this.upDownGamesCount.Location = new System.Drawing.Point(172, 22);
            this.upDownGamesCount.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.upDownGamesCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownGamesCount.Name = "upDownGamesCount";
            this.upDownGamesCount.Size = new System.Drawing.Size(51, 20);
            this.upDownGamesCount.TabIndex = 28;
            this.upDownGamesCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.upDownGamesCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.upDownGamesCount);
            this.groupBox1.Controls.Add(this.btStartGames);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.upDownThreads);
            this.groupBox1.Location = new System.Drawing.Point(12, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 106);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblTime);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.listPlayers);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.lblRunning);
            this.groupBox2.Controls.Add(this.lblGameTotalCount);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lblToRun);
            this.groupBox2.Controls.Add(this.lblFinished);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 112);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(496, 370);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(317, 25);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(49, 13);
            this.lblTime.TabIndex = 25;
            this.lblTime.Text = "00:00:00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(233, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Uběhlý čas";
            // 
            // TournamentDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(515, 528);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TournamentDialog";
            this.Text = "Turnaj";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TournamentDialog_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.upDownThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownGamesCount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btStartGames;
        private System.Windows.Forms.ListView listPlayers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader Skóre;
        private System.Windows.Forms.ColumnHeader Jméno;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblGameTotalCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblToRun;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblFinished;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblRunning;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown upDownThreads;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown upDownGamesCount;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label label7;
    }
}