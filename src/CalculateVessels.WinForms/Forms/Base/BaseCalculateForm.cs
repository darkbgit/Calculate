using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using FluentValidation;

namespace CalculateVessels.Forms.Base;

public abstract class BaseCalculateForm<T> : Form
    where T : class, IInputData
{
    private const int NewCalculatedElementIndex = -1;
    private int _calculatedElementIndex = NewCalculatedElementIndex;

    private readonly IEnumerable<ICalculateService<T>> _calculateServices;
    private readonly IPhysicalDataService _physicalDataService;
    private readonly IValidator<T> _validator;


    protected BaseCalculateForm(IEnumerable<ICalculateService<T>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<T> validator)
    {
        _calculateServices = calculateServices;
        _physicalDataService = physicalDataService;
        _validator = validator;
    }

    //protected T? InputData { get; set; }

    //protected IEnumerable<ICalculateService<T>> CalculateServices { get; }

    protected IPhysicalDataService PhysicalDataService => _physicalDataService;



    public void Show(T inputData, int calculatedElementIndex)
    {
        if (!IsValidInputData(inputData))
        {
            MessageBox.Show("Couldn't load initialize data.");
            return;
        }

        _calculatedElementIndex = calculatedElementIndex;
        Show();
        LoadInputData(inputData);
    }

    protected abstract string GetServiceName();

    protected abstract void LoadInputData(T inputData);

    protected abstract bool TryCollectInputData(out T inputData);

    protected bool TryCalculate(out ICalculatedElement? element)
    {
        element = null;

        if (!TryCollectInputData(out var inputData))
        {
            return false;
        }

        if (!IsValidInputDataWithErrorMessage(inputData)) return false;

        try
        {
            var service = GetCalculateService(GetServiceName());
            element = service.Calculate(inputData);
        }
        catch (CalculateException ex)
        {
            MessageBox.Show(ex.Message);
            return false;
        }

        if (element.ErrorList.Any())
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, element.ErrorList));
        }

        return true;
    }

    //protected ICalculatedElement? Calculate()
    //{
    //    ICalculatedElement element;

    //    try
    //    {
    //        element = GetCalculateService(GetServiceName()).Calculate(InputData
    //            ?? throw new InvalidOperationException());
    //    }
    //    catch (CalculateException ex)
    //    {
    //        MessageBox.Show(ex.Message);
    //        return null;
    //    }

    //    if (element.ErrorList.Any())
    //    {
    //        MessageBox.Show(string.Join<string>(Environment.NewLine, element.ErrorList));
    //    }

    //    return element;
    //}

    protected void SetCalculatedElementToStorage(Form? form, ICalculatedElement element)
    {
        if (form is MainForm main)
        {
            if (_calculatedElementIndex == NewCalculatedElementIndex)
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

        var steels = _physicalDataService
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
        var serviceNames = _calculateServices
                    .Select(s => s.Name as object)
                    .ToArray();
        comboBox.Items.AddRange(serviceNames);
        comboBox.SelectedIndex = 0;
    }

    private ICalculateService<T> GetCalculateService(string serviceName)
    {
        return _calculateServices
                       .FirstOrDefault(s => s.Name == serviceName)
                            ?? throw new InvalidOperationException("Service wasn't found.");
    }

    private bool IsValidInputDataWithErrorMessage(T inputData)
    {
        var validateResult = _validator.Validate(inputData);

        if (validateResult.IsValid) return true;

        MessageBox.Show(string.Join(Environment.NewLine, validateResult.Errors));
        return false;
    }

    private bool IsValidInputData(T inputData)
    {
        var validateResult = _validator.Validate(inputData);

        return validateResult.IsValid;
    }
}