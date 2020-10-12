using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Quota.Entities
{
    public class QuotaEntity
    {
        [Key]
        public int IdCuota { get; set; }
        public long IdCredito { get; set; }
        public decimal ValorCapital { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
