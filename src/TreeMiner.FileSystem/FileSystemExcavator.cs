

namespace TreeMiner.FileSystem
{
    internal class FileSystemExcavator : ITreeExcavator<FileSystemArtifact, FileSystemInfo, FileInfo, DirectoryInfo>
    {
        private readonly CancellationToken _cancellationToken;

        public FileSystemExcavator(CancellationToken cancellationToken = default)
        {
            _cancellationToken = cancellationToken;
        }
        
        public IEnumerable<FileSystemInfo> GetArtifacts(DirectoryInfo dirArtifact)
        {
            _cancellationToken.ThrowIfCancellationRequested();
            return dirArtifact.GetFileSystemInfos();
        }

        public bool OnDirArtifact(FileSystemArtifact dirArtifact, IEnumerable<FileSystemInfo> dirContent)
        {
            return true;
        }

        public bool OnException(ArtifactException<FileSystemInfo> exception)
        {
            return false;
        }

        public bool OnFileArtifact(FileSystemArtifact fileArtifact)
        {
            return true;
        }
    }
}
