using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.PhysicalData.Gost6533;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels.Forms;

public partial class GostEllForm : Form
{
    private readonly EllipsesList _ellipses;

    public GostEllForm()
    {
        InitializeComponent();
        _ellipses = Physical.Gost6533.GetEllipsesList();
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
        var diameters = type_cb.SelectedIndex switch
        {
            0 => _ellipses.Ell025In.Keys
                .Select(eb => eb.ToString(CultureInfo.CurrentCulture))
                .ToArray<object>(),
            1 => _ellipses.Ell025Out.Keys
                .Select(eb => eb.ToString(CultureInfo.CurrentCulture))
                .ToArray<object>(),
            _ => null
        };

        if (diameters == null)
        {
            MessageBox.Show("Error");
            return;
        }

        D_cb.Items.Clear();
        D_cb.Items.AddRange(diameters);
        D_cb.SelectedIndex = 0;
    }

    private void D_cb_SelectedIndexChanged(object sender, EventArgs e)
    {
        var sList = type_cb.SelectedIndex switch
        {
            0 => _ellipses.Ell025In[Convert.ToDouble(D_cb.Text)]
                ?.Select(s => s.s.ToString(CultureInfo.CurrentCulture))
                .ToArray<object>(),
            1 => _ellipses.Ell025Out[Convert.ToDouble(D_cb.Text)]
                ?.Select(s => s.s.ToString(CultureInfo.CurrentCulture))
                .ToArray<object>(),
            _ => null
        };

        if (sList == null)
        {
            MessageBox.Show("Error");
            return;
        }

        s_cb.Items.Clear();
        s_cb.Items.AddRange(sList);
        s_cb.SelectedIndex = 0;

    }

    private void Scb_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ellipse = type_cb.SelectedIndex switch
        {
            0 => _ellipses.Ell025In[Convert.ToDouble(D_cb.Text)]
                .FirstOrDefault(s =>
                    s.s.Equals(Convert.ToDouble(s_cb.Text))),
            1 => _ellipses.Ell025Out[Convert.ToDouble(D_cb.Text)]
                .FirstOrDefault(s =>
                    s.s.Equals(Convert.ToDouble(s_cb.Text))),
            _ => null
        };

        if (ellipse == null)
        {
            MessageBox.Show("Error");
            return;
        }

        H_tb.Text = ellipse.H.ToString(CultureInfo.CurrentCulture);
        h1_tb.Text = ellipse.h1.ToString(CultureInfo.CurrentCulture);
    }

    private void OK_b_Click(object sender, EventArgs e)
    {
        if (Owner is EllipticalShellForm ef)
        {
            ef.D_tb.Text = D_cb.Text;
            ef.H_tb.Text = H_tb.Text;
            ef.h1_tb.Text = h1_tb.Text;
            ef.s_tb.Text = s_cb.Text;
            ef.c3_tb.Text = (Convert.ToInt32(s_cb.Text) * 0.15).ToString(CultureInfo.InvariantCulture);
        }
        Close();
    }
}