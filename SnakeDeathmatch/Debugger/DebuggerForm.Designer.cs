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
            this._comboBoxBreakpoint = new System.Windows.Forms.ComboBox();
            this._buttonGoToTheEnd = new System.Windows.Forms.Button();
            this._imageList = new System.Windows.Forms.ImageList(this.components);
            this._treeView = new System.Windows.Forms.TreeView();
            this._splitter = new System.Windows.Forms.Splitter();
            this._panelControls = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this._comboBoxBreakpoint);
            this.panelTop.Controls.Add(this._buttonGoToTheEnd);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(649, 25);
            this.panelTop.TabIndex = 0;
            // 
            // _comboBoxBreakpoint
            // 
            this._comboBoxBreakpoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBoxBreakpoint.FormattingEnabled = true;
            this._comboBoxBreakpoint.Location = new System.Drawing.Point(2, 2);
            this._comboBoxBreakpoint.Name = "_comboBoxBreakpoint";
            this._comboBoxBreakpoint.Size = new System.Drawing.Size(218, 21);
            this._comboBoxBreakpoint.Sorted = true;
            this._comboBoxBreakpoint.TabIndex = 3;
            this._comboBoxBreakpoint.SelectedValueChanged += new System.EventHandler(this._comboBoxBreakpoint_SelectedValueChanged);
            // 
            // _buttonGoToTheEnd
            // 
            this._buttonGoToTheEnd.FlatAppearance.BorderSize = 0;
            this._buttonGoToTheEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._buttonGoToTheEnd.ImageKey = "continue.png";
            this._buttonGoToTheEnd.ImageList = this._imageList;
            this._buttonGoToTheEnd.Location = new System.Drawing.Point(223, 3);
            this._buttonGoToTheEnd.Margin = new System.Windows.Forms.Padding(0);
            this._buttonGoToTheEnd.Name = "_buttonGoToTheEnd";
            this._buttonGoToTheEnd.Size = new System.Drawing.Size(18, 18);
            this._buttonGoToTheEnd.TabIndex = 2;
            this._buttonGoToTheEnd.UseVisualStyleBackColor = true;
            this._buttonGoToTheEnd.Click += new System.EventHandler(this._buttonGoToTheEnd_Click);
            // 
            // _imageList
            // 
            this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
            this._imageList.TransparentColor = System.Drawing.Color.White;
            this._imageList.Images.SetKeyName(0, "continue.png");
            this._imageList.Images.SetKeyName(1, "visualizer.png");
            // 
            // _treeView
            // 
            this._treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._treeView.CheckBoxes = true;
            this._treeView.Dock = System.Windows.Forms.DockStyle.Left;
            this._treeView.Location = new System.Drawing.Point(0, 25);
            this._treeView.Name = "_treeView";
            this._treeView.Size = new System.Drawing.Size(250, 356);
            this._treeView.TabIndex = 1;
            this._treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this._treeView_AfterCheck);
            this._treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._treeView_AfterSelect);
            // 
            // _splitter
            // 
            this._splitter.Location = new System.Drawing.Point(250, 25);
            this._splitter.Name = "_splitter";
            this._splitter.Size = new System.Drawing.Size(3, 356);
            this._splitter.TabIndex = 2;
            this._splitter.TabStop = false;
            // 
            // _panelControls
            // 
            this._panelControls.AutoScroll = true;
            this._panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelControls.Location = new System.Drawing.Point(253, 25);
            this._panelControls.Name = "_panelControls";
            this._panelControls.Size = new System.Drawing.Size(396, 356);
            this._panelControls.TabIndex = 6;
            // 
            // DebuggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 381);
            this.Controls.Add(this._panelControls);
            this.Controls.Add(this._splitter);
            this.Controls.Add(this._treeView);
            this.Controls.Add(this.panelTop);
            this.Name = "DebuggerForm";
            this.Text = "Debugger";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DebuggerForm_FormClosed);
            this.Load += new System.EventHandler(this.DebuggerForm_Load);
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button _buttonGoToTheEnd;
        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.TreeView _treeView;
        private System.Windows.Forms.Splitter _splitter;
        private System.Windows.Forms.ComboBox _comboBoxBreakpoint;
        private System.Windows.Forms.Panel _panelControls;
    }
}