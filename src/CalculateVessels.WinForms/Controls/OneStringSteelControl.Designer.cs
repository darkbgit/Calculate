namespace CalculateVessels.Controls
{
    partial class OneStringSteelControl
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
            components = new System.ComponentModel.Container();
            thicknessComboBox = new ComboBox();
            steelComboBox = new ComboBox();
            label7 = new Label();
            designResourceCheckBox = new CheckBox();
            toolTip1 = new ToolTip(components);
            SuspendLayout();
            // 
            // thicknessComboBox
            // 
            thicknessComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            thicknessComboBox.FormattingEnabled = true;
            thicknessComboBox.Location = new Point(225, 3);
            thicknessComboBox.Margin = new Padding(4, 3, 4, 3);
            thicknessComboBox.Name = "thicknessComboBox";
            thicknessComboBox.Size = new Size(55, 23);
            thicknessComboBox.TabIndex = 23;
            toolTip1.SetToolTip(thicknessComboBox, "Диапазон толщин");
            // 
            // steelComboBox
            // 
            steelComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            steelComboBox.FormattingEnabled = true;
            steelComboBox.Location = new Point(78, 3);
            steelComboBox.Margin = new Padding(4, 3, 4, 3);
            steelComboBox.Name = "steelComboBox";
            steelComboBox.Size = new Size(139, 23);
            steelComboBox.TabIndex = 21;
            steelComboBox.SelectedIndexChanged += SteelComboBox_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(5, 6);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(65, 15);
            label7.TabIndex = 22;
            label7.Text = "Материал:";
            // 
            // designResourceCheckBox
            // 
            designResourceCheckBox.AutoSize = true;
            designResourceCheckBox.Location = new Point(287, 7);
            designResourceCheckBox.Name = "designResourceCheckBox";
            designResourceCheckBox.Size = new Size(15, 14);
            designResourceCheckBox.TabIndex = 27;
            toolTip1.SetToolTip(designResourceCheckBox, "Увеличенный расчетный рессурс");
            designResourceCheckBox.UseVisualStyleBackColor = true;
            // 
            // OneStringSteelControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(designResourceCheckBox);
            Controls.Add(thicknessComboBox);
            Controls.Add(steelComboBox);
            Controls.Add(label7);
            Margin = new Padding(0);
            MaximumSize = new Size(305, 29);
            MinimumSize = new Size(305, 29);
            Name = "OneStringSteelControl";
            Size = new Size(305, 29);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox thicknessComboBox;
        private ComboBox steelComboBox;
        private Label label7;
        private CheckBox designResourceCheckBox;
        private ToolTip toolTip1;
    }
}
