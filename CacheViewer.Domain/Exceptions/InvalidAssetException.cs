namespace CacheViewer.Domain.Exceptions
{
    using System;

    [Serializable]
    public class InvalidAssetException : ApplicationException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidAssetException" /> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public InvalidAssetException(string message)
            : base(message)
        {
        }

        public InvalidAssetException(string message, string message1)
            : base(message)
        {
        }
    }
}