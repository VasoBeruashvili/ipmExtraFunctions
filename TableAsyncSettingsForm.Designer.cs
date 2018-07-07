namespace ipmExtraFunctions
{
    partial class TableAsyncSettingsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableAsyncSettingsForm));
            this.comboBoxTables = new System.Windows.Forms.ComboBox();
            this.btnRefreshGrid = new System.Windows.Forms.Button();
            this.grdData = new System.Windows.Forms.DataGridView();
            this.col_id = new ipmControls.DataGridViewIntegerColumn();
            this.col_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_sync_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_table_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkAllData = new System.Windows.Forms.CheckBox();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.btnSaveTemplate = new System.Windows.Forms.ToolStripButton();
            this.toolStripAutogenerate = new System.Windows.Forms.ToolStripButton();
            this.toolStripExport = new System.Windows.Forms.ToolStripButton();
            this.topPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.toolStripMain.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxTables
            // 
            this.comboBoxTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTables.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.comboBoxTables.FormattingEnabled = true;
            this.comboBoxTables.Location = new System.Drawing.Point(9, 3);
            this.comboBoxTables.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxTables.Name = "comboBoxTables";
            this.comboBoxTables.Size = new System.Drawing.Size(319, 26);
            this.comboBoxTables.TabIndex = 4;
            this.comboBoxTables.Tag = "";
            // 
            // btnRefreshGrid
            // 
            this.btnRefreshGrid.Image = global::ipmExtraFunctions.Properties.Resources.refresh_16;
            this.btnRefreshGrid.Location = new System.Drawing.Point(333, 3);
            this.btnRefreshGrid.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefreshGrid.Name = "btnRefreshGrid";
            this.btnRefreshGrid.Size = new System.Drawing.Size(42, 26);
            this.btnRefreshGrid.TabIndex = 5;
            this.btnRefreshGrid.UseVisualStyleBackColor = true;
            this.btnRefreshGrid.Click += new System.EventHandler(this.btnRefreshGrid_Click);
            // 
            // grdData
            // 
            this.grdData.AllowUserToAddRows = false;
            this.grdData.AllowUserToDeleteRows = false;
            this.grdData.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.grdData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdData.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.MediumBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_id,
            this.col_name,
            this.col_code,
            this.col_sync_id,
            this.col_table_name});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdData.DefaultCellStyle = dataGridViewCellStyle3;
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grdData.GridColor = System.Drawing.Color.LightSkyBlue;
            this.grdData.Location = new System.Drawing.Point(0, 0);
            this.grdData.Margin = new System.Windows.Forms.Padding(4);
            this.grdData.Name = "grdData";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.MediumBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdData.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.grdData.RowHeadersVisible = false;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.grdData.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.grdData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdData.ShowCellErrors = false;
            this.grdData.ShowCellToolTips = false;
            this.grdData.ShowEditingIcon = false;
            this.grdData.ShowRowErrors = false;
            this.grdData.Size = new System.Drawing.Size(984, 553);
            this.grdData.TabIndex = 6;
            this.grdData.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.grdData_CellValidating);
            // 
            // col_id
            // 
            this.col_id.HeaderText = "ID";
            this.col_id.Name = "col_id";
            this.col_id.ReadOnly = true;
            this.col_id.Visible = false;
            // 
            // col_name
            // 
            this.col_name.HeaderText = "სახელი";
            this.col_name.Name = "col_name";
            this.col_name.ReadOnly = true;
            // 
            // col_code
            // 
            this.col_code.HeaderText = "კოდი";
            this.col_code.Name = "col_code";
            this.col_code.ReadOnly = true;
            // 
            // col_sync_id
            // 
            this.col_sync_id.HeaderText = "იდენტიფიკატორი";
            this.col_sync_id.Name = "col_sync_id";
            this.col_sync_id.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // col_table_name
            // 
            this.col_table_name.HeaderText = "table_name";
            this.col_table_name.Name = "col_table_name";
            this.col_table_name.ReadOnly = true;
            this.col_table_name.Visible = false;
            // 
            // checkAllData
            // 
            this.checkAllData.AutoSize = true;
            this.checkAllData.Location = new System.Drawing.Point(383, 7);
            this.checkAllData.Margin = new System.Windows.Forms.Padding(4);
            this.checkAllData.Name = "checkAllData";
            this.checkAllData.Size = new System.Drawing.Size(131, 22);
            this.checkAllData.TabIndex = 9;
            this.checkAllData.Text = "ყველა ჩანაწერი";
            this.checkAllData.UseVisualStyleBackColor = true;
            // 
            // toolStripMain
            // 
            this.toolStripMain.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSaveTemplate,
            this.toolStripAutogenerate,
            this.toolStripExport});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripMain.Size = new System.Drawing.Size(984, 25);
            this.toolStripMain.TabIndex = 11;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Image = global::ipmExtraFunctions.Properties.Resources.save_16;
            this.btnSaveTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(69, 22);
            this.btnSaveTemplate.Text = "შენახვა";
            this.btnSaveTemplate.Click += new System.EventHandler(this.btnSaveTemplate_Click);
            // 
            // toolStripAutogenerate
            // 
            this.toolStripAutogenerate.Image = global::ipmExtraFunctions.Properties.Resources.syncronization_16;
            this.toolStripAutogenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAutogenerate.Name = "toolStripAutogenerate";
            this.toolStripAutogenerate.Size = new System.Drawing.Size(142, 22);
            this.toolStripAutogenerate.Text = "ავტომატური შევსება";
            this.toolStripAutogenerate.Click += new System.EventHandler(this.btnAutoGenerate_Click);
            // 
            // toolStripExport
            // 
            this.toolStripExport.Image = global::ipmExtraFunctions.Properties.Resources.Excel_icon;
            this.toolStripExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripExport.Name = "toolStripExport";
            this.toolStripExport.Size = new System.Drawing.Size(82, 22);
            this.toolStripExport.Text = "ექსპორტი";
            this.toolStripExport.Click += new System.EventHandler(this.toolStripExport_Click);
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.comboBoxTables);
            this.topPanel.Controls.Add(this.btnRefreshGrid);
            this.topPanel.Controls.Add(this.checkAllData);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 25);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(984, 35);
            this.topPanel.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grdData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 553);
            this.panel1.TabIndex = 13;
            // 
            // TableAsyncSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 613);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.toolStripMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TableAsyncSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "გარე ბაზასთან კავშირის გაწყობა";
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBoxTables;
        private System.Windows.Forms.Button btnRefreshGrid;
        private System.Windows.Forms.CheckBox checkAllData;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton btnSaveTemplate;
        private System.Windows.Forms.ToolStripButton toolStripAutogenerate;
        private System.Windows.Forms.ToolStripButton toolStripExport;
        private ipmControls.DataGridViewIntegerColumn col_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_sync_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_table_name;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.DataGridView grdData;
    }
}