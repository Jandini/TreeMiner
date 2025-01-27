

namespace TreeMiner.FileSystem
{
    internal class FileSystemExcavator : IFileSystemExcavator<FileSystemArtifact>
    {        
        public IEnumerable<FileSystemInfo> GetArtifacts(DirectoryInfo dirArtifact)
        {
            return dirArtifact.GetFileSystemInfos();
        }

        public void OnDirArtifact(FileSystemArtifact dirArtifact, IEnumerable<FileSystemInfo> dirContent)
        {

        }

        public void OnException(ArtifactException<FileSystemInfo> exception)
        {
        }

        public void OnFileArtifact(FileSystemArtifact fileArtifact)
        {
        }
    }
}
