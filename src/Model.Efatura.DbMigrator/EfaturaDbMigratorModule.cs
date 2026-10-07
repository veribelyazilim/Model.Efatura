using Model.Efatura.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Model.Efatura.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EfaturaEntityFrameworkCoreModule),
    typeof(EfaturaApplicationContractsModule)
)]
public class EfaturaDbMigratorModule : AbpModule
{
}
