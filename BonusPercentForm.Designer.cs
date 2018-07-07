namespace ipmExtraFunctions
{
    partial class BonusPercentForm
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPrcFromGroup = new ipmControls.TextBoxDecimalInput();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPrcFromCoords = new ipmControls.TextBoxDecimalInput();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPrcFromSeniorCoords = new ipmControls.TextBoxDecimalInput();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPrcFromManagers = new ipmControls.TextBoxDecimalInput();
            this.toolStrip1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExit,
            this.btnSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(383, 25);
            this.toolStrip1.TabIndex = 9;
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
            // btnSave
            // 
            this.btnSave.Image = global::ipmExtraFunctions.Properties.Resources.save_16;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(62, 22);
            this.btnSave.Text = "შენახვა";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label5);
            this.panel7.Location = new System.Drawing.Point(0, 35);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(242, 25);
            this.panel7.TabIndex = 42;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(104, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "პირადი ჯგუფიდან:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPrcFromGroup
            // 
            this.txtPrcFromGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtPrcFromGroup.Location = new System.Drawing.Point(249, 35);
            this.txtPrcFromGroup.Name = "txtPrcFromGroup";
            this.txtPrcFromGroup.Size = new System.Drawing.Size(100, 24);
            this.txtPrcFromGroup.TabIndex = 1;
            this.txtPrcFromGroup.Text = "0.00";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(242, 25);
            this.panel1.TabIndex = 42;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(80, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "კოორდინატორებისგან:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPrcFromCoords
            // 
            this.txtPrcFromCoords.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtPrcFromCoords.Location = new System.Drawing.Point(249, 65);
            this.txtPrcFromCoords.Name = "txtPrcFromCoords";
            this.txtPrcFromCoords.Size = new System.Drawing.Size(100, 24);
            this.txtPrcFromCoords.TabIndex = 2;
            this.txtPrcFromCoords.Text = "0.00";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(0, 95);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(243, 25);
            this.panel2.TabIndex = 42;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(20, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(223, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "უფროსი კოორდინატორებისგან:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPrcFromSeniorCoords
            // 
            this.txtPrcFromSeniorCoords.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtPrcFromSeniorCoords.Location = new System.Drawing.Point(249, 95);
            this.txtPrcFromSeniorCoords.Name = "txtPrcFromSeniorCoords";
            this.txtPrcFromSeniorCoords.Size = new System.Drawing.Size(100, 24);
            this.txtPrcFromSeniorCoords.TabIndex = 3;
            this.txtPrcFromSeniorCoords.Text = "0.00";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(0, 125);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(243, 25);
            this.panel3.TabIndex = 42;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(125, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "მენეჯერებისგან:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPrcFromManagers
            // 
            this.txtPrcFromManagers.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtPrcFromManagers.Location = new System.Drawing.Point(249, 125);
            this.txtPrcFromManagers.Name = "txtPrcFromManagers";
            this.txtPrcFromManagers.Size = new System.Drawing.Size(100, 24);
            this.txtPrcFromManagers.TabIndex = 4;
            this.txtPrcFromManagers.Text = "0.00";
            // 
            // BonusPercentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 168);
            this.Controls.Add(this.txtPrcFromManagers);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.txtPrcFromSeniorCoords);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.txtPrcFromCoords);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtPrcFromGroup);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BonusPercentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "პროცენტები";
            this.Load += new System.EventHandler(this.BonusPercentForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label5;
        private ipmControls.TextBoxDecimalInput txtPrcFromGroup;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private ipmControls.TextBoxDecimalInput txtPrcFromCoords;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private ipmControls.TextBoxDecimalInput txtPrcFromSeniorCoords;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private ipmControls.TextBoxDecimalInput txtPrcFromManagers;
    }
}