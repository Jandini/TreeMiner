using System.Security.Cryptography;
using System.Text;

namespace TreeMiner.Tests
{
    public class TreeMiner_Tests
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

            var fileSystemMiner = new FileSystemMiner<T>();

            foreach (var artifact in fileSystemMiner.DigFileSystem(dirArtifact: rootArtifact, onFileArtifact: null, onException: null, exceptionAggregate: exceptionAggregate))
                yield return artifact;

            if (exceptionAggregate.Any())
                throw new AggregateException(exceptionAggregate);
        }



        [Fact]
        public void DigFileSystemTest()
        {
            var artifacts = Dig<TreeArtifact>(Environment.SystemDirectory);

            foreach (var artifact in artifacts)
            {
                Assert.NotNull(artifact);
            }
        }



        public static IEnumerable<TreeArtifactHash> Hash(string root) 
        {
            var exceptionAggregate = new List<Exception>();

            var rootArtifact = new TreeArtifactHash
            {
                Info = new DirectoryInfo(root),
                Id = Guid.Empty,
                Parent = Guid.Empty,
                Hash = Convert.ToHexString(MD5.HashData(Array.Empty<byte>()))
            };

            var fileSystemMiner = new FileSystemMiner<TreeArtifactHash>();

            foreach (var artifact in fileSystemMiner.DigFileSystem(
                dirArtifact: rootArtifact,                 
                onDirArtifact: (dir, content) =>
                {
                    var list = string.Join(';', content.OrderBy(a => a.Name).Select(s => string.Join(',', s.Name, (s as FileInfo)?.Length ?? 0)));
                    dir.Hash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(list)));

                    return true;
                },
                artifactType: ArtifactType.Directories,
                onFileArtifact: null, 

                onException: null, 
                exceptionAggregate: exceptionAggregate))


                yield return artifact;

            if (exceptionAggregate.Any())
                throw new AggregateException(exceptionAggregate);
        }


        [Fact]
        public void HashDirsTest()
        {

            var artifacts = Hash(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));

            foreach (var artifact in artifacts)
            {
                Assert.NotNull(artifact);
            }
        }
    }
}