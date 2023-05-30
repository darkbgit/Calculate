using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Helpers;
using CalculateVessels.Models;
using CalculateVessels.Output;

namespace CalculateVessels.Forms;

public partial class MainForm : Form
{
    private readonly IFormFactory _formFactory;
    private readonly IOutputService _outputService;

    public MainForm(IFormFactory formFactory, IOutputService outputService)
    {
        InitializeComponent();
        _outputService = outputService;
        _formFactory = formFactory;
        ElementsCollection = new ElementsCollection<ICalculatedElement>();
    }

    public CylindricalShellForm? CylindricalForm;
    public EllipticalShellForm? EllipticalForm;
    public FlatBottomWithAdditionalMomentForm? FlatBottomWithAdditionalMomentForm;
    public NozzleForm? NozzleForm;
    public FlatBottomForm? FlatBottomForm;
    public SaddleForm? SaddleForm;

    public BracketVerticalForm? BracketVerticalForm;
    // public HeatExchangerWithFixedTubePlatesForm heatExchangerWithFixedTubePlatesForm = null;

    internal ElementsCollection<ICalculatedElement> ElementsCollection { get; set; }

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
        if (!ElementsCollection.Any())
        {
            MessageBox.Show("Данных для вывода нет.");
            return;
        }

        if (!TryGetFilePath(file_tb.Text, out var path))
        {
            return;
        }

        try
        {
            _outputService.Output(path, GetOutputType(), ElementsCollection);
            MessageBox.Show("OK");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private static bool TryGetFilePath(string filePath, out string filePathWithFileExtension)
    {
        const string templatesFolder = "Templates";
        const string templateFile = "temp.docx";
        const string fileExtension = ".docx";

        filePathWithFileExtension = string.Empty;

        if (string.IsNullOrWhiteSpace(filePath))
        {
            MessageBox.Show("Введите место сохранения файла");
            return false;
        }

        if (filePath.Last() == '\\')
        {
            MessageBox.Show("Введите имя файла");
            return false;
        }

        filePathWithFileExtension = filePath + fileExtension;
        if (File.Exists(filePathWithFileExtension))
        {
            var bad = true;
            while (bad)
            {
                bad = false;
                try
                {
                    var f1 = File.Open(filePathWithFileExtension, FileMode.Append);
                    f1.Close();
                }
                catch
                {
                    MessageBox.Show($"Закройте {filePathWithFileExtension} и нажмите OK");
                    bad = true;
                }
            }
        }
        else
        {
            try
            {
                var templateFilePath = Path.Combine(templatesFolder, templateFile);
                File.Copy(templateFilePath, filePathWithFileExtension);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Нет шаблона temp.docx");
                return false;
            }
        }

        return true;
    }

    private OutputType GetOutputType()
    {
        return OutputType.Word;
    }

    private void OpenForm<T>(T? form)
     where T : Form
    {
        if (form == null)
        {
            form = _formFactory.Create<T>();
            if (form == null) return;
            form.Owner = this;
            form.Show();
        }
        else
        {
            form.Owner = this;
            form.Show();
        }
    }

    private void Cil_b_Click(object sender, EventArgs e)
    {
        if (CylindricalForm == null)
        {
            CylindricalForm = _formFactory.Create<CylindricalShellForm>();
            if (CylindricalForm == null) return;
            CylindricalForm.Owner = this;
            CylindricalForm.Show();
        }
        else
        {
            CylindricalForm.Owner = this;
            CylindricalForm.Show();
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
        if (EllipticalForm == null)
        {
            EllipticalForm = _formFactory.Create<EllipticalShellForm>();
            if (EllipticalForm == null) return;
            EllipticalForm.Owner = this;
            EllipticalForm.Show();
        }
        else
        {
            EllipticalForm.Owner = this;
            EllipticalForm.Show();
        }
    }

    private void Saddle_b_Click(object sender, EventArgs e)
    {
        OpenForm(SaddleForm);
        //if (SaddleForm == null)
        //{
        //    SaddleForm = _formFactory.Create<SaddleForm>();
        //    if (SaddleForm == null) return;
        //    SaddleForm.Owner = this;
        //    SaddleForm.Show();
        //}
        //else
        //{
        //    SaddleForm.Owner = this;
        //    SaddleForm.Show();
        //}
    }

    private void FlatBottom_b_Click(object sender, EventArgs e)
    {
        if (FlatBottomForm == null)
        {
            FlatBottomForm = _formFactory.Create<FlatBottomForm>();
            if (FlatBottomForm == null) return;
            FlatBottomForm.Owner = this;
            FlatBottomForm.Show();
        }
        else
        {
            FlatBottomForm.Owner = this;
            FlatBottomForm.Show();
        }
    }

    private void FlatBottomWithAdditionalMoment_b_Click(object sender, EventArgs e)
    {
        if (FlatBottomWithAdditionalMomentForm == null)
        {
            FlatBottomWithAdditionalMomentForm = _formFactory.Create<FlatBottomWithAdditionalMomentForm>();
            if (FlatBottomWithAdditionalMomentForm == null) return;
            FlatBottomWithAdditionalMomentForm.Owner = this;
            FlatBottomWithAdditionalMomentForm.Show();
        }
        else
        {
            FlatBottomWithAdditionalMomentForm.Owner = this;
            FlatBottomWithAdditionalMomentForm.Show();
        }
    }

    private void HeatExchangerWithFixedTubePlate_b_Click(object sender, EventArgs e)
    {
        //     if (heatExchangerWithFixedTubePlatesForm == null)
        //     {
        //         heatExchangerWithFixedTubePlatesForm = new HeatExchangerWithFixedTubePlatesForm { Owner = this };
        //         heatExchangerWithFixedTubePlatesForm.Show();
        //     }
        //     else
        //     {
        //         heatExchangerWithFixedTubePlatesForm.Owner = this;
        //         heatExchangerWithFixedTubePlatesForm.Show();
        //     }
    }

    private void BracketVertical_b_Click(object sender, EventArgs e)
    {
        OpenForm(BracketVerticalForm);
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
        if (lv.Items.Count <= 1) return;

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

    private void Up_b_Click(object sender, EventArgs e)
    {
        var idx = Word_lv.SelectedItems[0].Index;

        MoveSelectedItemListView(Word_lv, idx, true);

        (ElementsCollection[idx], ElementsCollection[idx - 1]) =
            (ElementsCollection[idx - 1], ElementsCollection[idx]);
    }

    private void Down_b_Click(object sender, EventArgs e)
    {
        var idx = Word_lv.SelectedItems[0].Index;

        MoveSelectedItemListView(Word_lv, idx, false);

        (ElementsCollection[idx], ElementsCollection[idx + 1]) =
            (ElementsCollection[idx + 1], ElementsCollection[idx]);

    }

    private void Del_b_Click(object sender, EventArgs e)
    {
        var idx = Word_lv.SelectedItems[0].Index;

        Word_lv.Items.RemoveAt(idx);
        ElementsCollection.RemoveAt(idx);
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

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (sender is not ToolStripItem { Name: "AboutToolStripMenuItem" }) return;

        var aboutBoxForm = _formFactory.Create<AboutBoxForm>();
        aboutBoxForm?.ShowDialog();
    }
}
