namespace Shadowbane.CacheViewer;

using Serilog;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        logger = Log.Logger = new LoggerConfiguration()
            // add console as logging target
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            // set default minimum level
            .MinimumLevel.Debug()
            .CreateLogger();

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new SBCacheObjectForm());
    }

    public static ILogger logger = null!;
}