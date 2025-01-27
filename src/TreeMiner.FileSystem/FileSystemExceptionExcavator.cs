

namespace TreeMiner.FileSystem
{
    internal class FileSystemExceptionExcavator : IFileSystemExcavator<FileSystemArtifact>
    {
        private readonly List<ArtifactException<FileSystemInfo>> _artifactExceptions;

        public FileSystemExceptionExcavator(List<ArtifactException<FileSystemInfo>> artifactExceptions)
        {
            _artifactExceptions = artifactExceptions;
        }
        
        public IEnumerable<FileSystemInfo> GetArtifacts(DirectoryInfo dirArtifact)
        {
            return dirArtifact.GetFileSystemInfos();
        }

        public bool OnDirArtifact(FileSystemArtifact dirArtifact, IEnumerable<FileSystemInfo> dirContent)
        {
            return true;
        }

        public bool OnException(ArtifactException<FileSystemInfo> exception)
        {
            _artifactExceptions.Add(exception);                
            return true;
        }

        public bool OnFileArtifact(FileSystemArtifact fileArtifact)
        {
            return true;
        }
    }
}
