namespace TreeMiner
{
    public static class GenericTreeMinerExtensions
    {
        /// <summary>
        /// Retrieves directory and file artifacts from the tree using the specified tree miner derived from GenericTreeMiner.
        /// </summary>
        /// <typeparam name="TTreeArtifact">The type of the tree artifacts.</typeparam>
        /// <typeparam name="TBaseArtifact">The base type of the artifacts.</typeparam>
        /// <typeparam name="TFileArtifact">The type of the file artifacts, derived from TBaseArtifact.</typeparam>
        /// <typeparam name="TDirArtifact">The type of the directory artifacts, derived from TBaseArtifact.</typeparam>
        /// <param name="miner">The tree miner instance.</param>
        /// <param name="dirArtifact">The root directory artifact.</param>
        /// <param name="getArtifacts">Function to retrieve artifacts for a given directory.</param>
        /// <param name="artifactType">The type of artifacts to include. Default is ArtifactType.All.</param>
        /// <param name="depthOption">The depth option for mining. Default is DepthOption.Deep.</param>
        /// <returns>An enumerable collection of directory and file artifacts.</returns>
        /// <exception cref="AggregateException">Thrown when one or more exceptions occur during artifact retrieval.</exception>
        public static IEnumerable<TTreeArtifact> GetArtifacts<TTreeArtifact, TBaseArtifact, TFileArtifact, TDirArtifact>(this GenericTreeMiner<TTreeArtifact, TBaseArtifact, TFileArtifact, TDirArtifact> miner, TTreeArtifact dirArtifact, Func<TDirArtifact, IEnumerable<TBaseArtifact>> getArtifacts, ArtifactType artifactType = ArtifactType.All, DepthOption depthOption = DepthOption.Deep) 
            where TTreeArtifact : ITreeArtifact, new() 
            where TFileArtifact : class, TBaseArtifact 
            where TDirArtifact : class, TBaseArtifact
        {
            var exceptionAggregate = new List<Exception>();
            
            foreach (var artifact in miner.GetArtifacts(dirArtifact, getArtifacts, exceptionAggregate, artifactType, depthOption))
                yield return artifact;

            if (exceptionAggregate.Any())
                throw new AggregateException(exceptionAggregate);
        }
    }
}
