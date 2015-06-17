using System;
using System.IO;
using CacheViewer.Domain.Parsers;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Parsers
{
    [TestFixture]
    public class WavefrontReaderTests
    {
        private int count;
        private WavefrontReader wavefrontReader;

        [SetUp]
        public void SetUp()
        {
            this.wavefrontReader = new WavefrontReader();
        }
        
        [Test]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_110168()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_110168.obj";
            WavefrontObject mayaObject;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }
            this.count = 522;

            Assert.IsNotNull(mayaObject);
            Assert.AreEqual(count, mayaObject.Normals.Count);
            Assert.AreEqual(count, mayaObject.Texcoords.Count);
            Assert.AreEqual(count, mayaObject.Positions.Count);
        }

        [Test]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_110171()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_110171.obj";
            WavefrontObject mayaObject;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }
            this.count = 522;

            Assert.IsNotNull(mayaObject);
            Assert.AreEqual(count, mayaObject.Normals.Count);
            Assert.AreEqual(count, mayaObject.Texcoords.Count);
            Assert.AreEqual(count, mayaObject.Positions.Count);
        }

        [Test]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_110181()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_110181.obj";
            WavefrontObject mayaObject;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }
            this.count = 522;

            Assert.IsNotNull(mayaObject);
            Assert.AreEqual(count, mayaObject.Normals.Count);
            Assert.AreEqual(count, mayaObject.Texcoords.Count);
            Assert.AreEqual(count, mayaObject.Positions.Count);
        }

        [Test]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_110185()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_110185.obj";
            WavefrontObject mayaObject;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }
            this.count = 522;

            Assert.IsNotNull(mayaObject);
            Assert.AreEqual(count, mayaObject.Normals.Count);
            Assert.AreEqual(count, mayaObject.Texcoords.Count);
            Assert.AreEqual(count, mayaObject.Positions.Count);
        }

        [Test]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_122901()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_122901.obj";
            WavefrontObject mayaObject;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }
            this.count = 829;

            Assert.IsNotNull(mayaObject);
            Assert.AreEqual(count, mayaObject.Normals.Count);
            Assert.AreEqual(count, mayaObject.Texcoords.Count);
            Assert.AreEqual(count, mayaObject.Positions.Count);
        }

        [Test]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_123101()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_123101.obj";
            WavefrontObject mayaObject;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }
            this.count = 511;

            Assert.IsNotNull(mayaObject);
            Assert.AreEqual(count, mayaObject.Normals.Count);
            Assert.AreEqual(count, mayaObject.Texcoords.Count);
            Assert.AreEqual(count, mayaObject.Positions.Count);
        }

        [Test]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_123001()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_123001.obj";
            WavefrontObject mayaObject;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }
            this.count = 395;

            Assert.IsNotNull(mayaObject);
            Assert.AreEqual(count, mayaObject.Normals.Count);
            Assert.AreEqual(count, mayaObject.Texcoords.Count);
            Assert.AreEqual(count, mayaObject.Positions.Count);
        }

        [Test]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_123201()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_123201.obj";
            WavefrontObject mayaObject;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }
            this.count = 807;

            Assert.IsNotNull(mayaObject);
            Assert.AreEqual(count, mayaObject.Normals.Count);
            Assert.AreEqual(count, mayaObject.Texcoords.Count);
            Assert.AreEqual(count, mayaObject.Positions.Count);
        }

        [Test]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_124006()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_124006.obj";
            WavefrontObject mayaObject;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }
            this.count = 747;

            Assert.IsNotNull(mayaObject);
            Assert.AreEqual(count, mayaObject.Normals.Count);
            Assert.AreEqual(count, mayaObject.Texcoords.Count);
            Assert.AreEqual(count, mayaObject.Positions.Count);
        }

        [TestCase(new object[] { "361", 1586 })]
        [TestCase(new object[] { "124441", 710 })]
        [TestCase(new object[] { "124442", 652 })]
        [TestCase(new object[] { "124483", 466 })]
        [TestCase(new object[] { "124484", 515 })]
        [TestCase(new object[] { "124669", 461 })]
        [TestCase(new object[] { "142067", 692 })]
        [TestCase(new object[] { "24002", 760 })]
        [TestCase(new object[] { "24004", 760 })]
        [TestCase(new object[] { "24102", 830 })]
        [TestCase(new object[] { "24218", 559 })]
        [TestCase(new object[] { "322044", 435 })]
        [TestCase(new object[] { "322058", 435 })]
        [TestCase(new object[] { "405056", 720 })]
        [TestCase(new object[] { "405201", 740 })]
        public void Read_Should_Return_Valid_WavefrontObject_For_Mesh_361(string id, int count)
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\WavefrontFiles\\Mesh_"+id+".obj";
            WavefrontObject mayaObject;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mayaObject = this.wavefrontReader.Read(fs);
            }

            Assert.IsNotNull(mayaObject);
            Assert.AreEqual(count, mayaObject.Normals.Count);
            Assert.AreEqual(count, mayaObject.Texcoords.Count);
            Assert.AreEqual(count, mayaObject.Positions.Count);
        }
    }
}