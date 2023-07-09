namespace TreeMiner
{
    public class FileSystemArtifact : ITreeArtifact
    {
        public Guid Id { get; set; }
        public Guid Parent { get; set; }
        public int Level { get; set; }
        public object? Info { get; set; }
    }
}
