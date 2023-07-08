using System.Security.Cryptography;
using System.Text;

namespace TreeMiner.Tests
{
    public class TreeMiner_Tests
    {

        public static IEnumerable<T> GetArtifacts<T>(string root) where T : ITreeArtifact, new()
        {
            var exceptionAggregate = new List<Exception>();
            
            var fileSystemMiner = new FileSystemMiner<T>();
            var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(root));            

            foreach (var artifact in fileSystemMiner.GetArtifacts(rootArtifact, (dirInfo) => dirInfo.GetFileSystemInfos()))
                yield return artifact;

            if (exceptionAggregate.Any())
                throw new AggregateException(exceptionAggregate);
        }


        [Fact]
        public void DigFileSystemTest()
        {
            var artifacts = GetArtifacts<TreeArtifact>(Environment.SystemDirectory);


            foreach (var artifact in artifacts)
            {
                Assert.NotNull(artifact);
            }
        }



        public static IEnumerable<TreeArtifactHash> Hash(string root)
        {
            var exceptionAggregate = new List<Exception>();

            var fileSystemMiner = new FileSystemMiner<TreeArtifactHash>();

            var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(root));

            rootArtifact.Hash = Convert.ToHexString(MD5.HashData(Array.Empty<byte>()));


            foreach (var artifact in fileSystemMiner.GetFileSystemArtifacts(
                dirArtifact: rootArtifact,
                onDirArtifact: (dir, content) =>
                {
                    var list = string.Join(';', content.OrderBy(a => a.Name).Select(s => string.Join(',', s.Name, (s as FileInfo)?.Length ?? 0)));
                    dir.Hash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(list)));

                    return true;
                },
                artifactType: ArtifactType.Directories,
                onFileArtifact: null,
                onException: (exception) => { exceptionAggregate.Add(exception); return true; })) 
            {
                yield return artifact;
            }

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