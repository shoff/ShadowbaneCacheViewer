namespace Shadowbane.CacheViewer;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shadowbane.Cache.IO;

internal static class Program
{
    private static readonly CancellationTokenSource cts = new CancellationTokenSource();
    public static readonly string[] exitWords = { "exit", "quit", "bye", "q", "stop", "leave" };

    // ReSharper disable once CognitiveComplexity
    public static async Task Main()
    {
        Log.Logger = new LoggerConfiguration()
            // add console as logging target
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            // set default minimum level
            .MinimumLevel.Debug()
            .CreateLogger();

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();

        services.AddOptions()
            .RegisterOptions(configuration)
            .RegisterDependencies();

        Console.ResetColor(); // reset the console color to default

        using var serviceProvider = services.BuildServiceProvider();

        
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        CacheViewForm cacheViewForm = new CacheViewForm(serviceProvider.GetRequiredService<IRenderableBuilder>());

        Application.Run(cacheViewForm);
    }

}