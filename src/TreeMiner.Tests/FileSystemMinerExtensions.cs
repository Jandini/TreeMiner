namespace TreeMiner.tests
{
    public static class FileSystemTreeMinerExtensions
    {

        public static IEnumerable<TTreeArtifact> GetArtifacts<TTreeArtifact, TBaseArtifact, TFileArtifact, TDirArtifact>(this GenericTreeMiner<TTreeArtifact, TBaseArtifact, TFileArtifact, TDirArtifact> miner, TTreeArtifact dirArtifact,
            Func<TDirArtifact, IEnumerable<TBaseArtifact>> getArtifacts,
            ArtifactType artifactType = ArtifactType.All,
            DepthOption depthOption = DepthOption.Deep) where TTreeArtifact : ITreeArtifact, new() where TFileArtifact : class, TBaseArtifact where TDirArtifact : class, TBaseArtifact
        {
            var exceptionAggregate = new List<Exception>();
            foreach (var artifact in miner.GetArtifacts(dirArtifact, getArtifacts, exceptionAggregate, artifactType, depthOption))
                yield return artifact;

            if (exceptionAggregate.Any())
                throw new AggregateException(exceptionAggregate);
        }
    }
}
