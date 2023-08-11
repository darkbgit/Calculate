using CalculateVessels.Data.Public.Enums;
using CalculateVessels.Data.Public.Models;
using CalculateVessels.DataAccess.EF.PhysicalData.Entities;

namespace CalculateVessels.Controls;

public partial class SteelControl : UserControl
{
    private List<SteelWithParametersDto> _steels = new();

    public SteelControl()
    {
        InitializeComponent();
    }

    public void SelectSteel(SteelWithIdsDto steel)
    {
        steelComboBox.SelectedItem = steel.SteelName;

        if (steel.DesignResourceId == null && steel.MinMaxThicknessId == null)
        {
            return;
        }

        var steelWithParam = GetSteelByName(steelComboBox.Text);

        if (steelWithParam.Any(s => s.DesignResource != null))
        {

            ShowDesignerResource((DesignResourceType?)steel.DesignResourceId);
        }
        else
        {
            HideDesignResource();
        }

        if (steelWithParam.Any(s => s.MinMaxThickness != null))
        {
            var thickness = steelWithParam
                .Where(s => s.MinMaxThickness != null)
                .Select(s => s.MinMaxThickness)
                .Distinct()
                .ToList();

            ShowThickness(thickness, steel.MinMaxThicknessId);
        }
        else
        {
            HideThickness();
        }
    }


    public void SetSteels(IEnumerable<SteelWithParametersDto> steels)
    {
        _steels = steels.ToList();
        var steelNames = _steels
            .Select(s => s.Name as object)
            .Distinct()
            .ToArray();

        steelComboBox.Items.AddRange(steelNames);

        steelComboBox.SelectedIndex = 0;
    }

    public SteelWithIdsDto GetSteel()
    {
        var result = _steels
            .First(s => s.Name == steelComboBox.Text
            && (designResourceComboBox.Items.OfType<DesignResourceType>().Any() ? s.DesignResource == (DesignResourceType)designResourceComboBox.SelectedItem : s.DesignResource == null)
            && (thicknessComboBox.Items.OfType<MinMaxThickness>().Any() ? s.MinMaxThickness == thicknessComboBox.SelectedItem : s.MinMaxThickness == null));

        return new SteelWithIdsDto
        {
            SteelId = result.Id,
            SteelName = result.Name,
            DesignResourceId = (int?)result.DesignResource,
            MinMaxThicknessId = result.MinMaxThickness?.Id,
        };
    }

    private void SteelComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        var steel = GetSteelByName(steelComboBox.Text);

        if (steel.Any(s => s.DesignResource != null))
        {
            ShowDesignerResource();
        }
        else
        {
            HideDesignResource();
        }

        if (steel.Any(s => s.MinMaxThickness != null))
        {
            var thickness = steel
                .Where(s => s.MinMaxThickness != null)
                .Select(s => s.MinMaxThickness)
                .ToList();

            ShowThickness(thickness);
        }
        else
        {
            HideThickness();
        }
    }

    private void ShowThickness(IEnumerable<MinMaxThickness> thickness, int? showedMinMaxThicknessId = null)
    {
        thicknessLabel.Visible = true;
        thicknessComboBox.Visible = true;
        thicknessComboBox.Items.Clear();
        thicknessComboBox.Items
            .AddRange(thickness
                .Select(t => t as object)
                .Distinct()
                .ToArray());

        var selectedIndex = showedMinMaxThicknessId == null
            ? 0
            : thicknessComboBox.Items.IndexOf(thicknessComboBox.Items.Cast<MinMaxThickness>().First(t => t.Id == showedMinMaxThicknessId));

        thicknessComboBox.SelectedIndex = selectedIndex;
    }

    private void HideThickness()
    {
        thicknessLabel.Visible = false;
        thicknessComboBox.Visible = false;
        thicknessComboBox.Items.Clear();
    }

    private void ShowDesignerResource(DesignResourceType? designResource = DesignResourceType.Standard)
    {
        if (designResource == null)
        {
            designResourceLabel.Visible = false;
            designResourceComboBox.Visible = false;
            designResourceComboBox.Items.Clear();
            return;
        }

        designResourceLabel.Visible = true;
        designResourceComboBox.Visible = true;
        designResourceComboBox.Items.Clear();
        designResourceComboBox.Items
            .AddRange(Enum.GetValues(typeof(DesignResourceType))
                .Cast<DesignResourceType>()
                .Select(dr => dr as object)
                .ToArray());

        var selectedIndex = designResourceComboBox.Items.IndexOf(designResourceComboBox.Items.Cast<DesignResourceType>().First(t => t == designResource));

        designResourceComboBox.SelectedIndex = selectedIndex;
    }
    private void HideDesignResource()
    {
        designResourceLabel.Visible = false;
        designResourceComboBox.Visible = false;
        designResourceComboBox.Items.Clear();
    }


    private List<SteelWithParametersDto> GetSteelByName(string steelName)
    {
        var steels = _steels
            .Where(s => s.Name == steelName)
            .ToList();

        return steels;
    }
}