using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Quota.Entities
{
    public class CreditEntity
    {
        [Key]
        public long IdCredito { get; set; }
        public long IdCliente { get; set; }
    }
}
