using System.Drawing;
using System.Windows.Forms;

namespace CalculateVessels.Controls;

partial class LoadingConditionControl
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
        temperatureLabel = new Label();
        temperatureTextBox = new TextBox();
        temperatureDemensionLabel = new Label();
        pressureLabel = new Label();
        pressureTextBox = new TextBox();
        pressureDemensionLabel = new Label();
        sigmaAllowLabel = new Label();
        EAllowLabel = new Label();
        sigmaAllowDemensionLabel = new Label();
        EAllowDemensionLabel = new Label();
        sigmaAllowTextBox = new TextBox();
        EAllowTextBox = new TextBox();
        isPressureOutsideCheckBox = new CheckBox();
        isSigmaAllowHandleCheckBox = new CheckBox();
        isEAllowHandleCheckBox = new CheckBox();
        groupBox1 = new GroupBox();
        groupBox1.SuspendLayout();
        SuspendLayout();
        // 
        // temperatureLabel
        // 
        temperatureLabel.AutoSize = true;
        temperatureLabel.Location = new Point(7, 25);
        temperatureLabel.Margin = new Padding(4, 0, 4, 0);
        temperatureLabel.Name = "temperatureLabel";
        temperatureLabel.Size = new Size(149, 15);
        temperatureLabel.TabIndex = 4;
        temperatureLabel.Text = "Расчетная температура, t:";
        // 
        // temperatureTextBox
        // 
        temperatureTextBox.Location = new Point(162, 22);
        temperatureTextBox.Margin = new Padding(4, 3, 4, 3);
        temperatureTextBox.Name = "temperatureTextBox";
        temperatureTextBox.Size = new Size(46, 23);
        temperatureTextBox.TabIndex = 3;
        temperatureTextBox.Text = "20";
        // 
        // temperatureDemensionLabel
        // 
        temperatureDemensionLabel.AutoSize = true;
        temperatureDemensionLabel.Location = new Point(212, 25);
        temperatureDemensionLabel.Margin = new Padding(0, 0, 4, 0);
        temperatureDemensionLabel.Name = "temperatureDemensionLabel";
        temperatureDemensionLabel.Size = new Size(20, 15);
        temperatureDemensionLabel.TabIndex = 8;
        temperatureDemensionLabel.Text = "°C";
        // 
        // pressureLabel
        // 
        pressureLabel.AutoSize = true;
        pressureLabel.Location = new Point(22, 55);
        pressureLabel.Margin = new Padding(4, 0, 4, 0);
        pressureLabel.Name = "pressureLabel";
        pressureLabel.Size = new Size(134, 15);
        pressureLabel.TabIndex = 9;
        pressureLabel.Text = "Расчетное давление, p:";
        // 
        // pressureTextBox
        // 
        pressureTextBox.Location = new Point(162, 52);
        pressureTextBox.Margin = new Padding(4, 3, 4, 3);
        pressureTextBox.Name = "pressureTextBox";
        pressureTextBox.Size = new Size(46, 23);
        pressureTextBox.TabIndex = 4;
        pressureTextBox.Text = "3.1";
        // 
        // pressureDemensionLabel
        // 
        pressureDemensionLabel.AutoSize = true;
        pressureDemensionLabel.Location = new Point(216, 55);
        pressureDemensionLabel.Margin = new Padding(4, 0, 4, 0);
        pressureDemensionLabel.Name = "pressureDemensionLabel";
        pressureDemensionLabel.Size = new Size(33, 15);
        pressureDemensionLabel.TabIndex = 13;
        pressureDemensionLabel.Text = "МПа";
        // 
        // sigmaAllowLabel
        // 
        sigmaAllowLabel.AutoSize = true;
        sigmaAllowLabel.Location = new Point(52, 76);
        sigmaAllowLabel.Margin = new Padding(4, 0, 4, 0);
        sigmaAllowLabel.Name = "sigmaAllowLabel";
        sigmaAllowLabel.Size = new Size(102, 30);
        sigmaAllowLabel.TabIndex = 16;
        sigmaAllowLabel.Text = "Допускаемое\r\n напряжение, [σ]:";
        sigmaAllowLabel.TextAlign = ContentAlignment.TopRight;
        // 
        // EAllowLabel
        // 
        EAllowLabel.AutoSize = true;
        EAllowLabel.Location = new Point(33, 105);
        EAllowLabel.Margin = new Padding(4, 0, 4, 0);
        EAllowLabel.Name = "EAllowLabel";
        EAllowLabel.Size = new Size(121, 30);
        EAllowLabel.TabIndex = 26;
        EAllowLabel.Text = "Модуль продольной\r\n упругости, E:";
        EAllowLabel.TextAlign = ContentAlignment.TopRight;
        // 
        // sigmaAllowDemensionLabel
        // 
        sigmaAllowDemensionLabel.AutoSize = true;
        sigmaAllowDemensionLabel.Location = new Point(216, 84);
        sigmaAllowDemensionLabel.Margin = new Padding(4, 0, 4, 0);
        sigmaAllowDemensionLabel.Name = "sigmaAllowDemensionLabel";
        sigmaAllowDemensionLabel.Size = new Size(33, 15);
        sigmaAllowDemensionLabel.TabIndex = 36;
        sigmaAllowDemensionLabel.Text = "МПа";
        // 
        // EAllowDemensionLabel
        // 
        EAllowDemensionLabel.AutoSize = true;
        EAllowDemensionLabel.Location = new Point(216, 113);
        EAllowDemensionLabel.Margin = new Padding(4, 0, 4, 0);
        EAllowDemensionLabel.Name = "EAllowDemensionLabel";
        EAllowDemensionLabel.Size = new Size(33, 15);
        EAllowDemensionLabel.TabIndex = 38;
        EAllowDemensionLabel.Text = "МПа";
        // 
        // sigmaAllowTextBox
        // 
        sigmaAllowTextBox.Enabled = false;
        sigmaAllowTextBox.Location = new Point(162, 81);
        sigmaAllowTextBox.Margin = new Padding(4, 3, 4, 3);
        sigmaAllowTextBox.Name = "sigmaAllowTextBox";
        sigmaAllowTextBox.Size = new Size(46, 23);
        sigmaAllowTextBox.TabIndex = 17;
        // 
        // EAllowTextBox
        // 
        EAllowTextBox.Enabled = false;
        EAllowTextBox.Location = new Point(162, 109);
        EAllowTextBox.Margin = new Padding(4, 3, 4, 3);
        EAllowTextBox.Name = "EAllowTextBox";
        EAllowTextBox.Size = new Size(46, 23);
        EAllowTextBox.TabIndex = 18;
        // 
        // isPressureOutsideCheckBox
        // 
        isPressureOutsideCheckBox.AutoSize = true;
        isPressureOutsideCheckBox.Location = new Point(256, 51);
        isPressureOutsideCheckBox.Name = "isPressureOutsideCheckBox";
        isPressureOutsideCheckBox.Size = new Size(81, 19);
        isPressureOutsideCheckBox.TabIndex = 14;
        isPressureOutsideCheckBox.Text = "наружное";
        isPressureOutsideCheckBox.UseVisualStyleBackColor = true;
        // 
        // isSigmaAllowHandleCheckBox
        // 
        isSigmaAllowHandleCheckBox.AutoSize = true;
        isSigmaAllowHandleCheckBox.Location = new Point(256, 74);
        isSigmaAllowHandleCheckBox.Name = "isSigmaAllowHandleCheckBox";
        isSigmaAllowHandleCheckBox.Size = new Size(75, 34);
        isSigmaAllowHandleCheckBox.TabIndex = 62;
        isSigmaAllowHandleCheckBox.Text = "Задать\r\nвручную";
        isSigmaAllowHandleCheckBox.UseVisualStyleBackColor = true;
        isSigmaAllowHandleCheckBox.CheckedChanged += SigmaAllowHandleCheckBox_CheckedChanged;
        // 
        // isEAllowHandleCheckBox
        // 
        isEAllowHandleCheckBox.AutoSize = true;
        isEAllowHandleCheckBox.Location = new Point(256, 103);
        isEAllowHandleCheckBox.Name = "isEAllowHandleCheckBox";
        isEAllowHandleCheckBox.Size = new Size(75, 34);
        isEAllowHandleCheckBox.TabIndex = 63;
        isEAllowHandleCheckBox.Text = "Задать\r\nвручную";
        isEAllowHandleCheckBox.UseVisualStyleBackColor = true;
        isEAllowHandleCheckBox.CheckedChanged += EHandleCheckBox_CheckedChanged;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(temperatureTextBox);
        groupBox1.Controls.Add(temperatureLabel);
        groupBox1.Controls.Add(isEAllowHandleCheckBox);
        groupBox1.Controls.Add(EAllowDemensionLabel);
        groupBox1.Controls.Add(temperatureDemensionLabel);
        groupBox1.Controls.Add(EAllowTextBox);
        groupBox1.Controls.Add(pressureLabel);
        groupBox1.Controls.Add(EAllowLabel);
        groupBox1.Controls.Add(pressureTextBox);
        groupBox1.Controls.Add(isSigmaAllowHandleCheckBox);
        groupBox1.Controls.Add(pressureDemensionLabel);
        groupBox1.Controls.Add(sigmaAllowDemensionLabel);
        groupBox1.Controls.Add(isPressureOutsideCheckBox);
        groupBox1.Controls.Add(sigmaAllowTextBox);
        groupBox1.Controls.Add(sigmaAllowLabel);
        groupBox1.Location = new Point(3, 3);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(350, 140);
        groupBox1.TabIndex = 154;
        groupBox1.TabStop = false;
        groupBox1.Text = "Условия нагружения";
        // 
        // LoadingConditionControl
        // 
        Controls.Add(groupBox1);
        Name = "LoadingConditionControl";
        Size = new Size(354, 144);
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        ResumeLayout(false);
    }

    private Label temperatureLabel;
    private Label temperatureDemensionLabel;
    private Label pressureLabel;
    private Label pressureDemensionLabel;
    private Label sigmaAllowLabel;
    private Label EAllowLabel;
    private Label sigmaAllowDemensionLabel;
    private Label EAllowDemensionLabel;
    private TextBox temperatureTextBox;
    private TextBox pressureTextBox;
    private TextBox sigmaAllowTextBox;
    private TextBox EAllowTextBox;
    private CheckBox isPressureOutsideCheckBox;
    private CheckBox isSigmaAllowHandleCheckBox;
    private CheckBox isEAllowHandleCheckBox;
    private GroupBox groupBox1;
}