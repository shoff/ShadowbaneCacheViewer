namespace CacheViewer.Domain.Factories
{
    using Archive;
    using Models;

    public interface IModelFactory
    {
        Mesh Create(CacheIndex cacheIndex);
        bool HasMeshId(int id);
    }
}