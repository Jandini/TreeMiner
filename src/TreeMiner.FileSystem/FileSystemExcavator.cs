

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
            return dirArtifact.GetFileSystemInfos();
        }

        public bool OnDirArtifact(FileSystemArtifact dirArtifact, IEnumerable<FileSystemInfo> dirContent)
        {
            return !_cancellationToken.IsCancellationRequested;
        }

        public bool OnException(ArtifactException<FileSystemInfo> exception)
        {
            throw exception;
        }

        public bool OnFileArtifact(FileSystemArtifact fileArtifact)
        {
            return !_cancellationToken.IsCancellationRequested;
        }
    }
}
