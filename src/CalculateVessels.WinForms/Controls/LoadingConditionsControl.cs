using CalculateVessels.Core.Base;
using CalculateVessels.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels.Controls;

public partial class LoadingConditionsControl : UserControl
{
    private const string AutoStrengthParameters = "auto";

    public LoadingConditionsControl()
    {
        InitializeComponent();
    }

    public bool Any() => loadingConditionsListView.Items.Cast<ListViewItem>().Any();

    public IEnumerable<LoadingCondition> GetLoadingConditions()
    {
        var dataInErr = new List<string>();

        var loadingConditions = loadingConditionsListView.Items
            .Cast<ListViewItem>()
            .ToList()
            .Select(i => new LoadingCondition
            {
                OrdinalNumber = Convert.ToInt32(i.SubItems[ordinalNumber_ch.Index].Text),
                IsPressureIn = i.SubItems[pressureType_ch.Index].Text == Properties.Resources.InsidePressure,
                p = Parameters.GetParam<double>(i.SubItems[p_ch.Index].Text, "p", dataInErr),
                t = Parameters.GetParam<double>(i.SubItems[t_ch.Index].Text, "t", dataInErr),
                SigmaAllow = i.SubItems[sigmaAllow_ch.Index].Text == AutoStrengthParameters
                    ? default
                    : Parameters.GetParam<double>(i.SubItems[sigmaAllow_ch.Index].Text, "[σ]", dataInErr),
                EAllow = i.SubItems[EAllow_ch.Index].Text == AutoStrengthParameters
                    ? default
                    : Parameters.GetParam<double>(i.SubItems[EAllow_ch.Index].Text, "E", dataInErr),
            })
            .ToList();

        if (!dataInErr.Any()) return loadingConditions;

        MessageBox.Show(string.Join(Environment.NewLine, dataInErr));
        return Enumerable.Empty<LoadingCondition>();
    }

    public void SetLoadingConditions(IEnumerable<LoadingCondition> loadingConditions)
    {
        loadingConditions
            .ToList()
            .ForEach(lc => AddLoadingCondition(lc));
    }

    private void AddLoadingCondition_btn_Click(object sender, EventArgs e)
    {
        var loadingCondition = ParentForm?.Controls
                                   .OfType<LoadingConditionControl>()
                                   .FirstOrDefault()
                                   ?.GetLoadingCondition()
            ?? throw new Exception("LoadingConditionGroupBox didn't found.");

        AddLoadingCondition(loadingCondition);
    }

    private void AddLoadingCondition(LoadingCondition loadingCondition, int maxLoadingConditions = 3)
    {
        if (loadingConditionsListView.Items.Count >= maxLoadingConditions)
        {
            MessageBox.Show($@"Максимальное число условий нагружения - {maxLoadingConditions}.");
            return;
        }

        var ordinalNumber = loadingConditionsListView.Items.Count + 1;

        var loadingConditions = new string[]
        {
            loadingCondition.IsPressureIn ? Properties.Resources.InsidePressure : Properties.Resources.OutsidePressure,
            loadingCondition.p.ToString(CultureInfo.CurrentCulture),
            loadingCondition.t.ToString(CultureInfo.CurrentCulture),
            loadingCondition.SigmaAllow == 0 ? AutoStrengthParameters : loadingCondition.SigmaAllow.ToString(CultureInfo.CurrentCulture),
            loadingCondition.EAllow == 0 ? AutoStrengthParameters : loadingCondition.EAllow.ToString(CultureInfo.CurrentCulture)
        };

        if (loadingConditionsListView.Items.Cast<ListViewItem>()
            .Any(i => i.SubItems.Cast<ListViewItem.ListViewSubItem>()
                .Select(si => si.Text)
                .Skip(1)
                .ToList()
                .SequenceEqual(loadingConditions)))
        {
            MessageBox.Show("Условие нагружение с данными параметрами уже существует.");
            return;
        }

        var newItem = new ListViewItem(ordinalNumber.ToString());

        newItem.SubItems.AddRange(loadingConditions);

        loadingConditionsListView.Items.Add(newItem);
    }

    private void DeleteLoadingCondition_btn_Click(object sender, EventArgs e)
    {
        if (loadingConditionsListView.SelectedItems.Count == 0)
        {
            return;
        }

        var selectedItem = loadingConditionsListView.SelectedItems.Cast<ListViewItem>().First();

        loadingConditionsListView.Items.Remove(selectedItem);

        int firstOrdinalNumber = 1;

        loadingConditionsListView.Items
            .Cast<ListViewItem>()
            .ToList()
            .ForEach(i =>
        {
            i.SubItems[ordinalNumber_ch.Index].Text = firstOrdinalNumber.ToString();
            firstOrdinalNumber++;
        });
    }
}