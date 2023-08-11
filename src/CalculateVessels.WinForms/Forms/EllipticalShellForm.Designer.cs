
using CalculateVessels.Controls;

namespace CalculateVessels.Forms;

partial class EllipticalShellForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EllipticalShellForm));
        label28 = new Label();
        label27 = new Label();
        label26 = new Label();
        label25 = new Label();
        label24 = new Label();
        label23 = new Label();
        label21 = new Label();
        cancel_b = new Button();
        calculate_btn = new Button();
        preCalculate_btn = new Button();
        groupBox4 = new GroupBox();
        p_d_l = new Label();
        scalc_l = new Label();
        button4 = new Button();
        button3 = new Button();
        defect_btn = new Button();
        checkBox1 = new CheckBox();
        getGostDim_b = new Button();
        getFi_b = new Button();
        s_tb = new TextBox();
        label17 = new Label();
        label15 = new Label();
        label14 = new Label();
        label13 = new Label();
        label12 = new Label();
        label11 = new Label();
        label10 = new Label();
        c3_tb = new TextBox();
        c2_tb = new TextBox();
        c1_tb = new TextBox();
        H_tb = new TextBox();
        D_tb = new TextBox();
        fi_tb = new TextBox();
        pictureBox = new PictureBox();
        Gost_cb = new ComboBox();
        label2 = new Label();
        button1 = new Button();
        name_tb = new TextBox();
        label1 = new Label();
        label20 = new Label();
        h1_tb = new TextBox();
        label29 = new Label();
        groupBox1 = new GroupBox();
        hemispherical_rb = new RadioButton();
        ell_rb = new RadioButton();
        isNozzleCalculateCheckBox = new CheckBox();
        loadingConditionControl = new LoadingConditionControl();
        loadingConditionsControl = new LoadingConditionsControl();
        steelControl = new SteelControl();
        groupBox4.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
        groupBox1.SuspendLayout();
        SuspendLayout();
        // 
        // label28
        // 
        label28.AutoSize = true;
        label28.BorderStyle = BorderStyle.Fixed3D;
        label28.Location = new Point(-2, 518);
        label28.Margin = new Padding(4, 0, 4, 0);
        label28.MaximumSize = new Size(0, 1);
        label28.MinimumSize = new Size(350, 0);
        label28.Name = "label28";
        label28.Size = new Size(350, 1);
        label28.TabIndex = 115;
        label28.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // label27
        // 
        label27.AutoSize = true;
        label27.Location = new Point(266, 526);
        label27.Margin = new Padding(4, 0, 4, 0);
        label27.Name = "label27";
        label27.Size = new Size(25, 15);
        label27.TabIndex = 114;
        label27.Text = "мм";
        // 
        // label26
        // 
        label26.AutoSize = true;
        label26.Location = new Point(266, 370);
        label26.Margin = new Padding(4, 0, 4, 0);
        label26.Name = "label26";
        label26.Size = new Size(25, 15);
        label26.TabIndex = 113;
        label26.Text = "мм";
        // 
        // label25
        // 
        label25.AutoSize = true;
        label25.Location = new Point(266, 438);
        label25.Margin = new Padding(4, 0, 4, 0);
        label25.Name = "label25";
        label25.Size = new Size(25, 15);
        label25.TabIndex = 112;
        label25.Text = "мм";
        // 
        // label24
        // 
        label24.AutoSize = true;
        label24.Location = new Point(266, 468);
        label24.Margin = new Padding(4, 0, 4, 0);
        label24.Name = "label24";
        label24.Size = new Size(25, 15);
        label24.TabIndex = 111;
        label24.Text = "мм";
        // 
        // label23
        // 
        label23.AutoSize = true;
        label23.Location = new Point(266, 498);
        label23.Margin = new Padding(4, 0, 4, 0);
        label23.Name = "label23";
        label23.Size = new Size(25, 15);
        label23.TabIndex = 110;
        label23.Text = "мм";
        // 
        // label21
        // 
        label21.AutoSize = true;
        label21.Location = new Point(267, 339);
        label21.Margin = new Padding(4, 0, 4, 0);
        label21.Name = "label21";
        label21.Size = new Size(25, 15);
        label21.TabIndex = 108;
        label21.Text = "мм";
        // 
        // cancel_b
        // 
        cancel_b.Location = new Point(244, 713);
        cancel_b.Margin = new Padding(4, 3, 4, 3);
        cancel_b.Name = "cancel_b";
        cancel_b.Size = new Size(88, 27);
        cancel_b.TabIndex = 107;
        cancel_b.Text = "Cancel";
        cancel_b.UseVisualStyleBackColor = true;
        cancel_b.Click += Cancel_btn_Click;
        // 
        // calculate_btn
        // 
        calculate_btn.Enabled = false;
        calculate_btn.Location = new Point(149, 713);
        calculate_btn.Margin = new Padding(4, 3, 4, 3);
        calculate_btn.Name = "calculate_btn";
        calculate_btn.Size = new Size(88, 27);
        calculate_btn.TabIndex = 106;
        calculate_btn.Text = "Расчет";
        calculate_btn.UseVisualStyleBackColor = true;
        calculate_btn.Click += Calculate_btn_Click;
        // 
        // preCalculate_btn
        // 
        preCalculate_btn.Location = new Point(11, 698);
        preCalculate_btn.Margin = new Padding(4, 3, 4, 3);
        preCalculate_btn.Name = "preCalculate_btn";
        preCalculate_btn.Size = new Size(130, 42);
        preCalculate_btn.TabIndex = 105;
        preCalculate_btn.Text = "Предварительный\r\nрасчет";
        preCalculate_btn.UseVisualStyleBackColor = true;
        preCalculate_btn.Click += PreCalculate_btn_Click;
        // 
        // groupBox4
        // 
        groupBox4.Controls.Add(p_d_l);
        groupBox4.Controls.Add(scalc_l);
        groupBox4.Location = new Point(381, 673);
        groupBox4.Margin = new Padding(4, 3, 4, 3);
        groupBox4.Name = "groupBox4";
        groupBox4.Padding = new Padding(4, 3, 4, 3);
        groupBox4.Size = new Size(270, 66);
        groupBox4.TabIndex = 104;
        groupBox4.TabStop = false;
        groupBox4.Text = "Результаты расчета";
        // 
        // p_d_l
        // 
        p_d_l.AutoSize = true;
        p_d_l.Location = new Point(8, 39);
        p_d_l.Margin = new Padding(4, 0, 4, 0);
        p_d_l.Name = "p_d_l";
        p_d_l.Size = new Size(22, 15);
        p_d_l.TabIndex = 1;
        p_d_l.Text = "[p]";
        // 
        // scalc_l
        // 
        scalc_l.AutoSize = true;
        scalc_l.Location = new Point(8, 18);
        scalc_l.Margin = new Padding(4, 0, 4, 0);
        scalc_l.Name = "scalc_l";
        scalc_l.Size = new Size(12, 15);
        scalc_l.TabIndex = 0;
        scalc_l.Text = "s";
        // 
        // button4
        // 
        button4.Location = new Point(361, 385);
        button4.Margin = new Padding(4, 3, 4, 3);
        button4.Name = "button4";
        button4.Size = new Size(187, 27);
        button4.TabIndex = 101;
        button4.Text = "Малоцикловая прочность >>";
        button4.UseVisualStyleBackColor = true;
        // 
        // button3
        // 
        button3.Location = new Point(361, 352);
        button3.Margin = new Padding(4, 3, 4, 3);
        button3.Name = "button3";
        button3.Size = new Size(176, 27);
        button3.TabIndex = 100;
        button3.Text = "Изоляция и футеровка >>";
        button3.UseVisualStyleBackColor = true;
        // 
        // defect_btn
        // 
        defect_btn.Enabled = false;
        defect_btn.Location = new Point(616, 310);
        defect_btn.Margin = new Padding(4, 3, 4, 3);
        defect_btn.Name = "defect_btn";
        defect_btn.Size = new Size(35, 27);
        defect_btn.TabIndex = 99;
        defect_btn.Text = ">>";
        defect_btn.UseVisualStyleBackColor = true;
        // 
        // checkBox1
        // 
        checkBox1.AutoSize = true;
        checkBox1.Location = new Point(407, 316);
        checkBox1.Margin = new Padding(4, 3, 4, 3);
        checkBox1.Name = "checkBox1";
        checkBox1.Size = new Size(201, 19);
        checkBox1.TabIndex = 98;
        checkBox1.Text = "Дефекты по ГОСТ 34233.11-2017";
        checkBox1.UseVisualStyleBackColor = true;
        checkBox1.CheckedChanged += Defect_chb_CheckedChanged;
        // 
        // getGostDim_b
        // 
        getGostDim_b.Location = new Point(305, 332);
        getGostDim_b.Margin = new Padding(4, 3, 4, 3);
        getGostDim_b.Name = "getGostDim_b";
        getGostDim_b.Size = new Size(43, 93);
        getGostDim_b.TabIndex = 97;
        getGostDim_b.Text = ">>";
        getGostDim_b.UseVisualStyleBackColor = true;
        getGostDim_b.Click += GetGostDim_btn_Click;
        // 
        // getFi_b
        // 
        getFi_b.Location = new Point(305, 302);
        getFi_b.Margin = new Padding(4, 3, 4, 3);
        getFi_b.Name = "getFi_b";
        getFi_b.Size = new Size(43, 23);
        getFi_b.TabIndex = 96;
        getFi_b.Text = ">>";
        getFi_b.UseVisualStyleBackColor = true;
        getFi_b.Click += GetPhi_btn_Click;
        // 
        // s_tb
        // 
        s_tb.Location = new Point(212, 523);
        s_tb.Margin = new Padding(4, 3, 4, 3);
        s_tb.Name = "s_tb";
        s_tb.Size = new Size(46, 23);
        s_tb.TabIndex = 91;
        s_tb.Text = "8";
        s_tb.TextChanged += DisabledCalculateBtn;
        // 
        // label17
        // 
        label17.AutoSize = true;
        label17.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        label17.Location = new Point(75, 526);
        label17.Margin = new Padding(4, 0, 4, 0);
        label17.Name = "label17";
        label17.Size = new Size(131, 15);
        label17.TabIndex = 90;
        label17.Text = "Принятая толщина, s:";
        // 
        // label15
        // 
        label15.AutoSize = true;
        label15.Location = new Point(28, 498);
        label15.Margin = new Padding(4, 0, 4, 0);
        label15.Name = "label15";
        label15.Size = new Size(178, 15);
        label15.TabIndex = 88;
        label15.Text = "Технологическая прибавка, c3:";
        // 
        // label14
        // 
        label14.AutoSize = true;
        label14.Location = new Point(73, 468);
        label14.Margin = new Padding(4, 0, 4, 0);
        label14.Name = "label14";
        label14.Size = new Size(133, 15);
        label14.TabIndex = 87;
        label14.Text = "Минусовой допуск, c2:";
        // 
        // label13
        // 
        label13.AutoSize = true;
        label13.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
        label13.Location = new Point(60, 438);
        label13.Margin = new Padding(4, 0, 4, 0);
        label13.Name = "label13";
        label13.Size = new Size(146, 13);
        label13.TabIndex = 86;
        label13.Text = "Прибавка на коррозию, c1:";
        // 
        // label12
        // 
        label12.AutoSize = true;
        label12.Location = new Point(20, 356);
        label12.Margin = new Padding(4, 0, 4, 0);
        label12.Name = "label12";
        label12.Size = new Size(186, 45);
        label12.TabIndex = 85;
        label12.Text = "Высота выпуклой части днища\r\nпо внутренней поверхности без\r\nучета цилиндрической части, Н:";
        label12.TextAlign = ContentAlignment.TopRight;
        // 
        // label11
        // 
        label11.AutoSize = true;
        label11.Location = new Point(27, 335);
        label11.Margin = new Padding(4, 0, 4, 0);
        label11.Name = "label11";
        label11.Size = new Size(179, 15);
        label11.TabIndex = 84;
        label11.Text = "Внутренний диаметр днища, D:";
        // 
        // label10
        // 
        label10.AutoSize = true;
        label10.ImageAlign = ContentAlignment.MiddleRight;
        label10.Location = new Point(14, 306);
        label10.Margin = new Padding(4, 0, 4, 0);
        label10.Name = "label10";
        label10.Size = new Size(192, 15);
        label10.TabIndex = 83;
        label10.Text = "К-т прочности сварного шва, φp:";
        // 
        // c3_tb
        // 
        c3_tb.Location = new Point(212, 494);
        c3_tb.Margin = new Padding(4, 3, 4, 3);
        c3_tb.Name = "c3_tb";
        c3_tb.Size = new Size(46, 23);
        c3_tb.TabIndex = 80;
        c3_tb.TextChanged += DisabledCalculateBtn;
        // 
        // c2_tb
        // 
        c2_tb.Location = new Point(212, 464);
        c2_tb.Margin = new Padding(4, 3, 4, 3);
        c2_tb.Name = "c2_tb";
        c2_tb.Size = new Size(46, 23);
        c2_tb.TabIndex = 79;
        c2_tb.Text = "0.8";
        c2_tb.TextChanged += DisabledCalculateBtn;
        // 
        // c1_tb
        // 
        c1_tb.Location = new Point(212, 434);
        c1_tb.Margin = new Padding(4, 3, 4, 3);
        c1_tb.Name = "c1_tb";
        c1_tb.Size = new Size(46, 23);
        c1_tb.TabIndex = 78;
        c1_tb.Text = "1";
        c1_tb.TextChanged += DisabledCalculateBtn;
        // 
        // H_tb
        // 
        H_tb.Location = new Point(212, 367);
        H_tb.Margin = new Padding(4, 3, 4, 3);
        H_tb.Name = "H_tb";
        H_tb.Size = new Size(46, 23);
        H_tb.TabIndex = 77;
        H_tb.TextChanged += DisabledCalculateBtn;
        // 
        // D_tb
        // 
        D_tb.Location = new Point(212, 332);
        D_tb.Margin = new Padding(4, 3, 4, 3);
        D_tb.Name = "D_tb";
        D_tb.Size = new Size(46, 23);
        D_tb.TabIndex = 76;
        D_tb.Text = "1000";
        D_tb.TextChanged += DisabledCalculateBtn;
        // 
        // fi_tb
        // 
        fi_tb.Location = new Point(212, 302);
        fi_tb.Margin = new Padding(4, 3, 4, 3);
        fi_tb.Name = "fi_tb";
        fi_tb.Size = new Size(46, 23);
        fi_tb.TabIndex = 75;
        fi_tb.Text = "1";
        fi_tb.TextChanged += DisabledCalculateBtn;
        // 
        // pictureBox
        // 
        pictureBox.Image = (Image)resources.GetObject("pictureBox.Image");
        pictureBox.InitialImage = (Image)resources.GetObject("pictureBox.InitialImage");
        pictureBox.Location = new Point(361, 44);
        pictureBox.Margin = new Padding(4, 3, 4, 3);
        pictureBox.Name = "pictureBox";
        pictureBox.Size = new Size(300, 200);
        pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
        pictureBox.TabIndex = 67;
        pictureBox.TabStop = false;
        // 
        // Gost_cb
        // 
        Gost_cb.DropDownStyle = ComboBoxStyle.DropDownList;
        Gost_cb.FormattingEnabled = true;
        Gost_cb.Location = new Point(214, 44);
        Gost_cb.Margin = new Padding(4, 3, 4, 3);
        Gost_cb.Name = "Gost_cb";
        Gost_cb.Size = new Size(139, 23);
        Gost_cb.TabIndex = 65;
        Gost_cb.Tag = "";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(64, 47);
        label2.Margin = new Padding(4, 0, 4, 0);
        label2.Name = "label2";
        label2.Size = new Size(144, 15);
        label2.TabIndex = 63;
        label2.Text = "Нормативный документ:";
        // 
        // button1
        // 
        button1.Location = new Point(361, 13);
        button1.Margin = new Padding(4, 3, 4, 3);
        button1.Name = "button1";
        button1.Size = new Size(284, 25);
        button1.TabIndex = 62;
        button1.Text = "Расчетная схема эллиптического днища";
        button1.UseVisualStyleBackColor = true;
        // 
        // name_tb
        // 
        name_tb.Location = new Point(214, 13);
        name_tb.Margin = new Padding(4, 3, 4, 3);
        name_tb.Name = "name_tb";
        name_tb.Size = new Size(139, 23);
        name_tb.TabIndex = 61;
        name_tb.TextChanged += DisabledCalculateBtn;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(91, 16);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(117, 15);
        label1.TabIndex = 60;
        label1.Text = "Название элемента:";
        // 
        // label20
        // 
        label20.AutoSize = true;
        label20.Location = new Point(58, 401);
        label20.Name = "label20";
        label20.Size = new Size(147, 30);
        label20.TabIndex = 116;
        label20.Text = "Длина цилиндрической\r\nотбартованной части, h1:";
        // 
        // h1_tb
        // 
        h1_tb.Location = new Point(212, 405);
        h1_tb.Name = "h1_tb";
        h1_tb.Size = new Size(46, 23);
        h1_tb.TabIndex = 117;
        h1_tb.TextChanged += DisabledCalculateBtn;
        // 
        // label29
        // 
        label29.AutoSize = true;
        label29.Location = new Point(269, 408);
        label29.Name = "label29";
        label29.Size = new Size(25, 15);
        label29.TabIndex = 118;
        label29.Text = "мм";
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(hemispherical_rb);
        groupBox1.Controls.Add(ell_rb);
        groupBox1.Location = new Point(361, 255);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(300, 49);
        groupBox1.TabIndex = 119;
        groupBox1.TabStop = false;
        groupBox1.Text = "Вид днища";
        // 
        // hemispherical_rb
        // 
        hemispherical_rb.AutoSize = true;
        hemispherical_rb.Location = new Point(146, 22);
        hemispherical_rb.Name = "hemispherical_rb";
        hemispherical_rb.Size = new Size(127, 19);
        hemispherical_rb.TabIndex = 1;
        hemispherical_rb.Text = "Полусферическое";
        hemispherical_rb.UseVisualStyleBackColor = true;
        hemispherical_rb.CheckedChanged += Ell_Hemispherical_rb_CheckedChanged;
        // 
        // ell_rb
        // 
        ell_rb.AutoSize = true;
        ell_rb.Checked = true;
        ell_rb.Location = new Point(6, 22);
        ell_rb.Name = "ell_rb";
        ell_rb.Size = new Size(110, 19);
        ell_rb.TabIndex = 0;
        ell_rb.TabStop = true;
        ell_rb.Text = "Эллиптическое";
        ell_rb.UseVisualStyleBackColor = true;
        ell_rb.CheckedChanged += Ell_Hemispherical_rb_CheckedChanged;
        // 
        // isNozzleCalculateCheckBox
        // 
        isNozzleCalculateCheckBox.AutoSize = true;
        isNozzleCalculateCheckBox.Location = new Point(14, 673);
        isNozzleCalculateCheckBox.Name = "isNozzleCalculateCheckBox";
        isNozzleCalculateCheckBox.Size = new Size(190, 19);
        isNozzleCalculateCheckBox.TabIndex = 124;
        isNozzleCalculateCheckBox.Text = "Расчитать штуцер в обечайке";
        isNozzleCalculateCheckBox.UseVisualStyleBackColor = true;
        // 
        // loadingConditionControl
        // 
        loadingConditionControl.Location = new Point(1, 152);
        loadingConditionControl.Name = "loadingConditionControl";
        loadingConditionControl.Size = new Size(354, 144);
        loadingConditionControl.TabIndex = 125;
        loadingConditionControl.TabStop = false;
        // 
        // loadingConditionsControl
        // 
        loadingConditionsControl.Location = new Point(10, 552);
        loadingConditionsControl.Name = "loadingConditionsControl";
        loadingConditionsControl.Size = new Size(380, 115);
        loadingConditionsControl.TabIndex = 126;
        // 
        // steelControl
        // 
        steelControl.Location = new Point(88, 73);
        steelControl.MaximumSize = new Size(265, 80);
        steelControl.MinimumSize = new Size(265, 80);
        steelControl.Name = "steelControl";
        steelControl.Size = new Size(265, 80);
        steelControl.TabIndex = 127;
        // 
        // EllipticalShellForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(664, 751);
        Controls.Add(steelControl);
        Controls.Add(loadingConditionsControl);
        Controls.Add(loadingConditionControl);
        Controls.Add(isNozzleCalculateCheckBox);
        Controls.Add(groupBox1);
        Controls.Add(label29);
        Controls.Add(h1_tb);
        Controls.Add(label20);
        Controls.Add(label28);
        Controls.Add(label27);
        Controls.Add(label26);
        Controls.Add(label25);
        Controls.Add(label24);
        Controls.Add(label23);
        Controls.Add(label21);
        Controls.Add(cancel_b);
        Controls.Add(calculate_btn);
        Controls.Add(preCalculate_btn);
        Controls.Add(groupBox4);
        Controls.Add(button4);
        Controls.Add(button3);
        Controls.Add(defect_btn);
        Controls.Add(checkBox1);
        Controls.Add(getGostDim_b);
        Controls.Add(getFi_b);
        Controls.Add(s_tb);
        Controls.Add(label17);
        Controls.Add(label15);
        Controls.Add(label14);
        Controls.Add(label13);
        Controls.Add(label12);
        Controls.Add(label11);
        Controls.Add(label10);
        Controls.Add(c3_tb);
        Controls.Add(c2_tb);
        Controls.Add(c1_tb);
        Controls.Add(H_tb);
        Controls.Add(D_tb);
        Controls.Add(fi_tb);
        Controls.Add(pictureBox);
        Controls.Add(Gost_cb);
        Controls.Add(label2);
        Controls.Add(button1);
        Controls.Add(name_tb);
        Controls.Add(label1);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "EllipticalShellForm";
        Text = "EllipticalShellForm";
        FormClosing += EllipticalShellForm_FormClosing;
        Load += EllipticalShellForm_Load;
        groupBox4.ResumeLayout(false);
        groupBox4.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label28;
    private Label label27;
    private Label label26;
    private Label label25;
    private Label label24;
    private Label label23;
    private Label label21;
    private Button cancel_b;
    private Button calculate_btn;
    private Button preCalculate_btn;
    private GroupBox groupBox4;
    private Label p_d_l;
    private Label scalc_l;
    private Button button4;
    private Button button3;
    private Button defect_btn;
    private CheckBox checkBox1;
    private Button getGostDim_b;
    private Button getFi_b;
    private Label label17;
    private Label label15;
    private Label label14;
    private Label label13;
    private Label label12;
    private Label label11;
    private Label label10;
    private TextBox c2_tb;
    private TextBox c1_tb;
    internal TextBox fi_tb;
    private PictureBox pictureBox;
    private ComboBox Gost_cb;
    private Label label2;
    private Button button1;
    private Label label1;
    private Label label20;
    private Label label29;
    internal TextBox H_tb;
    internal TextBox D_tb;
    internal TextBox h1_tb;
    internal TextBox s_tb;
    internal TextBox c3_tb;
    private GroupBox groupBox1;
    private RadioButton hemispherical_rb;
    private RadioButton ell_rb;
    private CheckBox isNozzleCalculateCheckBox;
    private LoadingConditionControl loadingConditionControl;
    private LoadingConditionsControl loadingConditionsControl;
    private SteelControl steelControl;
    internal TextBox name_tb;
}