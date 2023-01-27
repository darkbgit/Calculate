using System;
using System.Windows.Forms;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Forms;

public partial class MessageBoxCheckBox : Form
{
    public MessageBoxCheckBox(IElement element)
    {
        InitializeComponent();
        _element = element;
    }

    private readonly IElement _element;

    private void OK_b_Click(object sender, EventArgs e)
    {
        // Owner.Hide();
        // Close();
        // if (nozzle_cb.Checked)
        // {
        //     var nf = new NozzleForm(_element)
        //     {
        //         Owner = this.Owner,
        //     };

        //     nf.Show();
        // }
    }
}