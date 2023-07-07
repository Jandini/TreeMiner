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

            foreach (var artifact in TreeMiner<T>.Dig(dirArtifact: rootArtifact, onDirArtifact: null, onFileArtifact: null, onException: null, exceptionAggregate: exceptionAggregate))
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



        public static IEnumerable<TreeHashArtifact> Hash(string root) 
        {
            var exceptionAggregate = new List<Exception>();

            var rootArtifact = new TreeHashArtifact
            {
                Info = new DirectoryInfo(root),
                Id = Guid.Empty,
                Parent = Guid.Empty,
                Hash = Convert.ToHexString(MD5.HashData(Array.Empty<byte>()))
            };


            foreach (var artifact in TreeMiner<TreeHashArtifact>.Dig(dirArtifact: rootArtifact, 
                onDirArtifact: (dir, content) =>
                {
                    var list = string.Join(';', content.OrderBy(a => a.Name).Select(s => string.Join(',', s.Name, (s as FileInfo)?.Length ?? 0)));
                    dir.Hash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(list)));

                    return true;
                },
                onFileArtifact: (artifact)=>true, 
                onException: null, 
                exceptionAggregate: exceptionAggregate))

                yield return artifact;

            if (exceptionAggregate.Any())
                throw new AggregateException(exceptionAggregate);
        }


        [Fact]
        public void HashDirsTest()
        {

            var artifacts = Hash(Environment.SystemDirectory);

            foreach (var artifact in artifacts)
            {
                Assert.NotNull(artifact);
            }
        }
    }
}