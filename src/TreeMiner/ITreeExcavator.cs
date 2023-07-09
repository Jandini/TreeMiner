namespace TreeMiner
{
    public interface ITreeExcavator<TTreeArtifact, TBaseArtifact, TFileArtifact, TDirArtifact> where TTreeArtifact : ITreeArtifact, new() where TFileArtifact : class, TBaseArtifact where TDirArtifact : class, TBaseArtifact
    {
        public IEnumerable<TBaseArtifact> GetArtifacts(TDirArtifact dirArtifact);
        public bool OnDirArtifact(TTreeArtifact dirArtifact, IEnumerable<TBaseArtifact> dirContent);
        public bool OnFileArtifact(TTreeArtifact fileArtifact);
        public bool OnException(Exception exception);
    }
}
