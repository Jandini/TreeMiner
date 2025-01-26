namespace TreeMiner.FileSystem
{
    public class FileSystemMiner
    {
        public static IEnumerable<IFileSystemArtifact> GetArtifacts(string path, CancellationToken cancellationToken = default)
        {
            var fileSystemMiner = new GenericFileSystemMiner<FileSystemArtifact>();
            var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(path));
            return fileSystemMiner.GetArtifacts(rootArtifact, new FileSystemExcavator(cancellationToken));
        }

        public static IEnumerable<TTreeArtifact> GetArtifacts<TTreeArtifact>(string path, IFileSystemExcavator<TTreeArtifact> excavator, CancellationToken cancellationToken = default) where TTreeArtifact : IFileSystemArtifact, new()
        {
            var fileSystemMiner = new GenericFileSystemMiner<TTreeArtifact>();
            var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(path));
            return fileSystemMiner.GetArtifacts(rootArtifact, excavator);
        }

        public static IEnumerable<IFileSystemArtifact> GetArtifacts(string path, out List<ArtifactException<FileSystemInfo>> exceptions, CancellationToken cancellationToken = default)
        {
            var fileSystemMiner = new GenericFileSystemMiner<FileSystemArtifact>();
            var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(path));
            exceptions = new List<ArtifactException<FileSystemInfo>>();
            return fileSystemMiner.GetArtifacts(rootArtifact, new FileSystemExceptionExcavator(exceptions, cancellationToken));
        }
    }
}
