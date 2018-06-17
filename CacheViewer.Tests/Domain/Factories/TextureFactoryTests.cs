namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text;
    using CacheViewer.Domain.Data;
    using CacheViewer.Domain.Data.Entities;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Models;
    using NUnit.Framework;

    [TestFixture]
    public class TextureFactoryTests
    {
        [Test, Explicit]
        public void Save_All_To_Disk()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory + "Textures";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            List<int> badIds = new List<int>();
            foreach (var cacheIndex in TextureFactory.Instance.Indexes)
            {
                var texture = TextureFactory.Instance.TextureMap(cacheIndex.Identity);
                if (texture != null)
                {
                    texture.Save($"{folder}\\{cacheIndex.Identity}.png", ImageFormat.Png);
                }
                else
                {
                    badIds.Add(cacheIndex.Identity);
                }
            }

            var sb = new StringBuilder();
            foreach (var id in badIds)
            {
                sb.AppendLine($"{id}");
            }

            File.WriteAllText($"{folder}\\badids.csv", sb.ToString());
        }


        [Test, Explicit]
        public void Save_To_Sql()
        {

            using(var context = new DataContext())
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
    }
}