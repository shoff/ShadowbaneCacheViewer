using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
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