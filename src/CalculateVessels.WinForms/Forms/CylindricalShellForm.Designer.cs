using CalculateVessels.Controls;

namespace CalculateVessels.Forms;

partial class CylindricalShellForm
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
        label1 = new Label();
        Name_tb = new TextBox();
        button1 = new Button();
        label2 = new Label();
        Gost_cb = new ComboBox();
        fi_tb = new TextBox();
        D_tb = new TextBox();
        l_tb = new TextBox();
        c1_tb = new TextBox();
        c2_tb = new TextBox();
        c3_tb = new TextBox();
        label10 = new Label();
        label11 = new Label();
        label12 = new Label();
        label13 = new Label();
        label14 = new Label();
        label15 = new Label();
        label17 = new Label();
        s_tb = new TextBox();
        getFi_b = new Button();
        getL_b = new Button();
        defect_chb = new CheckBox();
        defect_btn = new Button();
        button3 = new Button();
        button4 = new Button();
        groupBox2 = new GroupBox();
        stressHand_rb = new RadioButton();
        stressCalc_rb = new RadioButton();
        force_gb = new GroupBox();
        F_tb = new TextBox();
        panel1 = new Panel();
        forceCompress_rb = new RadioButton();
        forceStretch_rb = new RadioButton();
        rb7 = new RadioButton();
        rb6 = new RadioButton();
        rb5 = new RadioButton();
        rb4 = new RadioButton();
        rb3 = new RadioButton();
        rb2 = new RadioButton();
        rb1 = new RadioButton();
        f_pb = new PictureBox();
        label20 = new Label();
        fq_panel = new Panel();
        fq_mes_l = new Label();
        fq_l = new Label();
        fq_tb = new TextBox();
        groupBox4 = new GroupBox();
        p_d_l = new Label();
        scalc_l = new Label();
        predCalc_b = new Button();
        calculate_btn = new Button();
        cancel_b = new Button();
        shell_pb = new PictureBox();
        label21 = new Label();
        label23 = new Label();
        label24 = new Label();
        label25 = new Label();
        label26 = new Label();
        label27 = new Label();
        label28 = new Label();
        M_gb = new GroupBox();
        label29 = new Label();
        M_tb = new TextBox();
        Q_gb = new GroupBox();
        label30 = new Label();
        Q_tb = new TextBox();
        isNozzleCalculateCheckBox = new CheckBox();
        loadingConditionControl = new LoadingConditionControl();
        loadingConditionsControl = new LoadingConditionsControl();
        steelControl = new SteelControl();
        groupBox2.SuspendLayout();
        force_gb.SuspendLayout();
        panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)f_pb).BeginInit();
        fq_panel.SuspendLayout();
        groupBox4.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)shell_pb).BeginInit();
        M_gb.SuspendLayout();
        Q_gb.SuspendLayout();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(101, 15);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(117, 15);
        label1.TabIndex = 0;
        label1.Text = "Название элемента:";
        // 
        // Name_tb
        // 
        Name_tb.Location = new Point(224, 12);
        Name_tb.Margin = new Padding(4, 3, 4, 3);
        Name_tb.Name = "Name_tb";
        Name_tb.Size = new Size(139, 23);
        Name_tb.TabIndex = 1;
        Name_tb.TextChanged += DisabledCalculateBtn;
        // 
        // button1
        // 
        button1.Location = new Point(371, 9);
        button1.Margin = new Padding(4, 3, 4, 3);
        button1.Name = "button1";
        button1.Size = new Size(300, 27);
        button1.TabIndex = 2;
        button1.Text = "Расчетная схема цилиндрической обечайки";
        button1.UseVisualStyleBackColor = true;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(74, 44);
        label2.Margin = new Padding(4, 0, 4, 0);
        label2.Name = "label2";
        label2.Size = new Size(144, 15);
        label2.TabIndex = 3;
        label2.Text = "Нормативный документ:";
        // 
        // Gost_cb
        // 
        Gost_cb.DropDownStyle = ComboBoxStyle.DropDownList;
        Gost_cb.FormattingEnabled = true;
        Gost_cb.Location = new Point(224, 41);
        Gost_cb.Margin = new Padding(4, 3, 4, 3);
        Gost_cb.Name = "Gost_cb";
        Gost_cb.Size = new Size(139, 23);
        Gost_cb.TabIndex = 2;
        Gost_cb.Tag = "";
        Gost_cb.TextChanged += DisabledCalculateBtn;
        // 
        // fi_tb
        // 
        fi_tb.Location = new Point(221, 306);
        fi_tb.Margin = new Padding(4, 3, 4, 3);
        fi_tb.Name = "fi_tb";
        fi_tb.Size = new Size(46, 23);
        fi_tb.TabIndex = 8;
        fi_tb.Text = "1";
        fi_tb.TextChanged += DisabledCalculateBtn;
        // 
        // D_tb
        // 
        D_tb.Location = new Point(221, 335);
        D_tb.Margin = new Padding(4, 3, 4, 3);
        D_tb.Name = "D_tb";
        D_tb.Size = new Size(46, 23);
        D_tb.TabIndex = 9;
        D_tb.Text = "1000";
        D_tb.TextChanged += DisabledCalculateBtn;
        // 
        // l_tb
        // 
        l_tb.Location = new Point(221, 364);
        l_tb.Margin = new Padding(4, 3, 4, 3);
        l_tb.Name = "l_tb";
        l_tb.Size = new Size(46, 23);
        l_tb.TabIndex = 10;
        l_tb.TextChanged += DisabledCalculateBtn;
        // 
        // c1_tb
        // 
        c1_tb.Location = new Point(221, 393);
        c1_tb.Margin = new Padding(4, 3, 4, 3);
        c1_tb.Name = "c1_tb";
        c1_tb.Size = new Size(46, 23);
        c1_tb.TabIndex = 11;
        c1_tb.Text = "2";
        c1_tb.TextChanged += DisabledCalculateBtn;
        // 
        // c2_tb
        // 
        c2_tb.Location = new Point(221, 422);
        c2_tb.Margin = new Padding(4, 3, 4, 3);
        c2_tb.Name = "c2_tb";
        c2_tb.Size = new Size(46, 23);
        c2_tb.TabIndex = 12;
        c2_tb.Text = "0.8";
        c2_tb.TextChanged += DisabledCalculateBtn;
        // 
        // c3_tb
        // 
        c3_tb.Location = new Point(221, 451);
        c3_tb.Margin = new Padding(4, 3, 4, 3);
        c3_tb.Name = "c3_tb";
        c3_tb.Size = new Size(46, 23);
        c3_tb.TabIndex = 13;
        c3_tb.TextChanged += DisabledCalculateBtn;
        // 
        // label10
        // 
        label10.AutoSize = true;
        label10.ImageAlign = ContentAlignment.MiddleRight;
        label10.Location = new Point(23, 310);
        label10.Margin = new Padding(4, 0, 4, 0);
        label10.Name = "label10";
        label10.Size = new Size(192, 15);
        label10.TabIndex = 27;
        label10.Text = "К-т прочности сварного шва, φp:";
        // 
        // label11
        // 
        label11.AutoSize = true;
        label11.Location = new Point(20, 338);
        label11.Margin = new Padding(4, 0, 4, 0);
        label11.Name = "label11";
        label11.Size = new Size(195, 15);
        label11.TabIndex = 28;
        label11.Text = "Внутренний диаметр обечайки, D:";
        // 
        // label12
        // 
        label12.AutoSize = true;
        label12.Location = new Point(48, 368);
        label12.Margin = new Padding(4, 0, 4, 0);
        label12.Name = "label12";
        label12.Size = new Size(167, 15);
        label12.TabIndex = 29;
        label12.Text = "Расчетная длина обечайки, l:";
        // 
        // label13
        // 
        label13.AutoSize = true;
        label13.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
        label13.Location = new Point(69, 397);
        label13.Margin = new Padding(4, 0, 4, 0);
        label13.Name = "label13";
        label13.Size = new Size(146, 13);
        label13.TabIndex = 30;
        label13.Text = "Прибавка на коррозию, c1:";
        // 
        // label14
        // 
        label14.AutoSize = true;
        label14.Location = new Point(82, 426);
        label14.Margin = new Padding(4, 0, 4, 0);
        label14.Name = "label14";
        label14.Size = new Size(133, 15);
        label14.TabIndex = 31;
        label14.Text = "Минусовой допуск, c2:";
        // 
        // label15
        // 
        label15.AutoSize = true;
        label15.Location = new Point(37, 455);
        label15.Margin = new Padding(4, 0, 4, 0);
        label15.Name = "label15";
        label15.Size = new Size(178, 15);
        label15.TabIndex = 32;
        label15.Text = "Технологическая прибавка, c3:";
        // 
        // label17
        // 
        label17.AutoSize = true;
        label17.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        label17.Location = new Point(84, 483);
        label17.Margin = new Padding(4, 0, 4, 0);
        label17.Name = "label17";
        label17.Size = new Size(131, 15);
        label17.TabIndex = 34;
        label17.Text = "Принятая толщина, s:";
        // 
        // s_tb
        // 
        s_tb.Location = new Point(221, 480);
        s_tb.Margin = new Padding(4, 3, 4, 3);
        s_tb.Name = "s_tb";
        s_tb.Size = new Size(46, 23);
        s_tb.TabIndex = 15;
        s_tb.Text = "14";
        s_tb.TextChanged += DisabledCalculateBtn;
        // 
        // getFi_b
        // 
        getFi_b.Location = new Point(313, 306);
        getFi_b.Margin = new Padding(4, 3, 4, 3);
        getFi_b.Name = "getFi_b";
        getFi_b.Size = new Size(43, 23);
        getFi_b.TabIndex = 40;
        getFi_b.Text = ">>";
        getFi_b.UseVisualStyleBackColor = true;
        getFi_b.Click += GetPhi_btn_Click;
        // 
        // getL_b
        // 
        getL_b.Enabled = false;
        getL_b.Location = new Point(313, 364);
        getL_b.Margin = new Padding(4, 3, 4, 3);
        getL_b.Name = "getL_b";
        getL_b.Size = new Size(43, 23);
        getL_b.TabIndex = 41;
        getL_b.Text = ">>";
        getL_b.UseVisualStyleBackColor = true;
        // 
        // defect_chb
        // 
        defect_chb.AutoSize = true;
        defect_chb.Location = new Point(10, 519);
        defect_chb.Margin = new Padding(4, 3, 4, 3);
        defect_chb.Name = "defect_chb";
        defect_chb.Size = new Size(201, 19);
        defect_chb.TabIndex = 42;
        defect_chb.Text = "Дефекты по ГОСТ 34233.11-2017";
        defect_chb.UseVisualStyleBackColor = true;
        defect_chb.CheckedChanged += Defect_chb_CheckedChanged;
        // 
        // defect_btn
        // 
        defect_btn.Enabled = false;
        defect_btn.Location = new Point(244, 514);
        defect_btn.Margin = new Padding(4, 3, 4, 3);
        defect_btn.Name = "defect_btn";
        defect_btn.Size = new Size(35, 27);
        defect_btn.TabIndex = 43;
        defect_btn.Text = ">>";
        defect_btn.UseVisualStyleBackColor = true;
        // 
        // button3
        // 
        button3.Enabled = false;
        button3.Location = new Point(529, 645);
        button3.Margin = new Padding(4, 3, 4, 3);
        button3.Name = "button3";
        button3.Size = new Size(168, 27);
        button3.TabIndex = 44;
        button3.Text = "Изоляция и футеровка >>";
        button3.UseVisualStyleBackColor = true;
        // 
        // button4
        // 
        button4.Enabled = false;
        button4.Location = new Point(522, 612);
        button4.Margin = new Padding(4, 3, 4, 3);
        button4.Name = "button4";
        button4.Size = new Size(175, 27);
        button4.TabIndex = 45;
        button4.Text = "Малоцикловая прочность >>";
        button4.UseVisualStyleBackColor = true;
        // 
        // groupBox2
        // 
        groupBox2.Controls.Add(stressHand_rb);
        groupBox2.Controls.Add(stressCalc_rb);
        groupBox2.Location = new Point(371, 249);
        groupBox2.Margin = new Padding(4, 3, 4, 3);
        groupBox2.Name = "groupBox2";
        groupBox2.Padding = new Padding(4, 3, 4, 3);
        groupBox2.Size = new Size(326, 52);
        groupBox2.TabIndex = 46;
        groupBox2.TabStop = false;
        groupBox2.Text = "Нагрузки";
        // 
        // stressHand_rb
        // 
        stressHand_rb.AutoSize = true;
        stressHand_rb.Location = new Point(191, 23);
        stressHand_rb.Margin = new Padding(4, 3, 4, 3);
        stressHand_rb.Name = "stressHand_rb";
        stressHand_rb.Size = new Size(125, 19);
        stressHand_rb.TabIndex = 1;
        stressHand_rb.Text = "Задавать вручную";
        stressHand_rb.UseVisualStyleBackColor = true;
        stressHand_rb.CheckedChanged += Stress_rb_CheckedChanged;
        // 
        // stressCalc_rb
        // 
        stressCalc_rb.AutoSize = true;
        stressCalc_rb.Checked = true;
        stressCalc_rb.Location = new Point(8, 23);
        stressCalc_rb.Margin = new Padding(4, 3, 4, 3);
        stressCalc_rb.Name = "stressCalc_rb";
        stressCalc_rb.Size = new Size(160, 19);
        stressCalc_rb.TabIndex = 0;
        stressCalc_rb.TabStop = true;
        stressCalc_rb.Text = "Определять при расчете";
        stressCalc_rb.UseVisualStyleBackColor = true;
        stressCalc_rb.CheckedChanged += Stress_rb_CheckedChanged;
        // 
        // force_gb
        // 
        force_gb.Controls.Add(F_tb);
        force_gb.Controls.Add(panel1);
        force_gb.Controls.Add(rb7);
        force_gb.Controls.Add(rb6);
        force_gb.Controls.Add(rb5);
        force_gb.Controls.Add(rb4);
        force_gb.Controls.Add(rb3);
        force_gb.Controls.Add(rb2);
        force_gb.Controls.Add(rb1);
        force_gb.Controls.Add(f_pb);
        force_gb.Controls.Add(label20);
        force_gb.Controls.Add(fq_panel);
        force_gb.Enabled = false;
        force_gb.Location = new Point(371, 307);
        force_gb.Margin = new Padding(4, 3, 4, 3);
        force_gb.Name = "force_gb";
        force_gb.Padding = new Padding(4, 3, 4, 3);
        force_gb.Size = new Size(326, 179);
        force_gb.TabIndex = 47;
        force_gb.TabStop = false;
        force_gb.Text = "Расчетное осевое усилие, F";
        // 
        // F_tb
        // 
        F_tb.Location = new Point(8, 147);
        F_tb.Name = "F_tb";
        F_tb.Size = new Size(100, 23);
        F_tb.TabIndex = 10;
        F_tb.Text = "0";
        F_tb.TextChanged += DisabledCalculateBtn;
        // 
        // panel1
        // 
        panel1.Controls.Add(forceCompress_rb);
        panel1.Controls.Add(forceStretch_rb);
        panel1.Location = new Point(8, 22);
        panel1.Name = "panel1";
        panel1.Size = new Size(308, 25);
        panel1.TabIndex = 9;
        // 
        // forceCompress_rb
        // 
        forceCompress_rb.AutoSize = true;
        forceCompress_rb.Location = new Point(138, 2);
        forceCompress_rb.Name = "forceCompress_rb";
        forceCompress_rb.Size = new Size(98, 19);
        forceCompress_rb.TabIndex = 1;
        forceCompress_rb.Text = "Сжимающие";
        forceCompress_rb.UseVisualStyleBackColor = true;
        forceCompress_rb.CheckedChanged += ForceStretchCompress_rb_CheckedChanged;
        // 
        // forceStretch_rb
        // 
        forceStretch_rb.AutoSize = true;
        forceStretch_rb.Checked = true;
        forceStretch_rb.Location = new Point(19, 2);
        forceStretch_rb.Name = "forceStretch_rb";
        forceStretch_rb.Size = new Size(113, 19);
        forceStretch_rb.TabIndex = 0;
        forceStretch_rb.TabStop = true;
        forceStretch_rb.Text = "Растягивающие";
        forceStretch_rb.UseVisualStyleBackColor = true;
        forceStretch_rb.CheckedChanged += ForceStretchCompress_rb_CheckedChanged;
        // 
        // rb7
        // 
        rb7.AutoSize = true;
        rb7.Enabled = false;
        rb7.Location = new Point(292, 49);
        rb7.Margin = new Padding(4, 3, 4, 3);
        rb7.Name = "rb7";
        rb7.Size = new Size(31, 19);
        rb7.TabIndex = 8;
        rb7.Text = "7";
        rb7.UseVisualStyleBackColor = true;
        rb7.CheckedChanged += Force_rb_CheckedChanged;
        // 
        // rb6
        // 
        rb6.AutoSize = true;
        rb6.Enabled = false;
        rb6.Location = new Point(254, 104);
        rb6.Margin = new Padding(4, 3, 4, 3);
        rb6.Name = "rb6";
        rb6.Size = new Size(31, 19);
        rb6.TabIndex = 7;
        rb6.Text = "6";
        rb6.UseVisualStyleBackColor = true;
        rb6.CheckedChanged += Force_rb_CheckedChanged;
        // 
        // rb5
        // 
        rb5.AutoSize = true;
        rb5.Enabled = false;
        rb5.Location = new Point(253, 76);
        rb5.Margin = new Padding(4, 3, 4, 3);
        rb5.Name = "rb5";
        rb5.Size = new Size(31, 19);
        rb5.TabIndex = 6;
        rb5.Text = "5";
        rb5.UseVisualStyleBackColor = true;
        rb5.CheckedChanged += Force_rb_CheckedChanged;
        // 
        // rb4
        // 
        rb4.AutoSize = true;
        rb4.Enabled = false;
        rb4.Location = new Point(253, 49);
        rb4.Margin = new Padding(4, 3, 4, 3);
        rb4.Name = "rb4";
        rb4.Size = new Size(31, 19);
        rb4.TabIndex = 5;
        rb4.Text = "4";
        rb4.UseVisualStyleBackColor = true;
        rb4.CheckedChanged += Force_rb_CheckedChanged;
        // 
        // rb3
        // 
        rb3.AutoSize = true;
        rb3.Enabled = false;
        rb3.Location = new Point(215, 104);
        rb3.Margin = new Padding(4, 3, 4, 3);
        rb3.Name = "rb3";
        rb3.Size = new Size(31, 19);
        rb3.TabIndex = 4;
        rb3.Text = "3";
        rb3.UseVisualStyleBackColor = true;
        rb3.CheckedChanged += Force_rb_CheckedChanged;
        // 
        // rb2
        // 
        rb2.AutoSize = true;
        rb2.Enabled = false;
        rb2.Location = new Point(215, 76);
        rb2.Margin = new Padding(4, 3, 4, 3);
        rb2.Name = "rb2";
        rb2.Size = new Size(31, 19);
        rb2.TabIndex = 3;
        rb2.Text = "2";
        rb2.UseVisualStyleBackColor = true;
        rb2.CheckedChanged += Force_rb_CheckedChanged;
        // 
        // rb1
        // 
        rb1.AutoSize = true;
        rb1.Checked = true;
        rb1.Enabled = false;
        rb1.Location = new Point(214, 49);
        rb1.Margin = new Padding(4, 3, 4, 3);
        rb1.Name = "rb1";
        rb1.Size = new Size(31, 19);
        rb1.TabIndex = 2;
        rb1.TabStop = true;
        rb1.Text = "1";
        rb1.UseVisualStyleBackColor = true;
        rb1.CheckedChanged += Force_rb_CheckedChanged;
        // 
        // f_pb
        // 
        f_pb.InitialImage = null;
        f_pb.Location = new Point(8, 49);
        f_pb.Margin = new Padding(4, 3, 4, 3);
        f_pb.Name = "f_pb";
        f_pb.Size = new Size(198, 92);
        f_pb.SizeMode = PictureBoxSizeMode.Zoom;
        f_pb.TabIndex = 1;
        f_pb.TabStop = false;
        // 
        // label20
        // 
        label20.AutoSize = true;
        label20.Location = new Point(115, 150);
        label20.Margin = new Padding(4, 0, 4, 0);
        label20.Name = "label20";
        label20.Size = new Size(16, 15);
        label20.TabIndex = 0;
        label20.Text = "H";
        // 
        // fq_panel
        // 
        fq_panel.Controls.Add(fq_mes_l);
        fq_panel.Controls.Add(fq_l);
        fq_panel.Controls.Add(fq_tb);
        fq_panel.Location = new Point(138, 144);
        fq_panel.Name = "fq_panel";
        fq_panel.Size = new Size(177, 30);
        fq_panel.TabIndex = 11;
        fq_panel.Visible = false;
        // 
        // fq_mes_l
        // 
        fq_mes_l.AutoSize = true;
        fq_mes_l.Location = new Point(146, 8);
        fq_mes_l.Name = "fq_mes_l";
        fq_mes_l.Size = new Size(0, 15);
        fq_mes_l.TabIndex = 2;
        // 
        // fq_l
        // 
        fq_l.AutoSize = true;
        fq_l.Location = new Point(22, 8);
        fq_l.Name = "fq_l";
        fq_l.Size = new Size(11, 15);
        fq_l.TabIndex = 1;
        fq_l.Text = "f";
        // 
        // fq_tb
        // 
        fq_tb.Location = new Point(39, 3);
        fq_tb.Name = "fq_tb";
        fq_tb.Size = new Size(100, 23);
        fq_tb.TabIndex = 0;
        fq_tb.TextChanged += DisabledCalculateBtn;
        // 
        // groupBox4
        // 
        groupBox4.Controls.Add(p_d_l);
        groupBox4.Controls.Add(scalc_l);
        groupBox4.Location = new Point(370, 686);
        groupBox4.Margin = new Padding(4, 3, 4, 3);
        groupBox4.Name = "groupBox4";
        groupBox4.Padding = new Padding(4, 3, 4, 3);
        groupBox4.Size = new Size(327, 66);
        groupBox4.TabIndex = 48;
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
        scalc_l.Location = new Point(8, 19);
        scalc_l.Margin = new Padding(4, 0, 4, 0);
        scalc_l.Name = "scalc_l";
        scalc_l.Size = new Size(12, 15);
        scalc_l.TabIndex = 0;
        scalc_l.Text = "s";
        // 
        // predCalc_b
        // 
        predCalc_b.Location = new Point(12, 707);
        predCalc_b.Margin = new Padding(4, 3, 4, 3);
        predCalc_b.Name = "predCalc_b";
        predCalc_b.Size = new Size(130, 42);
        predCalc_b.TabIndex = 16;
        predCalc_b.Text = "Предварительный\r\nрасчет";
        predCalc_b.UseVisualStyleBackColor = true;
        predCalc_b.Click += PreCalculate_btn_Click;
        // 
        // calculate_btn
        // 
        calculate_btn.Enabled = false;
        calculate_btn.Location = new Point(150, 720);
        calculate_btn.Margin = new Padding(4, 3, 4, 3);
        calculate_btn.Name = "calculate_btn";
        calculate_btn.Size = new Size(88, 27);
        calculate_btn.TabIndex = 17;
        calculate_btn.Text = "Расчет";
        calculate_btn.UseVisualStyleBackColor = true;
        calculate_btn.Click += Calculate_btn_Click;
        // 
        // cancel_b
        // 
        cancel_b.Location = new Point(245, 719);
        cancel_b.Margin = new Padding(4, 3, 4, 3);
        cancel_b.Name = "cancel_b";
        cancel_b.Size = new Size(88, 27);
        cancel_b.TabIndex = 18;
        cancel_b.Text = "Cancel";
        cancel_b.UseVisualStyleBackColor = true;
        cancel_b.Click += Cancel_btn_Click;
        // 
        // shell_pb
        // 
        shell_pb.InitialImage = null;
        shell_pb.Location = new Point(371, 43);
        shell_pb.Margin = new Padding(4, 3, 4, 3);
        shell_pb.Name = "shell_pb";
        shell_pb.Size = new Size(300, 200);
        shell_pb.SizeMode = PictureBoxSizeMode.AutoSize;
        shell_pb.TabIndex = 7;
        shell_pb.TabStop = false;
        // 
        // label21
        // 
        label21.AutoSize = true;
        label21.Location = new Point(276, 338);
        label21.Margin = new Padding(4, 0, 4, 0);
        label21.Name = "label21";
        label21.Size = new Size(25, 15);
        label21.TabIndex = 52;
        label21.Text = "мм";
        // 
        // label23
        // 
        label23.AutoSize = true;
        label23.Location = new Point(275, 455);
        label23.Margin = new Padding(4, 0, 4, 0);
        label23.Name = "label23";
        label23.Size = new Size(25, 15);
        label23.TabIndex = 54;
        label23.Text = "мм";
        // 
        // label24
        // 
        label24.AutoSize = true;
        label24.Location = new Point(275, 426);
        label24.Margin = new Padding(4, 0, 4, 0);
        label24.Name = "label24";
        label24.Size = new Size(25, 15);
        label24.TabIndex = 55;
        label24.Text = "мм";
        // 
        // label25
        // 
        label25.AutoSize = true;
        label25.Location = new Point(275, 397);
        label25.Margin = new Padding(4, 0, 4, 0);
        label25.Name = "label25";
        label25.Size = new Size(25, 15);
        label25.TabIndex = 56;
        label25.Text = "мм";
        // 
        // label26
        // 
        label26.AutoSize = true;
        label26.Location = new Point(275, 368);
        label26.Margin = new Padding(4, 0, 4, 0);
        label26.Name = "label26";
        label26.Size = new Size(25, 15);
        label26.TabIndex = 57;
        label26.Text = "мм";
        // 
        // label27
        // 
        label27.AutoSize = true;
        label27.Location = new Point(275, 483);
        label27.Margin = new Padding(4, 0, 4, 0);
        label27.Name = "label27";
        label27.Size = new Size(25, 15);
        label27.TabIndex = 58;
        label27.Text = "мм";
        // 
        // label28
        // 
        label28.AutoSize = true;
        label28.BorderStyle = BorderStyle.Fixed3D;
        label28.Location = new Point(-4, 476);
        label28.Margin = new Padding(4, 0, 4, 0);
        label28.MaximumSize = new Size(0, 1);
        label28.MinimumSize = new Size(350, 0);
        label28.Name = "label28";
        label28.Size = new Size(350, 1);
        label28.TabIndex = 59;
        label28.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // M_gb
        // 
        M_gb.Controls.Add(label29);
        M_gb.Controls.Add(M_tb);
        M_gb.Enabled = false;
        M_gb.Location = new Point(398, 492);
        M_gb.Name = "M_gb";
        M_gb.Size = new Size(299, 54);
        M_gb.TabIndex = 60;
        M_gb.TabStop = false;
        M_gb.Text = "Расчетный изгибающий момент, М";
        // 
        // label29
        // 
        label29.AutoSize = true;
        label29.Location = new Point(115, 27);
        label29.Name = "label29";
        label29.Size = new Size(37, 15);
        label29.TabIndex = 1;
        label29.Text = "Н мм";
        // 
        // M_tb
        // 
        M_tb.Location = new Point(8, 23);
        M_tb.Name = "M_tb";
        M_tb.Size = new Size(100, 23);
        M_tb.TabIndex = 0;
        M_tb.Text = "0";
        M_tb.TextChanged += DisabledCalculateBtn;
        // 
        // Q_gb
        // 
        Q_gb.Controls.Add(label30);
        Q_gb.Controls.Add(Q_tb);
        Q_gb.Enabled = false;
        Q_gb.Location = new Point(398, 552);
        Q_gb.Name = "Q_gb";
        Q_gb.Size = new Size(299, 54);
        Q_gb.TabIndex = 61;
        Q_gb.TabStop = false;
        Q_gb.Text = "Расчетное поперечное усилие, Q";
        // 
        // label30
        // 
        label30.AutoSize = true;
        label30.Location = new Point(115, 26);
        label30.Name = "label30";
        label30.Size = new Size(16, 15);
        label30.TabIndex = 3;
        label30.Text = "Н";
        // 
        // Q_tb
        // 
        Q_tb.Location = new Point(8, 22);
        Q_tb.Name = "Q_tb";
        Q_tb.Size = new Size(100, 23);
        Q_tb.TabIndex = 2;
        Q_tb.Text = "0";
        Q_tb.TextChanged += DisabledCalculateBtn;
        // 
        // isNozzleCalculateCheckBox
        // 
        isNozzleCalculateCheckBox.AutoSize = true;
        isNozzleCalculateCheckBox.Location = new Point(12, 682);
        isNozzleCalculateCheckBox.Name = "isNozzleCalculateCheckBox";
        isNozzleCalculateCheckBox.Size = new Size(190, 19);
        isNozzleCalculateCheckBox.TabIndex = 64;
        isNozzleCalculateCheckBox.Text = "Расчитать штуцер в обечайке";
        isNozzleCalculateCheckBox.UseVisualStyleBackColor = true;
        // 
        // loadingConditionControl
        // 
        loadingConditionControl.Location = new Point(10, 156);
        loadingConditionControl.Name = "loadingConditionControl";
        loadingConditionControl.Size = new Size(354, 144);
        loadingConditionControl.TabIndex = 71;
        loadingConditionControl.TabStop = false;
        // 
        // loadingConditionsControl
        // 
        loadingConditionsControl.Location = new Point(10, 561);
        loadingConditionsControl.MaximumSize = new Size(380, 115);
        loadingConditionsControl.MinimumSize = new Size(380, 115);
        loadingConditionsControl.Name = "loadingConditionsControl";
        loadingConditionsControl.Size = new Size(380, 115);
        loadingConditionsControl.TabIndex = 72;
        // 
        // steelControl
        // 
        steelControl.Location = new Point(98, 70);
        steelControl.MaximumSize = new Size(265, 80);
        steelControl.MinimumSize = new Size(265, 80);
        steelControl.Name = "steelControl";
        steelControl.Size = new Size(265, 80);
        steelControl.TabIndex = 73;
        // 
        // CylindricalShellForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancel_b;
        ClientSize = new Size(709, 761);
        Controls.Add(steelControl);
        Controls.Add(loadingConditionsControl);
        Controls.Add(loadingConditionControl);
        Controls.Add(isNozzleCalculateCheckBox);
        Controls.Add(Q_gb);
        Controls.Add(M_gb);
        Controls.Add(label28);
        Controls.Add(label27);
        Controls.Add(label26);
        Controls.Add(label25);
        Controls.Add(label24);
        Controls.Add(label23);
        Controls.Add(label21);
        Controls.Add(cancel_b);
        Controls.Add(calculate_btn);
        Controls.Add(predCalc_b);
        Controls.Add(groupBox4);
        Controls.Add(force_gb);
        Controls.Add(groupBox2);
        Controls.Add(button4);
        Controls.Add(button3);
        Controls.Add(defect_btn);
        Controls.Add(defect_chb);
        Controls.Add(getL_b);
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
        Controls.Add(l_tb);
        Controls.Add(D_tb);
        Controls.Add(fi_tb);
        Controls.Add(shell_pb);
        Controls.Add(Gost_cb);
        Controls.Add(label2);
        Controls.Add(button1);
        Controls.Add(Name_tb);
        Controls.Add(label1);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Margin = new Padding(4, 3, 4, 3);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CylindricalShellForm";
        Text = "Cylindrical shell";
        FormClosing += CylindricalShellForm_FormClosing;
        Load += CylindricalShellForm_Load;
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        force_gb.ResumeLayout(false);
        force_gb.PerformLayout();
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)f_pb).EndInit();
        fq_panel.ResumeLayout(false);
        fq_panel.PerformLayout();
        groupBox4.ResumeLayout(false);
        groupBox4.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)shell_pb).EndInit();
        M_gb.ResumeLayout(false);
        M_gb.PerformLayout();
        Q_gb.ResumeLayout(false);
        Q_gb.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private TextBox Name_tb;
    private Button button1;
    private Label label2;
    private ComboBox Gost_cb;
    private PictureBox shell_pb;
    internal TextBox fi_tb;
    private TextBox D_tb;
    private TextBox l_tb;
    private TextBox c1_tb;
    private TextBox c2_tb;
    private TextBox c3_tb;
    private Label label10;
    private Label label11;
    private Label label12;
    private Label label13;
    private Label label14;
    private Label label15;
    private Label label17;
    private TextBox s_tb;
    private Button getFi_b;
    private Button getL_b;
    private CheckBox defect_chb;
    private Button defect_btn;
    private Button button3;
    private Button button4;
    private GroupBox groupBox2;
    private RadioButton stressHand_rb;
    private RadioButton stressCalc_rb;
    private GroupBox force_gb;
    private RadioButton rb1;
    private PictureBox f_pb;
    private Label label20;
    private RadioButton rb7;
    private RadioButton rb6;
    private RadioButton rb5;
    private RadioButton rb4;
    private RadioButton rb3;
    private RadioButton rb2;
    private GroupBox groupBox4;
    private Label p_d_l;
    private Label scalc_l;
    private Button predCalc_b;
    private Button calculate_btn;
    private Button cancel_b;
    private Label label21;
    private Label label23;
    private Label label24;
    private Label label25;
    private Label label26;
    private Label label27;
    private Label label28;
    private Panel panel1;
    private RadioButton forceCompress_rb;
    private RadioButton forceStretch_rb;
    private TextBox F_tb;
    private GroupBox M_gb;
    private Label label29;
    private TextBox M_tb;
    private GroupBox Q_gb;
    private Label label30;
    private TextBox Q_tb;
    private Panel fq_panel;
    private Label fq_mes_l;
    private Label fq_l;
    private TextBox fq_tb;
    private CheckBox isNozzleCalculateCheckBox;
    private LoadingConditionControl loadingConditionControl;
    private LoadingConditionsControl loadingConditionsControl;
    private SteelControl steelControl;
}