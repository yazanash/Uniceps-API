namespace Uniceps.app.DTOs.ReleaseDtos
{
    public class ChunkUploadDto
    {
        public IFormFile Chunk { get; set; } = null!;
        public int ChunkIndex { get; set; }
        public int TotalChunks { get; set; }
        public string UploadId { get; set; } = "";
    }
}
