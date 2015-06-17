using CacheViewer.Domain.Archive;
using CacheViewer.Domain.Factories;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Factories
{
    [TestFixture]
    public class ArchiveFactoryTests
    {
        private ArchiveFactory archiveFactory;

        [SetUp]
        public void SetUp()
        {
            this.archiveFactory = new ArchiveFactory();
        }

        [Test]
        public void Build_CObjects_Should_Return_CObjects_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.CObjects);
            Assert.AreEqual(actual.Name, "CObjects.cache");
        }

        [Test]
        public void Build_CZone_Should_Return_CZone_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.CZone);
            Assert.AreEqual(actual.Name, "CZone.cache");
        }
        
        [Test]
        public void Build_Render_Should_Return_Render_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Render);
            Assert.AreEqual(actual.Name, "Render.cache");
        }
        
        [Test]
        public void Build_Dungeon_Should_Return_Dungeon_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Dungeon);
            Assert.AreEqual(actual.Name, "Dungeon.cache");
        }

        [Test]
        public void Build_Mesh_Should_Return_Mesh_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Mesh);
            Assert.AreEqual(actual.Name, "Mesh.cache");
        }
        
        [Test]
        public void Build_Motion_Should_Return_Motion_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Motion);
            Assert.AreEqual(actual.Name, "Motion.cache");
        }
        
        [Test]
        public void Build_Palette_Should_Return_Palette_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Palette);
            Assert.AreEqual(actual.Name, "Palette.cache");
        }
        
        [Test]
        public void Build_Skeleton_Should_Return_Skeleton_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Skeleton);
            Assert.AreEqual(actual.Name, "Skeleton.cache");
        }
       
        [Test]
        public void Build_Sound_Should_Return_Sound_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Sound);
            Assert.AreEqual(actual.Name, "Sound.cache");
        }

        [Test]
        public void Build_TerrainAlpha_Should_Return_TerrainAlpha_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.TerrainAlpha);
            Assert.AreEqual(actual.Name, "TerrainAlpha.cache");
        }
        
        [Test]
        public void Build_Textures_Should_Return_Textures_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Textures);
            Assert.AreEqual(actual.Name, "Textures.cache");
        }
        
        [Test]
        public void Build_Tile_Should_Return_Tile_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Tile);
            Assert.AreEqual(actual.Name, "Tile.cache");
        }
        
        [Test]
        public void Build_Visual_Should_Return_Visual_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Visual);
            Assert.AreEqual(actual.Name, "Visual.cache");
        }
        
        [Test]
        public void Build_Unknown_Should_Return_Unknown_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Unknown);
            Assert.AreEqual(actual.Name, "Unknown.cache");
        }
    }
}