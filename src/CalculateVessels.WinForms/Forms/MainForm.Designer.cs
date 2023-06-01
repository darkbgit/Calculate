
namespace CalculateVessels.Forms;

partial class MainForm
{
    /// <summary>
    /// Обязательная переменная конструктора.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Освободить все используемые ресурсы.
    /// </summary>
    /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Код, автоматически созданный конструктором форм Windows

    /// <summary>
    /// Требуемый метод для поддержки конструктора — не изменяйте 
    /// содержимое этого метода с помощью редактора кода.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        menuUp = new System.Windows.Forms.MenuStrip();
        FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        OpenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        ToolsStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        SpravkaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        Cil_b = new System.Windows.Forms.Button();
        Word_lv = new System.Windows.Forms.ListView();
        data_contextMenu = new System.Windows.Forms.ContextMenuStrip(components);
        up_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
        down_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        del_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
        delall_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
        Kon_b = new System.Windows.Forms.Button();
        Ell_b = new System.Windows.Forms.Button();
        MakeWord_b = new System.Windows.Forms.Button();
        file_tb = new System.Windows.Forms.TextBox();
        proekt_tb = new System.Windows.Forms.TextBox();
        polysfer_b = new System.Windows.Forms.Button();
        torosfer_b = new System.Windows.Forms.Button();
        flatBottom_b = new System.Windows.Forms.Button();
        flatBottomWithAdditionalMoment_b = new System.Windows.Forms.Button();
        button4 = new System.Windows.Forms.Button();
        button5 = new System.Windows.Forms.Button();
        button6 = new System.Windows.Forms.Button();
        button7 = new System.Windows.Forms.Button();
        button8 = new System.Windows.Forms.Button();
        Saddle_b = new System.Windows.Forms.Button();
        button10 = new System.Windows.Forms.Button();
        bracketVertical_b = new System.Windows.Forms.Button();
        button12 = new System.Windows.Forms.Button();
        button13 = new System.Windows.Forms.Button();
        button14 = new System.Windows.Forms.Button();
        heatExchengerWithFixedTubePlate_b = new System.Windows.Forms.Button();
        button16 = new System.Windows.Forms.Button();
        button17 = new System.Windows.Forms.Button();
        button18 = new System.Windows.Forms.Button();
        button19 = new System.Windows.Forms.Button();
        button20 = new System.Windows.Forms.Button();
        button21 = new System.Windows.Forms.Button();
        button22 = new System.Windows.Forms.Button();
        button23 = new System.Windows.Forms.Button();
        button24 = new System.Windows.Forms.Button();
        button25 = new System.Windows.Forms.Button();
        button26 = new System.Windows.Forms.Button();
        button27 = new System.Windows.Forms.Button();
        button28 = new System.Windows.Forms.Button();
        button29 = new System.Windows.Forms.Button();
        button30 = new System.Windows.Forms.Button();
        button31 = new System.Windows.Forms.Button();
        button32 = new System.Windows.Forms.Button();
        up_b = new System.Windows.Forms.Button();
        down_b = new System.Windows.Forms.Button();
        del_b = new System.Windows.Forms.Button();
        menuUp.SuspendLayout();
        data_contextMenu.SuspendLayout();
        SuspendLayout();
        // 
        // menuUp
        // 
        menuUp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { FileToolStripMenuItem, ToolsStripMenuItem, SpravkaToolStripMenuItem });
        menuUp.Location = new System.Drawing.Point(0, 0);
        menuUp.Name = "menuUp";
        menuUp.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
        menuUp.Size = new System.Drawing.Size(922, 24);
        menuUp.TabIndex = 1;
        menuUp.Text = "menuUp";
        menuUp.ItemClicked += MenuUp_ItemClicked;
        // 
        // FileToolStripMenuItem
        // 
        FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { OpenToolStripMenuItem1, SaveToolStripMenuItem });
        FileToolStripMenuItem.Name = "FileToolStripMenuItem";
        FileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
        FileToolStripMenuItem.Text = "Файл";
        // 
        // OpenToolStripMenuItem1
        // 
        OpenToolStripMenuItem1.Name = "OpenToolStripMenuItem1";
        OpenToolStripMenuItem1.Size = new System.Drawing.Size(173, 22);
        OpenToolStripMenuItem1.Text = "Открыть расчет";
        // 
        // SaveToolStripMenuItem
        // 
        SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
        SaveToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
        SaveToolStripMenuItem.Text = "Сохранить расчет";
        // 
        // ToolsStripMenuItem
        // 
        ToolsStripMenuItem.Name = "ToolsStripMenuItem";
        ToolsStripMenuItem.Size = new System.Drawing.Size(46, 20);
        ToolsStripMenuItem.Text = "Tools";
        // 
        // SpravkaToolStripMenuItem
        // 
        SpravkaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { AboutToolStripMenuItem, ExitToolStripMenuItem });
        SpravkaToolStripMenuItem.Name = "SpravkaToolStripMenuItem";
        SpravkaToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
        SpravkaToolStripMenuItem.Text = "Справка";
        // 
        // AboutToolStripMenuItem
        // 
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
        AboutToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
        AboutToolStripMenuItem.Text = "О программе";
        AboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;
        // 
        // ExitToolStripMenuItem
        // 
        ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
        ExitToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
        ExitToolStripMenuItem.Text = "Выход";
        // 
        // Cil_b
        // 
        Cil_b.Image = Properties.Resources.Icon1000;
        Cil_b.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
        Cil_b.Location = new System.Drawing.Point(14, 31);
        Cil_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        Cil_b.Name = "Cil_b";
        Cil_b.Size = new System.Drawing.Size(186, 48);
        Cil_b.TabIndex = 2;
        Cil_b.Text = "Цилиндрическая \r\nобечайка";
        Cil_b.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
        Cil_b.UseVisualStyleBackColor = true;
        Cil_b.Click += Cil_b_Click;
        // 
        // Word_lv
        // 
        Word_lv.ContextMenuStrip = data_contextMenu;
        Word_lv.GridLines = true;
        Word_lv.Location = new System.Drawing.Point(604, 27);
        Word_lv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        Word_lv.MultiSelect = false;
        Word_lv.Name = "Word_lv";
        Word_lv.Size = new System.Drawing.Size(259, 381);
        Word_lv.TabIndex = 3;
        Word_lv.UseCompatibleStateImageBehavior = false;
        Word_lv.View = System.Windows.Forms.View.List;
        Word_lv.ItemSelectionChanged += Word_lv_ItemSelectionChanged;
        Word_lv.Leave += Word_lv_Leave;
        // 
        // data_contextMenu
        // 
        data_contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { up_MenuItem, down_MenuItem, toolStripSeparator1, del_MenuItem, delall_MenuItem });
        data_contextMenu.Name = "data_contextMenu";
        data_contextMenu.Size = new System.Drawing.Size(140, 98);
        // 
        // up_MenuItem
        // 
        up_MenuItem.Name = "up_MenuItem";
        up_MenuItem.Size = new System.Drawing.Size(139, 22);
        up_MenuItem.Text = "Вверх";
        // 
        // down_MenuItem
        // 
        down_MenuItem.Name = "down_MenuItem";
        down_MenuItem.Size = new System.Drawing.Size(139, 22);
        down_MenuItem.Text = "Вниз";
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
        // 
        // del_MenuItem
        // 
        del_MenuItem.Name = "del_MenuItem";
        del_MenuItem.Size = new System.Drawing.Size(139, 22);
        del_MenuItem.Text = "Удалить";
        // 
        // delall_MenuItem
        // 
        delall_MenuItem.Name = "delall_MenuItem";
        delall_MenuItem.Size = new System.Drawing.Size(139, 22);
        delall_MenuItem.Text = "Удалить все";
        // 
        // Kon_b
        // 
        Kon_b.Location = new System.Drawing.Point(14, 85);
        Kon_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        Kon_b.Name = "Kon_b";
        Kon_b.Size = new System.Drawing.Size(186, 27);
        Kon_b.TabIndex = 4;
        Kon_b.Text = "Конический переход";
        Kon_b.UseVisualStyleBackColor = true;
        Kon_b.Click += Kon_b_Click;
        // 
        // Ell_b
        // 
        Ell_b.Location = new System.Drawing.Point(14, 118);
        Ell_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        Ell_b.Name = "Ell_b";
        Ell_b.Size = new System.Drawing.Size(186, 27);
        Ell_b.TabIndex = 5;
        Ell_b.Text = "Эллиптическое днище";
        Ell_b.UseVisualStyleBackColor = true;
        Ell_b.Click += Ell_b_Click;
        // 
        // MakeWord_b
        // 
        MakeWord_b.Location = new System.Drawing.Point(604, 472);
        MakeWord_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        MakeWord_b.Name = "MakeWord_b";
        MakeWord_b.Size = new System.Drawing.Size(114, 27);
        MakeWord_b.TabIndex = 6;
        MakeWord_b.Text = "Вывести в Word";
        MakeWord_b.UseVisualStyleBackColor = true;
        MakeWord_b.Click += MakeWord_b_Click;
        // 
        // file_tb
        // 
        file_tb.Location = new System.Drawing.Point(604, 443);
        file_tb.Name = "file_tb";
        file_tb.Size = new System.Drawing.Size(100, 23);
        file_tb.TabIndex = 7;
        file_tb.Text = "C:\\Calculate\\";
        // 
        // proekt_tb
        // 
        proekt_tb.Location = new System.Drawing.Point(604, 414);
        proekt_tb.Name = "proekt_tb";
        proekt_tb.Size = new System.Drawing.Size(189, 23);
        proekt_tb.TabIndex = 8;
        // 
        // polysfer_b
        // 
        polysfer_b.Enabled = false;
        polysfer_b.Location = new System.Drawing.Point(14, 151);
        polysfer_b.Name = "polysfer_b";
        polysfer_b.Size = new System.Drawing.Size(186, 27);
        polysfer_b.TabIndex = 9;
        polysfer_b.Text = "Полусферическое днище";
        polysfer_b.UseVisualStyleBackColor = true;
        // 
        // torosfer_b
        // 
        torosfer_b.Enabled = false;
        torosfer_b.Location = new System.Drawing.Point(14, 184);
        torosfer_b.Name = "torosfer_b";
        torosfer_b.Size = new System.Drawing.Size(186, 27);
        torosfer_b.TabIndex = 10;
        torosfer_b.Text = "Торосферическое днище";
        torosfer_b.UseVisualStyleBackColor = true;
        // 
        // flatBottom_b
        // 
        flatBottom_b.Location = new System.Drawing.Point(14, 217);
        flatBottom_b.Name = "flatBottom_b";
        flatBottom_b.Size = new System.Drawing.Size(186, 27);
        flatBottom_b.TabIndex = 11;
        flatBottom_b.Text = "Плоское днище";
        flatBottom_b.UseVisualStyleBackColor = true;
        flatBottom_b.Click += FlatBottom_b_Click;
        // 
        // flatBottomWithAdditionalMoment_b
        // 
        flatBottomWithAdditionalMoment_b.Location = new System.Drawing.Point(14, 250);
        flatBottomWithAdditionalMoment_b.Name = "flatBottomWithAdditionalMoment_b";
        flatBottomWithAdditionalMoment_b.Size = new System.Drawing.Size(186, 60);
        flatBottomWithAdditionalMoment_b.TabIndex = 12;
        flatBottomWithAdditionalMoment_b.Text = "Плоское днище с дополнительным краевым моментом";
        flatBottomWithAdditionalMoment_b.UseVisualStyleBackColor = true;
        flatBottomWithAdditionalMoment_b.Click += FlatBottomWithAdditionalMoment_b_Click;
        // 
        // button4
        // 
        button4.Enabled = false;
        button4.Location = new System.Drawing.Point(14, 316);
        button4.Name = "button4";
        button4.Size = new System.Drawing.Size(186, 27);
        button4.TabIndex = 14;
        button4.Text = "button4";
        button4.UseVisualStyleBackColor = true;
        // 
        // button5
        // 
        button5.Enabled = false;
        button5.Location = new System.Drawing.Point(14, 349);
        button5.Name = "button5";
        button5.Size = new System.Drawing.Size(186, 27);
        button5.TabIndex = 15;
        button5.Text = "button5";
        button5.UseVisualStyleBackColor = true;
        // 
        // button6
        // 
        button6.Enabled = false;
        button6.Location = new System.Drawing.Point(14, 382);
        button6.Name = "button6";
        button6.Size = new System.Drawing.Size(186, 27);
        button6.TabIndex = 16;
        button6.Text = "button6";
        button6.UseVisualStyleBackColor = true;
        // 
        // button7
        // 
        button7.Enabled = false;
        button7.Location = new System.Drawing.Point(14, 415);
        button7.Name = "button7";
        button7.Size = new System.Drawing.Size(186, 27);
        button7.TabIndex = 17;
        button7.Text = "button7";
        button7.UseVisualStyleBackColor = true;
        // 
        // button8
        // 
        button8.Enabled = false;
        button8.Location = new System.Drawing.Point(14, 448);
        button8.Name = "button8";
        button8.Size = new System.Drawing.Size(186, 27);
        button8.TabIndex = 18;
        button8.Text = "button8";
        button8.UseVisualStyleBackColor = true;
        // 
        // Saddle_b
        // 
        Saddle_b.Location = new System.Drawing.Point(207, 31);
        Saddle_b.Name = "Saddle_b";
        Saddle_b.Size = new System.Drawing.Size(186, 27);
        Saddle_b.TabIndex = 19;
        Saddle_b.Text = "Седловая опора";
        Saddle_b.UseVisualStyleBackColor = true;
        Saddle_b.Click += Saddle_b_Click;
        // 
        // button10
        // 
        button10.Enabled = false;
        button10.Location = new System.Drawing.Point(399, 31);
        button10.Name = "button10";
        button10.Size = new System.Drawing.Size(186, 27);
        button10.TabIndex = 20;
        button10.Text = "button10";
        button10.UseVisualStyleBackColor = true;
        // 
        // bracketVertical_b
        // 
        bracketVertical_b.Location = new System.Drawing.Point(207, 64);
        bracketVertical_b.Name = "bracketVertical_b";
        bracketVertical_b.Size = new System.Drawing.Size(186, 27);
        bracketVertical_b.TabIndex = 21;
        bracketVertical_b.Text = "Опорные лапы";
        bracketVertical_b.UseVisualStyleBackColor = true;
        bracketVertical_b.Click += BracketVertical_b_Click;
        // 
        // button12
        // 
        button12.Enabled = false;
        button12.Location = new System.Drawing.Point(207, 97);
        button12.Name = "button12";
        button12.Size = new System.Drawing.Size(186, 27);
        button12.TabIndex = 22;
        button12.Text = "button12";
        button12.UseVisualStyleBackColor = true;
        // 
        // button13
        // 
        button13.Enabled = false;
        button13.Location = new System.Drawing.Point(206, 130);
        button13.Name = "button13";
        button13.Size = new System.Drawing.Size(186, 27);
        button13.TabIndex = 23;
        button13.Text = "button13";
        button13.UseVisualStyleBackColor = true;
        // 
        // button14
        // 
        button14.Enabled = false;
        button14.Location = new System.Drawing.Point(207, 163);
        button14.Name = "button14";
        button14.Size = new System.Drawing.Size(186, 27);
        button14.TabIndex = 24;
        button14.Text = "button14";
        button14.UseVisualStyleBackColor = true;
        // 
        // heatExchengerWithFixedTubePlate_b
        // 
        heatExchengerWithFixedTubePlate_b.Image = Properties.Resources.Icon41000;
        heatExchengerWithFixedTubePlate_b.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
        heatExchengerWithFixedTubePlate_b.Location = new System.Drawing.Point(207, 196);
        heatExchengerWithFixedTubePlate_b.Name = "heatExchengerWithFixedTubePlate_b";
        heatExchengerWithFixedTubePlate_b.Size = new System.Drawing.Size(186, 60);
        heatExchengerWithFixedTubePlate_b.TabIndex = 25;
        heatExchengerWithFixedTubePlate_b.Text = "Теплообменный аппарат\r\n с неподвижными\r\n трубными решетками";
        heatExchengerWithFixedTubePlate_b.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
        heatExchengerWithFixedTubePlate_b.UseVisualStyleBackColor = true;
        heatExchengerWithFixedTubePlate_b.Click += HeatExchangerWithFixedTubePlate_b_Click;
        // 
        // button16
        // 
        button16.Enabled = false;
        button16.Location = new System.Drawing.Point(206, 427);
        button16.Name = "button16";
        button16.Size = new System.Drawing.Size(186, 27);
        button16.TabIndex = 31;
        button16.Text = "button16";
        button16.UseVisualStyleBackColor = true;
        // 
        // button17
        // 
        button17.Enabled = false;
        button17.Location = new System.Drawing.Point(206, 394);
        button17.Name = "button17";
        button17.Size = new System.Drawing.Size(186, 27);
        button17.TabIndex = 30;
        button17.Text = "button17";
        button17.UseVisualStyleBackColor = true;
        // 
        // button18
        // 
        button18.Enabled = false;
        button18.Location = new System.Drawing.Point(205, 361);
        button18.Name = "button18";
        button18.Size = new System.Drawing.Size(186, 27);
        button18.TabIndex = 29;
        button18.Text = "button18";
        button18.UseVisualStyleBackColor = true;
        // 
        // button19
        // 
        button19.Enabled = false;
        button19.Location = new System.Drawing.Point(206, 328);
        button19.Name = "button19";
        button19.Size = new System.Drawing.Size(186, 27);
        button19.TabIndex = 28;
        button19.Text = "button19";
        button19.UseVisualStyleBackColor = true;
        // 
        // button20
        // 
        button20.Enabled = false;
        button20.Location = new System.Drawing.Point(206, 295);
        button20.Name = "button20";
        button20.Size = new System.Drawing.Size(186, 27);
        button20.TabIndex = 27;
        button20.Text = "button20";
        button20.UseVisualStyleBackColor = true;
        // 
        // button21
        // 
        button21.Enabled = false;
        button21.Location = new System.Drawing.Point(206, 262);
        button21.Name = "button21";
        button21.Size = new System.Drawing.Size(186, 27);
        button21.TabIndex = 26;
        button21.Text = "button21";
        button21.UseVisualStyleBackColor = true;
        // 
        // button22
        // 
        button22.Enabled = false;
        button22.Location = new System.Drawing.Point(399, 394);
        button22.Name = "button22";
        button22.Size = new System.Drawing.Size(186, 27);
        button22.TabIndex = 42;
        button22.Text = "button22";
        button22.UseVisualStyleBackColor = true;
        // 
        // button23
        // 
        button23.Enabled = false;
        button23.Location = new System.Drawing.Point(399, 361);
        button23.Name = "button23";
        button23.Size = new System.Drawing.Size(186, 27);
        button23.TabIndex = 41;
        button23.Text = "button23";
        button23.UseVisualStyleBackColor = true;
        // 
        // button24
        // 
        button24.Enabled = false;
        button24.Location = new System.Drawing.Point(398, 328);
        button24.Name = "button24";
        button24.Size = new System.Drawing.Size(186, 27);
        button24.TabIndex = 40;
        button24.Text = "button24";
        button24.UseVisualStyleBackColor = true;
        // 
        // button25
        // 
        button25.Enabled = false;
        button25.Location = new System.Drawing.Point(399, 295);
        button25.Name = "button25";
        button25.Size = new System.Drawing.Size(186, 27);
        button25.TabIndex = 39;
        button25.Text = "button25";
        button25.UseVisualStyleBackColor = true;
        // 
        // button26
        // 
        button26.Enabled = false;
        button26.Location = new System.Drawing.Point(399, 262);
        button26.Name = "button26";
        button26.Size = new System.Drawing.Size(186, 27);
        button26.TabIndex = 38;
        button26.Text = "button26";
        button26.UseVisualStyleBackColor = true;
        // 
        // button27
        // 
        button27.Enabled = false;
        button27.Location = new System.Drawing.Point(399, 229);
        button27.Name = "button27";
        button27.Size = new System.Drawing.Size(186, 27);
        button27.TabIndex = 37;
        button27.Text = "button27";
        button27.UseVisualStyleBackColor = true;
        // 
        // button28
        // 
        button28.Enabled = false;
        button28.Location = new System.Drawing.Point(399, 196);
        button28.Name = "button28";
        button28.Size = new System.Drawing.Size(186, 27);
        button28.TabIndex = 36;
        button28.Text = "button28";
        button28.UseVisualStyleBackColor = true;
        // 
        // button29
        // 
        button29.Enabled = false;
        button29.Location = new System.Drawing.Point(399, 163);
        button29.Name = "button29";
        button29.Size = new System.Drawing.Size(186, 27);
        button29.TabIndex = 35;
        button29.Text = "button29";
        button29.UseVisualStyleBackColor = true;
        // 
        // button30
        // 
        button30.Enabled = false;
        button30.Location = new System.Drawing.Point(398, 130);
        button30.Name = "button30";
        button30.Size = new System.Drawing.Size(186, 27);
        button30.TabIndex = 34;
        button30.Text = "button30";
        button30.UseVisualStyleBackColor = true;
        // 
        // button31
        // 
        button31.Enabled = false;
        button31.Location = new System.Drawing.Point(399, 97);
        button31.Name = "button31";
        button31.Size = new System.Drawing.Size(186, 27);
        button31.TabIndex = 33;
        button31.Text = "button31";
        button31.UseVisualStyleBackColor = true;
        // 
        // button32
        // 
        button32.Enabled = false;
        button32.Location = new System.Drawing.Point(399, 64);
        button32.Name = "button32";
        button32.Size = new System.Drawing.Size(186, 27);
        button32.TabIndex = 32;
        button32.Text = "button32";
        button32.UseVisualStyleBackColor = true;
        // 
        // up_b
        // 
        up_b.Enabled = false;
        up_b.Image = (System.Drawing.Image)resources.GetObject("up_b.Image");
        up_b.Location = new System.Drawing.Point(870, 43);
        up_b.Name = "up_b";
        up_b.Size = new System.Drawing.Size(40, 25);
        up_b.TabIndex = 43;
        up_b.UseVisualStyleBackColor = true;
        up_b.Click += Up_b_Click;
        // 
        // down_b
        // 
        down_b.Enabled = false;
        down_b.Image = (System.Drawing.Image)resources.GetObject("down_b.Image");
        down_b.Location = new System.Drawing.Point(870, 75);
        down_b.Name = "down_b";
        down_b.Size = new System.Drawing.Size(40, 25);
        down_b.TabIndex = 44;
        down_b.UseVisualStyleBackColor = true;
        down_b.Click += Down_b_Click;
        // 
        // del_b
        // 
        del_b.Enabled = false;
        del_b.Image = (System.Drawing.Image)resources.GetObject("del_b.Image");
        del_b.Location = new System.Drawing.Point(870, 126);
        del_b.Name = "del_b";
        del_b.Size = new System.Drawing.Size(40, 25);
        del_b.TabIndex = 45;
        del_b.UseVisualStyleBackColor = true;
        del_b.Click += Del_b_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(922, 532);
        Controls.Add(del_b);
        Controls.Add(down_b);
        Controls.Add(up_b);
        Controls.Add(button22);
        Controls.Add(button23);
        Controls.Add(button24);
        Controls.Add(button25);
        Controls.Add(button26);
        Controls.Add(button27);
        Controls.Add(button28);
        Controls.Add(button29);
        Controls.Add(button30);
        Controls.Add(button31);
        Controls.Add(button32);
        Controls.Add(button16);
        Controls.Add(button17);
        Controls.Add(button18);
        Controls.Add(button19);
        Controls.Add(button20);
        Controls.Add(button21);
        Controls.Add(heatExchengerWithFixedTubePlate_b);
        Controls.Add(button14);
        Controls.Add(button13);
        Controls.Add(button12);
        Controls.Add(bracketVertical_b);
        Controls.Add(button10);
        Controls.Add(Saddle_b);
        Controls.Add(button8);
        Controls.Add(button7);
        Controls.Add(button6);
        Controls.Add(button5);
        Controls.Add(button4);
        Controls.Add(flatBottomWithAdditionalMoment_b);
        Controls.Add(flatBottom_b);
        Controls.Add(torosfer_b);
        Controls.Add(polysfer_b);
        Controls.Add(proekt_tb);
        Controls.Add(file_tb);
        Controls.Add(MakeWord_b);
        Controls.Add(Ell_b);
        Controls.Add(Kon_b);
        Controls.Add(Word_lv);
        Controls.Add(Cil_b);
        Controls.Add(menuUp);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        MainMenuStrip = menuUp;
        Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        Name = "MainForm";
        Text = "Расчет на прочность";
        menuUp.ResumeLayout(false);
        menuUp.PerformLayout();
        data_contextMenu.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }


    #endregion
    private System.Windows.Forms.MenuStrip menuUp;
    private System.Windows.Forms.Button Cil_b;
    private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem SpravkaToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
    private System.Windows.Forms.Button Kon_b;
    private System.Windows.Forms.Button Ell_b;
    private System.Windows.Forms.Button MakeWord_b;
    internal System.Windows.Forms.ListView Word_lv;
    private System.Windows.Forms.TextBox file_tb;
    private System.Windows.Forms.TextBox proekt_tb;
    private System.Windows.Forms.ContextMenuStrip data_contextMenu;
    private System.Windows.Forms.ToolStripMenuItem up_MenuItem;
    private System.Windows.Forms.ToolStripMenuItem down_MenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem del_MenuItem;
    private System.Windows.Forms.ToolStripMenuItem delall_MenuItem;
    private System.Windows.Forms.Button polysfer_b;
    private System.Windows.Forms.Button torosfer_b;
    private System.Windows.Forms.Button flatBottom_b;
    private System.Windows.Forms.Button flatBottomWithAdditionalMoment_b;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.Button button5;
    private System.Windows.Forms.Button button6;
    private System.Windows.Forms.Button button7;
    private System.Windows.Forms.Button button8;
    private System.Windows.Forms.Button Saddle_b;
    private System.Windows.Forms.Button button10;
    private System.Windows.Forms.Button bracketVertical_b;
    private System.Windows.Forms.Button button12;
    private System.Windows.Forms.Button button13;
    private System.Windows.Forms.Button button14;
    private System.Windows.Forms.Button heatExchengerWithFixedTubePlate_b;
    private System.Windows.Forms.Button button16;
    private System.Windows.Forms.Button button17;
    private System.Windows.Forms.Button button18;
    private System.Windows.Forms.Button button19;
    private System.Windows.Forms.Button button20;
    private System.Windows.Forms.Button button21;
    private System.Windows.Forms.Button button22;
    private System.Windows.Forms.Button button23;
    private System.Windows.Forms.Button button24;
    private System.Windows.Forms.Button button25;
    private System.Windows.Forms.Button button26;
    private System.Windows.Forms.Button button27;
    private System.Windows.Forms.Button button28;
    private System.Windows.Forms.Button button29;
    private System.Windows.Forms.Button button30;
    private System.Windows.Forms.Button button31;
    private System.Windows.Forms.Button button32;
    private System.Windows.Forms.ToolStripMenuItem ToolsStripMenuItem;
    private System.Windows.Forms.Button up_b;
    private System.Windows.Forms.Button down_b;
    private System.Windows.Forms.Button del_b;
}

