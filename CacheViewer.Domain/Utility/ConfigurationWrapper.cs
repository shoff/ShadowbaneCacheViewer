using System.Collections.Specialized;
using System.Configuration;

namespace CacheViewer.Domain.Utility
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Allows testing of classes that make use of AppSettings and ConnectionStrings 
    /// Sections of app.config and web.config files.
    /// </summary>
    public class ConfigurationWrapper : IConfigurationWrapper
    {
        /// <summary>
        /// </summary>
        /// <exception cref="ConfigurationErrorsException">Could not retrieve a 
        /// <see cref="T:System.Collections.Specialized.NameValueCollection" /> object with the application settings data.</exception>
        /// Summary:
        /// Gets the System.Configuration.AppSettingsSection data for the current application's
        /// default configuration.
        /// Returns:
        /// Returns a System.Collections.Specialized.NameValueCollection object that
        /// contains the contents of the System.Configuration.AppSettingsSection object
        /// for the current application's default configuration.
        /// Exceptions:
        /// System.Configuration.ConfigurationErrorsException:
        /// Could not retrieve a System.Collections.Specialized.NameValueCollection object
        /// with the application settings data.
        public NameValueCollection AppSettings
        {
            get
            {
                Contract.Assume(ConfigurationManager.AppSettings != null);
                return ConfigurationManager.AppSettings;
            }
        }

        /// <summary>
        /// Gets the System.Configuration.ConnectionStringsSection data for the current
        /// application's default configuration.
        /// Returns:
        /// Returns a System.Configuration.ConnectionStringSettingsCollection object
        /// that contains the contents of the System.Configuration.ConnectionStringsSection
        /// object for the current application's default configuration.
        /// </summary>       
        /// <exception cref="ConfigurationErrorsException" accessor="get">Could not retrieve a 
        /// <see cref="T:System.Configuration.ConnectionStringSettingsCollection" /> object.</exception>
        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                Contract.Assume(ConfigurationManager.ConnectionStrings != null);
                return ConfigurationManager.ConnectionStrings;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        /// Summary:
        /// Retrieves a specified configuration section for the current application's
        /// default configuration.
        /// Parameters:
        /// sectionName:
        /// The configuration section path and name.
        /// Returns:
        /// The specified System.Configuration.ConfigurationSection object, or null if
        /// the section does not exist.
        /// Exceptions:
        /// System.Configuration.ConfigurationErrorsException:
        /// A configuration file could not be loaded.
        public object GetSection(string sectionName)
        {
            Contract.Requires<ArgumentNullException>(sectionName != null);
            Contract.Ensures(Contract.Result<object>() != null);
            return ConfigurationManager.GetSection(sectionName);
        }

        /// <summary>
        /// </summary>
        /// <param name="userLevel"></param>
        /// <returns></returns>
        /// Summary:
        /// Opens the configuration file for the current application as a System.Configuration.Configuration
        /// object.
        /// Parameters:
        /// userLevel:
        /// The System.Configuration.ConfigurationUserLevel for which you are opening
        /// the configuration.
        /// Returns:
        /// A System.Configuration.Configuration object.
        /// Exceptions:
        /// System.Configuration.ConfigurationErrorsException:
        /// A configuration file could not be loaded.
        public Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel)
        {
            Contract.Ensures(Contract.Result<Configuration>() != null);
            return ConfigurationManager.OpenExeConfiguration(userLevel);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// Summary:
        /// Opens the machine configuration file on the current computer as a System.Configuration.Configuration
        /// object.
        /// Returns:
        /// A System.Configuration.Configuration object.
        /// Exceptions:
        /// System.Configuration.ConfigurationErrorsException:
        /// A configuration file could not be loaded.
        public Configuration OpenMachineConfiguration()
        {
            Contract.Ensures(Contract.Result<Configuration>() != null);
            return ConfigurationManager.OpenMachineConfiguration();
        }

        /// <summary>
        /// </summary>
        /// <param name="sectionName"></param>
        /// Summary:
        /// Refreshes the named section so the next time that it is retrieved it will
        /// be re-read from disk.
        /// Parameters:
        /// sectionName:
        /// The configuration section name or the configuration path and section name
        /// of the section to refresh.
        public void RefreshSection(string sectionName)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(sectionName));
            ConfigurationManager.RefreshSection(sectionName);
        }

        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException">Could not retrieve a 
        /// <see cref="T:System.Collections.Specialized.NameValueCollection" /> object with the application settings data.</exception>
        [Pure]
        public string GetAppSetting(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
            Contract.Assume(this.AppSettings != null);
            return AppSettings[name];
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static ConfigurationWrapper Instance
        {
            get
            {
                Contract.Ensures(Contract.Result<ConfigurationWrapper>() != null);
                return new ConfigurationWrapper();
            }
        }
    }
}