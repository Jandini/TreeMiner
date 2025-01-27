using TreeMiner.FileSystem;

namespace TreeMiner.Tests
{
    public class FileSystemHashArtifact : FileSystemArtifact, IFileSystemArtifact
    {
        public string? Hash { get; set; }
    }
}
