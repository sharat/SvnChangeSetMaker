namespace SvnChangeSet
{
    partial class Form1
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
            this.buttonGetArchiveInformation = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textBoxArchiveInformation = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonGetArchiveInformation
            // 
            this.buttonGetArchiveInformation.Location = new System.Drawing.Point(41, 110);
            this.buttonGetArchiveInformation.Name = "buttonGetArchiveInformation";
            this.buttonGetArchiveInformation.Size = new System.Drawing.Size(94, 37);
            this.buttonGetArchiveInformation.TabIndex = 0;
            this.buttonGetArchiveInformation.Text = "Get Archive Information";
            this.buttonGetArchiveInformation.UseVisualStyleBackColor = true;
            this.buttonGetArchiveInformation.Click += new System.EventHandler(this.buttonGetArchiveInformation_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Local Archive Path";
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(141, 25);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(253, 20);
            this.textBoxPath.TabIndex = 2;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(400, 23);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(30, 23);
            this.buttonBrowse.TabIndex = 3;
            this.buttonBrowse.Text = "...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            // 
            // textBoxArchiveInformation
            // 
            this.textBoxArchiveInformation.Location = new System.Drawing.Point(141, 110);
            this.textBoxArchiveInformation.Multiline = true;
            this.textBoxArchiveInformation.Name = "textBoxArchiveInformation";
            this.textBoxArchiveInformation.ReadOnly = true;
            this.textBoxArchiveInformation.Size = new System.Drawing.Size(253, 37);
            this.textBoxArchiveInformation.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 623);
            this.Controls.Add(this.textBoxArchiveInformation);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonGetArchiveInformation);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGetArchiveInformation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TextBox textBoxArchiveInformation;
    }
}

