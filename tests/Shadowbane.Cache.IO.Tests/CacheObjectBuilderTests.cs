// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

namespace Shadowbane.Cache.IO.Tests;

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Exporter.File;
using Moq;
using Xunit;

public class CacheObjectBuilderTests
{
    private const string PARSE_TIMING_FILE = "timings.csv";
    private static readonly string CACHE_EXPORT_PATH = "..\\..\\..\\..\\..\\CacheObjectExports/";
    private static readonly bool exportToFile = true;

    private static readonly List<object[]> identities =
        ArchiveLoader.ObjectArchive.CacheIndices
            .Select(x => new object[] { x.identity })
            .ToList();

    private readonly CacheObjectBuilder builder;
    private readonly Mock<IRenderableBuilder> renderableBuilder;

    private List<string> timings = new();

    public CacheObjectBuilderTests()
    {
        this.renderableBuilder = new Mock<IRenderableBuilder>();
        this.builder = new CacheObjectBuilder(this.renderableBuilder.Object);
    }

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
    public static IEnumerable<object[]> Data0 => identities.Skip(0).Take(100);
    public static IEnumerable<object[]> Data100 => identities.Skip(100).Take(100);
    public static IEnumerable<object[]> Data200 => identities.Skip(200).Take(100);
    public static IEnumerable<object[]> Data300 => identities.Skip(300).Take(100);
    public static IEnumerable<object[]> Data400 => identities.Skip(400).Take(100);
    public static IEnumerable<object[]> Data500 => identities.Skip(500).Take(100);
    public static IEnumerable<object[]> Data600 => identities.Skip(600).Take(100);
    public static IEnumerable<object[]> Data700 => identities.Skip(700).Take(100);
    public static IEnumerable<object[]> Data800 => identities.Skip(800).Take(100);
    public static IEnumerable<object[]> Data900 => identities.Skip(900).Take(100);

    [Theory]
    [MemberData(nameof(Data0))]
    public async Task Cache_Id_1_100_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data100))]
    public async Task Cache_Id_100_200_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data200))]
    public async Task Cache_Id_200_300_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data300))]
    public async Task Cache_Id_300_400_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data400))]
    public async Task Cache_Id_400_500_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data500))]
    public async Task Cache_Id_500_600_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data600))]
    public async Task Cache_Id_600_700_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data700))]
    public async Task Cache_Id_700_800_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data800))]
    public async Task Cache_Id_800_900_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data900))]
    public async Task Cache_Id_900_1000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    //[Theory]
    //[MemberData(nameof(Data1000))]
    //public void Cache_Id_1_1000_Parse_Correctly(uint identity)
    //{
    //    var cacheObject = this.builder.CreateAndParse(identity);
    //    Assert.NotNull(cacheObject);
    //}

    [Theory]
    [MemberData(nameof(Data2000))]
    public async Task Cache_Id_1001_2000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name.Replace('<', ' ').Replace('>', ' ')}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data3000))]
    public async Task Cache_Id_2001_3000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data4000))]
    public async Task Cache_Id_3001_4000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data5000))]
    public async Task Cache_Id_4001_5000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name.Replace('?', ' ')}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data6000))]
    public async Task Cache_Id_5001_6000_Parse_Correctly(uint identity)
    {
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    // [Theory]
    [MemberData(nameof(Data7000))]
    public async Task Cache_Id_6001_7000_Parse_CorrectlyAsync(uint identity)
    {
        var watch = new Stopwatch();
        watch.Start();
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        watch.Stop();

        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data8000))]
    public async Task Cache_Id_7001_8000_Parse_Correctly(uint identity)
    {
        var watch = new Stopwatch();
        watch.Start();
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        watch.Stop();

        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data9000))]
    public async Task Cache_Id_8001_9000_Parse_Correctly(uint identity)
    {
        var watch = new Stopwatch();
        watch.Start();
        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);
        watch.Stop();

        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Theory]
    [MemberData(nameof(Data10000))]
    public async Task Cache_Id_9001_Last_Parse_Correctly(uint identity)
    {
        if (identity > 1999999 && identity < 2000517)
        {
            return;
        }

        var cacheObject = this.builder.CreateAndParse(identity);
        Assert.NotNull(cacheObject);

        if (exportToFile)
        {
            await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
                $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
        }
    }

    [Fact]
    public void Cache_Id_2000000_Parses_Correctly()
    {
        var cacheObject = this.builder.CreateAndParse(2000000);
        Assert.NotNull(cacheObject);
    }


    //[Fact]
    //public async Task Master_Templar_Parses_Correctly_Async()
    //{
    //    uint identity = 252606;
    //    var cacheObject = this.builder.CreateAndParse(identity);

    //    await FileWriter.Writer.WriteAsync(cacheObject.Data, CACHE_EXPORT_PATH,
    //        $"{cacheObject.Name}{cacheObject.Identity.ToString(CultureInfo.InvariantCulture)}.cache");
    //    Assert.NotNull(cacheObject);
    //}
}