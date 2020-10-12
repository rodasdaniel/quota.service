using Domain.Quota.Entities;
using Infrastructure.Quota.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Quota.Data.Repository
{
    public class QuotaRepository : IQuotaRepository
    {
        #region Constructor
        private readonly ApplicationDBContext _context;
        public QuotaRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        #endregion 
        public async Task<bool> Create(List<QuotaEntity> quotas)
        {
            var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            try
            {
                foreach (QuotaEntity quota in quotas)
                {
                    _context.Cuota.Add(quota);
                    _context.SaveChanges();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public async Task<bool> CreditExist(long idClient, long idCredit)
        {
            CreditEntity creditEntity = _context.Credito.Where(c => c.IdCliente == idClient
                 && c.IdCredito == idCredit).FirstOrDefault();
            return (creditEntity != null);
        }

        public async Task<List<QuotaEntity>> GetByCredit(long idCredit)
        {
            return _context.Cuota.Where(c => c.IdCredito == idCredit).ToList();
        }
    }
}
