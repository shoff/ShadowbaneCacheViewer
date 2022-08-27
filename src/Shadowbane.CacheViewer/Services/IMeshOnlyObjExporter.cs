namespace Shadowbane.CacheViewer.Services;

using Models;

public interface IMeshOnlyObjExporter
{
    string? ModelDirectory { get; set; }
    Task<bool> ExportAsync(Mesh? mesh, string? modelName = null, string? modelDirectory = null);
}