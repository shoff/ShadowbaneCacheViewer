namespace Shadowbane.Cache.IO.Tests
{
    using IO;
    using Xunit;

    public class ArchiveLoaderTests : CacheLoaderBaseTest
    {

        [Fact]
        public void MeshArchive_Always_Returns_The_Same_Instance_Of_MeshArchive()
        {
            var mesh1 = ArchiveLoader.MeshArchive;
            var mesh2 = ArchiveLoader.MeshArchive;
            Assert.Equal(mesh1.InstanceId, mesh2.InstanceId);
        }
        
    }
}