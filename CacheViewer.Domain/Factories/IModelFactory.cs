namespace CacheViewer.Domain.Factories
{
    using Archive;
    using Models;

    public interface IModelFactory
    {
        /// <summary>
        ///     Creates the specified buffer.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <returns></returns>
        Mesh Create(CacheIndex cacheIndex);
    }
}