using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Core.Shells.Nozzle;
using CalculateVessels.Core.Shells.Nozzle.Enums;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels.Forms;

public partial class NozzleForm : Form
{
    private readonly IEnumerable<ICalculateService<NozzleInput>> _calculateServices;
    private readonly IPhysicalDataService _physicalDataService;

    private const string Perpendicular = "Перпендикулярно\n поверхности";
    private const string Transversely = "В плоскости\nпопер. сечения";
    private const string Offset = "Смещенный";
    private const string Slanted = "Наклонный";
    private const string Polar = "Полярная";
    private const string Cartesian = "Декартова";

    private const string TransverselyTextBoxName = "t_tb";
    private const string RNozzleTextBoxName = "Rsh_tb";

    private const string OmegaTextBoxName = "omega_tb";
    private const string GammaTextBoxName = "gamma_tb";

    private NozzleInput? _nozzleData;

    private RadioButton? placeRadioButton1;
    private RadioButton? placeRadioButton2;
    private RadioButton? placeRadioButton3;
    private RadioButton? placeRadioButton4;

    private Panel? mainPanel;

    private RadioButton? placeCartesianRadioButton;
    private RadioButton? placePolarRadioButton;

    private Label? coordinateLabel;
    private Panel? coordinatePanel;

    private ShellInputData _shellInputData;
    private ICalculatedElement _shellElement;

    public NozzleForm(IPhysicalDataService physicalDataService,
        IEnumerable<ICalculateService<NozzleInput>> calculateServices)
    {
        InitializeComponent();
        _physicalDataService = physicalDataService;
        _calculateServices = calculateServices;
    }

    public void Show(ICalculatedElement shellElement)
    {
        _shellElement = shellElement;
        _shellInputData = (ShellInputData)shellElement.InputData;

        ShowDialog();
    }

    private void Vid_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        var i = Convert.ToInt32(rb.Text[0].ToString());
        vid_pictureBox.Image =
            (Bitmap)(new ImageConverter()
                .ConvertFrom(Resources.ResourceManager.GetObject("Nozzle" + i)
                ?? throw new InvalidOperationException())
            ?? throw new InvalidOperationException());

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

    private void Place_rb_CheckedChanged(object? sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        switch (((ShellInputData)_shellElement.InputData).ShellType)
        {
            case ShellType.Cylindrical:
                place_pb.Image = rb.Text switch
                {
                    Perpendicular => (Bitmap)(new ImageConverter().ConvertFrom(Resources.CylRadial) ??
                                              throw new InvalidOperationException()),
                    Transversely => (Bitmap)(new ImageConverter().ConvertFrom(Resources.CylAxial) ??
                                             throw new InvalidOperationException()),
                    //case "Смещенный":
                    //    {
                    //        place_pb.Image =
                    //            (Bitmap)Properties.Resources.ResourceManager.GetObject("CylOffset");
                    //        break;
                    //    }
                    Slanted => (Bitmap)(new ImageConverter().ConvertFrom(Resources.CylTilted)
                                        ?? throw new InvalidOperationException()),
                    _ => place_pb.Image
                };
                Place_Draw(sender);

                break;
            case ShellType.Elliptical:
                switch (rb.Text)
                {
                    case Perpendicular:
                        if (coordinatePanel == null || placePolarRadioButton is { Checked: true })
                        {
                            place_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.EllRadial)
                                                      ?? throw new InvalidOperationException());
                        }
                        else if (placeCartesianRadioButton is { Checked: true })
                        {
                            place_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.EllRadialDekart)
                                                      ?? throw new InvalidOperationException());
                        }
                        break;

                    case Offset:
                        if (coordinatePanel == null || placePolarRadioButton is { Checked: true })
                        {
                            place_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.EllVert)
                                                      ?? throw new InvalidOperationException());
                        }
                        else if (placeCartesianRadioButton is { Checked: true })
                        {
                            place_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.EllVertDekart)
                                                      ?? throw new InvalidOperationException());
                        }
                        break;

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

    private void PlaceCoordinate_rb_CheckedChanged(object? sender, EventArgs e)
    {
        //RadioButton rb = sender as RadioButton;
        //if (rb == null || (rb.Checked && rb.Text == "Полярная"))
        //{
        //    place_pb.Image = (Bitmap)calcNet.Properties.Resources.EllRadial;
        //}

        Place_Draw(sender);
    }

    private void EllipseRadialDraw()
    {
        mainPanel?.Dispose();

        mainPanel = new Panel
        {
            Name = nameof(mainPanel),
            Location = new Point(2, 15),
            Size = new Size(300, 310)
        };

        var lab_1_1 = new Label
        {
            AutoSize = true,
            Text = "Смещение, Rш:",
            Location = new Point(8, 130)
        };
        var tb1 = new TextBox
        {
            Name = RNozzleTextBoxName,
            Size = new Size(100, 20),
            Location = new Point(8, 150)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(115, 154)
        };

        var lab_2_1 = new Label
        {
            AutoSize = true,
            Text = "Угол смещения оси, θ:",
            Location = new Point(8, 175)
        };
        var tb2 = new TextBox
        {
            Name = "theta_tb",
            Size = new Size(100, 20),
            Location = new Point(8, 195)
        };
        var lab_2_2 = new Label
        {
            Text = "°",
            Location = new Point(115, 199)
        };

        place_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.EllRadial)
            ?? throw new InvalidOperationException());

        mainPanel.Controls.AddRange(new Control[]
        {
            lab_1_1, tb1, lab_1_2, lab_2_1, tb2, lab_2_2
        });

        place_gb.Controls.Add(mainPanel);
    }

    private void EllipseRadialCartesianDraw()
    {
        mainPanel?.Dispose();

        mainPanel = new Panel
        {
            Name = nameof(mainPanel),
            Location = new Point(2, 15),
            Size = new Size(300, 310)
        };

        var lab_1_1 = new Label
        {
            AutoSize = true,
            Text = "Координата, x0:",
            Location = new Point(8, 130)
        };
        var tb1 = new TextBox
        {
            Name = "x0_tb",
            Size = new Size(100, 20),
            Location = new Point(8, 150)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(115, 154)
        };

        var lab_2_1 = new Label
        {
            AutoSize = true,
            Text = "Координата, y0:",
            Location = new Point(8, 175)
        };
        var tb2 = new TextBox
        {
            Name = "y0_tb",
            Size = new Size(100, 20),
            Location = new Point(8, 195)
        };
        var lab_2_2 = new Label
        {
            Text = "мм",
            Location = new Point(115, 199)
        };

        place_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.EllRadialDekart)
            ?? throw new InvalidOperationException());

        mainPanel.Controls.AddRange(new Control[]
        {
            lab_1_1, tb1, lab_1_2, lab_2_1, tb2, lab_2_2
        });

        place_gb.Controls.Add(mainPanel);
    }

    private void EllipseVerticalDraw()
    {
        mainPanel?.Dispose();

        mainPanel = new Panel
        {
            Name = nameof(mainPanel),
            Location = new Point(2, 15),
            Size = new Size(300, 310)
        };

        var lab_1_1 = new Label
        {
            AutoSize = true,
            Text = "Смещение, Rш:",
            Location = new Point(8, 130)
        };
        var tb1 = new TextBox
        {
            Name = RNozzleTextBoxName,
            Size = new Size(100, 20),
            Location = new Point(8, 150)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(115, 154)
        };

        var lab_2_1 = new Label
        {
            AutoSize = true,
            Text = "Угол смещения оси, θ:",
            Location = new Point(8, 175)
        };
        var tb2 = new TextBox
        {
            Name = "theta_tb",
            Size = new Size(100, 20),
            Location = new Point(8, 195)
        };
        var lab_2_2 = new Label
        {
            Text = "°",
            Location = new Point(115, 199)
        };
        place_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.EllVert)
            ?? throw new InvalidOperationException());

        mainPanel.Controls.AddRange(new Control[]
        {
            lab_1_1, tb1, lab_1_2, lab_2_1, tb2, lab_2_2
        });

        place_gb.Controls.Add(mainPanel);
    }

    private void EllipseVerticalCartesianDraw()
    {
        mainPanel?.Dispose();

        mainPanel = new Panel
        {
            Name = nameof(mainPanel),
            Location = new Point(2, 15),
            Size = new Size(300, 310)
        };

        var lab_1_1 = new Label
        {
            AutoSize = true,
            Text = "Координата, x0:",
            Location = new Point(8, 130)
        };
        var tb1 = new TextBox
        {
            Name = "x0_tb",
            Size = new Size(100, 20),
            Location = new Point(8, 150)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(115, 154)
        };

        var lab_2_1 = new Label
        {
            AutoSize = true,
            Text = "Координата, y0:",
            Location = new Point(8, 175)
        };
        var tb2 = new TextBox
        {
            Name = "y0_tb",
            Size = new Size(100, 20),
            Location = new Point(8, 195)
        };
        var lab_2_2 = new Label
        {
            Text = "мм",
            Location = new Point(115, 199)
        };

        place_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.EllVertDekart)
            ?? throw new InvalidOperationException());

        mainPanel.Controls.AddRange(new Control[]
        {
            lab_1_1, tb1, lab_1_2, lab_2_1, tb2, lab_2_2
        });

        place_gb.Controls.Add(mainPanel);
    }

    private void EllipseTiltedDraw()
    {
        mainPanel?.Dispose();

        mainPanel = new Panel
        {
            Name = nameof(mainPanel),
            Location = new Point(2, 15),
            Size = new Size(300, 310)
        };

        var lab_1_1 = new Label
        {
            AutoSize = true,
            Text = "Смещение, Rш:",
            Location = new Point(8, 130)
        };
        var tb1 = new TextBox
        {
            Name = RNozzleTextBoxName,
            Size = new Size(100, 20),
            Location = new Point(8, 150)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(115, 154)
        };

        var lab_2_1 = new Label
        {
            AutoSize = true,
            Text = "Угол смещения оси, θ:",
            Location = new Point(8, 175)
        };
        var tb2 = new TextBox
        {
            Name = "theta_tb",
            Size = new Size(100, 20),
            Location = new Point(8, 195)
        };
        var lab_2_2 = new Label
        {
            Text = "°",
            Location = new Point(115, 199)
        };

        var lab_3_1 = new Label
        {
            AutoSize = true,
            Text = "Угол наклона оси, γ:",
            Location = new Point(8, 220)
        };
        var tb3 = new TextBox
        {
            Name = GammaTextBoxName,
            Size = new Size(100, 20),
            Location = new Point(8, 240)
        };
        var lab_3_2 = new Label
        {
            Text = "°",
            Location = new Point(115, 244)
        };

        place_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.EllTilted)
            ?? throw new InvalidOperationException());

        mainPanel.Controls.AddRange(new Control[]
        {
            lab_1_1, tb1, lab_1_2, lab_2_1, tb2, lab_2_2, lab_3_1, tb3, lab_3_2
        });

        place_gb.Controls.Add(mainPanel);
    }

    private void EllipseTiltedCartesianDraw()
    {
        mainPanel?.Dispose();

        mainPanel = new Panel
        {
            Name = nameof(mainPanel),
            Location = new Point(2, 15),
            Size = new Size(300, 310)
        };

        var lab_1_1 = new Label
        {
            AutoSize = true,
            Text = "Координата, x0:",
            Location = new Point(8, 130)
        };
        var tb1 = new TextBox
        {
            Name = "x0_tb",
            Size = new Size(100, 20),
            Location = new Point(8, 150)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(115, 154)
        };

        var lab_2_1 = new Label
        {
            AutoSize = true,
            Text = "Координата, y0:",
            Location = new Point(8, 175)
        };
        var tb2 = new TextBox
        {
            Name = "y0_tb",
            Size = new Size(100, 20),
            Location = new Point(8, 195)
        };
        var lab_2_2 = new Label
        {
            Text = "мм",
            Location = new Point(115, 199)
        };

        var lab_3_1 = new Label
        {
            AutoSize = true,
            Text = "Угол наклона оси, γ:",
            Location = new Point(8, 220)
        };
        var tb3 = new TextBox
        {
            Name = GammaTextBoxName,
            Size = new Size(100, 20),
            Location = new Point(8, 240)
        };
        var lab_3_2 = new Label
        {
            Text = "°",
            Location = new Point(115, 244)
        };
        place_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.EllTiltedDekart)
            ?? throw new InvalidOperationException());

        mainPanel.Controls.AddRange(new Control[]
        {
            lab_1_1, tb1, lab_1_2, lab_2_1, tb2, lab_2_2, lab_3_1, tb3, lab_3_2
        });

        place_gb.Controls.Add(mainPanel);
    }

    private void Place_Draw(object? sender)
    {
        if (sender is not RadioButton rb) return;

        switch (((ShellInputData)_shellElement.InputData).ShellType)
        {
            case ShellType.Cylindrical:
                {
                    if (rb is { Checked: true, Text: Perpendicular })
                    {
                        mainPanel?.Dispose();

                        mainPanel = new Panel
                        {
                            Name = nameof(mainPanel),
                            Location = new Point(2, 15),
                            Size = new Size(300, 310)
                        };
                        var lab_1_1 = new Label
                        {
                            AutoSize = true,
                            Text = "Смещение, Lш:",
                            Location = new Point(8, 130)
                        };
                        var tb1 = new TextBox
                        {
                            Name = "Lsh_tb",
                            Size = new Size(100, 20),
                            Location = new Point(8, 150)
                        };
                        var lab_1_2 = new Label
                        {
                            Text = "м",
                            Location = new Point(115, 154)
                        };

                        var lab_2_1 = new Label
                        {
                            AutoSize = true,
                            Text = "Угол смещения оси, θ:",
                            Location = new Point(8, 175)
                        };
                        var tb2 = new TextBox
                        {
                            Name = "theta_tb",
                            Size = new Size(100, 20),
                            Location = new Point(8, 195)
                        };
                        var lab_2_2 = new Label
                        {
                            Text = "°",
                            Location = new Point(115, 199)
                        };

                        mainPanel.Controls.AddRange(new Control[]
                        {
                            lab_1_1, tb1, lab_1_2, lab_2_1, tb2, lab_2_2
                        });

                        place_gb.Controls.Add(mainPanel);
                    }
                    else if (rb is { Checked: true, Text: Transversely })
                    {
                        mainPanel?.Dispose();

                        mainPanel = new Panel
                        {
                            Name = nameof(mainPanel),
                            Location = new Point(2, 15),
                            Size = new Size(300, 310)
                        };
                        var lab_1_1 = new Label
                        {
                            AutoSize = true,
                            Text = "Смещение, Lш:",
                            Location = new Point(8, 130)
                        };
                        var tb1 = new TextBox
                        {
                            Name = "Lsh_tb",
                            Size = new Size(100, 20),
                            Location = new Point(8, 150)
                        };
                        var lab_1_2 = new Label
                        {
                            Text = "м",
                            Location = new Point(115, 154)
                        };

                        var lab_2_1 = new Label
                        {
                            AutoSize = true,
                            Text = "Угол смещения оси, θ:",
                            Location = new Point(8, 175)
                        };
                        var tb2 = new TextBox
                        {
                            Name = "theta_tb",
                            Size = new Size(100, 20),
                            Location = new Point(8, 195)
                        };
                        var lab_2_2 = new Label
                        {
                            Text = "°",
                            Location = new Point(115, 199)
                        };

                        var lab_3_1 = new Label
                        {
                            AutoSize = true,
                            Text = "Угол, ψ:",
                            Location = new Point(8, 220)
                        };
                        var tb3 = new TextBox
                        {
                            Name = "psi_tb",
                            Size = new Size(100, 20),
                            Location = new Point(8, 240)
                        };
                        var lab_3_2 = new Label
                        {
                            Text = "°",
                            Location = new Point(115, 244)
                        };

                        var lab_4_1 = new Label
                        {
                            AutoSize = true,
                            Text = "Смещение, t:",
                            Location = new Point(8, 265)
                        };
                        var tb4 = new TextBox
                        {
                            Name = TransverselyTextBoxName,
                            Size = new Size(100, 20),
                            Location = new Point(8, 285)
                        };
                        var lab_4_2 = new Label
                        {
                            Text = "мм",
                            Location = new Point(115, 289)
                        };

                        mainPanel.Controls.AddRange(new Control[]
                        {
                            lab_1_1, tb1, lab_1_2, lab_2_1, tb2, lab_2_2, lab_3_1, tb3, lab_3_2, lab_4_1, tb4, lab_4_2
                        });

                        place_gb.Controls.Add(mainPanel);
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

                    else if (rb is { Checked: true, Text: Slanted })
                    {
                        mainPanel?.Dispose();

                        mainPanel = new Panel
                        {
                            Name = nameof(mainPanel),
                            Location = new Point(2, 15),
                            Size = new Size(300, 310)
                        };
                        var lab_1_1 = new Label
                        {
                            AutoSize = true,
                            Text = "Смещение, Lш:",
                            Location = new Point(8, 130)
                        };
                        var tb1 = new TextBox
                        {
                            Name = "Lsh_tb",
                            Size = new Size(100, 20),
                            Location = new Point(8, 150)
                        };
                        var lab_1_2 = new Label
                        {
                            Text = "м",
                            Location = new Point(115, 154)
                        };

                        var lab_2_1 = new Label
                        {
                            AutoSize = true,
                            Text = "Угол смещения оси, θ:",
                            Location = new Point(8, 175)
                        };
                        var tb2 = new TextBox
                        {
                            Name = "theta_tb",
                            Size = new Size(100, 20),
                            Location = new Point(8, 195)
                        };
                        var lab_2_2 = new Label
                        {
                            Text = "°",
                            Location = new Point(115, 199)
                        };

                        var lab_3_1 = new Label
                        {
                            AutoSize = true,
                            Text = "Угол наклона оси, γ:",
                            Location = new Point(8, 220)
                        };
                        var tb3 = new TextBox
                        {
                            Name = GammaTextBoxName,
                            Size = new Size(100, 20),
                            Location = new Point(8, 240)
                        };
                        var lab_3_2 = new Label
                        {
                            Text = "°",
                            Location = new Point(115, 244)
                        };

                        var lab_4_1 = new Label
                        {
                            AutoSize = true,
                            Text = "Угол отклонения оси, ω:",
                            Location = new Point(8, 265)
                        };
                        var tb4 = new TextBox
                        {
                            Name = OmegaTextBoxName,
                            Size = new Size(100, 20),
                            Location = new Point(8, 285)
                        };
                        var lab_4_2 = new Label
                        {
                            Text = "°",
                            Location = new Point(115, 289)
                        };

                        mainPanel.Controls.AddRange(new Control[]
                        {
                            lab_1_1, tb1, lab_1_2, lab_2_1, tb2, lab_2_2, lab_3_1, tb3, lab_3_2, lab_4_1, tb4, lab_4_2
                        });

                        place_gb.Controls.Add(mainPanel);
                    }

                    break;
                }
            //case "kon":
            //    {

            //    }
            case ShellType.Elliptical:
                {
                    if (rb is { Checked: true, Text: Perpendicular })
                    {
                        if (coordinatePanel == null || placePolarRadioButton is { Checked: true })
                        {
                            EllipseRadialDraw();
                        }
                        else if (placeCartesianRadioButton is { Checked: true })
                        {
                            EllipseRadialCartesianDraw();
                        }
                    }
                    else if (rb is { Checked: true, Text: Offset })
                    {
                        if (coordinatePanel == null || placePolarRadioButton is { Checked: true })
                        {
                            EllipseVerticalDraw();
                        }
                        else if (placeCartesianRadioButton is { Checked: true })
                        {
                            EllipseVerticalCartesianDraw();
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
                    else if (rb is { Checked: true, Text: Polar })
                    {
                        if (place_gb == null || placeRadioButton1 is { Checked: true })
                        {
                            EllipseRadialDraw();
                        }
                        else if (placeRadioButton3 is { Checked: true })
                        {
                            EllipseVerticalDraw();
                        }
                        else if (placeRadioButton4 is { Checked: true })
                        {
                            EllipseTiltedDraw();
                        }
                    }
                    else if (rb is { Checked: true, Text: Cartesian })
                    {
                        if (place_gb == null || placeRadioButton1 is { Checked: true })
                        {
                            EllipseRadialCartesianDraw();
                        }
                        else if (placeRadioButton3 is { Checked: true })
                        {
                            EllipseVerticalCartesianDraw();
                        }
                        else if (placeRadioButton4 is { Checked: true })
                        {
                            EllipseTiltedCartesianDraw();
                        }
                    }
                    break;
                }
        }
    }

    private void NozzleForm_Load(object sender, EventArgs e)
    {
        var steels = _physicalDataService.GetSteels(SteelSource.G34233D1)
            .Select(s => s as object)
            .ToArray();

        steel1_cb.Items.AddRange(steels);
        steel1_cb.SelectedIndex = 0;
        steel2_cb.Items.AddRange(steels);
        steel2_cb.SelectedIndex = 0;
        steel3_cb.Items.AddRange(steels);
        steel3_cb.SelectedIndex = 0;

        var serviceNames = _calculateServices
            .Select(s => s.Name as object)
            .ToArray();
        Gost_cb.Items.AddRange(serviceNames);
        Gost_cb.SelectedIndex = 0;

        MessageBox.Show(((ShellInputData)_shellElement.InputData).ShellType.ToString());

        switch (((ShellInputData)_shellElement.InputData).ShellType)
        {
            case ShellType.Cylindrical:
                placeRadioButton1 = new RadioButton
                {
                    Text = Perpendicular,
                    Checked = true,
                    AutoSize = true,
                    Location = new Point(8, 22),
                    Margin = new Padding(4, 3, 4, 3),
                    Name = nameof(placeRadioButton1),
                    UseVisualStyleBackColor = true,
                };

                placeRadioButton1.CheckedChanged += Place_rb_CheckedChanged;
                //Size = new System.Drawing.Size(31, 19),

                //placerb_1.toggled[bool].emit(False)

                placeRadioButton2 = new RadioButton
                {
                    Text = Transversely,
                    AutoSize = true,
                    Location = new Point(8, 62),
                    Margin = new Padding(4, 3, 4, 3),
                    Name = nameof(placeRadioButton2),
                    UseVisualStyleBackColor = true
                };
                placeRadioButton2.CheckedChanged += Place_rb_CheckedChanged;

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

                placeRadioButton3 = new RadioButton
                {
                    Text = Slanted,
                    AutoSize = true,
                    Location = new Point(8, 102),
                    Margin = new Padding(4, 3, 4, 3),
                    Name = nameof(placeRadioButton3),
                    UseVisualStyleBackColor = true
                };
                placeRadioButton3.CheckedChanged += Place_rb_CheckedChanged;

                mainPanel = new Panel
                {
                    Name = nameof(mainPanel),
                    Location = new Point(2, 15),
                    Size = new Size(300, 310)
                };

                place_gb.Controls.AddRange(new Control[]
                {
                        placeRadioButton1, placeRadioButton2, placeRadioButton3, mainPanel
                });

                place_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.CylRadial)
                                          ?? throw new InvalidOperationException());
                break;
            // TODO: Добавить расчет конуса
            //else if (this.Owner is KonForm)
            //{
            //    TypeElement = "Kon";
            //}

            case ShellType.Elliptical:
                placeRadioButton1 = new RadioButton
                {
                    Text = Perpendicular,
                    Checked = true,
                    AutoSize = true,
                    Location = new Point(8, 30),
                    Margin = new Padding(4, 3, 4, 3),
                    Name = nameof(placeRadioButton1),
                    UseVisualStyleBackColor = true,
                };
                placeRadioButton1.CheckedChanged += Place_rb_CheckedChanged;
                //Size = new System.Drawing.Size(31, 19),

                placeRadioButton2 = new RadioButton
                {
                    Text = Offset,
                    AutoSize = true,
                    Location = new Point(8, 70),
                    Margin = new Padding(4, 3, 4, 3),
                    Name = nameof(placeRadioButton2),
                    UseVisualStyleBackColor = true
                };
                placeRadioButton2.CheckedChanged += Place_rb_CheckedChanged;
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

                mainPanel = new Panel
                {
                    Name = nameof(mainPanel),
                    Location = new Point(2, 15),
                    Size = new Size(400, 310)
                };

                placeCartesianRadioButton = new RadioButton
                {
                    Text = Cartesian,
                    AutoSize = true,
                    Location = new Point(210, 15),
                    Margin = new Padding(4, 3, 4, 3),
                    Name = nameof(placeCartesianRadioButton),
                    UseVisualStyleBackColor = true
                };
                placeCartesianRadioButton.CheckedChanged += PlaceCoordinate_rb_CheckedChanged;
                //Size = new System.Drawing.Size(31, 19),

                placePolarRadioButton = new RadioButton
                {
                    Text = Polar,
                    Checked = true,
                    AutoSize = true,
                    Location = new Point(125, 15),
                    Margin = new Padding(4, 3, 4, 3),
                    Name = nameof(placePolarRadioButton),
                    UseVisualStyleBackColor = true
                };
                placePolarRadioButton.CheckedChanged += PlaceCoordinate_rb_CheckedChanged;

                coordinateLabel = new Label
                {
                    Text = "Система координат:",
                    AutoSize = true,
                    Location = new Point(0, 15)
                };

                coordinatePanel = new Panel
                {
                    Name = nameof(coordinatePanel),
                    Location = new Point(110, 10),
                    Size = new Size(300, 35)
                };

                coordinatePanel.Controls.AddRange(new Control[]
                {
                        coordinateLabel, placeCartesianRadioButton, placePolarRadioButton
                });

                place_gb.Controls.AddRange(new Control[]
                {
                        placeRadioButton1, placeRadioButton2, mainPanel, coordinatePanel
                });

                place_pb.Location = new Point(place_pb.Location.X, place_pb.Location.Y + 35);
                place_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.EllRadial)
                    ?? throw new InvalidOperationException());
                break;
        }

        if (Owner is MainForm)
        {
            steel1_cb.SelectedItem = _shellInputData.Steel;
            steel2_cb.Text = _shellInputData.Steel;
            steel3_cb.Text = _shellInputData.Steel;
            p_tb.Text = _shellInputData.p.ToString();
            t_tb.Text = _shellInputData.t.ToString();

            nameEl_tb.Text = _shellInputData.Name;

            vn_rb.Checked = _shellInputData.IsPressureIn;
            nar_rb.Checked = !_shellInputData.IsPressureIn;

            pressure_gb.Enabled = false;
        }

        Place_Draw(place_gb.Controls
            .OfType<RadioButton>()
            .FirstOrDefault(rb => rb.Checked)
        ?? throw new InvalidOperationException());
    }

    private void Cancel_b_Click(object sender, EventArgs e)
    {
        Hide();
    }

    private bool CollectDataForPreliminarilyCalculation()
    {
        List<string> dataInErr = new();

        _nozzleData = new NozzleInput(_shellElement)
        {
            t = Parameters.GetParam<double>(t_tb.Text, "t", ref dataInErr, NumberStyles.Integer),
            steel1 = steel1_cb.Text,

            d = Parameters.GetParam<double>(d_tb.Text, "d", ref dataInErr),
            s1 = Parameters.GetParam<double>(s1_tb.Text, "s1", ref dataInErr),
            cs = Parameters.GetParam<double>(cs_tb.Text, "cs", ref dataInErr),
            cs1 = Parameters.GetParam<double>(cs1_tb.Text, "cs1", ref dataInErr),
            l1 = Parameters.GetParam<double>(l1_tb.Text, "l1", ref dataInErr),
            fi = Parameters.GetParam<double>(fi_tb.Text, "φ", ref dataInErr),
            fi1 = Parameters.GetParam<double>(fi1_tb.Text, "φ1", ref dataInErr),
            delta = Parameters.GetParam<double>(delta_tb.Text, "delta", ref dataInErr),
            delta1 = Parameters.GetParam<double>(delta1_tb.Text, "delta1", ref dataInErr),
            delta2 = Parameters.GetParam<double>(delta2_tb.Text, "delta2", ref dataInErr),

            SigmaAllow1 = sigmaHandle_cb.Checked
                ? Parameters.GetParam<double>(sigma_d1_tb.Text, "[σ1]", ref dataInErr)
                : default
        };


        //NozzleKind
        var checkedButton = vid_gb.Controls
                .OfType<RadioButton>()
                .FirstOrDefault(rb => rb.Checked);
        if (checkedButton != null)
        {
            _nozzleData.NozzleKind = (NozzleKind)Convert.ToInt32(checkedButton.Text.First().ToString());
        }
        else
        {
            dataInErr.Add("Невозможно определить тип штуцера");
        }


        if (!((ShellInputData)_shellElement.InputData).IsPressureIn)
        {
            if (EHandle_cb.Checked)
            {
                _nozzleData.E1 = Parameters.GetParam<double>(E1_tb.Text, "E1", ref dataInErr);
            }
        }

        if (_nozzleData.NozzleKind is NozzleKind.ImpassWithRing or NozzleKind.PassWithRing or NozzleKind.WithRingAndInPart)
        {
            _nozzleData.steel2 = steel2_cb.Text;
            _nozzleData.l2 = Parameters.GetParam<double>(l2_tb.Text, "l2", ref dataInErr);
            _nozzleData.s2 = Parameters.GetParam<double>(s2_tb.Text, "s2", ref dataInErr);
        }

        if (_nozzleData.NozzleKind is NozzleKind.PassWithoutRing or NozzleKind.PassWithRing or NozzleKind.WithRingAndInPart)
        {
            _nozzleData.steel3 = steel3_cb.Text;
            _nozzleData.l3 = Parameters.GetParam<double>(l3_tb.Text, "l3", ref dataInErr);
            _nozzleData.s3 = Parameters.GetParam<double>(s3_tb.Text, "s3", ref dataInErr);
        }

        var checkedPlaceButton = place_gb.Controls
            .OfType<RadioButton>()
            .FirstOrDefault(rb => rb.Checked)
            ?? throw new InvalidOperationException();

        var checkedRadioButtonText = checkedPlaceButton.Text;

        switch (checkedRadioButtonText)
        {
            case Perpendicular:
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
                    if (_shellInputData.ShellType is ShellType.Elliptical or
                        ShellType.Spherical or ShellType.Torospherical)
                    {
                        _nozzleData.omega = 0;
                    }
                }
                break;
            case Transversely:
                _nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_2;

                if (mainPanel?.Controls[TransverselyTextBoxName] is TextBox transverselyTextBox)
                {
                    _nozzleData.tTransversely = Parameters.GetParam<double>(transverselyTextBox.Text, "tTransversely", ref dataInErr);
                }
                break;
            case Offset:
                switch (_shellInputData.ShellType)
                {
                    case ShellType.Elliptical:
                        _nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_3;

                        if (mainPanel?.Controls[RNozzleTextBoxName] is TextBox rNozzleTextBox)
                        {
                            _nozzleData.ellx = Parameters.GetParam<double>(rNozzleTextBox.Text, "ellx", ref dataInErr);
                        }
                        break;
                    case ShellType.Cylindrical:
                        break;
                }
                break;
            case Slanted:
                switch (_shellInputData.ShellType)
                {
                    case ShellType.Elliptical:
                    case ShellType.Conical:
                        var omegaText = (mainPanel?.Controls[OmegaTextBoxName] as TextBox)?.Text;

                        _nozzleData.omega = Parameters.GetParam<double>(omegaText, "omeg", ref dataInErr);

                        var gammaTextElliptical = (mainPanel?.Controls[GammaTextBoxName] as TextBox)?.Text;

                        _nozzleData.gamma = Parameters.GetParam<double>(gammaTextElliptical, "gamma", ref dataInErr);
                        _nozzleData.Location = _nozzleData.omega == 0
                            ? NozzleLocation.LocationAccordingToParagraph_5_2_2_5
                            : NozzleLocation.LocationAccordingToParagraph_5_2_2_4;

                        break;

                    case ShellType.Spherical:
                    case ShellType.Torospherical:

                        _nozzleData.omega = 0;
                        var gammaTextSpherical = (mainPanel?.Controls[GammaTextBoxName] as TextBox)?.Text;

                        _nozzleData.gamma = Parameters.GetParam<double>(gammaTextSpherical, "gamma", ref dataInErr);

                        _nozzleData.Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_5;
                        break;

                }

                break;
        }

        if ((_nozzleData.cs + _nozzleData.cs1 > _nozzleData.s3) & _nozzleData.s3 > 0)
        {
            dataInErr.Add("cs+cs1 должно быть меньше s3");
        }


        var isNoError = !dataInErr.Any() && _nozzleData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_nozzleData.ErrorList)));
        }

        return isNoError;
    }

    private bool CollectDataForFinishCalculation()
    {
        var dataInErr = new List<string>();

        if (_nozzleData == null)
            throw new InvalidOperationException();

        _nozzleData.Name = name_tb.Text;

        var isNoError = !dataInErr.Any() && _nozzleData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_nozzleData.ErrorList)));
        }

        return isNoError;
    }

    private void PreCalc_b_Click(object sender, EventArgs e)
    {
        if (!CollectDataForPreliminarilyCalculation()) return;

        ICalculatedElement nozzle;

        try
        {
            nozzle = GetCalculateService().Calculate(_nozzleData
                ?? throw new InvalidOperationException());
        }
        catch (CalculateException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        if (nozzle.ErrorList.Any())
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, nozzle.ErrorList));
        }

        calc_b.Enabled = true;
        d0_l.Text = $@"d0={((NozzleCalculated)nozzle).d0:f2} мм";
        p_d_l.Text = $@"[p]={((NozzleCalculated)nozzle).p_d:f2} МПа";
        b_l.Text = $@"b={((NozzleCalculated)nozzle).b:f2} мм";

        MessageBox.Show(Resources.CalcComplete);
    }

    private void Calc_b_Click(object sender, EventArgs e)
    {
        if (!CollectDataForFinishCalculation()) return;

        ICalculatedElement nozzle;

        try
        {
            nozzle = GetCalculateService().Calculate(_nozzleData
             ?? throw new InvalidOperationException());
        }
        catch (CalculateException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        if (nozzle.ErrorList.Any())
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, nozzle.ErrorList));
        }

        d0_l.Text = $@"d0={((NozzleCalculated)nozzle).d0:f2} мм";
        p_d_l.Text = $@"[p]={((NozzleCalculated)nozzle).p_d:f2} МПа";
        b_l.Text = $@"b={((NozzleCalculated)nozzle).b:f2} мм";

        if (Owner is not MainForm main)
        {
            MessageBox.Show($"{nameof(MainForm)} error");
            return;
        }

        main.Word_lv.Items.Add(nozzle.ToString());
        main.ElementsCollection.Add(nozzle);

        MessageBox.Show(Resources.CalcComplete);
        Close();
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

    private ICalculateService<NozzleInput> GetCalculateService()
    {
        return _calculateServices
                   .FirstOrDefault(s => s.Name == Gost_cb.Text)
               ?? throw new InvalidOperationException("Service wasn't found.");
    }
}