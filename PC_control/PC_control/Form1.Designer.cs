namespace PC_control
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
            this.m_lbServer_status = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_tbxTime = new System.Windows.Forms.TextBox();
            this.m_pbxScreen = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_lblAltitude = new System.Windows.Forms.Label();
            this.m_lblAzimuth = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_pbxScreen)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_lbServer_status
            // 
            this.m_lbServer_status.AutoSize = true;
            this.m_lbServer_status.BackColor = System.Drawing.Color.Red;
            this.m_lbServer_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.m_lbServer_status.Location = new System.Drawing.Point(32, 25);
            this.m_lbServer_status.Name = "m_lbServer_status";
            this.m_lbServer_status.Size = new System.Drawing.Size(68, 25);
            this.m_lbServer_status.TabIndex = 0;
            this.m_lbServer_status.Text = "Offline";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(10, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Server status";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.m_lbServer_status);
            this.groupBox1.Location = new System.Drawing.Point(12, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(149, 74);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(13, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Time (5 - 100)";
            // 
            // m_tbxTime
            // 
            this.m_tbxTime.Location = new System.Drawing.Point(18, 128);
            this.m_tbxTime.Name = "m_tbxTime";
            this.m_tbxTime.Size = new System.Drawing.Size(100, 22);
            this.m_tbxTime.TabIndex = 4;
            this.m_tbxTime.Text = "100";
            this.m_tbxTime.TextChanged += new System.EventHandler(this.m_tbxTime_TextChanged);
            this.m_tbxTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m_tbxTime_KeyPress);
            // 
            // m_pbxScreen
            // 
            this.m_pbxScreen.Location = new System.Drawing.Point(167, 22);
            this.m_pbxScreen.Name = "m_pbxScreen";
            this.m_pbxScreen.Size = new System.Drawing.Size(621, 416);
            this.m_pbxScreen.TabIndex = 5;
            this.m_pbxScreen.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.m_lblAltitude);
            this.groupBox2.Controls.Add(this.m_lblAzimuth);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.groupBox2.Location = new System.Drawing.Point(18, 170);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(131, 70);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status";
            // 
            // m_lblAltitude
            // 
            this.m_lblAltitude.AutoSize = true;
            this.m_lblAltitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.m_lblAltitude.Location = new System.Drawing.Point(61, 39);
            this.m_lblAltitude.Name = "m_lblAltitude";
            this.m_lblAltitude.Size = new System.Drawing.Size(14, 16);
            this.m_lblAltitude.TabIndex = 4;
            this.m_lblAltitude.Text = "0";
            // 
            // m_lblAzimuth
            // 
            this.m_lblAzimuth.AutoSize = true;
            this.m_lblAzimuth.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.m_lblAzimuth.Location = new System.Drawing.Point(61, 22);
            this.m_lblAzimuth.Name = "m_lblAzimuth";
            this.m_lblAzimuth.Size = new System.Drawing.Size(14, 16);
            this.m_lblAzimuth.TabIndex = 3;
            this.m_lblAzimuth.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label4.Location = new System.Drawing.Point(9, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Altitude :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.Location = new System.Drawing.Point(9, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Azimuth :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.m_pbxScreen);
            this.Controls.Add(this.m_tbxTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_pbxScreen)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_lbServer_status;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_tbxTime;
        private System.Windows.Forms.PictureBox m_pbxScreen;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label m_lblAltitude;
        private System.Windows.Forms.Label m_lblAzimuth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}

