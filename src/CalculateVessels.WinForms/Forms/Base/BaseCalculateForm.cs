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
//[TypeDescriptionProvider(typeof(AbstractGenericFormDescriptionProvider))]
public abstract class BaseCalculateForm<T> : Form
    where T : class, IInputData
{
    private const int newCalculatedElementIndex = -1;
    private int _calculatedElementIndex = newCalculatedElementIndex;

    protected BaseCalculateForm(IEnumerable<ICalculateService<T>> calculateServices,
        IPhysicalDataService physicalDataService)
    {
        CalculateServices = calculateServices;
        PhysicalDataService = physicalDataService;
    }

    protected T? InputData { get; set; }

    protected IEnumerable<ICalculateService<T>> CalculateServices { get; }

    protected IPhysicalDataService PhysicalDataService { get; }

    public void Show(T inputData, int calculatedElementIndex)
    {
        if (!inputData.IsDataGood)
        {
            MessageBox.Show("Couldn't load initialize data.");
            return;
        }

        InputData = inputData;
        _calculatedElementIndex = calculatedElementIndex;
        Show();
        LoadInputData();
    }

    protected abstract bool CollectDataForPreliminarilyCalculation();

    protected abstract bool CollectDataForFinishCalculation();

    protected abstract string GetServiceName();

    protected abstract void LoadInputData();

    private ICalculateService<T> GetCalculateService(string serviceName)
    {
        return CalculateServices
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
            if (_calculatedElementIndex == newCalculatedElementIndex)
            {
                main.calculatedElementsControl.AddElement(element);
            }
            else
            {
                main.calculatedElementsControl.UpdateElement(element, _calculatedElementIndex);
            }
        }
        else
        {
            MessageBox.Show($"{nameof(MainForm)} error.");
        }
    }

    protected void LoadSteelsToComboBox(ComboBox comboBox, SteelSource source)
    {
        string[] preferredSteels = { "Ст3", "20", "12Х18Н10Т", "09Г2С" };

        comboBox.Items.Clear();

        var steels = PhysicalDataService
            .GetSteels(source)
            .ToList();

        var preferredSteelList = preferredSteels
            .Intersect(steels)
            .ToList();

        var otherSteels = steels
            .Except(preferredSteelList)
            .ToList();

        otherSteels.Sort();

        var result = preferredSteelList
            .Union(otherSteels)
            .Select(s => s as object)
            .ToArray();

        comboBox.Items.AddRange(result);
        comboBox.SelectedIndex = 0;
    }

    protected void LoadCalculateServicesNamesToComboBox(ComboBox comboBox)
    {
        comboBox.Items.Clear();
        var serviceNames = CalculateServices
                    .Select(s => s.Name as object)
                    .ToArray();
        comboBox.Items.AddRange(serviceNames);
        comboBox.SelectedIndex = 0;
    }
}