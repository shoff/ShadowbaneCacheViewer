using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Data;
    using CacheViewer.Domain.Data.Entities;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Factories;
    using Newtonsoft.Json;

    [TestFixture]
    public class MeshFactoryTests
    {
        [Test]
        public async Task Save_All_To_File()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory + "MeshIndexes";
            foreach (var index in MeshFactory.Instance.Indexes)
            {
                await MeshFactory.MeshArchive.SaveToFile(index, $"{folder}\\Caches");
                var mesh = MeshFactory.Instance.Create(index);
                var meshJson = JsonConvert.SerializeObject(mesh);
                File.WriteAllText($"{folder}\\Meshes\\{index.Identity}.json", meshJson);
            }
        }

        [Test]
        public void Save_Ids_To_Sql()
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
                        CompressedSize = (int) mesh.CacheIndex.CompressedSize,
                        NormalsCount = (int) mesh.NormalsCount,
                        FileOffset = (int) mesh.CacheIndex.Offset,
                        Id = mesh.Id,
                        Normals = string.Join(";", mesh.Normals.Map(v => $"{v.X}:{v.Y}:{v.Z}").ToArray()),
                        TexturesCount = mesh.Textures?.Count() ?? 0,
                        TextureVectors = string.Join(";", mesh.TextureVectors.Map(v => $"{v.X}:{v.Y}").ToArray()),
                        UncompressedSize = (int) mesh.CacheIndex.UnCompressedSize,
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


        private static string CreateFolders()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory + "MeshIndexes";
            
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
            Directory.CreateDirectory(folder);
            Directory.CreateDirectory($"{folder}\\Meshes");
            Directory.CreateDirectory($"{folder}\\Caches");

            return folder;
        }
    }
}