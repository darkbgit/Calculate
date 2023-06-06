using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace CalculateVessels.Elements;

[Designer(typeof(ControlDesigner))]
public sealed partial class LoadingConditionGroupBox : GroupBox
{
    public LoadingConditionGroupBox()
    {
        InitializeComponent();
    }

    public LoadingCondition? GetLoadingCondition()
    {
        var dataInErr = new List<string>();

        var loadingCondition = new LoadingCondition
        {
            OrdinalNumber = 1,
            IsPressureIn = !isPressureOutsideCheckBox.Checked,
            t = Parameters.GetParam<double>(temperatureTextBox.Text, "t", ref dataInErr, NumberStyles.Integer),
            p = Parameters.GetParam<double>(pressureTextBox.Text, "p", ref dataInErr),
            SigmaAllow = isSigmaAllowHandleCheckBox.Checked
                ? Parameters.GetParam<double>(sigmaAllowTextBox.Text, "[σ]", ref dataInErr)
                : default,
            EAllow = isEAllowHandleCheckBox.Checked
                ? Parameters.GetParam<double>(EAllowTextBox.Text, "E", ref dataInErr)
                : default
        };

        if (!dataInErr.Any())
        {
            return loadingCondition;
        }

        MessageBox.Show(string.Join(Environment.NewLine, dataInErr));
        return null;
    }

    private void SigmaAllowHandleCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        if (sender is CheckBox cb)
        {
            FormHelpers.EnabledIfCheck(sigmaAllowTextBox, cb.Checked);
        }
    }

    private void EHandleCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        if (sender is CheckBox cb)
        {
            FormHelpers.EnabledIfCheck(EAllowTextBox, cb.Checked);
        }
    }
}