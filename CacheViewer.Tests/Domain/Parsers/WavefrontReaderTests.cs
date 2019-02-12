namespace CacheViewer.Tests.Domain.Parsers
{
    using System;
    using System.IO;
    using CacheViewer.Domain.Parsers;
    using Xunit;

    public class WavefrontReaderTests
    {
        private int count;
        private readonly WavefrontReader wavefrontReader;

        public WavefrontReaderTests()
        {
            this.wavefrontReader = new WavefrontReader();
        }
        
        [Theory]
        [InlineData("361", 1586)]
        [InlineData("124441", 710)]
        [InlineData("124442", 652)]
        [InlineData("124483", 466)]
        [InlineData("124484", 515)]
        [InlineData("124669", 461)]
        [InlineData("142067", 692)]
        [InlineData("24002", 760)]
        [InlineData("24004", 760)]
        [InlineData("24102", 830)]
        [InlineData("24218", 559)]
        [InlineData("322044", 435)]
        [InlineData("322058", 435)]
        [InlineData("405056", 720)]
        [InlineData("405201", 740)]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_361(string id, int dataCount)
        {
            var file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_" + id + ".obj";
            WavefrontObject mayaObject;
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }

            Assert.NotNull(mayaObject);
            Assert.Equal(dataCount, mayaObject.Normals.Count);
            Assert.Equal(dataCount, mayaObject.Texcoords.Count);
            Assert.Equal(dataCount, mayaObject.Positions.Count);
        }

        [Fact]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_110168()
        {
            var file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_110168.obj";
            WavefrontObject mayaObject;
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }

            this.count = 522;

            Assert.NotNull(mayaObject);
            Assert.Equal(this.count, mayaObject.Normals.Count);
            Assert.Equal(this.count, mayaObject.Texcoords.Count);
            Assert.Equal(this.count, mayaObject.Positions.Count);
        }

        [Fact]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_110171()
        {
            var file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_110171.obj";
            WavefrontObject mayaObject;
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }

            this.count = 522;

            Assert.NotNull(mayaObject);
            Assert.Equal(this.count, mayaObject.Normals.Count);
            Assert.Equal(this.count, mayaObject.Texcoords.Count);
            Assert.Equal(this.count, mayaObject.Positions.Count);
        }

        [Fact]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_110181()
        {
            var file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_110181.obj";
            WavefrontObject mayaObject;
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }

            this.count = 522;

            Assert.NotNull(mayaObject);
            Assert.Equal(this.count, mayaObject.Normals.Count);
            Assert.Equal(this.count, mayaObject.Texcoords.Count);
            Assert.Equal(this.count, mayaObject.Positions.Count);
        }

        [Fact]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_110185()
        {
            var file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_110185.obj";
            WavefrontObject mayaObject;
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }

            this.count = 522;

            Assert.NotNull(mayaObject);
            Assert.Equal(this.count, mayaObject.Normals.Count);
            Assert.Equal(this.count, mayaObject.Texcoords.Count);
            Assert.Equal(this.count, mayaObject.Positions.Count);
        }

        [Fact]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_122901()
        {
            var file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_122901.obj";
            WavefrontObject mayaObject;
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }

            this.count = 829;

            Assert.NotNull(mayaObject);
            Assert.Equal(this.count, mayaObject.Normals.Count);
            Assert.Equal(this.count, mayaObject.Texcoords.Count);
            Assert.Equal(this.count, mayaObject.Positions.Count);
        }

        [Fact]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_123001()
        {
            var file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_123001.obj";
            WavefrontObject mayaObject;
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }

            this.count = 395;

            Assert.NotNull(mayaObject);
            Assert.Equal(this.count, mayaObject.Normals.Count);
            Assert.Equal(this.count, mayaObject.Texcoords.Count);
            Assert.Equal(this.count, mayaObject.Positions.Count);
        }

        [Fact]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_123101()
        {
            var file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_123101.obj";
            WavefrontObject mayaObject;
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }

            this.count = 511;

            Assert.NotNull(mayaObject);
            Assert.Equal(this.count, mayaObject.Normals.Count);
            Assert.Equal(this.count, mayaObject.Texcoords.Count);
            Assert.Equal(this.count, mayaObject.Positions.Count);
        }

        [Fact]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_123201()
        {
            var file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_123201.obj";
            WavefrontObject mayaObject;
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }

            this.count = 807;

            Assert.NotNull(mayaObject);
            Assert.Equal(this.count, mayaObject.Normals.Count);
            Assert.Equal(this.count, mayaObject.Texcoords.Count);
            Assert.Equal(this.count, mayaObject.Positions.Count);
        }

        [Fact]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_124006()
        {
            var file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_124006.obj";
            WavefrontObject mayaObject;
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }

            this.count = 747;

            Assert.NotNull(mayaObject);
            Assert.Equal(this.count, mayaObject.Normals.Count);
            Assert.Equal(this.count, mayaObject.Texcoords.Count);
            Assert.Equal(this.count, mayaObject.Positions.Count);
        }
    }
}