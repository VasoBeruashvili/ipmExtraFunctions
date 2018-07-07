namespace ipmExtraFunctions
{
    partial class ConstructionTemplateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConstructionTemplateForm));
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.btnSaveTemplate = new System.Windows.Forms.ToolStripButton();
            this.btnSaveAndNew = new System.Windows.Forms.ToolStripButton();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.txtName = new System.Windows.Forms.TextBox();
            this.pnlName = new System.Windows.Forms.Panel();
            this.lblName = new System.Windows.Forms.Label();
            this.m_GridGroupProducts = new System.Windows.Forms.DataGridView();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.toolStripTemplate = new System.Windows.Forms.ToolStrip();
            this.btnAddTemplate = new System.Windows.Forms.ToolStripButton();
            this.btnEditTemplate = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteTemplate = new System.Windows.Forms.ToolStripButton();
            this.ColID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColProducGrouptID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColProductGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripMain.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_GridGroupProducts)).BeginInit();
            this.pnlGrid.SuspendLayout();
            this.toolStripTemplate.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnClose,
            this.btnSaveTemplate,
            this.btnSaveAndNew});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(415, 25);
            this.toolStripMain.TabIndex = 0;
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
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Image = global::ipmExtraFunctions.Properties.Resources.save_16;
            this.btnSaveTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(68, 22);
            this.btnSaveTemplate.Text = "შენახვა";
            this.btnSaveTemplate.Click += new System.EventHandler(this.btnSaveTemplate_Click);
            // 
            // btnSaveAndNew
            // 
            this.btnSaveAndNew.Image = global::ipmExtraFunctions.Properties.Resources.save_and_new_16;
            this.btnSaveAndNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAndNew.Name = "btnSaveAndNew";
            this.btnSaveAndNew.Size = new System.Drawing.Size(123, 22);
            this.btnSaveAndNew.Text = "შენახვა და ახალი";
            this.btnSaveAndNew.Click += new System.EventHandler(this.btnSaveAndNew_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.txtName);
            this.pnlTop.Controls.Add(this.pnlName);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 25);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(415, 37);
            this.pnlTop.TabIndex = 1;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtName.Location = new System.Drawing.Point(113, 5);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(271, 24);
            this.txtName.TabIndex = 11;
            this.txtName.Tag = "დასახელება";
            // 
            // pnlName
            // 
            this.pnlName.Controls.Add(this.lblName);
            this.pnlName.Location = new System.Drawing.Point(11, 6);
            this.pnlName.Name = "pnlName";
            this.pnlName.Size = new System.Drawing.Size(96, 24);
            this.pnlName.TabIndex = 9;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblName.Location = new System.Drawing.Point(8, 0);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(88, 18);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "დასახელება:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_GridGroupProducts
            // 
            this.m_GridGroupProducts.AllowUserToAddRows = false;
            this.m_GridGroupProducts.AllowUserToDeleteRows = false;
            this.m_GridGroupProducts.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_GridGroupProducts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.m_GridGroupProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_GridGroupProducts.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.m_GridGroupProducts.BackgroundColor = System.Drawing.SystemColors.Window;
            this.m_GridGroupProducts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_GridGroupProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.m_GridGroupProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_GridGroupProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColID,
            this.ColProducGrouptID,
            this.ColProductGroupName});
            this.m_GridGroupProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_GridGroupProducts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.m_GridGroupProducts.GridColor = System.Drawing.Color.White;
            this.m_GridGroupProducts.Location = new System.Drawing.Point(0, 25);
            this.m_GridGroupProducts.Margin = new System.Windows.Forms.Padding(13, 5, 5, 5);
            this.m_GridGroupProducts.MultiSelect = false;
            this.m_GridGroupProducts.Name = "m_GridGroupProducts";
            this.m_GridGroupProducts.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_GridGroupProducts.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.m_GridGroupProducts.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_GridGroupProducts.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.m_GridGroupProducts.RowTemplate.Height = 26;
            this.m_GridGroupProducts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_GridGroupProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_GridGroupProducts.Size = new System.Drawing.Size(415, 403);
            this.m_GridGroupProducts.TabIndex = 2;
            this.m_GridGroupProducts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.m_GridGroupProducts_MouseDoubleClick);
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.m_GridGroupProducts);
            this.pnlGrid.Controls.Add(this.toolStripTemplate);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlGrid.Location = new System.Drawing.Point(0, 62);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(415, 428);
            this.pnlGrid.TabIndex = 3;
            // 
            // toolStripTemplate
            // 
            this.toolStripTemplate.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripTemplate.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTemplate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddTemplate,
            this.btnEditTemplate,
            this.btnDeleteTemplate});
            this.toolStripTemplate.Location = new System.Drawing.Point(0, 0);
            this.toolStripTemplate.Name = "toolStripTemplate";
            this.toolStripTemplate.Size = new System.Drawing.Size(415, 25);
            this.toolStripTemplate.TabIndex = 3;
            this.toolStripTemplate.Text = "toolStrip1";
            // 
            // btnAddTemplate
            // 
            this.btnAddTemplate.Image = global::ipmExtraFunctions.Properties.Resources.add_16;
            this.btnAddTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddTemplate.Name = "btnAddTemplate";
            this.btnAddTemplate.Size = new System.Drawing.Size(78, 22);
            this.btnAddTemplate.Text = "დამატება";
            this.btnAddTemplate.Click += new System.EventHandler(this.btnAddTemplate_Click);
            // 
            // btnEditTemplate
            // 
            this.btnEditTemplate.Image = global::ipmExtraFunctions.Properties.Resources.edit_16;
            this.btnEditTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditTemplate.Name = "btnEditTemplate";
            this.btnEditTemplate.Size = new System.Drawing.Size(102, 22);
            this.btnEditTemplate.Text = "რედაქტირება";
            this.btnEditTemplate.Click += new System.EventHandler(this.btnEditTemplate_Click);
            // 
            // btnDeleteTemplate
            // 
            this.btnDeleteTemplate.Image = global::ipmExtraFunctions.Properties.Resources.delete_16;
            this.btnDeleteTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteTemplate.Name = "btnDeleteTemplate";
            this.btnDeleteTemplate.Size = new System.Drawing.Size(61, 22);
            this.btnDeleteTemplate.Text = "წაშლა";
            this.btnDeleteTemplate.Click += new System.EventHandler(this.btnDeleteTemplate_Click);
            // 
            // ColID
            // 
            this.ColID.HeaderText = "ColID";
            this.ColID.Name = "ColID";
            this.ColID.ReadOnly = true;
            this.ColID.Visible = false;
            // 
            // ColProducGrouptID
            // 
            this.ColProducGrouptID.HeaderText = "ColProducGrouptID";
            this.ColProducGrouptID.Name = "ColProducGrouptID";
            this.ColProducGrouptID.ReadOnly = true;
            this.ColProducGrouptID.Visible = false;
            // 
            // ColProductGroupName
            // 
            this.ColProductGroupName.HeaderText = "პროდუქტების ჯგუფი";
            this.ColProductGroupName.Name = "ColProductGroupName";
            this.ColProductGroupName.ReadOnly = true;
            // 
            // ConstructionTemplateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 490);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.toolStripMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ConstructionTemplateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "შაბლონი";
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlName.ResumeLayout(false);
            this.pnlName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_GridGroupProducts)).EndInit();
            this.pnlGrid.ResumeLayout(false);
            this.pnlGrid.PerformLayout();
            this.toolStripTemplate.ResumeLayout(false);
            this.toolStripTemplate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.ToolStripButton btnSaveTemplate;
        private System.Windows.Forms.Panel pnlName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        public System.Windows.Forms.DataGridView m_GridGroupProducts;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.ToolStrip toolStripTemplate;
        private System.Windows.Forms.ToolStripButton btnDeleteTemplate;
        private System.Windows.Forms.ToolStripButton btnAddTemplate;
        private System.Windows.Forms.ToolStripButton btnEditTemplate;
        private System.Windows.Forms.ToolStripButton btnSaveAndNew;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColProducGrouptID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColProductGroupName;
    }
}