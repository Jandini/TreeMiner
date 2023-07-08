namespace TreeMiner.Tests
{

    public class FileSystemMiner<TTreeArtifact> : GenericTreeMiner<TTreeArtifact, FileSystemInfo, FileInfo, DirectoryInfo> where TTreeArtifact : ITreeArtifact, new()
    {        
        public IEnumerable<TTreeArtifact> GetFileSystemArtifacts(TTreeArtifact dirArtifact, ArtifactType artifactType = ArtifactType.All, DepthOption depthMode = DepthOption.Deep, Func<TTreeArtifact, IEnumerable<FileSystemInfo>, bool>? onDirArtifact = null, Func<TTreeArtifact, bool>? onFileArtifact = null, Func<Exception, bool>? onException = null) 
            => GetArtifacts(dirArtifact, (dirInfo) => dirInfo.GetFileSystemInfos(), artifactType, depthMode, onDirArtifact, onFileArtifact, onException);

        public IEnumerable<TTreeArtifact> GetFileSystemArtifacts(TTreeArtifact dirArtifact, ArtifactType artifactType = ArtifactType.All, DepthOption depthMode = DepthOption.Deep, List<Exception>? exceptionAggregate = null)
            => GetArtifacts(dirArtifact, (dirInfo) => dirInfo.GetFileSystemInfos(), artifactType, depthMode, exceptionAggregate);
    }
}
