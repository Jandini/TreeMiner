namespace TreeMiner.FileSystem
{
    public class FileSystemMiner
    {
        public static IEnumerable<IFileSystemArtifact> GetArtifacts(string path, CancellationToken cancellationToken = default, ExceptionOption exceptionOption = ExceptionOption.ThrowAfter)
        {
            var fileSystemMiner = new GenericFileSystemMiner<FileSystemArtifact>();
            var fileSystemExcavator = new FileSystemExcavator(exceptionOption);
            var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(path));
            
            foreach (var artifact in fileSystemMiner.GetArtifacts(rootArtifact, fileSystemExcavator, cancellationToken))
                yield return artifact;

            if (exceptionOption == ExceptionOption.ThrowAfter)
                fileSystemExcavator.ThrowExceptions();
        }

        public static IEnumerable<TTreeArtifact> GetArtifacts<TTreeArtifact>(string path, IFileSystemExcavator<TTreeArtifact> excavator, CancellationToken cancellationToken = default) where TTreeArtifact : IFileSystemArtifact, new()
        {
            var fileSystemMiner = new GenericFileSystemMiner<TTreeArtifact>();
            var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(path));
            return fileSystemMiner.GetArtifacts(rootArtifact, excavator, cancellationToken);
        }
     
    }
}
