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
    using Data;
    using Data.Entities;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Models;
    using CacheViewer.Domain.Models.Exportable;
    using Newtonsoft.Json;

    [TestFixture]
    public class CacheObjectFactoryTests
    {
        private readonly CacheObjectFactory cacheObjectFactory = CacheObjectFactory.Instance;

        [Test, Explicit]
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

        [Test, Explicit]
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

        [Test, Explicit]
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

        [Test, Explicit]
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

        [Test]
        public void Centaur_2004_Has_Correct_Number_Of_RenderIds()
        {
            using (var context = new SbCacheViewerContext())
            {
                var entity = (from c in context.CacheObjectEntities.Include(r => r.RenderEntities)
                    where c.CacheIndexIdentity == 2004
                    select c).First();

                Assert.AreEqual(33, entity.RenderEntities.Count);
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

        [Test]
        public void The_First_CacheIndex_Is_The_Sun()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.FirstOrDefault();
            var sun = this.cacheObjectFactory.CreateAndParse(cacheIndex);
            Assert.AreEqual(ObjectType.Sun, sun.Flag);
        }

        [Test]
        public void Centaur_Concave_Tower_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.CacheObjects[585000];
            var centaurConcaveTower = this.cacheObjectFactory.CreateAndParse(cacheIndex.CacheIndex1);
            Assert.AreEqual(ObjectType.Structure, centaurConcaveTower.Flag);
        }

        [Test]
        public void BeastMan_Hut_Parses_Correctly()
        {
            var cacheIndex = this.cacheObjectFactory.CacheObjects[484032];
            var beastmanHut = this.cacheObjectFactory.CreateAndParse(cacheIndex.CacheIndex1);
            Assert.AreEqual(ObjectType.Structure, beastmanHut.Flag);
        }

        [Test]
        public void Create_Simpler_Returns_A_Simple_CacheObject()
        {
            var cacheIndex = this.cacheObjectFactory.Indexes.First(x => x.Identity == 103);
            var cacheObject = this.cacheObjectFactory.CreateAndParse(cacheIndex);
            Assert.AreEqual(ObjectType.Simple, cacheObject.Flag);
        }

        #region Testcases

        [TestCase(124000)]
        [TestCase(124300)]
        [TestCase(144000)]
        [TestCase(144063)]
        [TestCase(144109)]
        [TestCase(144500)]
        [TestCase(1500000)]
        [TestCase(1600500)]
        [TestCase(1610000)]
        [TestCase(1612900)]
        [TestCase(162014)]
        [TestCase(162025)]
        [TestCase(162035)]
        [TestCase(402001)]
        [TestCase(402004)]
        [TestCase(402011)]
        [TestCase(404016)]
        [TestCase(404031)]
        [TestCase(404048)]
        [TestCase(422600)]
        [TestCase(422700)]
        [TestCase(423300)]
        [TestCase(423400)]
        [TestCase(423500)]
        [TestCase(423600)]
        [TestCase(424000)]
        [TestCase(424082)]
        [TestCase(424128)]
        [TestCase(424180)]
        [TestCase(424285)]
        [TestCase(424367)]
        [TestCase(424484)]
        [TestCase(424554)]
        [TestCase(424581)]
        [TestCase(44000)]
        [TestCase(622670)]
        [TestCase(622718)]
        [TestCase(622770)]
        [TestCase(64000)]
        [TestCase(64014)]
        [TestCase(64035)]
        [TestCase(64072)]
        [TestCase(64124)]
        [TestCase(64148)]
        [TestCase(64160)]
        [TestCase(64500)]
        [TestCase(64600)]
        [TestCase(64700)]
        [TestCase(64800)]
        [TestCase(585000)]
        [TestCase(585200)]
        [TestCase(585400)]
        [TestCase(585600)]
        [TestCase(585800)]
        [TestCase(586000)]
        [TestCase(586400)]
        [TestCase(586600)]
        [TestCase(564000)]
        [TestCase(564100)]
        [TestCase(564200)]
        [TestCase(564300)]
        [TestCase(564400)]
        [TestCase(564500)]
        [TestCase(564600)]
        [TestCase(5000400)]
        [TestCase(5000700)]
        [TestCase(5000800)]
        [TestCase(5001500)]
        [TestCase(5010000)]
        [TestCase(5010100)]
        [TestCase(5010200)]
        [TestCase(5031000)]
        [TestCase(5031200)]
        [TestCase(5031400)]
        [TestCase(482128)]
        [TestCase(482136)]
        [TestCase(482144)]
        [TestCase(484000)]
        [TestCase(484016)]
        [TestCase(484032)]
        [TestCase(484100)]
        [TestCase(484120)]
        [TestCase(484140)]
        [TestCase(484160)]
        [TestCase(484180)]
        [TestCase(484200)]
        [TestCase(484220)]
        [TestCase(484240)]
        [TestCase(484260)]
        [TestCase(484280)]
        [TestCase(484300)]
        [TestCase(484320)]
        [TestCase(484500)]
        [TestCase(460126)]
        [TestCase(460152)]
        [TestCase(460173)]
        [TestCase(460189)]
        [TestCase(460314)]
        [TestCase(460338)]
        [TestCase(460374)]
        [TestCase(460600)]
        [TestCase(460610)]
        [TestCase(460620)]
        [TestCase(460700)]
        [TestCase(460861)]
        [TestCase(460886)]
        [TestCase(460900)]
        [TestCase(460999)]
        [TestCase(461000)]
        [TestCase(461800)]
        [TestCase(461900)]
        [TestCase(462000)]
        [TestCase(462100)]
        [TestCase(462200)]
        [TestCase(462500)]
        [TestCase(804000)]
        [TestCase(814000)]
        [TestCase(5001100)]
        [TestCase(5050000)]
        [TestCase(482128)]
        [TestCase(482136)]
        [TestCase(482144)]

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

                Assert.AreEqual(count, ids.Count);
            }
        }

        #region TestCases
        [TestCase(124100)]
        [TestCase(124400)]
        [TestCase(124600)]
        [TestCase(1300000)]
        [TestCase(1300200)]
        [TestCase(1300400)]
        [TestCase(1300600)]
        [TestCase(1300800)]
        [TestCase(1301000)]
        [TestCase(1301200)]
        [TestCase(1301400)]
        [TestCase(1301600)]
        [TestCase(1301800)]
        [TestCase(1302000)]
        [TestCase(1302200)]
        [TestCase(1302400)]
        [TestCase(1302600)]
        [TestCase(1302800)]
        [TestCase(1303000)]
        [TestCase(1303200)]
        [TestCase(1303400)]
        [TestCase(1303600)]
        [TestCase(1303800)]
        [TestCase(1304000)]
        [TestCase(1304200)]
        [TestCase(1304400)]
        [TestCase(1304600)]
        [TestCase(1304800)]
        [TestCase(1305000)]
        [TestCase(1305200)]
        [TestCase(1305400)]
        [TestCase(1305600)]
        [TestCase(1305800)]
        [TestCase(1306000)]
        [TestCase(1306200)]
        [TestCase(1306400)]
        [TestCase(1306600)]
        [TestCase(1306800)]
        [TestCase(1307000)]
        [TestCase(1307200)]
        [TestCase(1307400)]
        [TestCase(1307600)]
        [TestCase(1307800)]
        [TestCase(1308000)]
        [TestCase(1308200)]
        [TestCase(1309000)]
        [TestCase(1309100)]
        [TestCase(1309200)]
        [TestCase(1309300)]
        [TestCase(1309400)]
        [TestCase(1309500)]
        [TestCase(1309600)]
        [TestCase(1309700)]
        [TestCase(1309800)]
        [TestCase(1309900)]
        [TestCase(1310000)]
        [TestCase(1310100)]
        [TestCase(1310200)]
        [TestCase(1310300)]
        [TestCase(1310400)]
        [TestCase(1310500)]
        [TestCase(1310600)]
        [TestCase(1320000)]
        [TestCase(1320200)]
        [TestCase(1320400)]
        [TestCase(1320600)]
        [TestCase(1320800)]
        [TestCase(1321000)]
        [TestCase(1321200)]
        [TestCase(1321400)]
        [TestCase(1321600)]
        [TestCase(1321800)]
        [TestCase(1322000)]
        [TestCase(1322200)]
        [TestCase(1322400)]
        [TestCase(1322600)]
        [TestCase(1322800)]
        [TestCase(1323000)]
        [TestCase(1323200)]
        [TestCase(1323400)]
        [TestCase(1323600)]
        [TestCase(1323800)]
        [TestCase(1324000)]
        [TestCase(1324200)]
        [TestCase(1324400)]
        [TestCase(1324600)]
        [TestCase(1324800)]
        [TestCase(1325000)]
        [TestCase(1325200)]
        [TestCase(1325400)]
        [TestCase(1325600)]
        [TestCase(1325800)]
        [TestCase(1326000)]
        [TestCase(1326200)]
        [TestCase(1326400)]
        [TestCase(1326600)]
        [TestCase(1326800)]
        [TestCase(1327000)]
        [TestCase(1327200)]
        [TestCase(1327400)]
        [TestCase(1327600)]
        [TestCase(1327800)]
        [TestCase(1330000)]
        [TestCase(1330100)]
        [TestCase(1330200)]
        [TestCase(1330300)]
        [TestCase(1330400)]
        [TestCase(1330500)]
        [TestCase(1330600)]
        [TestCase(1330700)]
        [TestCase(1330800)]
        [TestCase(1330900)]
        [TestCase(1331000)]
        [TestCase(1331100)]
        [TestCase(1331200)]
        [TestCase(1331300)]
        [TestCase(1331400)]
        [TestCase(1331500)]
        [TestCase(1331600)]
        [TestCase(1331700)]
        [TestCase(1331800)]
        [TestCase(1331900)]
        [TestCase(1332000)]
        [TestCase(1332100)]
        [TestCase(1332200)]
        [TestCase(1332300)]
        [TestCase(1332400)]
        [TestCase(1332500)]
        [TestCase(1340000)]
        [TestCase(1340400)]
        [TestCase(1340600)]
        [TestCase(1340800)]
        [TestCase(1341000)]
        [TestCase(1341200)]
        [TestCase(1341600)]
        [TestCase(1341800)]
        [TestCase(1342000)]
        [TestCase(1342400)]
        [TestCase(1342600)]
        [TestCase(1342800)]
        [TestCase(1343000)]
        [TestCase(1343400)]
        [TestCase(1343600)]
        [TestCase(1343800)]
        [TestCase(1344000)]
        [TestCase(1344200)]
        [TestCase(1344600)]
        [TestCase(1344800)]
        [TestCase(1345000)]
        [TestCase(1345200)]
        [TestCase(1345600)]
        [TestCase(1345800)]
        [TestCase(1346000)]
        [TestCase(1346200)]
        [TestCase(1346400)]
        [TestCase(1346600)]
        [TestCase(1346800)]
        [TestCase(1347000)]
        [TestCase(1348000)]
        [TestCase(1348100)]
        [TestCase(1348200)]
        [TestCase(1348300)]
        [TestCase(1348400)]
        [TestCase(1348500)]
        [TestCase(1348600)]
        [TestCase(1348700)]
        [TestCase(1348800)]
        [TestCase(1348900)]
        [TestCase(1349000)]
        [TestCase(1349100)]
        [TestCase(1349200)]
        [TestCase(1349300)]
        [TestCase(1349400)]
        [TestCase(1349500)]
        [TestCase(1349600)]
        [TestCase(144110)]
        [TestCase(144600)]
        [TestCase(144700)]
        [TestCase(144800)]
        [TestCase(144900)]
        [TestCase(1500100)]
        [TestCase(1500200)]
        [TestCase(1501200)]
        [TestCase(1501600)]
        [TestCase(1501800)]
        [TestCase(1600000)]
        [TestCase(1611800)]
        [TestCase(1611900)]
        [TestCase(1612000)]
        [TestCase(1613550)]
        [TestCase(164100)]
        [TestCase(1700000)]
        [TestCase(1700002)]
        [TestCase(1700100)]
        [TestCase(1700200)]
        [TestCase(1700300)]
        [TestCase(1700400)]
        [TestCase(1700500)]
        [TestCase(1700600)]
        [TestCase(1700700)]
        [TestCase(1700800)]
        [TestCase(1700900)]
        [TestCase(1701000)]
        [TestCase(1701100)]
        [TestCase(1701200)]
        [TestCase(1701300)]
        [TestCase(1701400)]
        [TestCase(1701500)]
        [TestCase(1701600)]
        [TestCase(1701700)]
        [TestCase(1701800)]
        [TestCase(1701900)]
        [TestCase(1702000)]
        [TestCase(1702100)]
        [TestCase(1702200)]
        [TestCase(1702300)]
        [TestCase(1702400)]
        [TestCase(1702500)]
        [TestCase(1702600)]
        [TestCase(1702700)]
        [TestCase(1702800)]
        [TestCase(1702900)]
        [TestCase(1703000)]
        [TestCase(1703100)]
        [TestCase(1703200)]
        [TestCase(1703300)]
        [TestCase(1703400)]
        [TestCase(1703500)]
        [TestCase(1703600)]
        [TestCase(1703700)]
        [TestCase(1703800)]
        [TestCase(1703900)]
        [TestCase(1704000)]
        [TestCase(1704100)]
        [TestCase(1704200)]
        [TestCase(1704300)]
        [TestCase(1704400)]
        [TestCase(1704500)]
        [TestCase(1704600)]
        [TestCase(1704700)]
        [TestCase(1704800)]
        [TestCase(1704900)]
        [TestCase(1705000)]
        [TestCase(1705100)]
        [TestCase(1705200)]
        [TestCase(1705300)]
        [TestCase(1705400)]
        [TestCase(1705500)]
        [TestCase(1705600)]
        [TestCase(1705700)]
        [TestCase(1705800)]
        [TestCase(1705900)]
        [TestCase(1706000)]
        [TestCase(1706100)]
        [TestCase(1706200)]
        [TestCase(1706300)]
        [TestCase(1706400)]
        [TestCase(1706500)]
        [TestCase(1706600)]
        [TestCase(1706700)]
        [TestCase(1706800)]
        [TestCase(1706900)]
        [TestCase(1707000)]
        [TestCase(1707100)]
        [TestCase(1707200)]
        [TestCase(1707300)]
        [TestCase(1707400)]
        [TestCase(1707500)]
        [TestCase(1707600)]
        [TestCase(1707700)]
        [TestCase(1707800)]
        [TestCase(1707900)]
        [TestCase(1708000)]
        [TestCase(1708100)]
        [TestCase(1708200)]
        [TestCase(1708300)]
        [TestCase(1708400)]
        [TestCase(1708500)]
        [TestCase(1708600)]
        [TestCase(1708700)]
        [TestCase(1708800)]
        [TestCase(1708900)]
        [TestCase(1709000)]
        [TestCase(1709100)]
        [TestCase(1709200)]
        [TestCase(1709300)]
        [TestCase(1709400)]
        [TestCase(1709500)]
        [TestCase(1709600)]
        [TestCase(1709700)]
        [TestCase(1709800)]
        [TestCase(1709900)]
        [TestCase(1710000)]
        [TestCase(1710100)]
        [TestCase(1800000)]
        [TestCase(1800100)]
        [TestCase(1800200)]
        [TestCase(1800300)]
        [TestCase(1800400)]
        [TestCase(1800500)]
        [TestCase(1800600)]
        [TestCase(1800700)]
        [TestCase(1800800)]
        [TestCase(1800900)]
        [TestCase(1801000)]
        [TestCase(1801100)]
        [TestCase(1801200)]
        [TestCase(1801300)]
        [TestCase(1801400)]
        [TestCase(1801500)]
        [TestCase(1801600)]
        [TestCase(1801700)]
        [TestCase(1801800)]
        [TestCase(1801900)]
        [TestCase(1802000)]
        [TestCase(1802100)]
        [TestCase(1802200)]
        [TestCase(1802300)]
        [TestCase(1802400)]
        [TestCase(1802500)]
        [TestCase(1802600)]
        [TestCase(1802700)]
        [TestCase(1802800)]
        [TestCase(1802900)]
        [TestCase(1803000)]
        [TestCase(1803100)]
        [TestCase(1803200)]
        [TestCase(1803300)]
        [TestCase(1803400)]
        [TestCase(1803500)]
        [TestCase(1803600)]
        [TestCase(1803700)]
        [TestCase(1803800)]
        [TestCase(1803900)]
        [TestCase(1804000)]
        [TestCase(1804100)]
        [TestCase(1804200)]
        [TestCase(1804300)]
        [TestCase(1804400)]
        [TestCase(1804500)]
        [TestCase(1804600)]
        [TestCase(1804700)]
        [TestCase(1804800)]
        [TestCase(1804900)]
        [TestCase(1805000)]
        [TestCase(1805100)]
        [TestCase(1805200)]
        [TestCase(1805300)]
        [TestCase(1805400)]
        [TestCase(1805500)]
        [TestCase(1805600)]
        [TestCase(1805700)]
        [TestCase(1805800)]
        [TestCase(1805900)]
        [TestCase(1806000)]
        [TestCase(1806100)]
        [TestCase(1806200)]
        [TestCase(24000)]
        [TestCase(24100)]
        [TestCase(24200)]
        [TestCase(24300)]
        [TestCase(24500)]
        [TestCase(322100)]
        [TestCase(322200)]
        [TestCase(402000)]
        [TestCase(404000)]
        [TestCase(405000)]
        [TestCase(405200)]
        [TestCase(405300)]
        [TestCase(405400)]
        [TestCase(405600)]
        [TestCase(405800)]
        [TestCase(406000)]
        [TestCase(406100)]
        [TestCase(406200)]
        [TestCase(406300)]
        [TestCase(406400)]
        [TestCase(406500)]
        [TestCase(406600)]
        [TestCase(406700)]
        [TestCase(406701)]
        [TestCase(406702)]
        [TestCase(406703)]
        [TestCase(406800)]
        [TestCase(406900)]
        [TestCase(407000)]
        [TestCase(407100)]
        [TestCase(407150)]
        [TestCase(407200)]
        [TestCase(407250)]
        [TestCase(407300)]
        [TestCase(407350)]
        [TestCase(407400)]
        [TestCase(407450)]
        [TestCase(407500)]
        [TestCase(407550)]
        [TestCase(407600)]
        [TestCase(407650)]
        [TestCase(407700)]
        [TestCase(407750)]
        [TestCase(407800)]
        [TestCase(407850)]
        [TestCase(407900)]
        [TestCase(407950)]
        [TestCase(408000)]
        [TestCase(408050)]
        [TestCase(408100)]
        [TestCase(408150)]
        [TestCase(408200)]
        [TestCase(408250)]
        [TestCase(408300)]
        [TestCase(408350)]
        [TestCase(408400)]
        [TestCase(408450)]
        [TestCase(408500)]
        [TestCase(408550)]
        [TestCase(408600)]
        [TestCase(408700)]
        [TestCase(410000)]
        [TestCase(410100)]
        [TestCase(410200)]
        [TestCase(410300)]
        [TestCase(425000)]
        [TestCase(425200)]
        [TestCase(425400)]
        [TestCase(425600)]
        [TestCase(425800)]
        [TestCase(426000)]
        [TestCase(426200)]
        [TestCase(426400)]
        [TestCase(426600)]
        [TestCase(426800)]
        [TestCase(444000)]
        [TestCase(444100)]
        [TestCase(444200)]
        [TestCase(444300)]
        [TestCase(444400)]
        [TestCase(444500)]
        [TestCase(444600)]
        [TestCase(444700)]
        [TestCase(444800)]
        [TestCase(444900)]
        [TestCase(445000)]
        [TestCase(445100)]
        [TestCase(445200)]
        [TestCase(445300)]
        [TestCase(445400)]
        [TestCase(445500)]
        [TestCase(445600)]
        [TestCase(450000)]
        [TestCase(450100)]
        [TestCase(450150)]
        [TestCase(450200)]
        [TestCase(450250)]
        [TestCase(450300)]
        [TestCase(450350)]
        [TestCase(450400)]
        [TestCase(450450)]
        [TestCase(450500)]
        [TestCase(450550)]
        [TestCase(450600)]
        [TestCase(450650)]
        [TestCase(450700)]
        [TestCase(450750)]
        [TestCase(450800)]
        [TestCase(450850)]
        [TestCase(450900)]
        [TestCase(450950)]
        [TestCase(451000)]
        [TestCase(452000)]
        [TestCase(452100)]
        [TestCase(452200)]
        [TestCase(452250)]
        [TestCase(452300)]
        [TestCase(452350)]
        [TestCase(452400)]
        [TestCase(452550)]
        [TestCase(452600)]
        [TestCase(452650)]
        [TestCase(452700)]
        [TestCase(452750)]
        [TestCase(452800)]
        [TestCase(452850)]
        [TestCase(452900)]
        [TestCase(452950)]
        [TestCase(453000)]
        [TestCase(454000)]
        [TestCase(454100)]
        [TestCase(454200)]
        [TestCase(454250)]
        [TestCase(454300)]
        [TestCase(454350)]
        [TestCase(454400)]
        [TestCase(454550)]
        [TestCase(454600)]
        [TestCase(454650)]
        [TestCase(454700)]
        [TestCase(454750)]
        [TestCase(454800)]
        [TestCase(454850)]
        [TestCase(454900)]
        [TestCase(454950)]
        [TestCase(455000)]
        [TestCase(455200)]
        [TestCase(455300)]
        [TestCase(455400)]
        [TestCase(455500)]
        [TestCase(455700)]
        [TestCase(455800)]
        [TestCase(456100)]
        [TestCase(456400)]
        [TestCase(456700)]
        [TestCase(457000)]
        [TestCase(457300)]
        [TestCase(457600)]
        [TestCase(457900)]
        [TestCase(458000)]
        [TestCase(458100)]
        [TestCase(458200)]
        [TestCase(5060000)]
        [TestCase(5060400)]
        [TestCase(5060800)]
        [TestCase(5070000)]
        [TestCase(5070200)]
        [TestCase(5070400)]
        [TestCase(5070600)]
        [TestCase(5070800)]
        [TestCase(5071000)]
        [TestCase(5071200)]
        [TestCase(5071400)]
        [TestCase(5071600)]
        [TestCase(524000)]
        [TestCase(565000)]
        [TestCase(584000)]
        [TestCase(584200)]
        [TestCase(584400)]
        [TestCase(584600)]
        [TestCase(584800)]
        [TestCase(586200)]
        [TestCase(590000)]
        [TestCase(590400)]
        [TestCase(590800)]
        [TestCase(591200)]
        [TestCase(591600)]
        [TestCase(592000)]
        [TestCase(592400)]
        [TestCase(592800)]
        [TestCase(593200)]
        [TestCase(593600)]
        [TestCase(594000)]
        [TestCase(594400)]
        [TestCase(594800)]
        [TestCase(595200)]
        [TestCase(595600)]
        [TestCase(596000)]
        [TestCase(596400)]
        [TestCase(596800)]
        [TestCase(600000)]
        [TestCase(600400)]
        [TestCase(600800)]
        [TestCase(601200)]
        [TestCase(601600)]
        [TestCase(602000)]
        [TestCase(602400)]
        [TestCase(602800)]
        [TestCase(605000)]
        [TestCase(605400)]
        [TestCase(605800)]
        [TestCase(606200)]
        [TestCase(606600)]
        [TestCase(607000)]
        [TestCase(607400)]
        [TestCase(607800)]
        [TestCase(608200)]
        [TestCase(608600)]
        [TestCase(609000)]
        [TestCase(609400)]
        [TestCase(610000)]
        [TestCase(610400)]
        [TestCase(610800)]
        [TestCase(611200)]
        [TestCase(611600)]
        [TestCase(612000)]
        [TestCase(612400)]
        [TestCase(612800)]
        [TestCase(614000)]
        [TestCase(614400)]
        [TestCase(614800)]
        [TestCase(615200)]
        [TestCase(615600)]
        [TestCase(616000)]
        [TestCase(616400)]
        [TestCase(616800)]
        [TestCase(617200)]
        [TestCase(617600)]
        [TestCase(618000)]
        [TestCase(618400)]
        [TestCase(65000)]
        [TestCase(65100)]
        [TestCase(820000)]
        [TestCase(820400)]
        [TestCase(820800)]
        [TestCase(821200)]
        [TestCase(821600)]
        [TestCase(822000)]
        [TestCase(822400)]
        [TestCase(822800)]
        [TestCase(823200)]
        [TestCase(823600)]
        [TestCase(824000)]
        [TestCase(824400)]
        [TestCase(824800)]
        [TestCase(825200)]
        [TestCase(825600)]
        [TestCase(826000)]
        [TestCase(826400)]
        [TestCase(826800)]
        [TestCase(827200)]
        [TestCase(827600)]
        [TestCase(880000)]
        [TestCase(880400)]
        [TestCase(880800)]
        [TestCase(881200)]
        [TestCase(881600)]
        [TestCase(882000)]
        [TestCase(882400)]
        [TestCase(882800)]
        [TestCase(883200)]
        [TestCase(883600)]
        [TestCase(884000)]
        [TestCase(884400)]
        [TestCase(972000)]
        [TestCase(972050)]
        [TestCase(972100)]
        [TestCase(972150)]
        [TestCase(972200)]
        [TestCase(972250)]
        [TestCase(972300)]
        [TestCase(972700)]
        [TestCase(972750)]
        [TestCase(972900)]
        [TestCase(972950)]
        [TestCase(973150)]
        [TestCase(973250)]
        [TestCase(973300)]
        [TestCase(973350)]
        [TestCase(973400)]
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
                Assert.AreEqual(count, ids.Count);
            }
        }

        #region TestCase
        [TestCase(2001)]
        [TestCase(2003)]
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

                Assert.AreEqual(count, ids.Count);
            }
        }

    }
}