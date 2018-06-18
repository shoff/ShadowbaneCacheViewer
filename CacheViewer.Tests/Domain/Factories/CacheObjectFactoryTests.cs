﻿using CacheViewer.Domain.Factories;
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
    using CacheViewer.Domain.Data;
    using CacheViewer.Domain.Data.Entities;
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
        private readonly RenderInformationFactory renderInformationFactory = RenderInformationFactory.Instance;

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
        public void Save_Textures_To_Sql()
        {
            using (var context = new DataContext())
            {
                int i = 0;
                foreach (var cacheIndex in TextureFactory.Instance.Indexes)
                {
                    i++;
                    Texture texture = TextureFactory.Instance.Build(cacheIndex.Identity, false);
                    //context.Textures.Add(texture);
                    var entity = new TextureEntity
                    {
                        Depth = texture.Depth,
                        Height = texture.Height,
                        TextureId = cacheIndex.Identity,
                        Width = texture.Width
                    };
                    context.Textures.Add(entity);

                    if (i > 1000)
                    {
                        context.Commit();
                        i = 0;
                    }
                }
                context.Commit();
            }
        }

        [Test, Explicit]
        public void Save_Mesh_To_Sql()
        {
            using (var context = new DataContext())
            {
                int save = 0;
                foreach (var index in MeshFactory.Instance.Indexes)
                {
                    save++;
                    var mesh = MeshFactory.Instance.Create(index);
                    var ent = new MeshEntity
                    {
                        CacheIndexIdentity = mesh.CacheIndex.Identity,
                        CompressedSize = (int)mesh.CacheIndex.CompressedSize,
                        NormalsCount = (int)mesh.NormalsCount,
                        FileOffset = (int)mesh.CacheIndex.Offset,
                        Id = mesh.Id,
                        Normals = string.Join(";", mesh.Normals.Map(v => $"{v.X}:{v.Y}:{v.Z}").ToArray()),
                        TexturesCount = mesh.Textures?.Count() ?? 0,
                        TextureVectors = string.Join(";", mesh.TextureVectors.Map(v => $"{v.X}:{v.Y}").ToArray()),
                        UncompressedSize = (int)mesh.CacheIndex.UnCompressedSize,
                        VertexCount = mesh.Vertices?.Count ?? 0,
                        Vertices = string.Join(";", mesh.Vertices.Map(v => $"{v.X}:{v.Y}:{v.Z}").ToArray())
                    };
                    context.MeshEntities.Add(ent);


                    if (save == 1000)
                    {
                        context.SaveChanges();
                        save = 0;
                    }
                }

                context.SaveChanges();
            }
        }

        [Test, Explicit]
        public void Save_RenderInformation_To_Sql()
        {
            using (var context = new DataContext())
            {
                var save = 0;
                foreach (var index in this.renderInformationFactory.RenderArchive.CacheIndices)
                {
                    save++;
                    // await this.renderInformationFactory.RenderArchive.SaveToFile(index, folder);
                    var render = this.renderInformationFactory.Create(index.Identity, index.Order, true);
                    var entity = new RenderEntity
                    {
                        ByteCount = render.ByteCount,
                        CacheIndexIdentity = render.CacheIndex.Identity,
                        CompressedSize = (int)render.CacheIndex.CompressedSize,
                        FileOffset = (int)render.CacheIndex.Offset,
                        HasMesh = render.HasMesh,
                        JointName = render.JointName,
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

            var folders = CreateFolders();
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
                                Math.Abs(centity.CacheIndexIdentity-renderId);

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