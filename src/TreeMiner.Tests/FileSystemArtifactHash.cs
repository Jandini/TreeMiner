namespace TreeMiner.Tests
{
    public class FileSystemArtifactHash : ITreeArtifact
    {
        public Guid Id { get; set; }
        public Guid Parent { get; set; }
        public int Level { get; set; }
        public object? Info { get; set; }
        public string? Hash { get; set; }
    }
}
