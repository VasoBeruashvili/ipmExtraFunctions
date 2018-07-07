namespace ipmExtraFunctions
{
    partial class ProvidersMoveForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProvidersMoveForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.btnSaveInExcel = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnApportion = new System.Windows.Forms.ToolStripButton();
            this.treeProviders = new System.Windows.Forms.TreeView();
            this.m_TreeImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitPanelMoves = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mGridProducts = new System.Windows.Forms.DataGridView();
            this.mGridSavedMoves = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelVendor = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnContragents = new System.Windows.Forms.Button();
            this.txtContragent = new System.Windows.Forms.TextBox();
            this.toolStripMain.SuspendLayout();
            this.splitPanelMoves.Panel1.SuspendLayout();
            this.splitPanelMoves.Panel2.SuspendLayout();
            this.splitPanelMoves.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mGridProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mGridSavedMoves)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelVendor.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnClose,
            this.btnSaveInExcel,
            this.btnRefresh,
            this.btnApportion});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1024, 25);
            this.toolStripMain.TabIndex = 2;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // btnClose
            // 
            this.btnClose.Image = global::ipmExtraFunctions.Properties.Resources.exit_16;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(76, 22);
            this.btnClose.Text = "დახურვა";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSaveInExcel
            // 
            this.btnSaveInExcel.Image = global::ipmExtraFunctions.Properties.Resources.Excel_icon;
            this.btnSaveInExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveInExcel.Name = "btnSaveInExcel";
            this.btnSaveInExcel.Size = new System.Drawing.Size(114, 22);
            this.btnSaveInExcel.Text = "შენახვა ფაილში";
            this.btnSaveInExcel.Click += new System.EventHandler(this.btnSaveInExcel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::ipmExtraFunctions.Properties.Resources.refresh_16;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(84, 22);
            this.btnRefresh.Text = "განახლება";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnApportion
            // 
            this.btnApportion.Image = global::ipmExtraFunctions.Properties.Resources.flash_16;
            this.btnApportion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApportion.Name = "btnApportion";
            this.btnApportion.Size = new System.Drawing.Size(90, 22);
            this.btnApportion.Text = "განაწილება";
            this.btnApportion.Click += new System.EventHandler(this.btnApportion_Click);
            // 
            // treeProviders
            // 
            this.treeProviders.Cursor = System.Windows.Forms.Cursors.Hand;
            this.treeProviders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeProviders.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeProviders.HotTracking = true;
            this.treeProviders.ImageIndex = 0;
            this.treeProviders.ImageList = this.m_TreeImageList;
            this.treeProviders.Location = new System.Drawing.Point(0, 0);
            this.treeProviders.Margin = new System.Windows.Forms.Padding(4);
            this.treeProviders.Name = "treeProviders";
            this.treeProviders.SelectedImageIndex = 0;
            this.treeProviders.Size = new System.Drawing.Size(260, 448);
            this.treeProviders.TabIndex = 3;
            this.treeProviders.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeProviders_NodeMouseClick);
            // 
            // m_TreeImageList
            // 
            this.m_TreeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_TreeImageList.ImageStream")));
            this.m_TreeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.m_TreeImageList.Images.SetKeyName(0, "folder.png");
            this.m_TreeImageList.Images.SetKeyName(1, "folder_closed.png");
            // 
            // splitPanelMoves
            // 
            this.splitPanelMoves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPanelMoves.Location = new System.Drawing.Point(0, 0);
            this.splitPanelMoves.Name = "splitPanelMoves";
            this.splitPanelMoves.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitPanelMoves.Panel1
            // 
            this.splitPanelMoves.Panel1.AutoScroll = true;
            this.splitPanelMoves.Panel1.Controls.Add(this.panel1);
            this.splitPanelMoves.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitPanelMoves.Panel2
            // 
            this.splitPanelMoves.Panel2.Controls.Add(this.mGridSavedMoves);
            this.splitPanelMoves.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitPanelMoves.Panel2Collapsed = true;
            this.splitPanelMoves.Size = new System.Drawing.Size(761, 448);
            this.splitPanelMoves.SplitterDistance = 350;
            this.splitPanelMoves.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.mGridProducts);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(761, 448);
            this.panel1.TabIndex = 1;
            // 
            // mGridProducts
            // 
            this.mGridProducts.AllowUserToAddRows = false;
            this.mGridProducts.AllowUserToDeleteRows = false;
            this.mGridProducts.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.mGridProducts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.mGridProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.mGridProducts.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.mGridProducts.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mGridProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.mGridProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.mGridProducts.DefaultCellStyle = dataGridViewCellStyle3;
            this.mGridProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mGridProducts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.mGridProducts.Location = new System.Drawing.Point(0, 0);
            this.mGridProducts.Margin = new System.Windows.Forms.Padding(4);
            this.mGridProducts.MultiSelect = false;
            this.mGridProducts.Name = "mGridProducts";
            this.mGridProducts.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mGridProducts.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.mGridProducts.RowHeadersVisible = false;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.mGridProducts.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.mGridProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mGridProducts.Size = new System.Drawing.Size(761, 448);
            this.mGridProducts.TabIndex = 0;
            this.mGridProducts.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.mGridProducts_CellEnter);
            // 
            // mGridSavedMoves
            // 
            this.mGridSavedMoves.AllowUserToAddRows = false;
            this.mGridSavedMoves.AllowUserToDeleteRows = false;
            this.mGridSavedMoves.AllowUserToResizeRows = false;
            this.mGridSavedMoves.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.mGridSavedMoves.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.mGridSavedMoves.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mGridSavedMoves.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.mGridSavedMoves.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.mGridSavedMoves.DefaultCellStyle = dataGridViewCellStyle7;
            this.mGridSavedMoves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mGridSavedMoves.GridColor = System.Drawing.SystemColors.Window;
            this.mGridSavedMoves.Location = new System.Drawing.Point(0, 0);
            this.mGridSavedMoves.MultiSelect = false;
            this.mGridSavedMoves.Name = "mGridSavedMoves";
            this.mGridSavedMoves.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mGridSavedMoves.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.mGridSavedMoves.RowHeadersVisible = false;
            this.mGridSavedMoves.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mGridSavedMoves.Size = new System.Drawing.Size(150, 46);
            this.mGridSavedMoves.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 63);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeProviders);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitPanelMoves);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(1024, 448);
            this.splitContainer1.SplitterDistance = 260;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 5;
            // 
            // panelVendor
            // 
            this.panelVendor.Controls.Add(this.panel3);
            this.panelVendor.Controls.Add(this.btnContragents);
            this.panelVendor.Controls.Add(this.txtContragent);
            this.panelVendor.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelVendor.Location = new System.Drawing.Point(0, 25);
            this.panelVendor.Name = "panelVendor";
            this.panelVendor.Size = new System.Drawing.Size(1024, 38);
            this.panelVendor.TabIndex = 6;
            this.panelVendor.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(5, 8);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(104, 23);
            this.panel3.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(2, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 18);
            this.label3.TabIndex = 18;
            this.label3.Text = "მომწოდებელი:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnContragents
            // 
            this.btnContragents.Image = global::ipmExtraFunctions.Properties.Resources.vendor_16;
            this.btnContragents.Location = new System.Drawing.Point(108, 7);
            this.btnContragents.Name = "btnContragents";
            this.btnContragents.Size = new System.Drawing.Size(24, 24);
            this.btnContragents.TabIndex = 6;
            this.btnContragents.UseVisualStyleBackColor = true;
            this.btnContragents.Click += new System.EventHandler(this.btnContragents_Click);
            // 
            // txtContragent
            // 
            this.txtContragent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtContragent.Location = new System.Drawing.Point(138, 7);
            this.txtContragent.Name = "txtContragent";
            this.txtContragent.ReadOnly = true;
            this.txtContragent.Size = new System.Drawing.Size(399, 24);
            this.txtContragent.TabIndex = 5;
            this.txtContragent.Tag = "მომწოდებელი";
            // 
            // ProvidersMoveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 511);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelVendor);
            this.Controls.Add(this.toolStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProvidersMoveForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "საწყობებში გადანაწილება";
            this.Load += new System.EventHandler(this.ProvidersMoveForm_Load);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.splitPanelMoves.Panel1.ResumeLayout(false);
            this.splitPanelMoves.Panel2.ResumeLayout(false);
            this.splitPanelMoves.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mGridProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mGridSavedMoves)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panelVendor.ResumeLayout(false);
            this.panelVendor.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.ToolStripButton btnSaveInExcel;
        private System.Windows.Forms.ToolStripButton btnApportion;
        private System.Windows.Forms.TreeView treeProviders;
        private System.Windows.Forms.SplitContainer splitPanelMoves;
        private System.Windows.Forms.DataGridView mGridProducts;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.ImageList m_TreeImageList;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView mGridSavedMoves;
        private System.Windows.Forms.Panel panelVendor;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnContragents;
        private System.Windows.Forms.TextBox txtContragent;
    }
}