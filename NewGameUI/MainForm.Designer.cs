namespace NewGameUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._panelTop = new System.Windows.Forms.Panel();
            this._buttonDebugger = new System.Windows.Forms.Button();
            this._buttonStep = new System.Windows.Forms.Button();
            this._checkboxStepping = new System.Windows.Forms.CheckBox();
            this._buttonReplay = new System.Windows.Forms.Button();
            this._panelRound = new System.Windows.Forms.Panel();
            this._labelRPS = new System.Windows.Forms.Label();
            this._labelRoundCounter = new System.Windows.Forms.Label();
            this._labelRound = new System.Windows.Forms.Label();
            this._buttonTest = new System.Windows.Forms.Button();
            this._buttonRestart = new System.Windows.Forms.Button();
            this._timerUI = new System.Windows.Forms.Timer(this.components);
            this._panelArena = new System.Windows.Forms.PictureBox();
            this._panelTop.SuspendLayout();
            this._panelRound.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._panelArena)).BeginInit();
            this.SuspendLayout();
            // 
            // _panelTop
            // 
            this._panelTop.AutoSize = true;
            this._panelTop.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this._panelTop.Controls.Add(this._buttonDebugger);
            this._panelTop.Controls.Add(this._buttonStep);
            this._panelTop.Controls.Add(this._checkboxStepping);
            this._panelTop.Controls.Add(this._buttonReplay);
            this._panelTop.Controls.Add(this._panelRound);
            this._panelTop.Controls.Add(this._buttonTest);
            this._panelTop.Controls.Add(this._buttonRestart);
            this._panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelTop.Location = new System.Drawing.Point(0, 0);
            this._panelTop.Name = "_panelTop";
            this._panelTop.Size = new System.Drawing.Size(876, 36);
            this._panelTop.TabIndex = 2;
            // 
            // _buttonDebugger
            // 
            this._buttonDebugger.Location = new System.Drawing.Point(304, 6);
            this._buttonDebugger.Name = "_buttonDebugger";
            this._buttonDebugger.Size = new System.Drawing.Size(74, 23);
            this._buttonDebugger.TabIndex = 11;
            this._buttonDebugger.Text = "Debugger";
            this._buttonDebugger.UseVisualStyleBackColor = true;
            // 
            // _buttonStep
            // 
            this._buttonStep.Enabled = false;
            this._buttonStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._buttonStep.Image = ((System.Drawing.Image)(resources.GetObject("_buttonStep.Image")));
            this._buttonStep.Location = new System.Drawing.Point(384, 6);
            this._buttonStep.Name = "_buttonStep";
            this._buttonStep.Size = new System.Drawing.Size(26, 23);
            this._buttonStep.TabIndex = 10;
            this._buttonStep.UseVisualStyleBackColor = true;
            this._buttonStep.Click += new System.EventHandler(this._buttonStep_Click);
            // 
            // _checkboxStepping
            // 
            this._checkboxStepping.AutoSize = true;
            this._checkboxStepping.Location = new System.Drawing.Point(416, 10);
            this._checkboxStepping.Name = "_checkboxStepping";
            this._checkboxStepping.Size = new System.Drawing.Size(76, 17);
            this._checkboxStepping.TabIndex = 9;
            this._checkboxStepping.Text = "Krokování";
            this._checkboxStepping.UseVisualStyleBackColor = true;
            this._checkboxStepping.CheckedChanged += new System.EventHandler(this._checkboxStepping_CheckedChanged);
            // 
            // _buttonReplay
            // 
            this._buttonReplay.Location = new System.Drawing.Point(165, 6);
            this._buttonReplay.Name = "_buttonReplay";
            this._buttonReplay.Size = new System.Drawing.Size(133, 23);
            this._buttonReplay.TabIndex = 3;
            this._buttonReplay.Text = "Replay saved game";
            this._buttonReplay.UseVisualStyleBackColor = true;
            this._buttonReplay.Click += new System.EventHandler(this._buttonReplay_Click);
            // 
            // _panelRound
            // 
            this._panelRound.Controls.Add(this._labelRPS);
            this._panelRound.Controls.Add(this._labelRoundCounter);
            this._panelRound.Controls.Add(this._labelRound);
            this._panelRound.Location = new System.Drawing.Point(525, 0);
            this._panelRound.Name = "_panelRound";
            this._panelRound.Size = new System.Drawing.Size(277, 33);
            this._panelRound.TabIndex = 2;
            // 
            // _labelRPS
            // 
            this._labelRPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._labelRPS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this._labelRPS.Location = new System.Drawing.Point(158, 6);
            this._labelRPS.Name = "_labelRPS";
            this._labelRPS.Size = new System.Drawing.Size(116, 20);
            this._labelRPS.TabIndex = 5;
            this._labelRPS.Text = "0 RPS";
            this._labelRPS.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _labelRoundCounter
            // 
            this._labelRoundCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._labelRoundCounter.ForeColor = System.Drawing.Color.LimeGreen;
            this._labelRoundCounter.Location = new System.Drawing.Point(85, 6);
            this._labelRoundCounter.Name = "_labelRoundCounter";
            this._labelRoundCounter.Size = new System.Drawing.Size(67, 18);
            this._labelRoundCounter.TabIndex = 4;
            this._labelRoundCounter.Text = "000000000";
            this._labelRoundCounter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _labelRound
            // 
            this._labelRound.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._labelRound.ForeColor = System.Drawing.Color.LimeGreen;
            this._labelRound.Location = new System.Drawing.Point(3, 6);
            this._labelRound.Name = "_labelRound";
            this._labelRound.Size = new System.Drawing.Size(76, 18);
            this._labelRound.TabIndex = 3;
            this._labelRound.Text = "Round";
            // 
            // _buttonTest
            // 
            this._buttonTest.Location = new System.Drawing.Point(84, 6);
            this._buttonTest.Name = "_buttonTest";
            this._buttonTest.Size = new System.Drawing.Size(75, 23);
            this._buttonTest.TabIndex = 1;
            this._buttonTest.Text = "Test";
            this._buttonTest.UseVisualStyleBackColor = true;
            // 
            // _buttonRestart
            // 
            this._buttonRestart.Location = new System.Drawing.Point(3, 6);
            this._buttonRestart.Name = "_buttonRestart";
            this._buttonRestart.Size = new System.Drawing.Size(75, 23);
            this._buttonRestart.TabIndex = 0;
            this._buttonRestart.Text = "Restart";
            this._buttonRestart.UseVisualStyleBackColor = true;
            this._buttonRestart.Click += new System.EventHandler(this._buttonRestart_Click);
            // 
            // _timerUI
            // 
            this._timerUI.Tick += new System.EventHandler(this._timerUI_Tick);
            // 
            // _panelArena
            // 
            this._panelArena.BackColor = System.Drawing.Color.Black;
            this._panelArena.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelArena.Location = new System.Drawing.Point(0, 36);
            this._panelArena.Name = "_panelArena";
            this._panelArena.Size = new System.Drawing.Size(876, 640);
            this._panelArena.TabIndex = 7;
            this._panelArena.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(876, 676);
            this.Controls.Add(this._panelArena);
            this.Controls.Add(this._panelTop);
            this.Name = "MainForm";
            this.Text = "Snake Deathmatch (New UI)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this._panelTop.ResumeLayout(false);
            this._panelTop.PerformLayout();
            this._panelRound.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._panelArena)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _panelTop;
        private System.Windows.Forms.Button _buttonTest;
        private System.Windows.Forms.Button _buttonRestart;
        private System.Windows.Forms.Panel _panelRound;
        private System.Windows.Forms.Label _labelRoundCounter;
        private System.Windows.Forms.Label _labelRound;
        private System.Windows.Forms.Timer _timerUI;
        private System.Windows.Forms.Label _labelRPS;
        private System.Windows.Forms.Button _buttonReplay;
        private System.Windows.Forms.PictureBox _panelArena;
        private System.Windows.Forms.Button _buttonStep;
        private System.Windows.Forms.CheckBox _checkboxStepping;
        private System.Windows.Forms.Button _buttonDebugger;

    }
}

