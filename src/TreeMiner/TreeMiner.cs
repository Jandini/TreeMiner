namespace TreeMiner
{
    public class TreeMiner<TTreeArtifact, TBaseArtifact, TFileArtifact, TDirArtifact> where TTreeArtifact : ITreeArtifact, new() where TFileArtifact : class, TBaseArtifact where TDirArtifact : class, TBaseArtifact
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
        public IEnumerable<TTreeArtifact> DigArtifacts(TTreeArtifact dirArtifact,
            Func<TDirArtifact, IEnumerable<TBaseArtifact>> getArtifacts,
            ArtifactType artifactType = ArtifactType.All, 
            DepthOption depthOption = DepthOption.Deep,            
            Func<TTreeArtifact, IEnumerable<TBaseArtifact>, bool>? onDirArtifact = null, 
            Func<TTreeArtifact, bool>? onFileArtifact = null, 
            Func<Exception, bool>? onException = null, 
            List<Exception>? exceptionAggregate = null) 
        {
            IEnumerable<TBaseArtifact>? dirContent = null;

            try
            {
                // Get directory content, both files and directories
                if (dirArtifact.Info is TDirArtifact dirInfo)
                    dirContent = getArtifacts.Invoke(dirInfo);
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
                if ((artifactType & ArtifactType.Directories) == ArtifactType.Directories)
                {
                    if ((onDirArtifact?.Invoke(dirArtifact, dirContent) ?? true) && dirArtifact.Id != Guid.Empty)
                        // Return this folder only if that is not root folder 
                        yield return dirArtifact;
                }

                // Mine directories recursively
                if (depthOption == DepthOption.Deep)
                {
                    foreach (TDirArtifact dirInfo in dirContent.OfType<TDirArtifact>())
                        foreach (var subDirInfo in DigArtifacts(new TTreeArtifact() { Id = Guid.NewGuid(), Level = dirArtifact.Level + 1, Parent = dirArtifact.Id, Info = dirInfo }, getArtifacts, artifactType, depthOption, onDirArtifact, onFileArtifact, onException, exceptionAggregate))
                            yield return subDirInfo;
                }

                // Check if tree miner should return file artifacts
                if ((artifactType & ArtifactType.Files) == ArtifactType.Files)
                {
                    // Create and return file artifacts found in directory content
                    foreach (TFileArtifact fileInfo in dirContent.OfType<TFileArtifact>())
                    {
                        var fileArtifact = new TTreeArtifact() { Id = Guid.NewGuid(), Parent = dirArtifact.Id, Level = dirArtifact.Level, Info = fileInfo };
                        if (onFileArtifact?.Invoke(fileArtifact) ?? true)
                            yield return fileArtifact;
                    }
                }
            }
        }
    }
}
