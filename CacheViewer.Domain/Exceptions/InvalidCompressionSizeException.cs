namespace CacheViewer.Domain.Exceptions
{
    using System;

    [Serializable]
    public sealed class InvalidCompressionSizeException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCompressionSizeException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
         public InvalidCompressionSizeException(string message)
            : base(message) { }
    }
}