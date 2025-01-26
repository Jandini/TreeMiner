using Microsoft.Extensions.Logging;
using TreeMiner.FileSystem;

namespace TreeMiner.Cli
{
    internal class Excavator(ILogger<Excavator> logger) : IFileSystemExcavator<Artifact>
    {

        public IEnumerable<FileSystemInfo> GetArtifacts(DirectoryInfo dirArtifact)
        {
            return dirArtifact.GetFileSystemInfos();   
        }

        public bool OnDirArtifact(Artifact dirArtifact, IEnumerable<FileSystemInfo> dirContent)
        {
            return true;
        }

        public bool OnException(ArtifactException<FileSystemInfo> exception)
        {
            logger.LogError(exception, "An exception occurred while processing artifact {Artifact}", exception.Artifact.Info.FullName);
            return true;
        }

        public bool OnFileArtifact(Artifact fileArtifact)
        {
            return true;
        }
    }
}
