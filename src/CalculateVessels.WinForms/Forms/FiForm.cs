using System.Windows.Forms;

namespace CalculateVessels.Forms;

public partial class FiForm : Form
{
    public FiForm()
    {
        InitializeComponent();
    }

    private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        const string FI_TEXT_BOX_NAME = "fi_tb";

        if (sender is LinkLabel linkLabel && Owner?.Controls[FI_TEXT_BOX_NAME] is TextBox fiTextBox)
        {
            fiTextBox.Text = linkLabel.Text;
        }

        Close();
    }
}