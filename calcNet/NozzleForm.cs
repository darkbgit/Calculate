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
        public NozzleForm()
        {
            InitializeComponent();
            element = Elements.ElementsList.Last();          
        }

        private readonly IElement element;

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
                switch ((element as Shell).ShellType)
                {
                    case ShellType.Cylindrical:
                        {
                            switch(rb.Text)
                            {
                                case "Перпендикулярно\n поверхности":
                                    {
                                        place_pb.Image = Properties.Resources.CylRadial;
                                        break;
                                    }
                                case "В плоскости\nпопер. сечения":
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
                                case "Наклонный":
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
                                case "Перпендикулярно\n поверхности":
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
                                case "Смещенный":
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

            switch ((element as Shell).ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        if (rb == null || (rb.Checked && rb.Text == "Перпендикулярно\n поверхности"))
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

                        else if (rb.Checked && rb.Text == "В плоскости\nпопер. сечения")
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

                        else if (rb.Checked && rb.Text == "Наклонный")
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
                        if (rb == null || (rb.Checked && rb.Text == "Перпендикулярно\n поверхности"))
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

                        else if (rb.Checked & rb.Text == "Смещенный")
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
            Set_steellist.Set_llist(steel1_cb);
            steel1_cb.SelectedIndex = 0;
            Set_steellist.Set_llist(steel2_cb);
            steel2_cb.SelectedIndex = 0;
            Set_steellist.Set_llist(steel3_cb);
            steel3_cb.SelectedIndex = 0;
            Gost_cb.SelectedIndex = 0;


            //var field = (string)typeof(Owner.GetType()).GetField("TypeElement").GetValue(null);
            //object value = field.GetValue();
            //var c = field;

            switch ((element as Shell).ShellType) // DataInOutShellWithNozzle.Data_In.shellType)
            {
                case ShellType.Cylindrical:
                    {
                        MessageBox.Show((element as Shell).ShellType.ToString());

                        RadioButton placerb_1 = new RadioButton
                        {
                            Text = "Перпендикулярно\n поверхности",
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
                            Text = "В плоскости\nпопер. сечения",
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
                            Text = "Наклонный",
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
                        System.Windows.Forms.MessageBox.Show((element as Shell).ShellType.ToString());

                        RadioButton placerb_1 = new RadioButton
                        {
                            Text = "Перпендикулярно\n поверхности",
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
                            Text = "Смещенный",
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
                steel1_cb.Text = Owner.Controls["steel_cb"].Text;
                steel2_cb.Text = Owner.Controls["steel_cb"].Text;
                steel3_cb.Text = Owner.Controls["steel_cb"].Text;
                p_tb.Text = Owner.Controls["dav_gb"].Controls["p_tb"].Text;
                t_tb.Text = Owner.Controls["t_tb"].Text;
                nameEl_tb.Text = Owner.Controls["name_tb"].Text;
                if (Owner.Controls["dav_gb"].Controls["vn_rb"] is RadioButton rb)
                {
                    if (rb.Checked)
                    {
                        vn_rb.Checked = true;
                    }
                    else
                    {
                        nar_rb.Checked = true;
                    }
                }
            }


            Place_Draw(null);
        }

        private void Cancel_b_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void PredCalc_b_Click(object sender, EventArgs e)
        {
            DataInOutShellWithNozzle.Data_In.isNeedMakeCalcNozzle = true;
            Data_in d_in = DataInOutShellWithNozzle.Data_In;
            Data_out d_out = DataInOutShellWithNozzle.Data_Out;
            DataNozzle_in dN_in = new DataNozzle_in();


            string dataInErr = "";

            //t
            InputClass.GetInput_t(t_tb, ref dN_in, ref dataInErr);

            //steel1
            dN_in.steel1 = steel1_cb.Text;

            if (dataInErr == "")
            {
                if (sigma_d1_tb.ReadOnly)
                {
                    sigma_d1_tb.ReadOnly = false;
                    //sigma_d1_tb.Text = Convert.ToString(CalcClass.GetSigma(dN_in.steel1, d_in.temp));
                    sigma_d1_tb.ReadOnly = true;
                }
                try
                {
                    dN_in.sigma_d1 = Convert.ToDouble(sigma_d1_tb.Text);
                }
                catch
                {
                    dataInErr += "[σ] неверные данные\n";
                }

                if (!vn_rb.Checked)
                {
                    //E1
                    InputClass.GetInput_E(E1_tb, ref dN_in,  ref dataInErr, 1);
                }
            }

            //try
            //{
            //    if (Convert.ToDouble(p_tb.Text) > 0 && Convert.ToDouble(p_tb.Text) < 1000)
            //    {
            //        d_in.p = Convert.ToDouble(p_tb.Text);
            //    }
            //    else
            //    {
            //        data_inerr += "p должно быть в диапазоне 0 - 1000\n";
            //    }
            //}
            //catch
            //{
            //    data_inerr += "p должно быть в диапазоне 0 - 1000\n";
            //}

            try
            {
                if (Convert.ToInt32(d_tb.Text) > 0)
                {
                    dN_in.D = Convert.ToInt32(d_tb.Text);
                }
                else
                {
                    dataInErr += "d неверные данные\n";
                }
            }
            catch
            {
                dataInErr += "d неверные данные\n";
            }

            try
            {
                if (Convert.ToDouble(s1_tb.Text) > 0)
                {
                    dN_in.s1 = Convert.ToDouble(s1_tb.Text);
                }
                else
                {
                    dataInErr += "s1 неверные данные\n";
                }
            }
            catch
            {
                dataInErr += "s1 неверные данные\n";
            }

            try
            {
                if (Convert.ToDouble(cs_tb.Text) >= 0)
                {
                    dN_in.cs = Convert.ToDouble(cs_tb.Text);
                }
                else
                {
                    dataInErr += "cs неверные данные\n";
                }
            }
            catch
            {
                dataInErr += "cs неверные данные\n";
            }

            try
            {
                if (Convert.ToDouble(cs1_tb.Text) >= 0)
                {
                    dN_in.cs1 = Convert.ToDouble(cs1_tb.Text);
                }
                else
                {
                    dataInErr += "cs1 неверные данные\n";
                }
            }
            catch
            {
                dataInErr += "cs1 неверные данные\n";
            }

            try
            {
                if (Convert.ToInt32(l1_tb.Text) >= 0)
                {
                    dN_in.l1 = Convert.ToInt32(l1_tb.Text);
                }
                else
                {
                    dataInErr += "l1 неверные данные\n";
                }
            }
            catch
            {
                dataInErr += "l1 неверные данные\n";
            }

            dN_in.steel2 = steel2_cb.Text;

            try
            {
                if (Convert.ToInt32(l2_tb.Text) >= 0)
                {
                    dN_in.l2 = Convert.ToInt32(l2_tb.Text);
                }
                else
                {
                    dataInErr += "l2 неверные данные\n";
                }
            }
            catch
            {
                dataInErr += "l2 неверные данные\n";
            }

            try
            {
                if (Convert.ToDouble(s2_tb.Text) >= 0)
                {
                    dN_in.s2 = Convert.ToDouble(s2_tb.Text);
                }
                else
                {
                    dataInErr += "s2 неверные данные\n";
                }
            }
            catch
            {
                dataInErr += "s2 неверные данные\n";
            }

            dN_in.steel3 = steel3_cb.Text;

            try
            {
                if (Convert.ToInt32(l3_tb.Text) >= 0)
                {
                    dN_in.l3 = Convert.ToInt32(l3_tb.Text);
                }
                else
                {
                    dataInErr += "l3 неверные данные\n";
                }
            }
            catch
            {
                dataInErr += "l3 неверные данные\n";
            }

            try
            {
                if (Convert.ToDouble(s3_tb.Text) >= 0)
                {
                    dN_in.s3 = Convert.ToDouble(s3_tb.Text);
                }
                else
                {
                    dataInErr += "s3 неверные данные\n";
                }
            }
            catch
            {
                dataInErr += "s3 неверные данные\n";
            }

            try
            {
                if (Convert.ToDouble(fi_tb.Text) > 0 & Convert.ToDouble(fi_tb.Text) <= 1)
                {
                    dN_in.fi = Convert.ToDouble(fi_tb.Text);
                }
                else
                {
                    dataInErr += "φ должен быть в диапазоне 0 - 1\n";
                }
            }
            catch
            {
                dataInErr += "φ должен быть в диапазоне 0 - 1\n";
            }

            try
            {
                if (Convert.ToDouble(fi1_tb.Text) > 0 & Convert.ToDouble(fi1_tb.Text) <= 1)
                {
                    dN_in.fi1 = Convert.ToDouble(fi1_tb.Text);
                }
                else
                {
                    dataInErr += "φ1 должен быть в диапазоне 0 - 1\n";
                }
            }
            catch
            {
                dataInErr += "φ1 должен быть в диапазоне 0 - 1\n";
            }

            try
            {
                if (Convert.ToInt32(delta_tb.Text) >= 0)
                {
                    dN_in.delta = Convert.ToInt32(delta_tb.Text);
                }
                else
                {
                    dataInErr += "delta должен быть в диапазоне 0 - \n";
                }
            }
            catch
            {
                dataInErr += "delta должен быть в диапазоне 0 - 1\n";
            }

            try
            {
                if (Convert.ToInt32(delta1_tb.Text) >= 0)
                {
                    dN_in.delta1 = Convert.ToInt32(delta1_tb.Text);
                }
                else
                {
                    dataInErr += "delta1 должен быть в диапазоне 0 - \n";
                }
            }
            catch
            {
                dataInErr += "delta1 должен быть в диапазоне 0 - 1\n";
            }

            try
            {
                if (Convert.ToInt32(delta2_tb.Text) >= 0)
                {
                    dN_in.delta2 = Convert.ToInt32(delta2_tb.Text);
                }
                else
                {
                    dataInErr += "delta2 должен быть в диапазоне 0 - \n";
                }
            }
            catch
            {
                dataInErr += "delta2 должен быть в диапазоне 0 - 1\n";
            }

            foreach (Control rb in Controls["vid_gb"].Controls)
            {
                if (rb is RadioButton && (rb as RadioButton).Checked)
                {
                    dN_in.nozzleKind = (NozzleKind)Convert.ToInt32(rb.Text.First().ToString());
                }
            }

            foreach (Control rb in Controls["place_gb"].Controls)
            {
                if (rb is RadioButton && (rb as RadioButton).Checked)
                {
                    //dN_in.location = (NozzleLocation)(Convert.ToInt32(rb.Name.Last().ToString()) - 1);

                    string chekedRadioButtonText;
                    chekedRadioButtonText = rb.Text;
                    switch (chekedRadioButtonText)
                    {
                        case "Перпендикулярно\n поверхности":
                            if (!dN_in.isOval)
                            {
                                if (dN_in.nozzleKind == NozzleKind.ImpassWithoutRing ||
                                    dN_in.nozzleKind == NozzleKind.ImpassWithRing ||
                                    dN_in.nozzleKind == NozzleKind.PassWithoutRing ||
                                    dN_in.nozzleKind == NozzleKind.PassWithRing ||
                                    dN_in.nozzleKind == NozzleKind.WithRingAndInPart ||
                                    dN_in.nozzleKind == NozzleKind.WithWealdedRing)
                                {
                                    dN_in.location = NozzleLocation.LocationAccordingToParagraph_5_2_2_1;
                                }
                                else if (dN_in.nozzleKind == NozzleKind.WithFlanging ||
                                        dN_in.nozzleKind == NozzleKind.WithTorusshapedInsert)
                                {
                                    dN_in.location = NozzleLocation.LocationAccordingToParagraph_5_2_2_7;
                                }
                            }
                            else
                            {
                                dN_in.location = NozzleLocation.LocationAccordingToParagraph_5_2_2_6;
                                if (d_in.shellType == ShellType.Elliptical ||
                                    d_in.shellType == ShellType.Spherical ||
                                    d_in.shellType == ShellType.Torospherical)
                                {
                                    dN_in.omega = 0;
                                }
                            }
                            break;
                        case "В плоскости\nпопер. сечения":
                            dN_in.location = NozzleLocation.LocationAccordingToParagraph_5_2_2_2;
                            dN_in.t = Convert.ToDouble((place_gb.Controls["pn"].Controls["t_tb"] as TextBox).Text);
                            break;
                        case "Смещенный":
                            switch (d_in.shellType)
                            {
                                case ShellType.Elliptical:
                                    dN_in.location = NozzleLocation.LocationAccordingToParagraph_5_2_2_3;
                                    dN_in.elx = Convert.ToDouble((place_gb.Controls["pn"].Controls["Rsh_tb"] as TextBox).Text);
                                    break;
                                case ShellType.Cylindrical:
                                    break;
                            }
                            break;
                        case "Наклонный":
                            switch (d_in.shellType)
                            {
                                case ShellType.Elliptical:
                                case ShellType.Conical:
                                    dN_in.omega = Convert.ToDouble((place_gb.Controls["pn"].Controls["omega_tb"] as TextBox).Text);
                                    dN_in.gamma = Convert.ToDouble((place_gb.Controls["pn"].Controls["gamma_tb"] as TextBox).Text);
                                    if (dN_in.omega == 0)
                                    {
                                        dN_in.location = NozzleLocation.LocationAccordingToParagraph_5_2_2_5;
                                    }
                                    else
                                    {
                                        dN_in.location = NozzleLocation.LocationAccordingToParagraph_5_2_2_4;
                                    }
                                    break;
                                case ShellType.Spherical:
                                case ShellType.Torospherical:
                                    dN_in.omega = 0;
                                    dN_in.gamma = Convert.ToDouble((place_gb.Controls["pn"].Controls["gamma_tb"] as TextBox).Text);
                                    dN_in.location = NozzleLocation.LocationAccordingToParagraph_5_2_2_5;
                                    break;
                            }
                            break;
                    }
                }
                break;
            }

            if ((dN_in.cs + dN_in.cs1 > dN_in.s3) & dN_in.s3 > 0)
            {
                dataInErr += "cs+cs1 должно быть меньше s3";
            }


            bool isNotError = dataInErr == "";
            if (isNotError)
            {
                DataNozzle_out dN_out = CalcClass.CalculateNozzle(d_in, d_out, dN_in);
                dataArrEl.DataN_In = dN_in;
                dataArrEl.DataN_Out = dN_out;
                d0_l.Text = $"d0={dN_out.d0:f2} мм";
                p_d_l.Text = $"[p]={dN_out.p_d:f2} МПа";
                b_l.Text = $"b={dN_out.b:f2} мм";
                if (dN_out.err != "")
                {
                    System.Windows.Forms.MessageBox.Show(dN_out.err);
                }
                //#globalvar.elementdatayk.append(dN_in)
                //#globalvar.elementdatayk.append(data_nozzleout)
                //globalvar.data_word.append([data_in, data_out, dN_in, data_nozzleout])
                //i = globalvar.word_lv.rowCount()
                //globalvar.word_lv.insertRow(i)
                //globalvar.word_lv.setData(globalvar.word_lv.index(i), f"{data_in.dia} мм, {data_in.press} МПа, {data_in.temp} C, {data_in.met}, {data_in.yk}")
                //parent().parent().lvCalc.setModel(globalvar.word_lv)
                //pbCalc.setEnabled(True)
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(dataInErr);
            }
        }

        private void Calc_b_Click(object sender, EventArgs e)
        {
            PredCalc_b_Click(sender, e);

            DataInOutShellWithNozzle.DataN_In.name = name_tb.Text;
            
       
            if (this.Owner.Owner is MainForm main)
            {
                int i;
                main.Word_lv.Items.Add($"{DataInOutShellWithNozzle.Data_In.D} мм, {DataInOutShellWithNozzle.Data_In.p} МПа, {DataInOutShellWithNozzle.Data_In.temp} C, {DataInOutShellWithNozzle.Data_In.shellType}, yk");
                i = main.Word_lv.Items.Count - 1;
                //DataWordOut.DataArr[0].  DataArr .DataOutArr[]. .Value = $"{d_in.D} мм, {d_in.p} МПа, {d_in.temp} C, {d_in.met}";
                
                dataArrEl.id = i + 1;
                switch (DataInOutShellWithNozzle.Data_In.shellType)
                {
                    case ShellType.Cylindrical:
                        dataArrEl.calculatedElementType = CalculatedElementType.CylindricalWhithNozzle;
                        break;
                    case ShellType.Conical:
                        dataArrEl.calculatedElementType = CalculatedElementType.ConicalWhithNozzle;
                        break;
                    case ShellType.Elliptical:
                        dataArrEl.calculatedElementType = CalculatedElementType.EllipticalWhithNozzle;
                        break;
                }
          
                DataWordOut.DataArr.Add(DataInOutShellWithNozzle);
                System.Windows.Forms.MessageBox.Show("Calculation complete");
                this.Hide();

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("MainForm Error");
            }
        }
    }
}
