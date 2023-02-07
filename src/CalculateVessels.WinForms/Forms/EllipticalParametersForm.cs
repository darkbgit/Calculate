using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.PhysicalData.Gost6533.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels.Forms;

public partial class EllipticalParametersForm : Form
{
    private readonly EllipsesParameters _ellipses;

    public EllipticalParametersForm(IPhysicalDataService physicalDataService)
    {
        InitializeComponent();
        _ellipses = physicalDataService.GetEllipsesParameters();
    }

    private void Cancel_b_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void GostEllForm_Load(object sender, EventArgs e)
    {
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

        var diameters = type_cb.SelectedIndex switch
        {
            0 => _ellipses.EllipticalBottomInsideDiameter025.Keys
                .Select(eb => eb.ToString(CultureInfo.CurrentCulture))
                .ToArray<object>(),
            1 => _ellipses.EllipticalBottomOutsideDiameter025.Keys
                .Select(eb => eb.ToString(CultureInfo.CurrentCulture))
                .ToArray<object>(),
            _ => null
        };

        if (diameters == null)
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

        var sParametersList = GetEllipseSParametersByDiameter(diameter);
        var sList = sParametersList?.Select(s => s.s.ToString(CultureInfo.CurrentCulture))
            .ToArray<object>(); ;

        if (sList == null)
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

        var sParameters = GetEllipseSParametersByDiameter(diameter)
            ?.FirstOrDefault(p => p.s.Equals(s));

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
        }
        Close();
    }

    private IEnumerable<EllipseSValueParameters>? GetEllipseSParametersByDiameter(double diameter)
    {
        List<EllipseSValueParameters>? result = null;

        _ = type_cb.SelectedIndex switch
        {
            0 => _ellipses.EllipticalBottomInsideDiameter025
                .TryGetValue(diameter, out result),
            1 => _ellipses.EllipticalBottomOutsideDiameter025
                .TryGetValue(diameter, out result),
            _ => false
        };

        return result;
    }

    private static double CalculateC3(double s) => s * 0.15;
}