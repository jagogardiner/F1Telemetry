
namespace F1Telemetry
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
            this.label1 = new System.Windows.Forms.Label();
            this.positionLabel = new System.Windows.Forms.Label();
            this.frontLeftTyreReport = new System.Windows.Forms.Label();
            this.frontRightTyreReport = new System.Windows.Forms.Label();
            this.rearLeftTyreReport = new System.Windows.Forms.Label();
            this.rearRightTyreReport = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Position";
            // 
            // positionLabel
            // 
            this.positionLabel.AutoSize = true;
            this.positionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.positionLabel.Location = new System.Drawing.Point(26, 36);
            this.positionLabel.Name = "positionLabel";
            this.positionLabel.Size = new System.Drawing.Size(39, 42);
            this.positionLabel.TabIndex = 1;
            this.positionLabel.Text = "2\r\n";
            // 
            // frontLeftTyreReport
            // 
            this.frontLeftTyreReport.AutoSize = true;
            this.frontLeftTyreReport.Location = new System.Drawing.Point(22, 94);
            this.frontLeftTyreReport.Name = "frontLeftTyreReport";
            this.frontLeftTyreReport.Size = new System.Drawing.Size(51, 13);
            this.frontLeftTyreReport.TabIndex = 3;
            this.frontLeftTyreReport.Text = "Front left:";
            // 
            // frontRightTyreReport
            // 
            this.frontRightTyreReport.AutoSize = true;
            this.frontRightTyreReport.Location = new System.Drawing.Point(22, 117);
            this.frontRightTyreReport.Name = "frontRightTyreReport";
            this.frontRightTyreReport.Size = new System.Drawing.Size(57, 13);
            this.frontRightTyreReport.TabIndex = 4;
            this.frontRightTyreReport.Text = "Front right:";
            // 
            // rearLeftTyreReport
            // 
            this.rearLeftTyreReport.AutoSize = true;
            this.rearLeftTyreReport.Location = new System.Drawing.Point(22, 141);
            this.rearLeftTyreReport.Name = "rearLeftTyreReport";
            this.rearLeftTyreReport.Size = new System.Drawing.Size(50, 13);
            this.rearLeftTyreReport.TabIndex = 5;
            this.rearLeftTyreReport.Text = "Rear left:";
            // 
            // rearRightTyreReport
            // 
            this.rearRightTyreReport.AutoSize = true;
            this.rearRightTyreReport.Location = new System.Drawing.Point(22, 166);
            this.rearRightTyreReport.Name = "rearRightTyreReport";
            this.rearRightTyreReport.Size = new System.Drawing.Size(56, 13);
            this.rearRightTyreReport.TabIndex = 6;
            this.rearRightTyreReport.Text = "Rear right:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 562);
            this.Controls.Add(this.rearRightTyreReport);
            this.Controls.Add(this.rearLeftTyreReport);
            this.Controls.Add(this.frontRightTyreReport);
            this.Controls.Add(this.frontLeftTyreReport);
            this.Controls.Add(this.positionLabel);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "F1 Telemetry";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label positionLabel;
        private System.Windows.Forms.Label frontLeftTyreReport;
        private System.Windows.Forms.Label frontRightTyreReport;
        private System.Windows.Forms.Label rearLeftTyreReport;
        private System.Windows.Forms.Label rearRightTyreReport;
    }
}

