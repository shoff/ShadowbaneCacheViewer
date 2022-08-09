namespace Shadowbane.CacheViewer.Services;

using Cache;
using Cache.IO;
using Models;

public class RealTimeModelService
{
    public async Task<List<IMesh>> GenerateModelAsync(uint cacheObjectIdentity)
    {
        var meshFactory = new MeshFactory(Program.logger);
        var renderEntities = new List<Renderable>();
        var meshEntities = new List<IMesh>();

        var meshModels = new List<IMesh>();
        foreach (var mesh in meshEntities)
        {
            if (mesh == null)
            {
                continue;
            }
            var cindex = ArchiveLoader.MeshArchive.CacheIndices.FirstOrDefault(c => c.identity == mesh.Identity);
            var m = meshFactory.Create(cindex);

            foreach (var rt in mesh.Textures)
            {
                // texture should not be null
                // var tex = TextureFactory.Instance.Build(rt.TextureId);
                m.Textures.Add(rt);
            }
            meshModels.Add(m);
        }

        Program.logger?.Information(
            $"GenerateModelAsync for {cacheObjectIdentity} returned mesh collection of {meshModels.Count}.");
        return meshModels;
    }
}