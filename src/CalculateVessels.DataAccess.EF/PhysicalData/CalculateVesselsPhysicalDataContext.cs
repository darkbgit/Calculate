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
    public DbSet<E34233D1> E34233D1 { get; set; }
    public DbSet<MinMaxThickness> SteelsMinMaxThickness { get; set; }
    public DbSet<EllipticalBottom6533Type> EllipticalBottom6533Types { get; set; }
    public DbSet<EllipticalBottom6533> EllipticalBottom6533 { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Sigma34233D1>()
        //    .HasNoKey();

        modelBuilder.Entity<Sigma34233D1>()
            .HasKey(sigma => new { sigma.SteelId, sigma.T, sigma.MinMaxThicknessId, sigma.DesignResourceId });

        modelBuilder.Entity<E34233D1>()
            .HasKey(e => new { e.SteelTypeId, e.T });

        modelBuilder.Entity<EllipticalBottom6533>()
            .HasKey(b => new { b.EllipticalBottomTypeId, b.D, b.s });
    }
}