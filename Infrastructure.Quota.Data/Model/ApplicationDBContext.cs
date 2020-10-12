using Domain.Quota.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Quota.Data.Model
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<QuotaEntity> Cuota { get; set; }
        public DbSet<CreditEntity> Credito { get; set; }
    }
}
