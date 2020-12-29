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
        }

        private string TypeElement;

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
                switch (TypeElement)
                {
                    case "Cil":
                        {
                            switch(rb.Text)
                            {
                                case "Радиальный":
                                    {
                                        place_pb.Image = (Bitmap)calcNet.Properties.Resources.ResourceManager.GetObject("CylRadial");
                                        break;
                                    }
                                case "В плоскости\nпопер. сечения":
                                    {
                                        place_pb.Image = (Bitmap)calcNet.Properties.Resources.ResourceManager.GetObject("CylAxial");
                                        break;
                                    }
                                case "Смещенный":
                                    {
                                        place_pb.Image = (Bitmap)calcNet.Properties.Resources.ResourceManager.GetObject("CylOffset");
                                        break;
                                    }
                                case "Наклонный":
                                    {
                                        place_pb.Image = (Bitmap)calcNet.Properties.Resources.ResourceManager.GetObject("CylTilted");
                                        break;
                                    }
                            }
                            Place_Draw(sender);

                            break;
                        }
                }
            }
        }

        private void Place_Draw(object sender)
        {
            RadioButton rb = sender as RadioButton;

            switch (TypeElement)
            {
                case "Cil":
                    {
                        if (rb == null || (rb.Checked & rb.Text == "Радиальный"))
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

                        else if (rb.Checked & rb.Text == "В плоскости\nпопер. сечения")
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

                        else if (rb.Checked & rb.Text == "Смещенный")
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
                                Text = "Смещение, lсм:",
                                Location = new System.Drawing.Point(8, 220)
                            };
                            TextBox tb3 = new TextBox
                            {
                                Name = "lsm_tb",
                                Size = new Size(100, 20),
                                Location = new System.Drawing.Point(8, 240)
                            };
                            Label lab_3_2 = new Label
                            {
                                Text = "мм",
                                Location = new System.Drawing.Point(115, 244)
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
                        }

                        else if (rb.Checked & rb.Text == "Наклонный")
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
                //case "Kon":
                //    {

                //    }
                //case "Ell":
                //    {

                //    }





                    

                    //elif self.parent().typeElement == 'el':
                    //    self.labsis = QtWidgets.QLabel('Система координат:')
                    //    self.placepolar_rb = QtWidgets.QRadioButton('Полярная')
                    //    self.placepolar_rb.setChecked(True)
                    //    self.placedekart_rb = QtWidgets.QRadioButton('Декартова')
                    //    self.rb_group1 = QtWidgets.QButtonGroup()
                    //    self.rb_group1.addButton(self.placepolar_rb)
                    //    self.rb_group1.addButton(self.placedekart_rb)



                    //    self.placerb_1 = QtWidgets.QRadioButton('Радиальный')
                    //    self.placerb_1.setChecked(True)
                    //    self.placerb_1.toggled[bool].emit(False)
                    //    self.placerb_2 = QtWidgets.QRadioButton('Вдоль оси')
                    //    self.placerb_3 = QtWidgets.QRadioButton('Наклонный')
                    //    self.placerb_group = QtWidgets.QButtonGroup()
                    //    self.placerb_group.addButton(self.placerb_1, 1)
                    //    self.placerb_group.addButton(self.placerb_2, 2)
                    //    self.placerb_group.addButton(self.placerb_3, 3)


                    //    self.pic = QtWidgets.QLabel()
                    //    self.pic.setScaledContents(True)
                    //    self.grid = QtWidgets.QGridLayout()
                    //    self.fr = QtWidgets.QFrame()
                    //    self.grid.addWidget(self.labsis, 0, 1)
                    //    self.grid.addWidget(self.placepolar_rb, 0, 2)
                    //    self.grid.addWidget(self.placedekart_rb, 0, 3)

                    //    self.grid.addWidget(self.placerb_1, 1, 0)
                    //    self.grid.addWidget(self.placerb_2, 2, 0)
                    //    self.grid.addWidget(self.placerb_3, 3, 0)
                    //    self.grid.addWidget(self.fr, 4, 0)
                    //    self.grid.addWidget(self.pic, 1, 1, 6, 3)
                    //    self.grid1 = QtWidgets.QGridLayout()
                    //    self.place_gb.setLayout(self.grid)


                    //    self.grid.addLayout(self.grid1, 5, 0)

                    //    # self.placerb_group.buttonToggled.connect(self.place)
                    //    self.placerb_1.toggled.connect(self.place)
                    //    self.placerb_2.toggled.connect(self.place)
                    //    self.placerb_3.toggled.connect(self.place)

                    //    self.placepolar_rb.toggled.connect(self.place)
                    //    self.placedekart_rb.toggled.connect(self.place)
                    //        }
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

            if (this.Owner is CilForm)
            {
                System.Windows.Forms.MessageBox.Show("Cil");
                TypeElement = "Cil";
                
                RadioButton placerb_1 = new RadioButton
                {
                    Text = "Радиальный",
                    Checked = true,
                    AutoSize = true,
                    Location = new System.Drawing.Point(8, 22),
                    Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                    Name = "placerb_1",
                    UseVisualStyleBackColor = true,
                };
                placerb_1.CheckedChanged += new EventHandler(Place_rb_CheckedChanged);
                //Size = new System.Drawing.Size(31, 19),

                //self.placerb_1.toggled[bool].emit(False)

                RadioButton placerb_2 = new RadioButton
                {
                    Text = "В плоскости\nпопер. сечения",
                    AutoSize = true,
                    Location = new System.Drawing.Point(8, 40),
                    Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                    Name = "placerb_2",
                    UseVisualStyleBackColor = true
                };
                placerb_2.CheckedChanged += new EventHandler(Place_rb_CheckedChanged);

                RadioButton placerb_3 = new RadioButton
                {
                    Text = "Смещенный",
                    AutoSize = true,
                    Location = new System.Drawing.Point(8, 76),
                    Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                    Name = "placerb_3",
                    UseVisualStyleBackColor = true
                };
                placerb_3.CheckedChanged += new EventHandler(Place_rb_CheckedChanged);
                RadioButton placerb_4 = new RadioButton
                {
                    Text = "Наклонный",
                    AutoSize = true,
                    Location = new System.Drawing.Point(8, 103),
                    Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                    Name = "placerb_4",
                    UseVisualStyleBackColor = true
                };
                placerb_4.CheckedChanged += new EventHandler(Place_rb_CheckedChanged);

                Panel pn = new Panel
                {
                    Name = "pn",
                    Location = new Point(2, 15),
                    Size = new Size(300, 310)
                };

                place_gb.Controls.Add(placerb_1);
                place_gb.Controls.Add(placerb_2);
                place_gb.Controls.Add(placerb_3);
                place_gb.Controls.Add(placerb_4);
                place_gb.Controls.Add(pn);
            }
            //else if (this.Owner is KonForm)
            //{
            //    TypeElement = "Kon";
            //}
            //else if (this.Owner is EllForm)
            //{
            //    TypeElement = "Ell";
            //}



            Place_Draw(null);
        }
    }
}
