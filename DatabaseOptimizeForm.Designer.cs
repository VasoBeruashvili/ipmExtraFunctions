namespace ipmExtraFunctions
{
    partial class DatabaseOptimizeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseOptimizeForm));
            this.checkShrink = new System.Windows.Forms.CheckBox();
            this.btnExec = new System.Windows.Forms.Button();
            this.checkDefragment = new System.Windows.Forms.CheckBox();
            this.checkRebuildIndex = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkShrink
            // 
            this.checkShrink.AutoSize = true;
            this.checkShrink.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkShrink.Location = new System.Drawing.Point(35, 29);
            this.checkShrink.Name = "checkShrink";
            this.checkShrink.Size = new System.Drawing.Size(189, 21);
            this.checkShrink.TabIndex = 57;
            this.checkShrink.Text = "მონაცემთა ოპტიმიზაცია";
            this.checkShrink.UseVisualStyleBackColor = true;
            // 
            // btnExec
            // 
            this.btnExec.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnExec.Location = new System.Drawing.Point(314, 116);
            this.btnExec.Name = "btnExec";
            this.btnExec.Size = new System.Drawing.Size(101, 32);
            this.btnExec.TabIndex = 58;
            this.btnExec.Text = "შესრულება";
            this.btnExec.UseVisualStyleBackColor = true;
            this.btnExec.Click += new System.EventHandler(this.btnExec_Click);
            // 
            // checkDefragment
            // 
            this.checkDefragment.AutoSize = true;
            this.checkDefragment.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkDefragment.Location = new System.Drawing.Point(35, 57);
            this.checkDefragment.Name = "checkDefragment";
            this.checkDefragment.Size = new System.Drawing.Size(139, 21);
            this.checkDefragment.TabIndex = 57;
            this.checkDefragment.Text = "დეფრაგმენტაცია";
            this.checkDefragment.UseVisualStyleBackColor = true;
            // 
            // checkRebuildIndex
            // 
            this.checkRebuildIndex.AutoSize = true;
            this.checkRebuildIndex.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkRebuildIndex.Location = new System.Drawing.Point(35, 85);
            this.checkRebuildIndex.Name = "checkRebuildIndex";
            this.checkRebuildIndex.Size = new System.Drawing.Size(135, 21);
            this.checkRebuildIndex.TabIndex = 57;
            this.checkRebuildIndex.Text = "რე-ინდექსირება";
            this.checkRebuildIndex.UseVisualStyleBackColor = true;
            // 
            // DatabaseOptimizeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 160);
            this.Controls.Add(this.btnExec);
            this.Controls.Add(this.checkRebuildIndex);
            this.Controls.Add(this.checkDefragment);
            this.Controls.Add(this.checkShrink);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(443, 199);
            this.MinimumSize = new System.Drawing.Size(443, 199);
            this.Name = "DatabaseOptimizeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ბაზის ოპტიმიზაცია";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkShrink;
        private System.Windows.Forms.Button btnExec;
        private System.Windows.Forms.CheckBox checkDefragment;
        private System.Windows.Forms.CheckBox checkRebuildIndex;
    }
}