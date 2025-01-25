# TreeMiner.FileSystem

[![Build](https://github.com/Jandini/TreeMiner/actions/workflows/build.yml/badge.svg)](https://github.com/Jandini/TreeMiner/actions/workflows/build.yml)
[![NuGet](https://github.com/Jandini/TreeMiner/actions/workflows/nuget.yml/badge.svg)](https://github.com/Jandini/TreeMiner/actions/workflows/nuget.yml)

Generic parent-child GUID generator for directory trees, tracking the parent-child relationship and returning file and directory artifacts with assigned GUIDs.

# Quick Start



Use `FileSystemMiner` to recursively retrieve directory and file artifacts from file system using `FileInfo` and `DirectoryInfo` objects.

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
