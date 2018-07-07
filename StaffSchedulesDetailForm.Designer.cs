namespace ipmExtraFunctions
{
    partial class StaffSchedulesDetailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StaffSchedulesDetailForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnProductsClear = new System.Windows.Forms.ToolStripButton();
            this.panelTop = new System.Windows.Forms.Panel();
            this.comboScheduleType = new System.Windows.Forms.ComboBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.comboPeriod = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.btnProductsGroup = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.comboStatus = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPurpose = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.comboStaff = new System.Windows.Forms.ComboBox();
            this.panel34 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.m_Grid = new ipmBaseListGridView.ContextFilterDataGridView();
            this.toolStrip1.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel34.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExit,
            this.btnSave,
            this.btnExcel,
            this.toolStripSeparator1,
            this.btnProductsClear});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1242, 25);
            this.toolStrip1.TabIndex = 17;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnExit
            // 
            this.btnExit.Image = global::ipmExtraFunctions.Properties.Resources.exit_16;
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(68, 22);
            this.btnExit.Text = "დახურვა";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::ipmExtraFunctions.Properties.Resources.save_16;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(62, 22);
            this.btnSave.Text = "შენახვა";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Image = global::ipmExtraFunctions.Properties.Resources.Excel_icon;
            this.btnExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(78, 22);
            this.btnExcel.Text = "ჩატვირთვა";
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnProductsClear
            // 
            this.btnProductsClear.Image = global::ipmExtraFunctions.Properties.Resources.x;
            this.btnProductsClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProductsClear.Name = "btnProductsClear";
            this.btnProductsClear.Size = new System.Drawing.Size(88, 22);
            this.btnProductsClear.Text = "გასუფთავება";
            this.btnProductsClear.Click += new System.EventHandler(this.btnProductsClear_Click);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.comboScheduleType);
            this.panelTop.Controls.Add(this.panel5);
            this.panelTop.Controls.Add(this.comboPeriod);
            this.panelTop.Controls.Add(this.panel4);
            this.panelTop.Controls.Add(this.btnProductsGroup);
            this.panelTop.Controls.Add(this.panel3);
            this.panelTop.Controls.Add(this.comboStatus);
            this.panelTop.Controls.Add(this.panel2);
            this.panelTop.Controls.Add(this.txtPurpose);
            this.panelTop.Controls.Add(this.panel1);
            this.panelTop.Controls.Add(this.comboStaff);
            this.panelTop.Controls.Add(this.panel34);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 25);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1242, 70);
            this.panelTop.TabIndex = 18;
            // 
            // comboScheduleType
            // 
            this.comboScheduleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboScheduleType.Enabled = false;
            this.comboScheduleType.FormattingEnabled = true;
            this.comboScheduleType.Items.AddRange(new object[] {
            "რაოდენობრივი",
            "თანხობრივი"});
            this.comboScheduleType.Location = new System.Drawing.Point(960, 38);
            this.comboScheduleType.Name = "comboScheduleType";
            this.comboScheduleType.Size = new System.Drawing.Size(131, 26);
            this.comboScheduleType.TabIndex = 46;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label6);
            this.panel5.Location = new System.Drawing.Point(879, 41);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(75, 23);
            this.panel5.TabIndex = 45;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Right;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(27, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 18);
            this.label6.TabIndex = 22;
            this.label6.Text = "გეგმა:";
            // 
            // comboPeriod
            // 
            this.comboPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPeriod.FormattingEnabled = true;
            this.comboPeriod.Items.AddRange(new object[] {
            "კვირა",
            "თვე",
            "კვარტალი",
            "წელი"});
            this.comboPeriod.Location = new System.Drawing.Point(632, 36);
            this.comboPeriod.Name = "comboPeriod";
            this.comboPeriod.Size = new System.Drawing.Size(232, 26);
            this.comboPeriod.TabIndex = 44;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label5);
            this.panel4.Location = new System.Drawing.Point(538, 36);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(88, 25);
            this.panel4.TabIndex = 43;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(15, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 18);
            this.label5.TabIndex = 22;
            this.label5.Text = "პერიოდი:";
            // 
            // btnProductsGroup
            // 
            this.btnProductsGroup.Image = global::ipmExtraFunctions.Properties.Resources.package_new_16;
            this.btnProductsGroup.Location = new System.Drawing.Point(141, 37);
            this.btnProductsGroup.Name = "btnProductsGroup";
            this.btnProductsGroup.Size = new System.Drawing.Size(24, 24);
            this.btnProductsGroup.TabIndex = 42;
            this.btnProductsGroup.UseVisualStyleBackColor = true;
            this.btnProductsGroup.Click += new System.EventHandler(this.btnProductsGroup_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(47, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(88, 25);
            this.panel3.TabIndex = 40;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(10, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 18);
            this.label3.TabIndex = 22;
            this.label3.Text = "საქონელი:";
            // 
            // comboStatus
            // 
            this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.Items.AddRange(new object[] {
            "აქტიური",
            "დასრულებული"});
            this.comboStatus.Location = new System.Drawing.Point(960, 6);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.Size = new System.Drawing.Size(131, 26);
            this.comboStatus.TabIndex = 39;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(879, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(75, 23);
            this.panel2.TabIndex = 38;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 18);
            this.label2.TabIndex = 22;
            this.label2.Text = "სტატუსი:";
            // 
            // txtPurpose
            // 
            this.txtPurpose.Location = new System.Drawing.Point(141, 6);
            this.txtPurpose.Name = "txtPurpose";
            this.txtPurpose.Size = new System.Drawing.Size(342, 24);
            this.txtPurpose.TabIndex = 37;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(132, 23);
            this.panel1.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(59, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 18);
            this.label1.TabIndex = 22;
            this.label1.Text = "შინაარსი:";
            // 
            // comboStaff
            // 
            this.comboStaff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStaff.FormattingEnabled = true;
            this.comboStaff.Location = new System.Drawing.Point(632, 6);
            this.comboStaff.Name = "comboStaff";
            this.comboStaff.Size = new System.Drawing.Size(232, 26);
            this.comboStaff.TabIndex = 35;
            this.comboStaff.SelectionChangeCommitted += new System.EventHandler(this.comboStaff_SelectionChangeCommitted);
            // 
            // panel34
            // 
            this.panel34.Controls.Add(this.label4);
            this.panel34.Location = new System.Drawing.Point(494, 9);
            this.panel34.Name = "panel34";
            this.panel34.Size = new System.Drawing.Size(132, 23);
            this.panel34.TabIndex = 34;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(17, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 18);
            this.label4.TabIndex = 22;
            this.label4.Text = "თანამშრომელი:";
            // 
            // m_Grid
            // 
            this.m_Grid.AllowUserToAddRows = false;
            this.m_Grid.AllowUserToDeleteRows = false;
            this.m_Grid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.m_Grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.m_Grid.AutoGenerateContextFilters = true;
            this.m_Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_Grid.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.MediumBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.m_Grid.ColumnHeadersHeight = 28;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_Grid.DefaultCellStyle = dataGridViewCellStyle3;
            this.m_Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.m_Grid.GridColor = System.Drawing.Color.LightSkyBlue;
            this.m_Grid.Location = new System.Drawing.Point(0, 95);
            this.m_Grid.Name = "m_Grid";
            this.m_Grid.PManager = null;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.MediumBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_Grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.m_Grid.RowHeadersVisible = false;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.m_Grid.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.m_Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_Grid.ShowCellErrors = false;
            this.m_Grid.ShowCellToolTips = false;
            this.m_Grid.ShowEditingIcon = false;
            this.m_Grid.ShowRowErrors = false;
            this.m_Grid.Size = new System.Drawing.Size(1242, 518);
            this.m_Grid.TabIndex = 19;
            this.m_Grid.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.m_Grid_ColumnAdded);
            // 
            // StaffSchedulesDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 613);
            this.Controls.Add(this.m_Grid);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "StaffSchedulesDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StaffSchedulesDetailForm";
            this.Load += new System.EventHandler(this.StaffSchedulesDetailForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel34.ResumeLayout(false);
            this.panel34.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_Grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.ComboBox comboStatus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPurpose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboStaff;
        private System.Windows.Forms.Panel panel34;
        private System.Windows.Forms.Label label4;
        public ipmBaseListGridView.ContextFilterDataGridView m_Grid;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button btnProductsGroup;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripButton btnProductsClear;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboPeriod;
        private System.Windows.Forms.ComboBox comboScheduleType;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripButton btnExcel;
    }
}