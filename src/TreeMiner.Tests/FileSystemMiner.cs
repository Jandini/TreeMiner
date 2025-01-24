namespace TreeMiner.Tests
{

    public class FileSystemMiner<TTreeArtifact> : GenericTreeMiner<TTreeArtifact, FileSystemInfo, FileInfo, DirectoryInfo> where TTreeArtifact : ITreeArtifact<FileSystemInfo>, new()
    {
        public IEnumerable<TTreeArtifact> GetFileSystemArtifacts(TTreeArtifact dirArtifact, Func<TTreeArtifact, IEnumerable<FileSystemInfo>, bool> onDirArtifact, Func<TTreeArtifact, bool>? onFileArtifact, Func<Exception, bool>? onException, ArtifactType artifactType = ArtifactType.All, DepthOption depthOption = DepthOption.Deep) 
            => GetArtifacts(dirArtifact, (dirInfo) => dirInfo.GetFileSystemInfos(), onDirArtifact, onFileArtifact, onException, depthOption, artifactType);

        public IEnumerable<TTreeArtifact> GetFileSystemArtifacts(TTreeArtifact dirArtifact, List<Exception> exceptionAggregate, ArtifactType artifactType = ArtifactType.All, DepthOption depthMode = DepthOption.Deep)
            => GetArtifacts(dirArtifact, (dirInfo) => dirInfo.GetFileSystemInfos(), exceptionAggregate, artifactType, depthMode);
    }
}
