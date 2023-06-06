using System.Drawing;
using System.Windows.Forms;

namespace CalculateVessels.Elements;

sealed partial class LoadingConditionGroupBox
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

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        //AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        temperatureLabel = new System.Windows.Forms.Label();
        temperatureTextBox = new TextBox();
        temperatureDemensionLabel = new System.Windows.Forms.Label();
        pressureLabel = new System.Windows.Forms.Label();
        pressureTextBox = new TextBox();
        pressureDemensionLabel = new System.Windows.Forms.Label();
        sigmaAllowLabel = new System.Windows.Forms.Label();
        EAllowLabel = new System.Windows.Forms.Label();
        sigmaAllowDemensionLabel = new System.Windows.Forms.Label();
        EAllowDemensionLabel = new System.Windows.Forms.Label();
        sigmaAllowTextBox = new TextBox();
        EAllowTextBox = new TextBox();
        isPressureOutsideCheckBox = new System.Windows.Forms.CheckBox();
        isSigmaAllowHandleCheckBox = new System.Windows.Forms.CheckBox();
        isEAllowHandleCheckBox = new CheckBox();
        SuspendLayout();
        // 
        // temperatureLabel
        // 
        temperatureLabel.Location = new Point(14, 25);
        temperatureLabel.AutoSize = true;
        temperatureLabel.Margin = new Padding(4, 0, 4, 0);
        temperatureLabel.Name = "label3";
        temperatureLabel.Size = new System.Drawing.Size(149, 15);
        temperatureLabel.TabIndex = 4;
        temperatureLabel.Text = "Расчетная температура, t:";
        // 
        // temperatureTextBox
        // 
        temperatureTextBox.Location = new Point(169, 22); 
        temperatureTextBox.Margin = new Padding(4, 3, 4, 3);
        temperatureTextBox.Name = "temperatureTextBox";
        temperatureTextBox.Size = new System.Drawing.Size(46, 23);
        temperatureTextBox.TabIndex = 3;
        temperatureTextBox.Text = "20";
        // 
        // temperatureDemensionLabel
        // 
        temperatureDemensionLabel.Location = new Point(219, 25);
        temperatureDemensionLabel.AutoSize = true;
        temperatureDemensionLabel.Margin = new Padding(0, 0, 4, 0);
        temperatureDemensionLabel.Name = "temperatureDemensionLabel";
        temperatureDemensionLabel.Size = new System.Drawing.Size(20, 15);
        temperatureDemensionLabel.TabIndex = 8;
        temperatureDemensionLabel.Text = "°C";
        // 
        // pressureLabel
        // 
        pressureLabel.Location = new Point(29, 55);
        pressureLabel.AutoSize = true;
        pressureLabel.Margin = new Padding(4, 0, 4, 0);
        pressureLabel.Name = "pressureLabel";
        pressureLabel.Size = new System.Drawing.Size(134, 15);
        pressureLabel.TabIndex = 9;
        pressureLabel.Text = "Расчетное давление, p:";
        // 
        // pressureTextBox
        // 
        pressureTextBox.Location = new Point(169, 52);
        pressureTextBox.Margin = new Padding(4, 3, 4, 3);
        pressureTextBox.Name = "pressureTextBox";
        pressureTextBox.Size = new System.Drawing.Size(46, 23);
        pressureTextBox.TabIndex = 4;
        pressureTextBox.Text = "3.1";
        // 
        // pressureDemensionLabel
        // 
        pressureDemensionLabel.Location = new Point(223, 55);
        pressureDemensionLabel.AutoSize = true;
        pressureDemensionLabel.Margin = new Padding(4, 0, 4, 0);
        pressureDemensionLabel.Name = "pressureDemensionLabel";
        pressureDemensionLabel.Size = new System.Drawing.Size(33, 15);
        pressureDemensionLabel.TabIndex = 13;
        pressureDemensionLabel.Text = "МПа";
        // 
        // isPressureOutsideCheckBox
        // 
        isPressureOutsideCheckBox.AutoSize = true;
        isPressureOutsideCheckBox.Location = new System.Drawing.Point(263, 51);
        isPressureOutsideCheckBox.Name = "isPressureOutsideCheckBox";
        isPressureOutsideCheckBox.Size = new System.Drawing.Size(81, 19);
        isPressureOutsideCheckBox.TabIndex = 14;
        isPressureOutsideCheckBox.Text = "наружное";
        isPressureOutsideCheckBox.UseVisualStyleBackColor = true;
        //isPressureOutsideCheckBox.CheckedChanged += OutsidePressureChecked_cb;
        //isPressureOutsideCheckBox.CheckStateChanged += DisabledCalculateBtn;
        // 
        // sigmaAllowLabel
        // 
        sigmaAllowLabel.AutoSize = true;
        sigmaAllowLabel.Location = new System.Drawing.Point(59, 76);
        sigmaAllowLabel.Margin = new Padding(4, 0, 4, 0);
        sigmaAllowLabel.Name = "sigmaAllowLabel";
        sigmaAllowLabel.Size = new System.Drawing.Size(102, 30);
        sigmaAllowLabel.TabIndex = 16;
        sigmaAllowLabel.Text = "Допускаемое\r\n напряжение, [σ]:";
        sigmaAllowLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
        // 
        // sigmaAllowTextBox
        // 
        sigmaAllowTextBox.Enabled = false;
        sigmaAllowTextBox.Location = new System.Drawing.Point(169, 80);
        sigmaAllowTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        sigmaAllowTextBox.Name = "sigma_d_tb";
        sigmaAllowTextBox.Size = new System.Drawing.Size(46, 23);
        sigmaAllowTextBox.TabIndex = 17;
        //sigmaAllowTextBox.TextChanged += DisabledCalculateBtn;
        // 
        // sigmaAllowDemensionLabel
        // 
        sigmaAllowDemensionLabel.AutoSize = true;
        sigmaAllowDemensionLabel.Location = new System.Drawing.Point(223, 84);
        sigmaAllowDemensionLabel.Margin = new Padding(4, 0, 4, 0);
        sigmaAllowDemensionLabel.Name = "sigmaAllowDemensionLabel";
        sigmaAllowDemensionLabel.Size = new System.Drawing.Size(33, 15);
        sigmaAllowDemensionLabel.TabIndex = 36;
        sigmaAllowDemensionLabel.Text = "МПа";
        // 
        // isSigmaAllowHandleCheckBox
        // 
        isSigmaAllowHandleCheckBox.AutoSize = true;
        isSigmaAllowHandleCheckBox.Location = new System.Drawing.Point(263, 74);
        isSigmaAllowHandleCheckBox.Name = "sigmaHandle_cb";
        isSigmaAllowHandleCheckBox.Size = new System.Drawing.Size(75, 34);
        isSigmaAllowHandleCheckBox.TabIndex = 62;
        isSigmaAllowHandleCheckBox.Text = "Задать\r\nвручную";
        isSigmaAllowHandleCheckBox.UseVisualStyleBackColor = true;
        isSigmaAllowHandleCheckBox.CheckedChanged += SigmaAllowHandleCheckBox_CheckedChanged;
        // 
        // EAllowLabel
        // 
        EAllowLabel.AutoSize = true;
        EAllowLabel.Location = new System.Drawing.Point(40, 105);
        EAllowLabel.Margin = new Padding(4, 0, 4, 0);
        EAllowLabel.Name = "EAllowLabel";
        EAllowLabel.Size = new System.Drawing.Size(121, 30);
        EAllowLabel.TabIndex = 26;
        EAllowLabel.Text = "Модуль продольной\r\n упругости, E:";
        EAllowLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
        // 
        // EAllowTextBox
        // 
        EAllowTextBox.Enabled = false;
        EAllowTextBox.Location = new System.Drawing.Point(169, 109);
        EAllowTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        EAllowTextBox.Name = "E_tb";
        EAllowTextBox.Size = new System.Drawing.Size(46, 23);
        EAllowTextBox.TabIndex = 18;
        //EAllowTextBox.TextChanged += DisabledCalculateBtn;
        // 
        // EAllowDemensionLabel
        // 
        EAllowDemensionLabel.AutoSize = true;
        EAllowDemensionLabel.Location = new System.Drawing.Point(223, 113);
        EAllowDemensionLabel.Margin = new Padding(4, 0, 4, 0);
        EAllowDemensionLabel.Name = "EAllowDemensionLabel";
        EAllowDemensionLabel.Size = new System.Drawing.Size(33, 15);
        EAllowDemensionLabel.TabIndex = 38;
        EAllowDemensionLabel.Text = "МПа";
        // 
        // isEAllowHandleCheckBox
        // 
        isEAllowHandleCheckBox.AutoSize = true;
        isEAllowHandleCheckBox.Location = new System.Drawing.Point(263, 103);
        isEAllowHandleCheckBox.Name = "EHandle_cb";
        isEAllowHandleCheckBox.Size = new System.Drawing.Size(75, 34);
        isEAllowHandleCheckBox.TabIndex = 63;
        isEAllowHandleCheckBox.Text = "Задать\r\nвручную";
        isEAllowHandleCheckBox.UseVisualStyleBackColor = true;
        isEAllowHandleCheckBox.CheckedChanged += EHandleCheckBox_CheckedChanged;

        // 
        // LoadingConditionGroupBox
        // 

        //AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        //AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(temperatureLabel);
        Controls.Add(temperatureTextBox);
        Controls.Add(temperatureDemensionLabel);
        Controls.Add(pressureLabel);
        Controls.Add(pressureTextBox);
        Controls.Add(pressureDemensionLabel);
        Controls.Add(isPressureOutsideCheckBox);
        Controls.Add(sigmaAllowLabel);
        Controls.Add(sigmaAllowTextBox);
        Controls.Add(sigmaAllowDemensionLabel);
        Controls.Add(isSigmaAllowHandleCheckBox);
        Controls.Add(EAllowLabel);
        Controls.Add(EAllowTextBox);
        Controls.Add(EAllowDemensionLabel);
        Controls.Add(isEAllowHandleCheckBox);
        Name = "loadingConditionGroupBox";
        Size = new System.Drawing.Size(351, 144);
        Text = "Условия нагружения";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label temperatureLabel;
    private System.Windows.Forms.Label temperatureDemensionLabel;
    private System.Windows.Forms.Label pressureLabel;
    private System.Windows.Forms.Label pressureDemensionLabel;
    private System.Windows.Forms.Label sigmaAllowLabel;
    private System.Windows.Forms.Label EAllowLabel;
    private System.Windows.Forms.Label sigmaAllowDemensionLabel;
    private System.Windows.Forms.Label EAllowDemensionLabel;
    private System.Windows.Forms.TextBox temperatureTextBox;
    private System.Windows.Forms.TextBox pressureTextBox;
    private System.Windows.Forms.TextBox sigmaAllowTextBox;
    private System.Windows.Forms.TextBox EAllowTextBox;
    private System.Windows.Forms.CheckBox isPressureOutsideCheckBox;
    private System.Windows.Forms.CheckBox isSigmaAllowHandleCheckBox;
    private System.Windows.Forms.CheckBox isEAllowHandleCheckBox;
}