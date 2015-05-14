namespace SnakeDeathmatch.Debugger
{
    partial class DebuggerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebuggerForm));
            this.panelTop = new System.Windows.Forms.Panel();
            this._buttonGoToTheEnd = new System.Windows.Forms.Button();
            this._imageList = new System.Windows.Forms.ImageList(this.components);
            this._buttonGoTo2 = new System.Windows.Forms.Button();
            this._buttonGoTo1 = new System.Windows.Forms.Button();
            this._treeView = new System.Windows.Forms.TreeView();
            this._splitter = new System.Windows.Forms.Splitter();
            this._panelBody = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this._buttonGoToTheEnd);
            this.panelTop.Controls.Add(this._buttonGoTo2);
            this.panelTop.Controls.Add(this._buttonGoTo1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(649, 24);
            this.panelTop.TabIndex = 0;
            // 
            // _buttonGoToTheEnd
            // 
            this._buttonGoToTheEnd.FlatAppearance.BorderSize = 0;
            this._buttonGoToTheEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._buttonGoToTheEnd.ImageKey = "go-to-the-end.png";
            this._buttonGoToTheEnd.ImageList = this._imageList;
            this._buttonGoToTheEnd.Location = new System.Drawing.Point(50, 4);
            this._buttonGoToTheEnd.Name = "_buttonGoToTheEnd";
            this._buttonGoToTheEnd.Size = new System.Drawing.Size(16, 16);
            this._buttonGoToTheEnd.TabIndex = 2;
            this._buttonGoToTheEnd.UseVisualStyleBackColor = true;
            // 
            // _imageList
            // 
            this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
            this._imageList.TransparentColor = System.Drawing.Color.White;
            this._imageList.Images.SetKeyName(0, "go-to-1.png");
            this._imageList.Images.SetKeyName(1, "go-to-2.png");
            this._imageList.Images.SetKeyName(2, "go-to-the-end.png");
            // 
            // _buttonGoTo2
            // 
            this._buttonGoTo2.FlatAppearance.BorderSize = 0;
            this._buttonGoTo2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._buttonGoTo2.ImageKey = "go-to-2.png";
            this._buttonGoTo2.ImageList = this._imageList;
            this._buttonGoTo2.Location = new System.Drawing.Point(28, 4);
            this._buttonGoTo2.Name = "_buttonGoTo2";
            this._buttonGoTo2.Size = new System.Drawing.Size(16, 16);
            this._buttonGoTo2.TabIndex = 1;
            this._buttonGoTo2.UseVisualStyleBackColor = true;
            // 
            // _buttonGoTo1
            // 
            this._buttonGoTo1.FlatAppearance.BorderSize = 0;
            this._buttonGoTo1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._buttonGoTo1.ImageKey = "go-to-1.png";
            this._buttonGoTo1.ImageList = this._imageList;
            this._buttonGoTo1.Location = new System.Drawing.Point(6, 4);
            this._buttonGoTo1.Name = "_buttonGoTo1";
            this._buttonGoTo1.Size = new System.Drawing.Size(16, 16);
            this._buttonGoTo1.TabIndex = 0;
            this._buttonGoTo1.UseVisualStyleBackColor = true;
            // 
            // _treeView
            // 
            this._treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._treeView.Dock = System.Windows.Forms.DockStyle.Left;
            this._treeView.Location = new System.Drawing.Point(0, 24);
            this._treeView.Name = "_treeView";
            this._treeView.Size = new System.Drawing.Size(250, 357);
            this._treeView.TabIndex = 1;
            // 
            // _splitter
            // 
            this._splitter.Location = new System.Drawing.Point(250, 24);
            this._splitter.Name = "_splitter";
            this._splitter.Size = new System.Drawing.Size(3, 357);
            this._splitter.TabIndex = 2;
            this._splitter.TabStop = false;
            // 
            // _panelBody
            // 
            this._panelBody.AutoScroll = true;
            this._panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelBody.Location = new System.Drawing.Point(253, 24);
            this._panelBody.Name = "_panelBody";
            this._panelBody.Size = new System.Drawing.Size(396, 357);
            this._panelBody.TabIndex = 3;
            // 
            // DebuggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 381);
            this.Controls.Add(this._panelBody);
            this.Controls.Add(this._splitter);
            this.Controls.Add(this._treeView);
            this.Controls.Add(this.panelTop);
            this.Name = "DebuggerForm";
            this.Text = "Debugger";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DebuggerForm_FormClosed);
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button _buttonGoToTheEnd;
        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.Button _buttonGoTo2;
        private System.Windows.Forms.Button _buttonGoTo1;
        private System.Windows.Forms.TreeView _treeView;
        private System.Windows.Forms.Splitter _splitter;
        private System.Windows.Forms.Panel _panelBody;
    }
}