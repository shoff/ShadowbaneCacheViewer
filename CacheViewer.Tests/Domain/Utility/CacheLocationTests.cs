using CacheViewer.Domain.Utility;
using Moq;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Utility
{
    [TestFixture]
    public class CacheLocationTests
    {
        private Mock<IConfigurationWrapper> configurationWrapper;
        private FileLocations fileLocations;

        [SetUp]
        public void SetUp()
        {
            this.configurationWrapper = new Mock<IConfigurationWrapper>();
            this.fileLocations = new FileLocations(this.configurationWrapper.Object);
        }

        [Test]
        public void GetCacheFolder_Should_Request_Key_Value_From_ConfigurationWrapper()
        {
            this.configurationWrapper.Setup(x => x.GetAppSetting("CacheFolder"))
                .Returns(@"C:\dev\CacheViewer\Cache\");

            this.fileLocations.GetCacheFolder();

            this.configurationWrapper.VerifyAll();
        }


    }
}