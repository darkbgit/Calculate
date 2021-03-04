using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calcNet
{
    public partial class NozzleForm : Form
    {
        public NozzleForm(IElement element, ShellDataIn shellDataIn)
        {
            InitializeComponent();
            this.shellDataIn = shellDataIn;
            this.element = element;
            nozzleData = new NozzleDataIn(shellDataIn);
        }

        private NozzleDataIn nozzleData;

        private readonly ShellDataIn shellDataIn;
        private readonly IElement element;

        private const string PERPENDICULAR = "Перпендикулярно\n поверхности";
        private const string TRANSVERSELY = "В плоскости\nпопер. сечения";
        private const string OFFSET = "Смещенный";
        private const string SLANTED = "Наклонный";

        private void Vid_rb_CheckedChanged(object sender, EventArgs e)
        {
            // приводим отправителя к элементу типа RadioButton
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                vid_pictureBox.Image = (Bitmap)calcNet.Properties.Resources.ResourceManager.GetObject("Nozzle" + rb.Text[0]); 
            }
        }

        private void Place_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                switch (shellDataIn.ShellType)
                {
                    case ShellType.Cylindrical:
                        {
                            switch(rb.Text)
                            {
                                case PERPENDICULAR:
                                    {
                                        place_pb.Image = Properties.Resources.CylRadial;
                                        break;
                                    }
                                case TRANSVERSELY:
                                    {
                                        place_pb.Image = Properties.Resources.CylAxial;
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
                                        place_pb.Image = Properties.Resources.CylTilted;
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
                                            place_pb.Image = Properties.Resources.EllRadial;
                                        }
                                        else if ((Controls["place_gb"].Controls["corPn"].Controls["placeDekart_rb"] as RadioButton).Checked == true)
                                        {
                                            place_pb.Image = Properties.Resources.EllRadialDekart;
                                        }
                                            break;
                                    }
                                case OFFSET:
                                    {
                                        if (Controls["place_gb"].Controls["corPn"] == null ||
                                            (Controls["place_gb"].Controls["corPn"]
                                                                .Controls["placePolar_rb"] as RadioButton).Checked == true)
                                        {
                                            place_pb.Image = Properties.Resources.EllVert;
                                        }
                                        else if ((Controls["place_gb"].Controls["corPn"].Controls["placeDekart_rb"] as RadioButton).Checked == true)
                                        {
                                            place_pb.Image = Properties.Resources.EllVertDekart;
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
            Panel pn = new Panel
            {
                Name = "pn",
                Location = new Point(2, 15),
                Size = new Size(300, 310)
            };
            
             Label lab_1_1 = new Label
            {
                AutoSize = true,
                Text = "Смещение, Rш:",
                Location = new System.Drawing.Point(8, 130)
            };
            TextBox tb1 = new TextBox
            {
                Name = "Rsh_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 150)
            };
            Label lab_1_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 154)
            };

            Label lab_2_1 = new Label
            {
                AutoSize = true,
                Text = "Угол смещения оси, θ:",
                Location = new System.Drawing.Point(8, 175)
            };
            TextBox tb2 = new TextBox
            {
                Name = "theta_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 195)
            };
            Label lab_2_2 = new Label
            {
                Text = "°",
                Location = new System.Drawing.Point(115, 199)
            };
            
            place_pb.Image = calcNet.Properties.Resources.EllRadial;
          

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
            Panel pn = new Panel
            {
                Name = "pn",
                Location = new Point(2, 15),
                Size = new Size(300, 310)
            };

            Label lab_1_1 = new Label
            {
                AutoSize = true,
                Text = "Координата, x0:",
                Location = new System.Drawing.Point(8, 130)
            };
            TextBox tb1 = new TextBox
            {
                Name = "x0_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 150)
            };
            Label lab_1_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 154)
            };

            Label lab_2_1 = new Label
            {
                AutoSize = true,
                Text = "Координата, y0:",
                Location = new System.Drawing.Point(8, 175)
            };
            TextBox tb2 = new TextBox
            {
                Name = "y0_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 195)
            };
            Label lab_2_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 199)
            };
            place_pb.Image = (Bitmap)calcNet.Properties.Resources.EllRadialDekart;
        

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
            Panel pn = new Panel
            {
                Name = "pn",
                Location = new Point(2, 15),
                Size = new Size(300, 310)
            };

            Label lab_1_1 = new Label
            {
                AutoSize = true,
                Text = "Смещение, Rш:",
                Location = new System.Drawing.Point(8, 130)
            };
            TextBox tb1 = new TextBox
            {
                Name = "Rsh_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 150)
            };
            Label lab_1_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 154)
            };

            Label lab_2_1 = new Label
            {
                AutoSize = true,
                Text = "Угол смещения оси, θ:",
                Location = new System.Drawing.Point(8, 175)
            };
            TextBox tb2 = new TextBox
            {
                Name = "theta_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 195)
            };
            Label lab_2_2 = new Label
            {
                Text = "°",
                Location = new System.Drawing.Point(115, 199)
            };
            place_pb.Image = (Bitmap)calcNet.Properties.Resources.EllVert;

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
            Panel pn = new Panel
            {
                Name = "pn",
                Location = new Point(2, 15),
                Size = new Size(300, 310)
            };

            Label lab_1_1 = new Label
            {
                AutoSize = true,
                Text = "Координата, x0:",
                Location = new System.Drawing.Point(8, 130)
            };
            TextBox tb1 = new TextBox
            {
                Name = "x0_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 150)
            };
            Label lab_1_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 154)
            };

            Label lab_2_1 = new Label
            {
                AutoSize = true,
                Text = "Координата, y0:",
                Location = new System.Drawing.Point(8, 175)
            };
            TextBox tb2 = new TextBox
            {
                Name = "y0_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 195)
            };
            Label lab_2_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 199)
            };
            place_pb.Image = (Bitmap)calcNet.Properties.Resources.EllVertDekart;


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
            Panel pn = new Panel
            {
                Name = "pn",
                Location = new Point(2, 15),
                Size = new Size(300, 310)
            };

            Label lab_1_1 = new Label
            {
                AutoSize = true,
                Text = "Смещение, Rш:",
                Location = new System.Drawing.Point(8, 130)
            };
            TextBox tb1 = new TextBox
            {
                Name = "Rsh_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 150)
            };
            Label lab_1_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 154)
            };

            Label lab_2_1 = new Label
            {
                AutoSize = true,
                Text = "Угол смещения оси, θ:",
                Location = new System.Drawing.Point(8, 175)
            };
            TextBox tb2 = new TextBox
            {
                Name = "theta_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 195)
            };
            Label lab_2_2 = new Label
            {
                Text = "°",
                Location = new System.Drawing.Point(115, 199)
            };

            Label lab_3_1 = new Label
            {
                AutoSize = true,
                Text = "Угол наклона оси, γ:",
                Location = new System.Drawing.Point(8, 220)
            };
            TextBox tb3 = new TextBox
            {
                Name = "gamma_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 240)
            };
            Label lab_3_2 = new Label
            {
                Text = "°",
                Location = new System.Drawing.Point(115, 244)
            };

            place_pb.Image = Properties.Resources.EllTilted;

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
            Panel pn = new Panel
            {
                Name = "pn",
                Location = new Point(2, 15),
                Size = new Size(300, 310)
            };

            Label lab_1_1 = new Label
            {
                AutoSize = true,
                Text = "Координата, x0:",
                Location = new System.Drawing.Point(8, 130)
            };
            TextBox tb1 = new TextBox
            {
                Name = "x0_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 150)
            };
            Label lab_1_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 154)
            };

            Label lab_2_1 = new Label
            {
                AutoSize = true,
                Text = "Координата, y0:",
                Location = new System.Drawing.Point(8, 175)
            };
            TextBox tb2 = new TextBox
            {
                Name = "y0_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 195)
            };
            Label lab_2_2 = new Label
            {
                Text = "мм",
                Location = new System.Drawing.Point(115, 199)
            };

            Label lab_3_1 = new Label
            {
                AutoSize = true,
                Text = "Угол наклона оси, γ:",
                Location = new System.Drawing.Point(8, 220)
            };
            TextBox tb3 = new TextBox
            {
                Name = "gamma_tb",
                Size = new Size(100, 20),
                Location = new System.Drawing.Point(8, 240)
            };
            Label lab_3_2 = new Label
            {
                Text = "°",
                Location = new System.Drawing.Point(115, 244)
            };
            place_pb.Image = (Bitmap)calcNet.Properties.Resources.EllTiltedDekart;


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
            RadioButton rb = sender as RadioButton;

            switch (shellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        if (rb == null || (rb.Checked && rb.Text == PERPENDICULAR))
                        {
                            place_gb.Controls["pn"].Dispose();
                            Panel pn = new Panel
                            {
                                Name = "pn",
                                Location = new Point(2, 15),
                                Size = new Size(300, 310)
                            };
                            Label lab_1_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Смещение, Lш:",
                                Location = new System.Drawing.Point(8, 130)
                            };
                            TextBox tb1 = new TextBox
                            {
                                Name = "Lsh_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 150)
                            };
                            Label lab_1_2 = new Label
                            {
                                Text = "м",
                                Location = new System.Drawing.Point(115, 154)
                            };

                            Label lab_2_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Угол смещения оси, θ:",
                                Location = new System.Drawing.Point(8, 175)
                            };
                            TextBox tb2 = new TextBox
                            {
                                Name = "theta_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 195)
                            };
                            Label lab_2_2 = new Label
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
                            Panel pn = new Panel
                            {
                                Name = "pn",
                                Location = new Point(2, 15),
                                Size = new Size(300, 310)
                            };
                            Label lab_1_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Смещение, Lш:",
                                Location = new System.Drawing.Point(8, 130)
                            };
                            TextBox tb1 = new TextBox
                            {
                                Name = "Lsh_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 150)
                            };
                            Label lab_1_2 = new Label
                            {
                                Text = "м",
                                Location = new System.Drawing.Point(115, 154)
                            };

                            Label lab_2_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Угол смещения оси, θ:",
                                Location = new System.Drawing.Point(8, 175)
                            };
                            TextBox tb2 = new TextBox
                            {
                                Name = "theta_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 195)
                            };
                            Label lab_2_2 = new Label
                            {
                                Text = "°",
                                Location = new System.Drawing.Point(115, 199)
                            };

                            Label lab_3_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Угол, ψ:",
                                Location = new System.Drawing.Point(8, 220)
                            };
                            TextBox tb3 = new TextBox
                            {
                                Name = "psi_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 240)
                            };
                            Label lab_3_2 = new Label
                            {
                                Text = "°",
                                Location = new System.Drawing.Point(115, 244)
                            };

                            Label lab_4_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Смещение, t:",
                                Location = new System.Drawing.Point(8, 265)
                            };
                            TextBox tb4 = new TextBox
                            {
                                Name = "t_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 285)
                            };
                            Label lab_4_2 = new Label
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
                            Panel pn = new Panel
                            {
                                Name = "pn",
                                Location = new Point(2, 15),
                                Size = new Size(300, 310)
                            };
                            Label lab_1_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Смещение, Lш:",
                                Location = new System.Drawing.Point(8, 130)
                            };
                            TextBox tb1 = new TextBox
                            {
                                Name = "Lsh_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 150)
                            };
                            Label lab_1_2 = new Label
                            {
                                Text = "м",
                                Location = new System.Drawing.Point(115, 154)
                            };

                            Label lab_2_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Угол смещения оси, θ:",
                                Location = new System.Drawing.Point(8, 175)
                            };
                            TextBox tb2 = new TextBox
                            {
                                Name = "theta_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 195)
                            };
                            Label lab_2_2 = new Label
                            {
                                Text = "°",
                                Location = new System.Drawing.Point(115, 199)
                            };

                            Label lab_3_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Угол наклона оси, γ:",
                                Location = new System.Drawing.Point(8, 220)
                            };
                            TextBox tb3 = new TextBox
                            {
                                Name = "gamma_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 240)
                            };
                            Label lab_3_2 = new Label
                            {
                                Text = "°",
                                Location = new System.Drawing.Point(115, 244)
                            };

                            Label lab_4_1 = new Label
                            {
                                AutoSize = true,
                                Text = "Угол отклонения оси, ω:",
                                Location = new System.Drawing.Point(8, 265)
                            };
                            TextBox tb4 = new TextBox
                            {
                                Name = "omega_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 285)
                            };
                            Label lab_4_2 = new Label
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
            SetSteelList.SetList(steel1_cb);
            steel1_cb.SelectedIndex = 0;
            SetSteelList.SetList(steel2_cb);
            steel2_cb.SelectedIndex = 0;
            SetSteelList.SetList(steel3_cb);
            steel3_cb.SelectedIndex = 0;
            Gost_cb.SelectedIndex = 0;


            //var field = (string)typeof(Owner.GetType()).GetField("TypeElement").GetValue(null);
            //object value = field.GetValue();
            //var c = field;

            switch (shellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        MessageBox.Show(shellDataIn.ShellType.ToString());

                        RadioButton placerb_1 = new RadioButton
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

                        RadioButton placerb_2 = new RadioButton
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
                        RadioButton placerb_3 = new RadioButton
                        {
                            Text = SLANTED,
                            AutoSize = true,
                            Location = new System.Drawing.Point(8, 102),
                            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                            Name = "placerb_3",
                            UseVisualStyleBackColor = true
                        };
                        placerb_3.CheckedChanged += new EventHandler(Place_rb_CheckedChanged);

                        Panel pn = new Panel
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

                        place_pb.Image = calcNet.Properties.Resources.CylRadial;
                    }
                    break;
                // TODO: Добавить расчет конуса
                //else if (this.Owner is KonForm)
                //{
                //    TypeElement = "Kon";
                //}

                case ShellType.Elliptical:
                    {
                        System.Windows.Forms.MessageBox.Show(shellDataIn.ShellType.ToString());

                        RadioButton placerb_1 = new RadioButton
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

                        RadioButton placerb_2 = new RadioButton
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

                        Panel pn = new Panel
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

                        RadioButton placeDekart_rb = new RadioButton
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

                        RadioButton placePolar_rb = new RadioButton
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

                        Label corL = new Label
                        {
                            Text = "Система координат:",
                            AutoSize = true,
                            Location = new Point(0, 15)
                        };

                        Panel corPn = new Panel
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
                        place_pb.Image = calcNet.Properties.Resources.EllRadial;

                    }
                    break;
            }

            if (this.Owner != null)
            {
                steel1_cb.Text = shellDataIn.Steel;
                steel2_cb.Text = shellDataIn.Steel;
                steel3_cb.Text = shellDataIn.Steel;
                p_tb.Text = shellDataIn.p.ToString();
                t_tb.Text = shellDataIn.t.ToString();
                if (shellDataIn.Name != null) nameEl_tb.Text = shellDataIn.Name;

                vn_rb.Checked = shellDataIn.IsPressureIn;

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
            //NozzleDataIn nozzleData = new NozzleDataIn(shellDataIn);

            List<string> dataInErr = new List<string>();

            //NozzleKind
            {
                var checkedButton = vid_gb.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked);
                nozzleData.NozzleKind = (NozzleKind) Convert.ToInt32(checkedButton.Text.First().ToString());
            }

            //t
            //InputClass.GetInput_t(t_tb, ref dN_in, ref dataInErr);
            {
                if (double.TryParse(t_tb.Text, System.Globalization.NumberStyles.Integer,
                    System.Globalization.CultureInfo.InvariantCulture, out double t))
                {
                    nozzleData.t = t;
                }
                else
                {
                    dataInErr.Add(nameof(t) + WRONG_INPUT);
                }
            }

            //steel1
            nozzleData.steel1 = steel1_cb.Text;

            nozzleData.CheckData();



            if (nozzleData.IsDataGood)
            {
                {
                    double sigma_d1 = 0;
                    if (sigma_d1_tb.ReadOnly)
                    {

                        CalcClass.GetSigma(nozzleData.steel1,
                            nozzleData.t,
                            ref sigma_d1,
                            ref dataInErr);
                        sigma_d1_tb.ReadOnly = false;
                        sigma_d1_tb.Text = sigma_d1.ToString();
                        sigma_d1_tb.ReadOnly = true;
                    }
                    else
                    {
                        if (!double.TryParse(sigma_d1_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                            System.Globalization.CultureInfo.InvariantCulture, out sigma_d1))
                        {
                            dataInErr.Add("[σ]" + WRONG_INPUT);
                        }
                    }

                    nozzleData.sigma_d1 = sigma_d1;
                }


                if (!nozzleData.ShellDataIn.IsPressureIn)
                {
                    //E1
                    double E1 = 0;
                    if (E1_tb.ReadOnly)
                    {

                        CalcClass.GetE(nozzleData.steel1,
                            nozzleData.t,
                            ref E1,
                            ref dataInErr);
                        E1_tb.ReadOnly = false;
                        E1_tb.Text = E1.ToString();
                        E1_tb.ReadOnly = true;
                    }
                    else
                    {
                        if (!double.TryParse(E1_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                            System.Globalization.CultureInfo.InvariantCulture, out E1))
                        {
                            dataInErr.Add("[σ]" + WRONG_INPUT);
                        }
                    }
                    nozzleData.E1 = E1;
                }
            }

            //d
            {
                if (double.TryParse(d_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double d))
                {
                    nozzleData.d = d;
                }
                else
                {
                    dataInErr.Add(nameof(d) + WRONG_INPUT);
                }
            }

            //s1
            {
                if (double.TryParse(s1_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double s1))
                {
                    nozzleData.s1 = s1;
                }
                else
                {
                    dataInErr.Add(nameof(s1) + WRONG_INPUT);
                }
            }

            //cs
            {
                if (double.TryParse(cs_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double cs))
                {
                    nozzleData.cs = cs;
                }
                else
                {
                    dataInErr.Add(nameof(cs) + WRONG_INPUT);
                }
            }

            //cs1
            {
                if (double.TryParse(cs1_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double cs1))
                {
                    nozzleData.cs1 = cs1;
                }
                else
                {
                    dataInErr.Add(nameof(cs1) + WRONG_INPUT);
                }
            }

            //l1
            {
                if (double.TryParse(l1_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double l1))
                {
                    nozzleData.l1 = l1;
                }
                else
                {
                    dataInErr.Add(nameof(l1) + WRONG_INPUT);
                }
            }

            //fi
            {
                if (double.TryParse(fi_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double fi))
                {
                    nozzleData.fi = fi;
                }
                else
                {
                    dataInErr.Add("φ" + WRONG_INPUT);
                }
            }

            //fi
            {
                if (double.TryParse(fi1_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double fi1))
                {
                    nozzleData.fi1 = fi1;
                }
                else
                {
                    dataInErr.Add("φ1" + WRONG_INPUT);
                }
            }

            //delta
            {
                if (double.TryParse(delta_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double delta))
                {
                    nozzleData.delta = delta;
                }
                else
                {
                    dataInErr.Add(nameof(delta) + WRONG_INPUT);
                }
            }

            //delta1
            {
                if (double.TryParse(delta1_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double delta1))
                {
                    nozzleData.delta1 = delta1;
                }
                else
                {
                    dataInErr.Add(nameof(delta1) + WRONG_INPUT);
                }
            }

            //delta2
            {
                if (double.TryParse(delta2_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double delta2))
                {
                    nozzleData.delta2 = delta2;
                }
                else
                {
                    dataInErr.Add(nameof(delta2) + WRONG_INPUT);
                }
            }

            if (nozzleData.NozzleKind == NozzleKind.ImpassWithRing ||
                nozzleData.NozzleKind == NozzleKind.PassWithRing ||
                nozzleData.NozzleKind == NozzleKind.WithRingAndInPart)
            {
                nozzleData.steel2 = steel2_cb.Text;

                //l2
                {
                    if (double.TryParse(l2_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out double l2))
                    {
                        nozzleData.l2 = l2;
                    }
                    else
                    {
                        dataInErr.Add(nameof(l2) + WRONG_INPUT);
                    }
                }

                //s2
                {
                    if (double.TryParse(s2_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out double s2))
                    {
                        nozzleData.s2 = s2;
                    }
                    else
                    {
                        dataInErr.Add(nameof(s2) + WRONG_INPUT);
                    }
                }

                //sigma_d2
                {
                    double sigma_d2 = 0;
                    CalcClass.GetSigma(nozzleData.steel2, nozzleData.t, ref sigma_d2, ref dataInErr);
                    nozzleData.sigma_d2 = sigma_d2;
                }

                //E2
                {
                    double E2 = 0;
                    CalcClass.GetE(nozzleData.steel2,
                            nozzleData.t,
                            ref E2,
                            ref dataInErr);

                    nozzleData.E2 = E2;

                }
            }

            if (nozzleData.NozzleKind == NozzleKind.PassWithoutRing ||
                nozzleData.NozzleKind == NozzleKind.PassWithRing ||
                nozzleData.NozzleKind == NozzleKind.WithRingAndInPart)
            {
                nozzleData.steel3 = steel3_cb.Text;

                //l3
                {
                    if (double.TryParse(l3_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out double l3))
                    {
                        nozzleData.l3 = l3;
                    }
                    else
                    {
                        dataInErr.Add(nameof(l3) + WRONG_INPUT);
                    }
                }

                //s3
                {
                    if (double.TryParse(s3_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out double s3))
                    {
                        nozzleData.s3 = s3;
                    }
                    else
                    {
                        dataInErr.Add(nameof(s3) + WRONG_INPUT);
                    }
                }

                //sigma_d3
                {
                    double sigma_d3 = 0;
                    CalcClass.GetSigma(nozzleData.steel3, nozzleData.t, ref sigma_d3, ref dataInErr);
                    nozzleData.sigma_d3 = sigma_d3;
                }

                //E3
                {
                    double E3 = 0;
                    CalcClass.GetE(nozzleData.steel3,
                            nozzleData.t,
                            ref E3,
                            ref dataInErr);

                    nozzleData.E3 = E3;

                }
            }

            var checkedPlaceButton = place_gb.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked);

            string chekedRadioButtonText;
            chekedRadioButtonText = checkedPlaceButton.Text;
            switch (chekedRadioButtonText)
            {
                case PERPENDICULAR:
                    if (!nozzleData.IsOval)
                    {
                        if (nozzleData.NozzleKind == NozzleKind.ImpassWithoutRing ||
                            nozzleData.NozzleKind == NozzleKind.ImpassWithRing ||
                            nozzleData.NozzleKind == NozzleKind.PassWithoutRing ||
                            nozzleData.NozzleKind == NozzleKind.PassWithRing ||
                            nozzleData.NozzleKind == NozzleKind.WithRingAndInPart ||
                            nozzleData.NozzleKind == NozzleKind.WithWealdedRing)
                        {
                            nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_1;
                        }
                        else if (nozzleData.NozzleKind == NozzleKind.WithFlanging ||
                                 nozzleData.NozzleKind == NozzleKind.WithTorusshapedInsert)
                        {
                            nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_7;
                        }
                    }
                    else
                    {
                        nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_6;
                        if (shellDataIn.ShellType == ShellType.Elliptical ||
                            shellDataIn.ShellType == ShellType.Spherical ||
                            shellDataIn.ShellType == ShellType.Torospherical)
                        {
                            nozzleData.omega = 0;
                        }
                    }

                    break;
                case TRANSVERSELY:
                    nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_2;
                    nozzleData.tTransversely = Convert.ToDouble((place_gb.Controls["pn"].Controls["t_tb"] as TextBox).Text);
                    break;
                case OFFSET:
                    switch (shellDataIn.ShellType)
                    {
                        case ShellType.Elliptical:
                            nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_3;
                            nozzleData.ellx =
                                Convert.ToDouble((place_gb.Controls["pn"].Controls["Rsh_tb"] as TextBox).Text);
                            break;
                        case ShellType.Cylindrical:
                            break;
                    }

                    break;
                case SLANTED:
                    switch (shellDataIn.ShellType)
                    {
                        case ShellType.Elliptical:
                        case ShellType.Conical:
                            nozzleData.omega =
                                Convert.ToDouble((place_gb.Controls["pn"].Controls["omega_tb"] as TextBox)
                                    .Text);
                            nozzleData.gamma =
                                Convert.ToDouble((place_gb.Controls["pn"].Controls["gamma_tb"] as TextBox)
                                    .Text);
                            if (nozzleData.omega == 0)
                            {
                                nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_5;
                            }
                            else
                            {
                                nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_4;
                            }

                            break;
                        case ShellType.Spherical:
                        case ShellType.Torospherical:
                            nozzleData.omega = 0;
                            nozzleData.gamma =
                                Convert.ToDouble((place_gb.Controls["pn"].Controls["gamma_tb"] as TextBox)
                                    .Text);
                            nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_5;
                            break;
                    }

                    break;
            }

            if ((nozzleData.cs + nozzleData.cs1 > nozzleData.s3) & nozzleData.s3 > 0)
            {
                dataInErr.Add("cs+cs1 должно быть меньше s3");
            }

            nozzleData.CheckData();
            bool isNotError = dataInErr.Count == 0 && nozzleData.IsDataGood;

            if (isNotError)
            {
                Nozzle nozzle = new Nozzle(element, nozzleData);
                nozzle.Calculate();
                if (!nozzle.IsCriticalError)
                {

                    d0_l.Text = $"d0={nozzle.d0:f2} мм";
                    p_d_l.Text = $"[p]={nozzle.p_d:f2} МПа";
                    b_l.Text = $"b={nozzle.b:f2} мм";
                    calc_b.Enabled = true;
                    if (nozzle.IsError)
                    {
                        System.Windows.Forms.MessageBox.Show(string.Join<string>(Environment.NewLine, nozzle.ErrorList));
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(string.Join<string>(Environment.NewLine, nozzle.ErrorList));
                }
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(nozzleData.ErrorList)));
            }
        }

        private void Calc_b_Click(object sender, EventArgs e)
        {

            nozzleData.Name = name_tb.Text;
            
       

                Nozzle nozzle = new Nozzle(element, nozzleData);
                nozzle.Calculate();
            if (!nozzle.IsCriticalError)
            {

                d0_l.Text = $"d0={nozzle.d0:f2} мм";
                p_d_l.Text = $"[p]={nozzle.p_d:f2} МПа";
                b_l.Text = $"b={nozzle.b:f2} мм";
                if (this.Owner.Owner is MainForm main)
                {
                    main.Word_lv.Items.Add(nozzle.ToString());
                    Elements.ElementsList.Add(nozzle);
                    System.Windows.Forms.MessageBox.Show("Calculation complete");
                    this.Hide();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("MainForm Error");
                }
                if (nozzle.IsError)
                {
                    System.Windows.Forms.MessageBox.Show(string.Join<string>(Environment.NewLine, nozzle.ErrorList));
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(string.Join<string>(Environment.NewLine, nozzle.ErrorList));
            }
        }
    }
}
