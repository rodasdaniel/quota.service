namespace Application.Quota.Dtos.Common
{
    public class TermDto
    {
        private decimal to;
        public int Months { get; set; }
        public decimal From { get; set; }
        public decimal To
        {
            get { return to; }

            set
            {
                if (value == 0) to = decimal.MaxValue;
                else to = value;
            }
        }
    }
}
