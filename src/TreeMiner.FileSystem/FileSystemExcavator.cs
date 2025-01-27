

namespace TreeMiner.FileSystem
{
    internal class FileSystemExcavator : IFileSystemExcavator<FileSystemArtifact>
    {        
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
            return false;
        }

        public bool OnFileArtifact(FileSystemArtifact fileArtifact)
        {
            return true;
        }
    }
}
