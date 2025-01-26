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

        public bool OnDirArtifact(FileSystemArtifact dirArtifact, IEnumerable<FileSystemInfo> dirContent)
        {
            return true;
        }

        public bool OnException(ArtifactException<FileSystemInfo> exception)
        {
            logger.LogError(exception, "An exception occurred while processing artifact {Artifact}", exception.Artifact.Info.FullName);
            return true;
        }

        public bool OnFileArtifact(FileSystemArtifact fileArtifact)
        {
            return true;
        }
    }
}
