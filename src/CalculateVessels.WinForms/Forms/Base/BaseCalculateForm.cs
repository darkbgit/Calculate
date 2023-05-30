using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels.Forms.Base;

//[TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<BaseCalculateForm<T>, Form>))]
public abstract class BaseCalculateForm<T> : Form
    where T : class, IInputData
{
    private readonly IEnumerable<ICalculateService<T>> _calculateServices;
    private readonly IPhysicalDataService _physicalDataService;

    protected T? InputData { get; set; }

    protected BaseCalculateForm(IEnumerable<ICalculateService<T>> calculateServices,
        IPhysicalDataService physicalDataService)
    {
        _calculateServices = calculateServices;
        _physicalDataService = physicalDataService;
    }

    protected abstract bool CollectDataForPreliminarilyCalculation();

    protected abstract bool CollectDataForFinishCalculation();

    protected abstract string GetServiceName();

    private ICalculateService<T> GetCalculateService(string serviceName)
    {
        return _calculateServices
                       .FirstOrDefault(s => s.Name == serviceName)
                            ?? throw new InvalidOperationException("Service wasn't found.");
    }

    protected ICalculatedElement? Calculate()
    {
        ICalculatedElement element;

        try
        {
            element = GetCalculateService(GetServiceName()).Calculate(InputData
                ?? throw new InvalidOperationException());
        }
        catch (CalculateException ex)
        {
            MessageBox.Show(ex.Message);
            return null;
        }

        if (element.ErrorList.Any())
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, element.ErrorList));
        }

        return element;
    }

    protected void SetCalculatedElementToStorage(Form? form, ICalculatedElement element)
    {
        if (form is MainForm main)
        {
            main.Word_lv.Items.Add(element.ToString());
            main.ElementsCollection.Add(element);
        }
        else
        {
            MessageBox.Show($"{nameof(MainForm)} error");
        }
    }

    protected void LoadSteelsToComboBox(ComboBox comboBox, SteelSource source)
    {
        comboBox.Items.Clear();

        var steels = _physicalDataService.GetSteels(source)
            .Select(s => s as object)
            .ToArray();

        comboBox.Items.AddRange(steels);
        comboBox.SelectedIndex = 0;
    }

    protected void LoadCalculateServicesNamesToComboBox(ComboBox comboBox)
    {
        comboBox.Items.Clear();
        var serviceNames = _calculateServices
                    .Select(s => s.Name as object)
                    .ToArray();
        comboBox.Items.AddRange(serviceNames);
        comboBox.SelectedIndex = 0;
    }
}