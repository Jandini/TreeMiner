namespace TreeMiner
{
    /// <summary>
    /// Represents a tree artifact in the tree mining process.
    /// </summary>
    public interface ITreeArtifact
    {
        /// <summary>
        /// Gets or sets the unique identifier of the artifact.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the parent artifact.
        /// </summary>
        Guid ParentId { get; set; }

        /// <summary>
        /// Gets or sets the level of the artifact in the tree.
        /// </summary>
        int Level { get; set; }

        /// <summary>
        /// Gets or sets additional information associated with the artifact.
        /// </summary>
        object? Info { get; set; }
    }
}