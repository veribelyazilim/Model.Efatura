using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Model.Efatura.Data;
using Volo.Abp.DependencyInjection;

namespace Model.Efatura.EntityFrameworkCore;

public class EntityFrameworkCoreEfaturaDbSchemaMigrator
    : IEfaturaDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreEfaturaDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the EfaturaDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<EfaturaDbContext>()
            .Database
            .MigrateAsync();
    }
}
