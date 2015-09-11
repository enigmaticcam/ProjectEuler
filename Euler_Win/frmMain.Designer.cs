namespace Euler_Win {
    partial class frmMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lstProblems = new System.Windows.Forms.ListBox();
            this.cmdGo = new System.Windows.Forms.Button();
            this.txtAnswer = new System.Windows.Forms.TextBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstProblems
            // 
            this.lstProblems.Location = new System.Drawing.Point(13, 13);
            this.lstProblems.Name = "lstProblems";
            this.lstProblems.Size = new System.Drawing.Size(301, 355);
            this.lstProblems.TabIndex = 0;
            this.lstProblems.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstProblems_MouseDoubleClick);
            // 
            // cmdGo
            // 
            this.cmdGo.Location = new System.Drawing.Point(12, 375);
            this.cmdGo.Name = "cmdGo";
            this.cmdGo.Size = new System.Drawing.Size(75, 23);
            this.cmdGo.TabIndex = 1;
            this.cmdGo.Text = "Go";
            this.cmdGo.UseVisualStyleBackColor = true;
            this.cmdGo.Click += new System.EventHandler(this.cmdGo_Click);
            // 
            // txtAnswer
            // 
            this.txtAnswer.Location = new System.Drawing.Point(94, 378);
            this.txtAnswer.Name = "txtAnswer";
            this.txtAnswer.ReadOnly = true;
            this.txtAnswer.Size = new System.Drawing.Size(220, 20);
            this.txtAnswer.TabIndex = 2;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(91, 409);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 13);
            this.lblTime.TabIndex = 3;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 431);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.txtAnswer);
            this.Controls.Add(this.cmdGo);
            this.Controls.Add(this.lstProblems);
            this.Name = "frmMain";
            this.Text = "frmMain";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstProblems;
        private System.Windows.Forms.Button cmdGo;
        private System.Windows.Forms.TextBox txtAnswer;
        private System.Windows.Forms.Label lblTime;
    }
}