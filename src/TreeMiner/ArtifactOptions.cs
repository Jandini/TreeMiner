using TreeMiner;

/// <summary>
/// Represents options for retrieving artifacts.
/// </summary>
public class ArtifactOptions
{

    /// <summary>
    /// Gets the default artifact options.
    /// </summary>
    public static ArtifactOptions Default { get; } = new ArtifactOptions();

    /// <summary>
    /// Gets or sets the depth option for artifact retrieval.
    /// </summary>
    public DepthOption ArtifactDepth { get; set; } = DepthOption.Deep;

    /// <summary>
    /// Gets or sets the artifact type for retrieval.
    /// </summary>
    public ArtifactType ArtifactType { get; set; } = ArtifactType.All;
}
