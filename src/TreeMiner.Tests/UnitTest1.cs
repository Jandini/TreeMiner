namespace TreeMiner.Tests
{
    public class UnitTest1
    {

        public static IEnumerable<T> Dig<T>(string root) where T : ITreeArtifact, new()
        {
            var exceptionAggregate = new List<Exception>();

            var rootArtifact = new T()
            {
                Info = new DirectoryInfo(root),
                Id = Guid.Empty,
                Parent = Guid.Empty
            };

            foreach (var artifact in TreeMiner<T>.Dig(dirArtifact: rootArtifact, onDirArtifact: null, onFileArtifact: null, onException: null, exceptionAggregate: exceptionAggregate))
                yield return artifact;

            if (exceptionAggregate.Any())
                throw new AggregateException(exceptionAggregate);
        }



        [Fact]
        public void Test1()
        {
            var artifacts = Dig<TreeArtifact>(Environment.SystemDirectory);

            foreach (var artifact in artifacts)
            {
                Assert.NotNull(artifact);
            }
        }
    }
}