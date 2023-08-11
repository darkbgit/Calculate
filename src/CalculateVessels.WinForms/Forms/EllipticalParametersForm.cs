using System.Globalization;
using CalculateVessels.Data.Public.Enums;
using CalculateVessels.Data.Public.Interfaces;

namespace CalculateVessels.Forms;

public partial class EllipticalParametersForm : Form
{
    private readonly IPhysicalDataService _physicalDataService;

    public EllipticalParametersForm(IPhysicalDataService physicalDataService)
    {
        _physicalDataService = physicalDataService;
        InitializeComponent();
    }

    private void Cancel_b_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void GostEllForm_Load(object sender, EventArgs e)
    {
        var types = Task.Run(() => _physicalDataService.GetEllipsesTypes6533Async()).Result
            .ToList()
            .Select(el => el.ToString())
            .ToArray<object>();

        type_cb.Items.Clear();
        type_cb.Items.AddRange(types);

        type_cb.SelectedIndex = 0;
    }

    private void Type_cb_SelectedIndexChanged(object sender, EventArgs e)
    {
        D_cb.Items.Clear();
        D_cb.Text = string.Empty;
        s_cb.Items.Clear();
        s_cb.Text = string.Empty;
        H_tb.Text = string.Empty;
        h1_tb.Text = string.Empty;

        var type = Enum.Parse<EllipticalBottom6533Type>(type_cb.Text);

        var diameters = Task.Run(() => _physicalDataService.GetEllipsesDiameters6533Async(type)).Result
            .Select(d => d as object)
            .ToArray();

        if (!diameters.Any())
        {
            MessageBox.Show("Error");
            return;
        }

        D_cb.Items.AddRange(diameters);
        D_cb.SelectedIndex = 0;
    }

    private void D_cb_SelectedIndexChanged(object sender, EventArgs e)
    {
        s_cb.Items.Clear();
        s_cb.Text = string.Empty;
        H_tb.Text = string.Empty;
        h1_tb.Text = string.Empty;

        if (!double.TryParse(D_cb.Text, out var diameter))
        {
            MessageBox.Show("Error");
            return;
        }

        var type = Enum.Parse<EllipticalBottom6533Type>(type_cb.Text);

        var sList = Task.Run(() => _physicalDataService.GetEllipsesThickness6533Async(type, diameter)).Result
                .Select(s => s as object)
                .ToArray();

        if (!sList.Any())
        {
            MessageBox.Show("Error");
            return;
        }

        s_cb.Items.AddRange(sList);
        s_cb.SelectedIndex = 0;
    }

    private void Scb_SelectedIndexChanged(object sender, EventArgs e)
    {
        H_tb.Text = string.Empty;
        h1_tb.Text = string.Empty;

        if (!double.TryParse(D_cb.Text, out var diameter))
        {
            MessageBox.Show("Error");
            return;
        }

        if (!double.TryParse(s_cb.Text, out var s))
        {
            MessageBox.Show("Error");
            return;
        }
        var type = Enum.Parse<EllipticalBottom6533Type>(type_cb.Text);

        var sParameters = Task.Run((() => _physicalDataService.GetEllipsesParameters6533Async(type, diameter, s)))
            .Result;

        if (sParameters == null)
        {
            MessageBox.Show("Error");
            return;
        }

        H_tb.Text = sParameters.H.ToString(CultureInfo.CurrentCulture);
        h1_tb.Text = sParameters.h1.ToString(CultureInfo.CurrentCulture);
    }

    private void OK_b_Click(object sender, EventArgs e)
    {
        if (Owner is EllipticalShellForm ef)
        {
            ef.D_tb.Text = D_cb.Text;
            ef.H_tb.Text = H_tb.Text;
            ef.h1_tb.Text = h1_tb.Text;
            ef.s_tb.Text = s_cb.Text;
            ef.c3_tb.Text = CalculateC3(Convert.ToInt32(s_cb.Text)).ToString(CultureInfo.InvariantCulture);
            ef.name_tb.Text = $@"Днище {D_cb.Text}-{h1_tb.Text}-{H_tb.Text} ГОСТ 6533-78";
        }
        Close();
    }

    private static double CalculateC3(double s) => s * 0.15;
}