namespace ipmExtraFunctions
{
    partial class ProvidersForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProvidersForm));
            this.pnlProviders = new System.Windows.Forms.SplitContainer();
            this.m_GridProducts = new System.Windows.Forms.DataGridView();
            this.mGridSavedOrders = new System.Windows.Forms.DataGridView();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.btnSaveInExcel = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnApportion = new System.Windows.Forms.ToolStripButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtContragent = new System.Windows.Forms.TextBox();
            this.btnContragents = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlProviders.Panel1.SuspendLayout();
            this.pnlProviders.Panel2.SuspendLayout();
            this.pnlProviders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_GridProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mGridSavedOrders)).BeginInit();
            this.toolStripMain.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlProviders
            // 
            this.pnlProviders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProviders.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.pnlProviders.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlProviders.Location = new System.Drawing.Point(0, 63);
            this.pnlProviders.Margin = new System.Windows.Forms.Padding(4);
            this.pnlProviders.Name = "pnlProviders";
            this.pnlProviders.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // pnlProviders.Panel1
            // 
            this.pnlProviders.Panel1.Controls.Add(this.m_GridProducts);
            // 
            // pnlProviders.Panel2
            // 
            this.pnlProviders.Panel2.Controls.Add(this.mGridSavedOrders);
            this.pnlProviders.Panel2Collapsed = true;
            this.pnlProviders.Panel2MinSize = 20;
            this.pnlProviders.Size = new System.Drawing.Size(920, 448);
            this.pnlProviders.SplitterDistance = 310;
            this.pnlProviders.SplitterWidth = 5;
            this.pnlProviders.TabIndex = 0;
            // 
            // m_GridProducts
            // 
            this.m_GridProducts.AllowUserToAddRows = false;
            this.m_GridProducts.AllowUserToDeleteRows = false;
            this.m_GridProducts.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_GridProducts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.m_GridProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.m_GridProducts.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.m_GridProducts.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_GridProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.m_GridProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_GridProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_GridProducts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.m_GridProducts.GridColor = System.Drawing.Color.White;
            this.m_GridProducts.Location = new System.Drawing.Point(0, 0);
            this.m_GridProducts.Margin = new System.Windows.Forms.Padding(13, 5, 5, 5);
            this.m_GridProducts.MultiSelect = false;
            this.m_GridProducts.Name = "m_GridProducts";
            this.m_GridProducts.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_GridProducts.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.m_GridProducts.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_GridProducts.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.m_GridProducts.RowTemplate.Height = 26;
            this.m_GridProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_GridProducts.Size = new System.Drawing.Size(920, 448);
            this.m_GridProducts.TabIndex = 0;
            this.m_GridProducts.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_GridProducts_CellEnter);
            // 
            // mGridSavedOrders
            // 
            this.mGridSavedOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mGridSavedOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mGridSavedOrders.Location = new System.Drawing.Point(0, 0);
            this.mGridSavedOrders.Name = "mGridSavedOrders";
            this.mGridSavedOrders.Size = new System.Drawing.Size(150, 46);
            this.mGridSavedOrders.TabIndex = 0;
            // 
            // toolStripMain
            // 
            this.toolStripMain.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnClose,
            this.btnSaveInExcel,
            this.btnRefresh,
            this.toolStripSeparator1,
            this.btnApportion});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(920, 25);
            this.toolStripMain.TabIndex = 1;
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.btnContragents);
            this.panel1.Controls.Add(this.txtContragent);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(920, 38);
            this.panel1.TabIndex = 2;
            // 
            // ProvidersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 511);
            this.Controls.Add(this.pnlProviders);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStripMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ProvidersForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "გაფართოებული შეკვეთა მომწოდებლებთან";
            this.Load += new System.EventHandler(this.ProvidersForm_Load);
            this.pnlProviders.Panel1.ResumeLayout(false);
            this.pnlProviders.Panel2.ResumeLayout(false);
            this.pnlProviders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_GridProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mGridSavedOrders)).EndInit();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer pnlProviders;
        public System.Windows.Forms.DataGridView m_GridProducts;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.ToolStripButton btnSaveInExcel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnApportion;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtContragent;
        private System.Windows.Forms.Button btnContragents;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView mGridSavedOrders;
    }
}