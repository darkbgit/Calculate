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
        label1 = new System.Windows.Forms.Label();
        Name_tb = new System.Windows.Forms.TextBox();
        button1 = new System.Windows.Forms.Button();
        label2 = new System.Windows.Forms.Label();
        label3 = new System.Windows.Forms.Label();
        Gost_cb = new System.Windows.Forms.ComboBox();
        t_tb = new System.Windows.Forms.TextBox();
        label4 = new System.Windows.Forms.Label();
        label5 = new System.Windows.Forms.Label();
        vn_rb = new System.Windows.Forms.RadioButton();
        nar_rb = new System.Windows.Forms.RadioButton();
        p_tb = new System.Windows.Forms.TextBox();
        dav_gb = new System.Windows.Forms.GroupBox();
        label6 = new System.Windows.Forms.Label();
        label7 = new System.Windows.Forms.Label();
        steel_cb = new System.Windows.Forms.ComboBox();
        label8 = new System.Windows.Forms.Label();
        sigma_d_tb = new System.Windows.Forms.TextBox();
        E_tb = new System.Windows.Forms.TextBox();
        fi_tb = new System.Windows.Forms.TextBox();
        D_tb = new System.Windows.Forms.TextBox();
        l_tb = new System.Windows.Forms.TextBox();
        c1_tb = new System.Windows.Forms.TextBox();
        c2_tb = new System.Windows.Forms.TextBox();
        c3_tb = new System.Windows.Forms.TextBox();
        label9 = new System.Windows.Forms.Label();
        label10 = new System.Windows.Forms.Label();
        label11 = new System.Windows.Forms.Label();
        label12 = new System.Windows.Forms.Label();
        label13 = new System.Windows.Forms.Label();
        label14 = new System.Windows.Forms.Label();
        label15 = new System.Windows.Forms.Label();
        label17 = new System.Windows.Forms.Label();
        s_tb = new System.Windows.Forms.TextBox();
        label18 = new System.Windows.Forms.Label();
        grtSigma_b = new System.Windows.Forms.Button();
        label19 = new System.Windows.Forms.Label();
        getE_b = new System.Windows.Forms.Button();
        getFi_b = new System.Windows.Forms.Button();
        getL_b = new System.Windows.Forms.Button();
        defect_chb = new System.Windows.Forms.CheckBox();
        defect_b = new System.Windows.Forms.Button();
        button3 = new System.Windows.Forms.Button();
        button4 = new System.Windows.Forms.Button();
        groupBox2 = new System.Windows.Forms.GroupBox();
        stressHand_rb = new System.Windows.Forms.RadioButton();
        stressCalc_rb = new System.Windows.Forms.RadioButton();
        force_gb = new System.Windows.Forms.GroupBox();
        F_tb = new System.Windows.Forms.TextBox();
        panel1 = new System.Windows.Forms.Panel();
        forceCompress_rb = new System.Windows.Forms.RadioButton();
        forceStretch_rb = new System.Windows.Forms.RadioButton();
        rb7 = new System.Windows.Forms.RadioButton();
        rb6 = new System.Windows.Forms.RadioButton();
        rb5 = new System.Windows.Forms.RadioButton();
        rb4 = new System.Windows.Forms.RadioButton();
        rb3 = new System.Windows.Forms.RadioButton();
        rb2 = new System.Windows.Forms.RadioButton();
        rb1 = new System.Windows.Forms.RadioButton();
        f_pb = new System.Windows.Forms.PictureBox();
        label20 = new System.Windows.Forms.Label();
        fq_panel = new System.Windows.Forms.Panel();
        fq_mes_l = new System.Windows.Forms.Label();
        fq_l = new System.Windows.Forms.Label();
        fq_tb = new System.Windows.Forms.TextBox();
        groupBox4 = new System.Windows.Forms.GroupBox();
        p_d_l = new System.Windows.Forms.Label();
        scalc_l = new System.Windows.Forms.Label();
        predCalc_b = new System.Windows.Forms.Button();
        calc_b = new System.Windows.Forms.Button();
        cancel_b = new System.Windows.Forms.Button();
        shell_pb = new System.Windows.Forms.PictureBox();
        label21 = new System.Windows.Forms.Label();
        label23 = new System.Windows.Forms.Label();
        label24 = new System.Windows.Forms.Label();
        label25 = new System.Windows.Forms.Label();
        label26 = new System.Windows.Forms.Label();
        label27 = new System.Windows.Forms.Label();
        label28 = new System.Windows.Forms.Label();
        M_gb = new System.Windows.Forms.GroupBox();
        label29 = new System.Windows.Forms.Label();
        M_tb = new System.Windows.Forms.TextBox();
        Q_gb = new System.Windows.Forms.GroupBox();
        label30 = new System.Windows.Forms.Label();
        Q_tb = new System.Windows.Forms.TextBox();
        sigmaHandle_cb = new System.Windows.Forms.CheckBox();
        EHandle_cb = new System.Windows.Forms.CheckBox();
        isNozzleCalculateCheckBox = new System.Windows.Forms.CheckBox();
        dav_gb.SuspendLayout();
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
        label1.Location = new System.Drawing.Point(101, 15);
        label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(117, 15);
        label1.TabIndex = 0;
        label1.Text = "Название элемента:";
        // 
        // Name_tb
        // 
        Name_tb.Location = new System.Drawing.Point(224, 12);
        Name_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        Name_tb.Name = "Name_tb";
        Name_tb.Size = new System.Drawing.Size(139, 23);
        Name_tb.TabIndex = 1;
        Name_tb.TextChanged += DisabledCalculateBtn;
        // 
        // button1
        // 
        button1.Location = new System.Drawing.Point(371, 9);
        button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(284, 27);
        button1.TabIndex = 2;
        button1.Text = "Расчетная схема цилиндрической обечайки";
        button1.UseVisualStyleBackColor = true;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(74, 44);
        label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(144, 15);
        label2.TabIndex = 3;
        label2.Text = "Нормативный документ:";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new System.Drawing.Point(69, 73);
        label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(149, 15);
        label3.TabIndex = 4;
        label3.Text = "Расчетная температура, t:";
        // 
        // Gost_cb
        // 
        Gost_cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        Gost_cb.FormattingEnabled = true;
        Gost_cb.Location = new System.Drawing.Point(224, 41);
        Gost_cb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        Gost_cb.Name = "Gost_cb";
        Gost_cb.Size = new System.Drawing.Size(139, 23);
        Gost_cb.TabIndex = 2;
        Gost_cb.Tag = "";
        Gost_cb.TextChanged += DisabledCalculateBtn;
        // 
        // t_tb
        // 
        t_tb.Location = new System.Drawing.Point(224, 70);
        t_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        t_tb.Name = "t_tb";
        t_tb.Size = new System.Drawing.Size(46, 23);
        t_tb.TabIndex = 3;
        t_tb.Text = "20";
        t_tb.TextChanged += DisabledCalculateBtn;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new System.Drawing.Point(274, 73);
        label4.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(20, 15);
        label4.TabIndex = 8;
        label4.Text = "°C";
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Location = new System.Drawing.Point(70, 19);
        label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label5.Name = "label5";
        label5.Size = new System.Drawing.Size(134, 15);
        label5.TabIndex = 9;
        label5.Text = "Расчетное давление, p:";
        // 
        // vn_rb
        // 
        vn_rb.AutoSize = true;
        vn_rb.Checked = true;
        vn_rb.Location = new System.Drawing.Point(79, 44);
        vn_rb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        vn_rb.Name = "vn_rb";
        vn_rb.Size = new System.Drawing.Size(88, 19);
        vn_rb.TabIndex = 5;
        vn_rb.TabStop = true;
        vn_rb.Text = "внутреннее";
        vn_rb.UseVisualStyleBackColor = true;
        vn_rb.CheckedChanged += Pressure_rb;
        // 
        // nar_rb
        // 
        nar_rb.AutoSize = true;
        nar_rb.Location = new System.Drawing.Point(183, 45);
        nar_rb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        nar_rb.Name = "nar_rb";
        nar_rb.Size = new System.Drawing.Size(80, 19);
        nar_rb.TabIndex = 6;
        nar_rb.Text = "наружное";
        nar_rb.UseVisualStyleBackColor = true;
        nar_rb.CheckedChanged += Pressure_rb;
        // 
        // p_tb
        // 
        p_tb.Location = new System.Drawing.Point(210, 15);
        p_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        p_tb.Name = "p_tb";
        p_tb.Size = new System.Drawing.Size(46, 23);
        p_tb.TabIndex = 4;
        p_tb.Text = "3.1";
        p_tb.TextChanged += DisabledCalculateBtn;
        // 
        // dav_gb
        // 
        dav_gb.Controls.Add(label6);
        dav_gb.Controls.Add(label5);
        dav_gb.Controls.Add(nar_rb);
        dav_gb.Controls.Add(p_tb);
        dav_gb.Controls.Add(vn_rb);
        dav_gb.Location = new System.Drawing.Point(14, 99);
        dav_gb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        dav_gb.Name = "dav_gb";
        dav_gb.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
        dav_gb.Size = new System.Drawing.Size(350, 77);
        dav_gb.TabIndex = 4;
        dav_gb.TabStop = false;
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Location = new System.Drawing.Point(264, 18);
        label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label6.Name = "label6";
        label6.Size = new System.Drawing.Size(33, 15);
        label6.TabIndex = 13;
        label6.Text = "МПа";
        // 
        // label7
        // 
        label7.AutoSize = true;
        label7.Location = new System.Drawing.Point(110, 185);
        label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label7.Name = "label7";
        label7.Size = new System.Drawing.Size(108, 15);
        label7.TabIndex = 14;
        label7.Text = "Марка материала:";
        // 
        // steel_cb
        // 
        steel_cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        steel_cb.FormattingEnabled = true;
        steel_cb.Location = new System.Drawing.Point(224, 182);
        steel_cb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        steel_cb.Name = "steel_cb";
        steel_cb.Size = new System.Drawing.Size(139, 23);
        steel_cb.TabIndex = 7;
        steel_cb.TextChanged += DisabledCalculateBtn;
        // 
        // label8
        // 
        label8.AutoSize = true;
        label8.Location = new System.Drawing.Point(30, 216);
        label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label8.Name = "label8";
        label8.Size = new System.Drawing.Size(102, 30);
        label8.TabIndex = 16;
        label8.Text = "Допускаемое\r\n напряжение, [σ]:";
        label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
        // 
        // sigma_d_tb
        // 
        sigma_d_tb.Enabled = false;
        sigma_d_tb.Location = new System.Drawing.Point(140, 220);
        sigma_d_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        sigma_d_tb.Name = "sigma_d_tb";
        sigma_d_tb.Size = new System.Drawing.Size(46, 23);
        sigma_d_tb.TabIndex = 17;
        sigma_d_tb.TextChanged += DisabledCalculateBtn;
        // 
        // E_tb
        // 
        E_tb.Enabled = false;
        E_tb.Location = new System.Drawing.Point(140, 258);
        E_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        E_tb.Name = "E_tb";
        E_tb.Size = new System.Drawing.Size(46, 23);
        E_tb.TabIndex = 18;
        E_tb.TextChanged += DisabledCalculateBtn;
        // 
        // fi_tb
        // 
        fi_tb.Location = new System.Drawing.Point(224, 292);
        fi_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        fi_tb.Name = "fi_tb";
        fi_tb.Size = new System.Drawing.Size(46, 23);
        fi_tb.TabIndex = 8;
        fi_tb.Text = "1";
        fi_tb.TextChanged += DisabledCalculateBtn;
        // 
        // D_tb
        // 
        D_tb.Location = new System.Drawing.Point(224, 321);
        D_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        D_tb.Name = "D_tb";
        D_tb.Size = new System.Drawing.Size(46, 23);
        D_tb.TabIndex = 9;
        D_tb.Text = "1000";
        D_tb.TextChanged += DisabledCalculateBtn;
        // 
        // l_tb
        // 
        l_tb.Enabled = false;
        l_tb.Location = new System.Drawing.Point(224, 350);
        l_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        l_tb.Name = "l_tb";
        l_tb.Size = new System.Drawing.Size(46, 23);
        l_tb.TabIndex = 10;
        l_tb.TextChanged += DisabledCalculateBtn;
        // 
        // c1_tb
        // 
        c1_tb.Location = new System.Drawing.Point(224, 379);
        c1_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        c1_tb.Name = "c1_tb";
        c1_tb.Size = new System.Drawing.Size(46, 23);
        c1_tb.TabIndex = 11;
        c1_tb.Text = "2";
        c1_tb.TextChanged += DisabledCalculateBtn;
        // 
        // c2_tb
        // 
        c2_tb.Location = new System.Drawing.Point(224, 408);
        c2_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        c2_tb.Name = "c2_tb";
        c2_tb.Size = new System.Drawing.Size(46, 23);
        c2_tb.TabIndex = 12;
        c2_tb.Text = "0.8";
        c2_tb.TextChanged += DisabledCalculateBtn;
        // 
        // c3_tb
        // 
        c3_tb.Location = new System.Drawing.Point(224, 437);
        c3_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        c3_tb.Name = "c3_tb";
        c3_tb.Size = new System.Drawing.Size(46, 23);
        c3_tb.TabIndex = 13;
        c3_tb.TextChanged += DisabledCalculateBtn;
        // 
        // label9
        // 
        label9.AutoSize = true;
        label9.Location = new System.Drawing.Point(11, 254);
        label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label9.Name = "label9";
        label9.Size = new System.Drawing.Size(121, 30);
        label9.TabIndex = 26;
        label9.Text = "Модуль продольной\r\n упругости, E:";
        label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
        // 
        // label10
        // 
        label10.AutoSize = true;
        label10.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
        label10.Location = new System.Drawing.Point(26, 296);
        label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label10.Name = "label10";
        label10.Size = new System.Drawing.Size(192, 15);
        label10.TabIndex = 27;
        label10.Text = "К-т прочности сварного шва, φp:";
        // 
        // label11
        // 
        label11.AutoSize = true;
        label11.Location = new System.Drawing.Point(23, 324);
        label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label11.Name = "label11";
        label11.Size = new System.Drawing.Size(195, 15);
        label11.TabIndex = 28;
        label11.Text = "Внутренний диаметр обечайки, D:";
        // 
        // label12
        // 
        label12.AutoSize = true;
        label12.Location = new System.Drawing.Point(51, 354);
        label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label12.Name = "label12";
        label12.Size = new System.Drawing.Size(167, 15);
        label12.TabIndex = 29;
        label12.Text = "Расчетная длина обечайки, l:";
        // 
        // label13
        // 
        label13.AutoSize = true;
        label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        label13.Location = new System.Drawing.Point(72, 383);
        label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label13.Name = "label13";
        label13.Size = new System.Drawing.Size(146, 13);
        label13.TabIndex = 30;
        label13.Text = "Прибавка на коррозию, c1:";
        // 
        // label14
        // 
        label14.AutoSize = true;
        label14.Location = new System.Drawing.Point(85, 412);
        label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label14.Name = "label14";
        label14.Size = new System.Drawing.Size(133, 15);
        label14.TabIndex = 31;
        label14.Text = "Минусовой допуск, c2:";
        // 
        // label15
        // 
        label15.AutoSize = true;
        label15.Location = new System.Drawing.Point(40, 441);
        label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label15.Name = "label15";
        label15.Size = new System.Drawing.Size(178, 15);
        label15.TabIndex = 32;
        label15.Text = "Технологическая прибавка, c3:";
        // 
        // label17
        // 
        label17.AutoSize = true;
        label17.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        label17.Location = new System.Drawing.Point(87, 469);
        label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label17.Name = "label17";
        label17.Size = new System.Drawing.Size(131, 15);
        label17.TabIndex = 34;
        label17.Text = "Принятая толщина, s:";
        // 
        // s_tb
        // 
        s_tb.Location = new System.Drawing.Point(224, 466);
        s_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        s_tb.Name = "s_tb";
        s_tb.Size = new System.Drawing.Size(46, 23);
        s_tb.TabIndex = 15;
        s_tb.Text = "14";
        // 
        // label18
        // 
        label18.AutoSize = true;
        label18.Location = new System.Drawing.Point(194, 224);
        label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label18.Name = "label18";
        label18.Size = new System.Drawing.Size(33, 15);
        label18.TabIndex = 36;
        label18.Text = "МПа";
        // 
        // grtSigma_b
        // 
        grtSigma_b.Location = new System.Drawing.Point(316, 220);
        grtSigma_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        grtSigma_b.Name = "grtSigma_b";
        grtSigma_b.Size = new System.Drawing.Size(43, 23);
        grtSigma_b.TabIndex = 37;
        grtSigma_b.Text = ">>";
        grtSigma_b.UseVisualStyleBackColor = true;
        // 
        // label19
        // 
        label19.AutoSize = true;
        label19.Location = new System.Drawing.Point(194, 262);
        label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label19.Name = "label19";
        label19.Size = new System.Drawing.Size(33, 15);
        label19.TabIndex = 38;
        label19.Text = "МПа";
        // 
        // getE_b
        // 
        getE_b.Enabled = false;
        getE_b.Location = new System.Drawing.Point(316, 258);
        getE_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        getE_b.Name = "getE_b";
        getE_b.Size = new System.Drawing.Size(43, 23);
        getE_b.TabIndex = 39;
        getE_b.Text = ">>";
        getE_b.UseVisualStyleBackColor = true;
        getE_b.Click += GetE_b_Click;
        // 
        // getFi_b
        // 
        getFi_b.Location = new System.Drawing.Point(316, 292);
        getFi_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        getFi_b.Name = "getFi_b";
        getFi_b.Size = new System.Drawing.Size(43, 23);
        getFi_b.TabIndex = 40;
        getFi_b.Text = ">>";
        getFi_b.UseVisualStyleBackColor = true;
        getFi_b.Click += GetFi_b_Click;
        // 
        // getL_b
        // 
        getL_b.Enabled = false;
        getL_b.Location = new System.Drawing.Point(316, 350);
        getL_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        getL_b.Name = "getL_b";
        getL_b.Size = new System.Drawing.Size(43, 23);
        getL_b.TabIndex = 41;
        getL_b.Text = ">>";
        getL_b.UseVisualStyleBackColor = true;
        // 
        // defect_chb
        // 
        defect_chb.AutoSize = true;
        defect_chb.Location = new System.Drawing.Point(13, 537);
        defect_chb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        defect_chb.Name = "defect_chb";
        defect_chb.Size = new System.Drawing.Size(201, 19);
        defect_chb.TabIndex = 42;
        defect_chb.Text = "Дефекты по ГОСТ 34233.11-2017";
        defect_chb.UseVisualStyleBackColor = true;
        defect_chb.CheckedChanged += Defect_chb_CheckedChanged;
        // 
        // defect_b
        // 
        defect_b.Enabled = false;
        defect_b.Location = new System.Drawing.Point(247, 532);
        defect_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        defect_b.Name = "defect_b";
        defect_b.Size = new System.Drawing.Size(35, 27);
        defect_b.TabIndex = 43;
        defect_b.Text = ">>";
        defect_b.UseVisualStyleBackColor = true;
        // 
        // button3
        // 
        button3.Enabled = false;
        button3.Location = new System.Drawing.Point(13, 565);
        button3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        button3.Name = "button3";
        button3.Size = new System.Drawing.Size(168, 27);
        button3.TabIndex = 44;
        button3.Text = "Изоляция и футеровка >>";
        button3.UseVisualStyleBackColor = true;
        // 
        // button4
        // 
        button4.Enabled = false;
        button4.Location = new System.Drawing.Point(189, 565);
        button4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        button4.Name = "button4";
        button4.Size = new System.Drawing.Size(175, 27);
        button4.TabIndex = 45;
        button4.Text = "Малоцикловая прочность >>";
        button4.UseVisualStyleBackColor = true;
        // 
        // groupBox2
        // 
        groupBox2.Controls.Add(stressHand_rb);
        groupBox2.Controls.Add(stressCalc_rb);
        groupBox2.Location = new System.Drawing.Point(371, 249);
        groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        groupBox2.Name = "groupBox2";
        groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
        groupBox2.Size = new System.Drawing.Size(326, 52);
        groupBox2.TabIndex = 46;
        groupBox2.TabStop = false;
        groupBox2.Text = "Нагрузки";
        // 
        // stressHand_rb
        // 
        stressHand_rb.AutoSize = true;
        stressHand_rb.Location = new System.Drawing.Point(191, 23);
        stressHand_rb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        stressHand_rb.Name = "stressHand_rb";
        stressHand_rb.Size = new System.Drawing.Size(125, 19);
        stressHand_rb.TabIndex = 1;
        stressHand_rb.Text = "Задавать вручную";
        stressHand_rb.UseVisualStyleBackColor = true;
        stressHand_rb.CheckedChanged += Stress_rb_CheckedChanged;
        // 
        // stressCalc_rb
        // 
        stressCalc_rb.AutoSize = true;
        stressCalc_rb.Checked = true;
        stressCalc_rb.Location = new System.Drawing.Point(8, 23);
        stressCalc_rb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        stressCalc_rb.Name = "stressCalc_rb";
        stressCalc_rb.Size = new System.Drawing.Size(160, 19);
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
        force_gb.Location = new System.Drawing.Point(371, 307);
        force_gb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        force_gb.Name = "force_gb";
        force_gb.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
        force_gb.Size = new System.Drawing.Size(326, 179);
        force_gb.TabIndex = 47;
        force_gb.TabStop = false;
        force_gb.Text = "Расчетное осевое усилие, F";
        // 
        // F_tb
        // 
        F_tb.Location = new System.Drawing.Point(8, 147);
        F_tb.Name = "F_tb";
        F_tb.Size = new System.Drawing.Size(100, 23);
        F_tb.TabIndex = 10;
        F_tb.Text = "0";
        F_tb.TextChanged += DisabledCalculateBtn;
        // 
        // panel1
        // 
        panel1.Controls.Add(forceCompress_rb);
        panel1.Controls.Add(forceStretch_rb);
        panel1.Location = new System.Drawing.Point(8, 22);
        panel1.Name = "panel1";
        panel1.Size = new System.Drawing.Size(308, 25);
        panel1.TabIndex = 9;
        // 
        // forceCompress_rb
        // 
        forceCompress_rb.AutoSize = true;
        forceCompress_rb.Location = new System.Drawing.Point(138, 2);
        forceCompress_rb.Name = "forceCompress_rb";
        forceCompress_rb.Size = new System.Drawing.Size(98, 19);
        forceCompress_rb.TabIndex = 1;
        forceCompress_rb.Text = "Сжимающие";
        forceCompress_rb.UseVisualStyleBackColor = true;
        forceCompress_rb.CheckedChanged += ForceStretchCompress_rb_CheckedChanged;
        // 
        // forceStretch_rb
        // 
        forceStretch_rb.AutoSize = true;
        forceStretch_rb.Checked = true;
        forceStretch_rb.Location = new System.Drawing.Point(19, 2);
        forceStretch_rb.Name = "forceStretch_rb";
        forceStretch_rb.Size = new System.Drawing.Size(113, 19);
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
        rb7.Location = new System.Drawing.Point(292, 49);
        rb7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        rb7.Name = "rb7";
        rb7.Size = new System.Drawing.Size(31, 19);
        rb7.TabIndex = 8;
        rb7.Text = "7";
        rb7.UseVisualStyleBackColor = true;
        rb7.CheckedChanged += Force_rb_CheckedChanged;
        // 
        // rb6
        // 
        rb6.AutoSize = true;
        rb6.Enabled = false;
        rb6.Location = new System.Drawing.Point(254, 104);
        rb6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        rb6.Name = "rb6";
        rb6.Size = new System.Drawing.Size(31, 19);
        rb6.TabIndex = 7;
        rb6.Text = "6";
        rb6.UseVisualStyleBackColor = true;
        rb6.CheckedChanged += Force_rb_CheckedChanged;
        // 
        // rb5
        // 
        rb5.AutoSize = true;
        rb5.Enabled = false;
        rb5.Location = new System.Drawing.Point(253, 76);
        rb5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        rb5.Name = "rb5";
        rb5.Size = new System.Drawing.Size(31, 19);
        rb5.TabIndex = 6;
        rb5.Text = "5";
        rb5.UseVisualStyleBackColor = true;
        rb5.CheckedChanged += Force_rb_CheckedChanged;
        // 
        // rb4
        // 
        rb4.AutoSize = true;
        rb4.Enabled = false;
        rb4.Location = new System.Drawing.Point(253, 49);
        rb4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        rb4.Name = "rb4";
        rb4.Size = new System.Drawing.Size(31, 19);
        rb4.TabIndex = 5;
        rb4.Text = "4";
        rb4.UseVisualStyleBackColor = true;
        rb4.CheckedChanged += Force_rb_CheckedChanged;
        // 
        // rb3
        // 
        rb3.AutoSize = true;
        rb3.Enabled = false;
        rb3.Location = new System.Drawing.Point(215, 104);
        rb3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        rb3.Name = "rb3";
        rb3.Size = new System.Drawing.Size(31, 19);
        rb3.TabIndex = 4;
        rb3.Text = "3";
        rb3.UseVisualStyleBackColor = true;
        rb3.CheckedChanged += Force_rb_CheckedChanged;
        // 
        // rb2
        // 
        rb2.AutoSize = true;
        rb2.Enabled = false;
        rb2.Location = new System.Drawing.Point(215, 76);
        rb2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        rb2.Name = "rb2";
        rb2.Size = new System.Drawing.Size(31, 19);
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
        rb1.Location = new System.Drawing.Point(214, 49);
        rb1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        rb1.Name = "rb1";
        rb1.Size = new System.Drawing.Size(31, 19);
        rb1.TabIndex = 2;
        rb1.TabStop = true;
        rb1.Text = "1";
        rb1.UseVisualStyleBackColor = true;
        rb1.CheckedChanged += Force_rb_CheckedChanged;
        // 
        // f_pb
        // 
        f_pb.InitialImage = null;
        f_pb.Location = new System.Drawing.Point(8, 49);
        f_pb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        f_pb.Name = "f_pb";
        f_pb.Size = new System.Drawing.Size(198, 92);
        f_pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        f_pb.TabIndex = 1;
        f_pb.TabStop = false;
        // 
        // label20
        // 
        label20.AutoSize = true;
        label20.Location = new System.Drawing.Point(115, 150);
        label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label20.Name = "label20";
        label20.Size = new System.Drawing.Size(16, 15);
        label20.TabIndex = 0;
        label20.Text = "H";
        // 
        // fq_panel
        // 
        fq_panel.Controls.Add(fq_mes_l);
        fq_panel.Controls.Add(fq_l);
        fq_panel.Controls.Add(fq_tb);
        fq_panel.Location = new System.Drawing.Point(138, 144);
        fq_panel.Name = "fq_panel";
        fq_panel.Size = new System.Drawing.Size(177, 30);
        fq_panel.TabIndex = 11;
        fq_panel.Visible = false;
        // 
        // fq_mes_l
        // 
        fq_mes_l.AutoSize = true;
        fq_mes_l.Location = new System.Drawing.Point(146, 8);
        fq_mes_l.Name = "fq_mes_l";
        fq_mes_l.Size = new System.Drawing.Size(0, 15);
        fq_mes_l.TabIndex = 2;
        // 
        // fq_l
        // 
        fq_l.AutoSize = true;
        fq_l.Location = new System.Drawing.Point(22, 8);
        fq_l.Name = "fq_l";
        fq_l.Size = new System.Drawing.Size(11, 15);
        fq_l.TabIndex = 1;
        fq_l.Text = "f";
        // 
        // fq_tb
        // 
        fq_tb.Location = new System.Drawing.Point(39, 3);
        fq_tb.Name = "fq_tb";
        fq_tb.Size = new System.Drawing.Size(100, 23);
        fq_tb.TabIndex = 0;
        fq_tb.TextChanged += DisabledCalculateBtn;
        // 
        // groupBox4
        // 
        groupBox4.Controls.Add(p_d_l);
        groupBox4.Controls.Add(scalc_l);
        groupBox4.Location = new System.Drawing.Point(371, 612);
        groupBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        groupBox4.Name = "groupBox4";
        groupBox4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
        groupBox4.Size = new System.Drawing.Size(270, 45);
        groupBox4.TabIndex = 48;
        groupBox4.TabStop = false;
        groupBox4.Text = "Результаты расчета";
        // 
        // p_d_l
        // 
        p_d_l.AutoSize = true;
        p_d_l.Location = new System.Drawing.Point(125, 18);
        p_d_l.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        p_d_l.Name = "p_d_l";
        p_d_l.Size = new System.Drawing.Size(22, 15);
        p_d_l.TabIndex = 1;
        p_d_l.Text = "[p]";
        // 
        // scalc_l
        // 
        scalc_l.AutoSize = true;
        scalc_l.Location = new System.Drawing.Point(7, 18);
        scalc_l.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        scalc_l.Name = "scalc_l";
        scalc_l.Size = new System.Drawing.Size(12, 15);
        scalc_l.TabIndex = 0;
        scalc_l.Text = "s";
        // 
        // predCalc_b
        // 
        predCalc_b.Location = new System.Drawing.Point(13, 612);
        predCalc_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        predCalc_b.Name = "predCalc_b";
        predCalc_b.Size = new System.Drawing.Size(130, 42);
        predCalc_b.TabIndex = 16;
        predCalc_b.Text = "Предварительный\r\nрасчет";
        predCalc_b.UseVisualStyleBackColor = true;
        predCalc_b.Click += PreCalc_b_Click;
        // 
        // calc_b
        // 
        calc_b.Enabled = false;
        calc_b.Location = new System.Drawing.Point(151, 625);
        calc_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        calc_b.Name = "calc_b";
        calc_b.Size = new System.Drawing.Size(88, 27);
        calc_b.TabIndex = 17;
        calc_b.Text = "Расчет";
        calc_b.UseVisualStyleBackColor = true;
        calc_b.Click += Calc_b_Click;
        // 
        // cancel_b
        // 
        cancel_b.Location = new System.Drawing.Point(246, 624);
        cancel_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        cancel_b.Name = "cancel_b";
        cancel_b.Size = new System.Drawing.Size(88, 27);
        cancel_b.TabIndex = 18;
        cancel_b.Text = "Cancel";
        cancel_b.UseVisualStyleBackColor = true;
        cancel_b.Click += Cancel_b_Click;
        // 
        // shell_pb
        // 
        shell_pb.InitialImage = null;
        shell_pb.Location = new System.Drawing.Point(371, 43);
        shell_pb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        shell_pb.Name = "shell_pb";
        shell_pb.Size = new System.Drawing.Size(300, 200);
        shell_pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        shell_pb.TabIndex = 7;
        shell_pb.TabStop = false;
        // 
        // label21
        // 
        label21.AutoSize = true;
        label21.Location = new System.Drawing.Point(279, 324);
        label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label21.Name = "label21";
        label21.Size = new System.Drawing.Size(25, 15);
        label21.TabIndex = 52;
        label21.Text = "мм";
        // 
        // label23
        // 
        label23.AutoSize = true;
        label23.Location = new System.Drawing.Point(278, 441);
        label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label23.Name = "label23";
        label23.Size = new System.Drawing.Size(25, 15);
        label23.TabIndex = 54;
        label23.Text = "мм";
        // 
        // label24
        // 
        label24.AutoSize = true;
        label24.Location = new System.Drawing.Point(278, 412);
        label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label24.Name = "label24";
        label24.Size = new System.Drawing.Size(25, 15);
        label24.TabIndex = 55;
        label24.Text = "мм";
        // 
        // label25
        // 
        label25.AutoSize = true;
        label25.Location = new System.Drawing.Point(278, 383);
        label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label25.Name = "label25";
        label25.Size = new System.Drawing.Size(25, 15);
        label25.TabIndex = 56;
        label25.Text = "мм";
        // 
        // label26
        // 
        label26.AutoSize = true;
        label26.Location = new System.Drawing.Point(278, 354);
        label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label26.Name = "label26";
        label26.Size = new System.Drawing.Size(25, 15);
        label26.TabIndex = 57;
        label26.Text = "мм";
        // 
        // label27
        // 
        label27.AutoSize = true;
        label27.Location = new System.Drawing.Point(278, 469);
        label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label27.Name = "label27";
        label27.Size = new System.Drawing.Size(25, 15);
        label27.TabIndex = 58;
        label27.Text = "мм";
        // 
        // label28
        // 
        label28.AutoSize = true;
        label28.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        label28.Location = new System.Drawing.Point(-1, 462);
        label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        label28.MaximumSize = new System.Drawing.Size(0, 1);
        label28.MinimumSize = new System.Drawing.Size(350, 0);
        label28.Name = "label28";
        label28.Size = new System.Drawing.Size(350, 1);
        label28.TabIndex = 59;
        label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // M_gb
        // 
        M_gb.Controls.Add(label29);
        M_gb.Controls.Add(M_tb);
        M_gb.Enabled = false;
        M_gb.Location = new System.Drawing.Point(371, 492);
        M_gb.Name = "M_gb";
        M_gb.Size = new System.Drawing.Size(326, 54);
        M_gb.TabIndex = 60;
        M_gb.TabStop = false;
        M_gb.Text = "Расчетный изгибающий момент, М";
        // 
        // label29
        // 
        label29.AutoSize = true;
        label29.Location = new System.Drawing.Point(115, 27);
        label29.Name = "label29";
        label29.Size = new System.Drawing.Size(37, 15);
        label29.TabIndex = 1;
        label29.Text = "Н мм";
        // 
        // M_tb
        // 
        M_tb.Location = new System.Drawing.Point(8, 23);
        M_tb.Name = "M_tb";
        M_tb.Size = new System.Drawing.Size(100, 23);
        M_tb.TabIndex = 0;
        M_tb.Text = "0";
        M_tb.TextChanged += DisabledCalculateBtn;
        // 
        // Q_gb
        // 
        Q_gb.Controls.Add(label30);
        Q_gb.Controls.Add(Q_tb);
        Q_gb.Enabled = false;
        Q_gb.Location = new System.Drawing.Point(371, 552);
        Q_gb.Name = "Q_gb";
        Q_gb.Size = new System.Drawing.Size(326, 54);
        Q_gb.TabIndex = 61;
        Q_gb.TabStop = false;
        Q_gb.Text = "Расчетное поперечное усилие, Q";
        // 
        // label30
        // 
        label30.AutoSize = true;
        label30.Location = new System.Drawing.Point(115, 26);
        label30.Name = "label30";
        label30.Size = new System.Drawing.Size(16, 15);
        label30.TabIndex = 3;
        label30.Text = "Н";
        // 
        // Q_tb
        // 
        Q_tb.Location = new System.Drawing.Point(8, 22);
        Q_tb.Name = "Q_tb";
        Q_tb.Size = new System.Drawing.Size(100, 23);
        Q_tb.TabIndex = 2;
        Q_tb.Text = "0";
        Q_tb.TextChanged += DisabledCalculateBtn;
        // 
        // sigmaHandle_cb
        // 
        sigmaHandle_cb.AutoSize = true;
        sigmaHandle_cb.Location = new System.Drawing.Point(234, 214);
        sigmaHandle_cb.Name = "sigmaHandle_cb";
        sigmaHandle_cb.Size = new System.Drawing.Size(75, 34);
        sigmaHandle_cb.TabIndex = 62;
        sigmaHandle_cb.Text = "Задать\r\nвручную";
        sigmaHandle_cb.UseVisualStyleBackColor = true;
        sigmaHandle_cb.CheckedChanged += SigmaHandle_cb_CheckedChanged;
        // 
        // EHandle_cb
        // 
        EHandle_cb.AutoSize = true;
        EHandle_cb.Location = new System.Drawing.Point(234, 252);
        EHandle_cb.Name = "EHandle_cb";
        EHandle_cb.Size = new System.Drawing.Size(75, 34);
        EHandle_cb.TabIndex = 63;
        EHandle_cb.Text = "Задать\r\nвручную";
        EHandle_cb.UseVisualStyleBackColor = true;
        EHandle_cb.CheckedChanged += EHandle_cb_CheckedChanged;
        // 
        // isNozzleCalculateCheckBox
        // 
        isNozzleCalculateCheckBox.AutoSize = true;
        isNozzleCalculateCheckBox.Location = new System.Drawing.Point(151, 598);
        isNozzleCalculateCheckBox.Name = "isNozzleCalculateCheckBox";
        isNozzleCalculateCheckBox.Size = new System.Drawing.Size(190, 19);
        isNozzleCalculateCheckBox.TabIndex = 64;
        isNozzleCalculateCheckBox.Text = "Расчитать штуцер в обечайке";
        isNozzleCalculateCheckBox.UseVisualStyleBackColor = true;
        // 
        // CylindricalShellForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(714, 674);
        Controls.Add(isNozzleCalculateCheckBox);
        Controls.Add(EHandle_cb);
        Controls.Add(sigmaHandle_cb);
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
        Controls.Add(calc_b);
        Controls.Add(predCalc_b);
        Controls.Add(groupBox4);
        Controls.Add(force_gb);
        Controls.Add(groupBox2);
        Controls.Add(button4);
        Controls.Add(button3);
        Controls.Add(defect_b);
        Controls.Add(defect_chb);
        Controls.Add(getL_b);
        Controls.Add(getFi_b);
        Controls.Add(getE_b);
        Controls.Add(label19);
        Controls.Add(grtSigma_b);
        Controls.Add(label18);
        Controls.Add(s_tb);
        Controls.Add(label17);
        Controls.Add(label15);
        Controls.Add(label14);
        Controls.Add(label13);
        Controls.Add(label12);
        Controls.Add(label11);
        Controls.Add(label10);
        Controls.Add(label9);
        Controls.Add(c3_tb);
        Controls.Add(c2_tb);
        Controls.Add(c1_tb);
        Controls.Add(l_tb);
        Controls.Add(D_tb);
        Controls.Add(fi_tb);
        Controls.Add(E_tb);
        Controls.Add(sigma_d_tb);
        Controls.Add(label8);
        Controls.Add(steel_cb);
        Controls.Add(label7);
        Controls.Add(dav_gb);
        Controls.Add(label4);
        Controls.Add(shell_pb);
        Controls.Add(t_tb);
        Controls.Add(Gost_cb);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(button1);
        Controls.Add(Name_tb);
        Controls.Add(label1);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CylindricalShellForm";
        Text = "CilForm";
        FormClosing += CilForm_FormClosing;
        Load += CilForm_Load;
        dav_gb.ResumeLayout(false);
        dav_gb.PerformLayout();
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

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox Name_tb;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox Gost_cb;
    private System.Windows.Forms.TextBox t_tb;
    private System.Windows.Forms.PictureBox shell_pb;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.RadioButton vn_rb;
    private System.Windows.Forms.RadioButton nar_rb;
    private System.Windows.Forms.TextBox p_tb;
    private System.Windows.Forms.GroupBox dav_gb;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.ComboBox steel_cb;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox sigma_d_tb;
    private System.Windows.Forms.TextBox E_tb;
    internal System.Windows.Forms.TextBox fi_tb;
    private System.Windows.Forms.TextBox D_tb;
    private System.Windows.Forms.TextBox l_tb;
    private System.Windows.Forms.TextBox c1_tb;
    private System.Windows.Forms.TextBox c2_tb;
    private System.Windows.Forms.TextBox c3_tb;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.TextBox s_tb;
    private System.Windows.Forms.Label label18;
    private System.Windows.Forms.Button grtSigma_b;
    private System.Windows.Forms.Label label19;
    private System.Windows.Forms.Button getE_b;
    private System.Windows.Forms.Button getFi_b;
    private System.Windows.Forms.Button getL_b;
    private System.Windows.Forms.CheckBox defect_chb;
    private System.Windows.Forms.Button defect_b;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.RadioButton stressHand_rb;
    private System.Windows.Forms.RadioButton stressCalc_rb;
    private System.Windows.Forms.GroupBox force_gb;
    private System.Windows.Forms.RadioButton rb1;
    private System.Windows.Forms.PictureBox f_pb;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.RadioButton rb7;
    private System.Windows.Forms.RadioButton rb6;
    private System.Windows.Forms.RadioButton rb5;
    private System.Windows.Forms.RadioButton rb4;
    private System.Windows.Forms.RadioButton rb3;
    private System.Windows.Forms.RadioButton rb2;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.Label p_d_l;
    private System.Windows.Forms.Label scalc_l;
    private System.Windows.Forms.Button predCalc_b;
    private System.Windows.Forms.Button calc_b;
    private System.Windows.Forms.Button cancel_b;
    private System.Windows.Forms.Label label21;
    private System.Windows.Forms.Label label23;
    private System.Windows.Forms.Label label24;
    private System.Windows.Forms.Label label25;
    private System.Windows.Forms.Label label26;
    private System.Windows.Forms.Label label27;
    private System.Windows.Forms.Label label28;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.RadioButton forceCompress_rb;
    private System.Windows.Forms.RadioButton forceStretch_rb;
    private System.Windows.Forms.TextBox F_tb;
    private System.Windows.Forms.GroupBox M_gb;
    private System.Windows.Forms.Label label29;
    private System.Windows.Forms.TextBox M_tb;
    private System.Windows.Forms.GroupBox Q_gb;
    private System.Windows.Forms.Label label30;
    private System.Windows.Forms.TextBox Q_tb;
    private System.Windows.Forms.Panel fq_panel;
    private System.Windows.Forms.Label fq_mes_l;
    private System.Windows.Forms.Label fq_l;
    private System.Windows.Forms.TextBox fq_tb;
    private System.Windows.Forms.CheckBox sigmaHandle_cb;
    private System.Windows.Forms.CheckBox EHandle_cb;
    private System.Windows.Forms.CheckBox isNozzleCalculateCheckBox;
}