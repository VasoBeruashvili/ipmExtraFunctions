namespace ipmExtraFunctions
{
    partial class ProfitabilityMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfitabilityMainForm));
            this.m_Grid = new System.Windows.Forms.DataGridView();
            this.col_path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Profitability = new ipmControls.DataGridViewDecimalColumn();
            this.col_ShareOfGaint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_ShareOfSales = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_gainPercOutPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_outSingleDiscountPerc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_outSpecOthersPerc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_gain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_averageGainNoVat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_averageRest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_outQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_outAmountNoVat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelPeriod = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblMonts = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.m_Period = new ipmControls.DateTimePickers();
            this.m_Toolbar = new System.Windows.Forms.ToolStrip();
            this.btnSelect = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExcel = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnExport = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGenerate = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnFillData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDropData = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.m_Grid)).BeginInit();
            this.panelPeriod.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.m_Toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_Grid
            // 
            this.m_Grid.AllowUserToAddRows = false;
            this.m_Grid.AllowUserToDeleteRows = false;
            this.m_Grid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(228)))), ((int)(((byte)(248)))));
            this.m_Grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.m_Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_Grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.m_Grid.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_Grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.m_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_path,
            this.col_number,
            this.col_name,
            this.col_Profitability,
            this.col_ShareOfGaint,
            this.col_ShareOfSales,
            this.col_gainPercOutPrice,
            this.col_outSingleDiscountPerc,
            this.col_outSpecOthersPerc,
            this.col_gain,
            this.col_averageGainNoVat,
            this.col_averageRest,
            this.col_outQuantity,
            this.col_outAmountNoVat});
            this.m_Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Grid.Location = new System.Drawing.Point(0, 103);
            this.m_Grid.Margin = new System.Windows.Forms.Padding(4);
            this.m_Grid.MultiSelect = false;
            this.m_Grid.Name = "m_Grid";
            this.m_Grid.ReadOnly = true;
            this.m_Grid.RowHeadersVisible = false;
            this.m_Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_Grid.Size = new System.Drawing.Size(1436, 539);
            this.m_Grid.TabIndex = 11;
            this.m_Grid.Sorted += new System.EventHandler(this.m_Grid_Sorted);
            this.m_Grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_Grid_CellDoubleClick);
            // 
            // col_path
            // 
            this.col_path.HeaderText = "path";
            this.col_path.Name = "col_path";
            this.col_path.ReadOnly = true;
            this.col_path.Visible = false;
            // 
            // col_number
            // 
            this.col_number.FillWeight = 30F;
            this.col_number.HeaderText = "№";
            this.col_number.Name = "col_number";
            this.col_number.ReadOnly = true;
            // 
            // col_name
            // 
            this.col_name.FillWeight = 150F;
            this.col_name.HeaderText = "დასახელება";
            this.col_name.Name = "col_name";
            this.col_name.ReadOnly = true;
            // 
            // col_Profitability
            // 
            this.col_Profitability.FillWeight = 70F;
            this.col_Profitability.HeaderText = "რენტაბელობა (%)";
            this.col_Profitability.Name = "col_Profitability";
            this.col_Profitability.ReadOnly = true;
            this.col_Profitability.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // col_ShareOfGaint
            // 
            this.col_ShareOfGaint.FillWeight = 70F;
            this.col_ShareOfGaint.HeaderText = "წილი მოგებაში (%)";
            this.col_ShareOfGaint.Name = "col_ShareOfGaint";
            this.col_ShareOfGaint.ReadOnly = true;
            // 
            // col_ShareOfSales
            // 
            this.col_ShareOfSales.FillWeight = 70F;
            this.col_ShareOfSales.HeaderText = "წილი გაყიდვებში (%)";
            this.col_ShareOfSales.Name = "col_ShareOfSales";
            this.col_ShareOfSales.ReadOnly = true;
            // 
            // col_gainPercOutPrice
            // 
            this.col_gainPercOutPrice.HeaderText = "მოგების % რეალიზ. ფასიდან";
            this.col_gainPercOutPrice.Name = "col_gainPercOutPrice";
            this.col_gainPercOutPrice.ReadOnly = true;
            // 
            // col_outSingleDiscountPerc
            // 
            this.col_outSingleDiscountPerc.FillWeight = 90F;
            this.col_outSingleDiscountPerc.HeaderText = "წილი საცალო + ფასდაკლება (%)";
            this.col_outSingleDiscountPerc.Name = "col_outSingleDiscountPerc";
            this.col_outSingleDiscountPerc.ReadOnly = true;
            // 
            // col_outSpecOthersPerc
            // 
            this.col_outSpecOthersPerc.FillWeight = 68.18006F;
            this.col_outSpecOthersPerc.HeaderText = "წილი საბითუმო + სხვა (%)";
            this.col_outSpecOthersPerc.Name = "col_outSpecOthersPerc";
            this.col_outSpecOthersPerc.ReadOnly = true;
            // 
            // col_gain
            // 
            this.col_gain.FillWeight = 80F;
            this.col_gain.HeaderText = "მოგება მთელ პერიოდზე დღგ -ს გარეშე";
            this.col_gain.Name = "col_gain";
            this.col_gain.ReadOnly = true;
            // 
            // col_averageGainNoVat
            // 
            this.col_averageGainNoVat.FillWeight = 88.70564F;
            this.col_averageGainNoVat.HeaderText = "საშ. მოგება  ერთ თვეზე  დღგ -ს გარეშე";
            this.col_averageGainNoVat.Name = "col_averageGainNoVat";
            this.col_averageGainNoVat.ReadOnly = true;
            // 
            // col_averageRest
            // 
            this.col_averageRest.FillWeight = 70F;
            this.col_averageRest.HeaderText = "საშუალო ნაშთი";
            this.col_averageRest.Name = "col_averageRest";
            this.col_averageRest.ReadOnly = true;
            // 
            // col_outQuantity
            // 
            this.col_outQuantity.FillWeight = 70.24408F;
            this.col_outQuantity.HeaderText = "რეალიზ. რაოდენობა";
            this.col_outQuantity.Name = "col_outQuantity";
            this.col_outQuantity.ReadOnly = true;
            // 
            // col_outAmountNoVat
            // 
            this.col_outAmountNoVat.FillWeight = 88.70564F;
            this.col_outAmountNoVat.HeaderText = "რეალიზ. თანხა დღგ-ს გარეშე";
            this.col_outAmountNoVat.Name = "col_outAmountNoVat";
            this.col_outAmountNoVat.ReadOnly = true;
            // 
            // panelPeriod
            // 
            this.panelPeriod.Controls.Add(this.panel3);
            this.panelPeriod.Controls.Add(this.panel2);
            this.panelPeriod.Controls.Add(this.panel1);
            this.panelPeriod.Controls.Add(this.m_Period);
            this.panelPeriod.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panelPeriod.Location = new System.Drawing.Point(0, 25);
            this.panelPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.panelPeriod.Name = "panelPeriod";
            this.panelPeriod.Size = new System.Drawing.Size(1436, 78);
            this.panelPeriod.TabIndex = 10;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblMonts);
            this.panel3.Location = new System.Drawing.Point(155, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(132, 22);
            this.panel3.TabIndex = 5;
            // 
            // lblMonts
            // 
            this.lblMonts.AutoSize = true;
            this.lblMonts.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblMonts.Location = new System.Drawing.Point(0, 0);
            this.lblMonts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMonts.Name = "lblMonts";
            this.lblMonts.Size = new System.Drawing.Size(0, 18);
            this.lblMonts.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(12, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(140, 22);
            this.panel2.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "თვეების რაოდენობა:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(75, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(77, 22);
            this.panel1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Location = new System.Drawing.Point(9, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "პერიოდი:";
            // 
            // m_Period
            // 
            this.m_Period.BackColor = System.Drawing.Color.Transparent;
            this.m_Period.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_Period.Location = new System.Drawing.Point(156, 7);
            this.m_Period.Margin = new System.Windows.Forms.Padding(6);
            this.m_Period.Name = "m_Period";
            this.m_Period.Size = new System.Drawing.Size(337, 30);
            this.m_Period.TabIndex = 0;
            // 
            // m_Toolbar
            // 
            this.m_Toolbar.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_Toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.m_Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSelect,
            this.btnRefresh,
            this.btnClose,
            this.toolStripSeparator1,
            this.btnExcel,
            this.toolStripSeparator3,
            this.btnGenerate});
            this.m_Toolbar.Location = new System.Drawing.Point(0, 0);
            this.m_Toolbar.Name = "m_Toolbar";
            this.m_Toolbar.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.m_Toolbar.Size = new System.Drawing.Size(1436, 25);
            this.m_Toolbar.TabIndex = 9;
            this.m_Toolbar.Text = "m_Toolbar";
            // 
            // btnSelect
            // 
            this.btnSelect.Image = ((System.Drawing.Image)(resources.GetObject("btnSelect.Image")));
            this.btnSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(65, 22);
            this.btnSelect.Text = "არჩევა";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
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
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(76, 22);
            this.btnClose.Text = "დახურვა";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExcel
            // 
            this.btnExcel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExport,
            this.btnPreview});
            this.btnExcel.Image = global::ipmExtraFunctions.Properties.Resources.Excel_icon;
            this.btnExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(66, 22);
            this.btnExcel.Text = "Excel";
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(172, 22);
            this.btnExport.Text = "ექსპორტი";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(172, 22);
            this.btnPreview.Text = "დათვალიერება";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnGenerate
            // 
            this.btnGenerate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFillData,
            this.toolStripSeparator2,
            this.btnDropData});
            this.btnGenerate.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnGenerate.Image = global::ipmExtraFunctions.Properties.Resources.note_pinned_16;
            this.btnGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(140, 22);
            this.btnGenerate.Text = "ნაშთების ცხრილი";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnFillData
            // 
            this.btnFillData.Image = global::ipmExtraFunctions.Properties.Resources.add_16;
            this.btnFillData.Name = "btnFillData";
            this.btnFillData.Size = new System.Drawing.Size(187, 22);
            this.btnFillData.Text = "ცხრილის შევსება";
            this.btnFillData.Click += new System.EventHandler(this.btnFillData_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(184, 6);
            // 
            // btnDropData
            // 
            this.btnDropData.Image = global::ipmExtraFunctions.Properties.Resources.delete_16;
            this.btnDropData.Name = "btnDropData";
            this.btnDropData.Size = new System.Drawing.Size(187, 22);
            this.btnDropData.Text = "ცხრილის განულება";
            this.btnDropData.Click += new System.EventHandler(this.btnDropData_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "path";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 30F;
            this.dataGridViewTextBoxColumn2.HeaderText = "№";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 37;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 200F;
            this.dataGridViewTextBoxColumn3.HeaderText = "დასახელება";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 243;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 70.24408F;
            this.dataGridViewTextBoxColumn4.HeaderText = "რეალიზ. რაოდენობა";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 85;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.FillWeight = 88.70564F;
            this.dataGridViewTextBoxColumn5.HeaderText = "რეალიზ. თანხა დ.ღ.გ ს გარეშე";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 108;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.FillWeight = 90F;
            this.dataGridViewTextBoxColumn6.HeaderText = "წილი საცალო+ფასდაკლება (%)";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 110;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.FillWeight = 68.18006F;
            this.dataGridViewTextBoxColumn7.HeaderText = "წილი საბითუმო+სხვა (%)";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 83;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.FillWeight = 88.70564F;
            this.dataGridViewTextBoxColumn8.HeaderText = "საშუალო ნაშთი";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 108;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.FillWeight = 52.69791F;
            this.dataGridViewTextBoxColumn9.HeaderText = "მოგება";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 64;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.FillWeight = 88.70564F;
            this.dataGridViewTextBoxColumn10.HeaderText = "საშ. მოგება დ.ღ.გ ს გარეშე ";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Width = 108;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "მოგების % რეალიზ. ფასიდან";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Width = 122;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.FillWeight = 70F;
            this.dataGridViewTextBoxColumn12.HeaderText = "წილი გაყიდვებში (%)";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Width = 122;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.FillWeight = 70F;
            this.dataGridViewTextBoxColumn13.HeaderText = "წილი მოგებაში (%)";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Width = 121;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.FillWeight = 70F;
            this.dataGridViewTextBoxColumn14.HeaderText = "რენტაბელობა (%)";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Width = 122;
            // 
            // ProfitabilityMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1436, 642);
            this.Controls.Add(this.m_Grid);
            this.Controls.Add(this.panelPeriod);
            this.Controls.Add(this.m_Toolbar);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ProfitabilityMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "რენტაბელობა - ყველა ჯგუფი";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.m_Grid)).EndInit();
            this.panelPeriod.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.m_Toolbar.ResumeLayout(false);
            this.m_Toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView m_Grid;
        private System.Windows.Forms.Panel panelPeriod;
        private System.Windows.Forms.Label label3;
        private ipmControls.DateTimePickers m_Period;
        public System.Windows.Forms.ToolStrip m_Toolbar;
        public System.Windows.Forms.ToolStripButton btnSelect;
        public System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblMonts;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripDropDownButton btnGenerate;
        private System.Windows.Forms.ToolStripMenuItem btnFillData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem btnDropData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripDropDownButton btnExcel;
        private System.Windows.Forms.ToolStripMenuItem btnExport;
        private System.Windows.Forms.ToolStripMenuItem btnPreview;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_path;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_name;
        private ipmControls.DataGridViewDecimalColumn col_Profitability;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_ShareOfGaint;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_ShareOfSales;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_gainPercOutPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_outSingleDiscountPerc;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_outSpecOthersPerc;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_gain;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_averageGainNoVat;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_averageRest;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_outQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_outAmountNoVat;

    }
}