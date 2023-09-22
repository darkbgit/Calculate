
namespace CalculateVessels.Forms
{
    partial class FlatBottomForm
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
            label28 = new Label();
            label27 = new Label();
            label25 = new Label();
            label24 = new Label();
            label23 = new Label();
            cancel_btn = new Button();
            calc_btn = new Button();
            preCalc_btn = new Button();
            groupBox4 = new GroupBox();
            p_d_l = new Label();
            scalc_l = new Label();
            type_gb = new GroupBox();
            rb15 = new RadioButton();
            rb14 = new RadioButton();
            rb13 = new RadioButton();
            rb12 = new RadioButton();
            rb11 = new RadioButton();
            rb10 = new RadioButton();
            rb9 = new RadioButton();
            rb8 = new RadioButton();
            rb7 = new RadioButton();
            rb6 = new RadioButton();
            rb5 = new RadioButton();
            rb4 = new RadioButton();
            rb3 = new RadioButton();
            rb2 = new RadioButton();
            rb1 = new RadioButton();
            type_pb = new PictureBox();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            checkBox1 = new CheckBox();
            getFi_b = new Button();
            s1_tb = new TextBox();
            label17 = new Label();
            label15 = new Label();
            label14 = new Label();
            label13 = new Label();
            label10 = new Label();
            c3_tb = new TextBox();
            c2_tb = new TextBox();
            c1_tb = new TextBox();
            phi_tb = new TextBox();
            Gost_cb = new ComboBox();
            label2 = new Label();
            name_tb = new TextBox();
            label1 = new Label();
            otv_gb = new GroupBox();
            manyHole_rb = new RadioButton();
            oneHole_rb = new RadioButton();
            label30 = new Label();
            holed_tb = new TextBox();
            hole_cb = new CheckBox();
            typePanel = new Panel();
            loadingConditionControl = new Controls.LoadingConditionControl();
            steelControl = new Controls.SteelControl();
            groupBox4.SuspendLayout();
            type_gb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)type_pb).BeginInit();
            otv_gb.SuspendLayout();
            SuspendLayout();
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.BorderStyle = BorderStyle.Fixed3D;
            label28.Location = new Point(1, 422);
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
            label27.Location = new Point(279, 430);
            label27.Margin = new Padding(4, 0, 4, 0);
            label27.Name = "label27";
            label27.Size = new Size(25, 15);
            label27.TabIndex = 114;
            label27.Text = "мм";
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(279, 342);
            label25.Margin = new Padding(4, 0, 4, 0);
            label25.Name = "label25";
            label25.Size = new Size(25, 15);
            label25.TabIndex = 112;
            label25.Text = "мм";
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(279, 372);
            label24.Margin = new Padding(4, 0, 4, 0);
            label24.Name = "label24";
            label24.Size = new Size(25, 15);
            label24.TabIndex = 111;
            label24.Text = "мм";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(279, 402);
            label23.Margin = new Padding(4, 0, 4, 0);
            label23.Name = "label23";
            label23.Size = new Size(25, 15);
            label23.TabIndex = 110;
            label23.Text = "мм";
            // 
            // cancel_btn
            // 
            cancel_btn.Location = new Point(247, 586);
            cancel_btn.Margin = new Padding(4, 3, 4, 3);
            cancel_btn.Name = "cancel_btn";
            cancel_btn.Size = new Size(88, 27);
            cancel_btn.TabIndex = 84;
            cancel_btn.Text = "Cancel";
            cancel_btn.UseVisualStyleBackColor = true;
            cancel_btn.Click += Cancel_b_Click;
            // 
            // calc_btn
            // 
            calc_btn.Enabled = false;
            calc_btn.Location = new Point(152, 586);
            calc_btn.Margin = new Padding(4, 3, 4, 3);
            calc_btn.Name = "calc_btn";
            calc_btn.Size = new Size(88, 27);
            calc_btn.TabIndex = 83;
            calc_btn.Text = "Расчет";
            calc_btn.UseVisualStyleBackColor = true;
            calc_btn.Click += CalculateButton_Click;
            // 
            // preCalc_btn
            // 
            preCalc_btn.Location = new Point(14, 578);
            preCalc_btn.Margin = new Padding(4, 3, 4, 3);
            preCalc_btn.Name = "preCalc_btn";
            preCalc_btn.Size = new Size(130, 42);
            preCalc_btn.TabIndex = 81;
            preCalc_btn.Text = "Предварительный\r\nрасчет";
            preCalc_btn.UseVisualStyleBackColor = true;
            preCalc_btn.Click += PreCalculateButton_Click;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(p_d_l);
            groupBox4.Controls.Add(scalc_l);
            groupBox4.Location = new Point(427, 570);
            groupBox4.Margin = new Padding(4, 3, 4, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(4, 3, 4, 3);
            groupBox4.Size = new Size(270, 58);
            groupBox4.TabIndex = 107;
            groupBox4.TabStop = false;
            groupBox4.Text = "Результаты расчета";
            // 
            // p_d_l
            // 
            p_d_l.AutoSize = true;
            p_d_l.Location = new Point(126, 18);
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
            // type_gb
            // 
            type_gb.Controls.Add(rb15);
            type_gb.Controls.Add(rb14);
            type_gb.Controls.Add(rb13);
            type_gb.Controls.Add(rb12);
            type_gb.Controls.Add(rb11);
            type_gb.Controls.Add(rb10);
            type_gb.Controls.Add(rb9);
            type_gb.Controls.Add(rb8);
            type_gb.Controls.Add(rb7);
            type_gb.Controls.Add(rb6);
            type_gb.Controls.Add(rb5);
            type_gb.Controls.Add(rb4);
            type_gb.Controls.Add(rb3);
            type_gb.Controls.Add(rb2);
            type_gb.Controls.Add(rb1);
            type_gb.Controls.Add(type_pb);
            type_gb.Location = new Point(372, 12);
            type_gb.Margin = new Padding(4, 3, 4, 3);
            type_gb.Name = "type_gb";
            type_gb.Padding = new Padding(4, 3, 4, 3);
            type_gb.Size = new Size(367, 244);
            type_gb.TabIndex = 106;
            type_gb.TabStop = false;
            type_gb.Text = "Конструкция днищ и крышек";
            // 
            // rb15
            // 
            rb15.AutoSize = true;
            rb15.Location = new Point(321, 211);
            rb15.Name = "rb15";
            rb15.Size = new Size(37, 19);
            rb15.TabIndex = 16;
            rb15.TabStop = true;
            rb15.Text = "15";
            rb15.UseVisualStyleBackColor = true;
            rb15.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb14
            // 
            rb14.AutoSize = true;
            rb14.Location = new Point(321, 185);
            rb14.Margin = new Padding(4, 3, 4, 3);
            rb14.Name = "rb14";
            rb14.Size = new Size(37, 19);
            rb14.TabIndex = 15;
            rb14.Text = "14";
            rb14.UseVisualStyleBackColor = true;
            rb14.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb13
            // 
            rb13.AutoSize = true;
            rb13.Location = new Point(321, 157);
            rb13.Margin = new Padding(4, 3, 4, 3);
            rb13.Name = "rb13";
            rb13.Size = new Size(37, 19);
            rb13.TabIndex = 14;
            rb13.Text = "13";
            rb13.UseVisualStyleBackColor = true;
            rb13.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb12
            // 
            rb12.AutoSize = true;
            rb12.Location = new Point(320, 130);
            rb12.Margin = new Padding(4, 3, 4, 3);
            rb12.Name = "rb12";
            rb12.Size = new Size(37, 19);
            rb12.TabIndex = 13;
            rb12.Text = "12";
            rb12.UseVisualStyleBackColor = true;
            rb12.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb11
            // 
            rb11.AutoSize = true;
            rb11.Location = new Point(320, 105);
            rb11.Margin = new Padding(4, 3, 4, 3);
            rb11.Name = "rb11";
            rb11.Size = new Size(37, 19);
            rb11.TabIndex = 12;
            rb11.Text = "11";
            rb11.UseVisualStyleBackColor = true;
            rb11.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb10
            // 
            rb10.AutoSize = true;
            rb10.Location = new Point(320, 77);
            rb10.Margin = new Padding(4, 3, 4, 3);
            rb10.Name = "rb10";
            rb10.Size = new Size(37, 19);
            rb10.TabIndex = 11;
            rb10.Text = "10";
            rb10.UseVisualStyleBackColor = true;
            rb10.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb9
            // 
            rb9.AutoSize = true;
            rb9.Location = new Point(320, 49);
            rb9.Margin = new Padding(4, 3, 4, 3);
            rb9.Name = "rb9";
            rb9.Size = new Size(31, 19);
            rb9.TabIndex = 10;
            rb9.Text = "9";
            rb9.UseVisualStyleBackColor = true;
            rb9.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb8
            // 
            rb8.AutoSize = true;
            rb8.Location = new Point(319, 22);
            rb8.Margin = new Padding(4, 3, 4, 3);
            rb8.Name = "rb8";
            rb8.Size = new Size(31, 19);
            rb8.TabIndex = 9;
            rb8.Text = "8";
            rb8.UseVisualStyleBackColor = true;
            rb8.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb7
            // 
            rb7.AutoSize = true;
            rb7.Location = new Point(282, 185);
            rb7.Margin = new Padding(4, 3, 4, 3);
            rb7.Name = "rb7";
            rb7.Size = new Size(31, 19);
            rb7.TabIndex = 8;
            rb7.Text = "7";
            rb7.UseVisualStyleBackColor = true;
            rb7.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb6
            // 
            rb6.AutoSize = true;
            rb6.Location = new Point(282, 157);
            rb6.Margin = new Padding(4, 3, 4, 3);
            rb6.Name = "rb6";
            rb6.Size = new Size(31, 19);
            rb6.TabIndex = 7;
            rb6.Text = "6";
            rb6.UseVisualStyleBackColor = true;
            rb6.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb5
            // 
            rb5.AutoSize = true;
            rb5.Location = new Point(281, 130);
            rb5.Margin = new Padding(4, 3, 4, 3);
            rb5.Name = "rb5";
            rb5.Size = new Size(31, 19);
            rb5.TabIndex = 6;
            rb5.Text = "5";
            rb5.UseVisualStyleBackColor = true;
            rb5.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb4
            // 
            rb4.AutoSize = true;
            rb4.Location = new Point(281, 105);
            rb4.Margin = new Padding(4, 3, 4, 3);
            rb4.Name = "rb4";
            rb4.Size = new Size(31, 19);
            rb4.TabIndex = 5;
            rb4.Text = "4";
            rb4.UseVisualStyleBackColor = true;
            rb4.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb3
            // 
            rb3.AutoSize = true;
            rb3.Location = new Point(281, 77);
            rb3.Margin = new Padding(4, 3, 4, 3);
            rb3.Name = "rb3";
            rb3.Size = new Size(31, 19);
            rb3.TabIndex = 4;
            rb3.Text = "3";
            rb3.UseVisualStyleBackColor = true;
            rb3.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb2
            // 
            rb2.AutoSize = true;
            rb2.Location = new Point(281, 49);
            rb2.Margin = new Padding(4, 3, 4, 3);
            rb2.Name = "rb2";
            rb2.Size = new Size(31, 19);
            rb2.TabIndex = 3;
            rb2.Text = "2";
            rb2.UseVisualStyleBackColor = true;
            rb2.CheckedChanged += Rb_CheckedChanged;
            // 
            // rb1
            // 
            rb1.AutoSize = true;
            rb1.Checked = true;
            rb1.Location = new Point(280, 22);
            rb1.Margin = new Padding(4, 3, 4, 3);
            rb1.Name = "rb1";
            rb1.Size = new Size(31, 19);
            rb1.TabIndex = 2;
            rb1.TabStop = true;
            rb1.Text = "1";
            rb1.UseVisualStyleBackColor = true;
            rb1.CheckedChanged += Rb_CheckedChanged;
            // 
            // type_pb
            // 
            type_pb.InitialImage = null;
            type_pb.Location = new Point(8, 22);
            type_pb.Margin = new Padding(4, 3, 4, 3);
            type_pb.Name = "type_pb";
            type_pb.Size = new Size(255, 200);
            type_pb.SizeMode = PictureBoxSizeMode.AutoSize;
            type_pb.TabIndex = 1;
            type_pb.TabStop = false;
            // 
            // button4
            // 
            button4.Location = new Point(192, 520);
            button4.Margin = new Padding(4, 3, 4, 3);
            button4.Name = "button4";
            button4.Size = new Size(170, 27);
            button4.TabIndex = 104;
            button4.Text = "Малоцикловая прочность >>";
            button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(14, 520);
            button3.Margin = new Padding(4, 3, 4, 3);
            button3.Name = "button3";
            button3.Size = new Size(170, 27);
            button3.TabIndex = 103;
            button3.Text = "Изоляция и футеровка >>";
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(252, 487);
            button2.Margin = new Padding(4, 3, 4, 3);
            button2.Name = "button2";
            button2.Size = new Size(35, 27);
            button2.TabIndex = 102;
            button2.Text = ">>";
            button2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(18, 492);
            checkBox1.Margin = new Padding(4, 3, 4, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(201, 19);
            checkBox1.TabIndex = 101;
            checkBox1.Text = "Дефекты по ГОСТ 34233.11-2017";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // getFi_b
            // 
            getFi_b.Location = new Point(322, 309);
            getFi_b.Margin = new Padding(4, 3, 4, 3);
            getFi_b.Name = "getFi_b";
            getFi_b.Size = new Size(43, 23);
            getFi_b.TabIndex = 99;
            getFi_b.Text = ">>";
            getFi_b.UseVisualStyleBackColor = true;
            // 
            // s1_tb
            // 
            s1_tb.Location = new Point(225, 427);
            s1_tb.Margin = new Padding(4, 3, 4, 3);
            s1_tb.Name = "s1_tb";
            s1_tb.Size = new Size(46, 23);
            s1_tb.TabIndex = 79;
            s1_tb.Text = "6";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label17.Location = new Point(39, 430);
            label17.Margin = new Padding(4, 0, 4, 0);
            label17.Name = "label17";
            label17.Size = new Size(180, 15);
            label17.TabIndex = 94;
            label17.Text = "Принятая толщина днища, s1:";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(41, 402);
            label15.Margin = new Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new Size(178, 15);
            label15.TabIndex = 92;
            label15.Text = "Технологическая прибавка, c3:";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(86, 372);
            label14.Margin = new Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new Size(133, 15);
            label14.TabIndex = 91;
            label14.Text = "Минусовой допуск, c2:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label13.Location = new Point(73, 342);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(146, 13);
            label13.TabIndex = 90;
            label13.Text = "Прибавка на коррозию, c1:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ImageAlign = ContentAlignment.MiddleRight;
            label10.Location = new Point(27, 313);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(192, 15);
            label10.TabIndex = 87;
            label10.Text = "К-т прочности сварного шва, φp:";
            // 
            // c3_tb
            // 
            c3_tb.Location = new Point(225, 398);
            c3_tb.Margin = new Padding(4, 3, 4, 3);
            c3_tb.Name = "c3_tb";
            c3_tb.Size = new Size(46, 23);
            c3_tb.TabIndex = 76;
            // 
            // c2_tb
            // 
            c2_tb.Location = new Point(225, 368);
            c2_tb.Margin = new Padding(4, 3, 4, 3);
            c2_tb.Name = "c2_tb";
            c2_tb.Size = new Size(46, 23);
            c2_tb.TabIndex = 75;
            c2_tb.Text = "1";
            // 
            // c1_tb
            // 
            c1_tb.Location = new Point(225, 338);
            c1_tb.Margin = new Padding(4, 3, 4, 3);
            c1_tb.Name = "c1_tb";
            c1_tb.Size = new Size(46, 23);
            c1_tb.TabIndex = 74;
            c1_tb.Text = "1";
            // 
            // phi_tb
            // 
            phi_tb.Location = new Point(225, 309);
            phi_tb.Margin = new Padding(4, 3, 4, 3);
            phi_tb.Name = "phi_tb";
            phi_tb.Size = new Size(46, 23);
            phi_tb.TabIndex = 71;
            phi_tb.Text = "1";
            // 
            // Gost_cb
            // 
            Gost_cb.DropDownStyle = ComboBoxStyle.DropDownList;
            Gost_cb.FormattingEnabled = true;
            Gost_cb.Location = new Point(225, 43);
            Gost_cb.Margin = new Padding(4, 3, 4, 3);
            Gost_cb.Name = "Gost_cb";
            Gost_cb.Size = new Size(139, 23);
            Gost_cb.TabIndex = 63;
            Gost_cb.Tag = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(74, 46);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(144, 15);
            label2.TabIndex = 64;
            label2.Text = "Нормативный документ:";
            // 
            // name_tb
            // 
            name_tb.Location = new Point(224, 12);
            name_tb.Margin = new Padding(4, 3, 4, 3);
            name_tb.Name = "name_tb";
            name_tb.Size = new Size(139, 23);
            name_tb.TabIndex = 61;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(101, 15);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(117, 15);
            label1.TabIndex = 60;
            label1.Text = "Название элемента:";
            // 
            // otv_gb
            // 
            otv_gb.Controls.Add(manyHole_rb);
            otv_gb.Controls.Add(oneHole_rb);
            otv_gb.Controls.Add(label30);
            otv_gb.Controls.Add(holed_tb);
            otv_gb.Enabled = false;
            otv_gb.Location = new Point(372, 263);
            otv_gb.Name = "otv_gb";
            otv_gb.Size = new Size(370, 98);
            otv_gb.TabIndex = 119;
            otv_gb.TabStop = false;
            // 
            // manyHole_rb
            // 
            manyHole_rb.AutoSize = true;
            manyHole_rb.Location = new Point(181, 25);
            manyHole_rb.Name = "manyHole_rb";
            manyHole_rb.Size = new Size(144, 19);
            manyHole_rb.TabIndex = 4;
            manyHole_rb.Text = "Несколько отверстий";
            manyHole_rb.UseVisualStyleBackColor = true;
            // 
            // oneHole_rb
            // 
            oneHole_rb.AutoSize = true;
            oneHole_rb.Checked = true;
            oneHole_rb.Location = new Point(13, 25);
            oneHole_rb.Name = "oneHole_rb";
            oneHole_rb.Size = new Size(112, 19);
            oneHole_rb.TabIndex = 3;
            oneHole_rb.TabStop = true;
            oneHole_rb.Text = "Одно отверстие";
            oneHole_rb.UseVisualStyleBackColor = true;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(13, 52);
            label30.Name = "label30";
            label30.Size = new Size(150, 30);
            label30.TabIndex = 2;
            label30.Text = "Диаметр отверстия\r\nили сумма длин хорд, мм";
            // 
            // holed_tb
            // 
            holed_tb.Location = new Point(169, 56);
            holed_tb.Name = "holed_tb";
            holed_tb.Size = new Size(100, 23);
            holed_tb.TabIndex = 1;
            // 
            // hole_cb
            // 
            hole_cb.AutoSize = true;
            hole_cb.Location = new Point(385, 262);
            hole_cb.Name = "hole_cb";
            hole_cb.Size = new Size(83, 19);
            hole_cb.TabIndex = 0;
            hole_cb.Text = "Отверстия";
            hole_cb.UseVisualStyleBackColor = true;
            hole_cb.CheckedChanged += Otv_cb_CheckedChanged;
            // 
            // typePanel
            // 
            typePanel.Location = new Point(372, 368);
            typePanel.Name = "typePanel";
            typePanel.Size = new Size(369, 196);
            typePanel.TabIndex = 120;
            // 
            // loadingConditionControl
            // 
            loadingConditionControl.Location = new Point(18, 158);
            loadingConditionControl.Name = "loadingConditionControl";
            loadingConditionControl.Size = new Size(355, 145);
            loadingConditionControl.TabIndex = 126;
            // 
            // steelControl
            // 
            steelControl.Location = new Point(98, 72);
            steelControl.MaximumSize = new Size(265, 80);
            steelControl.MinimumSize = new Size(265, 80);
            steelControl.Name = "steelControl";
            steelControl.Size = new Size(265, 80);
            steelControl.TabIndex = 127;
            // 
            // FlatBottomForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(754, 634);
            Controls.Add(steelControl);
            Controls.Add(loadingConditionControl);
            Controls.Add(typePanel);
            Controls.Add(hole_cb);
            Controls.Add(otv_gb);
            Controls.Add(label28);
            Controls.Add(label27);
            Controls.Add(label25);
            Controls.Add(label24);
            Controls.Add(label23);
            Controls.Add(cancel_btn);
            Controls.Add(calc_btn);
            Controls.Add(preCalc_btn);
            Controls.Add(groupBox4);
            Controls.Add(type_gb);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(checkBox1);
            Controls.Add(getFi_b);
            Controls.Add(s1_tb);
            Controls.Add(label17);
            Controls.Add(label15);
            Controls.Add(label14);
            Controls.Add(label13);
            Controls.Add(label10);
            Controls.Add(c3_tb);
            Controls.Add(c2_tb);
            Controls.Add(c1_tb);
            Controls.Add(phi_tb);
            Controls.Add(Gost_cb);
            Controls.Add(label2);
            Controls.Add(name_tb);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FlatBottomForm";
            Text = "Pldn";
            FormClosing += FlatBottomForm_FormClosing;
            Load += FlatBottomForm_Load;
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            type_gb.ResumeLayout(false);
            type_gb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)type_pb).EndInit();
            otv_gb.ResumeLayout(false);
            otv_gb.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label28;
        private Label label27;
        private Label label25;
        private Label label24;
        private Label label23;
        private Button cancel_btn;
        private Button calc_btn;
        private Button preCalc_btn;
        private GroupBox groupBox4;
        private Label p_d_l;
        private Label scalc_l;
        private GroupBox type_gb;
        private RadioButton rb7;
        private RadioButton rb6;
        private RadioButton rb5;
        private RadioButton rb4;
        private RadioButton rb3;
        private RadioButton rb2;
        private RadioButton rb1;
        private PictureBox type_pb;
        private Button button4;
        private Button button3;
        private Button button2;
        private CheckBox checkBox1;
        private Button getFi_b;
        private TextBox s1_tb;
        private Label label17;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label label10;
        private TextBox c3_tb;
        private TextBox c2_tb;
        private TextBox c1_tb;
        internal TextBox phi_tb;
        private ComboBox Gost_cb;
        private Label label2;
        private TextBox name_tb;
        private Label label1;
        private RadioButton rb14;
        private RadioButton rb13;
        private RadioButton rb12;
        private RadioButton rb11;
        private RadioButton rb10;
        private RadioButton rb9;
        private RadioButton rb8;
        private RadioButton rb15;
        private GroupBox otv_gb;
        private CheckBox hole_cb;
        private Label label30;
        private TextBox holed_tb;
        private RadioButton manyHole_rb;
        private RadioButton oneHole_rb;
        private Panel typePanel;
        private Controls.LoadingConditionControl loadingConditionControl;
        private Controls.SteelControl steelControl;
    }
}