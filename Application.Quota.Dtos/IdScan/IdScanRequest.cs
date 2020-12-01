using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Quota.Dtos
{
    public class IdScanRequest
    {
        public string id { get; set; }
        public SideDto side1 { get; set; }
        public SideDto side2 { get; set; }
    }
}
