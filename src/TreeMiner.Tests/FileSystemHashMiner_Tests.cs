using System.Security.Cryptography;
using System.Text;
using TreeMiner.FileSystem;

namespace TreeMiner.Tests
{
    public class FileSystemHashMiner_Tests
    {

        public static IEnumerable<T> GetFileSystemArtifacts<T>(string root) where T : IFileSystemArtifact, new()
        {            
            var fileSystemMiner = new GenericFileSystemMiner<T>();
            var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(root));

            foreach (var artifact in fileSystemMiner.GetArtifacts(rootArtifact, (dirInfo) => dirInfo.GetFileSystemInfos()))
                yield return artifact;            
        }


        [Fact]
        public void DigFileSystemTest()
        {
            var artifacts = GetFileSystemArtifacts<FileSystemArtifact>(Environment.SystemDirectory);

            Assert.NotEmpty(artifacts);          
        }



        public static IEnumerable<FileSystemHashArtifact> HashFunc(string root)
        {

            var fileSystemMiner = new GenericFileSystemMiner<FileSystemHashArtifact>();

            var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(root));

            rootArtifact.Hash = Convert.ToHexString(MD5.HashData(Array.Empty<byte>()));


            foreach (var artifact in fileSystemMiner.GetArtifacts(rootArtifact, new FileSystemHashExcavator()))
            {
                yield return artifact;
            }
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
            var fileSystemMiner = new GenericFileSystemMiner<FileSystemHashArtifact>();
            var fileSystemExcavator = new FileSystemHashExcavator();

            var rootDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));
            var rootArtifact = fileSystemMiner.GetRootArtifact(rootDir);

            var artifacts = fileSystemMiner.GetArtifacts(rootArtifact, fileSystemExcavator);


            var count = artifacts.Count();

            foreach (var artifact in artifacts)
            {
                Assert.NotNull(artifact);
            }
        }



        [Fact]
        public void GenericFileSystemTest()
        {
            var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var fileSystemMiner = new GenericTreeMiner<FileSystemArtifact, FileSystemInfo, FileInfo, DirectoryInfo>();
            var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(rootPath));
            
            var artifacts = fileSystemMiner.GetArtifacts(rootArtifact, (dirInfo) => dirInfo.GetFileSystemInfos());


            var count = artifacts.Count();

            foreach (var artifact in artifacts)
            {
                Assert.NotNull(artifact);
            }
        }
    }
}