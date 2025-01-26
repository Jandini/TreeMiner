namespace TreeMiner.FileSystem
{
    public interface IFileSystemExcavator<T> : ITreeExcavator<T, FileSystemInfo, FileInfo, DirectoryInfo> where T : IFileSystemArtifact, new()
    {

    }
}
