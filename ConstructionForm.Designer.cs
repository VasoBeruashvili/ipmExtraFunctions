namespace ipmExtraFunctions
{
    partial class ConstructionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConstructionForm));
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.btnChoose = new System.Windows.Forms.ToolStripButton();
            this.btnAddTemplate = new System.Windows.Forms.ToolStripButton();
            this.btnAddProduct = new System.Windows.Forms.ToolStripButton();
            this.btnRealization = new System.Windows.Forms.ToolStripButton();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlPattern = new System.Windows.Forms.Panel();
            this.lblTemplate = new System.Windows.Forms.Label();
            this.comboTemplate = new System.Windows.Forms.ComboBox();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblSelf = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblNum = new System.Windows.Forms.Label();
            this.toolStripMain.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlPattern.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnClose,
            this.btnChoose,
            this.btnAddTemplate,
            this.btnAddProduct,
            this.btnRealization});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(641, 25);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // btnClose
            // 
            this.btnClose.Image = global::ipmExtraFunctions.Properties.Resources.exit_16;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(68, 22);
            this.btnClose.Text = "დახურვა";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnChoose
            // 
            this.btnChoose.Image = global::ipmExtraFunctions.Properties.Resources.select_16;
            this.btnChoose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(58, 22);
            this.btnChoose.Text = "არჩევა";
            // 
            // btnAddTemplate
            // 
            this.btnAddTemplate.Image = global::ipmExtraFunctions.Properties.Resources.add_16;
            this.btnAddTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddTemplate.Name = "btnAddTemplate";
            this.btnAddTemplate.Size = new System.Drawing.Size(84, 22);
            this.btnAddTemplate.Text = "შაბლონები";
            this.btnAddTemplate.Click += new System.EventHandler(this.btnAddTemplate_Click);
            // 
            // btnAddProduct
            // 
            this.btnAddProduct.Image = global::ipmExtraFunctions.Properties.Resources.add_16;
            this.btnAddProduct.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(70, 22);
            this.btnAddProduct.Text = "დამატება";
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);
            // 
            // btnRealization
            // 
            this.btnRealization.Image = global::ipmExtraFunctions.Properties.Resources.note_pinned_16;
            this.btnRealization.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRealization.Name = "btnRealization";
            this.btnRealization.Size = new System.Drawing.Size(89, 22);
            this.btnRealization.Text = "რეალიზაცია";
            this.btnRealization.Click += new System.EventHandler(this.btnRealization_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.pnlPattern);
            this.pnlTop.Controls.Add(this.comboTemplate);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 25);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(641, 42);
            this.pnlTop.TabIndex = 1;
            // 
            // pnlPattern
            // 
            this.pnlPattern.Controls.Add(this.lblTemplate);
            this.pnlPattern.Location = new System.Drawing.Point(66, 9);
            this.pnlPattern.Name = "pnlPattern";
            this.pnlPattern.Size = new System.Drawing.Size(96, 24);
            this.pnlPattern.TabIndex = 9;
            // 
            // lblTemplate
            // 
            this.lblTemplate.AutoSize = true;
            this.lblTemplate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTemplate.Location = new System.Drawing.Point(20, 0);
            this.lblTemplate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(76, 18);
            this.lblTemplate.TabIndex = 0;
            this.lblTemplate.Text = "შაბლონი:";
            this.lblTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboTemplate
            // 
            this.comboTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboTemplate.FormattingEnabled = true;
            this.comboTemplate.Location = new System.Drawing.Point(163, 7);
            this.comboTemplate.Name = "comboTemplate";
            this.comboTemplate.Size = new System.Drawing.Size(366, 26);
            this.comboTemplate.TabIndex = 10;
            this.comboTemplate.SelectedValueChanged += new System.EventHandler(this.comboTemplate_SelectedValueChanged);
            // 
            // pnlRight
            // 
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlRight.Location = new System.Drawing.Point(163, 67);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(478, 441);
            this.pnlRight.TabIndex = 2;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlLeft.Location = new System.Drawing.Point(0, 67);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(163, 441);
            this.pnlLeft.TabIndex = 3;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lblSelf);
            this.pnlBottom.Controls.Add(this.lblPrice);
            this.pnlBottom.Controls.Add(this.lblNum);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlBottom.Location = new System.Drawing.Point(0, 508);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(641, 37);
            this.pnlBottom.TabIndex = 0;
            // 
            // lblSelf
            // 
            this.lblSelf.AutoSize = true;
            this.lblSelf.Location = new System.Drawing.Point(493, 10);
            this.lblSelf.Name = "lblSelf";
            this.lblSelf.Size = new System.Drawing.Size(31, 18);
            this.lblSelf.TabIndex = 2;
            this.lblSelf.Text = "self";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(406, 10);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(40, 18);
            this.lblPrice.TabIndex = 1;
            this.lblPrice.Text = "price";
            // 
            // lblNum
            // 
            this.lblNum.AutoSize = true;
            this.lblNum.Location = new System.Drawing.Point(319, 10);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(37, 18);
            this.lblNum.TabIndex = 0;
            this.lblNum.Text = "num";
            // 
            // ConstructionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 545);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.toolStripMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ConstructionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "აწყობა";
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlPattern.ResumeLayout(false);
            this.pnlPattern.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.ToolStripButton btnChoose;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.ToolStripButton btnAddTemplate;
        private System.Windows.Forms.Panel pnlPattern;
        private System.Windows.Forms.Label lblTemplate;
        private System.Windows.Forms.ComboBox comboTemplate;
        private System.Windows.Forms.ToolStripButton btnAddProduct;
        private System.Windows.Forms.ToolStripButton btnRealization;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblSelf;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblNum;
    }
}