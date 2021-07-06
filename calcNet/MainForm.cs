using System;
using System.Collections.Generic;
using System.Windows.Forms;
using calcNet;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels
{
    public partial class MainForm : Form
    {
              
        public MainForm()
        {
            InitializeComponent();
        }

        public CilForm cf = null;
        public NozzleForm nf = null;
        public EllForm ef = null;

        public PldnForm pdf = null;
        // TODO: KonForm 

        public SaddleForm saddleForm = null;
        



        private void MakeWord_b_Click(object sender, EventArgs e)
        {
            if (Word_lv.Items.Count > 0)
            {
                string f = file_tb.Text;
                if (f != "")
                {
                    f += ".docx";
                    if (System.IO.File.Exists(f))
                    {
                        try
                        {
                            var f1 = System.IO.File.Open(f, System.IO.FileMode.Append);
                            f1.Close();
                        }
                        catch
                        {
                            System.Windows.Forms.MessageBox.Show("Закройте" + f + "и нажмите OK");
                        }

                    }
                    else
                    {
                        System.IO.File.Copy("temp.docx", f);
                    }
                    List<int> bibliography = new List<int>();
                    //try
                    //{
                        foreach (IElement element in Elements.ElementsList)
                        {
                            element.MakeWord(f);
                        }

                        //for (int i = 0; i < DataWordOut.DataArr.Count; i++)
                        //{
                        //    //int inx = item.id;
                        //    if (DataWordOut.DataArr[i].id != 0)
                        //    {
                        //        switch (DataWordOut.DataArr[i].calculatedElementType)
                        //        {
                        //            case CalculatedElementType.Cylindrical:
                        //                MakeWord.MakeWord_cil(DataWordOut.DataArr[i].Data_In, DataWordOut.DataArr[i].Data_Out, f);
                        //                bibliography.Add(2);
                        //                break;
                        //            case CalculatedElementType.Conical:
                        //                bibliography.Add(2);
                        //                break;
                        //            case CalculatedElementType.Elliptical:
                        //                MakeWord.MakeWord_ell(DataWordOut.DataArr[i].Data_In, DataWordOut.DataArr[i].Data_Out, f);
                        //                bibliography.Add(2);
                        //                break;
                        //            case CalculatedElementType.CylindricalWhithNozzle:
                        //            case CalculatedElementType.ConicalWhithNozzle:
                        //            case CalculatedElementType.EllipticalWhithNozzle:
                        //                MakeWord.MakeWord_nozzle(DataWordOut.DataArr[i].Data_In, DataWordOut.DataArr[i].Data_Out, DataWordOut.DataArr[i].DataN_In, DataWordOut.DataArr[i].DataN_Out, f);
                        //                bibliography.Add(3);
                        //                break;
                        //            case CalculatedElementType.Saddle:
                        //                bibliography.Add(5);
                        //                break;
                        //            case CalculatedElementType.Heatexchenge:
                        //                bibliography.Add(7);
                        //                break;
                        //            case CalculatedElementType.FlatBottom:
                        //                bibliography.Add(2);
                        //                break;
                        //        }
                        //    }
                        //}
                        MakeWord.MakeLit(bibliography, f);
                        System.Windows.Forms.MessageBox.Show("OK");
                    //}
                    //catch (Exception)
                    //{
                        System.Windows.Forms.MessageBox.Show("Error" + e.ToString());
                    //}

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Введите имя файла");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Данных для вывода нет");
            }
            
            
        }

        private void Cil_b_Click(object sender, EventArgs e)
        {
            //CilForm cf = (CilForm) Application.OpenForms["CilForm"]; // создаем
            if (cf == null)
            {
                //cf.Dispose();
                cf = new CilForm { Owner = this };
                cf.Show();
            }
            else
            {
                cf.Owner = this;
                cf.Show();
            }
        }

        private void Kon_b_Click(object sender, EventArgs e)
        {
            //if (nf == null)
            //{
            //    nf = new NozzleForm { Owner = this };
            //    nf.Show();
            //}
            //else
            //{
            //    nf.Owner = this;
            //    nf.Show();
            //}
        }


        private void Ell_b_Click(object sender, EventArgs e)
        {
            if (ef == null)
            {
                ef = new EllForm { Owner = this };
                ef.Show();
            }
            else
            {
                ef.Owner = this;
                ef.Show();
            }
        }

        private void Saddle_b_Click(object sender, EventArgs e)
        {
            //CilForm cf = (CilForm) Application.OpenForms["CilForm"]; // создаем
            if (saddleForm == null)
            {
                //cf.Dispose();
                saddleForm = new SaddleForm { Owner = this };
                saddleForm.Show();
            }
            else
            {
                saddleForm.Owner = this;
                saddleForm.Show();
            }
        }

        private void Word_lv_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                del_b.Enabled = true;
                if (e.ItemIndex < Word_lv.Items.Count - 1)
                {
                    down_b.Enabled = true;
                }
                if (e.ItemIndex > 0)
                {
                    up_b.Enabled = true;
                }

            }
            else
            {
                del_b.Enabled = false;
                up_b.Enabled = false;
                down_b.Enabled = false;
            }
        }

        private static void MoveSelectedItemListView(System.Windows.Forms.ListView lv, int idx, bool moveUp)
        {
            if (lv.Items.Count > 1)
            {
                int offset = 0;
                //int idx = lv.SelectedItems[0].Index;
                if (idx >= 0)
                {
                    if (moveUp)
                    {
                        offset = -1;
                    }
                    else
                    {
                        offset = 1;
                    }
                }

                if (offset != 0)
                {
                    lv.BeginUpdate();

                    int selitem = idx + offset;
                    for (int i = 0; i < lv.Items[idx].SubItems.Count; i++)
                    {
                        string cache = lv.Items[selitem].SubItems[i].Text;
                        lv.Items[selitem].SubItems[i].Text = lv.Items[idx].SubItems[i].Text;
                        lv.Items[idx].SubItems[i].Text = cache;
                    }

                    lv.Focus();
                    lv.Items[selitem].Selected = true;
                    lv.EnsureVisible(selitem);

                    lv.EndUpdate();
                }
            }
        }
    

        private void Up_b_Click(object sender, EventArgs e)
        {
            int idx = Word_lv.SelectedItems[0].Index;
            MoveSelectedItemListView(Word_lv, idx,  true);
            var temp = Elements.ElementsList[idx];
            Elements.ElementsList[idx] = Elements.ElementsList[idx - 1];
            Elements.ElementsList[idx - 1] = temp;

        }

        private void Down_b_Click(object sender, EventArgs e)
        {
            int idx = Word_lv.SelectedItems[0].Index;
            MoveSelectedItemListView(Word_lv, idx, false);
            var temp = Elements.ElementsList[idx];
            Elements.ElementsList[idx] = Elements.ElementsList[idx + 1];
            Elements.ElementsList[idx + 1] = temp;
        }

        private void Del_b_Click(object sender, EventArgs e)
        {
            int idx = Word_lv.SelectedItems[0].Index;
            Word_lv.SelectedItems[0].Remove();
            Elements.ElementsList.RemoveAt(idx);
            //Word_lv.SelectedItems.Clear();

        }

        private void Word_lv_Leave(object sender, EventArgs e)
        {
            //up_b.Enabled = false;
            //down_b.Enabled = false;
            //del_b.Enabled = false;
            //if (sender is System.Windows.Forms.ListView lv)
            //{
            //    if (lv.Items.Count > 1)
            //    {
            //        Word_lv.SelectedItems[0].Selected = false;
            //    }
            //}
            
        }

        private void Pldn_b_Click(object sender, EventArgs e)
        {
            if (pdf == null)
            {
                pdf = new PldnForm { Owner = this };
                pdf.Show();
            }
            else
            {
                pdf.Owner = this;
                pdf.Show();
            }
        }

        private void MenuUp_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //if (e.ClickedItem.Name == "AboutToolStripMenuItem")
            //{
            //    AboutBox abf = new AboutBox();
            //    abf.ShowDialog();
            //}
        }

        private void AboutToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (sender is ToolStripItem it && it.Name == "AboutToolStripMenuItem")
            {
                AboutBox abf = new AboutBox();
                abf.ShowDialog();
            }
        }
    }
}
