namespace Application.Quota.Dtos
{
    public class InfoClientDto
    {
        public long IdClient { get; set; }
        public decimal TotalSpace { get; set; }
        public decimal AvailableSpace { get; set; }
    }
}
