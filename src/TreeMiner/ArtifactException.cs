namespace TreeMiner
{
    /// <summary>
    /// Exception class for artifacts in the tree mining process.
    /// </summary>
    public class ArtifactException : Exception
    {
        /// <summary>
        /// Gets or sets the artifact associated with the exception.
        /// </summary>
        public ITreeArtifact Artifact { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtifactException"/> class with the specified exception and artifact.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="artifact">The artifact associated with the exception.</param>
        public ArtifactException(Exception ex, ITreeArtifact artifact)
            : base(ex.Message, ex)
        {
            Artifact = artifact;
        }
    }
}
