namespace TreeMiner.FileSystem
{
    public class GenericFileSystemMiner<TTreeArtifact> : GenericTreeMiner<TTreeArtifact, FileSystemInfo, FileInfo, DirectoryInfo> where TTreeArtifact : IFileSystemArtifact, new()
    {
        
    }
}
