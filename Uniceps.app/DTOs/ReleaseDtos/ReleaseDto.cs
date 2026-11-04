using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.DTOs.ReleaseDtos
{
    public class ReleaseDto
    {
        public ReleaseDto(Release data)
        {
            ChangeLog = data.ChangeLog;
            DownloadSource = data.DownloadSource;
            DownloadUrl = data.DownloadUrl ?? "";
            ProductId = data.ProductId;
            TargetOS = data.TargetOS;
            Version = data.Version;
            ChangeLogAr = data.ChangeLogAr;
            CreatedAt = data.CreatedAt;
            TargetOSText = data.TargetOS.ToString();
            Id = data.Id;
            DownloadSourceText = data.DownloadSource.ToString();
            IsUrgent = data.IsUrgent;
        }
        public int Id { get; set; }
        public string Version { get; set; } = "";
        public TargetOS TargetOS { get; set; }
        public string TargetOSText { get; set; } = "";
        public DownloadSource DownloadSource { get; set; }
        public string DownloadSourceText { get; set; } = "";
        public string DownloadUrl { get; set; } = "";
        public string ChangeLog { get; set; } = "";
        public string ChangeLogAr { get; set; } = "";
        public int ProductId { get; set; }
        public bool IsUrgent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
