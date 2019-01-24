using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Factories;
    using Newtonsoft.Json;

    [TestFixture]
    public class MeshFactoryIntegrationTests
    {
        private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.None, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
        private MeshFactory factory;
        private readonly string folder = AppDomain.CurrentDomain.BaseDirectory + "\\MeshIndexes";

        [SetUp]
        public void SetUp()
        {
            this.factory = MeshFactory.Instance;
        }

        [Test, Explicit, Category("Integration")]
        public async Task Save_All_To_File()
        {
            CreateFolders();
            foreach (var index in this.factory.Indexes)
            {
                await this.factory.SaveToFile(index, $"{folder}\\Caches");

                var mesh = this.factory.Create(index);

                var meshJson = JsonConvert.SerializeObject(mesh, jsonSettings);

                File.WriteAllText($"{folder}\\Meshes\\{index.Identity}.json", meshJson);
            }
        }

        private void CreateFolders()
        {

            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
            Directory.CreateDirectory(folder);
            Directory.CreateDirectory($"{folder}\\Meshes");
            Directory.CreateDirectory($"{folder}\\Caches");
        }

        [Test, Explicit, Category("Integration")]
        public void Save_Mesh_To_Sql()
        {
            using (var context = new DataContext())
            {
                int save = 0;

                foreach (var index in this.factory.Indexes)
                {
                    save++;
                    var mesh = this.factory.Create(index);

                    var ent = (from m in context.MeshEntities
                               where m.Id == mesh.Id
                               select m).FirstOrDefault() ??
                        new MeshEntity
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

                    mesh = null;

                    var texture = (from r in context.RenderEntities
                                   join t in context.Textures on r.TextureId equals t.TextureId
                                   where r.MeshId == ent.Id && r.HasTexture && r.TextureId > 0
                                   select t).FirstOrDefault();

                    if (texture != null)
                    {
                        ent.Textures.Add(texture);
                    }

                    context.MeshEntities.Add(ent);


                    if (save == 10000)
                    {
                        context.SaveChanges();
                        save = 0;
                    }
                }

                context.SaveChanges();
            }
        }

        [Test, Explicit, Category("Integration")]
        public void Database_Record_Count_Matches_Index_Count()
        {
            var expected = this.factory.Indexes.Count();
            int actual;
            using (var context = new DataContext())
            {
                actual = context.MeshEntities.Count();
            }

            Assert.AreEqual(expected, actual);
        }
    }
}