namespace ipmExtraFunctions
{
    partial class FinaDebtExport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinaDebtExport));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.labelName = new System.Windows.Forms.Label();
            this.btnExposrt = new System.Windows.Forms.Button();
            this.m_Picker = new ipmControls.DateTimePickers();
            this.tabBases = new System.Windows.Forms.TabControl();
            this.tabPageDelete = new System.Windows.Forms.TabPage();
            this.panel1.SuspendLayout();
            this.panel12.SuspendLayout();
            this.tabBases.SuspendLayout();
            this.tabPageDelete.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel12);
            this.panel1.Controls.Add(this.btnExposrt);
            this.panel1.Controls.Add(this.m_Picker);
            this.panel1.Location = new System.Drawing.Point(6, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(434, 88);
            this.panel1.TabIndex = 7;
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
            // btnExposrt
            // 
            this.btnExposrt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.btnExposrt.Location = new System.Drawing.Point(327, 40);
            this.btnExposrt.Name = "btnExposrt";
            this.btnExposrt.Size = new System.Drawing.Size(92, 29);
            this.btnExposrt.TabIndex = 8;
            this.btnExposrt.Text = "ექსპორტი";
            this.btnExposrt.UseVisualStyleBackColor = true;
            this.btnExposrt.Click += new System.EventHandler(this.btnExposrt_Click);
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
            // tabBases
            // 
            this.tabBases.Controls.Add(this.tabPageDelete);
            this.tabBases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabBases.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.tabBases.Location = new System.Drawing.Point(0, 0);
            this.tabBases.Name = "tabBases";
            this.tabBases.SelectedIndex = 0;
            this.tabBases.Size = new System.Drawing.Size(466, 138);
            this.tabBases.TabIndex = 10;
            // 
            // tabPageDelete
            // 
            this.tabPageDelete.Controls.Add(this.panel1);
            this.tabPageDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.tabPageDelete.Location = new System.Drawing.Point(4, 27);
            this.tabPageDelete.Name = "tabPageDelete";
            this.tabPageDelete.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDelete.Size = new System.Drawing.Size(458, 107);
            this.tabPageDelete.TabIndex = 0;
            this.tabPageDelete.Text = "დავალიანების ექსპორტი";
            this.tabPageDelete.UseVisualStyleBackColor = true;
            // 
            // FinaDebtExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 138);
            this.Controls.Add(this.tabBases);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(482, 177);
            this.Name = "FinaDebtExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export";
            this.panel1.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.tabBases.ResumeLayout(false);
            this.tabPageDelete.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ipmControls.DateTimePickers m_Picker;
        private System.Windows.Forms.Button btnExposrt;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TabControl tabBases;
        private System.Windows.Forms.TabPage tabPageDelete;
    }
}