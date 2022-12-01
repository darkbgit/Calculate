using System.Windows.Forms;

namespace CalculateVessels
{
    public partial class FiForm : Form
    {


        public FiForm()
        {
            InitializeComponent();
        }

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // приводим отправителя к элементу типа LinkLabel
            LinkLabel ll = sender as LinkLabel;
            //MessageBox.Show(ll.Text);
            if (this.Owner != null)
            {
                (Owner.Controls["fi_tb"] as TextBox).Text = ll.Text;
            }
            this.Close();
        }
    }
}
