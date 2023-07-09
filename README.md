# TreeMiner

[![Build](https://github.com/Jandini/TreeMiner/actions/workflows/build.yml/badge.svg)](https://github.com/Jandini/TreeMiner/actions/workflows/build.yml)
[![NuGet](https://github.com/Jandini/TreeMiner/actions/workflows/nuget.yml/badge.svg)](https://github.com/Jandini/TreeMiner/actions/workflows/nuget.yml)

Generic parent-child GUID generator for directory trees, tracking the parent-child relationship and returning file and directory artifacts with assigned GUIDs.

# Quick Start

Recursively enumerate file system tree and retrieve file and folders with id and parent id represented by GUIDs.

Create class that will represent `ITreeArtifact` interface. 

```c#
/// <summary>
/// Represents a file system artifact in a directory tree.
/// </summary>
public class FileSystemArtifact : ITreeArtifact
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
    public object? Info { get; set; }
}
```



Use `GenericTreeMiner` to recursively retrieve directory and file artifacts from file system using `FileInfo` and `DirectoryInfo` objects.

```c#

// See https://aka.ms/new-console-template for more information
using TreeMiner;

var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

var fileSystemMiner = new GenericTreeMiner<FileSystemArtifact, FileSystemInfo, FileInfo, DirectoryInfo>();
var rootArtifact = fileSystemMiner.GetRootArtifact(new DirectoryInfo(rootPath));

var artifacts = fileSystemMiner.GetArtifacts(rootArtifact, (dirInfo) => dirInfo.GetFileSystemInfos());

foreach (var artifact in artifacts)
	Console.WriteLine($"{artifact.Id} {artifact.ParentId} [{(artifact.Info as FileSystemInfo)?.FullName}]");
```



The result looks like this:

```
ebec840e-ef89-47b0-93da-f63f5a4c2a2b 00000000-0000-0000-0000-000000000000 C:\Users\Matt\Desktop\Holiday Hack 2022
aef40c2f-c980-418e-9595-512e99a19f20 ebec840e-ef89-47b0-93da-f63f5a4c2a2b C:\Users\Matt\Desktop\Holiday Hack 2022\Wallet.md
4713e949-b89d-49d5-8d93-50b4c4afa04a 00000000-0000-0000-0000-000000000000 C:\Users\Matt\Desktop\speedtest
afd22f57-c427-44ad-8536-901a5341f92d 4713e949-b89d-49d5-8d93-50b4c4afa04a C:\Users\Matt\Desktop\speedtest\14_06_13-03-2023.png
b3c1d75b-2c8d-4fd4-949a-b588fdf2f0b4 4713e949-b89d-49d5-8d93-50b4c4afa04a C:\Users\Matt\Desktop\speedtest\14_08_13-03-2023.png
5953090d-cbfc-47c6-9f3f-6c3e39da477e 4713e949-b89d-49d5-8d93-50b4c4afa04a C:\Users\Matt\Desktop\speedtest\18_06_2013_17-15.png
307e31aa-768b-43b8-8a58-f12dce9fee60 4713e949-b89d-49d5-8d93-50b4c4afa04a C:\Users\Matt\Desktop\speedtest\23_03_12-03-2023.png
e14f50aa-a980-4979-b5bb-8d1853c96d4a 4713e949-b89d-49d5-8d93-50b4c4afa04a C:\Users\Matt\Desktop\speedtest\fix 1.png
06620fef-af7b-4575-9d3e-ec644b34ba73 4713e949-b89d-49d5-8d93-50b4c4afa04a C:\Users\Matt\Desktop\speedtest\fix 2.png
```



---
Created from [JandaBox](https://github.com/Jandini/JandaBox)

Tree icon created by [Freepik - Flaticon](https://www.flaticon.com/free-icons/tree)
