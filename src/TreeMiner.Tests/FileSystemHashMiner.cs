namespace TreeMiner.Tests
{

    internal class FileSystemHashMiner<TTreeArtifact> : GenericTreeMiner<TTreeArtifact, FileSystemInfo, FileInfo, DirectoryInfo> where TTreeArtifact : ITreeArtifact<FileSystemInfo>, new()
    {
        public IEnumerable<TTreeArtifact> GetFileSystemArtifacts(TTreeArtifact dirArtifact, Func<TTreeArtifact, IEnumerable<FileSystemInfo>, bool> onDirArtifact, Func<TTreeArtifact, bool>? onFileArtifact, Func<ArtifactException<FileSystemInfo>, bool>? onException, ArtifactType artifactType = ArtifactType.All, DepthOption depthOption = DepthOption.Deep) 
            => GetArtifacts(dirArtifact, (dirInfo) => dirInfo.GetFileSystemInfos(), onDirArtifact, onFileArtifact, onException, depthOption, artifactType);

        public IEnumerable<TTreeArtifact> GetFileSystemArtifacts(TTreeArtifact dirArtifact, List<ArtifactException<FileSystemInfo>> exceptionAggregate, ArtifactType artifactType = ArtifactType.All, DepthOption depthMode = DepthOption.Deep)
            => GetArtifacts(dirArtifact, (dirInfo) => dirInfo.GetFileSystemInfos(), exceptionAggregate, artifactType, depthMode);
    }
}
