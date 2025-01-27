namespace TreeMiner
{
    /// <summary>
    /// Generic tree miner class for retrieving directory and file artifacts from a tree structure.
    /// </summary>
    /// <typeparam name="TTreeArtifact">The type of the tree artifacts.</typeparam>
    /// <typeparam name="TBaseArtifact">The base type of the artifacts.</typeparam>
    /// <typeparam name="TFileArtifact">The type of the file artifacts, derived from TBaseArtifact.</typeparam>
    /// <typeparam name="TDirArtifact">The type of the directory artifacts, derived from TBaseArtifact.</typeparam> 
    public class GenericTreeMiner<TTreeArtifact, TBaseArtifact, TFileArtifact, TDirArtifact>
        where TTreeArtifact : ITreeArtifact<TBaseArtifact>, new()
        where TFileArtifact : class, TBaseArtifact
        where TDirArtifact : class, TBaseArtifact
    {
        // private new List<ArtifactException<TBaseArtifact>> _exceptionAggregate = new();

        /// <summary>
        /// Create a root artifact.
        /// </summary>
        /// <param name="root">The base artifact representing the root.</param>
        /// <param name="level">The level of the root artifact in the tree. Default is 0.</param>
        /// <param name="id">The unique identifier for the root artifact. Default is default(Guid).</param>
        /// <param name="parent">The unique identifier of the parent artifact. Default is default(Guid).</param>
        /// <returns>The root artifact.</returns>
        public TTreeArtifact GetRootArtifact(TBaseArtifact root, int level = 0, Guid id = default, Guid parent = default)
        {
            return new TTreeArtifact()
            {
                Id = id,
                ParentId = parent,
                Level = level,
                Info = root,
            };
        }



        /// <summary>
        /// Retrieves directory and file artifacts from the tree based on the root directory artifact.
        /// </summary>
        /// <param name="dirArtifact">The root directory artifact.</param>
        /// <param name="getArtifacts">Function to retrieve artifacts for a given directory.</param>
        /// <param name="onDirArtifact">Callback to enrich and filter directory artifacts. Return false to exclude an artifact.</param>
        /// <param name="onFileArtifact">Callback to enrich and filter file artifacts.</param>
        /// <param name="onException">Callback to handle exceptions. Return false to interrupt the mining process.</param>
        /// <param name="depthOption">The depth option for mining. Default is DepthOption.Deep.</param>
        /// <param name="artifactType">The type of artifacts to include. Default is ArtifactType.All.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An enumerable collection of directory and file artifacts.</returns>
        /// <exception cref="ArtifactException">Thrown when an exception occurs and onException callback does not handle it.</exception>
        public IEnumerable<TTreeArtifact> GetArtifacts(TTreeArtifact dirArtifact,
            Func<TDirArtifact, IEnumerable<TBaseArtifact>> getArtifacts,
            Action<TTreeArtifact, IEnumerable<TBaseArtifact>>? onDirArtifact,
            Action<TTreeArtifact>? onFileArtifact,
            Action<ArtifactException<TBaseArtifact>>? onException,
            DepthOption depthOption = DepthOption.Deep,
            ArtifactType artifactType = ArtifactType.All,
            CancellationToken cancellationToken = default)
        {
            IEnumerable<TBaseArtifact>? dirContent = null;
            cancellationToken.ThrowIfCancellationRequested();

            try
            {

                // Get directory content, both files and directories
                if (dirArtifact.Info is TDirArtifact dirInfo)
                    dirContent = getArtifacts.Invoke(dirInfo);

            }
            catch (Exception ex)
            {
                var aex = new ArtifactException<TBaseArtifact>(ex, dirArtifact);
                onException?.Invoke(aex);

                //if (exceptionOption == ExceptionOption.ThrowImmediatelly)
                //    throw new ArtifactException<TBaseArtifact>(ex, dirArtifact);
            }

            // Check if the content was retrivied successfully. The dirContent can be null if exception handler call back is provided.
            if (dirContent != null)
            {
                if ((artifactType & ArtifactType.Directories) == ArtifactType.Directories && dirArtifact.Id != Guid.Empty)
                {
                    onDirArtifact?.Invoke(dirArtifact, dirContent);

                    // Return this folder only if that is not root folder 
                    yield return dirArtifact;
                }

                // Mine directories recursively
                if (depthOption == DepthOption.Deep)
                {
                    foreach (TDirArtifact dirInfo in dirContent.OfType<TDirArtifact>())
                        foreach (var subDirInfo in GetArtifacts(new TTreeArtifact() { Id = Guid.NewGuid(), Level = dirArtifact.Level + 1, ParentId = dirArtifact.Id, Info = dirInfo }, getArtifacts, onDirArtifact, onFileArtifact, onException, depthOption, artifactType))
                            yield return subDirInfo;
                }

                // Check if tree miner should return file artifacts
                if ((artifactType & ArtifactType.Files) == ArtifactType.Files)
                {
                    // Create and return file artifacts found in directory content
                    foreach (TFileArtifact fileInfo in dirContent.OfType<TFileArtifact>())
                    {
                        var fileArtifact = new TTreeArtifact() { Id = Guid.NewGuid(), ParentId = dirArtifact.Id, Level = dirArtifact.Level, Info = fileInfo };
                        onFileArtifact?.Invoke(fileArtifact);

                        yield return fileArtifact;
                    }
                }
            }
        }


        /// <summary>
        /// Retrieves directory and file artifacts from the tree based on the root directory artifact using the specified tree excavator.
        /// </summary>
        /// <param name="dirArtifact">The root directory artifact.</param>
        /// <param name="treeExcavator">The tree excavator to use for mining artifacts.</param>
        /// <param name="artifactOptions">Options for artifact retrieval. Provides depth and type of artifact to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An enumerable collection of directory and file artifacts.</returns>
        /// <exception cref="ArtifactException">Thrown when an exception occurs during artifact retrieval.</exception>
        public IEnumerable<TTreeArtifact> GetArtifacts(TTreeArtifact dirArtifact, ITreeExcavator<TTreeArtifact, TBaseArtifact, TFileArtifact, TDirArtifact> treeExcavator, ArtifactOptions artifactOptions, CancellationToken cancellationToken = default)
        {
            IEnumerable<TBaseArtifact>? dirContent = null;
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Get directory content, both files and directories
                if (dirArtifact.Info is TDirArtifact dirInfo)
                    dirContent = treeExcavator.GetArtifacts(dirInfo);                
            }
            catch (Exception ex)
            {
                var aex = new ArtifactException<TBaseArtifact>(ex, dirArtifact);
                treeExcavator.OnException(aex);

                // throw new ArtifactException<TBaseArtifact>(ex, dirArtifact);
            }

            // Check if the content was retrivied successfully. The dirContent can be null if exception handler call back is provided.
            if (dirContent != null)
            {
                if ((artifactOptions.ArtifactType & ArtifactType.Directories) == ArtifactType.Directories)
                {
                    // Return this folder only if that is not root folder 
                    if (dirArtifact.Id != Guid.Empty)
                    {
                        treeExcavator.OnDirArtifact(dirArtifact, dirContent);
                        yield return dirArtifact;
                    }
                }

                // Mine directories recursively
                if (artifactOptions.ArtifactDepth == DepthOption.Deep)
                {
                    foreach (TDirArtifact dirInfo in dirContent.OfType<TDirArtifact>())
                        foreach (var subDirInfo in GetArtifacts(new TTreeArtifact() { Id = Guid.NewGuid(), Level = dirArtifact.Level + 1, ParentId = dirArtifact.Id, Info = dirInfo }, treeExcavator, artifactOptions, cancellationToken))
                            yield return subDirInfo;
                }

                // Check if tree miner should return file artifacts
                if ((artifactOptions.ArtifactType & ArtifactType.Files) == ArtifactType.Files)
                {
                    // Create and return file artifacts found in directory content
                    foreach (TFileArtifact fileInfo in dirContent.OfType<TFileArtifact>())
                    {
                        var fileArtifact = new TTreeArtifact() { Id = Guid.NewGuid(), ParentId = dirArtifact.Id, Level = dirArtifact.Level, Info = fileInfo };
                        treeExcavator.OnFileArtifact(fileArtifact);
                        
                        yield return fileArtifact;
                    }
                }
            }
        }


        /// <summary>
        /// Retrieves recursively directory and file artifacts from the tree based on the root directory artifact using the specified tree excavator.
        /// </summary>
        /// <param name="dirArtifact">The root directory artifact.</param>
        /// <param name="treeExcavator">The tree excavator to use for mining artifacts.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An enumerable collection of directory and file artifacts.</returns>
        /// <exception cref="ArtifactException">Thrown when an exception occurs during artifact retrieval.</exception>
        public IEnumerable<TTreeArtifact> GetArtifacts(TTreeArtifact dirArtifact, ITreeExcavator<TTreeArtifact, TBaseArtifact, TFileArtifact, TDirArtifact> treeExcavator, CancellationToken cancellationToken = default)
        {
            IEnumerable<TBaseArtifact>? dirContent = null;
            cancellationToken.ThrowIfCancellationRequested();

            try
            {

                // Get directory content, both files and directories
                if (dirArtifact.Info is TDirArtifact dirInfo)
                    dirContent = treeExcavator.GetArtifacts(dirInfo);                
            }
            catch (Exception ex)
            {
                var aex = new ArtifactException<TBaseArtifact>(ex, dirArtifact);
                treeExcavator.OnException(aex);
                    
                //throw new ArtifactException<TBaseArtifact>(ex, dirArtifact);
            }

            // Check if the content was retrivied successfully. The dirContent can be null if exception handler call back is provided.
            if (dirContent != null)
            {
                // Return this folder only if that is not root folder 
                if (dirArtifact.Id != Guid.Empty)
                {
                    treeExcavator.OnDirArtifact(dirArtifact, dirContent);
                    yield return dirArtifact;
                }

                // Mine directories recursively
                foreach (TDirArtifact dirInfo in dirContent.OfType<TDirArtifact>())
                {                  
                    foreach (var subDirInfo in GetArtifacts(new TTreeArtifact() { Id = Guid.NewGuid(), Level = dirArtifact.Level + 1, ParentId = dirArtifact.Id, Info = dirInfo }, treeExcavator, cancellationToken))
                    {                       
                        yield return subDirInfo;
                    }
                }

                // Create and return file artifacts found in directory content
                foreach (TFileArtifact fileInfo in dirContent.OfType<TFileArtifact>())
                {
                    var fileArtifact = new TTreeArtifact() { Id = Guid.NewGuid(), ParentId = dirArtifact.Id, Level = dirArtifact.Level, Info = fileInfo };
                    treeExcavator.OnFileArtifact(fileArtifact);

                    yield return fileArtifact;
                }
            }            
        }



        /// <summary>
        /// Recursively dig through the directory tree and yield tree artifacts with parent-child relationships represented by GUIDs.
        /// </summary>
        /// <param name="dirArtifact">Root directory artifact.</param>
        /// <param name="getArtifacts">Callback function to retrive file and sub directory artifacts from directory artifact.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="artifactType">Type of returned artifacts. Default is All.</param>
        /// <param name="depthOption">Mining depth option. Deep is recursive, Shallow is top directory only.</param>
        /// <param name="exceptionAggregate"></param>
        /// <returns>Directory and file artifacts.</returns>
        public IEnumerable<TTreeArtifact> GetArtifacts(TTreeArtifact dirArtifact,
            Func<TDirArtifact, IEnumerable<TBaseArtifact>> getArtifacts,
            List<ArtifactException<TBaseArtifact>> exceptionAggregate,
            CancellationToken cancellationToken = default,
            ArtifactType artifactType = ArtifactType.All,
            DepthOption depthOption = DepthOption.Deep)
        {
            IEnumerable<TBaseArtifact>? dirContent = null;
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Get directory content, both files and directories
                if (dirArtifact.Info is TDirArtifact dirInfo)
                    dirContent = getArtifacts.Invoke(dirInfo);
            }
            catch (Exception ex)
            {
                exceptionAggregate.Add(new ArtifactException<TBaseArtifact>(ex, dirArtifact));
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
                        foreach (var subDirInfo in GetArtifacts(new TTreeArtifact() { Id = Guid.NewGuid(), Level = dirArtifact.Level + 1, ParentId = dirArtifact.Id, Info = dirInfo }, getArtifacts, exceptionAggregate, cancellationToken, artifactType, depthOption))
                            yield return subDirInfo;
                }

                // Check if tree miner should return file artifacts
                if ((artifactType & ArtifactType.Files) == ArtifactType.Files)
                {
                    // Create and return file artifacts found in directory content
                    foreach (TFileArtifact fileInfo in dirContent.OfType<TFileArtifact>())
                        yield return new TTreeArtifact() { Id = Guid.NewGuid(), ParentId = dirArtifact.Id, Level = dirArtifact.Level, Info = fileInfo };
                }
            }
        }
    }
}
