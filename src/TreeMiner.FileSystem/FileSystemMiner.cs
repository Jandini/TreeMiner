namespace TreeMiner.FileSystem
{
    public class FileSystemMiner
    {
        public static IEnumerable<IFileSystemArtifact> GetArtifacts(string path, CancellationToken cancellationToken = default)
        {
            var fsm = new GenericFileSystemMiner<FileSystemArtifact>();
            var root = fsm.GetRootArtifact(new DirectoryInfo(path));
            return fsm.GetArtifacts(root, new FileSystemExcavator(cancellationToken));
        }


        public static IEnumerable<TTreeArtifact> GetArtifacts<TTreeArtifact>(string path, IFileSystemExcavator<TTreeArtifact> excavator, CancellationToken cancellationToken = default) where TTreeArtifact : IFileSystemArtifact, new()
        {
            var fsm = new GenericFileSystemMiner<TTreeArtifact>();
            var root = fsm.GetRootArtifact(new DirectoryInfo(path));
            return fsm.GetArtifacts(root, excavator);
        }

        public static IEnumerable<IFileSystemArtifact> GetArtifacts(string path, out List<ArtifactException<FileSystemInfo>> exceptions, CancellationToken cancellationToken = default)
        {
            var fsm = new GenericFileSystemMiner<FileSystemArtifact>();
            var root = fsm.GetRootArtifact(new DirectoryInfo(path));
            exceptions = new List<ArtifactException<FileSystemInfo>>();
            return fsm.GetArtifacts(root, new FileSystemExceptionExcavator(exceptions, cancellationToken));
        }
    }
}
