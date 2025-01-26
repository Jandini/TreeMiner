using TreeMiner.FileSystem;

namespace TreeMiner.Cli
{
    internal class Artifact : IFileSystemArtifact
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public int Level { get; set; }
        public FileSystemInfo Info { get; set; }
    }
}