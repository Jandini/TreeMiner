using Microsoft.Extensions.Logging;
using TreeMiner.Cli;
using TreeMiner.FileSystem;

internal class Main(
    ILogger<Main> logger,
    Excavator excavator)
{

    public async Task RunAsync(CancellationToken cancellationToken)
    {        


        var rootPath = @"C:\"; // Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        //var fileSystemMiner = new GenericTreeMiner<FileSystemArtifact, FileSystemInfo, FileInfo, DirectoryInfo>();
        //var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(rootPath));

        //var artifacts = fileSystemMiner.GetArtifacts(rootArtifact, (dirInfo) => new List<FileSystemInfo>().AddRange(dirInfo.GetDirectories()).ToArray());
    
        var artifacts = FileSystemMiner.GetArtifacts(rootPath, excavator, cancellationToken);

        //var artifacts = FileSystemMiner.GetArtifacts(rootPath, out var exceptions, cancellationToken);

        foreach (var artifact in artifacts)
            logger.LogDebug($"{{artifactId}} {{artifactParentId}} [{artifact.Info.FullName}]", artifact.Id, artifact.ParentId);

        await Task.CompletedTask;
    }
}