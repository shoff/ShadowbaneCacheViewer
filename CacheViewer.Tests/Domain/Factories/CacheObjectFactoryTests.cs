namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Models;
    using CacheViewer.Domain.Models.Exportable;
    using Data;
    using Data.Entities;
    using Newtonsoft.Json;
    using Tests;
    using Xunit;

    public class CacheObjectFactoryFacts
    {
        private readonly CacheObjectFactory cacheObjectFactory = CacheObjectFactory.Instance;

        [Fact(Skip = Skip.CREATES_FILES)]
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

        [Fact(Skip = Skip.CREATES_FILES)]
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

        [Fact(Skip = Skip.CONNECTS_TO_DB)]
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
                        CompressedSize = (int) render.CacheIndex.CompressedSize,
                        FileOffset = (int) render.CacheIndex.Offset,
                        HasMesh = render.HasMesh,
                        JointName = render.JointName,
                        HasTexture = render.HasTexture,
                        RenderCount = render.ChildCount,
                        MeshId = render.MeshId,
                        Order = render.Order,
                        Position = $"{render.Position.X}-{render.Position.Y}-{render.Position.Z}",
                        // Textures = render.Textures,
                        // TODO doesn't handle textures
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

        [Fact(Skip = Skip.CONNECTS_TO_DB)]
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
                            CompressedSize = (int) cobject.CacheIndex.CompressedSize,
                            FileOffset = (int) cobject.CacheIndex.Offset,
                            Name = cobject.Name,
                            ObjectType = cobject.Flag,
                            ObjectTypeDescription = objectTypeDictionary[cobject.Flag],
                            UncompressedSize = (int) cobject.CacheIndex.UnCompressedSize

                        };
                        context.CacheObjectEntities.Add(centity);
                    }

                    Dictionary<long, int> ids = new Dictionary<long, int>();
                    var structure = cobject;
                    using (var reader = structure.Data.CreateBinaryReaderUtf32())
                    {
                        reader.BaseStream.Position =
                            57; // this is common to all cache files and doesn't contain any render ids

                        while (reader.BaseStream.Position + 4 <= structure.Data.Count)
                        {
                            int renderId = reader.ReadInt32();

                            int range = renderId > centity.CacheIndexIdentity ?
                                Math.Abs(renderId - centity.CacheIndexIdentity) :
                                Math.Abs(centity.CacheIndexIdentity - renderId);

                            if (range < 5000 &&
                                Array.IndexOf(RenderInformationFactory.Instance.RenderArchive.IdentityArray, renderId) >
                                -1)
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
            var folder = AppDomain.CurrentDomain.BaseDirectory + "\\CacheObjectIndexes";
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

        [Fact]
        public void The_First_CacheIndex_Is_The_Sun()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault();
            var sun = this.cacheObjectFactory.CreateAndParse(cacheIndex);
            Assert.Equal(ObjectType.Sun, sun.Flag);
        }

        [Fact]
        public void Centaur_Concave_Tower_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.CacheObjects[585000];
            var centaurConcaveTower = this.cacheObjectFactory.CreateAndParse(cacheIndex.CacheIndex1);
            Assert.Equal(ObjectType.Structure, centaurConcaveTower.Flag);
        }

        [Fact]
        public void BeastMan_Hut_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.CacheObjects[484032];
            var beastmanHut = this.cacheObjectFactory.CreateAndParse(cacheIndex.CacheIndex1);
            Assert.Equal(ObjectType.Structure, beastmanHut.Flag);
        }

        [Fact]
        public void Create_Simpler_Returns_A_Simple_CacheObject()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.First(x => x.Identity == 103);
            var cacheObject = this.cacheObjectFactory.CreateAndParse(cacheIndex);
            Assert.Equal(ObjectType.Simple, cacheObject.Flag);
        }

        [Fact]
        public void ElvenTent_Parses_Successfully()
        {
            RenderInformationFactory.Instance.AppendModel = true;
            var cacheIndex = this.cacheObjectFactory.CacheObjects[423400];
            var elvenTent = this.cacheObjectFactory.CreateAndParse(cacheIndex.CacheIndex1, true);

            Assert.Equal(22, elvenTent.RenderCount);
            Assert.Equal(22, elvenTent.RenderIds.Count);
            Assert.Equal("Elven Tent", elvenTent.Name);
            Assert.Equal(ObjectType.Structure, elvenTent.Flag);
        }

        #region InlineData
        [Theory]
        [InlineData(124000)]
        [InlineData(124300)]
        [InlineData(144000)]
        [InlineData(144063)]
        [InlineData(144109)]
        [InlineData(144500)]
        [InlineData(1500000)]
        [InlineData(1600500)]
        [InlineData(1610000)]
        [InlineData(1612900)]
        [InlineData(162014)]
        [InlineData(162025)]
        [InlineData(162035)]
        [InlineData(402001)]
        [InlineData(402004)]
        [InlineData(402011)]
        [InlineData(404016)]
        [InlineData(404031)]
        [InlineData(404048)]
        [InlineData(422600)]
        [InlineData(422700)]
        [InlineData(423300)]
        [InlineData(423400)]
        [InlineData(423500)]
        [InlineData(423600)]
        [InlineData(424000)]
        [InlineData(424082)]
        [InlineData(424128)]
        [InlineData(424180)]
        [InlineData(424285)]
        [InlineData(424367)]
        [InlineData(424484)]
        [InlineData(424554)]
        [InlineData(424581)]
        [InlineData(44000)]
        [InlineData(622670)]
        [InlineData(622718)]
        [InlineData(622770)]
        [InlineData(64000)]
        [InlineData(64014)]
        [InlineData(64035)]
        [InlineData(64072)]
        [InlineData(64124)]
        [InlineData(64148)]
        [InlineData(64160)]
        [InlineData(64500)]
        [InlineData(64600)]
        [InlineData(64700)]
        [InlineData(64800)]
        [InlineData(585000)]
        [InlineData(585200)]
        [InlineData(585400)]
        [InlineData(585600)]
        [InlineData(585800)]
        [InlineData(586000)]
        [InlineData(586400)]
        [InlineData(586600)]
        [InlineData(564000)]
        [InlineData(564100)]
        [InlineData(564200)]
        [InlineData(564300)]
        [InlineData(564400)]
        [InlineData(564500)]
        [InlineData(564600)]
        [InlineData(5000400)]
        [InlineData(5000700)]
        [InlineData(5000800)]
        [InlineData(5001500)]
        [InlineData(5010000)]
        [InlineData(5010100)]
        [InlineData(5010200)]
        [InlineData(5031000)]
        [InlineData(5031200)]
        [InlineData(5031400)]
        [InlineData(482128)]
        [InlineData(482136)]
        [InlineData(482144)]
        [InlineData(484000)]
        [InlineData(484016)]
        [InlineData(484032)]
        [InlineData(484100)]
        [InlineData(484120)]
        [InlineData(484140)]
        [InlineData(484160)]
        [InlineData(484180)]
        [InlineData(484200)]
        [InlineData(484220)]
        [InlineData(484240)]
        [InlineData(484260)]
        [InlineData(484280)]
        [InlineData(484300)]
        [InlineData(484320)]
        [InlineData(484500)]
        [InlineData(460126)]
        [InlineData(460152)]
        [InlineData(460173)]
        [InlineData(460189)]
        [InlineData(460314)]
        [InlineData(460338)]
        [InlineData(460374)]
        [InlineData(460600)]
        [InlineData(460610)]
        [InlineData(460620)]
        [InlineData(460700)]
        [InlineData(460861)]
        [InlineData(460886)]
        [InlineData(460900)]
        [InlineData(460999)]
        [InlineData(461000)]
        [InlineData(461800)]
        [InlineData(461900)]
        [InlineData(462000)]
        [InlineData(462100)]
        [InlineData(462200)]
        [InlineData(462500)]
        [InlineData(804000)]
        [InlineData(814000)]
        [InlineData(5001100)]
        [InlineData(5050000)]
        #endregion
        public void All_Type_4_CObjects_RenderInfo_Is_Discoverable(int identity)
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.First(x => x.Identity == identity);
            var asset = this.cacheObjectFactory.CacheObjects[cacheIndex.Identity];

            int count = 0;
            StructureValidationResult structureValidationResult = new StructureValidationResult();

            using (var reader = asset.Item1.CreateBinaryReaderUtf32())
            {
                var ids = new List<int>();
                // reader.skip(4); // ignore "TNLC" tag
                // ReSharper disable once UnusedVariable
                var tnlc = reader.ReadInt32();
                // 4
                var flag = (ObjectType) reader.ReadInt32();
                var nameLength = reader.ReadUInt32();
                var name = reader.ReadAsciiString(nameLength);

                int readCount = 0;
                // should be 12 since the trailing byte may not be there if we are at the end of the file
                // once we have found a valid id then we will jump forward more than a byte at a time.

                while (reader.CanRead(12) && ids.Count == 0)
                {
                    structureValidationResult = reader.ValidateCobjectIdType4(identity);
                    readCount++;

                    if (structureValidationResult.IsValid)
                    {
                        ids.Add(structureValidationResult.Id);
                        count = reader.StructureRenderCount(structureValidationResult);
                    }
                    else
                    {
                        reader.BaseStream.Position = (structureValidationResult.InitialOffset + 1);
                    }
                }

                while (reader.CanRead(12) && structureValidationResult.IsValid &&
                    structureValidationResult.NullTerminator == 0)
                {
                    readCount++;
                    structureValidationResult = reader.ValidateCobjectIdType4(identity);
                    if (structureValidationResult.IsValid)
                    {
                        ids.Add(structureValidationResult.Id);
                    }
                }

                Console.WriteLine($"Read {readCount} unique integer values.");

                ids.Each(i => Console.Write($"{i}, "));
                Console.WriteLine();

                Assert.Equal(count, ids.Count);
            }
        }

        #region InlineDatas
        [Theory]
        [InlineData(124100)]
        [InlineData(124400)]
        [InlineData(124600)]
        [InlineData(1300000)]
        [InlineData(1300200)]
        [InlineData(1300400)]
        [InlineData(1300600)]
        [InlineData(1300800)]
        [InlineData(1301000)]
        [InlineData(1301200)]
        [InlineData(1301400)]
        [InlineData(1301600)]
        [InlineData(1301800)]
        [InlineData(1302000)]
        [InlineData(1302200)]
        [InlineData(1302400)]
        [InlineData(1302600)]
        [InlineData(1302800)]
        [InlineData(1303000)]
        [InlineData(1303200)]
        [InlineData(1303400)]
        [InlineData(1303600)]
        [InlineData(1303800)]
        [InlineData(1304000)]
        [InlineData(1304200)]
        [InlineData(1304400)]
        [InlineData(1304600)]
        [InlineData(1304800)]
        [InlineData(1305000)]
        [InlineData(1305200)]
        [InlineData(1305400)]
        [InlineData(1305600)]
        [InlineData(1305800)]
        [InlineData(1306000)]
        [InlineData(1306200)]
        [InlineData(1306400)]
        [InlineData(1306600)]
        [InlineData(1306800)]
        [InlineData(1307000)]
        [InlineData(1307200)]
        [InlineData(1307400)]
        [InlineData(1307600)]
        [InlineData(1307800)]
        [InlineData(1308000)]
        [InlineData(1308200)]
        [InlineData(1309000)]
        [InlineData(1309100)]
        [InlineData(1309200)]
        [InlineData(1309300)]
        [InlineData(1309400)]
        [InlineData(1309500)]
        [InlineData(1309600)]
        [InlineData(1309700)]
        [InlineData(1309800)]
        [InlineData(1309900)]
        [InlineData(1310000)]
        [InlineData(1310100)]
        [InlineData(1310200)]
        [InlineData(1310300)]
        [InlineData(1310400)]
        [InlineData(1310500)]
        [InlineData(1310600)]
        [InlineData(1320000)]
        [InlineData(1320200)]
        [InlineData(1320400)]
        [InlineData(1320600)]
        [InlineData(1320800)]
        [InlineData(1321000)]
        [InlineData(1321200)]
        [InlineData(1321400)]
        [InlineData(1321600)]
        [InlineData(1321800)]
        [InlineData(1322000)]
        [InlineData(1322200)]
        [InlineData(1322400)]
        [InlineData(1322600)]
        [InlineData(1322800)]
        [InlineData(1323000)]
        [InlineData(1323200)]
        [InlineData(1323400)]
        [InlineData(1323600)]
        [InlineData(1323800)]
        [InlineData(1324000)]
        [InlineData(1324200)]
        [InlineData(1324400)]
        [InlineData(1324600)]
        [InlineData(1324800)]
        [InlineData(1325000)]
        [InlineData(1325200)]
        [InlineData(1325400)]
        [InlineData(1325600)]
        [InlineData(1325800)]
        [InlineData(1326000)]
        [InlineData(1326200)]
        [InlineData(1326400)]
        [InlineData(1326600)]
        [InlineData(1326800)]
        [InlineData(1327000)]
        [InlineData(1327200)]
        [InlineData(1327400)]
        [InlineData(1327600)]
        [InlineData(1327800)]
        [InlineData(1330000)]
        [InlineData(1330100)]
        [InlineData(1330200)]
        [InlineData(1330300)]
        [InlineData(1330400)]
        [InlineData(1330500)]
        [InlineData(1330600)]
        [InlineData(1330700)]
        [InlineData(1330800)]
        [InlineData(1330900)]
        [InlineData(1331000)]
        [InlineData(1331100)]
        [InlineData(1331200)]
        [InlineData(1331300)]
        [InlineData(1331400)]
        [InlineData(1331500)]
        [InlineData(1331600)]
        [InlineData(1331700)]
        [InlineData(1331800)]
        [InlineData(1331900)]
        [InlineData(1332000)]
        [InlineData(1332100)]
        [InlineData(1332200)]
        [InlineData(1332300)]
        [InlineData(1332400)]
        [InlineData(1332500)]
        [InlineData(1340000)]
        [InlineData(1340400)]
        [InlineData(1340600)]
        [InlineData(1340800)]
        [InlineData(1341000)]
        [InlineData(1341200)]
        [InlineData(1341600)]
        [InlineData(1341800)]
        [InlineData(1342000)]
        [InlineData(1342400)]
        [InlineData(1342600)]
        [InlineData(1342800)]
        [InlineData(1343000)]
        [InlineData(1343400)]
        [InlineData(1343600)]
        [InlineData(1343800)]
        [InlineData(1344000)]
        [InlineData(1344200)]
        [InlineData(1344600)]
        [InlineData(1344800)]
        [InlineData(1345000)]
        [InlineData(1345200)]
        [InlineData(1345600)]
        [InlineData(1345800)]
        [InlineData(1346000)]
        [InlineData(1346200)]
        [InlineData(1346400)]
        [InlineData(1346600)]
        [InlineData(1346800)]
        [InlineData(1347000)]
        [InlineData(1348000)]
        [InlineData(1348100)]
        [InlineData(1348200)]
        [InlineData(1348300)]
        [InlineData(1348400)]
        [InlineData(1348500)]
        [InlineData(1348600)]
        [InlineData(1348700)]
        [InlineData(1348800)]
        [InlineData(1348900)]
        [InlineData(1349000)]
        [InlineData(1349100)]
        [InlineData(1349200)]
        [InlineData(1349300)]
        [InlineData(1349400)]
        [InlineData(1349500)]
        [InlineData(1349600)]
        [InlineData(144110)]
        [InlineData(144600)]
        [InlineData(144700)]
        [InlineData(144800)]
        [InlineData(144900)]
        [InlineData(1500100)]
        [InlineData(1500200)]
        [InlineData(1501200)]
        [InlineData(1501600)]
        [InlineData(1501800)]
        [InlineData(1600000)]
        [InlineData(1611800)]
        [InlineData(1611900)]
        [InlineData(1612000)]
        [InlineData(1613550)]
        [InlineData(164100)]
        [InlineData(1700000)]
        [InlineData(1700002)]
        [InlineData(1700100)]
        [InlineData(1700200)]
        [InlineData(1700300)]
        [InlineData(1700400)]
        [InlineData(1700500)]
        [InlineData(1700600)]
        [InlineData(1700700)]
        [InlineData(1700800)]
        [InlineData(1700900)]
        [InlineData(1701000)]
        [InlineData(1701100)]
        [InlineData(1701200)]
        [InlineData(1701300)]
        [InlineData(1701400)]
        [InlineData(1701500)]
        [InlineData(1701600)]
        [InlineData(1701700)]
        [InlineData(1701800)]
        [InlineData(1701900)]
        [InlineData(1702000)]
        [InlineData(1702100)]
        [InlineData(1702200)]
        [InlineData(1702300)]
        [InlineData(1702400)]
        [InlineData(1702500)]
        [InlineData(1702600)]
        [InlineData(1702700)]
        [InlineData(1702800)]
        [InlineData(1702900)]
        [InlineData(1703000)]
        [InlineData(1703100)]
        [InlineData(1703200)]
        [InlineData(1703300)]
        [InlineData(1703400)]
        [InlineData(1703500)]
        [InlineData(1703600)]
        [InlineData(1703700)]
        [InlineData(1703800)]
        [InlineData(1703900)]
        [InlineData(1704000)]
        [InlineData(1704100)]
        [InlineData(1704200)]
        [InlineData(1704300)]
        [InlineData(1704400)]
        [InlineData(1704500)]
        [InlineData(1704600)]
        [InlineData(1704700)]
        [InlineData(1704800)]
        [InlineData(1704900)]
        [InlineData(1705000)]
        [InlineData(1705100)]
        [InlineData(1705200)]
        [InlineData(1705300)]
        [InlineData(1705400)]
        [InlineData(1705500)]
        [InlineData(1705600)]
        [InlineData(1705700)]
        [InlineData(1705800)]
        [InlineData(1705900)]
        [InlineData(1706000)]
        [InlineData(1706100)]
        [InlineData(1706200)]
        [InlineData(1706300)]
        [InlineData(1706400)]
        [InlineData(1706500)]
        [InlineData(1706600)]
        [InlineData(1706700)]
        [InlineData(1706800)]
        [InlineData(1706900)]
        [InlineData(1707000)]
        [InlineData(1707100)]
        [InlineData(1707200)]
        [InlineData(1707300)]
        [InlineData(1707400)]
        [InlineData(1707500)]
        [InlineData(1707600)]
        [InlineData(1707700)]
        [InlineData(1707800)]
        [InlineData(1707900)]
        [InlineData(1708000)]
        [InlineData(1708100)]
        [InlineData(1708200)]
        [InlineData(1708300)]
        [InlineData(1708400)]
        [InlineData(1708500)]
        [InlineData(1708600)]
        [InlineData(1708700)]
        [InlineData(1708800)]
        [InlineData(1708900)]
        [InlineData(1709000)]
        [InlineData(1709100)]
        [InlineData(1709200)]
        [InlineData(1709300)]
        [InlineData(1709400)]
        [InlineData(1709500)]
        [InlineData(1709600)]
        [InlineData(1709700)]
        [InlineData(1709800)]
        [InlineData(1709900)]
        [InlineData(1710000)]
        [InlineData(1710100)]
        [InlineData(1800000)]
        [InlineData(1800100)]
        [InlineData(1800200)]
        [InlineData(1800300)]
        [InlineData(1800400)]
        [InlineData(1800500)]
        [InlineData(1800600)]
        [InlineData(1800700)]
        [InlineData(1800800)]
        [InlineData(1800900)]
        [InlineData(1801000)]
        [InlineData(1801100)]
        [InlineData(1801200)]
        [InlineData(1801300)]
        [InlineData(1801400)]
        [InlineData(1801500)]
        [InlineData(1801600)]
        [InlineData(1801700)]
        [InlineData(1801800)]
        [InlineData(1801900)]
        [InlineData(1802000)]
        [InlineData(1802100)]
        [InlineData(1802200)]
        [InlineData(1802300)]
        [InlineData(1802400)]
        [InlineData(1802500)]
        [InlineData(1802600)]
        [InlineData(1802700)]
        [InlineData(1802800)]
        [InlineData(1802900)]
        [InlineData(1803000)]
        [InlineData(1803100)]
        [InlineData(1803200)]
        [InlineData(1803300)]
        [InlineData(1803400)]
        [InlineData(1803500)]
        [InlineData(1803600)]
        [InlineData(1803700)]
        [InlineData(1803800)]
        [InlineData(1803900)]
        [InlineData(1804000)]
        [InlineData(1804100)]
        [InlineData(1804200)]
        [InlineData(1804300)]
        [InlineData(1804400)]
        [InlineData(1804500)]
        [InlineData(1804600)]
        [InlineData(1804700)]
        [InlineData(1804800)]
        [InlineData(1804900)]
        [InlineData(1805000)]
        [InlineData(1805100)]
        [InlineData(1805200)]
        [InlineData(1805300)]
        [InlineData(1805400)]
        [InlineData(1805500)]
        [InlineData(1805600)]
        [InlineData(1805700)]
        [InlineData(1805800)]
        [InlineData(1805900)]
        [InlineData(1806000)]
        [InlineData(1806100)]
        [InlineData(1806200)]
        [InlineData(24000)]
        [InlineData(24100)]
        [InlineData(24200)]
        [InlineData(24300)]
        [InlineData(24500)]
        [InlineData(322100)]
        [InlineData(322200)]
        [InlineData(402000)]
        [InlineData(404000)]
        [InlineData(405000)]
        [InlineData(405200)]
        [InlineData(405300)]
        [InlineData(405400)]
        [InlineData(405600)]
        [InlineData(405800)]
        [InlineData(406000)]
        [InlineData(406100)]
        [InlineData(406200)]
        [InlineData(406300)]
        [InlineData(406400)]
        [InlineData(406500)]
        [InlineData(406600)]
        [InlineData(406700)]
        [InlineData(406701)]
        [InlineData(406702)]
        [InlineData(406703)]
        [InlineData(406800)]
        [InlineData(406900)]
        [InlineData(407000)]
        [InlineData(407100)]
        [InlineData(407150)]
        [InlineData(407200)]
        [InlineData(407250)]
        [InlineData(407300)]
        [InlineData(407350)]
        [InlineData(407400)]
        [InlineData(407450)]
        [InlineData(407500)]
        [InlineData(407550)]
        [InlineData(407600)]
        [InlineData(407650)]
        [InlineData(407700)]
        [InlineData(407750)]
        [InlineData(407800)]
        [InlineData(407850)]
        [InlineData(407900)]
        [InlineData(407950)]
        [InlineData(408000)]
        [InlineData(408050)]
        [InlineData(408100)]
        [InlineData(408150)]
        [InlineData(408200)]
        [InlineData(408250)]
        [InlineData(408300)]
        [InlineData(408350)]
        [InlineData(408400)]
        [InlineData(408450)]
        [InlineData(408500)]
        [InlineData(408550)]
        [InlineData(408600)]
        [InlineData(408700)]
        [InlineData(410000)]
        [InlineData(410100)]
        [InlineData(410200)]
        [InlineData(410300)]
        [InlineData(425000)]
        [InlineData(425200)]
        [InlineData(425400)]
        [InlineData(425600)]
        [InlineData(425800)]
        [InlineData(426000)]
        [InlineData(426200)]
        [InlineData(426400)]
        [InlineData(426600)]
        [InlineData(426800)]
        [InlineData(444000)]
        [InlineData(444100)]
        [InlineData(444200)]
        [InlineData(444300)]
        [InlineData(444400)]
        [InlineData(444500)]
        [InlineData(444600)]
        [InlineData(444700)]
        [InlineData(444800)]
        [InlineData(444900)]
        [InlineData(445000)]
        [InlineData(445100)]
        [InlineData(445200)]
        [InlineData(445300)]
        [InlineData(445400)]
        [InlineData(445500)]
        [InlineData(445600)]
        [InlineData(450000)]
        [InlineData(450100)]
        [InlineData(450150)]
        [InlineData(450200)]
        [InlineData(450250)]
        [InlineData(450300)]
        [InlineData(450350)]
        [InlineData(450400)]
        [InlineData(450450)]
        [InlineData(450500)]
        [InlineData(450550)]
        [InlineData(450600)]
        [InlineData(450650)]
        [InlineData(450700)]
        [InlineData(450750)]
        [InlineData(450800)]
        [InlineData(450850)]
        [InlineData(450900)]
        [InlineData(450950)]
        [InlineData(451000)]
        [InlineData(452000)]
        [InlineData(452100)]
        [InlineData(452200)]
        [InlineData(452250)]
        [InlineData(452300)]
        [InlineData(452350)]
        [InlineData(452400)]
        [InlineData(452550)]
        [InlineData(452600)]
        [InlineData(452650)]
        [InlineData(452700)]
        [InlineData(452750)]
        [InlineData(452800)]
        [InlineData(452850)]
        [InlineData(452900)]
        [InlineData(452950)]
        [InlineData(453000)]
        [InlineData(454000)]
        [InlineData(454100)]
        [InlineData(454200)]
        [InlineData(454250)]
        [InlineData(454300)]
        [InlineData(454350)]
        [InlineData(454400)]
        [InlineData(454550)]
        [InlineData(454600)]
        [InlineData(454650)]
        [InlineData(454700)]
        [InlineData(454750)]
        [InlineData(454800)]
        [InlineData(454850)]
        [InlineData(454900)]
        [InlineData(454950)]
        [InlineData(455000)]
        [InlineData(455200)]
        [InlineData(455300)]
        [InlineData(455400)]
        [InlineData(455500)]
        [InlineData(455700)]
        [InlineData(455800)]
        [InlineData(456100)]
        [InlineData(456400)]
        [InlineData(456700)]
        [InlineData(457000)]
        [InlineData(457300)]
        [InlineData(457600)]
        [InlineData(457900)]
        [InlineData(458000)]
        [InlineData(458100)]
        [InlineData(458200)]
        [InlineData(5060000)]
        [InlineData(5060400)]
        [InlineData(5060800)]
        [InlineData(5070000)]
        [InlineData(5070200)]
        [InlineData(5070400)]
        [InlineData(5070600)]
        [InlineData(5070800)]
        [InlineData(5071000)]
        [InlineData(5071200)]
        [InlineData(5071400)]
        [InlineData(5071600)]
        [InlineData(524000)]
        [InlineData(565000)]
        [InlineData(584000)]
        [InlineData(584200)]
        [InlineData(584400)]
        [InlineData(584600)]
        [InlineData(584800)]
        [InlineData(586200)]
        [InlineData(590000)]
        [InlineData(590400)]
        [InlineData(590800)]
        [InlineData(591200)]
        [InlineData(591600)]
        [InlineData(592000)]
        [InlineData(592400)]
        [InlineData(592800)]
        [InlineData(593200)]
        [InlineData(593600)]
        [InlineData(594000)]
        [InlineData(594400)]
        [InlineData(594800)]
        [InlineData(595200)]
        [InlineData(595600)]
        [InlineData(596000)]
        [InlineData(596400)]
        [InlineData(596800)]
        [InlineData(600000)]
        [InlineData(600400)]
        [InlineData(600800)]
        [InlineData(601200)]
        [InlineData(601600)]
        [InlineData(602000)]
        [InlineData(602400)]
        [InlineData(602800)]
        [InlineData(605000)]
        [InlineData(605400)]
        [InlineData(605800)]
        [InlineData(606200)]
        [InlineData(606600)]
        [InlineData(607000)]
        [InlineData(607400)]
        [InlineData(607800)]
        [InlineData(608200)]
        [InlineData(608600)]
        [InlineData(609000)]
        [InlineData(609400)]
        [InlineData(610000)]
        [InlineData(610400)]
        [InlineData(610800)]
        [InlineData(611200)]
        [InlineData(611600)]
        [InlineData(612000)]
        [InlineData(612400)]
        [InlineData(612800)]
        [InlineData(614000)]
        [InlineData(614400)]
        [InlineData(614800)]
        [InlineData(615200)]
        [InlineData(615600)]
        [InlineData(616000)]
        [InlineData(616400)]
        [InlineData(616800)]
        [InlineData(617200)]
        [InlineData(617600)]
        [InlineData(618000)]
        [InlineData(618400)]
        [InlineData(65000)]
        [InlineData(65100)]
        [InlineData(820000)]
        [InlineData(820400)]
        [InlineData(820800)]
        [InlineData(821200)]
        [InlineData(821600)]
        [InlineData(822000)]
        [InlineData(822400)]
        [InlineData(822800)]
        [InlineData(823200)]
        [InlineData(823600)]
        [InlineData(824000)]
        [InlineData(824400)]
        [InlineData(824800)]
        [InlineData(825200)]
        [InlineData(825600)]
        [InlineData(826000)]
        [InlineData(826400)]
        [InlineData(826800)]
        [InlineData(827200)]
        [InlineData(827600)]
        [InlineData(880000)]
        [InlineData(880400)]
        [InlineData(880800)]
        [InlineData(881200)]
        [InlineData(881600)]
        [InlineData(882000)]
        [InlineData(882400)]
        [InlineData(882800)]
        [InlineData(883200)]
        [InlineData(883600)]
        [InlineData(884000)]
        [InlineData(884400)]
        [InlineData(972000)]
        [InlineData(972050)]
        [InlineData(972100)]
        [InlineData(972150)]
        [InlineData(972200)]
        [InlineData(972250)]
        [InlineData(972300)]
        [InlineData(972700)]
        [InlineData(972750)]
        [InlineData(972900)]
        [InlineData(972950)]
        [InlineData(973150)]
        [InlineData(973250)]
        [InlineData(973300)]
        [InlineData(973350)]
        [InlineData(973400)]
        #endregion
        public void All_Type_5_CObjects_RenderInfo_Is_Discoverable(int identity)
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.First(x => x.Identity == identity);
            var asset = this.cacheObjectFactory.CacheObjects[cacheIndex.Identity];

            int count = 0;
            var structureValidationResult = new StructureValidationResult();

            using (var reader = asset.Item1.CreateBinaryReaderUtf32())
            {
                var ids = new List<int>();
                var tnlc = reader.ReadInt32();
                var flag = (ObjectType) reader.ReadInt32();
                var nameLength = reader.ReadUInt32();
                var name = reader.ReadAsciiString(nameLength);
                int readCount = 0;

                // should be 12 since the trailing byte may not be there if we are at the end of the file
                // once we have found a valid id then we will jump forward more than a byte at a time.

                while (reader.CanRead(12) && ids.Count == 0)
                {
                    structureValidationResult = reader.ValidateCobjectIdType4(identity);
                    readCount++;

                    if (structureValidationResult.IsValid)
                    {
                        ids.Add(structureValidationResult.Id);
                        count = reader.StructureRenderCount(structureValidationResult);
                    }
                    else
                    {
                        reader.BaseStream.Position = (structureValidationResult.InitialOffset + 1);
                    }
                }

                while (reader.CanRead(12) && structureValidationResult.IsValid &&
                    structureValidationResult.NullTerminator == 0)
                {
                    readCount++;
                    structureValidationResult = reader.ValidateCobjectIdType4(identity);
                    if (structureValidationResult.IsValid)
                    {
                        ids.Add(structureValidationResult.Id);
                    }
                }
                Console.WriteLine($"Read {readCount} unique integer values.");
                ids.Each(i => Console.Write($"{i}, "));
                Console.WriteLine();
                Assert.Equal(count, ids.Count);
            }
        }

        #region InlineData
        [InlineData(2001)]
        [InlineData(2003)]
        #endregion
        public void All_Mobile_CObjects_RenderInfo_Is_Discoverable(int identity)
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.First(x => x.Identity == identity);
            var asset = this.cacheObjectFactory.CacheObjects[cacheIndex.Identity];

            int count = 0;
            StructureValidationResult structureValidationResult = new StructureValidationResult();

            using (var reader = asset.Item1.CreateBinaryReaderUtf32())
            {
                var ids = new List<int>();
                // reader.skip(4); // ignore "TNLC" tag
                // ReSharper disable once UnusedVariable
                var tnlc = reader.ReadInt32();
                // 4
                var flag = (ObjectType)reader.ReadInt32();
                var nameLength = reader.ReadUInt32();
                var name = reader.ReadAsciiString(nameLength);

                int readCount = 0;
                // should be 12 since the trailing byte may not be there if we are at the end of the file
                // once we have found a valid id then we will jump forward more than a byte at a time.

                while (reader.CanRead(12) && ids.Count == 0)
                {
                    structureValidationResult = reader.ValidateCobjectIdType4(identity);
                    readCount++;

                    if (structureValidationResult.IsValid)
                    {
                        ids.Add(structureValidationResult.Id);
                        count = reader.StructureRenderCount(structureValidationResult);
                    }
                    else
                    {
                        reader.BaseStream.Position = (structureValidationResult.InitialOffset + 1);
                    }
                }

                while (reader.CanRead(12) && structureValidationResult.IsValid &&
                    structureValidationResult.NullTerminator == 0)
                {
                    readCount++;
                    structureValidationResult = reader.ValidateCobjectIdType4(identity);
                    if (structureValidationResult.IsValid)
                    {
                        ids.Add(structureValidationResult.Id);
                    }
                }

                Console.WriteLine($"Read {readCount} unique integer values.");

                ids.Each(i => Console.Write($"{i}, "));
                Console.WriteLine();

                Assert.Equal(count, ids.Count);
            }
        }

    }
}