namespace ipmExtraFunctions
{
    partial class ProfitabilityDetailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfitabilityDetailForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.m_Grid = new System.Windows.Forms.DataGridView();
            this.col_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_selfCostNoVat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_selfCostVat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_outPriceSingle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_outPriceDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_outPriceSabitumo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_outPriceOther = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_restPeriodStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_amountPeriodStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_restPeriodEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_amountPeriodEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_quantityAverageRest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_amountAverageRest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelPeriod = new System.Windows.Forms.Panel();
            this.btnProduct = new System.Windows.Forms.Button();
            this.labelCode = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblMonts = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.m_Period = new ipmControls.DateTimePickers();
            this.m_Toolbar = new System.Windows.Forms.ToolStrip();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExcel = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnExport = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.m_Grid2 = new System.Windows.Forms.DataGridView();
            this.col_realize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_single = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_other = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_otherOther = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_sum = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.m_Grid)).BeginInit();
            this.panelPeriod.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.m_Toolbar.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_Grid2)).BeginInit();
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
            this.col_id,
            this.col_selfCostNoVat,
            this.col_selfCostVat,
            this.col_outPriceSingle,
            this.col_outPriceDiscount,
            this.col_outPriceSabitumo,
            this.col_outPriceOther,
            this.col_restPeriodStart,
            this.col_amountPeriodStart,
            this.col_restPeriodEnd,
            this.col_amountPeriodEnd,
            this.col_quantityAverageRest,
            this.col_amountAverageRest});
            this.m_Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Grid.Location = new System.Drawing.Point(0, 0);
            this.m_Grid.Margin = new System.Windows.Forms.Padding(4);
            this.m_Grid.MultiSelect = false;
            this.m_Grid.Name = "m_Grid";
            this.m_Grid.ReadOnly = true;
            this.m_Grid.RowHeadersVisible = false;
            this.m_Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_Grid.Size = new System.Drawing.Size(1436, 164);
            this.m_Grid.TabIndex = 11;
            this.m_Grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_Grid_CellContentClick);
            // 
            // col_id
            // 
            this.col_id.HeaderText = "ID";
            this.col_id.Name = "col_id";
            this.col_id.ReadOnly = true;
            this.col_id.Visible = false;
            // 
            // col_selfCostNoVat
            // 
            this.col_selfCostNoVat.FillWeight = 90F;
            this.col_selfCostNoVat.HeaderText = "თვ.ღირ. დ.ღ.გ ს გარეშე";
            this.col_selfCostNoVat.Name = "col_selfCostNoVat";
            this.col_selfCostNoVat.ReadOnly = true;
            // 
            // col_selfCostVat
            // 
            this.col_selfCostVat.FillWeight = 90F;
            this.col_selfCostVat.HeaderText = "თვ.ღირ. დ.ღ.გ ს ჩათვლით";
            this.col_selfCostVat.Name = "col_selfCostVat";
            this.col_selfCostVat.ReadOnly = true;
            // 
            // col_outPriceSingle
            // 
            this.col_outPriceSingle.FillWeight = 50F;
            this.col_outPriceSingle.HeaderText = "საცალო";
            this.col_outPriceSingle.Name = "col_outPriceSingle";
            this.col_outPriceSingle.ReadOnly = true;
            // 
            // col_outPriceDiscount
            // 
            this.col_outPriceDiscount.FillWeight = 50F;
            this.col_outPriceDiscount.HeaderText = "ფასდაკლება";
            this.col_outPriceDiscount.Name = "col_outPriceDiscount";
            this.col_outPriceDiscount.ReadOnly = true;
            // 
            // col_outPriceSabitumo
            // 
            this.col_outPriceSabitumo.FillWeight = 50F;
            this.col_outPriceSabitumo.HeaderText = "საბითუმო";
            this.col_outPriceSabitumo.Name = "col_outPriceSabitumo";
            this.col_outPriceSabitumo.ReadOnly = true;
            // 
            // col_outPriceOther
            // 
            this.col_outPriceOther.FillWeight = 50F;
            this.col_outPriceOther.HeaderText = "სხვა";
            this.col_outPriceOther.Name = "col_outPriceOther";
            this.col_outPriceOther.ReadOnly = true;
            // 
            // col_restPeriodStart
            // 
            this.col_restPeriodStart.FillWeight = 80F;
            this.col_restPeriodStart.HeaderText = "რაოდ. პერიოდის დასაწყისში";
            this.col_restPeriodStart.Name = "col_restPeriodStart";
            this.col_restPeriodStart.ReadOnly = true;
            // 
            // col_amountPeriodStart
            // 
            this.col_amountPeriodStart.FillWeight = 80F;
            this.col_amountPeriodStart.HeaderText = "თანხა პერიოდის დასაწყისში";
            this.col_amountPeriodStart.Name = "col_amountPeriodStart";
            this.col_amountPeriodStart.ReadOnly = true;
            // 
            // col_restPeriodEnd
            // 
            this.col_restPeriodEnd.FillWeight = 75F;
            this.col_restPeriodEnd.HeaderText = "რაოდ. პერიოდის ბოლოს";
            this.col_restPeriodEnd.Name = "col_restPeriodEnd";
            this.col_restPeriodEnd.ReadOnly = true;
            // 
            // col_amountPeriodEnd
            // 
            this.col_amountPeriodEnd.FillWeight = 75F;
            this.col_amountPeriodEnd.HeaderText = "თანხა პერიოდის ბოლოს";
            this.col_amountPeriodEnd.Name = "col_amountPeriodEnd";
            this.col_amountPeriodEnd.ReadOnly = true;
            // 
            // col_quantityAverageRest
            // 
            this.col_quantityAverageRest.FillWeight = 70F;
            this.col_quantityAverageRest.HeaderText = "საშუალო ნაშთი (რაოდ)";
            this.col_quantityAverageRest.Name = "col_quantityAverageRest";
            this.col_quantityAverageRest.ReadOnly = true;
            // 
            // col_amountAverageRest
            // 
            this.col_amountAverageRest.FillWeight = 70F;
            this.col_amountAverageRest.HeaderText = "საშუალო ნაშთი (თანხა)";
            this.col_amountAverageRest.Name = "col_amountAverageRest";
            this.col_amountAverageRest.ReadOnly = true;
            // 
            // panelPeriod
            // 
            this.panelPeriod.Controls.Add(this.btnProduct);
            this.panelPeriod.Controls.Add(this.labelCode);
            this.panelPeriod.Controls.Add(this.txtName);
            this.panelPeriod.Controls.Add(this.txtCode);
            this.panelPeriod.Controls.Add(this.panel3);
            this.panelPeriod.Controls.Add(this.panel2);
            this.panelPeriod.Controls.Add(this.panel1);
            this.panelPeriod.Controls.Add(this.m_Period);
            this.panelPeriod.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panelPeriod.Location = new System.Drawing.Point(0, 25);
            this.panelPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.panelPeriod.Name = "panelPeriod";
            this.panelPeriod.Size = new System.Drawing.Size(1436, 99);
            this.panelPeriod.TabIndex = 10;
            // 
            // btnProduct
            // 
            this.btnProduct.Image = global::ipmExtraFunctions.Properties.Resources.folder_closed;
            this.btnProduct.Location = new System.Drawing.Point(444, 36);
            this.btnProduct.Name = "btnProduct";
            this.btnProduct.Size = new System.Drawing.Size(24, 24);
            this.btnProduct.TabIndex = 44;
            this.btnProduct.UseVisualStyleBackColor = true;
            this.btnProduct.Click += new System.EventHandler(this.btnProduct_Click);
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.labelCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCode.Location = new System.Drawing.Point(80, 39);
            this.labelCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(73, 18);
            this.labelCode.TabIndex = 42;
            this.labelCode.Text = "საქონელი:";
            this.labelCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.SystemColors.Window;
            this.txtName.Location = new System.Drawing.Point(240, 37);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(198, 24);
            this.txtName.TabIndex = 43;
            this.txtName.Tag = "ნომერი";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Window;
            this.txtCode.Location = new System.Drawing.Point(156, 37);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(78, 24);
            this.txtCode.TabIndex = 43;
            this.txtCode.Tag = "ნომერი";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblMonts);
            this.panel3.Location = new System.Drawing.Point(156, 67);
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
            this.panel2.Location = new System.Drawing.Point(13, 67);
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
            this.btnRefresh,
            this.btnClose,
            this.toolStripSeparator1,
            this.btnExcel});
            this.m_Toolbar.Location = new System.Drawing.Point(0, 0);
            this.m_Toolbar.Name = "m_Toolbar";
            this.m_Toolbar.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.m_Toolbar.Size = new System.Drawing.Size(1436, 25);
            this.m_Toolbar.TabIndex = 9;
            this.m_Toolbar.Text = "m_Toolbar";
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
            this.btnExcel.Visible = false;
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(172, 22);
            this.btnExport.Text = "ექსპორტი";
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(172, 22);
            this.btnPreview.Text = "დათვალიერება";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.m_Grid);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 124);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1436, 164);
            this.panel4.TabIndex = 12;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.m_Grid2);
            this.panel5.Location = new System.Drawing.Point(249, 299);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(940, 243);
            this.panel5.TabIndex = 13;
            // 
            // m_Grid2
            // 
            this.m_Grid2.AllowUserToAddRows = false;
            this.m_Grid2.AllowUserToDeleteRows = false;
            this.m_Grid2.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(228)))), ((int)(((byte)(248)))));
            this.m_Grid2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.m_Grid2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_Grid2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.m_Grid2.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_Grid2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.m_Grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_Grid2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_realize,
            this.col_single,
            this.col_discount,
            this.col_other,
            this.col_otherOther,
            this.col_sum});
            this.m_Grid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Grid2.Location = new System.Drawing.Point(0, 0);
            this.m_Grid2.Margin = new System.Windows.Forms.Padding(4);
            this.m_Grid2.MultiSelect = false;
            this.m_Grid2.Name = "m_Grid2";
            this.m_Grid2.ReadOnly = true;
            this.m_Grid2.RowHeadersVisible = false;
            this.m_Grid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_Grid2.Size = new System.Drawing.Size(940, 243);
            this.m_Grid2.TabIndex = 12;
          
            // 
            // col_realize
            // 
            this.col_realize.FillWeight = 110F;
            this.col_realize.HeaderText = "გაყიდვები";
            this.col_realize.Name = "col_realize";
            this.col_realize.ReadOnly = true;
            // 
            // col_single
            // 
            this.col_single.FillWeight = 90F;
            this.col_single.HeaderText = "საცალო";
            this.col_single.Name = "col_single";
            this.col_single.ReadOnly = true;
            // 
            // col_discount
            // 
            this.col_discount.FillWeight = 90F;
            this.col_discount.HeaderText = "ფასდაკლება";
            this.col_discount.Name = "col_discount";
            this.col_discount.ReadOnly = true;
            // 
            // col_other
            // 
            this.col_other.FillWeight = 90F;
            this.col_other.HeaderText = "საბითუმო";
            this.col_other.Name = "col_other";
            this.col_other.ReadOnly = true;
            // 
            // col_otherOther
            // 
            this.col_otherOther.FillWeight = 90F;
            this.col_otherOther.HeaderText = "სხვა";
            this.col_otherOther.Name = "col_otherOther";
            this.col_otherOther.ReadOnly = true;
            // 
            // col_sum
            // 
            this.col_sum.FillWeight = 90F;
            this.col_sum.HeaderText = "სულ";
            this.col_sum.Name = "col_sum";
            this.col_sum.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.FillWeight = 90F;
            this.dataGridViewTextBoxColumn1.HeaderText = "path";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 30F;
            this.dataGridViewTextBoxColumn2.HeaderText = "№";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.FillWeight = 200F;
            this.dataGridViewTextBoxColumn3.HeaderText = "დასახელება";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.FillWeight = 70.24408F;
            this.dataGridViewTextBoxColumn4.HeaderText = "რეალიზ. რაოდენობა";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.FillWeight = 88.70564F;
            this.dataGridViewTextBoxColumn5.HeaderText = "რეალიზ. თანხა დ.ღ.გ ს გარეშე";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.FillWeight = 90F;
            this.dataGridViewTextBoxColumn6.HeaderText = "წილი საცალო+ფასდაკლება (%)";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn7.FillWeight = 68.18006F;
            this.dataGridViewTextBoxColumn7.HeaderText = "წილი საბითუმო+სხვა (%)";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.FillWeight = 88.70564F;
            this.dataGridViewTextBoxColumn8.HeaderText = "საშუალო ნაშთი";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Visible = false;
            this.dataGridViewTextBoxColumn8.Width = 108;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.FillWeight = 52.69791F;
            this.dataGridViewTextBoxColumn9.HeaderText = "მოგება";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Width = 64;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.FillWeight = 88.70564F;
            this.dataGridViewTextBoxColumn10.HeaderText = "საშ. მოგება დ.ღ.გ ს გარეშე ";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Width = 108;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.FillWeight = 50F;
            this.dataGridViewTextBoxColumn11.HeaderText = "მოგების % რეალიზ. ფასიდან";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Width = 122;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.FillWeight = 50F;
            this.dataGridViewTextBoxColumn12.HeaderText = "წილი გაყიდვებში (%)";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.Width = 122;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.FillWeight = 50F;
            this.dataGridViewTextBoxColumn13.HeaderText = "წილი მოგებაში (%)";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.Width = 121;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.FillWeight = 80F;
            this.dataGridViewTextBoxColumn14.HeaderText = "რენტაბელობა (%)";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.Width = 122;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.FillWeight = 75F;
            this.dataGridViewTextBoxColumn15.HeaderText = "რაოდ. პერიოდის ბოლოს";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.Width = 138;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.FillWeight = 75F;
            this.dataGridViewTextBoxColumn16.HeaderText = "რაოდ. პერიოდის ბოლოს";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.Width = 138;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.FillWeight = 75F;
            this.dataGridViewTextBoxColumn17.HeaderText = "თანხა პერიოდის ბოლოს";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.Width = 138;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.FillWeight = 70F;
            this.dataGridViewTextBoxColumn18.HeaderText = "საშუალო ნაშთი (რაოდ)";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.Width = 128;
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.FillWeight = 70F;
            this.dataGridViewTextBoxColumn19.HeaderText = "საშუალო ნაშთი (თანხა)";
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.Width = 129;
            // 
            // ProfitabilityDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1436, 554);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panelPeriod);
            this.Controls.Add(this.m_Toolbar);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1452, 614);
            this.Name = "ProfitabilityDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "რენტაბელობა - დეტალური";
            ((System.ComponentModel.ISupportInitialize)(this.m_Grid)).EndInit();
            this.panelPeriod.ResumeLayout(false);
            this.panelPeriod.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.m_Toolbar.ResumeLayout(false);
            this.m_Toolbar.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_Grid2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView m_Grid;
        private System.Windows.Forms.Panel panelPeriod;
        private System.Windows.Forms.Label label3;
        private ipmControls.DateTimePickers m_Period;
        public System.Windows.Forms.ToolStrip m_Toolbar;
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
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridView m_Grid2;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnProduct;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_realize;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_single;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_discount;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_other;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_otherOther;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_sum;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton btnExcel;
        private System.Windows.Forms.ToolStripMenuItem btnExport;
        private System.Windows.Forms.ToolStripMenuItem btnPreview;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_selfCostNoVat;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_selfCostVat;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_outPriceSingle;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_outPriceDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_outPriceSabitumo;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_outPriceOther;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_restPeriodStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_amountPeriodStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_restPeriodEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_amountPeriodEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_quantityAverageRest;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_amountAverageRest;

    }
}