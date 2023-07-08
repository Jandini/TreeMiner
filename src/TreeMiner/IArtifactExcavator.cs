namespace TreeMiner
{
    public interface IArtifactExcavator<TTreeArtifact, TBaseArtifact, TFileArtifact, TDirArtifact> where TTreeArtifact : ITreeArtifact, new() where TFileArtifact : class, TBaseArtifact where TDirArtifact : class, TBaseArtifact
    {
        public bool OnDirArtifact(TTreeArtifact dirArtifact, IEnumerable<TBaseArtifact> dirContent);
        public bool OnFileArtifact(TTreeArtifact fileArtifact);
        public bool OnException(Exception exception);
    }
}
