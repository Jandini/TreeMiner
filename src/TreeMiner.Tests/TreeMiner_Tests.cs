using System.Security.Cryptography;
using System.Text;

namespace TreeMiner.Tests
{
    public class TreeMiner_Tests
    {

        public static IEnumerable<T> GetFileSystemArtifacts<T>(string root) where T : ITreeArtifact, new()
        {            
            var fileSystemMiner = new FileSystemMiner<T>();
            var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(root));

            foreach (var artifact in fileSystemMiner.GetArtifacts(rootArtifact, (dirInfo) => dirInfo.GetFileSystemInfos()))
                yield return artifact;            
        }


        [Fact]
        public void DigFileSystemTest()
        {
            var artifacts = GetFileSystemArtifacts<FileSystemArtifact>(Environment.SystemDirectory);


            foreach (var artifact in artifacts)
            {
                Assert.NotNull(artifact);
            }
        }



        public static IEnumerable<FileSystemArtifactHash> HashFunc(string root)
        {
            var exceptionAggregate = new List<Exception>();

            var fileSystemMiner = new FileSystemMiner<FileSystemArtifactHash>();

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

            var artifacts = HashFunc(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));

            foreach (var artifact in artifacts)
            {
                Assert.NotNull(artifact);
            }
        }



        [Fact]
        public void ExcavatorDirsTest()
        {
            var fileSystemMiner = new FileSystemMiner<FileSystemArtifactHash>();
            var fileSystemExcavator = new FileSystemExcavatorHash();

            var rootDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));
            var rootArtifact = fileSystemMiner.GetRootArtifact(rootDir);

            var artifacts = fileSystemMiner.GetArtifacts(rootArtifact, fileSystemExcavator);


            var count = artifacts.Count();

            foreach (var artifact in artifacts)
            {
                Assert.NotNull(artifact);
            }
        }
    }
}