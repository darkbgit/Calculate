
namespace calc
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SpravkaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cil_b = new System.Windows.Forms.Button();
            this.Word_lv = new System.Windows.Forms.ListView();
            this.kon_b = new System.Windows.Forms.Button();
            this.ell_b = new System.Windows.Forms.Button();
            this.MakeWord_b = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.SpravkaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(584, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
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
            this.OpenToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.OpenToolStripMenuItem1.Text = "Открыть расчет";
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
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
            // cil_b
            // 
            this.cil_b.Location = new System.Drawing.Point(12, 27);
            this.cil_b.Name = "cil_b";
            this.cil_b.Size = new System.Drawing.Size(160, 23);
            this.cil_b.TabIndex = 2;
            this.cil_b.Text = "Цилиндрическая обечайка";
            this.cil_b.UseVisualStyleBackColor = true;
            this.cil_b.Click += new System.EventHandler(this.cil_b_Click);
            // 
            // Word_lv
            // 
            this.Word_lv.GridLines = true;
            this.Word_lv.HideSelection = false;
            this.Word_lv.Location = new System.Drawing.Point(310, 27);
            this.Word_lv.MultiSelect = false;
            this.Word_lv.Name = "Word_lv";
            this.Word_lv.Size = new System.Drawing.Size(262, 327);
            this.Word_lv.TabIndex = 3;
            this.Word_lv.UseCompatibleStateImageBehavior = false;
            // 
            // kon_b
            // 
            this.kon_b.Location = new System.Drawing.Point(13, 57);
            this.kon_b.Name = "kon_b";
            this.kon_b.Size = new System.Drawing.Size(159, 23);
            this.kon_b.TabIndex = 4;
            this.kon_b.Text = "Коническая обечайка";
            this.kon_b.UseVisualStyleBackColor = true;
            // 
            // ell_b
            // 
            this.ell_b.Location = new System.Drawing.Point(13, 87);
            this.ell_b.Name = "ell_b";
            this.ell_b.Size = new System.Drawing.Size(159, 23);
            this.ell_b.TabIndex = 5;
            this.ell_b.Text = "Эллиптическое днище";
            this.ell_b.UseVisualStyleBackColor = true;
            // 
            // MakeWord_b
            // 
            this.MakeWord_b.Location = new System.Drawing.Point(310, 370);
            this.MakeWord_b.Name = "MakeWord_b";
            this.MakeWord_b.Size = new System.Drawing.Size(98, 23);
            this.MakeWord_b.TabIndex = 6;
            this.MakeWord_b.Text = "Вывести в Word";
            this.MakeWord_b.UseVisualStyleBackColor = true;
            this.MakeWord_b.Click += new System.EventHandler(this.MakeWord_b_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.MakeWord_b);
            this.Controls.Add(this.ell_b);
            this.Controls.Add(this.kon_b);
            this.Controls.Add(this.Word_lv);
            this.Controls.Add(this.cil_b);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Расчет на прочность";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button cil_b;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SpravkaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.Button kon_b;
        private System.Windows.Forms.Button ell_b;
        private System.Windows.Forms.Button MakeWord_b;
        internal System.Windows.Forms.ListView Word_lv;
    }
}

