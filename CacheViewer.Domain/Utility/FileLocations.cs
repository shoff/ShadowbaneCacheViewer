namespace CacheViewer.Domain.Utility
{
    public class FileLocations
    {
        private readonly IConfigurationWrapper configurationWrapper;


        /// <summary>
        /// Initializes a new instance of the <see cref="FileLocations"/> class.
        /// </summary>
        /// <param name="configurationWrapper">The configuration wrapper.</param>
        public FileLocations(IConfigurationWrapper configurationWrapper)
        {
            this.configurationWrapper = configurationWrapper;
        }

        /// <summary>
        /// Gets the cache folder.
        /// </summary>
        /// <returns></returns>
        public string GetCacheFolder()
        {
            return this.configurationWrapper.GetAppSetting("CacheFolder");
        }

        /// <summary>
        /// Gets the export folder.
        /// </summary>
        /// <returns></returns>
        public string GetExportFolder()
        {
            return this.configurationWrapper.GetAppSetting("ExportFolder");
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static FileLocations Instance => new FileLocations(ConfigurationWrapper.Instance);
    }
}