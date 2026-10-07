using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Model.Efatura
{
    [ConnectionStringName("ModelConnStr")]
    public class ModelDbContext : AbpDbContext<ModelDbContext>
    {
        public ModelDbContext(DbContextOptions<ModelDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
