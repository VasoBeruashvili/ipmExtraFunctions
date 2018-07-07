namespace ipmExtraFunctions
{
    partial class UniPayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UniPayForm));
            this.tabControlUni = new System.Windows.Forms.TabControl();
            this.tabPagePayUni = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnContragentSubAccount = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnContragents = new System.Windows.Forms.Button();
            this.txtContragent = new System.Windows.Forms.TextBox();
            this.comboContragent = new System.Windows.Forms.ComboBox();
            this.txtDate = new System.Windows.Forms.DateTimePicker();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.labelName = new System.Windows.Forms.Label();
            this.tabControlUni.SuspendLayout();
            this.tabPagePayUni.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlUni
            // 
            this.tabControlUni.Controls.Add(this.tabPagePayUni);
            this.tabControlUni.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlUni.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControlUni.Location = new System.Drawing.Point(0, 0);
            this.tabControlUni.Name = "tabControlUni";
            this.tabControlUni.SelectedIndex = 0;
            this.tabControlUni.Size = new System.Drawing.Size(546, 206);
            this.tabControlUni.TabIndex = 2;
            // 
            // tabPagePayUni
            // 
            this.tabPagePayUni.Controls.Add(this.panel1);
            this.tabPagePayUni.Location = new System.Drawing.Point(4, 29);
            this.tabPagePayUni.Name = "tabPagePayUni";
            this.tabPagePayUni.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePayUni.Size = new System.Drawing.Size(538, 173);
            this.tabPagePayUni.TabIndex = 0;
            this.tabPagePayUni.Text = "დარიცხვა";
            this.tabPagePayUni.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnContragentSubAccount);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnContragents);
            this.panel1.Controls.Add(this.txtContragent);
            this.panel1.Controls.Add(this.comboContragent);
            this.panel1.Controls.Add(this.txtDate);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(532, 167);
            this.panel1.TabIndex = 6;
            // 
            // btnContragentSubAccount
            // 
            this.btnContragentSubAccount.Image = global::ipmExtraFunctions.Properties.Resources.flash_16;
            this.btnContragentSubAccount.Location = new System.Drawing.Point(391, 117);
            this.btnContragentSubAccount.Name = "btnContragentSubAccount";
            this.btnContragentSubAccount.Size = new System.Drawing.Size(113, 27);
            this.btnContragentSubAccount.TabIndex = 55;
            this.btnContragentSubAccount.Text = "დარიცხვა";
            this.btnContragentSubAccount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnContragentSubAccount.UseVisualStyleBackColor = true;
            this.btnContragentSubAccount.Click += new System.EventHandler(this.btnContragentSubAccount_Click);
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(7, 89);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(103, 23);
            this.panel2.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(42, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 18);
            this.label1.TabIndex = 18;
            this.label1.Text = "გრანტი:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnContragents
            // 
            this.btnContragents.Image = ((System.Drawing.Image)(resources.GetObject("btnContragents.Image")));
            this.btnContragents.Location = new System.Drawing.Point(110, 87);
            this.btnContragents.Name = "btnContragents";
            this.btnContragents.Size = new System.Drawing.Size(24, 24);
            this.btnContragents.TabIndex = 8;
            this.btnContragents.UseVisualStyleBackColor = true;
            this.btnContragents.Click += new System.EventHandler(this.btnContragents_Click);
            // 
            // txtContragent
            // 
            this.txtContragent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtContragent.Location = new System.Drawing.Point(141, 87);
            this.txtContragent.Name = "txtContragent";
            this.txtContragent.ReadOnly = true;
            this.txtContragent.Size = new System.Drawing.Size(363, 24);
            this.txtContragent.TabIndex = 7;
            this.txtContragent.Tag = "კონტრაგენტი";
            // 
            // comboContragent
            // 
            this.comboContragent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboContragent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.comboContragent.FormattingEnabled = true;
            this.comboContragent.Location = new System.Drawing.Point(115, 48);
            this.comboContragent.Name = "comboContragent";
            this.comboContragent.Size = new System.Drawing.Size(389, 26);
            this.comboContragent.TabIndex = 5;
            // 
            // txtDate
            // 
            this.txtDate.AllowDrop = true;
            this.txtDate.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.txtDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txtDate.Location = new System.Drawing.Point(115, 17);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(261, 26);
            this.txtDate.TabIndex = 1;
            // 
            // panel7
            // 
            this.panel7.AutoSize = true;
            this.panel7.Controls.Add(this.label3);
            this.panel7.Location = new System.Drawing.Point(7, 51);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(104, 23);
            this.panel7.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 18);
            this.label3.TabIndex = 18;
            this.label3.Text = "განყოფილება:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel5
            // 
            this.panel5.AutoSize = true;
            this.panel5.Controls.Add(this.labelName);
            this.panel5.Location = new System.Drawing.Point(42, 18);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(67, 25);
            this.panel5.TabIndex = 2;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelName.Location = new System.Drawing.Point(1, 0);
            this.labelName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(66, 18);
            this.labelName.TabIndex = 14;
            this.labelName.Text = "თარიღი:";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UniPayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 206);
            this.Controls.Add(this.tabControlUni);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UniPayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "უნივერსიტეტის ოპერაციები";
            this.Load += new System.EventHandler(this.UniPayForm_Load);
            this.tabControlUni.ResumeLayout(false);
            this.tabPagePayUni.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlUni;
        private System.Windows.Forms.TabPage tabPagePayUni;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.DateTimePicker txtDate;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboContragent;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnContragents;
        private System.Windows.Forms.TextBox txtContragent;
        private System.Windows.Forms.Button btnContragentSubAccount;

    }
}