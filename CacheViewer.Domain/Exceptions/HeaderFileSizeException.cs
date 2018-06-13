namespace CacheViewer.Domain.Exceptions
{
    using System;

    [Serializable]
    public sealed class HeaderFileSizeException : ApplicationException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HeaderFileSizeException" /> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public HeaderFileSizeException(string message)
            : base(message)
        {
        }
    }
}