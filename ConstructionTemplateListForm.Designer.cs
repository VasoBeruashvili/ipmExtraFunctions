namespace ipmExtraFunctions
{
    partial class ConstructionTemplateListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConstructionTemplateListForm));
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.btnChoose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddTemplate = new System.Windows.Forms.ToolStripButton();
            this.btnEditTemplate = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteTemplate = new System.Windows.Forms.ToolStripButton();
            this.m_GridTemplates = new System.Windows.Forms.DataGridView();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.ColTemplateID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTemplateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_GridTemplates)).BeginInit();
            this.pnlGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnClose,
            this.btnChoose,
            this.toolStripSeparator1,
            this.btnAddTemplate,
            this.btnEditTemplate,
            this.btnDeleteTemplate});
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
            // btnChoose
            // 
            this.btnChoose.Image = global::ipmExtraFunctions.Properties.Resources.select_16;
            this.btnChoose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(65, 22);
            this.btnChoose.Text = "არჩევა";
            this.btnChoose.Click += new System.EventHandler(this.btnChooseTemplate_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            // m_GridTemplates
            // 
            this.m_GridTemplates.AllowUserToAddRows = false;
            this.m_GridTemplates.AllowUserToDeleteRows = false;
            this.m_GridTemplates.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_GridTemplates.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.m_GridTemplates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_GridTemplates.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.m_GridTemplates.BackgroundColor = System.Drawing.SystemColors.Window;
            this.m_GridTemplates.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_GridTemplates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.m_GridTemplates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_GridTemplates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColTemplateID,
            this.ColTemplateName});
            this.m_GridTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_GridTemplates.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.m_GridTemplates.GridColor = System.Drawing.Color.White;
            this.m_GridTemplates.Location = new System.Drawing.Point(0, 0);
            this.m_GridTemplates.Margin = new System.Windows.Forms.Padding(13, 5, 5, 5);
            this.m_GridTemplates.MultiSelect = false;
            this.m_GridTemplates.Name = "m_GridTemplates";
            this.m_GridTemplates.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_GridTemplates.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.m_GridTemplates.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_GridTemplates.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.m_GridTemplates.RowTemplate.Height = 26;
            this.m_GridTemplates.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_GridTemplates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_GridTemplates.Size = new System.Drawing.Size(415, 465);
            this.m_GridTemplates.TabIndex = 2;
            this.m_GridTemplates.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.m_GridGroupProducts_MouseDoubleClick);
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.m_GridTemplates);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlGrid.Location = new System.Drawing.Point(0, 25);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(415, 465);
            this.pnlGrid.TabIndex = 3;
            // 
            // ColTemplateID
            // 
            this.ColTemplateID.HeaderText = "ColTemplateID";
            this.ColTemplateID.Name = "ColTemplateID";
            this.ColTemplateID.ReadOnly = true;
            this.ColTemplateID.Visible = false;
            // 
            // ColTemplateName
            // 
            this.ColTemplateName.HeaderText = "დასახელება";
            this.ColTemplateName.Name = "ColTemplateName";
            this.ColTemplateName.ReadOnly = true;
            // 
            // ConstructionTemplateListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 490);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.toolStripMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ConstructionTemplateListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "შაბლონები";
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_GridTemplates)).EndInit();
            this.pnlGrid.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.ToolStripButton btnChoose;
        public System.Windows.Forms.DataGridView m_GridTemplates;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnAddTemplate;
        private System.Windows.Forms.ToolStripButton btnEditTemplate;
        private System.Windows.Forms.ToolStripButton btnDeleteTemplate;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTemplateID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTemplateName;
    }
}