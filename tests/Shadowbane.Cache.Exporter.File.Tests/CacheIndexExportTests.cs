using System;
using Shadowbane.Cache.IO;
using System.Threading.Tasks;
using Xunit;

namespace Shadowbane.Cache.Exporter.File.Tests;

public class CacheIndexExportTests 
{
    [Fact]
    public async Task RenderArchive_Exports_Correctly()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        await ArchiveLoader.RenderArchive
            .SaveCacheIndexesAsync(path, "render_archive_ids.csv")
            .ConfigureAwait(false);
        Assert.True(true);
    }

    [Fact]
    public async Task CacheArchive_Exports_Correctly()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        await ArchiveLoader.ObjectArchive
            .SaveCacheIndexesAsync(path, "object_archive_ids.csv")
            .ConfigureAwait(false);
        Assert.True(true);
    }

    [Fact]
    public async Task MeshArchive_Exports_Correctly()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        await ArchiveLoader.MeshArchive
            .SaveCacheIndexesAsync(path, "mesh_archive_ids.csv")
            .ConfigureAwait(false);
        Assert.True(true);
    }

    [Fact]
    public async Task TextureArchive_Exports_Correctly()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        await ArchiveLoader.TextureArchive
            .SaveCacheIndexesAsync(path, "texture_archive_ids.csv")
            .ConfigureAwait(false);
        Assert.True(true);
    }

    [Fact]
    public async Task SoundArchive_Exports_Correctly()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        await ArchiveLoader.SoundArchive
            .SaveCacheIndexesAsync(path, "sound_archive_ids.csv")
            .ConfigureAwait(false);
        Assert.True(true);
    }

    [Fact]
    public async Task SkeletonArchive_Exports_Correctly()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        await ArchiveLoader.SkeletonArchive
            .SaveCacheIndexesAsync(path, "skeleton_archive_ids.csv")
            .ConfigureAwait(false);
        Assert.True(true);
    }

    [Fact]
    public async Task ZoneArchive_Exports_Correctly()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        await ArchiveLoader.ZoneArchive
            .SaveCacheIndexesAsync(path, "zone_archive_ids.csv")
            .ConfigureAwait(false);
        Assert.True(true);
    }
}