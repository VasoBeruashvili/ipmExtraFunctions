namespace ipmExtraFunctions
{
    partial class DeleteOperations
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeleteOperations));
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkAll = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.labelName = new System.Windows.Forms.Label();
            this.m_Picker = new ipmControls.DateTimePickers();
            this.btnDel = new System.Windows.Forms.Button();
            this.tabBases = new System.Windows.Forms.TabControl();
            this.tabPageDelete = new System.Windows.Forms.TabPage();
            this.tabPageRest = new System.Windows.Forms.TabPage();
            this.btnRest = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblStore = new System.Windows.Forms.Label();
            this.comboMethod = new System.Windows.Forms.ComboBox();
            this.comboStore = new System.Windows.Forms.ComboBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDate = new System.Windows.Forms.DateTimePicker();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel12.SuspendLayout();
            this.tabBases.SuspendLayout();
            this.tabPageDelete.SuspendLayout();
            this.tabPageRest.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkAll);
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel12);
            this.panel1.Controls.Add(this.m_Picker);
            this.panel1.Location = new System.Drawing.Point(6, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(434, 93);
            this.panel1.TabIndex = 7;
            // 
            // checkAll
            // 
            this.checkAll.AutoSize = true;
            this.checkAll.Enabled = false;
            this.checkAll.Location = new System.Drawing.Point(237, 67);
            this.checkAll.Name = "checkAll";
            this.checkAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkAll.Size = new System.Drawing.Size(194, 22);
            this.checkAll.TabIndex = 57;
            this.checkAll.Text = ":ყველა ოპერაციის წაშლა";
            this.checkAll.UseVisualStyleBackColor = true;
            this.checkAll.Visible = false;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtPassword.Location = new System.Drawing.Point(104, 39);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(315, 24);
            this.txtPassword.TabIndex = 56;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(12, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(84, 25);
            this.panel2.TabIndex = 55;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(17, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "პაროლი:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.labelName);
            this.panel12.Location = new System.Drawing.Point(12, 8);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(84, 25);
            this.panel12.TabIndex = 54;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelName.Location = new System.Drawing.Point(11, 0);
            this.labelName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(73, 18);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "პერიოდი:";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_Picker
            // 
            this.m_Picker.BackColor = System.Drawing.Color.Transparent;
            this.m_Picker.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_Picker.Location = new System.Drawing.Point(103, 7);
            this.m_Picker.Margin = new System.Windows.Forms.Padding(4);
            this.m_Picker.Name = "m_Picker";
            this.m_Picker.ProgramManager = null;
            this.m_Picker.Size = new System.Drawing.Size(316, 26);
            this.m_Picker.TabIndex = 5;
            // 
            // btnDel
            // 
            this.btnDel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.btnDel.Location = new System.Drawing.Point(339, 109);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(86, 29);
            this.btnDel.TabIndex = 8;
            this.btnDel.Text = "წაშლა";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // tabBases
            // 
            this.tabBases.Controls.Add(this.tabPageDelete);
            this.tabBases.Controls.Add(this.tabPageRest);
            this.tabBases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabBases.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.tabBases.Location = new System.Drawing.Point(0, 0);
            this.tabBases.Name = "tabBases";
            this.tabBases.SelectedIndex = 0;
            this.tabBases.Size = new System.Drawing.Size(466, 176);
            this.tabBases.TabIndex = 10;
            // 
            // tabPageDelete
            // 
            this.tabPageDelete.Controls.Add(this.panel1);
            this.tabPageDelete.Controls.Add(this.btnDel);
            this.tabPageDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.tabPageDelete.Location = new System.Drawing.Point(4, 27);
            this.tabPageDelete.Name = "tabPageDelete";
            this.tabPageDelete.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDelete.Size = new System.Drawing.Size(458, 145);
            this.tabPageDelete.TabIndex = 0;
            this.tabPageDelete.Text = "ოპერაციების წაშლა";
            this.tabPageDelete.UseVisualStyleBackColor = true;
            // 
            // tabPageRest
            // 
            this.tabPageRest.Controls.Add(this.btnRest);
            this.tabPageRest.Controls.Add(this.panel3);
            this.tabPageRest.Controls.Add(this.panel6);
            this.tabPageRest.Controls.Add(this.comboMethod);
            this.tabPageRest.Controls.Add(this.comboStore);
            this.tabPageRest.Controls.Add(this.panel5);
            this.tabPageRest.Controls.Add(this.txtDate);
            this.tabPageRest.Location = new System.Drawing.Point(4, 27);
            this.tabPageRest.Name = "tabPageRest";
            this.tabPageRest.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRest.Size = new System.Drawing.Size(458, 145);
            this.tabPageRest.TabIndex = 1;
            this.tabPageRest.Text = "ნაშთების ექსპორტი";
            this.tabPageRest.UseVisualStyleBackColor = true;
            // 
            // btnRest
            // 
            this.btnRest.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.btnRest.Location = new System.Drawing.Point(312, 91);
            this.btnRest.Name = "btnRest";
            this.btnRest.Size = new System.Drawing.Size(86, 26);
            this.btnRest.TabIndex = 9;
            this.btnRest.Text = "ექსპორტი";
            this.btnRest.UseVisualStyleBackColor = true;
            this.btnRest.Click += new System.EventHandler(this.btnRest_Click);
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(39, 28);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(71, 22);
            this.panel3.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(4, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 18);
            this.label3.TabIndex = 32;
            this.label3.Text = "მეთოდი:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel6
            // 
            this.panel6.AutoSize = true;
            this.panel6.Controls.Add(this.lblStore);
            this.panel6.Location = new System.Drawing.Point(39, 60);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(71, 22);
            this.panel6.TabIndex = 5;
            // 
            // lblStore
            // 
            this.lblStore.AutoSize = true;
            this.lblStore.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStore.Location = new System.Drawing.Point(5, 0);
            this.lblStore.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStore.Name = "lblStore";
            this.lblStore.Size = new System.Drawing.Size(66, 18);
            this.lblStore.TabIndex = 32;
            this.lblStore.Text = "საწყობი:";
            this.lblStore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboMethod
            // 
            this.comboMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.comboMethod.FormattingEnabled = true;
            this.comboMethod.Location = new System.Drawing.Point(113, 25);
            this.comboMethod.Name = "comboMethod";
            this.comboMethod.Size = new System.Drawing.Size(285, 26);
            this.comboMethod.TabIndex = 6;
            // 
            // comboStore
            // 
            this.comboStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.comboStore.FormattingEnabled = true;
            this.comboStore.Location = new System.Drawing.Point(113, 57);
            this.comboStore.Name = "comboStore";
            this.comboStore.Size = new System.Drawing.Size(285, 26);
            this.comboStore.TabIndex = 6;
            // 
            // panel5
            // 
            this.panel5.AutoSize = true;
            this.panel5.Controls.Add(this.label2);
            this.panel5.Location = new System.Drawing.Point(39, 94);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(71, 25);
            this.panel5.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(5, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 18);
            this.label2.TabIndex = 14;
            this.label2.Text = "თარიღი:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDate
            // 
            this.txtDate.AllowDrop = true;
            this.txtDate.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.txtDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txtDate.Location = new System.Drawing.Point(113, 93);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(190, 24);
            this.txtDate.TabIndex = 1;
            // 
            // DeleteOperations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 176);
            this.Controls.Add(this.tabBases);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DeleteOperations";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ბაზის ოპერაციები";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.tabBases.ResumeLayout(false);
            this.tabPageDelete.ResumeLayout(false);
            this.tabPageRest.ResumeLayout(false);
            this.tabPageRest.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ipmControls.DateTimePickers m_Picker;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TabControl tabBases;
        private System.Windows.Forms.TabPage tabPageDelete;
        private System.Windows.Forms.TabPage tabPageRest;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker txtDate;
        private System.Windows.Forms.Button btnRest;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label lblStore;
        private System.Windows.Forms.ComboBox comboStore;
        private System.Windows.Forms.CheckBox checkAll;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboMethod;
    }
}