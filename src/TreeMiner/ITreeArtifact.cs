namespace TreeMiner
{
    public interface ITreeArtifact
    {
        Guid Id { get; set; }
        Guid Parent { get; set; }
        int Level { get; set; }
        object? Info { get; set; }
    }
}
