using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Elements.Bottoms.Enums;
using CalculateVessels.Core.Elements.Bottoms.FlatBottom;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Forms.MiddleForms;
using CalculateVessels.Helpers;
using FluentValidation;

namespace CalculateVessels.Forms;

public sealed partial class FlatBottomForm : FlatBottomFormMiddle
{
    private TextBox? _DTextBox;
    private TextBox? _D2TextBox;
    private TextBox? _D3TextBox;
    private TextBox? _DcpTextBox;
    private TextBox? _sTextBox;
    private TextBox? _aTextBox;
    private TextBox? _rTextBox;
    private TextBox? _h1TextBox;
    private TextBox? _gammaTextBox;
    private TextBox? _s2TextBox;
    private TextBox? _s3TextBox;
    private TextBox? _s4TextBox;

    public FlatBottomForm(IEnumerable<ICalculateService<FlatBottomInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<FlatBottomInput> validator)
    : base(calculateServices, physicalDataService, validator)
    {
        InitializeComponent();
    }

    protected override void LoadInputData(FlatBottomInput inputData)
    {
        throw new NotImplementedException();
    }

    protected override string GetServiceName()
    {
        return Gost_cb.Text;
    }

    private void FlatBottomForm_Load(object sender, EventArgs e)
    {
        LoadSteelsToComboBox(steel_cb, SteelSource.G34233D1);
        LoadCalculateServicesNamesToComboBox(Gost_cb);

        type_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.pldn1)
                                 ?? throw new InvalidOperationException());
        TypeDraw(rb1);
    }

    private void FlatBottomForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (sender is not FlatBottomForm) return;

        if (Owner is not MainForm { FlatBottomForm: not null } main) return;

        main.FlatBottomForm = null;
    }

    private void Cancel_b_Click(object sender, EventArgs e)
    {
        Hide();
    }

    private void Rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        var typePbImage = Resources.ResourceManager.GetObject("pldn" + rb.Text)
                          ?? throw new InvalidOperationException();
        type_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(typePbImage)
                                 ?? throw new InvalidOperationException());
        TypeDraw(sender);
    }

    protected override bool TryCollectInputData(out FlatBottomInput inputData)
    {
        var dataInErr = new List<string>();

        inputData = new FlatBottomInput
        {
            Name = name_tb.Text,
            s1 = Parameters.GetParam<double>(s1_tb.Text, "s1", dataInErr),
            t = Parameters.GetParam<double>(t_tb.Text, "t", dataInErr, NumberStyles.Integer),
            Steel = steel_cb.Text,
            p = Parameters.GetParam<double>(p_tb.Text, "p", dataInErr),
            fi = Parameters.GetParam<double>(fi_tb.Text, "φ", dataInErr),
            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", dataInErr),
            FlatBottomType = Parameters.GetParam<int>(type_gb.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked)?.Text,
                "SaddleType", dataInErr),
            SigmaAllow = sigmaHandle_cb.Checked
                ? Parameters.GetParam<double>(sigma_d_tb.Text, "[σ]", dataInErr)
                : default
        };

        switch (inputData.FlatBottomType)
        {
            case 1:
            case 2:
                inputData.D = Parameters.GetParam<double>(_DTextBox?.Text, "D", dataInErr);
                inputData.s = Parameters.GetParam<double>(_sTextBox?.Text, "s", dataInErr);
                inputData.a = Parameters.GetParam<double>(_aTextBox?.Text, "a", dataInErr);
                break;
            case 3:
            case 4:
            case 5:
                inputData.D = Parameters.GetParam<double>(_DTextBox?.Text, "D", dataInErr);
                inputData.s = Parameters.GetParam<double>(_sTextBox?.Text, "s", dataInErr);
                break;
            case 6:
                goto case 2;
            case 7:
            case 8:
                goto case 5;
            case 9:
                inputData.D = Parameters.GetParam<double>(_DTextBox?.Text, "D", dataInErr);
                inputData.s = Parameters.GetParam<double>(_sTextBox?.Text, "s", dataInErr);
                inputData.r = Parameters.GetParam<double>(_rTextBox?.Text, "r", dataInErr);
                inputData.h1 = Parameters.GetParam<double>(_h1TextBox?.Text, "h1", dataInErr);
                break;
            case 10:
                inputData.D = Parameters.GetParam<double>(_DTextBox?.Text, "D", dataInErr);
                inputData.s = Parameters.GetParam<double>(_sTextBox?.Text, "s", dataInErr);
                inputData.r = Parameters.GetParam<double>(_rTextBox?.Text, "r", dataInErr);
                inputData.gamma = Parameters.GetParam<double>(_gammaTextBox?.Text, "gamma", dataInErr);
                inputData.s2 = Parameters.GetParam<double>(_s2TextBox?.Text, "s2", dataInErr);
                break;
            case 11:
                inputData.D2 = Parameters.GetParam<double>(_D2TextBox?.Text, "D2", dataInErr);
                inputData.D3 = Parameters.GetParam<double>(_D3TextBox?.Text, "D3", dataInErr);
                inputData.s2 = Parameters.GetParam<double>(_s2TextBox?.Text, "s2", dataInErr);
                break;
            case 12:
                inputData.D2 = Parameters.GetParam<double>(_D2TextBox?.Text, "D2", dataInErr);
                inputData.Dcp = Parameters.GetParam<double>(_DcpTextBox?.Text, "Dcp", dataInErr);
                inputData.s2 = Parameters.GetParam<double>(_s2TextBox?.Text, "s2", dataInErr);
                break;
            case 13:
            case 14:
            case 15:
                dataInErr.Add("SaddleType 13, 14, 15 unsupported");
                break;
            default:
                dataInErr.Add("SaddleType error");
                break;
        }

        if (!hole_cb.Checked)
        {
            inputData.Hole = HoleInFlatBottom.WithoutHole;
        }
        else
        {
            var d = Parameters.GetParam<double>(holed_tb.Text, "d", dataInErr);
            if (oneHole_rb.Checked)
            {
                inputData.Hole = HoleInFlatBottom.OneHole;
                inputData.d = d;
            }
            else
            {
                inputData.Hole = HoleInFlatBottom.MoreThenOneHole;
                inputData.di = d;
            }
        }

        if (!dataInErr.Any()) return true;

        MessageBox.Show(string.Join(Environment.NewLine, dataInErr));
        return false;
    }

    private void Otv_cb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is CheckBox cb)
        {
            otv_gb.Enabled = cb.Checked;
        }
    }

    private void Type1_2_6Draw()
    {
        typePanel.Controls.Clear();

        var lab_1_1 = new Label
        {
            AutoSize = true,
            Text = "Внутренний диаметр смежного элемента, D:",
            Location = new Point(8, 8)
        };
        _DTextBox = new TextBox
        {
            Name = "D_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 4)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 8)
        };

        var lab_2_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Толщина стенки смежного элемента, s:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 38)
        };
        _sTextBox = new TextBox
        {
            Name = "s_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 34)
        };
        var lab_2_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 38)
        };

        var lab_3_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Катет сварного шва, а:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 68)
        };
        _aTextBox = new TextBox
        {
            Name = "a_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 64)
        };
        var lab_3_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 68)
        };

        typePanel.Controls.AddRange(new Control[]
        {
            lab_1_1, _DTextBox, lab_1_2, lab_2_1, _sTextBox, lab_2_2, lab_3_1, _aTextBox, lab_3_2
        });
    }

    private void Type3_4_5_7_8Draw()
    {
        var lab_1_1 = new Label
        {
            AutoSize = true,
            Text = "Внутренний диаметр смежного элемента, D:",
            Location = new Point(8, 8)
        };
        _DTextBox = new TextBox
        {
            Name = "D_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 4)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 8)
        };

        var lab_2_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Толщина стенки смежного элемента, s:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 38)
        };
        _sTextBox = new TextBox
        {
            Name = "s_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 34)
        };
        var lab_2_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 38)
        };

        typePanel.Controls.AddRange(new Control[]
        {
            lab_1_1, _DTextBox, lab_1_2, lab_2_1, _sTextBox, lab_2_2
        });
    }

    private void Type9_13Draw()
    {
        var lab_1_1 = new Label
        {
            AutoSize = true,
            Text = "Внутренний диаметр смежного элемента, D:",
            Location = new Point(8, 8)
        };
        _DTextBox = new TextBox
        {
            Name = "D_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 4)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 8)
        };

        var lab_2_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Толщина стенки смежного элемента, s:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 38)
        };
        _sTextBox = new TextBox
        {
            Name = "s_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 34)
        };
        var lab_2_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 38)
        };

        var lab_3_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Радиус выточки, r:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 68)
        };
        _rTextBox = new TextBox
        {
            Name = "r_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 64)
        };
        var lab_3_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 68)
        };

        var lab_4_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Высота выточки, h1:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 98)
        };
        _h1TextBox = new TextBox
        {
            Name = "h1_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 94)
        };
        var lab_4_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 98)
        };

        typePanel.Controls.AddRange(new Control[]
        {
            lab_1_1, _DTextBox, lab_1_2, lab_2_1, _sTextBox, lab_2_2, lab_3_1, _rTextBox, lab_3_2, lab_4_1, _h1TextBox, lab_4_2
        });
    }

    private void Type10Draw()
    {
        var lab_1_1 = new Label
        {
            AutoSize = true,
            Text = "Внутренний диаметр смежного элемента, D:",
            Location = new Point(8, 8)
        };
        _DTextBox = new TextBox
        {
            Name = "D_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 4)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 8)
        };

        var lab_2_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Толщина стенки смежного элемента, s:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 38)
        };
        _sTextBox = new TextBox
        {
            Name = "s_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 34)
        };
        var lab_2_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 38)
        };

        var lab_3_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Радиус выточки, r:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 68)
        };
        _rTextBox = new TextBox
        {
            Name = "r_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 64)
        };
        var lab_3_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 68)
        };

        var lab_4_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Угол выхода выточки, γ:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 98)
        };
        _gammaTextBox = new TextBox
        {
            Name = "gamma_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 94)
        };
        var lab_4_2 = new Label
        {
            Text = "°",
            Location = new Point(318, 98)
        };

        var lab_5_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Толщина, s2:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 128)
        };
        _s2TextBox = new TextBox
        {
            Name = "s2_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 124)
        };
        var lab_5_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 128)
        };

        typePanel.Controls.AddRange(new Control[]
        {
            lab_1_1, _DTextBox, lab_1_2, lab_2_1, _sTextBox, lab_2_2, lab_3_1, _rTextBox, lab_3_2, lab_4_1, _gammaTextBox, lab_4_2, lab_5_1, _s2TextBox, lab_5_2
        });
    }

    private void Type11Draw()
    {
        var lab_1_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Диаметр утоненной части, D2:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 8)
        };
        _D2TextBox = new TextBox
        {
            Name = "D2_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 4)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 8)
        };

        var lab_2_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Диаметр болтовой окружности, D3:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 38)
        };
        _D3TextBox = new TextBox
        {
            Name = "D3_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 34)
        };
        var lab_2_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 38)
        };

        var lab_3_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Толщина в зоне утонения, s2:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 68)
        };
        _s2TextBox = new TextBox
        {
            Name = "s2_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 64)
        };
        var lab_3_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 68)
        };

        typePanel.Controls.AddRange(new Control[]
        {
            lab_1_1, _D2TextBox, lab_1_2, lab_2_1, _D3TextBox, lab_2_2, lab_3_1, _s2TextBox, lab_3_2
        });
    }

    private void Type12Draw()
    {
        var lab_1_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Диаметр утоненной части, D2:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 8)
        };
        _D2TextBox = new TextBox
        {
            Name = "D2_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 4)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 8)
        };

        var lab_2_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Расчетный диаметр прокладки, Dсп:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 38)
        };
        _DcpTextBox = new TextBox
        {
            Name = "Dsp_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 34)
        };
        var lab_2_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 38)
        };

        var lab_3_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Толщина в зоне утонения, s2:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 68)
        };
        _s2TextBox = new TextBox
        {
            Name = "s2_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 64)
        };
        var lab_3_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 68)
        };

        typePanel.Controls.AddRange(new Control[]
        {
            lab_1_1, _D2TextBox, lab_1_2, lab_2_1, _DcpTextBox, lab_2_2, lab_3_1, _s2TextBox, lab_3_2
        });
    }

    private void Type14Draw()
    {
        var lab_1_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Диаметр утоненной части, D2:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 8)
        };
        _D2TextBox = new TextBox
        {
            Name = "D2_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 4)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 8)
        };

        var lab_2_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Диаметр болтовой окружности, D3:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 38)
        };
        _D3TextBox = new TextBox
        {
            Name = "D3_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 34)
        };
        var lab_2_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 38)
        };

        var lab_3_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Расчетный диаметр прокладки, Dсп:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 68)
        };
        _DcpTextBox = new TextBox
        {
            Name = "Dsp_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 64)
        };
        var lab_3_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 68)
        };

        var lab_4_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Толщина в зоне утонения, s2:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 98)
        };
        _s2TextBox = new TextBox
        {
            Name = "s2_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 94)
        };
        var lab_4_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 98)
        };

        var lab_5_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Толщина вне уплотнения, s3:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 128)
        };
        _s3TextBox = new TextBox
        {
            Name = "s3_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 124)
        };
        var lab_5_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 128)
        };

        typePanel.Controls.AddRange(new Control[]
        {
            lab_1_1, _D2TextBox, lab_1_2,
            lab_2_1, _D3TextBox, lab_2_2,
            lab_3_1, _DcpTextBox, lab_3_2,
            lab_4_1, _s2TextBox, lab_4_2,
            lab_5_1, _s3TextBox, lab_5_2
        });
    }

    private void Type15Draw()
    {
        var lab_1_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Диаметр утоненной части, D2:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 8)
        };
        _D2TextBox = new TextBox
        {
            Name = "D2_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 4)
        };
        var lab_1_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 8)
        };

        var lab_2_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Диаметр болтовой окружности, D3:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 38)
        };
        _D3TextBox = new TextBox
        {
            Name = "D3_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 34)
        };
        var lab_2_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 38)
        };

        var lab_3_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Расчетный диаметр прокладки, Dсп:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 68)
        };
        _DcpTextBox = new TextBox
        {
            Name = "Dsp_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 64)
        };
        var lab_3_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 68)
        };

        var lab_4_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Толщина в зоне утонения, s2:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 98)
        };
        _s2TextBox = new TextBox
        {
            Name = "s2_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 94)
        };
        var lab_4_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 98)
        };

        var lab_5_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Толщина вне уплотнения, s3:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 128)
        };
        _s3TextBox = new TextBox
        {
            Name = "s3_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 124)
        };
        var lab_5_2 = new Label
        {
            Text = "мм",
            Location = new Point(318, 128)
        };

        var lab_6_1 = new Label
        {
            Size = new Size(252, 15),
            Text = "Ширина паза под прокладку, s4:",
            TextAlign = ContentAlignment.MiddleRight,
            Location = new Point(8, 158)
        };
        _s4TextBox = new TextBox
        {
            Name = "s4_tb",
            Size = new Size(46, 23),
            Location = new Point(264, 154)
        };
        Label lab_6_2 = new()
        {
            Text = "мм",
            Location = new Point(318, 158)
        };

        typePanel.Controls.AddRange(new Control[]
        {
            lab_1_1, _D2TextBox, lab_1_2,
            lab_2_1, _D3TextBox, lab_2_2,
            lab_3_1, _DcpTextBox, lab_3_2,
            lab_4_1, _s2TextBox, lab_4_2,
            lab_5_1, _s3TextBox, lab_5_2,
            lab_6_1, _s4TextBox, lab_6_2
        });
    }

    private void TypeDraw(object sender)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        typePanel.Controls.Clear();

        switch (Convert.ToInt32(rb.Text))
        {
            case 1:
            case 2:
                Type1_2_6Draw();
                break;
            case 3:
            case 4:
            case 5:
                Type3_4_5_7_8Draw();
                break;
            case 6:
                goto case 2;
            case 7:
            case 8:
                goto case 5;
            case 9:
                Type9_13Draw();
                break;
            case 10:
                Type10Draw();
                break;
            case 11:
                Type11Draw();
                break;
            case 12:
                Type12Draw();
                break;
            case 13:
                goto case 9;
            case 14:
                Type14Draw();
                break;
            case 15:
                Type15Draw();
                break;
        }
    }

    private void PreCalculateButton_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;
        p_d_l.Text = string.Empty;
        calc_btn.Enabled = false;

        if (!TryCalculate(out var bottom)) return;

        if (bottom == null) throw new NullReferenceException();

        calc_btn.Enabled = true;
        scalc_l.Text = $@"sp={((FlatBottomCalculated)bottom).s1:f3} мм";
        p_d_l.Text = $@"pd={((FlatBottomCalculated)bottom).p_d:f2} МПа";

        MessageBox.Show(Resources.CalcComplete);
    }

    private void CalculateButton_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;

        if (!TryCalculate(out var bottom)) return;

        if (bottom == null) throw new NullReferenceException();


        calc_btn.Enabled = true;
        scalc_l.Text = $@"sp={((FlatBottomCalculated)bottom).s1:f3} мм";
        p_d_l.Text = $@"pd={((FlatBottomCalculated)bottom).p_d:f2} МПа";

        SetCalculatedElementToStorage(Owner, bottom);

        MessageBox.Show(Resources.CalcComplete);
        Close();
    }
}