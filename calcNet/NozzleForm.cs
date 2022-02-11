using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Models;
using CalculateVessels.Core.Shells.DataIn;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Core.Shells.Nozzle;
using CalculateVessels.Core.Shells.Nozzle.Enums;
using CalculateVessels.Data.PhysicalData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Data.Properties;

namespace CalculateVessels
{
    public partial class NozzleForm : Form
    {
        private const string PERPENDICULAR = "Перпендикулярно\n поверхности";
        private const string TRANSVERSELY = "В плоскости\nпопер. сечения";
        private const string OFFSET = "Смещенный";
        private const string SLANTED = "Наклонный";

        private NozzleInputData _nozzleData;

        private readonly IElement _shellElement;

        private readonly ShellDataIn _shellDataIn;

        public NozzleForm(IElement shellElement)
        {
            InitializeComponent();
            _shellElement = shellElement;
            _shellDataIn = (ShellDataIn)shellElement.CalculatedData.InputData;
        }

        public IInputData DataIn => _nozzleData;


        private void Vid_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                int i = Convert.ToInt32(rb.Text[0].ToString());
                vid_pictureBox.Image =
                    (Bitmap)new ImageConverter()
                    .ConvertFrom(CalculateVessels.Data.Properties.Resources.ResourceManager.GetObject("Nozzle" + i.ToString()));

                ring_gb.Enabled = false;
                in_gb.Enabled = false;
                switch (i)
                {
                    case (int)NozzleKind.ImpassWithRing:
                        ring_gb.Enabled = true;
                        break;
                    case (int)NozzleKind.PassWithoutRing:
                        in_gb.Enabled = true;
                        break;
                    case (int)NozzleKind.PassWithRing:
                    case (int)NozzleKind.WithRingAndInPart:
                        ring_gb.Enabled = true;
                        in_gb.Enabled = true;
                        break;
                }
            }
        }

        private void Place_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton { Checked: true } rb)
            {
                switch (((ShellDataIn)_shellElement.CalculatedData.InputData).ShellType)
                {
                    case ShellType.Cylindrical:
                        {
                            switch (rb.Text)
                            {
                                case PERPENDICULAR:
                                    {
                                        place_pb.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.CylRadial);
                                        break;
                                    }
                                case TRANSVERSELY:
                                    {
                                        place_pb.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.CylAxial);
                                        break;
                                    }
                                //case "Смещенный":
                                //    {
                                //        place_pb.Image =
                                //            (Bitmap)Properties.Resources.ResourceManager.GetObject("CylOffset");
                                //        break;
                                //    }
                                case SLANTED:
                                    {
                                        place_pb.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.CylTilted);
                                        break;
                                    }
                            }
                            Place_Draw(sender);

                            break;
                        }
                    case ShellType.Elliptical:
                        {
                            switch (rb.Text)
                            {
                                case PERPENDICULAR:
                                    {
                                        if (Controls["place_gb"].Controls["corPn"] == null ||
                                            (Controls["place_gb"].Controls["corPn"]
                                                                 .Controls["placePolar_rb"] as RadioButton).Checked == true)
                                        {
                                            place_pb.Image = (Bitmap)new ImageConverter()
                                                .ConvertFrom(CalculateVessels.Data.Properties.Resources.EllRadial);
                                        }
                                        else if ((Controls["place_gb"].Controls["corPn"].Controls["placeDekart_rb"] as RadioButton).Checked == true)
                                        {
                                            place_pb.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.EllRadialDekart);
                                        }
                                        break;
                                    }
                                case OFFSET:
                                    {
                                        if (Controls["place_gb"].Controls["corPn"] == null ||
                                            (Controls["place_gb"].Controls["corPn"]
                                                                .Controls["placePolar_rb"] as RadioButton).Checked == true)
                                        {
                                            place_pb.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.EllVert);
                                        }
                                        else if ((Controls["place_gb"].Controls["corPn"].Controls["placeDekart_rb"] as RadioButton).Checked == true)
                                        {
                                            place_pb.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.EllVertDekart);
                                        }
                                        break;
                                    }
                                    //case "Наклонный":
                                    //    {
                                    //        if (Controls["place_gb"].Controls["corPn"] == null ||
                                    //            (Controls["place_gb"].Controls["corPn"]
                                    //                                .Controls["placePolar_rb"] as RadioButton).Checked == true)
                                    //        {
                                    //            place_pb.Image = Properties.Resources.EllTilted;
                                    //        }
                                    //        else if ((Controls["place_gb"].Controls["corPn"].Controls["placeDekart_rb"] as RadioButton).Checked == true)
                                    //        {
                                    //            place_pb.Image = Properties.Resources.EllTiltedDekart;
                                    //        }
                                    //        break;
                                    //    }
                            }
                            Place_Draw(sender);
                            break;
                        }
                }
            }
        }

        private void PlaceCoordinat_rb_CheckedChanged(object sender, EventArgs e)
        {
            //RadioButton rb = sender as RadioButton;
            //if (rb == null || (rb.Checked && rb.Text == "Полярная"))
            //{
            //    place_pb.Image = (Bitmap)calcNet.Properties.Resources.EllRadial;
            //}

            Place_Draw(sender);
        }

        private void EllRadialDraw()
        {
            place_gb.Controls["pn"].Dispose();
            Panel pn = new()
            {
                Name = "pn",
                Location = new Point(2, 15),
                Size = new Size(300, 310)
            };

            var lab_1_1 = new Label
            {
                AutoSize = true,
                Text = "Смещение, Rш:",
                Location = new System.Drawing.Point(8, 130)
            };
            var tb1 = new TextBox
            {
                Name = "Rsh_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 150)
            };
            var lab_1_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 154)
            };

            var lab_2_1 = new Label
            {
                AutoSize = true,
                Text = "Угол смещения оси, θ:",
                Location = new System.Drawing.Point(8, 175)
            };
            var tb2 = new TextBox
            {
                Name = "theta_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 195)
            };
            var lab_2_2 = new Label
            {
                Text = "°",
                Location = new System.Drawing.Point(115, 199)
            };

            place_pb.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.EllRadial);

            place_gb.Controls.Add(pn);

            place_gb.Controls["pn"].Controls.Add(lab_1_1);
            place_gb.Controls["pn"].Controls.Add(tb1);
            place_gb.Controls["pn"].Controls.Add(lab_1_2);
            place_gb.Controls["pn"].Controls.Add(lab_2_1);
            place_gb.Controls["pn"].Controls.Add(tb2);
            place_gb.Controls["pn"].Controls.Add(lab_2_2);
        }

        private void EllRadialDekartDraw()
        {
            place_gb.Controls["pn"].Dispose();
            var pn = new Panel
            {
                Name = "pn",
                Location = new Point(2, 15),
                Size = new Size(300, 310)
            };

            var lab_1_1 = new Label
            {
                AutoSize = true,
                Text = "Координата, x0:",
                Location = new System.Drawing.Point(8, 130)
            };
            var tb1 = new TextBox
            {
                Name = "x0_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 150)
            };
            var lab_1_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 154)
            };

            var lab_2_1 = new Label
            {
                AutoSize = true,
                Text = "Координата, y0:",
                Location = new System.Drawing.Point(8, 175)
            };
            var tb2 = new TextBox
            {
                Name = "y0_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 195)
            };
            var lab_2_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 199)
            };
            place_pb.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.EllRadialDekart);

            place_gb.Controls.Add(pn);

            place_gb.Controls["pn"].Controls.Add(lab_1_1);
            place_gb.Controls["pn"].Controls.Add(tb1);
            place_gb.Controls["pn"].Controls.Add(lab_1_2);
            place_gb.Controls["pn"].Controls.Add(lab_2_1);
            place_gb.Controls["pn"].Controls.Add(tb2);
            place_gb.Controls["pn"].Controls.Add(lab_2_2);
        }

        private void EllVertDraw()
        {
            place_gb.Controls["pn"].Dispose();
            var pn = new Panel
            {
                Name = "pn",
                Location = new Point(2, 15),
                Size = new Size(300, 310)
            };

            var lab_1_1 = new Label
            {
                AutoSize = true,
                Text = "Смещение, Rш:",
                Location = new System.Drawing.Point(8, 130)
            };
            var tb1 = new TextBox
            {
                Name = "Rsh_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 150)
            };
            var lab_1_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 154)
            };

            var lab_2_1 = new Label
            {
                AutoSize = true,
                Text = "Угол смещения оси, θ:",
                Location = new System.Drawing.Point(8, 175)
            };
            var tb2 = new TextBox
            {
                Name = "theta_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 195)
            };
            var lab_2_2 = new Label
            {
                Text = "°",
                Location = new System.Drawing.Point(115, 199)
            };
            place_pb.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.EllVert);

            place_gb.Controls.Add(pn);

            place_gb.Controls["pn"].Controls.Add(lab_1_1);
            place_gb.Controls["pn"].Controls.Add(tb1);
            place_gb.Controls["pn"].Controls.Add(lab_1_2);
            place_gb.Controls["pn"].Controls.Add(lab_2_1);
            place_gb.Controls["pn"].Controls.Add(tb2);
            place_gb.Controls["pn"].Controls.Add(lab_2_2);
        }

        private void EllVertDekartDraw()
        {
            place_gb.Controls["pn"].Dispose();
            Panel pn = new()
            {
                Name = "pn",
                Location = new Point(2, 15),
                Size = new Size(300, 310)
            };

            var lab_1_1 = new Label
            {
                AutoSize = true,
                Text = "Координата, x0:",
                Location = new System.Drawing.Point(8, 130)
            };
            var tb1 = new TextBox
            {
                Name = "x0_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 150)
            };
            var lab_1_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 154)
            };

            var lab_2_1 = new Label
            {
                AutoSize = true,
                Text = "Координата, y0:",
                Location = new System.Drawing.Point(8, 175)
            };
            var tb2 = new TextBox
            {
                Name = "y0_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 195)
            };
            var lab_2_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 199)
            };
            place_pb.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.EllVertDekart);

            place_gb.Controls.Add(pn);

            place_gb.Controls["pn"].Controls.Add(lab_1_1);
            place_gb.Controls["pn"].Controls.Add(tb1);
            place_gb.Controls["pn"].Controls.Add(lab_1_2);
            place_gb.Controls["pn"].Controls.Add(lab_2_1);
            place_gb.Controls["pn"].Controls.Add(tb2);
            place_gb.Controls["pn"].Controls.Add(lab_2_2);
        }

        private void EllTiltedDraw()
        {
            place_gb.Controls["pn"].Dispose();
            Panel pn = new()
            {
                Name = "pn",
                Location = new Point(2, 15),
                Size = new Size(300, 310)
            };

            var lab_1_1 = new Label
            {
                AutoSize = true,
                Text = "Смещение, Rш:",
                Location = new System.Drawing.Point(8, 130)
            };
            var tb1 = new TextBox
            {
                Name = "Rsh_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 150)
            };
            var lab_1_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 154)
            };

            var lab_2_1 = new Label
            {
                AutoSize = true,
                Text = "Угол смещения оси, θ:",
                Location = new System.Drawing.Point(8, 175)
            };
            var tb2 = new TextBox
            {
                Name = "theta_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 195)
            };
            var lab_2_2 = new Label
            {
                Text = "°",
                Location = new System.Drawing.Point(115, 199)
            };

            var lab_3_1 = new Label
            {
                AutoSize = true,
                Text = "Угол наклона оси, γ:",
                Location = new System.Drawing.Point(8, 220)
            };
            var tb3 = new TextBox
            {
                Name = "gamma_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 240)
            };
            var lab_3_2 = new Label
            {
                Text = "°",
                Location = new System.Drawing.Point(115, 244)
            };

            place_pb.Image = (Bitmap)new ImageConverter()
                .ConvertFrom(CalculateVessels.Data.Properties.Resources.EllTilted);

            place_gb.Controls.Add(pn);

            place_gb.Controls["pn"].Controls.Add(lab_1_1);
            place_gb.Controls["pn"].Controls.Add(tb1);
            place_gb.Controls["pn"].Controls.Add(lab_1_2);
            place_gb.Controls["pn"].Controls.Add(lab_2_1);
            place_gb.Controls["pn"].Controls.Add(tb2);
            place_gb.Controls["pn"].Controls.Add(lab_2_2);
            place_gb.Controls["pn"].Controls.Add(lab_3_1);
            place_gb.Controls["pn"].Controls.Add(tb3);
            place_gb.Controls["pn"].Controls.Add(lab_3_2);
        }

        private void EllTiltedDekartDraw()
        {
            place_gb.Controls["pn"].Dispose();
            Panel pn = new()
            {
                Name = "pn",
                Location = new Point(2, 15),
                Size = new Size(300, 310)
            };

            var lab_1_1 = new Label
            {
                AutoSize = true,
                Text = "Координата, x0:",
                Location = new System.Drawing.Point(8, 130)
            };
            var tb1 = new TextBox
            {
                Name = "x0_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 150)
            };
            var lab_1_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 154)
            };

            var lab_2_1 = new Label
            {
                AutoSize = true,
                Text = "Координата, y0:",
                Location = new System.Drawing.Point(8, 175)
            };
            var tb2 = new TextBox
            {
                Name = "y0_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 195)
            };
            var lab_2_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 199)
            };

            var lab_3_1 = new Label
            {
                AutoSize = true,
                Text = "Угол наклона оси, γ:",
                Location = new System.Drawing.Point(8, 220)
            };
            var tb3 = new TextBox
            {
                Name = "gamma_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 240)
            };
            var lab_3_2 = new Label
            {
                Text = "°",
                Location = new System.Drawing.Point(115, 244)
            };
            place_pb.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.EllTiltedDekart);

            place_gb.Controls.Add(pn);

            place_gb.Controls["pn"].Controls.Add(lab_1_1);
            place_gb.Controls["pn"].Controls.Add(tb1);
            place_gb.Controls["pn"].Controls.Add(lab_1_2);
            place_gb.Controls["pn"].Controls.Add(lab_2_1);
            place_gb.Controls["pn"].Controls.Add(tb2);
            place_gb.Controls["pn"].Controls.Add(lab_2_2);
            place_gb.Controls["pn"].Controls.Add(lab_3_1);
            place_gb.Controls["pn"].Controls.Add(tb3);
            place_gb.Controls["pn"].Controls.Add(lab_3_2);
        }

        private void Place_Draw(object sender)
        {
            var rb = sender as RadioButton;

            switch (_shellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        if (rb == null || (rb.Checked && rb.Text == PERPENDICULAR))
                        {
                            place_gb.Controls["pn"].Dispose();
                            Panel pn = new()
                            {
                                Name = "pn",
                                Location = new Point(2, 15),
                                Size = new Size(300, 310)
                            };
                            var lab_1_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Смещение, Lш:",
                                Location = new System.Drawing.Point(8, 130)
                            };
                            var tb1 = new TextBox
                            {
                                Name = "Lsh_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 150)
                            };
                            var lab_1_2 = new Label
                            {
                                Text = "м",
                                Location = new System.Drawing.Point(115, 154)
                            };

                            var lab_2_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Угол смещения оси, θ:",
                                Location = new System.Drawing.Point(8, 175)
                            };
                            var tb2 = new TextBox
                            {
                                Name = "theta_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 195)
                            };
                            var lab_2_2 = new Label
                            {
                                Text = "°",
                                Location = new System.Drawing.Point(115, 199)
                            };

                            place_gb.Controls.Add(pn);

                            place_gb.Controls["pn"].Controls.Add(lab_1_1);
                            place_gb.Controls["pn"].Controls.Add(tb1);
                            place_gb.Controls["pn"].Controls.Add(lab_1_2);
                            place_gb.Controls["pn"].Controls.Add(lab_2_1);
                            place_gb.Controls["pn"].Controls.Add(tb2);
                            place_gb.Controls["pn"].Controls.Add(lab_2_2);


                        }

                        else if (rb.Checked && rb.Text == TRANSVERSELY)
                        {
                            place_gb.Controls["pn"].Dispose();
                            Panel pn = new()
                            {
                                Name = "pn",
                                Location = new Point(2, 15),
                                Size = new Size(300, 310)
                            };
                            var lab_1_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Смещение, Lш:",
                                Location = new System.Drawing.Point(8, 130)
                            };
                            var tb1 = new TextBox
                            {
                                Name = "Lsh_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 150)
                            };
                            var lab_1_2 = new Label
                            {
                                Text = "м",
                                Location = new System.Drawing.Point(115, 154)
                            };

                            var lab_2_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Угол смещения оси, θ:",
                                Location = new System.Drawing.Point(8, 175)
                            };
                            var tb2 = new TextBox
                            {
                                Name = "theta_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 195)
                            };
                            var lab_2_2 = new Label
                            {
                                Text = "°",
                                Location = new System.Drawing.Point(115, 199)
                            };

                            var lab_3_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Угол, ψ:",
                                Location = new System.Drawing.Point(8, 220)
                            };
                            var tb3 = new TextBox
                            {
                                Name = "psi_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 240)
                            };
                            var lab_3_2 = new Label
                            {
                                Text = "°",
                                Location = new System.Drawing.Point(115, 244)
                            };

                            var lab_4_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Смещение, t:",
                                Location = new System.Drawing.Point(8, 265)
                            };
                            var tb4 = new TextBox
                            {
                                Name = "t_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 285)
                            };
                            var lab_4_2 = new Label
                            {
                                Text = "мм",
                                Location = new System.Drawing.Point(115, 289)
                            };
                            place_gb.Controls.Add(pn);

                            place_gb.Controls["pn"].Controls.Add(lab_1_1);
                            place_gb.Controls["pn"].Controls.Add(tb1);
                            place_gb.Controls["pn"].Controls.Add(lab_1_2);
                            place_gb.Controls["pn"].Controls.Add(lab_2_1);
                            place_gb.Controls["pn"].Controls.Add(tb2);
                            place_gb.Controls["pn"].Controls.Add(lab_2_2);
                            place_gb.Controls["pn"].Controls.Add(lab_3_1);
                            place_gb.Controls["pn"].Controls.Add(tb3);
                            place_gb.Controls["pn"].Controls.Add(lab_3_2);
                            place_gb.Controls["pn"].Controls.Add(lab_4_1);
                            place_gb.Controls["pn"].Controls.Add(tb4);
                            place_gb.Controls["pn"].Controls.Add(lab_4_2);
                        }

                        /*
                        //else if (rb.Checked && rb.Text == "Смещенный")
                        //{
                        //    place_gb.Controls["pn"].Dispose();
                        //    Panel pn = new Panel
                        //    {
                        //        Name = "pn",
                        //        Location = new Point(2, 15),
                        //        Size = new Size(300, 310)
                        //    };
                        //    Label lab_1_1 = new Label
                        //    {
                        //        AutoSize = true,
                        //        Text = "Смещение, Lш:",
                        //        Location = new System.Drawing.Point(8, 130)
                        //    };
                        //    TextBox tb1 = new TextBox
                        //    {
                        //        Name = "Lsh_tb",
                        //        Size = new Size(100, 20),
                        //        Location = new System.Drawing.Point(8, 150)
                        //    };
                        //    Label lab_1_2 = new Label
                        //    {
                        //        Text = "м",
                        //        Location = new System.Drawing.Point(115, 154)
                        //    };

                        //    Label lab_2_1 = new Label
                        //    {
                        //        AutoSize = true,
                        //        Text = "Угол смещения оси, θ:",
                        //        Location = new System.Drawing.Point(8, 175)
                        //    };
                        //    TextBox tb2 = new TextBox
                        //    {
                        //        Name = "theta_tb",
                        //        Size = new Size(100, 20),
                        //        Location = new System.Drawing.Point(8, 195)
                        //    };
                        //    Label lab_2_2 = new Label
                        //    {
                        //        Text = "°",
                        //        Location = new System.Drawing.Point(115, 199)
                        //    };

                        //    Label lab_3_1 = new Label
                        //    {
                        //        AutoSize = true,
                        //        Text = "Смещение, lсм:",
                        //        Location = new System.Drawing.Point(8, 220)
                        //    };
                        //    TextBox tb3 = new TextBox
                        //    {
                        //        Name = "lsm_tb",
                        //        Size = new Size(100, 20),
                        //        Location = new System.Drawing.Point(8, 240)
                        //    };
                        //    Label lab_3_2 = new Label
                        //    {
                        //        Text = "мм",
                        //        Location = new System.Drawing.Point(115, 244)
                        //    };


                        //    place_gb.Controls.Add(pn);

                        //    place_gb.Controls["pn"].Controls.Add(lab_1_1);
                        //    place_gb.Controls["pn"].Controls.Add(tb1);
                        //    place_gb.Controls["pn"].Controls.Add(lab_1_2);
                        //    place_gb.Controls["pn"].Controls.Add(lab_2_1);
                        //    place_gb.Controls["pn"].Controls.Add(tb2);
                        //    place_gb.Controls["pn"].Controls.Add(lab_2_2);
                        //    place_gb.Controls["pn"].Controls.Add(lab_3_1);
                        //    place_gb.Controls["pn"].Controls.Add(tb3);
                        //    place_gb.Controls["pn"].Controls.Add(lab_3_2);
                        //}
                        */

                        else if (rb.Checked && rb.Text == SLANTED)
                        {
                            place_gb.Controls["pn"].Dispose();
                            Panel pn = new()
                            {
                                Name = "pn",
                                Location = new Point(2, 15),
                                Size = new Size(300, 310)
                            };
                            var lab_1_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Смещение, Lш:",
                                Location = new System.Drawing.Point(8, 130)
                            };
                            var tb1 = new TextBox
                            {
                                Name = "Lsh_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 150)
                            };
                            var lab_1_2 = new Label
                            {
                                Text = "м",
                                Location = new System.Drawing.Point(115, 154)
                            };

                            var lab_2_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Угол смещения оси, θ:",
                                Location = new System.Drawing.Point(8, 175)
                            };
                            var tb2 = new TextBox
                            {
                                Name = "theta_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 195)
                            };
                            var lab_2_2 = new Label
                            {
                                Text = "°",
                                Location = new System.Drawing.Point(115, 199)
                            };

                            var lab_3_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Угол наклона оси, γ:",
                                Location = new System.Drawing.Point(8, 220)
                            };
                            var tb3 = new TextBox
                            {
                                Name = "gamma_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 240)
                            };
                            var lab_3_2 = new Label
                            {
                                Text = "°",
                                Location = new System.Drawing.Point(115, 244)
                            };

                            var lab_4_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Угол отклонения оси, ω:",
                                Location = new System.Drawing.Point(8, 265)
                            };
                            var tb4 = new TextBox
                            {
                                Name = "omega_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 285)
                            };
                            var lab_4_2 = new Label
                            {
                                Text = "°",
                                Location = new System.Drawing.Point(115, 289)
                            };
                            place_gb.Controls.Add(pn);

                            place_gb.Controls["pn"].Controls.Add(lab_1_1);
                            place_gb.Controls["pn"].Controls.Add(tb1);
                            place_gb.Controls["pn"].Controls.Add(lab_1_2);
                            place_gb.Controls["pn"].Controls.Add(lab_2_1);
                            place_gb.Controls["pn"].Controls.Add(tb2);
                            place_gb.Controls["pn"].Controls.Add(lab_2_2);
                            place_gb.Controls["pn"].Controls.Add(lab_3_1);
                            place_gb.Controls["pn"].Controls.Add(tb3);
                            place_gb.Controls["pn"].Controls.Add(lab_3_2);
                            place_gb.Controls["pn"].Controls.Add(lab_4_1);
                            place_gb.Controls["pn"].Controls.Add(tb4);
                            place_gb.Controls["pn"].Controls.Add(lab_4_2);
                        }

                        break;
                    }
                //case "kon":
                //    {

                //    }
                case ShellType.Elliptical:
                    {
                        if (rb == null || rb.Checked && rb.Text == PERPENDICULAR)
                        {
                            if (Controls["place_gb"].Controls["corPn"] == null ||
                                (Controls["place_gb"].Controls["corPn"].Controls["placePolar_rb"] as RadioButton).Checked == true)
                            {
                                EllRadialDraw();
                            }
                            else if ((Controls["place_gb"].Controls["corPn"].Controls["placeDekart_rb"] as RadioButton).Checked == true)
                            {
                                EllRadialDekartDraw();
                            }
                        }

                        else if (rb.Checked & rb.Text == OFFSET)
                        {
                            if (Controls["place_gb"].Controls["corPn"] == null ||
                                (Controls["place_gb"].Controls["corPn"].Controls["placePolar_rb"] as RadioButton).Checked == true)
                            {
                                EllVertDraw();
                            }
                            else if ((Controls["place_gb"].Controls["corPn"].Controls["placeDekart_rb"] as RadioButton).Checked == true)
                            {
                                EllVertDekartDraw();
                            }
                        }

                        //else if (rb.Checked & rb.Text == "Наклонный")
                        //{
                        //    if (Controls["place_gb"].Controls["corPn"] == null ||
                        //        (Controls["place_gb"].Controls["corPn"].Controls["placePolar_rb"] as RadioButton).Checked == true)
                        //    {
                        //        EllTiltedDraw();
                        //    }
                        //    else if ((Controls["place_gb"].Controls["corPn"].Controls["placeDekart_rb"] as RadioButton).Checked == true)
                        //    {
                        //        EllTiltedDekartDraw();
                        //    }
                        //}

                        else if (rb == null || (rb.Checked && rb.Text == "Полярная"))
                        {
                            if (Controls["place_gb"] == null ||
                                (Controls["place_gb"].Controls["placerb_1"] as RadioButton).Checked == true)
                            {
                                EllRadialDraw();
                            }
                            else if ((Controls["place_gb"].Controls["placerb_3"] as RadioButton).Checked == true)
                            {
                                EllVertDraw();
                            }
                            else if ((Controls["place_gb"].Controls["placerb_4"] as RadioButton).Checked == true)
                            {
                                EllTiltedDraw();
                            }
                        }

                        else if (rb.Checked && rb.Text == "Декартова")
                        {
                            if (Controls["place_gb"] == null ||
                                (Controls["place_gb"].Controls["placerb_1"] as RadioButton).Checked == true)
                            {
                                EllRadialDekartDraw();
                            }
                            else if ((Controls["place_gb"].Controls["placerb_3"] as RadioButton).Checked == true)
                            {
                                EllVertDekartDraw();
                            }
                            else if ((Controls["place_gb"].Controls["placerb_4"] as RadioButton).Checked == true)
                            {
                                EllTiltedDekartDraw();
                            }
                        }

                        break;
                    }
            }
        }

        private void NozzleForm_Load(object sender, EventArgs e)
        {
            var steels = Physical.Gost34233_1.GetSteelsList()?.ToArray();
            if (steels != null)
            {
                steel1_cb.Items.AddRange(steels);
                steel1_cb.SelectedIndex = 0;
                steel2_cb.Items.AddRange(steels);
                steel2_cb.SelectedIndex = 0;
                steel3_cb.Items.AddRange(steels);
                steel3_cb.SelectedIndex = 0;
            }

            Gost_cb.SelectedIndex = 0;


            //var field = (string)typeof(Owner.GetType()).GetField("TypeElement").GetValue(null);
            //object value = field.GetValue();
            //var c = field;

            MessageBox.Show(_shellDataIn.ShellType.ToString());

            switch (_shellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        RadioButton placerb_1 = new()
                        {
                            Text = PERPENDICULAR,
                            Checked = true,
                            AutoSize = true,
                            Location = new System.Drawing.Point(8, 22),
                            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                            Name = "placerb_1",
                            UseVisualStyleBackColor = true,
                        };
                        placerb_1.CheckedChanged += new EventHandler(Place_rb_CheckedChanged);
                        //Size = new System.Drawing.Size(31, 19),

                        //placerb_1.toggled[bool].emit(False)

                        RadioButton placerb_2 = new()
                        {
                            Text = TRANSVERSELY,
                            AutoSize = true,
                            Location = new System.Drawing.Point(8, 62),
                            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                            Name = "placerb_2",
                            UseVisualStyleBackColor = true
                        };
                        placerb_2.CheckedChanged += new EventHandler(Place_rb_CheckedChanged);

                        //RadioButton placerb_3 = new RadioButton
                        //{
                        //    Text = "Смещенный",
                        //    AutoSize = true,
                        //    Location = new System.Drawing.Point(8, 76),
                        //    Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                        //    Name = "placerb_3",
                        //    UseVisualStyleBackColor = true
                        //};
                        //placerb_3.CheckedChanged += new EventHandler(Place_rb_CheckedChanged);
                        RadioButton placerb_3 = new()
                        {
                            Text = SLANTED,
                            AutoSize = true,
                            Location = new System.Drawing.Point(8, 102),
                            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                            Name = "placerb_3",
                            UseVisualStyleBackColor = true
                        };
                        placerb_3.CheckedChanged += new EventHandler(Place_rb_CheckedChanged);

                        Panel pn = new()
                        {
                            Name = "pn",
                            Location = new Point(2, 15),
                            Size = new Size(300, 310)
                        };

                        place_gb.Controls.Add(placerb_1);
                        place_gb.Controls.Add(placerb_2);
                        place_gb.Controls.Add(placerb_3);
                        //place_gb.Controls.Add(placerb_4);
                        place_gb.Controls.Add(pn);

                        place_pb.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.CylRadial);
                    }
                    break;
                // TODO: Добавить расчет конуса
                //else if (this.Owner is KonForm)
                //{
                //    TypeElement = "Kon";
                //}

                case ShellType.Elliptical:
                    {
                        RadioButton placerb_1 = new()
                        {
                            Text = PERPENDICULAR,
                            Checked = true,
                            AutoSize = true,
                            Location = new System.Drawing.Point(8, 30),
                            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                            Name = "placerb_1",
                            UseVisualStyleBackColor = true,
                        };
                        placerb_1.CheckedChanged += new EventHandler(Place_rb_CheckedChanged);
                        //Size = new System.Drawing.Size(31, 19),

                        RadioButton placerb_2 = new()
                        {
                            Text = OFFSET,
                            AutoSize = true,
                            Location = new System.Drawing.Point(8, 70),
                            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                            Name = "placerb_2",
                            UseVisualStyleBackColor = true
                        };
                        placerb_2.CheckedChanged += new EventHandler(Place_rb_CheckedChanged);
                        //RadioButton placerb_4 = new RadioButton
                        //{
                        //    Text = "Наклонный",
                        //    AutoSize = true,
                        //    Location = new System.Drawing.Point(8, 70),
                        //    Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                        //    Name = "placerb_4",
                        //    UseVisualStyleBackColor = true
                        //};
                        //placerb_4.CheckedChanged += new EventHandler(Place_rb_CheckedChanged);

                        Panel pn = new()
                        {
                            Name = "pn",
                            Location = new Point(2, 15),
                            Size = new Size(400, 310)
                        };

                        place_gb.Controls.Add(placerb_1);
                        place_gb.Controls.Add(placerb_2);
                        //place_gb.Controls.Add(placerb_3);
                        //place_gb.Controls.Add(placerb_4);
                        place_gb.Controls.Add(pn);

                        RadioButton placeDekart_rb = new()
                        {
                            Text = "Декартова",
                            AutoSize = true,
                            Location = new System.Drawing.Point(210, 15),
                            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                            Name = "placeDekart_rb",
                            UseVisualStyleBackColor = true
                        };
                        placeDekart_rb.CheckedChanged += new EventHandler(PlaceCoordinat_rb_CheckedChanged);
                        //Size = new System.Drawing.Size(31, 19),

                        RadioButton placePolar_rb = new()
                        {
                            Text = "Полярная",
                            Checked = true,
                            AutoSize = true,
                            Location = new System.Drawing.Point(125, 15),
                            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                            Name = "placePolar_rb",
                            UseVisualStyleBackColor = true
                        };
                        placePolar_rb.CheckedChanged += new EventHandler(PlaceCoordinat_rb_CheckedChanged);

                        var corL = new Label
                        {
                            Text = "Система координат:",
                            AutoSize = true,
                            Location = new Point(0, 15)
                        };

                        Panel corPn = new()
                        {
                            Name = "corPn",
                            Location = new Point(110, 10),
                            Size = new Size(300, 35)
                        };

                        corPn.Controls.Add(corL);
                        corPn.Controls.Add(placeDekart_rb);
                        corPn.Controls.Add(placePolar_rb);

                        place_gb.Controls.Add(corPn);

                        place_pb.Location = new Point(place_pb.Location.X, place_pb.Location.Y + 35);
                        place_pb.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.EllRadial);

                    }
                    break;
            }

            if (this.Owner != null)
            {
                steel1_cb.Text = _shellDataIn.Steel;
                steel2_cb.Text = _shellDataIn.Steel;
                steel3_cb.Text = _shellDataIn.Steel;
                p_tb.Text = _shellDataIn.p.ToString();
                t_tb.Text = _shellDataIn.t.ToString();
                if (string.IsNullOrWhiteSpace(_shellDataIn.Name))
                    nameEl_tb.Text = _shellDataIn.Name;

                vn_rb.Checked = _shellDataIn.IsPressureIn;
                nar_rb.Checked = !_shellDataIn.IsPressureIn;

                pressure_gb.Enabled = false;
            }


            Place_Draw(null);
        }

        private void Cancel_b_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void PredCalc_b_Click(object sender, EventArgs e)
        {
            const string WRONG_INPUT = " неверный ввод";

            _nozzleData = new NozzleInputData(_shellElement.CalculatedData);

            //NozzleDataIn nozzleData = new NozzleDataIn(shellDataIn);

            List<string> dataInErr = new();

            //NozzleKind
            {
                var checkedButton = vid_gb.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked);
                _nozzleData.NozzleKind = (NozzleKind)Convert.ToInt32(checkedButton.Text.First().ToString());
            }

            //t
            {
                if (double.TryParse(t_tb.Text, NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var t))
                {
                    _nozzleData.t = t;
                }
                else
                {
                    dataInErr.Add(nameof(t) + WRONG_INPUT);
                }
            }

            //steel1
            _nozzleData.steel1 = steel1_cb.Text;

            //[σ1]
            if (sigmaHandle_cb.Checked)
            {
                if (double.TryParse(sigma_d1_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var sigma_d1))
                {
                    _nozzleData.sigma_d1 = sigma_d1;
                }
                else
                {
                    dataInErr.Add("[σ]" + WRONG_INPUT);
                }
            }


            if (!_shellDataIn.IsPressureIn)
            {
                //E1
                if (EHandle_cb.Checked)
                {
                    if (double.TryParse(E1_tb.Text, NumberStyles.AllowDecimalPoint,
                            CultureInfo.InvariantCulture, out var E1))
                    {
                        _nozzleData.E1 = E1;
                    }
                    else
                    {
                        dataInErr.Add("E" + WRONG_INPUT);
                    }
                }

            }

            //d
            {
                if (double.TryParse(d_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var d))
                {
                    _nozzleData.d = d;
                }
                else
                {
                    dataInErr.Add(nameof(d) + WRONG_INPUT);
                }
            }

            //s1
            {
                if (double.TryParse(s1_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var s1))
                {
                    _nozzleData.s1 = s1;
                }
                else
                {
                    dataInErr.Add(nameof(s1) + WRONG_INPUT);
                }
            }

            //cs
            {
                if (double.TryParse(cs_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var cs))
                {
                    _nozzleData.cs = cs;
                }
                else
                {
                    dataInErr.Add(nameof(cs) + WRONG_INPUT);
                }
            }

            //cs1
            {
                if (double.TryParse(cs1_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var cs1))
                {
                    _nozzleData.cs1 = cs1;
                }
                else
                {
                    dataInErr.Add(nameof(cs1) + WRONG_INPUT);
                }
            }

            //l1
            {
                if (double.TryParse(l1_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var l1))
                {
                    _nozzleData.l1 = l1;
                }
                else
                {
                    dataInErr.Add(nameof(l1) + WRONG_INPUT);
                }
            }

            //fi
            {
                if (double.TryParse(fi_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var fi))
                {
                    _nozzleData.fi = fi;
                }
                else
                {
                    dataInErr.Add("φ" + WRONG_INPUT);
                }
            }

            //fi1
            {
                if (double.TryParse(fi1_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var fi1))
                {
                    _nozzleData.fi1 = fi1;
                }
                else
                {
                    dataInErr.Add("φ1" + WRONG_INPUT);
                }
            }

            //delta
            {
                if (double.TryParse(delta_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var delta))
                {
                    _nozzleData.delta = delta;
                }
                else
                {
                    dataInErr.Add(nameof(delta) + WRONG_INPUT);
                }
            }

            //delta1
            {
                if (double.TryParse(delta1_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var delta1))
                {
                    _nozzleData.delta1 = delta1;
                }
                else
                {
                    dataInErr.Add(nameof(delta1) + WRONG_INPUT);
                }
            }

            //delta2
            {
                if (double.TryParse(delta2_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var delta2))
                {
                    _nozzleData.delta2 = delta2;
                }
                else
                {
                    dataInErr.Add(nameof(delta2) + WRONG_INPUT);
                }
            }

            if (_nozzleData.NozzleKind is NozzleKind.ImpassWithRing or NozzleKind.PassWithRing or NozzleKind.WithRingAndInPart)
            {
                _nozzleData.steel2 = steel2_cb.Text;

                //l2
                {
                    if (double.TryParse(l2_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var l2))
                    {
                        _nozzleData.l2 = l2;
                    }
                    else
                    {
                        dataInErr.Add(nameof(l2) + WRONG_INPUT);
                    }
                }

                //s2
                {
                    if (double.TryParse(s2_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var s2))
                    {
                        _nozzleData.s2 = s2;
                    }
                    else
                    {
                        dataInErr.Add(nameof(s2) + WRONG_INPUT);
                    }
                }

            }

            if (_nozzleData.NozzleKind is NozzleKind.PassWithoutRing or NozzleKind.PassWithRing or NozzleKind.WithRingAndInPart)
            {
                _nozzleData.steel3 = steel3_cb.Text;

                //l3
                {
                    if (double.TryParse(l3_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var l3))
                    {
                        _nozzleData.l3 = l3;
                    }
                    else
                    {
                        dataInErr.Add(nameof(l3) + WRONG_INPUT);
                    }
                }

                //s3
                {
                    if (double.TryParse(s3_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var s3))
                    {
                        _nozzleData.s3 = s3;
                    }
                    else
                    {
                        dataInErr.Add(nameof(s3) + WRONG_INPUT);
                    }
                }
            }

            var checkedPlaceButton = place_gb.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked);

            var checkedRadioButtonText = checkedPlaceButton.Text;
            switch (checkedRadioButtonText)
            {
                case PERPENDICULAR:
                    if (!_nozzleData.IsOval)
                    {
                        if (_nozzleData.NozzleKind is NozzleKind.ImpassWithoutRing or
                            NozzleKind.ImpassWithRing or
                            NozzleKind.PassWithoutRing or
                            NozzleKind.PassWithRing or
                            NozzleKind.WithRingAndInPart or
                            NozzleKind.WithWealdedRing)
                        {
                            _nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_1;
                        }
                        else if (_nozzleData.NozzleKind is NozzleKind.WithFlanging or NozzleKind.WithTorusshapedInsert)
                        {
                            _nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_7;
                        }
                    }
                    else
                    {
                        _nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_6;
                        if (_shellDataIn.ShellType is ShellType.Elliptical or
                                ShellType.Spherical or ShellType.Torospherical)
                        {
                            _nozzleData.omega = 0;
                        }
                    }
                    break;
                case TRANSVERSELY:
                {
                    _nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_2;

                    var text = (place_gb.Controls["pn"].Controls["t_tb"] as TextBox)?.Text;

                    if (double.TryParse(text, NumberStyles.AllowDecimalPoint,
                            CultureInfo.InvariantCulture, out var tTransversely))
                    {
                        _nozzleData.tTransversely = tTransversely;
                    }
                    else
                    {
                        dataInErr.Add(nameof(tTransversely) + WRONG_INPUT);
                    }

                    break;
                }
                case OFFSET:
                {
                    switch (_shellDataIn.ShellType)
                    {
                        case ShellType.Elliptical:
                            _nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_3;

                            var text = (place_gb.Controls["pn"].Controls["Rsh_tb"] as TextBox)?.Text;

                            if (double.TryParse(text, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var ellx))
                            {
                                _nozzleData.ellx = ellx;
                            }
                            else
                            {
                                dataInErr.Add(nameof(ellx) + WRONG_INPUT);
                            }
                            break;
                        case ShellType.Cylindrical:
                            break;
                    }

                    break;
                }
                case SLANTED:
                {
                    switch (_shellDataIn.ShellType)
                    {
                        case ShellType.Elliptical:
                        case ShellType.Conical:
                        {

                            var omegaText = (place_gb.Controls["pn"].Controls["omega_tb"] as TextBox)?.Text;

                            if (double.TryParse(omegaText, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var omega))
                            {
                                _nozzleData.omega = omega;
                            }
                            else
                            {
                                dataInErr.Add(nameof(omega) + WRONG_INPUT);
                            }

                            var gammaText = (place_gb.Controls["pn"].Controls["gamma_tb"] as TextBox)?.Text;

                            if (double.TryParse(gammaText, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var gamma))
                            {
                                _nozzleData.gamma = gamma;
                            }
                            else
                            {
                                dataInErr.Add(nameof(gamma) + WRONG_INPUT);
                            }

                            _nozzleData.Location = _nozzleData.omega == 0
                                ? NozzleLocation.LocationAccordingToParagraph_5_2_2_5
                                : NozzleLocation.LocationAccordingToParagraph_5_2_2_4;

                            break;
                        }
                        case ShellType.Spherical:
                        case ShellType.Torospherical:
                        {
                            _nozzleData.omega = 0;
                            var gammaText = (place_gb.Controls["pn"].Controls["gamma_tb"] as TextBox)?.Text;

                            if (double.TryParse(gammaText, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out var gamma))
                            {
                                _nozzleData.gamma = gamma;
                            }
                            else
                            {
                                dataInErr.Add(nameof(gamma) + WRONG_INPUT);
                            }

                            _nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_5;
                            break;
                        }
                    }

                    break;
                }
            }

            if ((_nozzleData.cs + _nozzleData.cs1 > _nozzleData.s3) & _nozzleData.s3 > 0)
            {
                dataInErr.Add("cs+cs1 должно быть меньше s3");
            }


            bool isNotError = dataInErr.Any() && DataIn.IsDataGood;

            if (isNotError)
            {
                IElement nozzle = new Nozzle(_nozzleData);

                try
                {
                    nozzle.Calculate();
                }
                catch (CalculateException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                if (nozzle.IsCalculated)
                {
                    if (nozzle.CalculatedData.ErrorList.Any())
                    {
                        MessageBox.Show(string.Join<string>(Environment.NewLine, nozzle.CalculatedData.ErrorList));
                    }

                    calc_b.Enabled = true;
                    d0_l.Text = $@"d0={((NozzleCalculatedData)nozzle.CalculatedData).d0:f2} мм";
                    p_d_l.Text = $@"[p]={((NozzleCalculatedData)nozzle.CalculatedData).p_d:f2} МПа";
                    b_l.Text = $@"b={((NozzleCalculatedData)nozzle.CalculatedData).b:f2} мм";
                }

                MessageBox.Show(Resources.CalcComplete);
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_nozzleData.ErrorList)));
            }

        }

        private void Calc_b_Click(object sender, EventArgs e)
        {

            _nozzleData.Name = name_tb.Text;

            IElement nozzle = new Nozzle(_nozzleData);

            try
            {
                nozzle.Calculate();
            }
            catch (CalculateException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (Owner.Owner is MainForm main)
            {
                main.Word_lv.Items.Add(nozzle.ToString());
                main.ElementsCollection.Add(nozzle);

                //_form.Hide();
            }
            else
            {
                MessageBox.Show("MainForm Error");
            }

            if (nozzle.IsCalculated)
            {
                if (nozzle.CalculatedData.ErrorList.Any())
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, nozzle.CalculatedData.ErrorList));
                }

                d0_l.Text = $@"d0={((NozzleCalculatedData) nozzle.CalculatedData).d0:f2} мм";
                p_d_l.Text = $@"[p]={((NozzleCalculatedData) nozzle.CalculatedData).p_d:f2} МПа";
                b_l.Text = $@"b={((NozzleCalculatedData) nozzle.CalculatedData).b:f2} мм";



                MessageBox.Show(Resources.CalcComplete);
            }
        }

        private void SigmaHandle_cb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox cb)
            {
                sigma_d1_tb.Enabled = cb.Checked;
            }
        }

        private void EHandle_cb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox cb)
            {
                E1_tb.Enabled = cb.Checked;
            }
        }
    }
}
