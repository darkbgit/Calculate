
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
        label28 = new System.Windows.Forms.Label();
        label27 = new System.Windows.Forms.Label();
        label26 = new System.Windows.Forms.Label();
        label25 = new System.Windows.Forms.Label();
        label24 = new System.Windows.Forms.Label();
        label23 = new System.Windows.Forms.Label();
        label21 = new System.Windows.Forms.Label();
        cancel_b = new System.Windows.Forms.Button();
        calculate_btn = new System.Windows.Forms.Button();
        preCalculate_btn = new System.Windows.Forms.Button();
        groupBox4 = new System.Windows.Forms.GroupBox();
        p_d_l = new System.Windows.Forms.Label();
        scalc_l = new System.Windows.Forms.Label();
        button4 = new System.Windows.Forms.Button();
        button3 = new System.Windows.Forms.Button();
        defect_btn = new System.Windows.Forms.Button();
        checkBox1 = new System.Windows.Forms.CheckBox();
        getGostDim_b = new System.Windows.Forms.Button();
        getFi_b = new System.Windows.Forms.Button();
        s_tb = new System.Windows.Forms.TextBox();
        label17 = new System.Windows.Forms.Label();
        label15 = new System.Windows.Forms.Label();
        label14 = new System.Windows.Forms.Label();
        label13 = new System.Windows.Forms.Label();
        label12 = new System.Windows.Forms.Label();
        label11 = new System.Windows.Forms.Label();
        label10 = new System.Windows.Forms.Label();
        c3_tb = new System.Windows.Forms.TextBox();
        c2_tb = new System.Windows.Forms.TextBox();
        c1_tb = new System.Windows.Forms.TextBox();
        H_tb = new System.Windows.Forms.TextBox();
        D_tb = new System.Windows.Forms.TextBox();
        fi_tb = new System.Windows.Forms.TextBox();
        steel_cb = new System.Windows.Forms.ComboBox();
        label7 = new System.Windows.Forms.Label();
        pictureBox = new System.Windows.Forms.PictureBox();
        Gost_cb = new System.Windows.Forms.ComboBox();
        label2 = new System.Windows.Forms.Label();
        button1 = new System.Windows.Forms.Button();
        name_tb = new System.Windows.Forms.TextBox();
        label1 = new System.Windows.Forms.Label();
        label20 = new System.Windows.Forms.Label();
        h1_tb = new System.Windows.Forms.TextBox();
        label29 = new System.Windows.Forms.Label();
        groupBox1 = new System.Windows.Forms.GroupBox();
        hemispherical_rb = new System.Windows.Forms.RadioButton();
        ell_rb = new System.Windows.Forms.RadioButton();
        isNozzleCalculateCheckBox = new System.Windows.Forms.CheckBox();
        loadingConditionControl = new LoadingConditionControl();
        loadingConditionsControl = new LoadingConditionsControl();
        groupBox4.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
        groupBox1.SuspendLayout();
        SuspendLayout();
        // 
        // label28
        // 
        label28.AutoSize = true;
        label28.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        label28.Location = new System.Drawing.Point(0, 468);
        label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label28.MaximumSize = new System.Drawing.Size(0, 1);
        label28.MinimumSize = new System.Drawing.Size(350, 0);
        label28.Name = "label28";
        label28.Size = new System.Drawing.Size(350, 1);
        label28.TabIndex = 115;
        label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // label27
        // 
        label27.AutoSize = true;
        label27.Location = new System.Drawing.Point(268, 476);
        label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label27.Name = "label27";
        label27.Size = new System.Drawing.Size(25, 15);
        label27.TabIndex = 114;
        label27.Text = "мм";
        // 
        // label26
        // 
        label26.AutoSize = true;
        label26.Location = new System.Drawing.Point(268, 320);
        label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label26.Name = "label26";
        label26.Size = new System.Drawing.Size(25, 15);
        label26.TabIndex = 113;
        label26.Text = "мм";
        // 
        // label25
        // 
        label25.AutoSize = true;
        label25.Location = new System.Drawing.Point(268, 388);
        label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label25.Name = "label25";
        label25.Size = new System.Drawing.Size(25, 15);
        label25.TabIndex = 112;
        label25.Text = "мм";
        // 
        // label24
        // 
        label24.AutoSize = true;
        label24.Location = new System.Drawing.Point(268, 418);
        label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label24.Name = "label24";
        label24.Size = new System.Drawing.Size(25, 15);
        label24.TabIndex = 111;
        label24.Text = "мм";
        // 
        // label23
        // 
        label23.AutoSize = true;
        label23.Location = new System.Drawing.Point(268, 448);
        label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label23.Name = "label23";
        label23.Size = new System.Drawing.Size(25, 15);
        label23.TabIndex = 110;
        label23.Text = "мм";
        // 
        // label21
        // 
        label21.AutoSize = true;
        label21.Location = new System.Drawing.Point(269, 289);
        label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label21.Name = "label21";
        label21.Size = new System.Drawing.Size(25, 15);
        label21.TabIndex = 108;
        label21.Text = "мм";
        // 
        // cancel_b
        // 
        cancel_b.Location = new System.Drawing.Point(246, 663);
        cancel_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        cancel_b.Name = "cancel_b";
        cancel_b.Size = new System.Drawing.Size(88, 27);
        cancel_b.TabIndex = 107;
        cancel_b.Text = "Cancel";
        cancel_b.UseVisualStyleBackColor = true;
        cancel_b.Click += Cancel_btn_Click;
        // 
        // calculate_btn
        // 
        calculate_btn.Enabled = false;
        calculate_btn.Location = new System.Drawing.Point(151, 663);
        calculate_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        calculate_btn.Name = "calculate_btn";
        calculate_btn.Size = new System.Drawing.Size(88, 27);
        calculate_btn.TabIndex = 106;
        calculate_btn.Text = "Расчет";
        calculate_btn.UseVisualStyleBackColor = true;
        calculate_btn.Click += Calculate_btn_Click;
        // 
        // preCalculate_btn
        // 
        preCalculate_btn.Location = new System.Drawing.Point(13, 648);
        preCalculate_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        preCalculate_btn.Name = "preCalculate_btn";
        preCalculate_btn.Size = new System.Drawing.Size(130, 42);
        preCalculate_btn.TabIndex = 105;
        preCalculate_btn.Text = "Предварительный\r\nрасчет";
        preCalculate_btn.UseVisualStyleBackColor = true;
        preCalculate_btn.Click += PreCalculate_btn_Click;
        // 
        // groupBox4
        // 
        groupBox4.Controls.Add(p_d_l);
        groupBox4.Controls.Add(scalc_l);
        groupBox4.Location = new System.Drawing.Point(381, 626);
        groupBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        groupBox4.Name = "groupBox4";
        groupBox4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
        groupBox4.Size = new System.Drawing.Size(270, 66);
        groupBox4.TabIndex = 104;
        groupBox4.TabStop = false;
        groupBox4.Text = "Результаты расчета";
        // 
        // p_d_l
        // 
        p_d_l.AutoSize = true;
        p_d_l.Location = new System.Drawing.Point(8, 39);
        p_d_l.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        p_d_l.Name = "p_d_l";
        p_d_l.Size = new System.Drawing.Size(22, 15);
        p_d_l.TabIndex = 1;
        p_d_l.Text = "[p]";
        // 
        // scalc_l
        // 
        scalc_l.AutoSize = true;
        scalc_l.Location = new System.Drawing.Point(8, 18);
        scalc_l.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        scalc_l.Name = "scalc_l";
        scalc_l.Size = new System.Drawing.Size(12, 15);
        scalc_l.TabIndex = 0;
        scalc_l.Text = "s";
        // 
        // button4
        // 
        button4.Location = new System.Drawing.Point(361, 385);
        button4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        button4.Name = "button4";
        button4.Size = new System.Drawing.Size(187, 27);
        button4.TabIndex = 101;
        button4.Text = "Малоцикловая прочность >>";
        button4.UseVisualStyleBackColor = true;
        // 
        // button3
        // 
        button3.Location = new System.Drawing.Point(361, 352);
        button3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        button3.Name = "button3";
        button3.Size = new System.Drawing.Size(176, 27);
        button3.TabIndex = 100;
        button3.Text = "Изоляция и футеровка >>";
        button3.UseVisualStyleBackColor = true;
        // 
        // defect_btn
        // 
        defect_btn.Enabled = false;
        defect_btn.Location = new System.Drawing.Point(616, 310);
        defect_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        defect_btn.Name = "defect_btn";
        defect_btn.Size = new System.Drawing.Size(35, 27);
        defect_btn.TabIndex = 99;
        defect_btn.Text = ">>";
        defect_btn.UseVisualStyleBackColor = true;
        // 
        // checkBox1
        // 
        checkBox1.AutoSize = true;
        checkBox1.Location = new System.Drawing.Point(407, 316);
        checkBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        checkBox1.Name = "checkBox1";
        checkBox1.Size = new System.Drawing.Size(201, 19);
        checkBox1.TabIndex = 98;
        checkBox1.Text = "Дефекты по ГОСТ 34233.11-2017";
        checkBox1.UseVisualStyleBackColor = true;
        checkBox1.CheckedChanged += Defect_chb_CheckedChanged;
        // 
        // getGostDim_b
        // 
        getGostDim_b.Location = new System.Drawing.Point(307, 282);
        getGostDim_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        getGostDim_b.Name = "getGostDim_b";
        getGostDim_b.Size = new System.Drawing.Size(43, 93);
        getGostDim_b.TabIndex = 97;
        getGostDim_b.Text = ">>";
        getGostDim_b.UseVisualStyleBackColor = true;
        getGostDim_b.Click += GetGostDim_btn_Click;
        // 
        // getFi_b
        // 
        getFi_b.Location = new System.Drawing.Point(307, 252);
        getFi_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        getFi_b.Name = "getFi_b";
        getFi_b.Size = new System.Drawing.Size(43, 23);
        getFi_b.TabIndex = 96;
        getFi_b.Text = ">>";
        getFi_b.UseVisualStyleBackColor = true;
        getFi_b.Click += GetPhi_btn_Click;
        // 
        // s_tb
        // 
        s_tb.Location = new System.Drawing.Point(214, 473);
        s_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        s_tb.Name = "s_tb";
        s_tb.Size = new System.Drawing.Size(46, 23);
        s_tb.TabIndex = 91;
        s_tb.Text = "8";
        s_tb.TextChanged += DisabledCalculateBtn;
        // 
        // label17
        // 
        label17.AutoSize = true;
        label17.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        label17.Location = new System.Drawing.Point(77, 476);
        label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label17.Name = "label17";
        label17.Size = new System.Drawing.Size(131, 15);
        label17.TabIndex = 90;
        label17.Text = "Принятая толщина, s:";
        // 
        // label15
        // 
        label15.AutoSize = true;
        label15.Location = new System.Drawing.Point(30, 448);
        label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label15.Name = "label15";
        label15.Size = new System.Drawing.Size(178, 15);
        label15.TabIndex = 88;
        label15.Text = "Технологическая прибавка, c3:";
        // 
        // label14
        // 
        label14.AutoSize = true;
        label14.Location = new System.Drawing.Point(75, 418);
        label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label14.Name = "label14";
        label14.Size = new System.Drawing.Size(133, 15);
        label14.TabIndex = 87;
        label14.Text = "Минусовой допуск, c2:";
        // 
        // label13
        // 
        label13.AutoSize = true;
        label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        label13.Location = new System.Drawing.Point(62, 388);
        label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label13.Name = "label13";
        label13.Size = new System.Drawing.Size(146, 13);
        label13.TabIndex = 86;
        label13.Text = "Прибавка на коррозию, c1:";
        // 
        // label12
        // 
        label12.AutoSize = true;
        label12.Location = new System.Drawing.Point(22, 306);
        label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label12.Name = "label12";
        label12.Size = new System.Drawing.Size(186, 45);
        label12.TabIndex = 85;
        label12.Text = "Высота выпуклой части днища\r\nпо внутренней поверхности без\r\nучета цилиндрической части, Н:";
        label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
        // 
        // label11
        // 
        label11.AutoSize = true;
        label11.Location = new System.Drawing.Point(29, 285);
        label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label11.Name = "label11";
        label11.Size = new System.Drawing.Size(179, 15);
        label11.TabIndex = 84;
        label11.Text = "Внутренний диаметр днища, D:";
        // 
        // label10
        // 
        label10.AutoSize = true;
        label10.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
        label10.Location = new System.Drawing.Point(16, 256);
        label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label10.Name = "label10";
        label10.Size = new System.Drawing.Size(192, 15);
        label10.TabIndex = 83;
        label10.Text = "К-т прочности сварного шва, φp:";
        // 
        // c3_tb
        // 
        c3_tb.Location = new System.Drawing.Point(214, 444);
        c3_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        c3_tb.Name = "c3_tb";
        c3_tb.Size = new System.Drawing.Size(46, 23);
        c3_tb.TabIndex = 80;
        c3_tb.TextChanged += DisabledCalculateBtn;
        // 
        // c2_tb
        // 
        c2_tb.Location = new System.Drawing.Point(214, 414);
        c2_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        c2_tb.Name = "c2_tb";
        c2_tb.Size = new System.Drawing.Size(46, 23);
        c2_tb.TabIndex = 79;
        c2_tb.Text = "0.8";
        c2_tb.TextChanged += DisabledCalculateBtn;
        // 
        // c1_tb
        // 
        c1_tb.Location = new System.Drawing.Point(214, 384);
        c1_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        c1_tb.Name = "c1_tb";
        c1_tb.Size = new System.Drawing.Size(46, 23);
        c1_tb.TabIndex = 78;
        c1_tb.Text = "1";
        c1_tb.TextChanged += DisabledCalculateBtn;
        // 
        // H_tb
        // 
        H_tb.Location = new System.Drawing.Point(214, 317);
        H_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        H_tb.Name = "H_tb";
        H_tb.Size = new System.Drawing.Size(46, 23);
        H_tb.TabIndex = 77;
        H_tb.TextChanged += DisabledCalculateBtn;
        // 
        // D_tb
        // 
        D_tb.Location = new System.Drawing.Point(214, 282);
        D_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        D_tb.Name = "D_tb";
        D_tb.Size = new System.Drawing.Size(46, 23);
        D_tb.TabIndex = 76;
        D_tb.Text = "1000";
        D_tb.TextChanged += DisabledCalculateBtn;
        // 
        // fi_tb
        // 
        fi_tb.Location = new System.Drawing.Point(214, 252);
        fi_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        fi_tb.Name = "fi_tb";
        fi_tb.Size = new System.Drawing.Size(46, 23);
        fi_tb.TabIndex = 75;
        fi_tb.Text = "1";
        fi_tb.TextChanged += DisabledCalculateBtn;
        // 
        // steel_cb
        // 
        steel_cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        steel_cb.FormattingEnabled = true;
        steel_cb.Location = new System.Drawing.Point(214, 73);
        steel_cb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        steel_cb.Name = "steel_cb";
        steel_cb.Size = new System.Drawing.Size(139, 23);
        steel_cb.TabIndex = 71;
        // 
        // label7
        // 
        label7.AutoSize = true;
        label7.Location = new System.Drawing.Point(100, 76);
        label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label7.Name = "label7";
        label7.Size = new System.Drawing.Size(108, 15);
        label7.TabIndex = 70;
        label7.Text = "Марка материала:";
        // 
        // pictureBox
        // 
        pictureBox.Image = (System.Drawing.Image)resources.GetObject("pictureBox.Image");
        pictureBox.InitialImage = (System.Drawing.Image)resources.GetObject("pictureBox.InitialImage");
        pictureBox.Location = new System.Drawing.Point(361, 44);
        pictureBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        pictureBox.Name = "pictureBox";
        pictureBox.Size = new System.Drawing.Size(300, 200);
        pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        pictureBox.TabIndex = 67;
        pictureBox.TabStop = false;
        // 
        // Gost_cb
        // 
        Gost_cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        Gost_cb.FormattingEnabled = true;
        Gost_cb.Location = new System.Drawing.Point(214, 44);
        Gost_cb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        Gost_cb.Name = "Gost_cb";
        Gost_cb.Size = new System.Drawing.Size(139, 23);
        Gost_cb.TabIndex = 65;
        Gost_cb.Tag = "";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(64, 47);
        label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(144, 15);
        label2.TabIndex = 63;
        label2.Text = "Нормативный документ:";
        // 
        // button1
        // 
        button1.Location = new System.Drawing.Point(361, 13);
        button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(284, 25);
        button1.TabIndex = 62;
        button1.Text = "Расчетная схема эллиптического днища";
        button1.UseVisualStyleBackColor = true;
        // 
        // name_tb
        // 
        name_tb.Location = new System.Drawing.Point(214, 13);
        name_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        name_tb.Name = "name_tb";
        name_tb.Size = new System.Drawing.Size(139, 23);
        name_tb.TabIndex = 61;
        name_tb.TextChanged += DisabledCalculateBtn;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(91, 16);
        label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(117, 15);
        label1.TabIndex = 60;
        label1.Text = "Название элемента:";
        // 
        // label20
        // 
        label20.AutoSize = true;
        label20.Location = new System.Drawing.Point(60, 351);
        label20.Name = "label20";
        label20.Size = new System.Drawing.Size(147, 30);
        label20.TabIndex = 116;
        label20.Text = "Длина цилиндрической\r\nотбартованной части, h1:";
        // 
        // h1_tb
        // 
        h1_tb.Location = new System.Drawing.Point(214, 355);
        h1_tb.Name = "h1_tb";
        h1_tb.Size = new System.Drawing.Size(46, 23);
        h1_tb.TabIndex = 117;
        h1_tb.TextChanged += DisabledCalculateBtn;
        // 
        // label29
        // 
        label29.AutoSize = true;
        label29.Location = new System.Drawing.Point(271, 358);
        label29.Name = "label29";
        label29.Size = new System.Drawing.Size(25, 15);
        label29.TabIndex = 118;
        label29.Text = "мм";
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(hemispherical_rb);
        groupBox1.Controls.Add(ell_rb);
        groupBox1.Location = new System.Drawing.Point(361, 255);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new System.Drawing.Size(300, 49);
        groupBox1.TabIndex = 119;
        groupBox1.TabStop = false;
        groupBox1.Text = "Вид днища";
        // 
        // hemispherical_rb
        // 
        hemispherical_rb.AutoSize = true;
        hemispherical_rb.Location = new System.Drawing.Point(146, 22);
        hemispherical_rb.Name = "hemispherical_rb";
        hemispherical_rb.Size = new System.Drawing.Size(127, 19);
        hemispherical_rb.TabIndex = 1;
        hemispherical_rb.Text = "Полусферическое";
        hemispherical_rb.UseVisualStyleBackColor = true;
        hemispherical_rb.CheckedChanged += Ell_Hemispherical_rb_CheckedChanged;
        // 
        // ell_rb
        // 
        ell_rb.AutoSize = true;
        ell_rb.Checked = true;
        ell_rb.Location = new System.Drawing.Point(6, 22);
        ell_rb.Name = "ell_rb";
        ell_rb.Size = new System.Drawing.Size(110, 19);
        ell_rb.TabIndex = 0;
        ell_rb.TabStop = true;
        ell_rb.Text = "Эллиптическое";
        ell_rb.UseVisualStyleBackColor = true;
        ell_rb.CheckedChanged += Ell_Hemispherical_rb_CheckedChanged;
        // 
        // isNozzleCalculateCheckBox
        // 
        isNozzleCalculateCheckBox.AutoSize = true;
        isNozzleCalculateCheckBox.Location = new System.Drawing.Point(16, 623);
        isNozzleCalculateCheckBox.Name = "isNozzleCalculateCheckBox";
        isNozzleCalculateCheckBox.Size = new System.Drawing.Size(190, 19);
        isNozzleCalculateCheckBox.TabIndex = 124;
        isNozzleCalculateCheckBox.Text = "Расчитать штуцер в обечайке";
        isNozzleCalculateCheckBox.UseVisualStyleBackColor = true;
        // 
        // loadingConditionControl
        // 
        loadingConditionControl.Location = new System.Drawing.Point(3, 102);
        loadingConditionControl.Name = "loadingConditionControl";
        loadingConditionControl.Size = new System.Drawing.Size(354, 144);
        loadingConditionControl.TabIndex = 125;
        loadingConditionControl.TabStop = false;
        // 
        // loadingConditionsControl
        // 
        loadingConditionsControl.Location = new System.Drawing.Point(12, 502);
        loadingConditionsControl.Name = "loadingConditionsControl";
        loadingConditionsControl.Size = new System.Drawing.Size(380, 115);
        loadingConditionsControl.TabIndex = 126;
        // 
        // EllipticalShellForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(664, 704);
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
        Controls.Add(steel_cb);
        Controls.Add(label7);
        Controls.Add(pictureBox);
        Controls.Add(Gost_cb);
        Controls.Add(label2);
        Controls.Add(button1);
        Controls.Add(name_tb);
        Controls.Add(label1);
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

    private System.Windows.Forms.Label label28;
    private System.Windows.Forms.Label label27;
    private System.Windows.Forms.Label label26;
    private System.Windows.Forms.Label label25;
    private System.Windows.Forms.Label label24;
    private System.Windows.Forms.Label label23;
    private System.Windows.Forms.Label label21;
    private System.Windows.Forms.Button cancel_b;
    private System.Windows.Forms.Button calculate_btn;
    private System.Windows.Forms.Button preCalculate_btn;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.Label p_d_l;
    private System.Windows.Forms.Label scalc_l;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button defect_btn;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.Button getGostDim_b;
    private System.Windows.Forms.Button getFi_b;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox c2_tb;
    private System.Windows.Forms.TextBox c1_tb;
    internal System.Windows.Forms.TextBox fi_tb;
    private System.Windows.Forms.ComboBox steel_cb;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.PictureBox pictureBox;
    private System.Windows.Forms.ComboBox Gost_cb;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.TextBox name_tb;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.Label label29;
    internal System.Windows.Forms.TextBox H_tb;
    internal System.Windows.Forms.TextBox D_tb;
    internal System.Windows.Forms.TextBox h1_tb;
    internal System.Windows.Forms.TextBox s_tb;
    internal System.Windows.Forms.TextBox c3_tb;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton hemispherical_rb;
    private System.Windows.Forms.RadioButton ell_rb;
    private System.Windows.Forms.CheckBox isNozzleCalculateCheckBox;
    private LoadingConditionControl loadingConditionControl;
    private LoadingConditionsControl loadingConditionsControl;
}