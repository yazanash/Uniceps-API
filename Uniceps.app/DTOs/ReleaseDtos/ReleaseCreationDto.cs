using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.DTOs.ReleaseDtos
{
    public class ReleaseCreationDto
    {
        public string Version { get; set; } = "";
        public TargetOS TargetOS { get; set; }
        public DownloadSource DownloadSource { get; set; }
        public string ChangeLog { get; set; } = "";
        public int ProductId { get; set; }
        public string? DownloadUrl { get; set; }
        public string ChangeLogAr { get; set; } = "";
    }
}
