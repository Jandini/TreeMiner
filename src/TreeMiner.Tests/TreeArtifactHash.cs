namespace TreeMine.Tests
{
    public class TreeArtifactHash : ITreeArtifact
    {
        public Guid Id { get; set; }
        public Guid Parent { get; set; }
        public int Level { get; set; }
        public object? Info { get; set; }
        public string? Hash { get; set; }
    }
}
