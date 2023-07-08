namespace TreeMine
{
    public class ArtifactException : Exception
    {
        public ITreeArtifact Artifact { get; set; }

        public ArtifactException(Exception ex, ITreeArtifact artifact) 
            : base(ex.Message, ex)
        {            
            Artifact = artifact;
        }
    }
}
