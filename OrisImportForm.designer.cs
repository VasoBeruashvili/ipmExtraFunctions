namespace ipmExtraFunctions
{
    partial class OrisImportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrisImportForm));
            this.panel3 = new System.Windows.Forms.Panel();
            this.m_Toolbar = new System.Windows.Forms.ToolStrip();
            this.btnBrowse = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_Grid = new System.Windows.Forms.DataGridView();
            this.ColCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColTdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPurpose = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDebet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCurrency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColFinaDebet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColFinaCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColInside = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuImport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectIem = new System.Windows.Forms.ToolStripMenuItem();
            this.unselectItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExecute = new System.Windows.Forms.ToolStripButton();
            this.panel3.SuspendLayout();
            this.m_Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_Grid)).BeginInit();
            this.menuImport.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.m_Toolbar);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1264, 74);
            this.panel3.TabIndex = 28;
            // 
            // m_Toolbar
            // 
            this.m_Toolbar.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_Toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.m_Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBrowse,
            this.toolStripSeparator1,
            this.btnExecute});
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
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 25F;
            this.dataGridViewTextBoxColumn2.HeaderText = "თარიღი";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 60;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 20F;
            this.dataGridViewTextBoxColumn3.HeaderText = "ს/ზ ნომერი";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 47;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "ა/ფ ნომერი";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 239;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.FillWeight = 50F;
            this.dataGridViewTextBoxColumn5.HeaderText = "მომწოდებელი";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 120;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.FillWeight = 80F;
            this.dataGridViewTextBoxColumn6.HeaderText = "დანიშნულება";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 191;
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
            this.ColPurpose,
            this.ColDebet,
            this.ColCredit,
            this.ColAmount,
            this.ColCurrency,
            this.ColRate,
            this.ColFinaDebet,
            this.ColFinaCredit,
            this.ColStatus,
            this.ColInside});
            this.m_Grid.ContextMenuStrip = this.menuImport;
            this.m_Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Grid.GridColor = System.Drawing.Color.LightSkyBlue;
            this.m_Grid.Location = new System.Drawing.Point(0, 74);
            this.m_Grid.Margin = new System.Windows.Forms.Padding(4);
            this.m_Grid.MultiSelect = false;
            this.m_Grid.Name = "m_Grid";
            this.m_Grid.RowHeadersVisible = false;
            this.m_Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_Grid.Size = new System.Drawing.Size(1264, 607);
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
            dataGridViewCellStyle3.Format = "dd/MM/yyyy";
            dataGridViewCellStyle3.NullValue = null;
            this.ColTdate.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColTdate.FillWeight = 20F;
            this.ColTdate.HeaderText = "თარიღი";
            this.ColTdate.Name = "ColTdate";
            // 
            // ColPurpose
            // 
            dataGridViewCellStyle4.Format = "dd/MM/yyyy HH:mm:ss";
            dataGridViewCellStyle4.NullValue = null;
            this.ColPurpose.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColPurpose.FillWeight = 30F;
            this.ColPurpose.HeaderText = "საფუძველი";
            this.ColPurpose.Name = "ColPurpose";
            // 
            // ColDebet
            // 
            this.ColDebet.FillWeight = 15F;
            this.ColDebet.HeaderText = "დებეტი";
            this.ColDebet.Name = "ColDebet";
            // 
            // ColCredit
            // 
            this.ColCredit.FillWeight = 15F;
            this.ColCredit.HeaderText = "კრედიტი";
            this.ColCredit.Name = "ColCredit";
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
            // 
            // ColCurrency
            // 
            this.ColCurrency.FillWeight = 15F;
            this.ColCurrency.HeaderText = "ვალუტა";
            this.ColCurrency.Name = "ColCurrency";
            // 
            // ColRate
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.ColRate.DefaultCellStyle = dataGridViewCellStyle6;
            this.ColRate.FillWeight = 15F;
            this.ColRate.HeaderText = "კურსი";
            this.ColRate.Name = "ColRate";
            // 
            // ColFinaDebet
            // 
            this.ColFinaDebet.FillWeight = 20F;
            this.ColFinaDebet.HeaderText = "დებეტი(FINA)";
            this.ColFinaDebet.Name = "ColFinaDebet";
            // 
            // ColFinaCredit
            // 
            this.ColFinaCredit.FillWeight = 20F;
            this.ColFinaCredit.HeaderText = "კრედიტი(FINA)";
            this.ColFinaCredit.Name = "ColFinaCredit";
            // 
            // ColStatus
            // 
            this.ColStatus.FillWeight = 30F;
            this.ColStatus.HeaderText = "სტატუსი";
            this.ColStatus.Name = "ColStatus";
            // 
            // ColInside
            // 
            this.ColInside.HeaderText = "Inside";
            this.ColInside.Name = "ColInside";
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
            // OrisImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.m_Grid);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OrisImportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "იმპორტი";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.m_Toolbar.ResumeLayout(false);
            this.m_Toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_Grid)).EndInit();
            this.menuImport.ResumeLayout(false);
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
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPurpose;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDebet;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCurrency;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFinaDebet;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFinaCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColInside;
        private System.Windows.Forms.ContextMenuStrip menuImport;
        private System.Windows.Forms.ToolStripMenuItem selectIem;
        private System.Windows.Forms.ToolStripMenuItem unselectItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnExecute;
    }
}