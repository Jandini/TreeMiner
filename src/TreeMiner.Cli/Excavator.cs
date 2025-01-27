using Microsoft.Extensions.Logging;
using TreeMiner.FileSystem;

namespace TreeMiner.Cli
{
    internal class Excavator(ILogger<Excavator> logger) : IFileSystemExcavator<FileSystemArtifact>
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
            logger.LogError(exception, "An exception occurred while processing artifact {Artifact}", exception.Artifact.Info.FullName);
        }

        public void OnFileArtifact(FileSystemArtifact fileArtifact)
        {
            
        }
    }
}
