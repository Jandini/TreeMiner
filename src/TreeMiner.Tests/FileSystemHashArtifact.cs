namespace TreeMiner.Tests
{
    public class FileSystemHashArtifact : ITreeArtifact<FileSystemInfo>
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public int Level { get; set; }
        public FileSystemInfo? Info { get; set; }
        public string? Hash { get; set; }
    }
}
