namespace OWOVRC
{
    partial class OWO_VRC_FORM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OWO_VRC_FORM));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdArgLabel = new System.Windows.Forms.Label();
            this.debug = new System.Windows.Forms.Label();
            this.Freq = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.Stren = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.Freq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Stren)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(305, 130);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "CommandLine args:";
            // 
            // cmdArgLabel
            // 
            this.cmdArgLabel.AutoSize = true;
            this.cmdArgLabel.Location = new System.Drawing.Point(12, 187);
            this.cmdArgLabel.Name = "cmdArgLabel";
            this.cmdArgLabel.Size = new System.Drawing.Size(51, 13);
            this.cmdArgLabel.TabIndex = 2;
            this.cmdArgLabel.Text = "args here";
            // 
            // debug
            // 
            this.debug.AutoSize = true;
            this.debug.Location = new System.Drawing.Point(12, 218);
            this.debug.Name = "debug";
            this.debug.Size = new System.Drawing.Size(109, 13);
            this.debug.TabIndex = 3;
            this.debug.Text = "debug msg goes here";
            // 
            // Freq
            // 
            this.Freq.Location = new System.Drawing.Point(144, 262);
            this.Freq.Name = "Freq";
            this.Freq.Size = new System.Drawing.Size(38, 20);
            this.Freq.TabIndex = 9;
            this.Freq.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.Freq.ValueChanged += new System.EventHandler(this.Freq_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 264);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 39);
            this.label7.TabIndex = 10;
            this.label7.Text = "Top: Frequency\r\nBottom: Strength\r\n0-100";
            // 
            // Stren
            // 
            this.Stren.Location = new System.Drawing.Point(144, 281);
            this.Stren.Name = "Stren";
            this.Stren.Size = new System.Drawing.Size(38, 20);
            this.Stren.TabIndex = 12;
            this.Stren.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.Stren.ValueChanged += new System.EventHandler(this.Stren_ValueChanged);
            // 
            // OWO_VRC_FORM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 323);
            this.Controls.Add(this.Stren);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Freq);
            this.Controls.Add(this.debug);
            this.Controls.Add(this.cmdArgLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "OWO_VRC_FORM";
            this.Text = "OWO VRC";
            this.Load += new System.EventHandler(this.OWO_VRC_FORM_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Freq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Stren)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label cmdArgLabel;
        private System.Windows.Forms.Label debug;
        private System.Windows.Forms.NumericUpDown Freq;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown Stren;
    }
}

