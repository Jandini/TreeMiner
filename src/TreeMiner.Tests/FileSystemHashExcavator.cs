using System.Security.Cryptography;
using System.Text;
using TreeMiner.FileSystem;

namespace TreeMiner.Tests
{
    internal class FileSystemHashExcavator : IFileSystemExcavator<FileSystemHashArtifact>
    {

        private readonly List<Exception> _exceptionAggregate = new();

        public IEnumerable<FileSystemInfo> GetArtifacts(DirectoryInfo dirArtifact) => dirArtifact.GetFileSystemInfos();

        public void OnDirArtifact(FileSystemHashArtifact dirArtifact, IEnumerable<FileSystemInfo> dirContent)
        {
            var list = string.Join(';', dirContent.OrderBy(a => a.Name).Select(s => string.Join(',', s.Name, (s as FileInfo)?.Length ?? 0)));
            dirArtifact.Hash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(list)));
        }

        public void OnException(ArtifactException<FileSystemInfo> exception)
        {
            _exceptionAggregate.Add(exception); 
        }

        public void OnFileArtifact(FileSystemHashArtifact fileArtifact)
        {
            throw new NotImplementedException();
        }
    }
}
