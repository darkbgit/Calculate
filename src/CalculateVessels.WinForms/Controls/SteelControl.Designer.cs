namespace CalculateVessels.Controls
{
    partial class SteelControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            steelComboBox = new ComboBox();
            label7 = new Label();
            thicknessComboBox = new ComboBox();
            thicknessLabel = new Label();
            designResourceComboBox = new ComboBox();
            designResourceLabel = new Label();
            SuspendLayout();
            // 
            // steelComboBox
            // 
            steelComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            steelComboBox.FormattingEnabled = true;
            steelComboBox.Location = new Point(83, 3);
            steelComboBox.Margin = new Padding(4, 3, 4, 3);
            steelComboBox.Name = "steelComboBox";
            steelComboBox.Size = new Size(139, 23);
            steelComboBox.TabIndex = 15;
            steelComboBox.SelectedIndexChanged += SteelComboBox_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(10, 6);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(65, 15);
            label7.TabIndex = 16;
            label7.Text = "Материал:";
            // 
            // thicknessComboBox
            // 
            thicknessComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            thicknessComboBox.FormattingEnabled = true;
            thicknessComboBox.Location = new Point(4, 52);
            thicknessComboBox.Margin = new Padding(4, 3, 4, 3);
            thicknessComboBox.Name = "thicknessComboBox";
            thicknessComboBox.Size = new Size(112, 23);
            thicknessComboBox.TabIndex = 17;
            // 
            // thicknessLabel
            // 
            thicknessLabel.AutoSize = true;
            thicknessLabel.Location = new Point(22, 34);
            thicknessLabel.Margin = new Padding(4, 0, 4, 0);
            thicknessLabel.Name = "thicknessLabel";
            thicknessLabel.Size = new Size(82, 15);
            thicknessLabel.TabIndex = 18;
            thicknessLabel.Text = "Толщина, мм";
            // 
            // designResourceComboBox
            // 
            designResourceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            designResourceComboBox.FormattingEnabled = true;
            designResourceComboBox.Location = new Point(124, 52);
            designResourceComboBox.Margin = new Padding(4, 3, 4, 3);
            designResourceComboBox.Name = "designResourceComboBox";
            designResourceComboBox.Size = new Size(139, 23);
            designResourceComboBox.TabIndex = 19;
            // 
            // designResourceLabel
            // 
            designResourceLabel.AutoSize = true;
            designResourceLabel.Location = new Point(126, 34);
            designResourceLabel.Margin = new Padding(4, 0, 4, 0);
            designResourceLabel.Name = "designResourceLabel";
            designResourceLabel.Size = new Size(121, 15);
            designResourceLabel.TabIndex = 20;
            designResourceLabel.Text = "Расчетный ресурс, ч";
            // 
            // SteelControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(designResourceComboBox);
            Controls.Add(designResourceLabel);
            Controls.Add(thicknessComboBox);
            Controls.Add(thicknessLabel);
            Controls.Add(steelComboBox);
            Controls.Add(label7);
            MaximumSize = new Size(265, 80);
            MinimumSize = new Size(265, 80);
            Name = "SteelControl";
            Size = new Size(265, 80);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox steelComboBox;
        private Label label7;
        private ComboBox thicknessComboBox;
        private Label thicknessLabel;
        private ComboBox designResourceComboBox;
        private Label designResourceLabel;
    }
}
