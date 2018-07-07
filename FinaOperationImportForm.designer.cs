namespace ipmExtraFunctions
{
    partial class FinaOperationImportForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinaOperationImportForm));
            this.panel3 = new System.Windows.Forms.Panel();
            this.m_Toolbar = new System.Windows.Forms.ToolStrip();
            this.btnBrowse = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExecute = new System.Windows.Forms.ToolStripButton();
            this.m_Grid = new System.Windows.Forms.DataGridView();
            this.ColCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColTdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColOperation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPurpose = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStatusId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColUid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColInside = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuImport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectIem = new System.Windows.Forms.ToolStripMenuItem();
            this.unselectItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lblResult = new System.Windows.Forms.Label();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExport = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnPreviewToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExportToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3.SuspendLayout();
            this.m_Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_Grid)).BeginInit();
            this.menuImport.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.m_Toolbar);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1264, 46);
            this.panel3.TabIndex = 28;
            // 
            // m_Toolbar
            // 
            this.m_Toolbar.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_Toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.m_Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBrowse,
            this.toolStripSeparator1,
            this.btnExecute,
            this.toolStripSeparator2,
            this.btnExport});
            this.m_Toolbar.Location = new System.Drawing.Point(0, 0);
            this.m_Toolbar.Name = "m_Toolbar";
            this.m_Toolbar.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.m_Toolbar.Size = new System.Drawing.Size(1264, 25);
            this.m_Toolbar.TabIndex = 10;
            this.m_Toolbar.Text = "m_Toolbar";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Image = global::ipmExtraFunctions.Properties.Resources.attachment;
            this.btnBrowse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(123, 22);
            this.btnBrowse.Text = "ფაილის არჩევა";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExecute
            // 
            this.btnExecute.Image = global::ipmExtraFunctions.Properties.Resources.check2_321;
            this.btnExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(82, 22);
            this.btnExecute.Text = "იმპორტი";
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // m_Grid
            // 
            this.m_Grid.AllowUserToAddRows = false;
            this.m_Grid.AllowUserToDeleteRows = false;
            this.m_Grid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(228)))), ((int)(((byte)(248)))));
            this.m_Grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.m_Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_Grid.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.m_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColCheck,
            this.ColTdate,
            this.ColOperation,
            this.ColPurpose,
            this.ColAmount,
            this.ColStatus,
            this.ColStatusId,
            this.ColUid,
            this.ColInside});
            this.m_Grid.ContextMenuStrip = this.menuImport;
            this.m_Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Grid.GridColor = System.Drawing.Color.LightSkyBlue;
            this.m_Grid.Location = new System.Drawing.Point(0, 46);
            this.m_Grid.Margin = new System.Windows.Forms.Padding(4);
            this.m_Grid.MultiSelect = false;
            this.m_Grid.Name = "m_Grid";
            this.m_Grid.RowHeadersVisible = false;
            this.m_Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_Grid.Size = new System.Drawing.Size(1264, 605);
            this.m_Grid.TabIndex = 29;
            // 
            // ColCheck
            // 
            this.ColCheck.FalseValue = "false";
            this.ColCheck.FillWeight = 5F;
            this.ColCheck.HeaderText = "x";
            this.ColCheck.Name = "ColCheck";
            this.ColCheck.TrueValue = "true";
            // 
            // ColTdate
            // 
            dataGridViewCellStyle3.Format = "yyyy/MM/dd HH:mm:ss";
            dataGridViewCellStyle3.NullValue = null;
            this.ColTdate.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColTdate.FillWeight = 25F;
            this.ColTdate.HeaderText = "თარიღი";
            this.ColTdate.Name = "ColTdate";
            this.ColTdate.ReadOnly = true;
            // 
            // ColOperation
            // 
            this.ColOperation.FillWeight = 30F;
            this.ColOperation.HeaderText = "ოპერაცია";
            this.ColOperation.Name = "ColOperation";
            this.ColOperation.ReadOnly = true;
            // 
            // ColPurpose
            // 
            dataGridViewCellStyle4.Format = "dd/MM/yyyy HH:mm:ss";
            dataGridViewCellStyle4.NullValue = null;
            this.ColPurpose.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColPurpose.FillWeight = 80F;
            this.ColPurpose.HeaderText = "საფუძველი";
            this.ColPurpose.Name = "ColPurpose";
            this.ColPurpose.ReadOnly = true;
            // 
            // ColAmount
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.ColAmount.DefaultCellStyle = dataGridViewCellStyle5;
            this.ColAmount.FillWeight = 15F;
            this.ColAmount.HeaderText = "თანხა";
            this.ColAmount.Name = "ColAmount";
            this.ColAmount.ReadOnly = true;
            // 
            // ColStatus
            // 
            this.ColStatus.FillWeight = 30F;
            this.ColStatus.HeaderText = "სტატუსი";
            this.ColStatus.Name = "ColStatus";
            this.ColStatus.ReadOnly = true;
            // 
            // ColStatusId
            // 
            this.ColStatusId.HeaderText = "ColStatusId";
            this.ColStatusId.Name = "ColStatusId";
            this.ColStatusId.ReadOnly = true;
            this.ColStatusId.Visible = false;
            // 
            // ColUid
            // 
            this.ColUid.HeaderText = "ColUid";
            this.ColUid.Name = "ColUid";
            this.ColUid.ReadOnly = true;
            this.ColUid.Visible = false;
            // 
            // ColInside
            // 
            this.ColInside.HeaderText = "Inside";
            this.ColInside.Name = "ColInside";
            this.ColInside.ReadOnly = true;
            this.ColInside.Visible = false;
            // 
            // menuImport
            // 
            this.menuImport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectIem,
            this.unselectItem});
            this.menuImport.Name = "menuCopy";
            this.menuImport.Size = new System.Drawing.Size(186, 48);
            // 
            // selectIem
            // 
            this.selectIem.Image = global::ipmExtraFunctions.Properties.Resources.check2_321;
            this.selectIem.Name = "selectIem";
            this.selectIem.Size = new System.Drawing.Size(185, 22);
            this.selectIem.Text = "მონიშვნა";
            this.selectIem.Click += new System.EventHandler(this.selectIem_Click);
            // 
            // unselectItem
            // 
            this.unselectItem.Image = global::ipmExtraFunctions.Properties.Resources.delete2_321_24;
            this.unselectItem.Name = "unselectItem";
            this.unselectItem.Size = new System.Drawing.Size(185, 22);
            this.unselectItem.Text = "მონიშვნის მოხსნა";
            this.unselectItem.Click += new System.EventHandler(this.unselectItem_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle6.Format = "yyyy/MM/dd HH:mm:ss";
            dataGridViewCellStyle6.NullValue = null;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn1.FillWeight = 25F;
            this.dataGridViewTextBoxColumn1.HeaderText = "id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 170;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 25F;
            this.dataGridViewTextBoxColumn2.HeaderText = "თარიღი";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 60;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle7.Format = "dd/MM/yyyy HH:mm:ss";
            dataGridViewCellStyle7.NullValue = null;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn3.FillWeight = 20F;
            this.dataGridViewTextBoxColumn3.HeaderText = "ს/ზ ნომერი";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 47;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N2";
            dataGridViewCellStyle8.NullValue = null;
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn4.FillWeight = 15F;
            this.dataGridViewTextBoxColumn4.HeaderText = "ა/ფ ნომერი";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 239;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.FillWeight = 50F;
            this.dataGridViewTextBoxColumn5.HeaderText = "მომწოდებელი";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 120;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.FillWeight = 80F;
            this.dataGridViewTextBoxColumn6.HeaderText = "დანიშნულება";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            this.dataGridViewTextBoxColumn6.Width = 191;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "ColUid";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Inside";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.lblResult);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 651);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1264, 30);
            this.panel7.TabIndex = 30;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(13, 4);
            this.lblResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(62, 18);
            this.lblResult.TabIndex = 2;
            this.lblResult.Text = "შედეგი:";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExport
            // 
            this.btnExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPreviewToExcel,
            this.btnExportToExcel});
            this.btnExport.Image = global::ipmExtraFunctions.Properties.Resources.Excel_icon;
            this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(96, 22);
            this.btnExport.Text = "ექსპორტი";
            // 
            // btnPreviewToExcel
            // 
            this.btnPreviewToExcel.Name = "btnPreviewToExcel";
            this.btnPreviewToExcel.Size = new System.Drawing.Size(178, 22);
            this.btnPreviewToExcel.Text = "დათვალიერება";
            this.btnPreviewToExcel.Click += new System.EventHandler(this.btnPreviewToExcel_Click);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(178, 22);
            this.btnExportToExcel.Text = "შენახვა ფაილში";
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // FinaOperationImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.m_Grid);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FinaOperationImportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ოპერაციების იმპორტი";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.m_Toolbar.ResumeLayout(false);
            this.m_Toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_Grid)).EndInit();
            this.menuImport.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView m_Grid;
        public System.Windows.Forms.ToolStrip m_Toolbar;
        private System.Windows.Forms.ToolStripButton btnBrowse;
        private System.Windows.Forms.ContextMenuStrip menuImport;
        private System.Windows.Forms.ToolStripMenuItem selectIem;
        private System.Windows.Forms.ToolStripMenuItem unselectItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnExecute;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColOperation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPurpose;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStatusId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColUid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColInside;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripDropDownButton btnExport;
        private System.Windows.Forms.ToolStripMenuItem btnPreviewToExcel;
        private System.Windows.Forms.ToolStripMenuItem btnExportToExcel;
    }
}