namespace ipmExtraFunctions
{
    partial class EmailSendGeneralForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailSendGeneralForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.btnSendEmail = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBCC = new System.Windows.Forms.TextBox();
            this.m_GridRecipients = new System.Windows.Forms.DataGridView();
            this.contact_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contact_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.param1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.param2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnFromList = new System.Windows.Forms.ToolStripMenuItem();
            this.btnManually = new System.Windows.Forms.ToolStripMenuItem();
            this.btnImportFromExcel = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelMailFrom = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.comboBoxEmailFrom = new System.Windows.Forms.ComboBox();
            this.txtBodyText = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_GridRecipients)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
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
            this.toolStrip1.Size = new System.Drawing.Size(734, 25);
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel7, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtBCC, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_GridRecipients, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtSubject, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxEmailFrom, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtBodyText, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 587);
            this.tableLayoutPanel1.TabIndex = 36;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label5);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(3, 35);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(194, 25);
            this.panel7.TabIndex = 41;
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
            this.txtBCC.Location = new System.Drawing.Point(203, 35);
            this.txtBCC.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtBCC.MaxLength = 500;
            this.txtBCC.Name = "txtBCC";
            this.txtBCC.Size = new System.Drawing.Size(521, 24);
            this.txtBCC.TabIndex = 40;
            this.txtBCC.Tag = "დასახელება";
            // 
            // m_GridRecipients
            // 
            this.m_GridRecipients.AllowUserToAddRows = false;
            this.m_GridRecipients.AllowUserToDeleteRows = false;
            this.m_GridRecipients.AllowUserToResizeRows = false;
            this.m_GridRecipients.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_GridRecipients.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.m_GridRecipients.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_GridRecipients.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.m_GridRecipients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_GridRecipients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.contact_id,
            this.dataGridViewTextBoxColumn1,
            this.contact_name,
            this.email,
            this.status,
            this.param1,
            this.param2,
            this.status_value});
            this.tableLayoutPanel1.SetColumnSpan(this.m_GridRecipients, 3);
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.RoyalBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_GridRecipients.DefaultCellStyle = dataGridViewCellStyle2;
            this.m_GridRecipients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_GridRecipients.Location = new System.Drawing.Point(3, 241);
            this.m_GridRecipients.Name = "m_GridRecipients";
            this.m_GridRecipients.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_GridRecipients.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.m_GridRecipients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_GridRecipients.Size = new System.Drawing.Size(728, 343);
            this.m_GridRecipients.TabIndex = 39;
            // 
            // contact_id
            // 
            this.contact_id.HeaderText = "contact_id";
            this.contact_id.Name = "contact_id";
            this.contact_id.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "ორგანიზაცია";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // contact_name
            // 
            this.contact_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.contact_name.HeaderText = "საკონტაქტო პირი";
            this.contact_name.Name = "contact_name";
            this.contact_name.ReadOnly = true;
            this.contact_name.Width = 160;
            // 
            // email
            // 
            this.email.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.email.HeaderText = "ელ. ფოსტა";
            this.email.Name = "email";
            // 
            // status
            // 
            this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.status.HeaderText = "სტატუსი";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Width = 89;
            // 
            // param1
            // 
            this.param1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.param1.HeaderText = "ველი <name>";
            this.param1.Name = "param1";
            this.param1.Width = 70;
            // 
            // param2
            // 
            this.param2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.param2.HeaderText = "ველი <param>";
            this.param2.Name = "param2";
            this.param2.Width = 70;
            // 
            // status_value
            // 
            this.status_value.HeaderText = "status_value";
            this.status_value.Name = "status_value";
            this.status_value.Visible = false;
            // 
            // toolStrip2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.toolStrip2, 3);
            this.toolStrip2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnImportFromExcel,
            this.btnDelete});
            this.toolStrip2.Location = new System.Drawing.Point(0, 213);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(734, 25);
            this.toolStrip2.TabIndex = 38;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnAdd
            // 
            this.btnAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFromList,
            this.btnManually});
            this.btnAdd.Image = global::ipmExtraFunctions.Properties.Resources.add_16;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(79, 22);
            this.btnAdd.Text = "დამატება";
            // 
            // btnFromList
            // 
            this.btnFromList.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.btnFromList.Name = "btnFromList";
            this.btnFromList.Size = new System.Drawing.Size(125, 22);
            this.btnFromList.Text = "სიიდან";
            this.btnFromList.Click += new System.EventHandler(this.btnFromList_Click);
            // 
            // btnManually
            // 
            this.btnManually.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.btnManually.Name = "btnManually";
            this.btnManually.Size = new System.Drawing.Size(125, 22);
            this.btnManually.Text = "ხელით";
            this.btnManually.Click += new System.EventHandler(this.btnManually_Click);
            // 
            // btnImportFromExcel
            // 
            this.btnImportFromExcel.Image = global::ipmExtraFunctions.Properties.Resources.Excel_icon;
            this.btnImportFromExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImportFromExcel.Name = "btnImportFromExcel";
            this.btnImportFromExcel.Size = new System.Drawing.Size(69, 22);
            this.btnImportFromExcel.Text = "იმპორტი";
            this.btnImportFromExcel.Click += new System.EventHandler(this.btnImportFromExcel_Click);
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
            // panel3
            // 
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 96);
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
            // panel4
            // 
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 66);
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
            this.txtSubject.Location = new System.Drawing.Point(203, 66);
            this.txtSubject.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtSubject.MaxLength = 50;
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(521, 24);
            this.txtSubject.TabIndex = 1;
            this.txtSubject.Tag = "დასახელება";
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
            this.comboBoxEmailFrom.Size = new System.Drawing.Size(521, 26);
            this.comboBoxEmailFrom.TabIndex = 3;
            this.comboBoxEmailFrom.Tag = "სახეობა";
            // 
            // txtBodyText
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtBodyText, 2);
            this.txtBodyText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBodyText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtBodyText.HideSelection = false;
            this.txtBodyText.Location = new System.Drawing.Point(203, 96);
            this.txtBodyText.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.txtBodyText.MaxLength = 10000;
            this.txtBodyText.Multiline = true;
            this.txtBodyText.Name = "txtBodyText";
            this.txtBodyText.Size = new System.Drawing.Size(521, 107);
            this.txtBodyText.TabIndex = 2;
            this.txtBodyText.Tag = "დასახელება";
            // 
            // EmailSendGeneralForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 612);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EmailSendGeneralForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ელ. ფოსტის გაგზავნა";
            this.Load += new System.EventHandler(this.EmailSendGeneralForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_GridRecipients)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.ToolStripButton btnSendEmail;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelMailFrom;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.ComboBox comboBoxEmailFrom;
        private System.Windows.Forms.TextBox txtBodyText;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripDropDownButton btnAdd;
        private System.Windows.Forms.ToolStripMenuItem btnFromList;
        private System.Windows.Forms.ToolStripMenuItem btnManually;
        private System.Windows.Forms.ToolStripButton btnImportFromExcel;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.DataGridView m_GridRecipients;
        private System.Windows.Forms.DataGridViewTextBoxColumn contact_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn contact_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn email;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn param1;
        private System.Windows.Forms.DataGridViewTextBoxColumn param2;
        private System.Windows.Forms.DataGridViewTextBoxColumn status_value;
        private System.Windows.Forms.TextBox txtBCC;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label5;
    }
}