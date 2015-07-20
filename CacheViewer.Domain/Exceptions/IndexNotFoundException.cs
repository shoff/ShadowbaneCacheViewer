using System;

namespace CacheViewer.Domain.Exceptions
{
    [Serializable]
    public class IndexNotFoundException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexNotFoundException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="indexId">The index identifier.</param>
        public IndexNotFoundException(Type type, int indexId)
            : base(string.Format(DomainMessages.IndexNotFoundException, type.Name, indexId))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexNotFoundException"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="indexId">The index identifier.</param>
        public IndexNotFoundException(string  name, int indexId)
            : base(string.Format(DomainMessages.IndexNotFoundException, name, indexId))
        {
        }
    }
}