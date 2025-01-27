


namespace TreeMiner.FileSystem
{
    internal class FileSystemExcavator : IFileSystemExcavator<FileSystemArtifact>
    {        

        private readonly List<ArtifactException<FileSystemInfo>> _artifactExceptions = new();
        private readonly ExceptionOption _exceptionOption;

        public FileSystemExcavator(ExceptionOption exceptionOption)
        {
            _exceptionOption = exceptionOption;
        }

        public IEnumerable<FileSystemInfo> GetArtifacts(DirectoryInfo dirArtifact)
        {
            return dirArtifact.GetFileSystemInfos();
        }

        public void OnDirArtifact(FileSystemArtifact dirArtifact, IEnumerable<FileSystemInfo> dirContent)
        {

        }

        public void OnException(ArtifactException<FileSystemInfo> exception)
        {
            if (_exceptionOption == ExceptionOption.ThrowImmediatelly)
                throw exception;
            else if (_exceptionOption == ExceptionOption.ThrowAfter)
                _artifactExceptions.Add(exception);
        }

        public void OnFileArtifact(FileSystemArtifact fileArtifact)
        {
        }

        internal void ThrowExceptions()
        {
            if (_exceptionOption == ExceptionOption.ThrowAfter && _artifactExceptions.Any())
                throw new AggregateException(_artifactExceptions);
        }
    }
}
