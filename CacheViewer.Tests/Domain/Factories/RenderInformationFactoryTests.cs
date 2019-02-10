namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Factories.Providers;
    using CacheViewer.Domain.Models;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class RenderInformationFactoryTests
    {
        private readonly RenderInformationFactory renderInformationFactory = RenderInformationFactory.Instance;

        [Test]
        public void Figuring_Out_541()
        {
            var asset = renderInformationFactory.RenderArchive[541];
            using (var reader = asset.Item1.CreateBinaryReaderUtf32())
            {
                RenderInformation ri = new RenderInformation();
                reader.ParseTypeFour(ri);
            }
        }

        [Test]
        public void Figure_Out_First_Twelve_Bytes()
        {
            var data = new List<(string, string, string, string, string)>();
            data.Add(("Index", "Type", "Short", "Date", "No Clue"));
            foreach (var index in this.renderInformationFactory.RenderArchive.CacheIndices)
            {
                var asset = renderInformationFactory.RenderArchive[index.Identity];

                using (var reader = asset.Item1.CreateBinaryReaderUtf32())
                {
                    var tuple = (index.Identity.ToString(),
                        reader.ReadInt32().ToString(),
                        reader.ReadUInt16().ToString(),
                        reader.ReadToDate()?.ToString() ?? "Null bytes",
                        reader.ReadInt32().ToString());

                    data.Add(tuple);
                }
            }

            var file = AppDomain.CurrentDomain.BaseDirectory + "\\RenderIndexes\\first_twelvel.csv";

            StringBuilder sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append($"{t.Item1}, {t.Item2},{t.Item3}, {t.Item4}, {t.Item5}\r\n");
            }

            File.WriteAllText(file, sb.ToString());
        }

        [Test]
        public void All_Type_Twos_Parse_Correctly()
        {
            foreach (var index in RenderProviders.type2RenderInfos)
            {
                var render = this.renderInformationFactory.Create(index, 0, true);
                Assert.AreEqual(render.TextureCount, render.Textures.Count);
            }
        }

        [Test]
        public void All_Type_Threes_Parse_Correctly()
        {
            foreach (var index in RenderProviders.type3RenderInfos)
            {
                var render = this.renderInformationFactory.Create(index, 0, true);

                if (index == 510)
                {
                    Assert.AreEqual(510, render.MeshId);
                }
                Assert.AreEqual(render.TextureCount, render.Textures.Count);
            }
        }

        [Test]
        public void All_Type_Fours_Parse_Correctly()
        {
            foreach (var index in RenderProviders.type4RenderInfos)
            {
                var render = this.renderInformationFactory.Create(index, 0, true);
                Assert.AreEqual(render.TextureCount, render.Textures.Count);
            }
        }

        [Test]
        public void Hair_6535_Parses_Correctly()
        {
            var render = this.renderInformationFactory.Create(6535, 0, true);

        }
        
        [Test]
        public void RI_12004_Parses_Correctly()
        {
            var render = this.renderInformationFactory.Create(12004, 0, true);

        }

        [Test]
        public void RI_1800_Parses_Correctly()
        {
            var render = this.renderInformationFactory.Create(1800, 0, true);
            Assert.AreEqual(4, render.Textures.Count);
            Assert.AreEqual(1800, render.Textures[0]);
            Assert.AreEqual(1801, render.Textures[1]);
            Assert.AreEqual(1802, render.Textures[2]);
            Assert.AreEqual(1803, render.Textures[3]);
            Assert.AreEqual(2003, render.ModifiedDate.Value.Year);
        }

        [Test]
        public void RI_20_Parses_Correctly()
        {
            var asset = renderInformationFactory.RenderArchive[20];
            using (var reader = asset.Item1.CreateBinaryReaderUtf32())
            {
                var ri = this.renderInformationFactory.Create(20, 0, true);
            }
        }

        [Test]
        public void RenderInfo_Parses_Correctly()
        {
            foreach (var index in this.renderInformationFactory.Indexes)
            {
                var render = this.renderInformationFactory.Create(index.Identity, 0, true);

                if (render.JointNameSize > 0)
                {
                    Assert.True(render.JointName.Length > 0);
                }

                if (render.JointNameSize > 0)
                {
                    Assert.AreEqual(render.JointNameSize, render.JointName.Length);
                }

                Assert.AreEqual(render.TextureCount, render.Textures.Count);      
            }
        }

        [Test, Explicit]
        public async Task Temp_OutPut_All_To_Files()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory + "\\RenderIndexes";
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
            using (var context = new SbCacheViewerContext())
            {
                var save = 0;
                foreach (var index in this.renderInformationFactory.RenderArchive.CacheIndices)
                {
                    save++;
                    // await this.renderInformationFactory.RenderArchive.SaveToFileAsync(index, folder);
                    var render = this.renderInformationFactory.Create(index.Identity, index.Order, true);
                    var entity = context.RenderEntities.FirstOrDefault(r => r.CacheIndexIdentity == render.CacheIndex.Identity) ??
                        new RenderEntity
                        {
                            ByteCount = render.ByteCount,
                            CacheIndexIdentity = render.CacheIndex.Identity,
                            CompressedSize = (int)render.CacheIndex.CompressedSize,
                            FileOffset = (int)render.CacheIndex.Offset,
                            HasMesh = render.HasMesh,
                            HasTexture = render.HasTexture,
                            RenderCount = render.ChildCount,
                            JointName = render.JointName,
                            MeshId = render.MeshId,
                            Order = render.Order,
                            Position = $"{render.Position.X}-{render.Position.Y}-{render.Position.Z}",
                            //Textures = render.Textures,
                            UncompressedSize = (int)render.CacheIndex.UnCompressedSize
                        };

                    foreach (var texture in render.Textures)
                    {
                        if (entity.TextureEntities.All(t => t.TextureId != texture))
                        {
                            var textureEntity = context.Textures.FirstOrDefault(t => t.TextureId == texture);
                            if (textureEntity != null)
                            {
                                entity.TextureEntities.Add(textureEntity);
                            }
                        }
                    }

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
            using (var context = new SbCacheViewerContext())
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
        public void Render_424060_Should_Have_A_TextureId()
        {
            var ci = this.renderInformationFactory.RenderArchive.CacheIndices.FirstOrDefault(i => i.Identity == 424060);
            var render = this.renderInformationFactory.Create(ci);
            Assert.AreEqual(424003, render.Textures);
        }
    }
}