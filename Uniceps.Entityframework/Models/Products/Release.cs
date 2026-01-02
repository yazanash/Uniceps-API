using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.Products
{
    public class Release
    {
        public int Id { get; set; }
        public string Version { get; set; } = "";
        public TargetOS TargetOS { get; set; }
        public DownloadSource DownloadSource { get; set; }
        public string DownloadUrl { get; set; } = "";
        public string ChangeLog { get; set; } = "";
        public string ChangeLogAr { get; set; } = "";
        public int ProductId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum TargetOS
    {
        Windows = 1,
        MacOS = 2,
        Linux = 3,
        IOS = 4,
        Android = 5,
    }
    public enum DownloadSource
    {
        GooglePlay=1,
        AppStore = 2,
        MicrosoftStore = 3,
        Website = 4
    }
}
