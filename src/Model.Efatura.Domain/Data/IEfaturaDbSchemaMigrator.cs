using System.Threading.Tasks;

namespace Model.Efatura.Data;

public interface IEfaturaDbSchemaMigrator
{
    Task MigrateAsync();
}
