namespace IpmConverting
{
    partial class SyncNecesittyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncNecesittyForm));
            this.tlStrip_Main = new System.Windows.Forms.ToolStrip();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.m_Tree = new System.Windows.Forms.TreeView();
            this.ContextMenuSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuUncheck = new System.Windows.Forms.ToolStripMenuItem();
            this.m_TreeImageList = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.tlStrip_Main.SuspendLayout();
            this.ContextMenuSelect.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlStrip_Main
            // 
            this.tlStrip_Main.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tlStrip_Main.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExit,
            this.btnSave});
            this.tlStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.tlStrip_Main.Name = "tlStrip_Main";
            this.tlStrip_Main.Size = new System.Drawing.Size(535, 25);
            this.tlStrip_Main.TabIndex = 56;
            this.tlStrip_Main.Text = "toolStrip1";
            // 
            // btnExit
            // 
            this.btnExit.Image = global::ipmExtraFunctions.Properties.Resources.exit_16;
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 22);
            this.btnExit.Text = "დახურვა";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::ipmExtraFunctions.Properties.Resources.save_16;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(73, 22);
            this.btnSave.Text = "შენახვა";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // m_Tree
            // 
            this.m_Tree.CheckBoxes = true;
            this.m_Tree.ContextMenuStrip = this.ContextMenuSelect;
            this.m_Tree.Cursor = System.Windows.Forms.Cursors.Hand;
            this.m_Tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Tree.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_Tree.HideSelection = false;
            this.m_Tree.HotTracking = true;
            this.m_Tree.ImageIndex = 0;
            this.m_Tree.ImageList = this.m_TreeImageList;
            this.m_Tree.LineColor = System.Drawing.Color.LightSkyBlue;
            this.m_Tree.Location = new System.Drawing.Point(5, 5);
            this.m_Tree.Name = "m_Tree";
            this.m_Tree.SelectedImageIndex = 1;
            this.m_Tree.Size = new System.Drawing.Size(525, 508);
            this.m_Tree.TabIndex = 73;
            this.m_Tree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.m_Tree_AfterCheck);
            // 
            // ContextMenuSelect
            // 
            this.ContextMenuSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuCheck,
            this.MenuUncheck});
            this.ContextMenuSelect.Name = "contextMenuStrip1";
            this.ContextMenuSelect.Size = new System.Drawing.Size(187, 70);
            this.ContextMenuSelect.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenu_Opening);
            // 
            // MenuCheck
            // 
            this.MenuCheck.Font = new System.Drawing.Font("Sylfaen", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MenuCheck.Name = "MenuCheck";
            this.MenuCheck.Size = new System.Drawing.Size(186, 22);
            this.MenuCheck.Text = "მონიშვნა";
            // 
            // MenuUncheck
            // 
            this.MenuUncheck.Font = new System.Drawing.Font("Sylfaen", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MenuUncheck.Name = "MenuUncheck";
            this.MenuUncheck.Size = new System.Drawing.Size(186, 22);
            this.MenuUncheck.Text = "მონიშვნის მოსხნა";
            // 
            // m_TreeImageList
            // 
            this.m_TreeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_TreeImageList.ImageStream")));
            this.m_TreeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.m_TreeImageList.Images.SetKeyName(0, "folder.png");
            this.m_TreeImageList.Images.SetKeyName(1, "folder_closed.png");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_Tree);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 46);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(535, 518);
            this.panel1.TabIndex = 74;
            // 
            // SyncNecesittyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 564);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tlStrip_Main);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(551, 603);
            this.Name = "SyncNecesittyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "სინქრონიზაციის მოთხოვნა";
            this.Load += new System.EventHandler(this.SyncNecesittyForm_Load);
            this.tlStrip_Main.ResumeLayout(false);
            this.tlStrip_Main.PerformLayout();
            this.ContextMenuSelect.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tlStrip_Main;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.TreeView m_Tree;
        private System.Windows.Forms.ImageList m_TreeImageList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip ContextMenuSelect;
        private System.Windows.Forms.ToolStripMenuItem MenuCheck;
        private System.Windows.Forms.ToolStripMenuItem MenuUncheck;
    }
}

