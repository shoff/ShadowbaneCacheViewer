namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CacheViewer.Data;
    using CacheViewer.Data.Entities;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Factories;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class RenderInformationFactoryTests
    {
        private readonly RenderInformationFactory renderInformationFactory = RenderInformationFactory.Instance;

        [Test, Explicit]
        public async Task Temp_OutPut_All_To_Files()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory + "RenderIndexes";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            foreach (var index in this.renderInformationFactory.RenderArchive.CacheIndices)
            {
                await this.renderInformationFactory.RenderArchive.SaveToFileAsync(index, folder);
            }
        }

        [Test, Explicit]
        public void Temp_OutPut_All_To_Json()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory + "RenderIndexes";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            foreach (var index in this.renderInformationFactory.RenderArchive.CacheIndices)
            {
                // await this.renderInformationFactory.RenderArchive.SaveToFileAsync(index, folder);
                var render = this.renderInformationFactory.Create(index.Identity, index.Order, true);
                var renderJson = JsonConvert.SerializeObject(render);
                File.WriteAllText($"{folder}\\{this.renderInformationFactory.RenderArchive.Name}_{index.Identity}.json",
                    renderJson);
            }
        }

        [Test, Explicit]
        public void Save_To_Sql()
        {
            using (var context = new DataContext())
            {
                var save = 0;
                foreach (var index in this.renderInformationFactory.RenderArchive.CacheIndices)
                {
                    save++;
                    // await this.renderInformationFactory.RenderArchive.SaveToFileAsync(index, folder);
                    var render = this.renderInformationFactory.Create(index.Identity, index.Order, true);
                    var entity = new RenderEntity
                    {
                        ByteCount = render.ByteCount,
                        CacheIndexIdentity = render.CacheIndex.Identity,
                        CompressedSize = (int) render.CacheIndex.CompressedSize,
                        FileOffset = (int) render.CacheIndex.Offset,
                        HasMesh = render.HasMesh,
                        HasTexture = render.HasTexture,
                        RenderCount = render.ChildCount,
                        JointName = render.JointName,
                        MeshId = render.MeshId,
                        Order = render.Order,
                        Position = $"{render.Position.X}-{render.Position.Y}-{render.Position.Z}",
                        TextureId = render.TextureId,
                        UncompressedSize = (int) render.CacheIndex.UnCompressedSize
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
        public void Discover_Render_Textures()
        {
            using (var context = new DataContext())
            {
                var textureIds = (from t in context.Textures
                    select t.TextureId).ToList();

                var save = 0;
                foreach (var index in this.renderInformationFactory.RenderArchive.CacheIndices)
                {
                    var data1 = this.renderInformationFactory.RenderArchive[index.Identity].Item1;
                    var data2 = this.renderInformationFactory.RenderArchive[index.Identity].Item2;

                    using (var reader = data1.CreateBinaryReaderUtf32())
                    {
                        reader.BaseStream.Position = 39;
                        while (reader.BaseStream.Position + 4 <= data1.Count)
                        {
                            var possibleId = reader.ReadInt32();
                            var rangeIsCorrect = index.Identity > 400 ? possibleId > 100 : possibleId < 400;
                            if (rangeIsCorrect && textureIds.Contains(possibleId))
                            {
                                var entity = new RenderTexture
                                {
                                    Offset = reader.BaseStream.Position,
                                    RenderId = index.Identity,
                                    TextureId = possibleId
                                };
                                context.RenderTextures.Add(entity);
                                save++;
                            }
                            reader.BaseStream.Position -= 3;
                        }
                        if (save > 1000)
                        {
                            save = 0;
                            Console.WriteLine("Saving changes");
                            context.SaveChanges();
                        }
                    }

                    if (data2.Count > 0)
                    {
                        using (var reader = data2.CreateBinaryReaderUtf32())
                        {
                            // now we output shit tons of entities to find all posible texture id locations
                            reader.BaseStream.Position = 39;
                            while (reader.BaseStream.Position + 4 <= data2.Count)
                            {
                                var possibleId = reader.ReadInt32();
                                var rangeIsCorrect = index.Identity > 400 ? possibleId > 100 : possibleId < 400;
                                if (rangeIsCorrect && textureIds.Contains(possibleId))
                                {
                                    var entity = new RenderTexture
                                    {
                                        Offset = reader.BaseStream.Position,
                                        RenderId = index.Identity,
                                        TextureId = possibleId
                                    };
                                    context.RenderTextures.Add(entity);
                                    save++;
                                }
                                reader.BaseStream.Position -= 3;
                            }
                        }
                        if (save > 1000)
                        {
                            Console.WriteLine("Saving changes");
                            save = 0;
                            context.SaveChanges();
                        }
                    }
                }

                context.SaveChanges();
            }
        }

        [Test]
        public void RenderArchive_Should_Have_Correct_Indeces_Count()
        {
            //for (int i = 0; i < this.renderInformationFactory.Indexes.Length; i++)
            //{
            //    try
            //    {
            //        var renderInfo = this.renderInformationFactory.CreateAndParse(this.renderInformationFactory.Indexes[i].Identity, addByteData: true);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //}
        }

        [Test]
        public void Render_424060_Should_Have_A_TextureId()
        {
            var ci = this.renderInformationFactory.RenderArchive.CacheIndices.FirstOrDefault(i => i.Identity == 424060);
            var render = this.renderInformationFactory.Create(ci);
            Assert.AreEqual(424003, render.TextureId);
        }
    }
}