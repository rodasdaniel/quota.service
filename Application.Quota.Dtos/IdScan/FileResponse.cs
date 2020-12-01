using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Quota.Dtos
{
    public class FileResponse
    {
        public double Prom { get; set; }
        public List<FileResponseItem> ResponseItems { get; set; }
    }
}
