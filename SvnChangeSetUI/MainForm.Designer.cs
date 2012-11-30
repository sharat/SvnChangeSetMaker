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
            this.buttonCheckModifications = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonZip = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonCheckModifications
            // 
            this.buttonCheckModifications.Location = new System.Drawing.Point(563, 10);
            this.buttonCheckModifications.Name = "buttonCheckModifications";
            this.buttonCheckModifications.Size = new System.Drawing.Size(107, 45);
            this.buttonCheckModifications.TabIndex = 5;
            this.buttonCheckModifications.Text = "Check Modifications";
            this.buttonCheckModifications.UseVisualStyleBackColor = true;
            this.buttonCheckModifications.Click += new System.EventHandler(this.buttonCheckModifications_Click);
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(12, 63);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(771, 446);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Local Archive Path";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(116, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(441, 20);
            this.textBox1.TabIndex = 8;
            // 
            // buttonZip
            // 
            this.buttonZip.Location = new System.Drawing.Point(676, 10);
            this.buttonZip.Name = "buttonZip";
            this.buttonZip.Size = new System.Drawing.Size(107, 45);
            this.buttonZip.TabIndex = 5;
            this.buttonZip.Text = "Zip It!";
            this.buttonZip.UseVisualStyleBackColor = true;
            this.buttonZip.Click += new System.EventHandler(this.buttonZip_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 522);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.buttonZip);
            this.Controls.Add(this.buttonCheckModifications);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCheckModifications;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonZip;
    }
}

