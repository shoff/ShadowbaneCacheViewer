namespace CacheViewer.Domain.Utility
{
    using System;
    using System.Diagnostics.Contracts;

    public class FileLocations
    {
        private readonly IConfigurationWrapper configurationWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLocations"/> class.
        /// </summary>
        /// <param name="configurationWrapper">The configuration wrapper.</param>
        public FileLocations(IConfigurationWrapper configurationWrapper)
        {
            Contract.Requires<ArgumentNullException>(configurationWrapper != null);
            this.configurationWrapper = configurationWrapper;
        }

        /// <summary>
        /// Gets the cache folder.
        /// </summary>
        /// <returns></returns>
        [Pure]
        public string GetCacheFolder()
        {
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
            return this.configurationWrapper.GetAppSetting("CacheFolder");
        }

        /// <summary>
        /// Gets the export folder.
        /// </summary>
        /// <returns></returns>
        [Pure]
        public string GetExportFolder()
        {
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
            return this.configurationWrapper.GetAppSetting("ExportFolder");
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static FileLocations Instance
        {
            get
            {
                Contract.Ensures(Contract.Result<FileLocations>() != null);
                return new FileLocations(ConfigurationWrapper.Instance);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.configurationWrapper != null);
            Contract.Invariant(this.GetExportFolder() != null);
            Contract.Invariant(this.GetCacheFolder() != null);
            Contract.Invariant(Instance != null);
        }
    }
}