namespace ipmExtraFunctions
{
    partial class ExMSTemplatesForm
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
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.btnCloseValues = new System.Windows.Forms.ToolStripButton();
            this.btnSaveValues = new System.Windows.Forms.ToolStripButton();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlColumnValues = new System.Windows.Forms.Panel();
            this.toolStrip4.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip4
            // 
            this.toolStrip4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStrip4.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCloseValues,
            this.btnSaveValues});
            this.toolStrip4.Location = new System.Drawing.Point(0, 0);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.Size = new System.Drawing.Size(384, 25);
            this.toolStrip4.TabIndex = 69;
            this.toolStrip4.Text = "toolStrip2";
            // 
            // btnCloseValues
            // 
            this.btnCloseValues.Image = global::ipmExtraFunctions.Properties.Resources.exit_16;
            this.btnCloseValues.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCloseValues.Name = "btnCloseValues";
            this.btnCloseValues.Size = new System.Drawing.Size(76, 22);
            this.btnCloseValues.Text = "დახურვა";
            this.btnCloseValues.Click += new System.EventHandler(this.btnCloseValues_Click);
            // 
            // btnSaveValues
            // 
            this.btnSaveValues.Image = global::ipmExtraFunctions.Properties.Resources.save_16;
            this.btnSaveValues.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveValues.Name = "btnSaveValues";
            this.btnSaveValues.Size = new System.Drawing.Size(68, 22);
            this.btnSaveValues.Text = "შენახვა";
            this.btnSaveValues.ToolTipText = "შენახვა";
            this.btnSaveValues.Click += new System.EventHandler(this.btnSaveValues_Click);
            // 
            // pnlContainer
            // 
            this.pnlContainer.AutoScroll = true;
            this.pnlContainer.Controls.Add(this.tableLayoutPanel3);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(0, 25);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(384, 437);
            this.pnlContainer.TabIndex = 70;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.pnlColumnValues, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(384, 437);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // pnlColumnValues
            // 
            this.pnlColumnValues.AutoScroll = true;
            this.pnlColumnValues.BackColor = System.Drawing.Color.Transparent;
            this.pnlColumnValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlColumnValues.Font = new System.Drawing.Font("Sylfaen", 9.75F);
            this.pnlColumnValues.Location = new System.Drawing.Point(3, 10);
            this.pnlColumnValues.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
            this.pnlColumnValues.Name = "pnlColumnValues";
            this.pnlColumnValues.Size = new System.Drawing.Size(371, 424);
            this.pnlColumnValues.TabIndex = 0;
            // 
            // ExMSTemplatesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 462);
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.toolStrip4);
            this.Name = "ExMSTemplatesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ExMSTemplatesForm";
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            this.pnlContainer.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripButton btnCloseValues;
        public System.Windows.Forms.ToolStripButton btnSaveValues;
        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel pnlColumnValues;
    }
}