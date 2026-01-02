using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.Products
{
    public class DownloadLog
    {
        public int Id { get; set; }
        public int ReleaseId { get;set; }
        public string IPAddress { get; set; } = "";
        public string UserAgent { get; set; } = "";
        public DateTime DownloadedAt {  get; set; }=DateTime.UtcNow;
    }
}
