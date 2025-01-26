namespace TreeMiner
{
    public enum ExceptionOption
    {
        /// <summary>
        /// Ignores all exceptions.
        /// </summary>
        Ignore,

        /// <summary>
        /// Throws an exception immediatelly after it is caught.
        /// </summary>
        ThrowImmediatelly,

        /// <summary>
        /// Throws an aggregate exception after all exceptions have been collected.
        /// </summary>
        ThrowAggregate,
    }
}
