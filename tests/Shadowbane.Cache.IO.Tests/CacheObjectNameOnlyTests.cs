namespace Shadowbane.Cache.IO.Tests;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;


public class CacheObjectNameOnlyTests
{
    private readonly CacheObjectBuilder builder;
    private static readonly List<object[]> identities =
        ArchiveLoader.ObjectArchive.CacheIndices
            .Select(x => new object[] { x.identity })
            .ToList();
    public static IEnumerable<object[]> Data1000 => identities.Skip(0).Take(1000);
    public static IEnumerable<object[]> Data2000 => identities.Skip(1000).Take(1000);
    public static IEnumerable<object[]> Data3000 => identities.Skip(2000).Take(1000);
    public static IEnumerable<object[]> Data4000 => identities.Skip(3000).Take(1000);
    public static IEnumerable<object[]> Data5000 => identities.Skip(4000).Take(1000);
    public static IEnumerable<object[]> Data6000 => identities.Skip(5000).Take(1000);
    public static IEnumerable<object[]> Data7000 => identities.Skip(6000).Take(1000);
    public static IEnumerable<object[]> Data8000 => identities.Skip(7000).Take(1000);
    public static IEnumerable<object[]> Data9000 => identities.Skip(8000).Take(1000);
    public static IEnumerable<object[]> Data10000 => identities.Skip(9000);

    public CacheObjectNameOnlyTests()
    {
        this.builder = new CacheObjectBuilder();
         
    }
    [Theory]
    [MemberData(nameof(Data1000))]
    public void Cache_Id_0_1000_Parse_Correctly(uint identity)
    {
        var watch = new Stopwatch();
        watch.Start();
        var cacheObject = this.builder.NameOnly(identity);
        Assert.NotNull(cacheObject);
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 1000);
        CacheIndex renderIndex = ArchiveLoader.RenderArchive.CacheIndices.FirstOrDefault(c => c.identity == cacheObject.RenderId);
        Assert.Equal(cacheObject.RenderId, renderIndex.identity);
    }


    [Theory]
    [MemberData(nameof(Data2000))]
    public void Cache_Id_1001_2000_Parse_Correctly(uint identity)
    {
        var watch = new Stopwatch();
        watch.Start();
        var cacheObject = this.builder.NameOnly(identity);
        Assert.NotNull(cacheObject);
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 1000);
        CacheIndex renderIndex = ArchiveLoader.RenderArchive.CacheIndices.FirstOrDefault(c => c.identity == cacheObject.RenderId);
        Assert.Equal(cacheObject.RenderId, renderIndex.identity);
    }

    [Theory]
    [MemberData(nameof(Data3000))]
    public void Cache_Id_2001_3000_Parse_Correctly(uint identity)
    {
        var watch = new Stopwatch();
        watch.Start();
        var cacheObject = this.builder.NameOnly(identity);
        Assert.NotNull(cacheObject);
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 1000);
        CacheIndex renderIndex = ArchiveLoader.RenderArchive.CacheIndices.FirstOrDefault(c => c.identity == cacheObject.RenderId);
        Assert.Equal(cacheObject.RenderId, renderIndex.identity);
    }

    [Theory]
    [MemberData(nameof(Data4000))]
    public void Cache_Id_3001_4000_Parse_Correctly(uint identity)
    {
        var watch = new Stopwatch();
        watch.Start();
        var cacheObject = this.builder.NameOnly(identity);
        Assert.NotNull(cacheObject);
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 5000);
        CacheIndex renderIndex = ArchiveLoader.RenderArchive.CacheIndices.FirstOrDefault(c => c.identity == cacheObject.RenderId);
        Assert.Equal(cacheObject.RenderId, renderIndex.identity);
    }

    [Theory]
    [MemberData(nameof(Data5000))]
    public void Cache_Id_4001_5000_Parse_Correctly(uint identity)
    {
        var watch = new Stopwatch();
        watch.Start(); 
        var cacheObject = this.builder.NameOnly(identity);
        Assert.NotNull(cacheObject);
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 5000);
        CacheIndex renderIndex = ArchiveLoader.RenderArchive.CacheIndices.FirstOrDefault(c => c.identity == cacheObject.RenderId);
        Assert.Equal(cacheObject.RenderId, renderIndex.identity);
    }

    [Theory]
    [MemberData(nameof(Data6000))]
    public void Cache_Id_5001_6000_Parse_Correctly(uint identity)
    {
        var watch = new Stopwatch();
        watch.Start();
        
        var cacheObject = this.builder.NameOnly(identity);
        Assert.NotNull(cacheObject); 
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 5000);
        CacheIndex renderIndex = ArchiveLoader.RenderArchive.CacheIndices.FirstOrDefault(c => c.identity == cacheObject.RenderId);
        Assert.Equal(cacheObject.RenderId, renderIndex.identity);
    }

    [Theory]
    [MemberData(nameof(Data7000))]
    public void Cache_Id_6001_7000_Parse_CorrectlyAsync(uint identity)
    {
        var watch = new Stopwatch();
        watch.Start();
        var cacheObject = this.builder.NameOnly(identity);
        Assert.NotNull(cacheObject);
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 5000);
        CacheIndex renderIndex = ArchiveLoader.RenderArchive.CacheIndices.FirstOrDefault(c => c.identity == cacheObject.RenderId);
        Assert.Equal(cacheObject.RenderId, renderIndex.identity);
    }

    [Theory]
    [MemberData(nameof(Data8000))]
    public void Cache_Id_7001_8000_Parse_Correctly(uint identity)
    {
        var watch = new Stopwatch();
        watch.Start();
        var cacheObject = this.builder.NameOnly(identity);
        Assert.NotNull(cacheObject);
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 5000);
        CacheIndex renderIndex = ArchiveLoader.RenderArchive.CacheIndices.FirstOrDefault(c => c.identity == cacheObject.RenderId);
        Assert.Equal(cacheObject.RenderId, renderIndex.identity);
    }

    [Theory]
    [MemberData(nameof(Data9000))]
    public void Cache_Id_8001_9000_Parse_Correctly(uint identity)
    {
        var watch = new Stopwatch();
        watch.Start();
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 5000);
        CacheIndex renderIndex = ArchiveLoader.RenderArchive.CacheIndices.FirstOrDefault(c => c.identity == cacheObject.RenderId);
        Assert.Equal(cacheObject.RenderId, renderIndex.identity);
    }

    [Theory]
    [MemberData(nameof(Data10000))]
    public void Cache_Id_9001_Last_Parse_Correctly(uint identity)
    {
        if (identity is > 1999999 and < 2000517)
        {
            return;
        }
        var watch = new Stopwatch();
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 5000);
        CacheIndex renderIndex = ArchiveLoader.RenderArchive.CacheIndices.FirstOrDefault(c => c.identity == cacheObject.RenderId);
        Assert.Equal(cacheObject.RenderId, renderIndex.identity);
    }
}