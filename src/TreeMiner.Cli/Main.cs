using Microsoft.Extensions.Logging;
using TreeMiner;

internal class Main
{
    private readonly ILogger<Main> _logger;

    public Main(ILogger<Main> logger)
    {
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Hello, World!");


        var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        var fileSystemMiner = new GenericTreeMiner<FileSystemArtifact, FileSystemInfo, FileInfo, DirectoryInfo>();
        var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(rootPath));

        var artifacts = fileSystemMiner.GetArtifacts(rootArtifact, (dirInfo) => dirInfo.GetFileSystemInfos());

        foreach (var artifact in artifacts)
            Console.WriteLine($"{artifact.Id} {artifact.ParentId} [{artifact.Info.FullName}]");


        await Task.Delay(1000, cancellationToken);
    }
}