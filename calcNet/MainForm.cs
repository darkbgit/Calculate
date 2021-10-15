using CalculateVessels.Core.Bibliography;
using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Models;

namespace CalculateVessels
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }


        public CilForm cylindricalForm = null;
        public EllForm ef = null;
        public FlatBottomWithAdditionalMomentForm flatBottomWithAdditionalMomentForm = null;
        public NozzleForm nf = null;
        public FlatBottomForm pdf = null;
        public SaddleForm saddleForm = null;
        public HeatExchangerWithFixedTubePlatesForm heatExchangerWithFixedTubePlatesForm = null;

        public ElementsCollection ElementsCollection { get; set; } = new();

        // TODO: KonForm 



        //private void MakeWord_b_Click(object sender, EventArgs e)
        //{
        //    if (Word_lv.Items.Count > 0)
        //    {
        //        string f = file_tb.Text;
        //        if (f != "")
        //        {
        //            f += ".docx";
        //            if (System.IO.File.Exists(f))
        //            {
        //                try
        //                {
        //                    var f1 = System.IO.File.Open(f, System.IO.FileMode.Append);
        //                    f1.Close();
        //                }
        //                catch
        //                {
        //                    System.Windows.Forms.MessageBox.Show("Закройте" + f + "и нажмите OK");
        //                }

        //            }
        //            else
        //            {
        //                try
        //                {
        //                    System.IO.File.Copy("temp.docx", f);
        //                }
        //                catch
        //                {
        //                    System.Windows.Forms.MessageBox.Show("Нет шаблона temp.docx");
        //                }
        //            }
        //            var bibliography = new List<string>();

        //            foreach (IElement element in Elements.ElementsList)
        //            {
        //                try
        //                {
        //                    element.MakeWord(f);
        //                    bibliography = bibliography.Union(element.Bibliography).ToList();
        //                }

        //                catch (Exception)
        //                {
        //                    System.Windows.Forms.MessageBox.Show($"Couldn't create word file for element {element}" + e.ToString());
        //                }
        //            }

        //            try
        //            {
        //                Bibliography.AddBibliography(bibliography, f);
        //                System.Windows.Forms.MessageBox.Show("OK");
        //            }
        //            catch
        //            {
        //                System.Windows.Forms.MessageBox.Show("Couldn't create word file for bibliography" + e.ToString());
        //            }


        //        }
        //        else
        //        {
        //            System.Windows.Forms.MessageBox.Show("Введите имя файла");
        //        }
        //    }
        //    else
        //    {
        //        System.Windows.Forms.MessageBox.Show("Данных для вывода нет");
        //    }

        //}

        private void MakeWord_b_Click(object sender, EventArgs e)
        {
            var path = GetFilePath(file_tb.Text);

            if (path == null) return;

            if (!ElementsCollection.Elements.Any())
            {
                MessageBox.Show("Данных для вывода нет");
            }

            MakeWord(ElementsCollection, path);
        }

        private static string GetFilePath(string filePath)
        {
            if (filePath == "")
            {
                MessageBox.Show("Введите место сохранения файла");
                return null;
            }

            if (filePath.Last() == '\\')
            {
                MessageBox.Show("Введите имя файла");
                return null;
            }

            filePath += ".docx";
            if (System.IO.File.Exists(filePath))
            {
                var bad = true;
                while (bad)
                {
                    bad = false;
                    try
                    {
                        var f1 = System.IO.File.Open(filePath, System.IO.FileMode.Append);
                        f1.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Закройте" + filePath + "и нажмите OK");
                        bad = true;
                    }
                }
            }
            else
            {
                try
                {
                    System.IO.File.Copy("temp.docx", filePath);
                }
                catch
                {
                    MessageBox.Show("Нет шаблона temp.docx");
                    return null;
                }
            }
            return filePath;
        }
    

        private static void MakeWord(ElementsCollection elements, string filePath)
        {
            if (!elements.Elements.Any()) return;

            List<string> bibliography = new();

            foreach (var elementForCalculate in elements.Elements)
            {
                try
                {
                    elementForCalculate.Element.MakeWord(filePath);
                    bibliography = bibliography.Union(elementForCalculate.Element.Bibliography).ToList();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Couldn't create word file for element {elementForCalculate.Element}" + e.ToString());
                }
            }

            try
            {
                Bibliography.AddBibliography(bibliography, filePath);
                MessageBox.Show("OK");
            }
            catch (Exception e)
            {
                MessageBox.Show("Couldn't create word file for bibliography" + e.ToString());
            }
        }

        private void Cil_b_Click(object sender, EventArgs e)
        {
            //CilForm cf = (CilForm) Application.OpenForms["CilForm"]; // создаем
            if (cylindricalForm == null)
            {
                //cf.Dispose();
                cylindricalForm = new CilForm { Owner = this };
                cylindricalForm.Show();
            }
            else
            {
                cylindricalForm.Owner = this;
                cylindricalForm.Show();
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
            if (saddleForm == null)
            {
                saddleForm = new SaddleForm { Owner = this };
                saddleForm.Show();
            }
            else
            {
                saddleForm.Owner = this;
                saddleForm.Show();
            }
        }

        private void FlatBottom_b_Click(object sender, EventArgs e)
        {
            if (pdf == null)
            {
                pdf = new FlatBottomForm { Owner = this };
                pdf.Show();
            }
            else
            {
                pdf.Owner = this;
                pdf.Show();
            }
        }

        private void FlatBottomWithAdditionalMoment_b_Click(object sender, EventArgs e)
        {
            if (flatBottomWithAdditionalMomentForm == null)
            {
                flatBottomWithAdditionalMomentForm = new FlatBottomWithAdditionalMomentForm { Owner = this };
                flatBottomWithAdditionalMomentForm.Show();
            }
            else
            {
                flatBottomWithAdditionalMomentForm.Owner = this;
                flatBottomWithAdditionalMomentForm.Show();
            }
        }

        private void HeatExchangerWithFixedTubePlate_b_Click(object sender, EventArgs e)
        {
            if (heatExchangerWithFixedTubePlatesForm == null)
            {
                heatExchangerWithFixedTubePlatesForm = new HeatExchangerWithFixedTubePlatesForm { Owner = this };
                heatExchangerWithFixedTubePlatesForm.Show();
            }
            else
            {
                heatExchangerWithFixedTubePlatesForm.Owner = this;
                heatExchangerWithFixedTubePlatesForm.Show();
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

        private static void MoveSelectedItemListView(ListView lv, int idx, bool moveUp)
        {
            if (lv.Items.Count > 1)
            {
                int offset = 0;
                //int idx = lv.SelectedItems[0].Index;
                if (idx >= 0)
                {
                    offset = moveUp ? -1 : 1;
                }

                if (offset != 0)
                {
                    lv.BeginUpdate();

                    int selectItem = idx + offset;
                    for (int i = 0; i < lv.Items[idx].SubItems.Count; i++)
                    {
                        (lv.Items[selectItem].SubItems[i].Text, lv.Items[idx].SubItems[i].Text) = 
                            (lv.Items[idx].SubItems[i].Text, lv.Items[selectItem].SubItems[i].Text);
                    }

                    lv.Focus();
                    lv.Items[selectItem].Selected = true;
                    lv.EnsureVisible(selectItem);

                    lv.EndUpdate();
                }
            }
        }
    
        private void Up_b_Click(object sender, EventArgs e)
        {
            int idx = Word_lv.SelectedItems[0].Index;

            MoveSelectedItemListView(Word_lv, idx,  true);

            (ElementsCollection.Elements[idx], ElementsCollection.Elements[idx - 1]) = 
                (ElementsCollection.Elements[idx - 1], ElementsCollection.Elements[idx]);
        }

        private void Down_b_Click(object sender, EventArgs e)
        {
            int idx = Word_lv.SelectedItems[0].Index;

            MoveSelectedItemListView(Word_lv, idx, false);

            (ElementsCollection.Elements[idx], ElementsCollection.Elements[idx + 1]) =
                (ElementsCollection.Elements[idx + 1], ElementsCollection.Elements[idx]);

        }

        private void Del_b_Click(object sender, EventArgs e)
        {
            int idx = Word_lv.SelectedItems[0].Index;

            Word_lv.Items.RemoveAt(idx);
            ElementsCollection.Elements.RemoveAt(idx);
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
            if (sender is ToolStripItem {Name: "AboutToolStripMenuItem"})
            {
                var abf = new AboutBox();
                abf.ShowDialog();
            }
        }




    }
}
