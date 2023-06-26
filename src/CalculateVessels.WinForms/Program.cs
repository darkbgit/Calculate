using CalculateVessels.Core.DI;
using CalculateVessels.Data.DI;
using CalculateVessels.Forms;
using CalculateVessels.Helpers;
using CalculateVessels.Output.DI;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CalculateVessels;

internal static class Program
{
    public static IServiceProvider ServiceProvider { get; private set; }

    [STAThread]
    static void Main()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
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

                var forms = typeof(Program).Assembly
                    .GetTypes()
                    .Where(t => (t.BaseType == typeof(Form) && !t.IsAbstract)
                                || (t.BaseType?.BaseType == typeof(Form) && !t.IsAbstract && t.Name[^6..] != "Middle")
                                || t.BaseType?.BaseType?.BaseType == typeof(Form)
                        || t.BaseType?.BaseType?.BaseType?.BaseType == typeof(Form))
                    .ToList();

                forms.ForEach(form =>
                {
                    services.AddTransient(form);
                });

                var serviceCollectionForData = new ServiceCollectionForData();
                serviceCollectionForData.RegisterDependencies(services);

                var serviceCollectionForCore = new ServiceCollectionForCore();
                serviceCollectionForCore.RegisterDependencies(services);

                var serviceCollectionForOutput = new ServiceCollectionForOutput();
                serviceCollectionForOutput.RegisterDependencies(services);
            });
    }
}