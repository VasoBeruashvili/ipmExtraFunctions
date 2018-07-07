namespace ipmExtraFunctions
{
    partial class GlobalOperationForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalOperationForm));
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelPeriod = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panelFind = new System.Windows.Forms.Panel();
            this.txtFilter = new ipmControls.SearchTextBox();
            this.comboFilter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.m_Period = new ipmControls.DateTimePickers();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.checkAll = new System.Windows.Forms.CheckBox();
            this.panelGrid = new System.Windows.Forms.Panel();
            this.tabGlobal = new System.Windows.Forms.TabControl();
            this.tabPageLoad = new System.Windows.Forms.TabPage();
            this.m_GridServices = new System.Windows.Forms.DataGridView();
            this.ColNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_tdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Cnumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_brigaed_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_contragent_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_pasport_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageDelete = new System.Windows.Forms.TabPage();
            this.m_GridDeleteds = new System.Windows.Forms.DataGridView();
            this.DColNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dcol_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dcol_tdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dcol_Cnumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dcol_brigaed_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dcol_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dcol_contragent_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dcol_pasport_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dcol_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dcol_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExecute = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop.SuspendLayout();
            this.panelPeriod.SuspendLayout();
            this.panelFind.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelGrid.SuspendLayout();
            this.tabGlobal.SuspendLayout();
            this.tabPageLoad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_GridServices)).BeginInit();
            this.tabPageDelete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_GridDeleteds)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panelPeriod);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panelTop.Location = new System.Drawing.Point(0, 25);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1199, 34);
            this.panelTop.TabIndex = 37;
            // 
            // panelPeriod
            // 
            this.panelPeriod.Controls.Add(this.btnRefresh);
            this.panelPeriod.Controls.Add(this.panelFind);
            this.panelPeriod.Controls.Add(this.panel1);
            this.panelPeriod.Controls.Add(this.m_Period);
            this.panelPeriod.Location = new System.Drawing.Point(270, 2);
            this.panelPeriod.Name = "panelPeriod";
            this.panelPeriod.Size = new System.Drawing.Size(926, 31);
            this.panelPeriod.TabIndex = 6;
            this.panelPeriod.Visible = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::ipmExtraFunctions.Properties.Resources.refresh_16;
            this.btnRefresh.Location = new System.Drawing.Point(893, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(26, 26);
            this.btnRefresh.TabIndex = 12;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panelFind
            // 
            this.panelFind.Controls.Add(this.txtFilter);
            this.panelFind.Controls.Add(this.comboFilter);
            this.panelFind.Controls.Add(this.label1);
            this.panelFind.Location = new System.Drawing.Point(57, 1);
            this.panelFind.Name = "panelFind";
            this.panelFind.Size = new System.Drawing.Size(419, 30);
            this.panelFind.TabIndex = 11;
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(90, 3);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(153, 24);
            this.txtFilter.TabIndex = 7;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // comboFilter
            // 
            this.comboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFilter.FormattingEnabled = true;
            this.comboFilter.Location = new System.Drawing.Point(249, 1);
            this.comboFilter.Name = "comboFilter";
            this.comboFilter.Size = new System.Drawing.Size(167, 26);
            this.comboFilter.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "ძებნა:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(489, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(82, 22);
            this.panel1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Location = new System.Drawing.Point(9, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "პერიოდი:";
            // 
            // m_Period
            // 
            this.m_Period.BackColor = System.Drawing.Color.Transparent;
            this.m_Period.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_Period.Location = new System.Drawing.Point(574, 3);
            this.m_Period.Margin = new System.Windows.Forms.Padding(6);
            this.m_Period.Name = "m_Period";
            this.m_Period.ProgramManager = null;
            this.m_Period.Size = new System.Drawing.Size(316, 27);
            this.m_Period.TabIndex = 4;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.checkAll);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 438);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1199, 35);
            this.panelBottom.TabIndex = 38;
            // 
            // checkAll
            // 
            this.checkAll.AutoSize = true;
            this.checkAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkAll.Location = new System.Drawing.Point(3, 6);
            this.checkAll.Name = "checkAll";
            this.checkAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkAll.Size = new System.Drawing.Size(164, 22);
            this.checkAll.TabIndex = 58;
            this.checkAll.Text = ":სრული შესრულება";
            this.checkAll.UseVisualStyleBackColor = true;
            // 
            // panelGrid
            // 
            this.panelGrid.Controls.Add(this.tabGlobal);
            this.panelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrid.Location = new System.Drawing.Point(0, 59);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(1199, 379);
            this.panelGrid.TabIndex = 39;
            // 
            // tabGlobal
            // 
            this.tabGlobal.Controls.Add(this.tabPageLoad);
            this.tabGlobal.Controls.Add(this.tabPageDelete);
            this.tabGlobal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabGlobal.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.tabGlobal.Location = new System.Drawing.Point(0, 0);
            this.tabGlobal.Name = "tabGlobal";
            this.tabGlobal.SelectedIndex = 0;
            this.tabGlobal.Size = new System.Drawing.Size(1199, 379);
            this.tabGlobal.TabIndex = 19;
            this.tabGlobal.SelectedIndexChanged += new System.EventHandler(this.tabGlobal_SelectedIndexChanged);
            // 
            // tabPageLoad
            // 
            this.tabPageLoad.Controls.Add(this.m_GridServices);
            this.tabPageLoad.Location = new System.Drawing.Point(4, 27);
            this.tabPageLoad.Name = "tabPageLoad";
            this.tabPageLoad.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLoad.Size = new System.Drawing.Size(1191, 348);
            this.tabPageLoad.TabIndex = 0;
            this.tabPageLoad.Text = "ჩასატვირთი ოპერაციები";
            this.tabPageLoad.UseVisualStyleBackColor = true;
            // 
            // m_GridServices
            // 
            this.m_GridServices.AllowUserToAddRows = false;
            this.m_GridServices.AllowUserToDeleteRows = false;
            this.m_GridServices.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.SkyBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_GridServices.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.m_GridServices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_GridServices.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.m_GridServices.BackgroundColor = System.Drawing.Color.White;
            this.m_GridServices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_GridServices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.m_GridServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_GridServices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColNumber,
            this.col_id,
            this.col_tdate,
            this.col_Cnumber,
            this.col_brigaed_name,
            this.col_name,
            this.col_contragent_code,
            this.col_pasport_number,
            this.col_user});
            this.m_GridServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_GridServices.Location = new System.Drawing.Point(3, 3);
            this.m_GridServices.Name = "m_GridServices";
            this.m_GridServices.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_GridServices.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.m_GridServices.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.m_GridServices.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.m_GridServices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_GridServices.Size = new System.Drawing.Size(1185, 342);
            this.m_GridServices.TabIndex = 18;
            this.m_GridServices.Tag = "მომსახურების სია";
            // 
            // ColNumber
            // 
            this.ColNumber.FillWeight = 20F;
            this.ColNumber.HeaderText = "№";
            this.ColNumber.Name = "ColNumber";
            this.ColNumber.ReadOnly = true;
            // 
            // col_id
            // 
            this.col_id.HeaderText = "ID";
            this.col_id.Name = "col_id";
            this.col_id.ReadOnly = true;
            this.col_id.Visible = false;
            // 
            // col_tdate
            // 
            this.col_tdate.HeaderText = "თარიღი";
            this.col_tdate.Name = "col_tdate";
            this.col_tdate.ReadOnly = true;
            // 
            // col_Cnumber
            // 
            this.col_Cnumber.HeaderText = "ხელშეკრულება";
            this.col_Cnumber.Name = "col_Cnumber";
            this.col_Cnumber.ReadOnly = true;
            // 
            // col_brigaed_name
            // 
            this.col_brigaed_name.HeaderText = "ბრიგადა";
            this.col_brigaed_name.Name = "col_brigaed_name";
            this.col_brigaed_name.ReadOnly = true;
            // 
            // col_name
            // 
            this.col_name.HeaderText = "სახელი, გვარი";
            this.col_name.Name = "col_name";
            this.col_name.ReadOnly = true;
            // 
            // col_contragent_code
            // 
            this.col_contragent_code.HeaderText = "პირადი ნომერი";
            this.col_contragent_code.Name = "col_contragent_code";
            this.col_contragent_code.ReadOnly = true;
            // 
            // col_pasport_number
            // 
            this.col_pasport_number.HeaderText = "პასპორტის ნომერი";
            this.col_pasport_number.Name = "col_pasport_number";
            this.col_pasport_number.ReadOnly = true;
            // 
            // col_user
            // 
            this.col_user.HeaderText = "ოპერატორი";
            this.col_user.Name = "col_user";
            this.col_user.ReadOnly = true;
            // 
            // tabPageDelete
            // 
            this.tabPageDelete.Controls.Add(this.m_GridDeleteds);
            this.tabPageDelete.Location = new System.Drawing.Point(4, 27);
            this.tabPageDelete.Name = "tabPageDelete";
            this.tabPageDelete.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDelete.Size = new System.Drawing.Size(1191, 348);
            this.tabPageDelete.TabIndex = 1;
            this.tabPageDelete.Text = "შესრულებული ოპერაციები";
            this.tabPageDelete.UseVisualStyleBackColor = true;
            // 
            // m_GridDeleteds
            // 
            this.m_GridDeleteds.AllowUserToAddRows = false;
            this.m_GridDeleteds.AllowUserToDeleteRows = false;
            this.m_GridDeleteds.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.SkyBlue;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_GridDeleteds.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.m_GridDeleteds.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_GridDeleteds.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.m_GridDeleteds.BackgroundColor = System.Drawing.Color.White;
            this.m_GridDeleteds.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_GridDeleteds.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.m_GridDeleteds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_GridDeleteds.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DColNumber,
            this.Dcol_id,
            this.Dcol_tdate,
            this.Dcol_Cnumber,
            this.Dcol_brigaed_name,
            this.Dcol_name,
            this.Dcol_contragent_code,
            this.Dcol_pasport_number,
            this.Dcol_user,
            this.Dcol_status});
            this.m_GridDeleteds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_GridDeleteds.Location = new System.Drawing.Point(3, 3);
            this.m_GridDeleteds.Name = "m_GridDeleteds";
            this.m_GridDeleteds.ReadOnly = true;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_GridDeleteds.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.m_GridDeleteds.RowHeadersVisible = false;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_GridDeleteds.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.m_GridDeleteds.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_GridDeleteds.Size = new System.Drawing.Size(1185, 342);
            this.m_GridDeleteds.TabIndex = 19;
            this.m_GridDeleteds.Tag = "მომსახურების სია";
            // 
            // DColNumber
            // 
            this.DColNumber.FillWeight = 20F;
            this.DColNumber.HeaderText = "№";
            this.DColNumber.Name = "DColNumber";
            this.DColNumber.ReadOnly = true;
            // 
            // Dcol_id
            // 
            this.Dcol_id.HeaderText = "ID";
            this.Dcol_id.Name = "Dcol_id";
            this.Dcol_id.ReadOnly = true;
            this.Dcol_id.Visible = false;
            // 
            // Dcol_tdate
            // 
            this.Dcol_tdate.HeaderText = "თარიღი";
            this.Dcol_tdate.Name = "Dcol_tdate";
            this.Dcol_tdate.ReadOnly = true;
            // 
            // Dcol_Cnumber
            // 
            this.Dcol_Cnumber.HeaderText = "ხელშეკრულება";
            this.Dcol_Cnumber.Name = "Dcol_Cnumber";
            this.Dcol_Cnumber.ReadOnly = true;
            // 
            // Dcol_brigaed_name
            // 
            this.Dcol_brigaed_name.FillWeight = 80F;
            this.Dcol_brigaed_name.HeaderText = "ბრიგადა";
            this.Dcol_brigaed_name.Name = "Dcol_brigaed_name";
            this.Dcol_brigaed_name.ReadOnly = true;
            // 
            // Dcol_name
            // 
            this.Dcol_name.HeaderText = "სახელი, გვარი";
            this.Dcol_name.Name = "Dcol_name";
            this.Dcol_name.ReadOnly = true;
            // 
            // Dcol_contragent_code
            // 
            this.Dcol_contragent_code.HeaderText = "პირადი ნომერი";
            this.Dcol_contragent_code.Name = "Dcol_contragent_code";
            this.Dcol_contragent_code.ReadOnly = true;
            // 
            // Dcol_pasport_number
            // 
            this.Dcol_pasport_number.HeaderText = "პასპორტის ნომერი";
            this.Dcol_pasport_number.Name = "Dcol_pasport_number";
            this.Dcol_pasport_number.ReadOnly = true;
            // 
            // Dcol_user
            // 
            this.Dcol_user.HeaderText = "ოპერატორი";
            this.Dcol_user.Name = "Dcol_user";
            this.Dcol_user.ReadOnly = true;
            // 
            // Dcol_status
            // 
            this.Dcol_status.FillWeight = 80F;
            this.Dcol_status.HeaderText = "სტატუსი";
            this.Dcol_status.Name = "Dcol_status";
            this.Dcol_status.ReadOnly = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExport,
            this.toolStripSeparator1,
            this.btnExecute});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1199, 25);
            this.toolStrip1.TabIndex = 40;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Image = global::ipmExtraFunctions.Properties.Resources.Excel_icon;
            this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(87, 22);
            this.btnExport.Text = "ექსპორტი";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExecute
            // 
            this.btnExecute.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoad,
            this.btnDelete});
            this.btnExecute.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnExecute.Image = global::ipmExtraFunctions.Properties.Resources.flash_16;
            this.btnExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(109, 22);
            this.btnExecute.Text = "შესრულება";
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnLoad.Image = global::ipmExtraFunctions.Properties.Resources.select_16;
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(143, 22);
            this.btnLoad.Text = "ჩატვირთვა";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnDelete.Image = global::ipmExtraFunctions.Properties.Resources.delete_16;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(143, 22);
            this.btnDelete.Text = "წაშლა";
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // GlobalOperationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 473);
            this.Controls.Add(this.panelGrid);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GlobalOperationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Global Operations:";
            this.panelTop.ResumeLayout(false);
            this.panelPeriod.ResumeLayout(false);
            this.panelFind.ResumeLayout(false);
            this.panelFind.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.panelGrid.ResumeLayout(false);
            this.tabGlobal.ResumeLayout(false);
            this.tabPageLoad.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_GridServices)).EndInit();
            this.tabPageDelete.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_GridDeleteds)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.CheckBox checkAll;
        public System.Windows.Forms.DataGridView m_GridServices;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_tdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Cnumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_brigaed_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_contragent_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_pasport_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_user;
        private System.Windows.Forms.ToolStripDropDownButton btnExecute;
        private System.Windows.Forms.TabControl tabGlobal;
        private System.Windows.Forms.TabPage tabPageLoad;
        private System.Windows.Forms.TabPage tabPageDelete;
        private System.Windows.Forms.ToolStripMenuItem btnLoad;
        private System.Windows.Forms.ToolStripMenuItem btnDelete;
        public System.Windows.Forms.DataGridView m_GridDeleteds;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private ipmControls.DateTimePickers m_Period;
        private System.Windows.Forms.Panel panelPeriod;
        private System.Windows.Forms.DataGridViewTextBoxColumn DColNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dcol_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dcol_tdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dcol_Cnumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dcol_brigaed_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dcol_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dcol_contragent_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dcol_pasport_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dcol_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dcol_status;
        private System.Windows.Forms.Panel panelFind;
        private System.Windows.Forms.ComboBox comboFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRefresh;
        private ipmControls.SearchTextBox txtFilter;
    }
}