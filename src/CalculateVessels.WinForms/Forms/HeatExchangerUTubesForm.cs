using System.Globalization;
using CalculateVessels.Core.Elements.HeatExchangers.Enums;
using CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerUTube;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Forms.MiddleForms;
using CalculateVessels.Helpers;
using FluentValidation;

namespace CalculateVessels.Forms;

public sealed partial class HeatExchangerUTubesForm : HeatExchangerUTubesMiddleForm
{
    public HeatExchangerUTubesForm(IEnumerable<ICalculateService<HeatExchangerUTubeInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<HeatExchangerUTubeInput> validator)
        : base(calculateServices, physicalDataService, validator)
    {
        InitializeComponent();
    }

    protected override void LoadInputData(HeatExchangerUTubeInput inputData)
    {
        throw new NotImplementedException();
    }

    protected override string GetServiceName()
    {
        return servicesComboBox.Text;
    }

    protected override bool TryCollectInputData(out HeatExchangerUTubeInput inputData)
    {
        var dataInErr = new List<string>();

        inputData = new HeatExchangerUTubeInput
        {
            Name = Name_tb.Text,
            Steelp = steelpComboBox.Text,
            TK = Parameters.GetParam<double>(TCalculateK_tb.Text, "TK", dataInErr),
            TT = Parameters.GetParam<double>(TCalculateT_tb.Text, "TT", dataInErr),
            d0 = Parameters.GetParam<double>(d0_tb.Text, "d0", dataInErr),
            sT = Parameters.GetParam<double>(sT_tb.Text, "sT", dataInErr),
            pM = Parameters.GetParam<double>(pM_tb.Text, "pM", dataInErr),
            pT = Parameters.GetParam<double>(pT_tb.Text, "pT", dataInErr),
            IsSpecialRequirements = isSpecialRequirementsComboBox.Checked,
            a1 = Parameters.GetParam<double>(a1_tb.Text, "a1", dataInErr),
            Dcp = Parameters.GetParam<double>(Dcp_tb.Text, "Dcp", dataInErr),
            c = Parameters.GetParam<double>(c_tb.Text, "c", dataInErr),
            sp = Parameters.GetParam<double>(sp_tb.Text, "sp", dataInErr),
            DE = Parameters.GetParam<double>(DE_tb.Text, "DE", dataInErr),
            DB = Parameters.GetParam<double>(DB_tb.Text, "DB", dataInErr),
            spr = Parameters.GetParam<double>(spr_tb.Text, "spr", dataInErr)
        };

        var fixTubeType = tubeFixTypeGroupBox.Controls
            .OfType<RadioButton>()
            .First(rb => rb.Checked)
            .Name
            .Last()
            .ToString();

        var fixTubeTypeNumber = Parameters.GetParam<int>(fixTubeType, "tube fixing type", dataInErr, NumberStyles.Integer);

        inputData.TubeFixType = (UTubeFixType)fixTubeTypeNumber;

        if (!dataInErr.Any()) return true;

        MessageBox.Show(string.Join(Environment.NewLine, dataInErr));
        return false;
    }

    private void HeatExchangerUTubesForm_Load(object sender, EventArgs e)
    {
        LoadSteelsToComboBox(steelpComboBox, SteelSource.G34233D1);
        LoadCalculateServicesNamesToComboBox(servicesComboBox);
    }

    private void HeatExchangerUTubesForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (sender is not HeatExchangerUTubesForm) return;

        if (Owner is not MainForm { HeatExchangerUTubesForm: not null } main) return;

        main.HeatExchangerUTubesForm = null;
    }

    private void Cancel_btn_Click(object sender, EventArgs e)
    {
        Hide();
    }

    private void PreCalculateButton_Click(object sender, EventArgs e)
    {
        if (!TryCalculate(out var heatExchanger)) return;

        if (heatExchanger == null) throw new NullReferenceException();

        calculateButton.Enabled = true;
        MessageBox.Show(Resources.CalcComplete);
    }

    private void CalculateButton_Click(object sender, EventArgs e)
    {
        if (!TryCalculate(out var heatExchanger)) return;

        if (heatExchanger == null) throw new NullReferenceException();

        SetCalculatedElementToStorage(Owner, heatExchanger);

        MessageBox.Show(Resources.CalcComplete);
        Close();
    }
}