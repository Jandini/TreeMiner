namespace TreeMine
{
    public class TreeArtifact : ITreeArtifact
    {
        public Guid Id { get; set; }
        public Guid Parent { get; set; }
        public int Level { get; set; }
        public object? Info { get; set; }
    }
}
