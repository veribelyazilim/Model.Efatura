using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Model.Efatura.Data;

/* This is used if database provider does't define
 * IEfaturaDbSchemaMigrator implementation.
 */
public class NullEfaturaDbSchemaMigrator : IEfaturaDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
