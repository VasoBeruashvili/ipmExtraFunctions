namespace ipmExtraFunctions
{
    partial class EmailSendForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailSendForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.btnSendEmail = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelMailFrom = new System.Windows.Forms.Label();
            this.comboBoxEmailFrom = new System.Windows.Forms.ComboBox();
            this.comboBoxTemplate = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBodyText = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.btnAddFile = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteEmail = new System.Windows.Forms.ToolStripButton();
            this.m_GridFiles = new System.Windows.Forms.DataGridView();
            this.icon = new System.Windows.Forms.DataGridViewImageColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.file_path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEmailTo = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBCC = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_GridFiles)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExit,
            this.btnSendEmail});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(791, 25);
            this.toolStrip1.TabIndex = 6;
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
            // btnSendEmail
            // 
            this.btnSendEmail.Image = global::ipmExtraFunctions.Properties.Resources.mail_out;
            this.btnSendEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSendEmail.Name = "btnSendEmail";
            this.btnSendEmail.Size = new System.Drawing.Size(67, 22);
            this.btnSendEmail.Text = "გაგზავნა";
            this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelMailFrom);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(194, 25);
            this.panel2.TabIndex = 7;
            // 
            // labelMailFrom
            // 
            this.labelMailFrom.AutoSize = true;
            this.labelMailFrom.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelMailFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelMailFrom.Location = new System.Drawing.Point(31, 0);
            this.labelMailFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMailFrom.Name = "labelMailFrom";
            this.labelMailFrom.Size = new System.Drawing.Size(163, 18);
            this.labelMailFrom.TabIndex = 0;
            this.labelMailFrom.Text = "გამგზავნის ელ. ფოსტა:";
            this.labelMailFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxEmailFrom
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxEmailFrom, 2);
            this.comboBoxEmailFrom.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxEmailFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEmailFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.comboBoxEmailFrom.FormattingEnabled = true;
            this.comboBoxEmailFrom.Location = new System.Drawing.Point(203, 3);
            this.comboBoxEmailFrom.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.comboBoxEmailFrom.Name = "comboBoxEmailFrom";
            this.comboBoxEmailFrom.Size = new System.Drawing.Size(578, 26);
            this.comboBoxEmailFrom.TabIndex = 3;
            this.comboBoxEmailFrom.Tag = "სახეობა";
            this.comboBoxEmailFrom.SelectedValueChanged += new System.EventHandler(this.comboBoxEmailFrom_SelectedValueChanged);
            // 
            // comboBoxTemplate
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxTemplate, 2);
            this.comboBoxTemplate.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.comboBoxTemplate.FormattingEnabled = true;
            this.comboBoxTemplate.Location = new System.Drawing.Point(203, 97);
            this.comboBoxTemplate.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.comboBoxTemplate.Name = "comboBoxTemplate";
            this.comboBoxTemplate.Size = new System.Drawing.Size(578, 26);
            this.comboBoxTemplate.TabIndex = 6;
            this.comboBoxTemplate.Tag = "სახეობა";
            this.comboBoxTemplate.SelectedValueChanged += new System.EventHandler(this.comboBoxTemplate_SelectedValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 97);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 25);
            this.panel1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(120, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "შაბლონი:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 159);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(194, 20);
            this.panel3.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(133, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "ტექსტი:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBodyText
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtBodyText, 2);
            this.txtBodyText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBodyText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtBodyText.HideSelection = false;
            this.txtBodyText.Location = new System.Drawing.Point(203, 159);
            this.txtBodyText.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.txtBodyText.MaxLength = 10000;
            this.txtBodyText.Multiline = true;
            this.txtBodyText.Name = "txtBodyText";
            this.txtBodyText.Size = new System.Drawing.Size(578, 169);
            this.txtBodyText.TabIndex = 2;
            this.txtBodyText.Tag = "დასახელება";
            this.txtBodyText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBodyText_KeyDown);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxTemplate, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtSubject, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxEmailFrom, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtBodyText, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.panel6, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtEmailTo, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel7, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtBCC, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(791, 508);
            this.tableLayoutPanel1.TabIndex = 35;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 129);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(194, 20);
            this.panel4.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(122, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "სათაური:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSubject
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtSubject, 2);
            this.txtSubject.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtSubject.Location = new System.Drawing.Point(203, 129);
            this.txtSubject.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtSubject.MaxLength = 50;
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(578, 24);
            this.txtSubject.TabIndex = 1;
            this.txtSubject.Tag = "დასახელება";
            // 
            // panel5
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel5, 3);
            this.panel5.Controls.Add(this.toolStrip3);
            this.panel5.Controls.Add(this.m_GridFiles);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(3, 341);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(785, 164);
            this.panel5.TabIndex = 37;
            // 
            // toolStrip3
            // 
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddFile,
            this.btnDeleteEmail});
            this.toolStrip3.Location = new System.Drawing.Point(0, 4);
            this.toolStrip3.Margin = new System.Windows.Forms.Padding(3);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(785, 25);
            this.toolStrip3.TabIndex = 3;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // btnAddFile
            // 
            this.btnAddFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.btnAddFile.Image = global::ipmExtraFunctions.Properties.Resources.attachment;
            this.btnAddFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(122, 22);
            this.btnAddFile.Text = "ფაილის მიბმა";
            this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
            // 
            // btnDeleteEmail
            // 
            this.btnDeleteEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.btnDeleteEmail.Image = global::ipmExtraFunctions.Properties.Resources.delete_16;
            this.btnDeleteEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteEmail.Name = "btnDeleteEmail";
            this.btnDeleteEmail.Size = new System.Drawing.Size(71, 22);
            this.btnDeleteEmail.Text = "წაშლა";
            this.btnDeleteEmail.Click += new System.EventHandler(this.btnDeleteEmail_Click);
            // 
            // m_GridFiles
            // 
            this.m_GridFiles.AllowUserToAddRows = false;
            this.m_GridFiles.AllowUserToDeleteRows = false;
            this.m_GridFiles.AllowUserToResizeRows = false;
            this.m_GridFiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_GridFiles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.m_GridFiles.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_GridFiles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.m_GridFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_GridFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.icon,
            this.name,
            this.file_path});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_GridFiles.DefaultCellStyle = dataGridViewCellStyle2;
            this.m_GridFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_GridFiles.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.m_GridFiles.Location = new System.Drawing.Point(0, 29);
            this.m_GridFiles.Name = "m_GridFiles";
            this.m_GridFiles.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_GridFiles.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.m_GridFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_GridFiles.Size = new System.Drawing.Size(785, 135);
            this.m_GridFiles.TabIndex = 4;
            // 
            // icon
            // 
            this.icon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.icon.HeaderText = " ";
            this.icon.Name = "icon";
            this.icon.ReadOnly = true;
            this.icon.Width = 19;
            // 
            // name
            // 
            this.name.HeaderText = "სახელი";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // file_path
            // 
            this.file_path.HeaderText = "file_path";
            this.file_path.Name = "file_path";
            this.file_path.Visible = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label4);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 35);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(194, 25);
            this.panel6.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(43, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "მიმღების ელ. ფოსტა:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEmailTo
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtEmailTo, 2);
            this.txtEmailTo.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtEmailTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtEmailTo.Location = new System.Drawing.Point(203, 35);
            this.txtEmailTo.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtEmailTo.MaxLength = 500;
            this.txtEmailTo.Name = "txtEmailTo";
            this.txtEmailTo.Size = new System.Drawing.Size(578, 24);
            this.txtEmailTo.TabIndex = 4;
            this.txtEmailTo.Tag = "დასახელება";
            this.txtEmailTo.TextChanged += new System.EventHandler(this.txtEmailTo_TextChanged);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label5);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(3, 66);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(194, 25);
            this.panel7.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(150, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "BCC:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBCC
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtBCC, 2);
            this.txtBCC.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtBCC.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtBCC.Location = new System.Drawing.Point(203, 66);
            this.txtBCC.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtBCC.MaxLength = 500;
            this.txtBCC.Name = "txtBCC";
            this.txtBCC.Size = new System.Drawing.Size(578, 24);
            this.txtBCC.TabIndex = 5;
            this.txtBCC.Tag = "დასახელება";
            this.txtBCC.TextChanged += new System.EventHandler(this.txtBCC_TextChanged);
            // 
            // EmailSendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 533);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EmailSendForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ელ. ფოსტის გაგზავნა: ახალი";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_GridFiles)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelMailFrom;
        private System.Windows.Forms.ComboBox comboBoxEmailFrom;
        private System.Windows.Forms.ComboBox comboBoxTemplate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBodyText;
        private System.Windows.Forms.ToolStripButton btnSendEmail;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.DataGridView m_GridFiles;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton btnAddFile;
        private System.Windows.Forms.ToolStripButton btnDeleteEmail;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtEmailTo;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBCC;
        private System.Windows.Forms.DataGridViewImageColumn icon;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn file_path;
    }
}