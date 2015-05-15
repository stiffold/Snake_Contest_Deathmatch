namespace NewGameUI.Dialogs
{
    partial class OpenReplayDialog
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
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.btOpen = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstFiles
            // 
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.Location = new System.Drawing.Point(5, 10);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(320, 199);
            this.lstFiles.TabIndex = 0;
            // 
            // btOpen
            // 
            this.btOpen.Location = new System.Drawing.Point(112, 215);
            this.btOpen.Name = "btOpen";
            this.btOpen.Size = new System.Drawing.Size(95, 23);
            this.btOpen.TabIndex = 1;
            this.btOpen.Text = "Vybrat";
            this.btOpen.UseVisualStyleBackColor = true;
            this.btOpen.Click += new System.EventHandler(this.btOpen_Click);
            // 
            // btClose
            // 
            this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btClose.Location = new System.Drawing.Point(224, 215);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(97, 23);
            this.btClose.TabIndex = 2;
            this.btClose.Text = "Zavřít";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // OpenReplayDialog
            // 
            this.AcceptButton = this.btOpen;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btClose;
            this.ClientSize = new System.Drawing.Size(333, 244);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btOpen);
            this.Controls.Add(this.lstFiles);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenReplayDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open Replay ";
            this.Load += new System.EventHandler(this.OpenReplayDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Button btOpen;
        private System.Windows.Forms.Button btClose;
    }
}