namespace TreeMiner
{
    public static class TreeMiner<T> where T : ITreeArtifact, new()
    {        

        /// <summary>
        /// Recursively dig through the directory tree and yield tree artifacts with parent-child relationships represented by GUIDs.
        /// </summary>
        /// <param name="dirArtifact">Root directory artifact.</param>
        /// <param name="onDirArtifact">Enrich and filter directory artifacts. If false is returned then artifact will not be returned.</param>
        /// <param name="onFileArtifact">Enrich and filter file artifacts.</param>
        /// <param name="onException">Exception handler. If not provided or false is returned then exception is throw and mining is interrupted.</param>
        /// <param name="exceptionAggregate"></param>
        /// <returns>Directory and file artifacts.</returns>
        public static IEnumerable<T> Dig(T dirArtifact, Func<T, IEnumerable<FileSystemInfo>, bool>? onDirArtifact, Func<T, bool>? onFileArtifact, Func<Exception, bool>? onException, List<Exception>? exceptionAggregate) 
        {
            IEnumerable<FileSystemInfo>? dirContent = null;

            try
            {
                // Get directory content, both files and directories
                if (dirArtifact.Info is DirectoryInfo dirInfo)
                    dirContent = dirInfo.GetFileSystemInfos();
            }
            catch (Exception ex)
            {
                exceptionAggregate?.Add(ex);
                if (!(onException?.Invoke(ex) ?? exceptionAggregate != null))
                    throw;                
            }

            // Check if the content was retrivied successfully. The dirContent can be null if exception handler call back is provided.
            if (dirContent != null)
            {
                if ((onDirArtifact?.Invoke(dirArtifact, dirContent) ?? true) && dirArtifact.Id != Guid.Empty)
                    // Return this folder only if that is not root folder 
                    yield return dirArtifact;

                // Mine directories recursively
                foreach (DirectoryInfo dirInfo in dirContent.OfType<DirectoryInfo>())
                     foreach (var subDirInfo in Dig(new T() { Id = Guid.NewGuid(), Level = dirArtifact.Level + 1, Parent = dirArtifact.Id, Info = dirInfo }, onDirArtifact!, onFileArtifact, onException, exceptionAggregate))
                           yield return subDirInfo;

                if (onFileArtifact != null)
                {
                    // Create and return file artifacts found in directory content
                    foreach (FileInfo fileInfo in dirContent.OfType<FileInfo>())
                    {
                        var fileArtifact = new T() { Id = Guid.NewGuid(), Parent = dirArtifact.Id, Level = dirArtifact.Level, Info = fileInfo };
                        if (onFileArtifact.Invoke(fileArtifact))
                            yield return fileArtifact;
                    }
                }
            }
        }
    }
}
