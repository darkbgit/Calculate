
namespace calcNet
{
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
            this.components = new System.ComponentModel.Container();
            this.menuUp = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SpravkaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Cil_b = new System.Windows.Forms.Button();
            this.Word_lv = new System.Windows.Forms.ListView();
            this.data_contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.up_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.down_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.del_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delall_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Kon_b = new System.Windows.Forms.Button();
            this.Ell_b = new System.Windows.Forms.Button();
            this.MakeWord_b = new System.Windows.Forms.Button();
            this.file_tb = new System.Windows.Forms.TextBox();
            this.proekt_tb = new System.Windows.Forms.TextBox();
            this.menuUp.SuspendLayout();
            this.data_contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuUp
            // 
            this.menuUp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.SpravkaToolStripMenuItem});
            this.menuUp.Location = new System.Drawing.Point(0, 0);
            this.menuUp.Name = "menuUp";
            this.menuUp.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuUp.Size = new System.Drawing.Size(681, 24);
            this.menuUp.TabIndex = 1;
            this.menuUp.Text = "menuUp";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem1,
            this.SaveToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.FileToolStripMenuItem.Text = "Файл";
            // 
            // OpenToolStripMenuItem1
            // 
            this.OpenToolStripMenuItem1.Name = "OpenToolStripMenuItem1";
            this.OpenToolStripMenuItem1.Size = new System.Drawing.Size(173, 22);
            this.OpenToolStripMenuItem1.Text = "Открыть расчет";
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.SaveToolStripMenuItem.Text = "Сохранить расчет";
            // 
            // SpravkaToolStripMenuItem
            // 
            this.SpravkaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.SpravkaToolStripMenuItem.Name = "SpravkaToolStripMenuItem";
            this.SpravkaToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.SpravkaToolStripMenuItem.Text = "Справка";
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.AboutToolStripMenuItem.Text = "О программе";
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.ExitToolStripMenuItem.Text = "Выход";
            // 
            // Cil_b
            // 
            this.Cil_b.Location = new System.Drawing.Point(14, 31);
            this.Cil_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Cil_b.Name = "Cil_b";
            this.Cil_b.Size = new System.Drawing.Size(187, 27);
            this.Cil_b.TabIndex = 2;
            this.Cil_b.Text = "Цилиндрическая обечайка";
            this.Cil_b.UseVisualStyleBackColor = true;
            this.Cil_b.Click += new System.EventHandler(this.Cil_b_Click);
            // 
            // Word_lv
            // 
            this.Word_lv.ContextMenuStrip = this.data_contextMenu;
            this.Word_lv.GridLines = true;
            this.Word_lv.HideSelection = false;
            this.Word_lv.Location = new System.Drawing.Point(362, 31);
            this.Word_lv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Word_lv.MultiSelect = false;
            this.Word_lv.Name = "Word_lv";
            this.Word_lv.Size = new System.Drawing.Size(305, 381);
            this.Word_lv.TabIndex = 3;
            this.Word_lv.UseCompatibleStateImageBehavior = false;
            this.Word_lv.View = System.Windows.Forms.View.List;
            // 
            // data_contextMenu
            // 
            this.data_contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.up_MenuItem,
            this.down_MenuItem,
            this.toolStripSeparator1,
            this.del_MenuItem,
            this.delall_MenuItem});
            this.data_contextMenu.Name = "data_contextMenu";
            this.data_contextMenu.Size = new System.Drawing.Size(140, 98);
            // 
            // up_MenuItem
            // 
            this.up_MenuItem.Name = "up_MenuItem";
            this.up_MenuItem.Size = new System.Drawing.Size(139, 22);
            this.up_MenuItem.Text = "Вверх";
            // 
            // down_MenuItem
            // 
            this.down_MenuItem.Name = "down_MenuItem";
            this.down_MenuItem.Size = new System.Drawing.Size(139, 22);
            this.down_MenuItem.Text = "Вниз";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
            // 
            // del_MenuItem
            // 
            this.del_MenuItem.Name = "del_MenuItem";
            this.del_MenuItem.Size = new System.Drawing.Size(139, 22);
            this.del_MenuItem.Text = "Удалить";
            // 
            // delall_MenuItem
            // 
            this.delall_MenuItem.Name = "delall_MenuItem";
            this.delall_MenuItem.Size = new System.Drawing.Size(139, 22);
            this.delall_MenuItem.Text = "Удалить все";
            // 
            // Kon_b
            // 
            this.Kon_b.Location = new System.Drawing.Point(15, 66);
            this.Kon_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Kon_b.Name = "Kon_b";
            this.Kon_b.Size = new System.Drawing.Size(186, 27);
            this.Kon_b.TabIndex = 4;
            this.Kon_b.Text = "Коническая обечайка";
            this.Kon_b.UseVisualStyleBackColor = true;
            this.Kon_b.Click += new System.EventHandler(this.Kon_b_Click);
            // 
            // Ell_b
            // 
            this.Ell_b.Location = new System.Drawing.Point(15, 100);
            this.Ell_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Ell_b.Name = "Ell_b";
            this.Ell_b.Size = new System.Drawing.Size(186, 27);
            this.Ell_b.TabIndex = 5;
            this.Ell_b.Text = "Эллиптическое днище";
            this.Ell_b.UseVisualStyleBackColor = true;
            this.Ell_b.Click += new System.EventHandler(this.Ell_b_Click);
            // 
            // MakeWord_b
            // 
            this.MakeWord_b.Location = new System.Drawing.Point(362, 476);
            this.MakeWord_b.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MakeWord_b.Name = "MakeWord_b";
            this.MakeWord_b.Size = new System.Drawing.Size(114, 27);
            this.MakeWord_b.TabIndex = 6;
            this.MakeWord_b.Text = "Вывести в Word";
            this.MakeWord_b.UseVisualStyleBackColor = true;
            this.MakeWord_b.Click += new System.EventHandler(this.MakeWord_b_Click);
            // 
            // file_tb
            // 
            this.file_tb.Location = new System.Drawing.Point(362, 447);
            this.file_tb.Name = "file_tb";
            this.file_tb.Size = new System.Drawing.Size(100, 23);
            this.file_tb.TabIndex = 7;
            // 
            // proekt_tb
            // 
            this.proekt_tb.Location = new System.Drawing.Point(362, 418);
            this.proekt_tb.Name = "proekt_tb";
            this.proekt_tb.Size = new System.Drawing.Size(189, 23);
            this.proekt_tb.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 532);
            this.Controls.Add(this.proekt_tb);
            this.Controls.Add(this.file_tb);
            this.Controls.Add(this.MakeWord_b);
            this.Controls.Add(this.Ell_b);
            this.Controls.Add(this.Kon_b);
            this.Controls.Add(this.Word_lv);
            this.Controls.Add(this.Cil_b);
            this.Controls.Add(this.menuUp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuUp;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.Text = "Расчет на прочность";
            this.menuUp.ResumeLayout(false);
            this.menuUp.PerformLayout();
            this.data_contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

