using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Model.Efatura.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class EfaturaDbContextFactory : IDesignTimeDbContextFactory<EfaturaDbContext>
{
    public EfaturaDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        EfaturaEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<EfaturaDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new EfaturaDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Model.Efatura.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
