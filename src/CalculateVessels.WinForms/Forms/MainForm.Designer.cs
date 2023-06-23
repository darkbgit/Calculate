
using CalculateVessels.Controls;

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
        menuUp = new System.Windows.Forms.MenuStrip();
        FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        OpenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        ToolsStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        SpravkaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        cylindrical_btn = new System.Windows.Forms.Button();
        data_contextMenu = new System.Windows.Forms.ContextMenuStrip(components);
        up_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
        down_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        del_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
        delall_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
        conical_btn = new System.Windows.Forms.Button();
        elliptical_btn = new System.Windows.Forms.Button();
        MakeWord_b = new System.Windows.Forms.Button();
        filePathTextBox = new System.Windows.Forms.TextBox();
        proekt_tb = new System.Windows.Forms.TextBox();
        polysfer_b = new System.Windows.Forms.Button();
        torosfer_b = new System.Windows.Forms.Button();
        flatBottom_btn = new System.Windows.Forms.Button();
        flatBottomWithAdditionalMoment_btn = new System.Windows.Forms.Button();
        button4 = new System.Windows.Forms.Button();
        button5 = new System.Windows.Forms.Button();
        button6 = new System.Windows.Forms.Button();
        button7 = new System.Windows.Forms.Button();
        button8 = new System.Windows.Forms.Button();
        saddle_btn = new System.Windows.Forms.Button();
        button10 = new System.Windows.Forms.Button();
        bracketVertical_btn = new System.Windows.Forms.Button();
        button12 = new System.Windows.Forms.Button();
        button13 = new System.Windows.Forms.Button();
        button14 = new System.Windows.Forms.Button();
        heatExchengerStationaryTubePlate_btn = new System.Windows.Forms.Button();
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
        openFileDialog = new System.Windows.Forms.OpenFileDialog();
        saveFileDialogRst = new System.Windows.Forms.SaveFileDialog();
        calculatedElementsControl = new CalculatedElementsControl();
        label1 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        chooseFileNimeButton = new System.Windows.Forms.Button();
        saveFileDialogDocx = new System.Windows.Forms.SaveFileDialog();
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
        menuUp.Size = new System.Drawing.Size(984, 24);
        menuUp.TabIndex = 1;
        menuUp.Text = "menuUp";
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
        OpenToolStripMenuItem1.Click += OpenToolStripMenuItem1_Click;
        // 
        // SaveToolStripMenuItem
        // 
        SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
        SaveToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
        SaveToolStripMenuItem.Text = "Сохранить расчет";
        SaveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
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
        // cylindrical_btn
        // 
        cylindrical_btn.Image = Properties.Resources.IconCyllindricalShell;
        cylindrical_btn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
        cylindrical_btn.Location = new System.Drawing.Point(14, 31);
        cylindrical_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        cylindrical_btn.Name = "cylindrical_btn";
        cylindrical_btn.Size = new System.Drawing.Size(186, 48);
        cylindrical_btn.TabIndex = 2;
        cylindrical_btn.Text = "Цилиндрическая \r\nобечайка";
        cylindrical_btn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
        cylindrical_btn.UseVisualStyleBackColor = true;
        cylindrical_btn.Click += Cylindrical_btn_Click;
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
        // conical_btn
        // 
        conical_btn.Image = Properties.Resources.IconConicalShell;
        conical_btn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
        conical_btn.Location = new System.Drawing.Point(14, 85);
        conical_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        conical_btn.Name = "conical_btn";
        conical_btn.Size = new System.Drawing.Size(186, 27);
        conical_btn.TabIndex = 4;
        conical_btn.Text = "Конический переход";
        conical_btn.UseVisualStyleBackColor = true;
        conical_btn.Click += Conical_btn_Click;
        // 
        // elliptical_btn
        // 
        elliptical_btn.Location = new System.Drawing.Point(14, 118);
        elliptical_btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        elliptical_btn.Name = "elliptical_btn";
        elliptical_btn.Size = new System.Drawing.Size(186, 27);
        elliptical_btn.TabIndex = 5;
        elliptical_btn.Text = "Эллиптическое днище";
        elliptical_btn.UseVisualStyleBackColor = true;
        elliptical_btn.Click += Elliptical_btn_Click;
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
        // filePathTextBox
        // 
        filePathTextBox.Location = new System.Drawing.Point(702, 443);
        filePathTextBox.Name = "filePathTextBox";
        filePathTextBox.Size = new System.Drawing.Size(158, 23);
        filePathTextBox.TabIndex = 7;
        filePathTextBox.Text = "C:\\Calculate\\";
        // 
        // proekt_tb
        // 
        proekt_tb.Location = new System.Drawing.Point(702, 414);
        proekt_tb.Name = "proekt_tb";
        proekt_tb.Size = new System.Drawing.Size(209, 23);
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
        // flatBottom_btn
        // 
        flatBottom_btn.Location = new System.Drawing.Point(14, 217);
        flatBottom_btn.Name = "flatBottom_btn";
        flatBottom_btn.Size = new System.Drawing.Size(186, 27);
        flatBottom_btn.TabIndex = 11;
        flatBottom_btn.Text = "Плоское днище";
        flatBottom_btn.UseVisualStyleBackColor = true;
        flatBottom_btn.Click += FlatBottom_btn_Click;
        // 
        // flatBottomWithAdditionalMoment_btn
        // 
        flatBottomWithAdditionalMoment_btn.Location = new System.Drawing.Point(14, 250);
        flatBottomWithAdditionalMoment_btn.Name = "flatBottomWithAdditionalMoment_btn";
        flatBottomWithAdditionalMoment_btn.Size = new System.Drawing.Size(186, 60);
        flatBottomWithAdditionalMoment_btn.TabIndex = 12;
        flatBottomWithAdditionalMoment_btn.Text = "Плоское днище с дополнительным краевым моментом";
        flatBottomWithAdditionalMoment_btn.UseVisualStyleBackColor = true;
        flatBottomWithAdditionalMoment_btn.Click += FlatBottomWithAdditionalMoment_btn_Click;
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
        // saddle_btn
        // 
        saddle_btn.Image = Properties.Resources.IconSaddle;
        saddle_btn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
        saddle_btn.Location = new System.Drawing.Point(207, 31);
        saddle_btn.Name = "saddle_btn";
        saddle_btn.Size = new System.Drawing.Size(186, 27);
        saddle_btn.TabIndex = 19;
        saddle_btn.Text = "Седловая опора";
        saddle_btn.UseVisualStyleBackColor = true;
        saddle_btn.Click += Saddle_btn_Click;
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
        // bracketVertical_btn
        // 
        bracketVertical_btn.Image = Properties.Resources.IconBracketVertical;
        bracketVertical_btn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
        bracketVertical_btn.Location = new System.Drawing.Point(207, 64);
        bracketVertical_btn.Name = "bracketVertical_btn";
        bracketVertical_btn.Size = new System.Drawing.Size(186, 27);
        bracketVertical_btn.TabIndex = 21;
        bracketVertical_btn.Text = "Опорные лапы";
        bracketVertical_btn.UseVisualStyleBackColor = true;
        bracketVertical_btn.Click += BracketVertical_btn_Click;
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
        // heatExchengerStationaryTubePlate_btn
        // 
        heatExchengerStationaryTubePlate_btn.Image = Properties.Resources.IconHeatExchanger;
        heatExchengerStationaryTubePlate_btn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
        heatExchengerStationaryTubePlate_btn.Location = new System.Drawing.Point(207, 196);
        heatExchengerStationaryTubePlate_btn.Name = "heatExchengerStationaryTubePlate_btn";
        heatExchengerStationaryTubePlate_btn.Size = new System.Drawing.Size(186, 60);
        heatExchengerStationaryTubePlate_btn.TabIndex = 25;
        heatExchengerStationaryTubePlate_btn.Text = "Теплообменный аппарат\r\n с неподвижными\r\n трубными решетками";
        heatExchengerStationaryTubePlate_btn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
        heatExchengerStationaryTubePlate_btn.UseVisualStyleBackColor = true;
        heatExchengerStationaryTubePlate_btn.Click += HeatExchangerStationaryTubePlates_btn_Click;
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
        // openFileDialog
        // 
        openFileDialog.FileName = "openFileDialog1";
        // 
        // saveFileDialogRst
        // 
        saveFileDialogRst.DefaultExt = "rst";
        saveFileDialogRst.Filter = "|*.rst";
        // 
        // calculatedElementsControl
        // 
        calculatedElementsControl.Location = new System.Drawing.Point(591, 31);
        calculatedElementsControl.Name = "calculatedElementsControl";
        calculatedElementsControl.Size = new System.Drawing.Size(380, 380);
        calculatedElementsControl.TabIndex = 47;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(604, 417);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(92, 15);
        label1.TabIndex = 48;
        label1.Text = "Номер проекта";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(627, 446);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(69, 15);
        label2.TabIndex = 49;
        label2.Text = "Имя файла";
        // 
        // chooseFileNimeButton
        // 
        chooseFileNimeButton.Location = new System.Drawing.Point(866, 442);
        chooseFileNimeButton.Name = "chooseFileNimeButton";
        chooseFileNimeButton.Size = new System.Drawing.Size(45, 23);
        chooseFileNimeButton.TabIndex = 50;
        chooseFileNimeButton.Text = "...";
        chooseFileNimeButton.UseVisualStyleBackColor = true;
        chooseFileNimeButton.Click += ChooseFileNameButton_Click;
        // 
        // saveFileDialogDocx
        // 
        saveFileDialogDocx.DefaultExt = "rst";
        saveFileDialogDocx.Filter = "|*.docx";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(984, 511);
        Controls.Add(chooseFileNimeButton);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(calculatedElementsControl);
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
        Controls.Add(heatExchengerStationaryTubePlate_btn);
        Controls.Add(button14);
        Controls.Add(button13);
        Controls.Add(button12);
        Controls.Add(bracketVertical_btn);
        Controls.Add(button10);
        Controls.Add(saddle_btn);
        Controls.Add(button8);
        Controls.Add(button7);
        Controls.Add(button6);
        Controls.Add(button5);
        Controls.Add(button4);
        Controls.Add(flatBottomWithAdditionalMoment_btn);
        Controls.Add(flatBottom_btn);
        Controls.Add(torosfer_b);
        Controls.Add(polysfer_b);
        Controls.Add(proekt_tb);
        Controls.Add(filePathTextBox);
        Controls.Add(MakeWord_b);
        Controls.Add(elliptical_btn);
        Controls.Add(conical_btn);
        Controls.Add(cylindrical_btn);
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
    private System.Windows.Forms.Button cylindrical_btn;
    private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem SpravkaToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
    private System.Windows.Forms.Button conical_btn;
    private System.Windows.Forms.Button elliptical_btn;
    private System.Windows.Forms.Button MakeWord_b;
    private System.Windows.Forms.TextBox filePathTextBox;
    private System.Windows.Forms.TextBox proekt_tb;
    private System.Windows.Forms.ContextMenuStrip data_contextMenu;
    private System.Windows.Forms.ToolStripMenuItem up_MenuItem;
    private System.Windows.Forms.ToolStripMenuItem down_MenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem del_MenuItem;
    private System.Windows.Forms.ToolStripMenuItem delall_MenuItem;
    private System.Windows.Forms.Button polysfer_b;
    private System.Windows.Forms.Button torosfer_b;
    private System.Windows.Forms.Button flatBottom_btn;
    private System.Windows.Forms.Button flatBottomWithAdditionalMoment_btn;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.Button button5;
    private System.Windows.Forms.Button button6;
    private System.Windows.Forms.Button button7;
    private System.Windows.Forms.Button button8;
    private System.Windows.Forms.Button saddle_btn;
    private System.Windows.Forms.Button button10;
    private System.Windows.Forms.Button bracketVertical_btn;
    private System.Windows.Forms.Button button12;
    private System.Windows.Forms.Button button13;
    private System.Windows.Forms.Button button14;
    private System.Windows.Forms.Button heatExchengerStationaryTubePlate_btn;
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
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.SaveFileDialog saveFileDialogRst;
    internal CalculatedElementsControl calculatedElementsControl;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button chooseFileNimeButton;
    private System.Windows.Forms.SaveFileDialog saveFileDialogDocx;
}

