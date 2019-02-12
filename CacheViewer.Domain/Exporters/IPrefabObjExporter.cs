namespace CacheViewer.Domain.Exporters
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IPrefabObjExporter
    {
        Task CreatePrefabIndividualFiles(List<Mesh> meshModels, string modelName);
        Task CreatePrefabAsync(List<Mesh> meshModels, string modelName);
        string ModelDirectory { get; set; }
    }
}