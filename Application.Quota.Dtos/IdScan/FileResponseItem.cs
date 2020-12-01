using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Quota.Dtos
{
    public class FileResponseItem
    {

        public string TransactionID { get; set; }
        public int Time { get; set; }
        public IdScanResponse Response { get; set; }
        public string ResponseCode { get; set; }
    }
}
