namespace StoredProcedureRunner
{
    partial class frmRunSP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRunSP));
            this.btnRun = new System.Windows.Forms.Button();
            this.lblDBase = new System.Windows.Forms.Label();
            this.lblSPName = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(17, 60);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(131, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "&Run Stored Procedure";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblDBase
            // 
            this.lblDBase.AutoSize = true;
            this.lblDBase.Location = new System.Drawing.Point(14, 11);
            this.lblDBase.Name = "lblDBase";
            this.lblDBase.Size = new System.Drawing.Size(84, 13);
            this.lblDBase.TabIndex = 1;
            this.lblDBase.Text = "Database Name";
            // 
            // lblSPName
            // 
            this.lblSPName.AutoSize = true;
            this.lblSPName.Location = new System.Drawing.Point(14, 24);
            this.lblSPName.Name = "lblSPName";
            this.lblSPName.Size = new System.Drawing.Size(121, 13);
            this.lblSPName.TabIndex = 2;
            this.lblSPName.Text = "Stored Procedure Name";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(154, 60);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmRunSP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 95);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblSPName);
            this.Controls.Add(this.lblDBase);
            this.Controls.Add(this.btnRun);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRunSP";
            this.Text = "Stored Procedure Runner 1.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label lblDBase;
        private System.Windows.Forms.Label lblSPName;
        private System.Windows.Forms.Button btnCancel;
    }
}

