namespace TreeMiner
{
    public class GenericTreeMiner<TTreeArtifact, TBaseArtifact, TFileArtifact, TDirArtifact> where TTreeArtifact : ITreeArtifact, new() where TFileArtifact : class, TBaseArtifact where TDirArtifact : class, TBaseArtifact
    {

        /// <summary>
        /// Create root artifact.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="level"></param>
        /// <param name="id"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public TTreeArtifact GetRootArtifact(TBaseArtifact root, int level = 0, Guid id = default, Guid parent = default)
        {
            return new TTreeArtifact()
            {
                Id = id,
                Parent = parent,
                Level = level,
                Info = root,
            };
        }

        /// <summary>
        /// Recursively dig through the directory tree and yield tree artifacts with parent-child relationships represented by GUIDs.
        /// </summary>
        /// <param name="dirArtifact">Root directory artifact.</param>
        /// <param name="getArtifacts"></param>
        /// <param name="artifactType"></param>
        /// <param name="depthOption"></param>
        /// <param name="onDirArtifact">Enrich and filter directory artifacts. If false is returned then artifact will not be returned.</param>
        /// <param name="onFileArtifact">Enrich and filter file artifacts.</param>
        /// <param name="onException">Exception handler. If not provided or false is returned then exception is throw and mining is interrupted.</param>
        /// <returns>Directory and file artifacts.</returns>
        /// <exception cref="ArtifactException"></exception>
        public IEnumerable<TTreeArtifact> GetArtifacts(TTreeArtifact dirArtifact,
            Func<TDirArtifact, IEnumerable<TBaseArtifact>> getArtifacts,
            ArtifactType artifactType = ArtifactType.All, 
            DepthOption depthOption = DepthOption.Deep,            
            Func<TTreeArtifact, IEnumerable<TBaseArtifact>, bool>? onDirArtifact = null, 
            Func<TTreeArtifact, bool>? onFileArtifact = null, 
            Func<Exception, bool>? onException = null) 
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
                if (!(onException?.Invoke(ex) ?? false))
                    throw new ArtifactException(ex, dirArtifact);
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
                        foreach (var subDirInfo in GetArtifacts(new TTreeArtifact() { Id = Guid.NewGuid(), Level = dirArtifact.Level + 1, Parent = dirArtifact.Id, Info = dirInfo }, getArtifacts, artifactType, depthOption, onDirArtifact, onFileArtifact, onException))
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



        /// <summary>
        /// Recursively dig through the directory tree and yield tree artifacts with parent-child relationships represented by GUIDs.
        /// </summary>
        /// <param name="dirArtifact">Root directory artifact.</param>
        /// <param name="getArtifacts">Callback function to retrive file and sub directory artifacts from directory artifact.</param>
        /// <param name="artifactType">Type of returned artifacts. Default is All.</param>
        /// <param name="depthOption">Mining depth option. Deep is recursive, Shallow is top directory only.</param>
        /// <param name="exceptionAggregate"></param>
        /// <returns>Directory and file artifacts.</returns>
        public IEnumerable<TTreeArtifact> GetArtifacts(TTreeArtifact dirArtifact,
            Func<TDirArtifact, IEnumerable<TBaseArtifact>> getArtifacts,
            ArtifactType artifactType = ArtifactType.All,
            DepthOption depthOption = DepthOption.Deep,
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
                exceptionAggregate?.Add(new ArtifactException(ex, dirArtifact));
            }

            // Check if the content was retrivied successfully. The dirContent can be null if exception handler call back is provided.
            if (dirContent != null)
            {
                if ((artifactType & ArtifactType.Directories) == ArtifactType.Directories)
                {
                    if (dirArtifact.Id != Guid.Empty)
                        // Return this folder only if that is not root folder 
                        yield return dirArtifact;
                }

                // Mine directories recursively
                if (depthOption == DepthOption.Deep)
                {
                    foreach (TDirArtifact dirInfo in dirContent.OfType<TDirArtifact>())
                        foreach (var subDirInfo in GetArtifacts(new TTreeArtifact() { Id = Guid.NewGuid(), Level = dirArtifact.Level + 1, Parent = dirArtifact.Id, Info = dirInfo }, getArtifacts, artifactType, depthOption, exceptionAggregate))
                            yield return subDirInfo;
                }

                // Check if tree miner should return file artifacts
                if ((artifactType & ArtifactType.Files) == ArtifactType.Files)
                {
                    // Create and return file artifacts found in directory content
                    foreach (TFileArtifact fileInfo in dirContent.OfType<TFileArtifact>())
                        yield return new TTreeArtifact() { Id = Guid.NewGuid(), Parent = dirArtifact.Id, Level = dirArtifact.Level, Info = fileInfo };                                                
                }
            }
        }        
    }
}
