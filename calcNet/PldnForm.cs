﻿using CalculateVessels.Data.PhysicalData;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalculateVessels
{
    public partial class PldnForm : Form
    {
        public PldnForm()
        {
            InitializeComponent();
        }

        private void Rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                type_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject("pldn" + rb.Text));
                Type_Draw(sender);
            }
        }

        private void PldnForm_Load(object sender, EventArgs e)
        {
            var steels = Physical.Gost34233_1.GetSteelsList();
            if (steels != null)
            {
                steel_cb.Items.AddRange(steels);
                steel_cb.SelectedIndex = 0;
            }

            Gost_cb.SelectedIndex = 0;
            type_pb.Image = (Bitmap)new ImageConverter().ConvertFrom
                (Data.Properties.Resources.pldn1);
            Type_Draw(rb1);
        }

        private void PldnForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is PldnForm)
            {
                if (this.Owner is MainForm {pdf: { }} main)
                {
                    main.pdf = null;
                }
            }
        }

        private void Cancel_b_Click(object sender, EventArgs e)
        {
            this.Hide();
            
        }

        private void Otv_cb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox cb)
            {
                otv_gb.Enabled = cb.Checked;
            }
        }

        private void Type_Draw(object sender)
        {
            RadioButton rb = sender as RadioButton;

            if (!rb.Checked) return;

            switch (Convert.ToInt32(rb.Text))
            {
                case 1:
                case 2:
                {
                    typePanel.Controls.Clear();

                    var lab_1_1 = new Label
                    {
                        AutoSize = true,
                        Text = "Внутренний диаметр смежного элемента, D:",
                        Location = new System.Drawing.Point(8, 8)
                    };
                    var tb1 = new TextBox
                    {
                        Name = "D_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 4)
                    };
                    var lab_1_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 8)
                    };

                    var lab_2_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Толщина стенки смежного элемента, s:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new System.Drawing.Point(8, 38)
                    };
                    var tb2 = new TextBox
                    {
                        Name = "s_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 34)
                    };
                    var lab_2_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 38)
                    };

                    var lab_3_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Катет сварного шва, а:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new System.Drawing.Point(8, 68)
                    };
                    var tb3 = new TextBox
                    {
                        Name = "a_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 64)
                    };
                    var lab_3_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 68)
                    };


                    typePanel.Controls.Add(lab_1_1);
                    typePanel.Controls.Add(tb1);
                    typePanel.Controls.Add(lab_1_2);
                    typePanel.Controls.Add(lab_2_1);
                    typePanel.Controls.Add(tb2);
                    typePanel.Controls.Add(lab_2_2);
                    typePanel.Controls.Add(lab_3_1);
                    typePanel.Controls.Add(tb3);
                    typePanel.Controls.Add(lab_3_2);

                    break;
                }

                case 3:
                case 4:
                case 5:
                {
                    typePanel.Controls.Clear();

                    var lab_1_1 = new Label
                    {
                        AutoSize = true,
                        Text = "Внутренний диаметр смежного элемента, D:",
                        Location = new System.Drawing.Point(8, 8)
                    };
                    var tb1 = new TextBox
                    {
                        Name = "D_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 4)
                    };
                    var lab_1_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 8)
                    };

                    var lab_2_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Толщина стенки смежного элемента, s:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new System.Drawing.Point(8, 38)
                    };
                    var tb2 = new TextBox
                    {
                        Name = "s_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 34)
                    };
                    var lab_2_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 38)
                    };

                    typePanel.Controls.Add(lab_1_1);
                    typePanel.Controls.Add(tb1);
                    typePanel.Controls.Add(lab_1_2);
                    typePanel.Controls.Add(lab_2_1);
                    typePanel.Controls.Add(tb2);
                    typePanel.Controls.Add(lab_2_2);

                    break;
                }

                case 6:
                    goto case 2;
                case 7:
                case 8:
                    goto case 5;
                case 9:
                {
                    typePanel.Controls.Clear();

                    var lab_1_1 = new Label
                    {
                        AutoSize = true,
                        Text = "Внутренний диаметр смежного элемента, D:",
                        Location = new System.Drawing.Point(8, 8)
                    };
                    var tb1 = new TextBox
                    {
                        Name = "D_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 4)
                    };
                    var lab_1_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 8)
                    };

                    var lab_2_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Толщина стенки смежного элемента, s:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new System.Drawing.Point(8, 38)
                    };
                    var tb2 = new TextBox
                    {
                        Name = "s_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 34)
                    };
                    var lab_2_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 38)
                    };

                    var lab_3_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Радиус выточки, r:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new System.Drawing.Point(8, 68)
                    };
                    var tb3 = new TextBox
                    {
                        Name = "r_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 64)
                    };
                    var lab_3_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 68)
                    };

                    var lab_4_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Высота выточки, h1:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new System.Drawing.Point(8, 98)
                    };
                    var tb4 = new TextBox
                    {
                        Name = "h1_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 94)
                    };
                    var lab_4_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 98)
                    };

                    typePanel.Controls.Add(lab_1_1);
                    typePanel.Controls.Add(tb1);
                    typePanel.Controls.Add(lab_1_2);
                    typePanel.Controls.Add(lab_2_1);
                    typePanel.Controls.Add(tb2);
                    typePanel.Controls.Add(lab_2_2);
                    typePanel.Controls.Add(lab_3_1);
                    typePanel.Controls.Add(tb3);
                    typePanel.Controls.Add(lab_3_2);
                    typePanel.Controls.Add(lab_4_1);
                    typePanel.Controls.Add(tb4);
                    typePanel.Controls.Add(lab_4_2);

                    break;
                }
                case 10:
                {
                    typePanel.Controls.Clear();

                    var lab_1_1 = new Label
                    {
                        AutoSize = true,
                        Text = "Внутренний диаметр смежного элемента, D:",
                        Location = new System.Drawing.Point(8, 8)
                    };
                    var tb1 = new TextBox
                    {
                        Name = "D_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 4)
                    };
                    var lab_1_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 8)
                    };

                    var lab_2_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Толщина стенки смежного элемента, s:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new System.Drawing.Point(8, 38)
                    };
                    var tb2 = new TextBox
                    {
                        Name = "s_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 34)
                    };
                    var lab_2_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 38)
                    };

                    var lab_3_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Радиус выточки, r:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new System.Drawing.Point(8, 68)
                    };
                    var tb3 = new TextBox
                    {
                        Name = "r_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 64)
                    };
                    var lab_3_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 68)
                    };

                    var lab_4_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Угол выхода выточки, γ:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new System.Drawing.Point(8, 98)
                    };
                    var tb4 = new TextBox
                    {
                        Name = "gamma_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 94)
                    };
                    var lab_4_2 = new Label
                    {
                        Text = "°",
                        Location = new System.Drawing.Point(318, 98)
                    };

                    typePanel.Controls.Add(lab_1_1);
                    typePanel.Controls.Add(tb1);
                    typePanel.Controls.Add(lab_1_2);
                    typePanel.Controls.Add(lab_2_1);
                    typePanel.Controls.Add(tb2);
                    typePanel.Controls.Add(lab_2_2);
                    typePanel.Controls.Add(lab_3_1);
                    typePanel.Controls.Add(tb3);
                    typePanel.Controls.Add(lab_3_2);
                    typePanel.Controls.Add(lab_4_1);
                    typePanel.Controls.Add(tb4);
                    typePanel.Controls.Add(lab_4_2);

                    break;
                }
                case 11:
                    goto case 9;
                case 12:
                {
                    typePanel.Controls.Clear();

                    var lab_1_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Диаметр утоненной части, D2:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new System.Drawing.Point(8, 8)
                    };
                    var tb1 = new TextBox
                    {
                        Name = "D2_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 4)
                    };
                    var lab_1_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 8)
                    };

                    var lab_2_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Диаметр болтовой окружности, D3:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(8, 38)
                    };
                    var tb2 = new TextBox
                    {
                        Name = "D3_tb",
                        Size = new Size(46, 23),
                        Location = new Point(264, 34)
                    };
                    var lab_2_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 38)
                    };

                    var lab_3_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Толщина в зоне утонения, s2:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(8, 68)
                    };
                    var tb3 = new TextBox
                    {
                        Name = "s2_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 64)
                    };
                    var lab_3_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 68)
                    };

                    typePanel.Controls.Add(lab_1_1);
                    typePanel.Controls.Add(tb1);
                    typePanel.Controls.Add(lab_1_2);
                    typePanel.Controls.Add(lab_2_1);
                    typePanel.Controls.Add(tb2);
                    typePanel.Controls.Add(lab_2_2);
                    typePanel.Controls.Add(lab_3_1);
                    typePanel.Controls.Add(tb3);
                    typePanel.Controls.Add(lab_3_2);

                    break;
                }
                case 13:
                {
                    typePanel.Controls.Clear();

                    var lab_1_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Диаметр утоненной части, D2:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new System.Drawing.Point(8, 8)
                    };
                    var tb1 = new TextBox
                    {
                        Name = "D2_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 4)
                    };
                    var lab_1_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 8)
                    };

                    var lab_2_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Расчетный диаметр прокладки, Dсп:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(8, 38)
                    };
                    var tb2 = new TextBox
                    {
                        Name = "Dsp_tb",
                        Size = new Size(46, 23),
                        Location = new Point(264, 34)
                    };
                    var lab_2_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 38)
                    };

                    var lab_3_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Толщина в зоне утонения, s2:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(8, 68)
                    };
                    var tb3 = new TextBox
                    {
                        Name = "s2_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 64)
                    };
                    var lab_3_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 68)
                    };

                    typePanel.Controls.Add(lab_1_1);
                    typePanel.Controls.Add(tb1);
                    typePanel.Controls.Add(lab_1_2);
                    typePanel.Controls.Add(lab_2_1);
                    typePanel.Controls.Add(tb2);
                    typePanel.Controls.Add(lab_2_2);
                    typePanel.Controls.Add(lab_3_1);
                    typePanel.Controls.Add(tb3);
                    typePanel.Controls.Add(lab_3_2);

                    break;
                }
                case 14:
                {
                    typePanel.Controls.Clear();

                    var lab_1_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Диаметр утоненной части, D2:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new System.Drawing.Point(8, 8)
                    };
                    var tb1 = new TextBox
                    {
                        Name = "D2_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 4)
                    };
                    var lab_1_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 8)
                    };

                    var lab_2_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Диаметр болтовой окружности, D3:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(8, 38)
                    };
                    var tb2 = new TextBox
                    {
                        Name = "D3_tb",
                        Size = new Size(46, 23),
                        Location = new Point(264, 34)
                    };
                    var lab_2_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 38)
                    };

                    var lab_3_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Расчетный диаметр прокладки, Dсп:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(8, 68)
                    };
                    var tb3 = new TextBox
                    {
                        Name = "Dsp_tb",
                        Size = new Size(46, 23),
                        Location = new Point(264, 64)
                    };
                    var lab_3_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 68)
                    };

                    var lab_4_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Толщина в зоне утонения, s2:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(8, 98)
                    };
                    var tb4 = new TextBox
                    {
                        Name = "s2_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 94)
                    };
                    var lab_4_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 98)
                    };

                    var lab_5_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Толщина вне уплотнения, s3:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(8, 128)
                    };
                    var tb5 = new TextBox
                    {
                        Name = "s3_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 124)
                    };
                    var lab_5_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 128)
                    };

                    typePanel.Controls.Add(lab_1_1);
                    typePanel.Controls.Add(tb1);
                    typePanel.Controls.Add(lab_1_2);
                    typePanel.Controls.Add(lab_2_1);
                    typePanel.Controls.Add(tb2);
                    typePanel.Controls.Add(lab_2_2);
                    typePanel.Controls.Add(lab_3_1);
                    typePanel.Controls.Add(tb3);
                    typePanel.Controls.Add(lab_3_2);
                    typePanel.Controls.Add(lab_4_1);
                    typePanel.Controls.Add(tb4);
                    typePanel.Controls.Add(lab_4_2);
                    typePanel.Controls.Add(lab_5_1);
                    typePanel.Controls.Add(tb5);
                    typePanel.Controls.Add(lab_5_2);

                    break;
                }
                case 15:
                {
                    typePanel.Controls.Clear();

                    var lab_1_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Диаметр утоненной части, D2:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new System.Drawing.Point(8, 8)
                    };
                    var tb1 = new TextBox
                    {
                        Name = "D2_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 4)
                    };
                    var lab_1_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 8)
                    };

                    var lab_2_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Диаметр болтовой окружности, D3:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(8, 38)
                    };
                    var tb2 = new TextBox
                    {
                        Name = "D3_tb",
                        Size = new Size(46, 23),
                        Location = new Point(264, 34)
                    };
                    var lab_2_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 38)
                    };

                    var lab_3_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Расчетный диаметр прокладки, Dсп:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(8, 68)
                    };
                    var tb3 = new TextBox
                    {
                        Name = "Dsp_tb",
                        Size = new Size(46, 23),
                        Location = new Point(264, 64)
                    };
                    var lab_3_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 68)
                    };

                    var lab_4_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Толщина в зоне утонения, s2:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(8, 98)
                    };
                    var tb4 = new TextBox
                    {
                        Name = "s2_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 94)
                    };
                    var lab_4_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 98)
                    };

                    var lab_5_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Толщина вне уплотнения, s3:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(8, 128)
                    };
                    var tb5 = new TextBox
                    {
                        Name = "s3_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 124)
                    };
                    var lab_5_2 = new Label
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 128)
                    };

                    var lab_6_1 = new Label
                    {
                        Size = new Size(252, 15),
                        Text = "Ширина паза под прокладку, s4:",
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(8, 158)
                    };
                    TextBox tb6 = new()
                    {
                        Name = "s4_tb",
                        Size = new Size(46, 23),
                        Location = new System.Drawing.Point(264, 154)
                    };
                    Label lab_6_2 = new()
                    {
                        Text = "мм",
                        Location = new System.Drawing.Point(318, 158)
                    };

                    typePanel.Controls.Add(lab_1_1);
                    typePanel.Controls.Add(tb1);
                    typePanel.Controls.Add(lab_1_2);
                    typePanel.Controls.Add(lab_2_1);
                    typePanel.Controls.Add(tb2);
                    typePanel.Controls.Add(lab_2_2);
                    typePanel.Controls.Add(lab_3_1);
                    typePanel.Controls.Add(tb3);
                    typePanel.Controls.Add(lab_3_2);
                    typePanel.Controls.Add(lab_4_1);
                    typePanel.Controls.Add(tb4);
                    typePanel.Controls.Add(lab_4_2);
                    typePanel.Controls.Add(lab_5_1);
                    typePanel.Controls.Add(tb5);
                    typePanel.Controls.Add(lab_5_2);
                    typePanel.Controls.Add(lab_6_1);
                    typePanel.Controls.Add(tb6);
                    typePanel.Controls.Add(lab_6_2);

                    break;
                }
            }
        }
    }
}
