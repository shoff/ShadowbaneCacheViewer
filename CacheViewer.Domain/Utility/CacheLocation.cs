namespace CacheViewer.Domain.Utility
{
    public class CacheLocation
    {
        private readonly IConfigurationWrapper configurationWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheLocation"/> class.
        /// </summary>
        /// <param name="configurationWrapper">The configuration wrapper.</param>
        public CacheLocation(IConfigurationWrapper configurationWrapper)
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
    }
}