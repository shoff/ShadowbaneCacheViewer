namespace CacheViewer.Tests.Domain.Factories
{
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Factories;
    using Xunit;

    public class ArchiveFactoryTests
    {
        private readonly ArchiveFactory archiveFactory;

        public ArchiveFactoryTests()
        {
            this.archiveFactory = new ArchiveFactory();
        }
        
        [Fact]
        public void Build_CObjects_Should_Return_CObjects_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.CObjects);
            Assert.Equal("CObjects.cache", actual.Name);
        }

        [Fact]
        public void Build_CZone_Should_Return_CZone_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.CZone);
            Assert.Equal("CZone.cache", actual.Name);
        }

        [Fact]
        public void Build_Dungeon_Should_Return_Dungeon_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Dungeon);
            Assert.Equal("Dungeon.cache", actual.Name);
        }

        [Fact]
        public void Build_Mesh_Should_Return_Mesh_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Mesh);
            Assert.Equal("Mesh.cache", actual.Name);
        }

        [Fact]
        public void Build_Motion_Should_Return_Motion_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Motion);
            Assert.Equal("Motion.cache", actual.Name);
        }

        [Fact]
        public void Build_Palette_Should_Return_Palette_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Palette);
            Assert.Equal("Palette.cache", actual.Name);
        }

        [Fact]
        public void Build_Render_Should_Return_Render_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Render);
            Assert.Equal("Render.cache", actual.Name);
        }

        [Fact]
        public void Build_Skeleton_Should_Return_Skeleton_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Skeleton);
            Assert.Equal("Skeleton.cache", actual.Name);
        }

        [Fact]
        public void Build_Sound_Should_Return_Sound_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Sound);
            Assert.Equal("Sound.cache", actual.Name);
        }

        [Fact]
        public void Build_TerrainAlpha_Should_Return_TerrainAlpha_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.TerrainAlpha);
            Assert.Equal("TerrainAlpha.cache", actual.Name);
        }

        [Fact]
        public void Build_Textures_Should_Return_Textures_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Textures);
            Assert.Equal("Textures.cache", actual.Name);
        }

        [Fact]
        public void Build_Tile_Should_Return_Tile_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Tile);
            Assert.Equal("Tile.cache", actual.Name);
        }

        [Fact(Skip = "Still need to create a cache maker")]
        public void Build_Unknown_Should_Return_Unknown_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Unknown);
            Assert.Equal("Unknown.cache", actual.Name);
        }

        [Fact]
        public void Build_Visual_Should_Return_Visual_Archive()
        {
            var actual = this.archiveFactory.Build(CacheFile.Visual);
            Assert.Equal("Visual.cache", actual.Name);
        }
    }
}