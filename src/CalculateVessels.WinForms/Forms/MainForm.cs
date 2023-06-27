using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core;
using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Elements.Shells.Conical;
using CalculateVessels.Core.Elements.Shells.Cylindrical;
using CalculateVessels.Core.Elements.Shells.Elliptical;
using CalculateVessels.Core.Elements.Shells.Nozzle;
using CalculateVessels.Core.Elements.Supports.Saddle;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Persistance.Enums;
using CalculateVessels.Helpers;
using CalculateVessels.Output;

namespace CalculateVessels.Forms;

public partial class MainForm : Form
{
    private readonly IFormFactory _formFactory;
    private readonly IOutputService _outputService;
    private readonly IPersistanceService _persistanceService;

    public MainForm(IFormFactory formFactory,
     IOutputService outputService,
     IPersistanceService persistanceService)
    {
        InitializeComponent();
        _outputService = outputService;
        _formFactory = formFactory;
        _persistanceService = persistanceService;
    }

    public CylindricalShellForm? CylindricalForm;
    public EllipticalShellForm? EllipticalForm;
    public ConicalShellForm? ConicalForm;
    public FlatBottomWithAdditionalMomentForm? FlatBottomWithAdditionalMomentForm;
    public NozzleForm? NozzleForm;
    public FlatBottomForm? FlatBottomForm;
    public SaddleForm? SaddleForm;
    public BracketVerticalForm? BracketVerticalForm;
    public HeatExchangerStationaryTubePlatesForm? HeatExchangerStationaryTubePlatesForm;


    private void MakeWord_b_Click(object sender, EventArgs e)
    {
        var elements = calculatedElementsControl.GetElements()
            .ToList();

        if (!elements.Any())
        {
            MessageBox.Show("Данных для вывода нет.");
            return;
        }

        if (!TryGetFilePath(filePathTextBox.Text, out var path))
        {
            return;
        }

        try
        {
            _outputService.Output(path, GetOutputType(), elements);
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

    private void OpenForm<T>(ref T? form)
     where T : Form
    {
        if (form == null)
        {
            form = _formFactory.Create<T>();
            if (form == null) return;
        }

        form.Owner = this;
        form.Show();
    }

    private void Cylindrical_btn_Click(object sender, EventArgs e)
    {
        OpenForm(ref CylindricalForm);
    }

    private void Conical_btn_Click(object sender, EventArgs e)
    {
        OpenForm(ref ConicalForm);
    }

    private void Elliptical_btn_Click(object sender, EventArgs e)
    {
        OpenForm(ref EllipticalForm);
    }

    private void Saddle_btn_Click(object sender, EventArgs e)
    {
        OpenForm(ref SaddleForm);
    }

    private void FlatBottom_btn_Click(object sender, EventArgs e)
    {
        OpenForm(ref FlatBottomForm);
    }

    private void FlatBottomWithAdditionalMoment_btn_Click(object sender, EventArgs e)
    {
        OpenForm(ref FlatBottomWithAdditionalMomentForm);
    }

    private void HeatExchangerStationaryTubePlates_btn_Click(object sender, EventArgs e)
    {
        OpenForm(ref HeatExchangerStationaryTubePlatesForm);
    }

    private void BracketVertical_btn_Click(object sender, EventArgs e)
    {
        OpenForm(ref BracketVerticalForm);
    }

    internal bool IsAnyWindowOpen()
    {
        return CylindricalForm != null ||
               ConicalForm != null ||
               EllipticalForm != null ||
               NozzleForm != null ||
               SaddleForm != null ||
               FlatBottomForm != null ||
               FlatBottomWithAdditionalMomentForm != null ||
               HeatExchangerStationaryTubePlatesForm != null;
    }

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (sender is not ToolStripItem { Name: "AboutToolStripMenuItem" }) return;

        var aboutBoxForm = _formFactory.Create<AboutBoxForm>();
        aboutBoxForm?.ShowDialog();
    }

    private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        saveFileDialogRst.ShowDialog();

        if (string.IsNullOrWhiteSpace(saveFileDialogRst.FileName))
            return;

        var elements = calculatedElementsControl.GetElements()
            .ToList();

        if (!elements.Any())
        {
            MessageBox.Show("Нет данных для сохранения.");
            return;
        }

        try
        {
            _persistanceService.Save(elements, saveFileDialogRst.FileName, SaveType.Json);
        }
        catch (PersistanceException ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void OpenToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        openFileDialog.ShowDialog();

        if (string.IsNullOrWhiteSpace(openFileDialog.FileName))
        {
            return;
        }

        IEnumerable<ICalculatedElement> calculatedElements;

        try
        {
            calculatedElements = _persistanceService.Open(openFileDialog.FileName, SaveType.Json);
        }
        catch (PersistanceException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        calculatedElementsControl.LoadElements(calculatedElements);
    }

    public void CheckAndCreateFormForEditElement(ICalculatedElement calculatedElement, int elementIndex)
    {
        if (calculatedElement is not CalculatedElement element) return;

        var type = element.Type;

        switch (type)
        {
            case nameof(CylindricalShellCalculated):
                CheckAndCreateForm(ref CylindricalForm);
                CylindricalForm?.Show((CylindricalShellInput)((CylindricalShellCalculated)element).InputData, elementIndex);
                break;
            case nameof(ConicalShellCalculated):
                CheckAndCreateForm(ref ConicalForm);
                ConicalForm?.Show((ConicalShellInput)((ConicalShellCalculated)element).InputData, elementIndex);
                break;
            case nameof(EllipticalShellCalculated):
                CheckAndCreateForm(ref EllipticalForm);
                EllipticalForm?.Show((EllipticalShellInput)((EllipticalShellCalculated)element).InputData, elementIndex);
                break;
            case nameof(NozzleCalculated):
                CheckAndCreateForm(ref NozzleForm);
                NozzleForm?.Show((NozzleInput)((NozzleCalculated)element).InputData, elementIndex);
                break;
            case nameof(SaddleCalculated):
                CheckAndCreateForm(ref SaddleForm);
                SaddleForm?.Show((SaddleInput)((SaddleCalculated)element).InputData, elementIndex);
                break;
            default:
                throw new InvalidOperationException($"Couldn't create form for type {type}.");
        }
    }

    private void ChooseFileNameButton_Click(object sender, EventArgs e)
    {
        saveFileDialogDocx.ShowDialog();

        if (string.IsNullOrWhiteSpace(saveFileDialogDocx.FileName))
            return;

        filePathTextBox.Text = saveFileDialogDocx.FileName;
    }

    private void CheckAndCreateForm<T>(ref T? form)
        where T : Form
    {
        if (form != null)
        {
            MessageBox.Show($"Other {form.Text} is opened.");
        }

        form = _formFactory.Create<T>()
            ?? throw new NullReferenceException("Unable to create form.");
        form.Owner = this;
    }
}
