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
            this._listBoxFiles = new System.Windows.Forms.ListBox();
            this._buttonOpen = new System.Windows.Forms.Button();
            this._buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _listBoxFiles
            // 
            this._listBoxFiles.FormattingEnabled = true;
            this._listBoxFiles.Location = new System.Drawing.Point(5, 10);
            this._listBoxFiles.Name = "_listBoxFiles";
            this._listBoxFiles.Size = new System.Drawing.Size(320, 199);
            this._listBoxFiles.TabIndex = 0;
            // 
            // _buttonOpen
            // 
            this._buttonOpen.Location = new System.Drawing.Point(112, 215);
            this._buttonOpen.Name = "_buttonOpen";
            this._buttonOpen.Size = new System.Drawing.Size(95, 23);
            this._buttonOpen.TabIndex = 1;
            this._buttonOpen.Text = "Open";
            this._buttonOpen.UseVisualStyleBackColor = true;
            this._buttonOpen.Click += new System.EventHandler(this._buttonOpen_Click);
            // 
            // _buttonCancel
            // 
            this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._buttonCancel.Location = new System.Drawing.Point(224, 215);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(97, 23);
            this._buttonCancel.TabIndex = 2;
            this._buttonCancel.Text = "Cancel";
            this._buttonCancel.UseVisualStyleBackColor = true;
            this._buttonCancel.Click += new System.EventHandler(this._buttonCancel_Click);
            // 
            // OpenReplayDialog
            // 
            this.AcceptButton = this._buttonOpen;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._buttonCancel;
            this.ClientSize = new System.Drawing.Size(333, 244);
            this.Controls.Add(this._buttonCancel);
            this.Controls.Add(this._buttonOpen);
            this.Controls.Add(this._listBoxFiles);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenReplayDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open Replay ";
            this.Load += new System.EventHandler(this.OpenReplayDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox _listBoxFiles;
        private System.Windows.Forms.Button _buttonOpen;
        private System.Windows.Forms.Button _buttonCancel;
    }
}