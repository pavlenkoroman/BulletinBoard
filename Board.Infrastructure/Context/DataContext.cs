using System.Reflection;
using Board.Domain;
using Board.Infrastructure.Context.Converters;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Context;

public class DataContext : DbContext
{
    protected DataContext()
    {
    }

    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);

        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<DateTime>()
            .HaveConversion<DateTimeValueConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresEnum<RatingType>();
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
