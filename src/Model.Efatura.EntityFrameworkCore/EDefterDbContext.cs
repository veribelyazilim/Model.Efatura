using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Model.Efatura
{
    /// <summary>
    /// E-defter prosedürünün (ZRHGROUPAS202601.dbo.EDEFTER) çalıştığı veritabanı bağlantısı.
    /// Entity içermez, sadece bağlantıyı taşır (ModelDbContext ile aynı yaklaşım).
    /// </summary>
    [ConnectionStringName("EDefterConnStr")]
    public class EDefterDbContext : AbpDbContext<EDefterDbContext>
    {
        public EDefterDbContext(DbContextOptions<EDefterDbContext> options)
            : base(options)
        {

        }
    }
}
