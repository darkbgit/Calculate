using System;
using System.Windows.Forms;
using CalculateVessels.Core.Elements.Shells.Cylindrical;
using CalculateVessels.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Helpers;

public class FormFactory : IFormFactory
{
    private readonly IServiceScope _scope;

    public FormFactory(IServiceScopeFactory scopeFactory)
    {
        _scope = scopeFactory.CreateScope();
    }

    public T? Create<T>() where T : Form
    {
        return _scope.ServiceProvider.GetService<T>();
    }

    public Form? Create(string calculatedElementType)
    {
        switch (calculatedElementType)
        {
            case nameof(CylindricalShellCalculated):
                return _scope.ServiceProvider.GetService<CylindricalShellForm>();
            default:
                throw new Exception($"{calculatedElementType} type didn't registered.");
        }
    }
}