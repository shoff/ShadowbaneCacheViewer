using CacheViewer.Domain.Factories;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Models;
    using CacheViewer.Domain.Models.Exportable;
    using Newtonsoft.Json;

    [TestFixture]
    public class CacheObjectFactoryTests
    {
        // this breaks testing in isolation, but the alternative is to start passing this as an interface
        // which would impact performance HUGELY so ...
        private readonly CacheObjectsCache cacheObjectsCache = CacheObjectsCache.Instance;

        [Test, Explicit]
        public async Task Temp_OutPut_All_To_Files()
        {
            var folder = CreateFolders();

            foreach (var index in this.cacheObjectsCache.CacheObjects.CacheIndices)
            {
                var cobject = this.cacheObjectsCache.Create(index);
                var saveFolder = this.GetFolderName(folder, cobject);
                await this.cacheObjectsCache.CacheObjects.SaveToFile(index, saveFolder);
            }
        }

        [Test, Explicit]
        public void Temp_OutPut_All_To_Json()
        {
            var folder = CreateFolders();

            foreach (var index in this.cacheObjectsCache.CacheObjects.CacheIndices)
            {
                var cobject = this.cacheObjectsCache.Create(index);
                var cobjectJson = JsonConvert.SerializeObject(cobject);
                string fileName = this.GetFileName(folder, cobject, index.Identity);
                File.WriteAllText(fileName, cobjectJson);
            }
        }

        private string GetFolderName(string folder, ICacheObject cacheObject)
        {
            return $"{folder}\\{objectTypeDicitonary[cacheObject.Flag]}";
        }

        private string GetFileName(string folder, ICacheObject cobject, int indexIdentity)
        {
            var objectName = string.Join("_", $"{cobject.Name}".Split(Path.GetInvalidFileNameChars()));
            return $"{folder}\\{objectTypeDicitonary[cobject.Flag]}\\{objectName}_{indexIdentity}.json";
            // $"{folder}\\{objectTypeDicitonary[cobject.Flag]}\\{cobject.Name}_{indexIdentity}.json";
        }

        private static readonly Dictionary<ObjectType, string> objectTypeDicitonary = new Dictionary<ObjectType, string>
        {
            {ObjectType.Sun, "Sun"},
            {ObjectType.Simple, "Simple"},
            {ObjectType.Structure, "Structure"},
            {ObjectType.Interactive, "Interactive"},
            {ObjectType.Equipment, "Equipment"},
            {ObjectType.Mobile, "Mobile"},
            {ObjectType.Deed, "Deed"},
            {ObjectType.Warrant, "Warrant"},
            {ObjectType.Unknown, "Unknown"},
            {ObjectType.Particle, "Particle"}
        };

        private static string CreateFolders()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory + "CacheObjectIndexes";
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
            Directory.CreateDirectory(folder);
            Directory.CreateDirectory(folder + "\\Sun");
            Directory.CreateDirectory(folder + "\\Simple");
            Directory.CreateDirectory(folder + "\\Structure");
            Directory.CreateDirectory(folder + "\\Interactive");
            Directory.CreateDirectory(folder + "\\Equipment");
            Directory.CreateDirectory(folder + "\\Mobile");
            Directory.CreateDirectory(folder + "\\Deed");
            Directory.CreateDirectory(folder + "\\Warrant");
            Directory.CreateDirectory(folder + "\\Unknown");
            Directory.CreateDirectory(folder + "\\Particle");

            return folder;
        }

        [Test]
        public void Do_ANY_FUCKING_Type_4_Work()
        {
            //var indexs = (from i in this.cacheObjectsCache.Indexes
            //    where i.Flag == 4
            //    select i).ToArray();

            Dictionary<int, bool> hasValidMesh = new Dictionary<int, bool>();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Name,RenderId,InventoryTextureId,Unknown,MapTex,NumberOfMeshes,UnParsedBytes");
            foreach (var i in this.cacheObjectsCache.Indexes)
            {
                var cobject = this.cacheObjectsCache.CreateAndParse(i);
                if (cobject.Flag == ObjectType.Structure)
                {
                    Structure structure = (Structure) cobject;
                    sb.AppendLine($"{cobject.Name},{structure.RenderId},{structure.InventoryTextureId},{structure.IUnk},{structure.MapTex},{structure.NumberOfMeshes},{structure.UnParsedBytes}");
                }
            }
            var folder = AppDomain.CurrentDomain.BaseDirectory + "CacheObjectIndexes";
            File.WriteAllText($"{folder}\\invalidmeshes.csv", sb.ToString());
        }

        [Test]
        public void The_First_CacheIndex_Is_The_Sun()
        {
            var cacheIndex = this.cacheObjectsCache.Indexes.FirstOrDefault();
            var sun = this.cacheObjectsCache.CreateAndParse(cacheIndex);
            Assert.AreEqual(ObjectType.Sun, sun.Flag);
        }

        [Test]
        public void Centaur_Concave_Tower_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectsCache.CacheObjects[585000];
            var centaurConcaveTower = this.cacheObjectsCache.CreateAndParse(cacheIndex.CacheIndex1);
            Assert.AreEqual(ObjectType.Structure, centaurConcaveTower.Flag);
        }

        [Test]
        public void BeastMan_Hut_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectsCache.CacheObjects[484032];
            var beastmanHut = this.cacheObjectsCache.CreateAndParse(cacheIndex.CacheIndex1);
            Assert.AreEqual(ObjectType.Structure, beastmanHut.Flag);
        }


        [Test]
        public void Create_Simpler_Returns_A_Simple_CacheObject()
        {
            var cacheIndex = this.cacheObjectsCache.Indexes.First(x => x.Identity == 103);
            var cacheObject = this.cacheObjectsCache.CreateAndParse(cacheIndex);
            Assert.AreEqual(ObjectType.Simple, cacheObject.Flag);
        }

    }
}