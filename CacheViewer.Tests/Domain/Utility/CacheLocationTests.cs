namespace CacheViewer.Tests.Domain.Utility
{
    using CacheViewer.Domain.Utility;
    using Moq;
    using Xunit;

    public class CacheLocationTests
    {
        private readonly Mock<IConfigurationWrapper> configurationWrapper;
        private readonly FileLocations fileLocations;

        public CacheLocationTests()
        {
            this.configurationWrapper = new Mock<IConfigurationWrapper>();
            this.fileLocations = new FileLocations(this.configurationWrapper.Object);
        }

        [Fact]
        public void GetCacheFolder_Should_Request_Key_Value_From_ConfigurationWrapper()
        {
            this.configurationWrapper.Setup(x => x.GetAppSetting("CacheFolder"))
                .Returns(@"C:\dev\CacheViewer\Cache\");

            this.fileLocations.GetCacheFolder();

            this.configurationWrapper.VerifyAll();
        }
    }
}