namespace TreeMiner
{
    public enum ExceptionOption
    {

        /// <summary>
        /// Throws an exception immediatelly after it is caught.
        /// </summary>
        ThrowImmediatelly,

        /// <summary>
        /// Throws an aggregate exception after all exceptions have been collected.
        /// </summary>
        ThrowAfter,

        /// <summary>
        /// Ignores exception and continues the operation.
        /// </summary>
        Continue,
    }
}
