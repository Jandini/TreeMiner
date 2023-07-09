namespace TreeMiner
{
    public interface ITreeArtifact
    {
        Guid Id { get; set; }
        Guid ParentId { get; set; }
        int Level { get; set; }
        object? Info { get; set; }
    }
}
