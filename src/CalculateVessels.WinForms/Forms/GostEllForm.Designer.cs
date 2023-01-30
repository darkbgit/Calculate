
namespace CalculateVessels.Forms
{
    partial class GostEllForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.type_cb = new System.Windows.Forms.ComboBox();
            this.D_cb = new System.Windows.Forms.ComboBox();
            this.s_cb = new System.Windows.Forms.ComboBox();
            this.H_tb = new System.Windows.Forms.TextBox();
            this.h1_tb = new System.Windows.Forms.TextBox();
            this.OK_b = new System.Windows.Forms.Button();
            this.Cancel_b = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Днище";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Диаметр, мм";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Толщина, мм";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Н, мм";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(50, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "h1, мм";
            // 
            // type_cb
            // 
            this.type_cb.FormattingEnabled = true;
            this.type_cb.Items.AddRange(new object[] {
            "hн = 0,25 Dвн ГОСТ 6533-78",
            "hн = 0,25 Dн ГОСТ 6533-78",
            "hн = 0,2 Dвн ГОСТ 6533-78"});
            this.type_cb.Location = new System.Drawing.Point(100, 12);
            this.type_cb.Name = "type_cb";
            this.type_cb.Size = new System.Drawing.Size(121, 23);
            this.type_cb.TabIndex = 5;
            this.type_cb.SelectedIndexChanged += new System.EventHandler(this.Type_cb_SelectedIndexChanged);
            // 
            // D_cb
            // 
            this.D_cb.FormattingEnabled = true;
            this.D_cb.Location = new System.Drawing.Point(100, 41);
            this.D_cb.Name = "D_cb";
            this.D_cb.Size = new System.Drawing.Size(121, 23);
            this.D_cb.TabIndex = 6;
            this.D_cb.SelectedIndexChanged += new System.EventHandler(this.D_cb_SelectedIndexChanged);
            // 
            // s_cb
            // 
            this.s_cb.FormattingEnabled = true;
            this.s_cb.Location = new System.Drawing.Point(100, 70);
            this.s_cb.Name = "s_cb";
            this.s_cb.Size = new System.Drawing.Size(121, 23);
            this.s_cb.TabIndex = 7;
            this.s_cb.SelectedIndexChanged += new System.EventHandler(this.Scb_SelectedIndexChanged);
            // 
            // H_tb
            // 
            this.H_tb.Location = new System.Drawing.Point(100, 99);
            this.H_tb.Name = "H_tb";
            this.H_tb.Size = new System.Drawing.Size(100, 23);
            this.H_tb.TabIndex = 8;
            // 
            // h1_tb
            // 
            this.h1_tb.Location = new System.Drawing.Point(100, 128);
            this.h1_tb.Name = "h1_tb";
            this.h1_tb.Size = new System.Drawing.Size(100, 23);
            this.h1_tb.TabIndex = 9;
            // 
            // OK_b
            // 
            this.OK_b.Location = new System.Drawing.Point(35, 157);
            this.OK_b.Name = "OK_b";
            this.OK_b.Size = new System.Drawing.Size(75, 23);
            this.OK_b.TabIndex = 10;
            this.OK_b.Text = "OK";
            this.OK_b.UseVisualStyleBackColor = true;
            this.OK_b.Click += new System.EventHandler(this.OK_b_Click);
            // 
            // Cancel_b
            // 
            this.Cancel_b.Location = new System.Drawing.Point(125, 157);
            this.Cancel_b.Name = "Cancel_b";
            this.Cancel_b.Size = new System.Drawing.Size(75, 23);
            this.Cancel_b.TabIndex = 11;
            this.Cancel_b.Text = "Cancel";
            this.Cancel_b.UseVisualStyleBackColor = true;
            this.Cancel_b.Click += new System.EventHandler(this.Cancel_b_Click);
            // 
            // GostEllForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 191);
            this.Controls.Add(this.Cancel_b);
            this.Controls.Add(this.OK_b);
            this.Controls.Add(this.h1_tb);
            this.Controls.Add(this.H_tb);
            this.Controls.Add(this.s_cb);
            this.Controls.Add(this.D_cb);
            this.Controls.Add(this.type_cb);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GostEllForm";
            this.Text = "GostEllForm";
            this.Load += new System.EventHandler(this.GostEllForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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