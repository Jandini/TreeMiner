# TreeMiner

[![Build](https://github.com/Jandini/TreeMiner/actions/workflows/build.yml/badge.svg)](https://github.com/Jandini/TreeMiner/actions/workflows/build.yml)
[![NuGet](https://github.com/Jandini/TreeMiner/actions/workflows/nuget.yml/badge.svg)](https://github.com/Jandini/TreeMiner/actions/workflows/nuget.yml)

Generic parent-child GUID generator for directory trees, tracking the parent-child relationship and returning file and directory artifacts with assigned GUIDs.

# Quick Start

Recursively enumerate file system tree and retrieve file and folders with id and parent id represented by GUIDs.

Create class that will represent `ITreeArtifact` interface. 

```c#
using TreeMiner;

public class FileSystemArtifact : ITreeArtifact<FileSystemInfo>
{
    /// <summary>
    /// Gets or sets the unique identifier of the artifact.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the parent artifact.
    /// </summary>
    public Guid ParentId { get; set; }

    /// <summary>
    /// Gets or sets the level of the artifact in the directory tree.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Gets or sets additional information associated with the artifact.
    /// </summary>
    public FileSystemInfo Info { get; set; }
}
```



Use `GenericTreeMiner` to recursively retrieve directory and file artifacts from file system using `FileInfo` and `DirectoryInfo` objects.

```c#

// See https://aka.ms/new-console-template for more information
using TreeMiner;

var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

var fileSystemMiner = new GenericTreeMiner<FileSystemArtifact, FileSystemInfo, FileInfo, DirectoryInfo>();
var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(rootPath));

var artifacts = fileSystemMiner.GetArtifacts(rootArtifact, (dirInfo) => dirInfo.GetFileSystemInfos());

foreach (var artifact in artifacts)
    Console.WriteLine($"{artifact.Id} {artifact.ParentId} [{artifact.Info.FullName}]");
```




---
Created from [JandaBox](https://github.com/Jandini/JandaBox)

Tree icon created by [Freepik - Flaticon](https://www.flaticon.com/free-icons/tree)
