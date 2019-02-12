namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Factories.Providers;
    using CacheViewer.Domain.Models;
    using Data;
    using Data.Entities;
    using Newtonsoft.Json;
    using Xunit;

    public class RenderInformationFactoryFacts
    {
        private readonly RenderInformationFactory renderInformationFactory = RenderInformationFactory.Instance;

        [Fact]
        public void Figuring_Out_541()
        {
            var asset = this.renderInformationFactory.RenderArchive[541];
            using (var reader = asset.Item1.CreateBinaryReaderUtf32())
            {
                RenderInformation ri = new RenderInformation();
                reader.ParseTypeFour(ri);
            }
        }

        [Fact(Skip = "Long running")]
        public void Figure_Out_First_Twelve_Bytes()
        {
            var data = new List<(string, string, string, string, string)>();
            data.Add(("Index", "Type", "Short", "Date", "No Clue"));
            foreach (var index in this.renderInformationFactory.RenderArchive.CacheIndices)
            {
                var asset = this.renderInformationFactory.RenderArchive[index.Identity];

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

        [Fact]
        public void All_Type_Twos_Parse_Correctly()
        {
            foreach (var index in RenderProviders.type2RenderInfos)
            {
                var render = this.renderInformationFactory.Create(index, 0, true);
                Assert.Equal((int)render.TextureCount, render.Textures.Count);
            }
        }

        [Fact]
        public void All_Type_Threes_Parse_Correctly()
        {
            foreach (var index in RenderProviders.type3RenderInfos)
            {
                var render = this.renderInformationFactory.Create(index, 0, true);

                if (index == 510)
                {
                    Assert.Equal(510, render.MeshId);
                }
                Assert.Equal((int)render.TextureCount, render.Textures.Count);
            }
        }

        [Fact]
        public void All_Type_Fours_Parse_Correctly()
        {
            foreach (var index in RenderProviders.type4RenderInfos)
            {
                var render = this.renderInformationFactory.Create(index, 0, true);
                Assert.Equal((int)render.TextureCount, render.Textures.Count);
            }
        }

        [Fact]
        public void Hair_6535_Parses_Correctly()
        {
            var render = this.renderInformationFactory.Create(6535, 0, true);
            Assert.True(render.HasTexture);
            Assert.True(render.HasMesh);
            Assert.Equal(6535, render.MeshId);
        }

        [Fact]
        public void Facting_Type_4_12017()
        {
            var render = this.renderInformationFactory.Create(12017, 0, true);
            Assert.False(render.HasTexture);
        }

        [Fact]
        public void Facting_Type_4_3026()
        {
            var render = this.renderInformationFactory.Create(3026, 0, true);
            Assert.False(render.HasTexture);
            Assert.True(render.HasMesh);
            Assert.Equal(0, render.MeshId);
        }

        [Fact]
        public void Facting_Type_4_3010()
        {
            var render = this.renderInformationFactory.Create(3010, 0, true);
            Assert.False(render.HasTexture);
            Assert.True(render.HasMesh);
            Assert.Equal(0, render.MeshId);
        }

        [Fact]
        public void Facting_A_Type_2()
        {
            var render = this.renderInformationFactory.Create(110109, 0, true);
            Assert.Equal(2, render.Textures.Count);
            Assert.Equal(2, (int)render.TextureCount);
            Assert.True(render.HasTexture);
        }

        [Fact]
        public void Facting_Type_1_600088()
        {
            var render = this.renderInformationFactory.Create(600088, 0, true);
            Assert.Equal(0, render.RenderCount);
            Assert.Single(render.Textures);
            Assert.Equal(1, (int)render.TextureCount);
            Assert.True(render.HasTexture);
            Assert.Equal(600087, render.MeshId);
            Assert.True(render.ValidMeshFound);
        }
        
        [Fact]
        public void Facting_Type_1_42005()
        {
            var render = this.renderInformationFactory.Create(42005, 0, true);
            Assert.Equal(0, render.RenderCount);
            Assert.Single(render.Textures);
            Assert.Equal(1, (int)render.TextureCount);
            Assert.True(render.HasTexture);
            Assert.Equal(42008, render.MeshId);
            Assert.True(render.ValidMeshFound);
        }

        [Fact]
        public void RI_12004_Parses_Correctly()
        {
            var render = this.renderInformationFactory.Create(12004, 0, true);

        }

        [Fact]
        public void RI_1800_Parses_Correctly()
        {
            var render = this.renderInformationFactory.Create(1800, 0, true);
            Assert.Equal(4, render.Textures.Count);
            Assert.Equal(1800, render.Textures[0]);
            Assert.Equal(1801, render.Textures[1]);
            Assert.Equal(1802, render.Textures[2]);
            Assert.Equal(1803, render.Textures[3]);
            Assert.Equal(2003, render.ModifiedDate.Value.Year);
        }

        [Fact]
        public void RI_20_Parses_Correctly()
        {
            var asset = this.renderInformationFactory.RenderArchive[20];
            using (var reader = asset.Item1.CreateBinaryReaderUtf32())
            {
                var ri = this.renderInformationFactory.Create(20, 0, true);
            }
        }

        [Fact]
        public void SomeDuplicate_RenderId_Are_Identical()
        {
            var render1 = this.renderInformationFactory.Create(64146, 0, true);
            var render2 = this.renderInformationFactory.Create(64146, 1, true);

            Assert.Equal(render1.ChildCount, render2.ChildCount);
            Assert.Equal(render1.MeshId, render2.MeshId);
            Assert.Equal(render1.TextureCount, render2.TextureCount);
            Assert.Equal(render1.CreateDate, render2.CreateDate);
        }

        [Fact(Skip ="Long running")]
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
                    Assert.Equal((int)render.JointNameSize, render.JointName.Length);
                }

                Assert.Equal((int)render.TextureCount, render.Textures.Count);      
            }
        }

        [Fact(Skip = "Creates files")]
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

        [Fact(Skip = "Creates files")]
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

        [Fact(Skip = "Connects to DB")]
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

        [Fact(Skip = "Long running and obsolete")]
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

        [Fact]
        public void Render_424060_Should_Have_A_TextureId()
        {
            var ci = this.renderInformationFactory.RenderArchive.CacheIndices.FirstOrDefault(i => i.Identity == 424060);
            var render = this.renderInformationFactory.Create(ci);
            Assert.Equal(424003, render.Textures.First());
        }
    }
}