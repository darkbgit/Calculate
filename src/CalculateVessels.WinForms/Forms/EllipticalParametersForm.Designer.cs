
namespace CalculateVessels.Forms
{
    partial class EllipticalParametersForm
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
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            type_cb = new System.Windows.Forms.ComboBox();
            D_cb = new System.Windows.Forms.ComboBox();
            s_cb = new System.Windows.Forms.ComboBox();
            H_tb = new System.Windows.Forms.TextBox();
            h1_tb = new System.Windows.Forms.TextBox();
            OK_b = new System.Windows.Forms.Button();
            Cancel_b = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(48, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(46, 15);
            label1.TabIndex = 0;
            label1.Text = "Днище";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(15, 44);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(79, 15);
            label2.TabIndex = 1;
            label2.Text = "Диаметр, мм";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 73);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(82, 15);
            label3.TabIndex = 2;
            label3.Text = "Толщина, мм";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(54, 102);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(40, 15);
            label4.TabIndex = 3;
            label4.Text = "Н, мм";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(50, 131);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(44, 15);
            label5.TabIndex = 4;
            label5.Text = "h1, мм";
            // 
            // type_cb
            // 
            type_cb.FormattingEnabled = true;
            type_cb.Items.AddRange(new object[] { "hв = 0,25 Dв ГОСТ 6533-78", "hв = 0,2 Dв ГОСТ 6533-78", "hн = 0,25 Dн ГОСТ 6533-78" });
            type_cb.Location = new System.Drawing.Point(100, 12);
            type_cb.Name = "type_cb";
            type_cb.Size = new System.Drawing.Size(121, 23);
            type_cb.TabIndex = 5;
            type_cb.SelectedIndexChanged += Type_cb_SelectedIndexChanged;
            // 
            // D_cb
            // 
            D_cb.FormattingEnabled = true;
            D_cb.Location = new System.Drawing.Point(100, 41);
            D_cb.Name = "D_cb";
            D_cb.Size = new System.Drawing.Size(121, 23);
            D_cb.TabIndex = 6;
            D_cb.SelectedIndexChanged += D_cb_SelectedIndexChanged;
            // 
            // s_cb
            // 
            s_cb.FormattingEnabled = true;
            s_cb.Location = new System.Drawing.Point(100, 70);
            s_cb.Name = "s_cb";
            s_cb.Size = new System.Drawing.Size(121, 23);
            s_cb.TabIndex = 7;
            s_cb.SelectedIndexChanged += Scb_SelectedIndexChanged;
            // 
            // H_tb
            // 
            H_tb.Location = new System.Drawing.Point(100, 99);
            H_tb.Name = "H_tb";
            H_tb.Size = new System.Drawing.Size(100, 23);
            H_tb.TabIndex = 8;
            // 
            // h1_tb
            // 
            h1_tb.Location = new System.Drawing.Point(100, 128);
            h1_tb.Name = "h1_tb";
            h1_tb.Size = new System.Drawing.Size(100, 23);
            h1_tb.TabIndex = 9;
            // 
            // OK_b
            // 
            OK_b.Location = new System.Drawing.Point(35, 157);
            OK_b.Name = "OK_b";
            OK_b.Size = new System.Drawing.Size(75, 23);
            OK_b.TabIndex = 10;
            OK_b.Text = "OK";
            OK_b.UseVisualStyleBackColor = true;
            OK_b.Click += OK_b_Click;
            // 
            // Cancel_b
            // 
            Cancel_b.Location = new System.Drawing.Point(125, 157);
            Cancel_b.Name = "Cancel_b";
            Cancel_b.Size = new System.Drawing.Size(75, 23);
            Cancel_b.TabIndex = 11;
            Cancel_b.Text = "Cancel";
            Cancel_b.UseVisualStyleBackColor = true;
            Cancel_b.Click += Cancel_b_Click;
            // 
            // EllipticalParametersForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(239, 191);
            Controls.Add(Cancel_b);
            Controls.Add(OK_b);
            Controls.Add(h1_tb);
            Controls.Add(H_tb);
            Controls.Add(s_cb);
            Controls.Add(D_cb);
            Controls.Add(type_cb);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Name = "EllipticalParametersForm";
            Text = "GostEllForm";
            Load += GostEllForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox type_cb;
        private System.Windows.Forms.ComboBox D_cb;
        private System.Windows.Forms.ComboBox s_cb;
        private System.Windows.Forms.TextBox H_tb;
        private System.Windows.Forms.TextBox h1_tb;
        private System.Windows.Forms.Button OK_b;
        private System.Windows.Forms.Button Cancel_b;
    }
}