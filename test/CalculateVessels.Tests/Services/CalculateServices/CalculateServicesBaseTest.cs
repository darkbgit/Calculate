using System.Configuration;
using CalculateVessels.Data.Public.Interfaces;
using CalculateVessels.Data.Database;
using CalculateVessels.DataAccess.EF.PhysicalData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public abstract class CalculateServicesBaseTest
{
    protected readonly IPhysicalDataService PhysicalDataService;

    public CalculateServicesBaseTest()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<CalculateVesselsPhysicalDataContext>();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new CalculateVesselsPhysicalDataContext(optionsBuilder.Options);

        PhysicalDataService = new PhysicalDataService(context);
    }
}