using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.PhysicalData.Gost6533;

namespace CalculateVessels
{
    public partial class GostEllForm : Form
    {
        public GostEllForm()
        {
            InitializeComponent();
            _elepses = Physical.Gost6533.GetEllipsesList();
        }

        private void Cancel_b_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private readonly EllipsesList _elepses;

        private void GostEllForm_Load(object sender, EventArgs e)
        {
            type_cb.SelectedIndex = 0;
        }

        private void Type_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var diameters = type_cb.SelectedIndex switch
            {
                0 => _elepses.Ell025In
                    .Select(eb => eb.Diameter.ToString(CultureInfo.CurrentCulture))
                    .ToArray<object>(),
                1 => _elepses.Ell025Out
                    .Select(eb => eb.Diameter.ToString(CultureInfo.CurrentCulture))
                    .ToArray<object>(),
                _ => null
            };

            if (diameters != null)
            {
                D_cb.Items.Clear();
                D_cb.Items.AddRange(diameters);
                D_cb.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Error");
                this.Close();
            }
        }

        private void D_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sList = type_cb.SelectedIndex switch
            {
                0 => _elepses.Ell025In
                    .FirstOrDefault(eb=>
                        eb.Diameter.Equals(Convert.ToDouble(D_cb.Text)))
                    ?.SValue
                    ?.Select(s => s.s.ToString(CultureInfo.CurrentCulture))
                    .ToArray<object>(),
                1 => _elepses.Ell025Out
                    .FirstOrDefault(eb =>
                        eb.Diameter.Equals(Convert.ToDouble(D_cb.Text)))
                    ?.SValue
                    ?.Select(s => s.s.ToString(CultureInfo.CurrentCulture))
                    .ToArray<object>(),
                _ => null
            };
            if (sList != null)
            {
                s_cb.Items.Clear();
                s_cb.Items.AddRange(sList);
                s_cb.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Error");
                this.Close();
            }
        }

        private void Scb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ellipse = type_cb.SelectedIndex switch
            {
                0 => _elepses.Ell025In
                    .FirstOrDefault(eb =>
                        eb.Diameter.Equals(Convert.ToDouble(D_cb.Text)))
                    ?.SValue
                    .FirstOrDefault(s => 
                        s.s.Equals(Convert.ToDouble(s_cb.Text))),
                1 => _elepses.Ell025Out
                    .FirstOrDefault(eb =>
                        eb.Diameter.Equals(Convert.ToDouble(D_cb.Text)))
                    ?.SValue
                    .FirstOrDefault(s =>
                        s.s.Equals(Convert.ToDouble(s_cb.Text))),
                _ => null
            };

            if (ellipse != null)
            {
                H_tb.Text = ellipse.H.ToString(CultureInfo.CurrentCulture);
                h1_tb.Text = ellipse.h1.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                MessageBox.Show("Error");
                this.Close();
            }
        }

        private void OK_b_Click(object sender, EventArgs e)
        {
            if (this.Owner is EllForm ef)
            {
                ef.D_tb.Text = D_cb.Text;
                ef.H_tb.Text = H_tb.Text;
                ef.h1_tb.Text = h1_tb.Text;
                ef.s_tb.Text = s_cb.Text;
                ef.c3_tb.Text = (Convert.ToInt32(s_cb.Text) * 0.15).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            this.Close();
        }
    }
}
