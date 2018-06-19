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
    using CacheViewer.Data;
    using CacheViewer.Data.Entities;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Extensions;
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



        [Test, Explicit]
        public void Save_RenderInformation_To_Sql()
        {
             RenderInformationFactory renderInformationFactory = RenderInformationFactory.Instance;

            using (var context = new DataContext())
            {
                var save = 0;
                foreach (var index in renderInformationFactory.RenderArchive.CacheIndices)
                {
                    save++;
                    // await this.renderInformationFactory.RenderArchive.SaveToFile(index, folder);
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
                        TextureId = render.TextureId,
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

            using (var context = new DataContext())
            {
                var save = 0;
                foreach (var i in this.cacheObjectsCache.Indexes)
                {
                    save++;
                    totalCacheItems++;

                    var cobject = this.cacheObjectsCache.CreateAndParse(i);

                    //if (cobject.Flag == ObjectType.Structure)
                    //{
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
                            ObjectTypeDescription = objectTypeDicitonary[cobject.Flag],
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

        [TestCase(424000)]
        public void Update_One_Cache_File_In_Sql(int cacheIndexId)
        {
            //var sb = new StringBuilder();
            //foreach (var index in this.cacheObjectsCache.Indexes)
            //{
            //    sb.AppendLine($"{index.Identity}");
            //}

            //var folder = AppDomain.CurrentDomain.BaseDirectory + "CacheObjectIndexes";
            //File.WriteAllText($"{folder}\\AllRenderIds.txt", sb.ToString());
            using (var context = new DataContext())
            {
                CacheIndex cacheIndex = new CacheIndex();
                foreach (var c in this.cacheObjectsCache.Indexes)
                {
                    cacheIndex = c;

                    if (cacheIndex.Identity.ToString() == cacheIndexId.ToString())
                    {
                        break;
                    }
                }
 

                // cacheIndex = this.cacheObjectsCache.Indexes.FirstOrDefault(i => i.Identity == cacheIndexId);
                var cobject = this.cacheObjectsCache.CreateAndParse(cacheIndex);

                var centity = (from c in context.CacheObjectEntities
                               where c.CacheIndexIdentity == cacheIndexId
                               select c).FirstOrDefault();

                if (centity == null)
                {
                    centity = new CacheObjectEntity
                    {
                        CacheIndexIdentity = cobject.CacheIndex.Identity,
                        CompressedSize = (int)cobject.CacheIndex.CompressedSize,
                        FileOffset = (int)cobject.CacheIndex.Offset,
                        Name = cobject.Name,
                        ObjectType = cobject.Flag,
                        ObjectTypeDescription = objectTypeDicitonary[cobject.Flag],
                        UncompressedSize = (int)cobject.CacheIndex.UnCompressedSize

                    };
                    context.CacheObjectEntities.Add(centity);
                }

                var structure = cobject;
                using (var reader = structure.Data.CreateBinaryReaderUtf32())
                {
                    reader.BaseStream.Position = 57; // this is common to all cache files and doesn't contain any render ids

                    while (reader.BaseStream.Position + 4 <= structure.Data.Count)
                    {
                        int renderId = reader.ReadInt32();

                        if (renderId == 0)
                        {
                            continue;
                        }

                        int range = renderId > centity.CacheIndexIdentity ?
                            Math.Abs(renderId - centity.CacheIndexIdentity) :
                            Math.Abs(centity.CacheIndexIdentity - renderId);

                        if (range < 5000 && Array.IndexOf(RenderInformationFactory.Instance.RenderArchive.IdentityArray, renderId) > -1)
                        {
                            centity.RenderAndOffsets.Add(new RenderAndOffset
                            {
                                RenderId = renderId,
                                OffSet = reader.BaseStream.Position,
                                CacheIndexId = cacheIndexId
                            });
                        }
                        reader.BaseStream.Position -= 3;
                    }
                }
                context.SaveChanges();
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
            foreach (var i in this.cacheObjectsCache.Indexes)
            {
                var cobject = this.cacheObjectsCache.CreateAndParse(i);
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