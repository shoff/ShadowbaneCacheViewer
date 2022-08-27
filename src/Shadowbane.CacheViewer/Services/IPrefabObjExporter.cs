namespace Shadowbane.CacheViewer.Services;

using Cache;

public interface IPrefabObjExporter
{
    Task CreateIndividualObjFiles(ICollection<IMesh>? meshModels, string modelName);
    Task CreateSingleObjFile(ICollection<IMesh>? meshModels, string? modelName);
    string? ModelDirectory { get; set; }
}