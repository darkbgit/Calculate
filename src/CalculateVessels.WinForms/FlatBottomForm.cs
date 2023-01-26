using CalculateVessels.Core.Bottoms.Enums;
using CalculateVessels.Core.Bottoms.FlatBottom;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.PhysicalData.Gost34233_1;
using CalculateVessels.Data.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels
{
    public partial class FlatBottomForm : Form
    {
        private FlatBottomInputData _inputData;

        public FlatBottomForm()
        {
            InitializeComponent();
        }

        public IInputData InputData => _inputData;


        private void Rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton { Checked: true } rb)
            {
                type_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject("pldn" + rb.Text));
                Type_Draw(sender);
            }
        }

        private void FlatBottomForm_Load(object sender, EventArgs e)
        {
            var steels = Gost34233_1.GetSteelsList()?.ToArray();
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

        private void FlatBottomForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is FlatBottomForm)
            {
                if (this.Owner is MainForm { pdf: { } } main)
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
            var rb = sender as RadioButton;

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

                        var lab_5_1 = new Label
                        {
                            Size = new Size(252, 15),
                            Text = "Толщина, s2:",
                            TextAlign = ContentAlignment.MiddleRight,
                            Location = new Point(8, 128)
                        };
                        var tb5 = new TextBox
                        {
                            Name = "s2_tb",
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
                case 11:
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
                case 13:
                    goto case 9;
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

        private void PreCalc_btn_Click(object sender, EventArgs e)
        {
            c_tb.Text = "";
            scalc_l.Text = "";
            p_d_l.Text = "";
            calc_btn.Enabled = false;

            _inputData = new FlatBottomInputData();

            var dataInErr = new List<string>();

            //t
            {
                if (double.TryParse(t_tb.Text, NumberStyles.Integer,
                        CultureInfo.InvariantCulture, out var t))
                {
                    _inputData.t = t;
                }
                else
                {
                    dataInErr.Add("t неверный ввод");
                }
            }

            //p
            {
                if (double.TryParse(p_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var p))
                {
                    _inputData.p = p;
                }
                else
                {
                    dataInErr.Add("p неверный ввод");
                }
            }

            //steel
            _inputData.Steel = steel_cb.Text;

            //
            //_cylindricalShellDataIn.IsPressureIn = vn_rb.Checked;

            //[σ]
            if (sigmaHandle_cb.Checked)
            {
                if (double.TryParse(sigma_d_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var sigmaAllow))
                {
                    _inputData.SigmaAllow = sigmaAllow;
                }
                else
                {
                    dataInErr.Add("[σ] неверный ввод");
                }
            }

            //if (!_inputData.IsPressureIn)
            //{
            //    //E
            //    if (EHandle_cb.Checked)
            //    {
            //        if (double.TryParse(sigma_d_tb.Text, NumberStyles.AllowDecimalPoint,
            //            CultureInfo.InvariantCulture, out var E))
            //        {
            //            _inputData.E = E;
            //        }
            //        else
            //        {
            //            dataInErr.Add("E неверный ввод");
            //        }
            //    }
            //}


            //fi
            {
                if (double.TryParse(fi_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var fi))
                {
                    _inputData.fi = fi;
                }
                else
                {
                    dataInErr.Add("φ неверный ввод");
                }
            }

            //c1
            {
                if (double.TryParse(c1_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var c1))
                {
                    _inputData.c1 = c1;
                }
                else
                {
                    dataInErr.Add("c1 неверный ввод");
                }
            }

            //c2
            {
                if (c2_tb.Text == "")
                {
                    _inputData.c2 = 0;
                }
                else if (double.TryParse(c2_tb.Text, NumberStyles.AllowDecimalPoint,
                             CultureInfo.InvariantCulture, out var c2))
                {
                    _inputData.c2 = c2;
                }
                else
                {
                    dataInErr.Add("c2 неверный ввод");
                }
            }

            //c3
            {
                if (c3_tb.Text == "")
                {
                    _inputData.c3 = 0;
                }
                else if (double.TryParse(c3_tb.Text, NumberStyles.AllowDecimalPoint,
                             CultureInfo.InvariantCulture, out var c3))
                {
                    _inputData.c3 = c3;
                }
                else
                {
                    dataInErr.Add("c3 неверный ввод");
                }
            }

            _inputData.Type =
                Convert.ToInt32(type_gb.Controls
                    .OfType<RadioButton>()
                    .FirstOrDefault(rb => rb.Checked)?.Text);

            switch (_inputData.Type)
            {
                case 1:
                case 2:
                    //D
                    {
                        var valueD = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "D_tb")?.Text;
                        if (valueD == null)
                        {
                            dataInErr.Add("D невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(valueD, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var D))
                            {
                                _inputData.D = D;
                            }
                            else
                            {
                                dataInErr.Add("D неверный ввод");
                            }
                        }
                    }

                    //s
                    {
                        var values = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "s_tb")?.Text;
                        if (values == null)
                        {
                            dataInErr.Add("s невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(values, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var s))
                            {
                                _inputData.s = s;
                            }
                            else
                            {
                                dataInErr.Add("s неверный ввод");
                            }
                        }
                    }

                    //a
                    {
                        var valuea = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "a_tb")?.Text;
                        if (valuea == null)
                        {
                            dataInErr.Add("a невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(valuea, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var a))
                            {
                                _inputData.a = a;
                            }
                            else
                            {
                                dataInErr.Add("a неверный ввод");
                            }
                        }
                    }

                    break;
                case 3:
                case 4:
                case 5:
                    //D
                    {
                        var valueD = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "D_tb")?.Text;
                        if (valueD == null)
                        {
                            dataInErr.Add("D невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(valueD, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var D))
                            {
                                _inputData.D = D;
                            }
                            else
                            {
                                dataInErr.Add("D неверный ввод");
                            }
                        }
                    }

                    //s
                    {
                        var values = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "s_tb")?.Text;
                        if (values == null)
                        {
                            dataInErr.Add("s невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(values, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var s))
                            {
                                _inputData.s = s;
                            }
                            else
                            {
                                dataInErr.Add("s неверный ввод");
                            }
                        }
                    }
                    break;
                case 6:
                    goto case 2;
                case 7:
                case 8:
                    goto case 5;
                case 9:
                    //D
                    {
                        var valueD = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "D_tb")?.Text;
                        if (valueD == null)
                        {
                            dataInErr.Add("D невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(valueD, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var D))
                            {
                                _inputData.D = D;
                            }
                            else
                            {
                                dataInErr.Add("D неверный ввод");
                            }
                        }
                    }

                    //s
                    {
                        var values = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "s_tb")?.Text;
                        if (values == null)
                        {
                            dataInErr.Add("s невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(values, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var s))
                            {
                                _inputData.s = s;
                            }
                            else
                            {
                                dataInErr.Add("s неверный ввод");
                            }
                        }
                    }

                    //r
                    {
                        var valuer = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "r_tb")?.Text;
                        if (valuer == null)
                        {
                            dataInErr.Add("r невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(valuer, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var r))
                            {
                                _inputData.r = r;
                            }
                            else
                            {
                                dataInErr.Add("r неверный ввод");
                            }
                        }
                    }

                    //h1
                    {
                        var valueh1 = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "h1_tb")?.Text;
                        if (valueh1 == null)
                        {
                            dataInErr.Add("h1 невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(valueh1, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var h1))
                            {
                                _inputData.h1 = h1;
                            }
                            else
                            {
                                dataInErr.Add("h1 неверный ввод");
                            }
                        }
                    }
                    break;

                case 10:
                    //D
                    {
                        var valueD = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "D_tb")?.Text;
                        if (valueD == null)
                        {
                            dataInErr.Add("D невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(valueD, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var D))
                            {
                                _inputData.D = D;
                            }
                            else
                            {
                                dataInErr.Add("D неверный ввод");
                            }
                        }
                    }

                    //s
                    {
                        var values = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "s_tb")?.Text;
                        if (values == null)
                        {
                            dataInErr.Add("s невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(values, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var s))
                            {
                                _inputData.s = s;
                            }
                            else
                            {
                                dataInErr.Add("s неверный ввод");
                            }
                        }
                    }

                    //r
                    {
                        var valuer = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "r_tb")?.Text;
                        if (valuer == null)
                        {
                            dataInErr.Add("r невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(valuer, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var r))
                            {
                                _inputData.r = r;
                            }
                            else
                            {
                                dataInErr.Add("r неверный ввод");
                            }
                        }
                    }

                    //gamma
                    {
                        var valuegamma = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "gamma_tb")
                            ?.Text;
                        if (valuegamma == null)
                        {
                            dataInErr.Add("gamma невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(valuegamma, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var gamma))
                            {
                                _inputData.gamma = gamma;
                            }
                            else
                            {
                                dataInErr.Add("gamma неверный ввод");
                            }
                        }
                    }

                    //s2
                    {
                        var values2 = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "s2_tb")?.Text;
                        if (values2 == null)
                        {
                            dataInErr.Add("s2 невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(values2, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var s2))
                            {
                                _inputData.s2 = s2;
                            }
                            else
                            {
                                dataInErr.Add("s2 неверный ввод");
                            }
                        }
                    }

                    break;

                case 11:
                    //D2
                    {
                        var valueD2 = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "D2_tb")?.Text;
                        if (valueD2 == null)
                        {
                            dataInErr.Add("D2 невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(valueD2, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var D2))
                            {
                                _inputData.D2 = D2;
                            }
                            else
                            {
                                dataInErr.Add("D2 неверный ввод");
                            }
                        }
                    }

                    //D3
                    {
                        var valueD3 = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "D3_tb")?.Text;
                        if (valueD3 == null)
                        {
                            dataInErr.Add("D3 невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(valueD3, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var D3))
                            {
                                _inputData.D3 = D3;
                            }
                            else
                            {
                                dataInErr.Add("D3 неверный ввод");
                            }
                        }
                    }

                    //s2
                    {
                        var values2 = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "s2_tb")?.Text;
                        if (values2 == null)
                        {
                            dataInErr.Add("s2 невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(values2, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var s2))
                            {
                                _inputData.s2 = s2;
                            }
                            else
                            {
                                dataInErr.Add("s2 неверный ввод");
                            }
                        }
                    }

                    break;

                case 12:
                    //D2
                    {
                        var valueD2 = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "D2_tb")?.Text;
                        if (valueD2 == null)
                        {
                            dataInErr.Add("D2 невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(valueD2, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var D2))
                            {
                                _inputData.D2 = D2;
                            }
                            else
                            {
                                dataInErr.Add("D2 неверный ввод");
                            }
                        }
                    }

                    //Dcp
                    {
                        var valueDcp = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "Dcp_tb")?.Text;
                        if (valueDcp == null)
                        {
                            dataInErr.Add("Dcp невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(valueDcp, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var Dcp))
                            {
                                _inputData.Dcp = Dcp;
                            }
                            else
                            {
                                dataInErr.Add("Dcp неверный ввод");
                            }
                        }
                    }

                    //s2
                    {
                        var values2 = typePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "s2_tb")?.Text;
                        if (values2 == null)
                        {
                            dataInErr.Add("s2 невозможно найти");
                        }
                        else
                        {
                            if (double.TryParse(values2, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var s2))
                            {
                                _inputData.s2 = s2;
                            }
                            else
                            {
                                dataInErr.Add("s2 неверный ввод");
                            }
                        }
                    }

                    break;
                case 13:
                case 14:
                case 15:
                    dataInErr.Add("Type 13, 14, 15 unsupported");
                    break;
                default:
                    dataInErr.Add("Type error");
                    break;
            }

            if (!hole_cb.Checked)
            {
                _inputData.Hole = HoleInFlatBottom.WithoutHole;
            }
            else
            {
                if (double.TryParse(holed_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var d))
                {
                    if (oneHole_rb.Checked)
                    {
                        _inputData.Hole = HoleInFlatBottom.OneHole;
                        _inputData.d = d;
                    }
                    else
                    {
                        _inputData.Hole = HoleInFlatBottom.MoreThenOneHole;
                        _inputData.di = d;
                    }
                }
                else
                {
                    dataInErr.Add("d неверный ввод");
                }
            }

            //s1
            {
                if (s1_tb.Text == "")
                {
                    _inputData.s1 = 0;
                }
                else if (double.TryParse(s1_tb.Text, NumberStyles.AllowDecimalPoint,
                             CultureInfo.InvariantCulture, out var s1))
                {
                    _inputData.s1 = s1;
                }
                else
                {
                    dataInErr.Add("s1 неверный ввод");
                }
            }


            var isError = dataInErr.Any() || !InputData.IsDataGood;

            if (isError)
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_inputData.ErrorList)));
                return;
            }

            IElement bottom = new FlatBottom(_inputData);

            try
            {
                bottom.Calculate();
            }
            catch (CalculateException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (bottom.IsCalculated)
            {
                if (bottom.CalculatedData.ErrorList.Any())
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, bottom.CalculatedData.ErrorList));
                }

                calc_btn.Enabled = true;
                scalc_l.Text = $@"sp={((FlatBottomCalculatedData)bottom.CalculatedData).s1:f3} мм";
                p_d_l.Text =
                    $@"pd={((FlatBottomCalculatedData)bottom.CalculatedData).p_d:f2} МПа";
                MessageBox.Show(Resources.CalcComplete);
            }
        }

        private void Calc_b_Click(object sender, EventArgs e)
        {
            scalc_l.Text = "";

            List<string> dataInErr = new();

            //name
            _inputData.Name = name_tb.Text;

            //s1
            {
                if (s1_tb.Text == "")
                {
                    _inputData.s1 = 0;
                }
                else if (double.TryParse(s1_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var s1))
                {
                    _inputData.s1 = s1;
                }
                else
                {
                    dataInErr.Add("s1 неверный ввод");
                }
            }


            var isError = dataInErr.Any() || InputData.IsDataGood;

            if (isError)
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_inputData.ErrorList)));
                return;
            }

            IElement bottom = new FlatBottom(_inputData);

            try
            {
                bottom.Calculate();
            }
            catch (CalculateException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (Owner is MainForm main)
            {
                main.Word_lv.Items.Add(bottom.ToString());
                main.ElementsCollection.Add(bottom);

                //_form.Hide();
            }
            else
            {
                MessageBox.Show("MainForm Error");
            }

            if (bottom.IsCalculated)
            {
                if (bottom.CalculatedData.ErrorList.Any())
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, bottom.CalculatedData.ErrorList));
                }

                calc_btn.Enabled = true;
                scalc_l.Text = $@"sp={((FlatBottomCalculatedData)bottom.CalculatedData).s1:f3} мм";
                p_d_l.Text =
                    $@"pd={((FlatBottomCalculatedData)bottom.CalculatedData).p_d:f2} МПа";
                MessageBox.Show(Resources.CalcComplete);
            }
        }
    }
}
