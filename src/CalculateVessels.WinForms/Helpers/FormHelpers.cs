using CalculateVessels.Controls;
using CalculateVessels.Core.Base;
using CalculateVessels.Data.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels.Helpers;

internal static class FormHelpers
{
    private const string AutoStrengthParameters = "auto";

    public static void EnabledIfCheck(Control control, bool value)
    {
        control.Enabled = value;
    }

    public static void EnabledIfCheck(IEnumerable<Control> controls, bool value)
    {
        controls.ToList()
            .ForEach(control => control.Enabled = value);
    }

    public static void AddLoadingCondition(ListView listView, string pressureType, string p, string t,
            string sigmaAllow, string EAllow, int maxLoadingConditions = 3)
    {
        if (listView.Items.Count >= maxLoadingConditions)
        {
            MessageBox.Show($@"Максимальное число условий нагружения - {maxLoadingConditions}.");
            return;
        }

        var loadingConditions = new string[]
        {
            pressureType, p, t,
            sigmaAllow == string.Empty ? AutoStrengthParameters : sigmaAllow,
            EAllow == string.Empty ? AutoStrengthParameters : EAllow
        };

        if (listView.Items.Cast<ListViewItem>()
            .Any(i => i.SubItems.Cast<ListViewItem.ListViewSubItem>()
                .Select(si => si.Text)
                .ToList()
                .SequenceEqual(loadingConditions)))
        {
            MessageBox.Show("Условие нагружение с данными параметрами уже существует.");
            return;
        }

        var newItem = new ListViewItem(loadingConditions.First());

        newItem.SubItems.AddRange(loadingConditions[1..]);

        listView.Items.Add(newItem);
    }

    public static void AddLoadingCondition(ListView listView, LoadingCondition loadingCondition, int maxLoadingConditions = 3)
    {
        if (listView.Items.Count >= maxLoadingConditions)
        {
            MessageBox.Show($@"Максимальное число условий нагружения - {maxLoadingConditions}.");
            return;
        }

        var loadingConditions = new string[]
        {
            loadingCondition.IsPressureIn ? Properties.Resources.InsidePressure : Properties.Resources.OutsidePressure,
            loadingCondition.p.ToString(),
            loadingCondition.t.ToString(),
            loadingCondition.SigmaAllow == 0 ? AutoStrengthParameters : loadingCondition.SigmaAllow.ToString(),
            loadingCondition.EAllow == 0 ? AutoStrengthParameters : loadingCondition.EAllow.ToString()
        };

        if (listView.Items.Cast<ListViewItem>()
            .Any(i => i.SubItems.Cast<ListViewItem.ListViewSubItem>()
                .Select(si => si.Text)
                .ToList()
                .SequenceEqual(loadingConditions)))
        {
            MessageBox.Show("Условие нагружение с данными параметрами уже существует.");
            return;
        }

        var newItem = new ListViewItem(loadingConditions.First());

        newItem.SubItems.AddRange(loadingConditions[1..]);

        listView.Items.Add(newItem);
    }

    public static void DeleteLoadingCondition(ListView listView)
    {
        if (listView.SelectedItems.Count == 0)
        {
            return;
        }

        var selectedItem = listView.SelectedItems.Cast<ListViewItem>().First();

        listView.Items.Remove(selectedItem);
    }

    public static IEnumerable<LoadingCondition> ParseLoadingConditionsFromListView(ListView listView, List<string> dataInErr)
    {
        const int pressureTypeIndex = 0,
            pressureIndex = 1,
            tIndex = 2,
            sigmaAllowIndex = 3,
            EAllowIndex = 4;

        //var dataInErr = new List<string>();

        var loadingConditions = listView.Items
            .Cast<ListViewItem>()
            .ToList()
            .Select((i, j) => new LoadingCondition
            {
                IsPressureIn = i.SubItems[pressureTypeIndex].Text == Properties.Resources.InsidePressure,
                p = Parameters.GetParam<double>(i.SubItems[pressureIndex].Text, "p", dataInErr),
                t = Parameters.GetParam<double>(i.SubItems[tIndex].Text, "t", dataInErr),
                OrdinalNumber = j + 1,
                SigmaAllow = i.SubItems[sigmaAllowIndex].Text == AutoStrengthParameters ? default :
                    Parameters.GetParam<double>(i.SubItems[sigmaAllowIndex].Text, "[σ]", dataInErr),
                EAllow = i.SubItems[EAllowIndex].Text == AutoStrengthParameters ? default :
                    Parameters.GetParam<double>(i.SubItems[EAllowIndex].Text, "E", dataInErr),
            })
            .ToList();

        return loadingConditions;
    }

    public static LoadingCondition ParseLoadingConditionFromForm(CheckBox isPressureOutside, TextBox t, TextBox p,
        CheckBox sigmaHandle, TextBox sigmaAllow, CheckBox EHandle, TextBox EAllow, List<string> dataInErr)
    {
        var loadingCondition = new LoadingCondition
        {
            IsPressureIn = !isPressureOutside.Checked,
            t = Parameters.GetParam<double>(t.Text, "t", dataInErr, NumberStyles.Integer),
            p = Parameters.GetParam<double>(p.Text, "p", dataInErr),
            OrdinalNumber = 1,
            SigmaAllow = sigmaHandle.Checked
                ? Parameters.GetParam<double>(sigmaAllow.Text, "[σ]", dataInErr)
                : default
        };

        if (loadingCondition.IsPressureIn)
            return loadingCondition;

        if (EHandle.Checked)
        {
            loadingCondition.EAllow = Parameters.GetParam<double>(EAllow.Text, "E", dataInErr);
        }

        return loadingCondition;
    }

    public static IEnumerable<LoadingCondition> CollectLoadingConditions(ListView loadingConditionsListView, CheckBox isPressureOutsideCheckBox,
        TextBox temperatureTextBox, TextBox pressureTextBox, CheckBox sigmaHandleCheckBox, TextBox sigmaAllowTextBox, CheckBox EHandleCheckBox, TextBox EAllowTextBox,
        List<string> dataInErr)
    {
        if (loadingConditionsListView.Items.Count == 0)
        {

            var loadingCondition = ParseLoadingConditionFromForm(isPressureOutsideCheckBox, temperatureTextBox, pressureTextBox,
                sigmaHandleCheckBox, sigmaAllowTextBox,
                EHandleCheckBox, EAllowTextBox, dataInErr);

            return new List<LoadingCondition> { loadingCondition };
        }

        var loadingConditions = ParseLoadingConditionsFromListView(loadingConditionsListView, dataInErr);
        return loadingConditions;
    }

    /// <summary>
    /// Parse <see cref="IEnumerable{LoadingCondition}"/> from form with <seealso cref="LoadingConditionsControl"/> and <see cref="LoadingConditionControl"/>.
    /// </summary>
    /// <returns><see cref="IEnumerable{LoadingCondition}"/> if data parsed correctly. Else returns <see cref="Enumerable.Empty{LoadingCondition}"/> and shows MessageBox with errors.</returns>
    public static IEnumerable<LoadingCondition> ParseLoadingConditions(LoadingConditionsControl loadingConditionsControl, LoadingConditionControl loadingConditionGroupBox)
    {
        if (!loadingConditionsControl.Any())
        {
            var loadingCondition = loadingConditionGroupBox.GetLoadingCondition();

            return loadingCondition == null ? Enumerable.Empty<LoadingCondition>() : new List<LoadingCondition> { loadingCondition };
        }

        var loadingConditions = loadingConditionsControl
            .GetLoadingConditions()
            .ToList();

        return loadingConditions;
    }

    public static Bitmap GetBitmapFromResource(string resourceName)
    {
        return (Bitmap)(new ImageConverter().ConvertFrom(Resources.ResourceManager.GetObject(resourceName)
        ?? throw new InvalidOperationException())
            ?? throw new InvalidOperationException());
    }

    public static Bitmap GetBitmapFromBytes(byte[] bytes)
    {
        return (Bitmap)(new ImageConverter().ConvertFrom(bytes)
                        ?? throw new InvalidOperationException());
    }
}