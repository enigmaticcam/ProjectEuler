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
            this.txtFileInput = new System.Windows.Forms.TextBox();
            this.cmdFileInput = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstProblems
            // 
            this.lstProblems.Location = new System.Drawing.Point(13, 13);
            this.lstProblems.Name = "lstProblems";
            this.lstProblems.Size = new System.Drawing.Size(301, 355);
            this.lstProblems.TabIndex = 0;
            this.lstProblems.SelectedIndexChanged += new System.EventHandler(this.lstProblems_SelectedIndexChanged);
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
            this.lblTime.Location = new System.Drawing.Point(91, 438);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 13);
            this.lblTime.TabIndex = 3;
            // 
            // txtFileInput
            // 
            this.txtFileInput.Enabled = false;
            this.txtFileInput.Location = new System.Drawing.Point(94, 405);
            this.txtFileInput.Name = "txtFileInput";
            this.txtFileInput.Size = new System.Drawing.Size(220, 20);
            this.txtFileInput.TabIndex = 4;
            // 
            // cmdFileInput
            // 
            this.cmdFileInput.Enabled = false;
            this.cmdFileInput.Location = new System.Drawing.Point(13, 402);
            this.cmdFileInput.Name = "cmdFileInput";
            this.cmdFileInput.Size = new System.Drawing.Size(75, 23);
            this.cmdFileInput.TabIndex = 5;
            this.cmdFileInput.Text = "File Input:";
            this.cmdFileInput.UseVisualStyleBackColor = true;
            this.cmdFileInput.Click += new System.EventHandler(this.cmdFileInput_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 460);
            this.Controls.Add(this.cmdFileInput);
            this.Controls.Add(this.txtFileInput);
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
        private System.Windows.Forms.TextBox txtFileInput;
        private System.Windows.Forms.Button cmdFileInput;
    }
}