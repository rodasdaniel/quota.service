using Domain.Quota.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Quota.Data.Repository
{
    public interface IQuotaRepository
    {
        Task<bool> Create(List<QuotaEntity> quotas);
        Task<List<QuotaEntity>> GetByCredit(long idCredit);
        Task<bool> CreditExist(long idClient, long idCredit);
    }
}
