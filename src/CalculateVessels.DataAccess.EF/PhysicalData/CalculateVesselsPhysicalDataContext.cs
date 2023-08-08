using CalculateVessels.DataAccess.EF.PhysicalData.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalculateVessels.DataAccess.EF.PhysicalData;

public class CalculateVesselsPhysicalDataContext : DbContext
{
    public CalculateVesselsPhysicalDataContext(DbContextOptions<CalculateVesselsPhysicalDataContext> options)
        : base(options)
    {

    }

    public DbSet<Steel> Steels { get; set; }
    public DbSet<DesignResource> DesignResources { get; set; }
    public DbSet<Sigma34233D1> Sigmas34233D1 { get; set; }
    public DbSet<MinMaxThickness> SteelsMinMaxThickness { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Sigma34233D1>()
        //    .HasNoKey();

        modelBuilder.Entity<Sigma34233D1>()
            .HasKey(sigma => new { sigma.SteelId, sigma.T, sigma.MinMaxThicknessId, sigma.DesignResourceId });
    }
}