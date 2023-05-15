namespace Shadowbane.CacheViewer;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shadowbane.Cache.IO;
using Shadowbane.CacheViewer.Config;
using Shadowbane.CacheViewer.Services;

public static class Ioc
{
    public static IServiceCollection RegisterOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DirectoryOptions>(configuration.GetSection("DirectoryOptions"));
        return services;
    }

    public static IServiceCollection RegisterDependencies(
        this IServiceCollection services)
    {
        var serilogLogger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("log.txt").CreateLogger();

        services.AddLogging(builder =>
        {
            builder.AddSerilog(logger: serilogLogger, dispose: true);
        });

        services.AddSingleton<IRealTimeModelService, RealTimeModelService>();
        services.AddSingleton<IStructureService, StructureService>();
        services.AddSingleton<IMeshOnlyObjExporter, MeshOnlyObjExporter>();
        services.AddSingleton<IMeshFactory, MeshFactory>();
        services.AddSingleton<IRenderableBuilder, RenderableBuilder>();
        services.AddSingleton<ICacheRecordBuilder, CacheObjectBuilder>();
        services.AddSingleton<CacheViewForm>();
        return services;
    }

}