using TreeMiner;

public class FileSystemArtifact : ITreeArtifact<FileSystemInfo>
{
    /// <summary>
    /// Gets or sets the unique identifier of the artifact.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the parent artifact.
    /// </summary>
    public Guid ParentId { get; set; }

    /// <summary>
    /// Gets or sets the level of the artifact in the directory tree.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Gets or sets additional information associated with the artifact.
    /// </summary>
    public FileSystemInfo Info { get; set; }
}