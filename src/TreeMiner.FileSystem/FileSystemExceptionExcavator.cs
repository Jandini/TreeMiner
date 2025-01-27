

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

        public void OnDirArtifact(FileSystemArtifact dirArtifact, IEnumerable<FileSystemInfo> dirContent)
        {

        }

        public void OnException(ArtifactException<FileSystemInfo> exception)
        {
            _artifactExceptions.Add(exception);                
        }

        public void OnFileArtifact(FileSystemArtifact fileArtifact)
        {

        }
    }
}
