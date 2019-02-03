using CacheViewer.Domain.Factories;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Models;
    using CacheViewer.Domain.Models.Exportable;
    using Newtonsoft.Json;

    [TestFixture]
    public class CacheObjectFactoryTests
    {
        private readonly CacheObjectFactory cacheObjectFactory = CacheObjectFactory.Instance;

        [Test, Explicit]
        public async Task Temp_OutPut_All_To_Files()
        {
            var folder = CreateFolders();

            foreach (var index in this.cacheObjectFactory.CacheObjects.CacheIndices)
            {
                var cobject = this.cacheObjectFactory.Create(index);
                var saveFolder = this.GetFolderName(folder, cobject);
                await this.cacheObjectFactory.CacheObjects.SaveToFileAsync(index, saveFolder);
            }
        }

        [Test, Explicit]
        public void Temp_OutPut_All_To_Json()
        {
            var folder = CreateFolders();

            foreach (var index in this.cacheObjectFactory.CacheObjects.CacheIndices)
            {
                var cobject = this.cacheObjectFactory.Create(index);
                var cobjectJson = JsonConvert.SerializeObject(cobject);
                string fileName = this.GetFileName(folder, cobject, index.Identity);
                File.WriteAllText(fileName, cobjectJson);
            }
        }

        private string GetFolderName(string folder, ICacheObject cacheObject)
        {
            return $"{folder}\\{objectTypeDictionary[cacheObject.Flag]}";
        }

        private string GetFileName(string folder, ICacheObject cobject, int indexIdentity)
        {
            var objectName = string.Join("_", $"{cobject.Name}".Split(Path.GetInvalidFileNameChars()));
            return $"{folder}\\{objectTypeDictionary[cobject.Flag]}\\{objectName}_{indexIdentity}.json";
            // $"{folder}\\{objectTypeDictionary[cobject.Flag]}\\{cobject.Name}_{indexIdentity}.json";
        }

        private static readonly Dictionary<ObjectType, string> objectTypeDictionary = new Dictionary<ObjectType, string>
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

        [Test, Explicit]
        public void Save_RenderInformation_To_Sql()
        {
            RenderInformationFactory renderInformationFactory = RenderInformationFactory.Instance;

            using (var context = new SbCacheViewerContext())
            {
                var save = 0;
                foreach (var index in renderInformationFactory.RenderArchive.CacheIndices)
                {
                    save++;
                    // await this.renderInformationFactory.RenderArchive.SaveToFileAsync(index, folder);
                    var render = renderInformationFactory.Create(index.Identity, index.Order, true);
                    var entity = new RenderEntity
                    {
                        ByteCount = render.ByteCount,
                        CacheIndexIdentity = render.CacheIndex.Identity,
                        CompressedSize = (int)render.CacheIndex.CompressedSize,
                        FileOffset = (int)render.CacheIndex.Offset,
                        HasMesh = render.HasMesh,
                        JointName = render.JointName,
                        HasTexture = render.HasTexture,
                        RenderCount = render.ChildCount,
                        MeshId = render.MeshId,
                        Order = render.Order,
                        Position = $"{render.Position.X}-{render.Position.Y}-{render.Position.Z}",
                        // Textures = render.Textures,
                        // TODO doesn't handle textures
                        UncompressedSize = (int)render.CacheIndex.UnCompressedSize
                    };
                    context.RenderEntities.Add(entity);

                    if (save == 1000)
                    {
                        save = 0;
                        context.SaveChanges();
                    }
                }

                context.SaveChanges();
            }
        }

        [Test, Explicit]
        public void Save_All_Cache_Files_To_Sql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Name,CacheId,RenderIds Found,");
            var folder = AppDomain.CurrentDomain.BaseDirectory + "CacheObjectIndexes";
            var totalCacheItems = 0;

            using (var context = new SbCacheViewerContext())
            {
                var save = 0;
                foreach (var i in this.cacheObjectFactory.Indexes)
                {
                    save++;
                    totalCacheItems++;

                    var cobject = this.cacheObjectFactory.CreateAndParse(i);

                    var centity = (from c in context.CacheObjectEntities
                                   where c.CacheIndexIdentity == cobject.CacheIndex.Identity
                                   select c).AsNoTracking().FirstOrDefault();

                    if (centity == null)
                    {
                        centity = new CacheObjectEntity
                        {
                            CacheIndexIdentity = cobject.CacheIndex.Identity,
                            CompressedSize = (int)cobject.CacheIndex.CompressedSize,
                            FileOffset = (int)cobject.CacheIndex.Offset,
                            Name = cobject.Name,
                            ObjectType = cobject.Flag,
                            ObjectTypeDescription = objectTypeDictionary[cobject.Flag],
                            UncompressedSize = (int)cobject.CacheIndex.UnCompressedSize

                        };
                        context.CacheObjectEntities.Add(centity);
                    }

                    Dictionary<long, int> ids = new Dictionary<long, int>();
                    var structure = cobject;
                    using (var reader = structure.Data.CreateBinaryReaderUtf32())
                    {
                        reader.BaseStream.Position = 57; // this is common to all cache files and doesn't contain any render ids

                        while (reader.BaseStream.Position + 4 <= structure.Data.Count)
                        {
                            int renderId = reader.ReadInt32();

                            int range = renderId > centity.CacheIndexIdentity ?
                                Math.Abs(renderId - centity.CacheIndexIdentity) :
                                Math.Abs(centity.CacheIndexIdentity - renderId);

                            if (range < 5000 && Array.IndexOf(RenderInformationFactory.Instance.RenderArchive.IdentityArray, renderId) > -1)
                            {
                                ids.Add(reader.BaseStream.Position, renderId);
                                centity.RenderAndOffsets.Add(new RenderAndOffset
                                {
                                    RenderId = renderId,
                                    OffSet = reader.BaseStream.Position
                                });
                            }
                            reader.BaseStream.Position -= 3;
                        }
                    }

                    sb.Append($"{structure.Name},{i.Identity},{ids.Count}");
                    ids.Each(x => sb.Append($",{x.Key},{x.Value}"));
                    sb.Append(Environment.NewLine);

                    if (save == 1000)
                    {
                        context.SaveChanges();
                        File.AppendAllText($"{folder}\\cache-render-ids.csv", sb.ToString());
                        sb.Clear();
                        save = 0;
                    }
                }

                context.SaveChanges();
                sb.AppendLine($"TotalCacheItems,{totalCacheItems}");
                File.AppendAllText($"{folder}\\cache-render-ids.csv", sb.ToString());
            }
        }

        //[TestCase(585000)]
        //[TestCase(300)]
        //[TestCase(380)]
        //[TestCase(44000)]
        //[TestCase(64000)]
        //[TestCase(64014)]
        //[TestCase(64035)]
        //[TestCase(64072)]
        //[TestCase(64124)]
        //[TestCase(64148)]
        //[TestCase(64160)]
        //[TestCase(64500)]
        //[TestCase(64600)]
        //[TestCase(64700)]
        //[TestCase(64800)]
        //[TestCase(124000)]
        //[TestCase(124300)]
        //[TestCase(144000)]
        //[TestCase(144063)]
        //[TestCase(144109)]
        //[TestCase(144500)]
        //[TestCase(162014)]
        //[TestCase(162025)]
        //[TestCase(162035)]
        //[TestCase(402001)]
        //[TestCase(402004)]
        //[TestCase(402011)]
        //[TestCase(404016)]
        //[TestCase(404031)]
        //[TestCase(404048)]
        //[TestCase(422600)]
        //[TestCase(422700)]
        //[TestCase(423300)]
        //[TestCase(423400)]
        //[TestCase(423500)]
        //[TestCase(423600)]
        //[TestCase(424000)]
        //[TestCase(424082)]
        //[TestCase(424128)]
        //[TestCase(424180)]
        //[TestCase(424285)]
        //[TestCase(424367)]
        //[TestCase(424484)]
        //[TestCase(424554)]
        //[TestCase(424581)]
        //[TestCase(460126)]
        //[TestCase(460152)]
        //[TestCase(460173)]
        //[TestCase(460189)]
        //[TestCase(460314)]
        //[TestCase(460338)]
        //[TestCase(460374)]
        //[TestCase(460600)]
        //[TestCase(460610)]
        //[TestCase(460620)]
        //[TestCase(460700)]
        //[TestCase(460861)]
        //[TestCase(460886)]
        //[TestCase(460900)]
        //[TestCase(460999)]
        //[TestCase(461000)]
        //[TestCase(461800)]
        //[TestCase(461900)]
        //[TestCase(462000)]
        //[TestCase(462100)]
        //[TestCase(462200)]
        //[TestCase(462500)]
        //[TestCase(482128)]
        //[TestCase(482136)]
        //[TestCase(482144)]
        //[TestCase(484000)]
        //[TestCase(484016)]
        //[TestCase(484032)]
        //[TestCase(484100)]
        //[TestCase(484120)]
        //[TestCase(484140)]
        //[TestCase(484160)]
        //[TestCase(484180)]
        //[TestCase(484200)]
        //[TestCase(484220)]
        //[TestCase(484240)]
        //[TestCase(484260)]
        //[TestCase(484280)]
        //[TestCase(484300)]
        //[TestCase(484320)]
        //[TestCase(484500)]
        //[TestCase(564000)]
        //[TestCase(564100)]
        //[TestCase(564200)]
        //[TestCase(564300)]
        //[TestCase(564400)]
        //[TestCase(564500)]
        //[TestCase(564600)]
        //[TestCase(585000)]
        //[TestCase(585200)]
        //[TestCase(585400)]
        //[TestCase(585600)]
        //[TestCase(585800)]
        //[TestCase(586000)]
        //[TestCase(586400)]
        //[TestCase(586600)]
        //[TestCase(622670)]
        //[TestCase(622718)]
        //[TestCase(622770)]
        //[TestCase(804000)]
        //[TestCase(814000)]
        //[TestCase(1500000)]
        //[TestCase(1600500)]
        //[TestCase(1610000)]
        //[TestCase(1612900)]
        //[TestCase(5000400)]
        //[TestCase(5000700)]
        //[TestCase(5000800)]
        //[TestCase(5001100)]
        //[TestCase(5001500)]
        //[TestCase(5010000)]
        //[TestCase(5010100)]
        //[TestCase(5010200)]
        //[TestCase(5031000)]
        //[TestCase(5031200)]
        //[TestCase(5031400)]
        //[TestCase(5050000)]
        //public async Task Update_One_Cache_File_In_Sql(int cacheIndexId)
        //{
        //    using (var context = new SbCacheViewerContext())
        //    {
        //        var cacheIndex = this.cacheObjectFactory.FindById(cacheIndexId);
        //        await this.cacheObjectFactory.SaveToFileAsync(cacheIndex, AppDomain.CurrentDomain.BaseDirectory + "\\CObjects");

        //        var cobject = this.cacheObjectFactory.CreateAndParse(cacheIndex);

        //        var centity = (from c in context.CacheObjectEntities
        //                       where c.CacheIndexIdentity == cacheIndexId
        //                       select c).FirstOrDefault();

        //        if (centity == null)
        //        {
        //            centity = new CacheObjectEntity
        //            {
        //                CacheIndexIdentity = cobject.CacheIndex.Identity,
        //                CompressedSize = (int)cobject.CacheIndex.CompressedSize,
        //                FileOffset = (int)cobject.CacheIndex.Offset,
        //                Name = cobject.Name,
        //                ObjectType = cobject.Flag,
        //                ObjectTypeDescription = objectTypeDictionary[cobject.Flag],
        //                UncompressedSize = (int)cobject.CacheIndex.UnCompressedSize

        //            };
        //            context.CacheObjectEntities.Add(centity);
        //        }

        //        var structure = cobject;
        //        using (var reader = structure.Data.CreateBinaryReaderUtf32())
        //        {
        //            reader.BaseStream.Position = 57; // this is common to all cache files and doesn't contain any render ids

        //            while (reader.BaseStream.Position + 4 <= structure.Data.Count)
        //            {
        //                int renderId = reader.ReadInt32();

        //                if (renderId == 0)
        //                {
        //                    continue;
        //                }

        //                int range = renderId > centity.CacheIndexIdentity ?
        //                    Math.Abs(renderId - centity.CacheIndexIdentity) :
        //                    Math.Abs(centity.CacheIndexIdentity - renderId);

        //                if (range < 5000 && Array.IndexOf(RenderInformationFactory.Instance.RenderArchive.IdentityArray, renderId) > -1)
        //                {
        //                    centity.RenderAndOffsets.Add(new RenderAndOffset
        //                    {
        //                        RenderId = renderId,
        //                        OffSet = reader.BaseStream.Position,
        //                        CacheIndexId = cacheIndexId
        //                    });
        //                }
        //                reader.BaseStream.Position -= 3;
        //            }
        //        }
        //        context.SaveChanges();
        //    }
        //}

        [Test]
        public void Centaur_2004_Has_Correct_Number_Of_RenderIds()
        {
            using (var context = new SbCacheViewerContext())
            {
                var entity = (from c in context.CacheObjectEntities.Include(r => r.RenderEntities)
                    where c.CacheIndexIdentity == 2004 select c).First();

                Assert.AreEqual(33, entity.RenderEntities.Count);
            }
        }

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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Name,RenderId,InventoryTextureId,Unknown,MapTex,NumberOfMeshes,UnParsedBytes");
            foreach (var i in this.cacheObjectFactory.Indexes)
            {
                var cobject = this.cacheObjectFactory.CreateAndParse(i);
                if (cobject.Flag == ObjectType.Structure)
                {
                    Structure structure = (Structure)cobject;
                    sb.AppendLine($"{cobject.Name},{structure.RenderId},{structure.InventoryTextureId},{structure.IUnk},{structure.MapTex},{structure.NumberOfMeshes},{structure.UnParsedBytes}");
                }
            }
            var folder = AppDomain.CurrentDomain.BaseDirectory + "CacheObjectIndexes";
            File.WriteAllText($"{folder}\\invalidmeshes.csv", sb.ToString());
        }

        [Test]
        public void The_First_CacheIndex_Is_The_Sun()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault();
            var sun = this.cacheObjectFactory.CreateAndParse(cacheIndex);
            Assert.AreEqual(ObjectType.Sun, sun.Flag);
        }

        [Test]
        public void Centaur_Concave_Tower_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.CacheObjects[585000];
            var centaurConcaveTower = this.cacheObjectFactory.CreateAndParse(cacheIndex.CacheIndex1);
            Assert.AreEqual(ObjectType.Structure, centaurConcaveTower.Flag);
        }

        [Test]
        public void BeastMan_Hut_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.CacheObjects[484032];
            var beastmanHut = this.cacheObjectFactory.CreateAndParse(cacheIndex.CacheIndex1);
            Assert.AreEqual(ObjectType.Structure, beastmanHut.Flag);
        }


        [Test]
        public void Create_Simpler_Returns_A_Simple_CacheObject()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.First(x => x.Identity == 103);
            var cacheObject = this.cacheObjectFactory.CreateAndParse(cacheIndex);
            Assert.AreEqual(ObjectType.Simple, cacheObject.Flag);
        }

    }
}