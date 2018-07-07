namespace ipmExtraFunctions
{
    partial class ContragentRelationElementForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContragentRelationElementForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.btnRefResh = new System.Windows.Forms.ToolStripButton();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.m_Tree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblFullDebt = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkGenerateTypes = new System.Windows.Forms.CheckBox();
            this.chckSearchByCode = new System.Windows.Forms.CheckBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtTel = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxGeneral = new System.Windows.Forms.GroupBox();
            this.txtDebt = new ipmControls.TextBoxDecimalInput();
            this.txtParentName = new System.Windows.Forms.TextBox();
            this.panelName = new System.Windows.Forms.Panel();
            this.labelName = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.panel23 = new System.Windows.Forms.Panel();
            this.labelDebt = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBoxGeneral.SuspendLayout();
            this.panelName.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel23.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.toolStrip1, 2);
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExit,
            this.btnRefResh,
            this.btnAdd,
            this.btnEdit,
            this.btnDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(976, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnExit
            // 
            this.btnExit.Image = global::ipmExtraFunctions.Properties.Resources.exit_161;
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(68, 22);
            this.btnExit.Text = "დახურვა";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRefResh
            // 
            this.btnRefResh.Image = global::ipmExtraFunctions.Properties.Resources.refresh_16;
            this.btnRefResh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefResh.Name = "btnRefResh";
            this.btnRefResh.Size = new System.Drawing.Size(74, 22);
            this.btnRefResh.Text = "განახლება";
            this.btnRefResh.Click += new System.EventHandler(this.btnRefResh_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::ipmExtraFunctions.Properties.Resources.add_16;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(70, 22);
            this.btnAdd.Text = "დამატება";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::ipmExtraFunctions.Properties.Resources.edit_16;
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(90, 22);
            this.btnEdit.Text = "რედაქტირება";
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::ipmExtraFunctions.Properties.Resources.delete_16;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(57, 22);
            this.btnDelete.Text = "წაშლა";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // m_Tree
            // 
            this.m_Tree.AllowDrop = true;
            this.m_Tree.BackColor = System.Drawing.Color.White;
            this.m_Tree.Cursor = System.Windows.Forms.Cursors.Hand;
            this.m_Tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Tree.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_Tree.HideSelection = false;
            this.m_Tree.HotTracking = true;
            this.m_Tree.ImageIndex = 0;
            this.m_Tree.ImageList = this.imageList1;
            this.m_Tree.Location = new System.Drawing.Point(3, 74);
            this.m_Tree.Name = "m_Tree";
            this.m_Tree.SelectedImageIndex = 0;
            this.m_Tree.Size = new System.Drawing.Size(608, 486);
            this.m_Tree.TabIndex = 8;
            this.m_Tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.m_Tree_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "customer_16.png");
            this.imageList1.Images.SetKeyName(1, "folder.png");
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblFullDebt, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.m_Tree, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(976, 591);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // lblFullDebt
            // 
            this.lblFullDebt.AutoSize = true;
            this.lblFullDebt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFullDebt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFullDebt.Location = new System.Drawing.Point(4, 563);
            this.lblFullDebt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFullDebt.Name = "lblFullDebt";
            this.lblFullDebt.Size = new System.Drawing.Size(606, 28);
            this.lblFullDebt.TabIndex = 15;
            this.lblFullDebt.Text = "სრული დავალიანება: 0.00 ლარი";
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.checkGenerateTypes);
            this.panel1.Controls.Add(this.chckSearchByCode);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(970, 40);
            this.panel1.TabIndex = 9;
            // 
            // checkGenerateTypes
            // 
            this.checkGenerateTypes.AutoSize = true;
            this.checkGenerateTypes.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.checkGenerateTypes.Location = new System.Drawing.Point(632, 10);
            this.checkGenerateTypes.Name = "checkGenerateTypes";
            this.checkGenerateTypes.Size = new System.Drawing.Size(153, 22);
            this.checkGenerateTypes.TabIndex = 20;
            this.checkGenerateTypes.Text = "ტიპების გენერაცია";
            this.checkGenerateTypes.UseVisualStyleBackColor = true;
            // 
            // chckSearchByCode
            // 
            this.chckSearchByCode.AutoSize = true;
            this.chckSearchByCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.chckSearchByCode.Location = new System.Drawing.Point(442, 10);
            this.chckSearchByCode.Name = "chckSearchByCode";
            this.chckSearchByCode.Size = new System.Drawing.Size(174, 22);
            this.chckSearchByCode.TabIndex = 20;
            this.chckSearchByCode.Text = "ძებნა პირადი ნომრით";
            this.chckSearchByCode.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtSearch.Location = new System.Drawing.Point(100, 9);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(210, 24);
            this.txtSearch.TabIndex = 19;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(4, 8);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 24);
            this.label6.TabIndex = 18;
            this.label6.Text = "კლიენტი:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.btnSearch.Image = global::ipmExtraFunctions.Properties.Resources.search_16;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(316, 6);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(120, 30);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "ძებნა";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.groupBoxGeneral);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(617, 74);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(356, 486);
            this.panel2.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAddress);
            this.groupBox1.Controls.Add(this.txtTel);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.panel6);
            this.groupBox1.Controls.Add(this.panel5);
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.groupBox1.Location = new System.Drawing.Point(0, 199);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(356, 287);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "დამატებითი ინფორმაცია";
            // 
            // txtAddress
            // 
            this.txtAddress.BackColor = System.Drawing.Color.White;
            this.txtAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtAddress.Location = new System.Drawing.Point(8, 165);
            this.txtAddress.MaxLength = 150;
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.ReadOnly = true;
            this.txtAddress.Size = new System.Drawing.Size(340, 111);
            this.txtAddress.TabIndex = 31;
            this.txtAddress.Tag = "მისამართი";
            // 
            // txtTel
            // 
            this.txtTel.BackColor = System.Drawing.Color.White;
            this.txtTel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtTel.Location = new System.Drawing.Point(8, 108);
            this.txtTel.MaxLength = 11;
            this.txtTel.Name = "txtTel";
            this.txtTel.ReadOnly = true;
            this.txtTel.Size = new System.Drawing.Size(140, 24);
            this.txtTel.TabIndex = 29;
            this.txtTel.Tag = "კოდი";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtCode.Location = new System.Drawing.Point(8, 51);
            this.txtCode.MaxLength = 11;
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(140, 24);
            this.txtCode.TabIndex = 29;
            this.txtCode.Tag = "კოდი";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label4);
            this.panel6.Location = new System.Drawing.Point(8, 138);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(217, 25);
            this.panel6.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "მისამართი:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label2);
            this.panel5.Location = new System.Drawing.Point(8, 81);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(217, 25);
            this.panel5.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "ტელეფონი:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label3);
            this.panel4.Location = new System.Drawing.Point(8, 25);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(217, 25);
            this.panel4.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "პირადი ნომერი:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBoxGeneral
            // 
            this.groupBoxGeneral.Controls.Add(this.txtDebt);
            this.groupBoxGeneral.Controls.Add(this.txtParentName);
            this.groupBoxGeneral.Controls.Add(this.panelName);
            this.groupBoxGeneral.Controls.Add(this.panel3);
            this.groupBoxGeneral.Controls.Add(this.txtName);
            this.groupBoxGeneral.Controls.Add(this.panel23);
            this.groupBoxGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.groupBoxGeneral.Location = new System.Drawing.Point(0, 0);
            this.groupBoxGeneral.Name = "groupBoxGeneral";
            this.groupBoxGeneral.Size = new System.Drawing.Size(356, 199);
            this.groupBoxGeneral.TabIndex = 32;
            this.groupBoxGeneral.TabStop = false;
            this.groupBoxGeneral.Text = "ძირითადი";
            // 
            // txtDebt
            // 
            this.txtDebt.Location = new System.Drawing.Point(8, 104);
            this.txtDebt.Name = "txtDebt";
            this.txtDebt.Size = new System.Drawing.Size(140, 24);
            this.txtDebt.TabIndex = 28;
            this.txtDebt.Text = "0.00";
            // 
            // txtParentName
            // 
            this.txtParentName.BackColor = System.Drawing.Color.White;
            this.txtParentName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtParentName.Location = new System.Drawing.Point(8, 161);
            this.txtParentName.MaxLength = 100;
            this.txtParentName.Name = "txtParentName";
            this.txtParentName.ReadOnly = true;
            this.txtParentName.Size = new System.Drawing.Size(340, 24);
            this.txtParentName.TabIndex = 11;
            this.txtParentName.Tag = "დასახელება";
            // 
            // panelName
            // 
            this.panelName.Controls.Add(this.labelName);
            this.panelName.Location = new System.Drawing.Point(8, 25);
            this.panelName.Name = "panelName";
            this.panelName.Size = new System.Drawing.Size(145, 25);
            this.panelName.TabIndex = 10;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelName.Location = new System.Drawing.Point(0, 0);
            this.labelName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(73, 18);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "კლიენტი:";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(8, 134);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(217, 25);
            this.panel3.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "ზემდგომი კლიენტი:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtName.Location = new System.Drawing.Point(8, 51);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(340, 24);
            this.txtName.TabIndex = 11;
            this.txtName.Tag = "დასახელება";
            // 
            // panel23
            // 
            this.panel23.Controls.Add(this.labelDebt);
            this.panel23.Location = new System.Drawing.Point(8, 80);
            this.panel23.Name = "panel23";
            this.panel23.Size = new System.Drawing.Size(174, 23);
            this.panel23.TabIndex = 27;
            // 
            // labelDebt
            // 
            this.labelDebt.AutoSize = true;
            this.labelDebt.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDebt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelDebt.Location = new System.Drawing.Point(0, 0);
            this.labelDebt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDebt.Name = "labelDebt";
            this.labelDebt.Size = new System.Drawing.Size(150, 18);
            this.labelDebt.TabIndex = 14;
            this.labelDebt.Text = "დავალიანება (ლარი):";
            this.labelDebt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ContragentRelationElementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 591);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ContragentRelationElementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "კლიენტების სტრუქტურა";
            this.Load += new System.EventHandler(this.ContragentRelationElementForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.groupBoxGeneral.ResumeLayout(false);
            this.groupBoxGeneral.PerformLayout();
            this.panelName.ResumeLayout(false);
            this.panelName.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel23.ResumeLayout(false);
            this.panel23.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnDelete;
        public System.Windows.Forms.TreeView m_Tree;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Panel panelName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Panel panel23;
        private System.Windows.Forms.Label labelDebt;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtParentName;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtTel;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBoxGeneral;
        private System.Windows.Forms.CheckBox chckSearchByCode;
        private System.Windows.Forms.ToolStripButton btnRefResh;
        private ipmControls.TextBoxDecimalInput txtDebt;
        private System.Windows.Forms.Label lblFullDebt;
        private System.Windows.Forms.CheckBox checkGenerateTypes;
    }
}