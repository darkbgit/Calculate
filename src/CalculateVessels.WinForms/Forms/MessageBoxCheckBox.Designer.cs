
namespace CalculateVessels.Forms;

partial class MessageBoxCheckBox
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
        this.OK_b = new System.Windows.Forms.Button();
        this.nozzle_cb = new System.Windows.Forms.CheckBox();
        this.label1 = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // OK_b
        // 
        this.OK_b.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.OK_b.Location = new System.Drawing.Point(84, 49);
        this.OK_b.Name = "OK_b";
        this.OK_b.Size = new System.Drawing.Size(75, 23);
        this.OK_b.TabIndex = 0;
        this.OK_b.Text = "OK";
        this.OK_b.UseVisualStyleBackColor = true;
        this.OK_b.Click += new System.EventHandler(this.OK_b_Click);
        // 
        // nozzle_cb
        // 
        this.nozzle_cb.AutoSize = true;
        this.nozzle_cb.Location = new System.Drawing.Point(122, 9);
        this.nozzle_cb.Name = "nozzle_cb";
        this.nozzle_cb.Size = new System.Drawing.Size(114, 34);
        this.nozzle_cb.TabIndex = 2;
        this.nozzle_cb.Text = "Расчитать узел\r\nврезки штуцера";
        this.nozzle_cb.UseVisualStyleBackColor = true;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(12, 18);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(103, 15);
        this.label1.TabIndex = 3;
        this.label1.Text = "Расчет выполнен";
        // 
        // MessageBoxCheckBox
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(234, 81);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.nozzle_cb);
        this.Controls.Add(this.OK_b);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "MessageBoxCheckBox";
        this.Text = "Результат";
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button OK_b;
    private System.Windows.Forms.CheckBox nozzle_cb;
    private System.Windows.Forms.Label label1;
}