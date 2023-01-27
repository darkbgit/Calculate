using System.Windows.Forms;
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
}