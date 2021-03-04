using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace calcNet
{
    public partial class GostEllForm : Form
    {
        public GostEllForm()
        {
            InitializeComponent();
        }

        private void Cancel_b_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GostEllForm_Load(object sender, EventArgs e)
        {
            type_cb.SelectedIndex = 0;
            //string list = "";
            //if (type_cb.SelectedIndex == 0)
            //{
            //    list = "ell025vn";
            //}
            //XmlDocument doc = new XmlDocument();
            //doc.Load(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data\data.xml"));
            //var root = doc.DocumentElement;
            //XmlNode ell_list = root.SelectSingleNode("ell_list").SelectSingleNode(list);
            //foreach (XmlNode s in ell_list.ChildNodes)
            //{
            //    D_cb.Items.Add(s.Attributes["D"].Value);
            //}
        }

        private void Type_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string list = "";
            switch (type_cb.SelectedIndex)
            {
                case 0:
                    list = "ell025vn";
                    break;
                case 1:
                    list = "ell025nar";
                    break;
                case 2:
                    list = "ell02vn";
                    break;
            }
            
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data\data.xml"));
            var root = doc.DocumentElement;
            XmlNode ell_list = root.SelectSingleNode("ell_list").SelectSingleNode(list);
            D_cb.Items.Clear();
            foreach (XmlNode D in ell_list.ChildNodes)
            {
                D_cb.Items.Add(D.Attributes["D"].Value);
            }
            D_cb.SelectedIndex = 0;
        }

        private void D_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string list = "";
            switch (type_cb.SelectedIndex)
            {
                case 0:
                    list = "ell025vn";
                    break;
                case 1:
                    list = "ell025nar";
                    break;
                case 2:
                    list = "ell02vn";
                    break;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data\data.xml"));
            var root = doc.DocumentElement;
            XmlNode ell_list = root.SelectSingleNode("ell_list")
                .SelectSingleNode(list)
                .SelectSingleNode("//Dia[@D = '" + D_cb.Text + "']");
            s_cb.Items.Clear();
            foreach (XmlNode s in ell_list.ChildNodes)
            {
                s_cb.Items.Add(s.Attributes["s"].Value);
            }
            s_cb.SelectedIndex = 0;
        }

        private void Scb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string list = "";
            switch (type_cb.SelectedIndex)
            {
                case 0:
                    list = "ell025vn";
                    break;
                case 1:
                    list = "ell025nar";
                    break;
                case 2:
                    list = "ell02vn";
                    break;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data\data.xml"));
            var root = doc.DocumentElement;
            XmlNode s_list = root.SelectSingleNode("ell_list")
                .SelectSingleNode(list)
                .SelectSingleNode("//Dia[@D = '" + D_cb.Text + "']")
                .SelectSingleNode("el[@s ='" + s_cb.Text + "']");
            
            H_tb.Text = s_list.Attributes["H"].Value;
            h1_tb.Text = s_list.Attributes["h1"].Value;
            
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
