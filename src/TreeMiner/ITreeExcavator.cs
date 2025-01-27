namespace TreeMiner
{
    public interface ITreeExcavator<TTreeArtifact, TBaseArtifact, TFileArtifact, TDirArtifact>
        where TTreeArtifact : ITreeArtifact<TBaseArtifact>, new()
        where TFileArtifact : class, TBaseArtifact
        where TDirArtifact : class, TBaseArtifact
    {
        /// <summary>
        /// Gets the artifacts from the directory artifact.
        /// </summary>
        /// <param name="dirArtifact">Directory artifact</param>
        /// <returns>An enumerable collection of base artifacts</returns>
        public IEnumerable<TBaseArtifact> GetArtifacts(TDirArtifact dirArtifact);

        /// <summary>
        /// This method is called for every directory artifact found in the tree.
        /// </summary>
        /// <param name="dirArtifact">Directory artifact</param>
        /// <param name="dirContent">Enumerable directory content. Provides files and directories.</param>
        public void OnDirArtifact(TTreeArtifact dirArtifact, IEnumerable<TBaseArtifact> dirContent);

        /// <summary>
        /// This method is called for every file artifact found in the tree.
        /// </summary>
        /// <param name="fileArtifact">File artifact</param>
        public void OnFileArtifact(TTreeArtifact fileArtifact);

        /// <summary>
        /// This method is called when an exception occurs during tree mining.
        /// </summary>
        /// <param name="exception">Exception that occurred</param>
        public void OnException(ArtifactException<TBaseArtifact> exception);
    }
}
