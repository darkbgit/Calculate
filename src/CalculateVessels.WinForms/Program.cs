using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Cylindrical;
using CalculateVessels.Forms;
using CalculateVessels.Helpers;
using CalculateVessels.Output;
using CalculateVessels.Output.DI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CalculateVessels;

internal static class Program
{
    public static IServiceProvider ServiceProvider { get; private set; }

    [STAThread]
    static void Main()
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        ServiceProvider = CreateHostBuilder().Build().Services;

        Application.Run(ServiceProvider.GetService<MainForm>());
    }


    private static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IFormFactory, FormFactory>();

                services.AddScoped<ICalculateService<CylindricalShellInput>, CylindricalShellCalculateService>();

                var forms = typeof(Program).Assembly
                    .GetTypes()
                    .Where(t => t.BaseType == typeof(Form))
                    .ToList();

                forms.ForEach(form =>
                {
                    services.AddTransient(form);
                });

                var serviceCollectionForOutput = new ServiceCollectionForOutput();
                serviceCollectionForOutput.RegisterDependencies(services);

            });
    }
}