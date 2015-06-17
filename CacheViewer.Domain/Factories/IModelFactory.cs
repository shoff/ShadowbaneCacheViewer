using CacheViewer.Domain.Archive;
using CacheViewer.Domain.Models;

namespace CacheViewer.Domain.Factories
{
    public interface IModelFactory
    {
        /// <summary>
        /// Creates the specified buffer.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <returns></returns>
        Mesh Create(CacheIndex cacheIndex);
    }
}