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
        
        [Fact]
        public void TextureArchive_Always_Returns_The_Same_Instance_Of_TextureArchive()
        {
            var texture1 = ArchiveLoader.TextureArchive;
            var texture2 = ArchiveLoader.TextureArchive;
            Assert.Equal(texture1.InstanceId, texture2.InstanceId);
        }

        [Fact]
        public void ObjectArchive_Always_Returns_The_Same_Instance_Of_ObjectArchive()
        {
            var object1 = ArchiveLoader.ObjectArchive;
            var object2 = ArchiveLoader.ObjectArchive;
            Assert.Equal(object1.InstanceId, object2.InstanceId);
        }

        [Fact]
        public void SoundArchive_Always_Returns_The_Same_Instance_Of_SoundArchive()
        {
            var sound1 = ArchiveLoader.SoundArchive;
            var sound2 = ArchiveLoader.SoundArchive;
            Assert.Equal(sound1.InstanceId, sound2.InstanceId);
        }

        [Fact]
        public void ZoneArchive_Always_Returns_The_Same_Instance_Of_ZoneArchive()
        {
            var zone1 = ArchiveLoader.ZoneArchive;
            var zone2 = ArchiveLoader.ZoneArchive;
            Assert.Equal(zone1.InstanceId, zone2.InstanceId);
        }

        [Fact]
        public void SkeletonArchive_Always_Returns_The_Same_Instance_Of_SkeletonArchive()
        {
            var skeleton1 = ArchiveLoader.SkeletonArchive;
            var skeleton2 = ArchiveLoader.SkeletonArchive;
            Assert.Equal(skeleton1.InstanceId, skeleton2.InstanceId);
        }

        [Fact]
        public void RenderArchive_Always_Returns_The_Same_Instance_Of_RenderArchive()
        {
            var render1 = ArchiveLoader.RenderArchive;
            var render2 = ArchiveLoader.RenderArchive;
            Assert.Equal(render1.InstanceId, render2.InstanceId);
        }

    }
}