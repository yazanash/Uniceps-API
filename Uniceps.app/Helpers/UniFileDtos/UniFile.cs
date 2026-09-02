namespace Uniceps.app.Helpers.UniFileDtos
{
    public class UniFile<T>
    {
        public MetaData? Meta { get; set; }
        public T? Data { get; set; }
    }
    public class MetaData
    {
        public string? Source { get; set; }
        public int SchemaVersion { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? FileType { get; set; }
    }
}
