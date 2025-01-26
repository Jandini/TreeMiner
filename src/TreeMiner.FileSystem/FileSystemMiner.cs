namespace TreeMiner.FileSystem
{
    public class FileSystemMiner<TTreeArtifact> : GenericTreeMiner<TTreeArtifact, FileSystemInfo, FileInfo, DirectoryInfo> where TTreeArtifact : IFileSystemArtifact, new()
    {

        public static IEnumerable<IFileSystemArtifact> GetArtifacts(string path, CancellationToken cancellationToken = default)
        {
            var fsm = new FileSystemMiner<FileSystemArtifact>();
            var root = fsm.GetRootArtifact(new DirectoryInfo(path));
            return fsm.GetArtifacts(root, new FileSystemExcavator(cancellationToken));            
        }        
    }
}
