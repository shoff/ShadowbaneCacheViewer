using System;
using CacheViewer.Domain.Archive;
using CacheViewer.Domain.Extensions;
using CacheViewer.Domain.Factories;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Models
{
    [TestFixture]
    public class InteractiveTests
    {
        private CacheObjectFactory cacheObjectFactory;
        private CObjects cobjects;
        private Render renderArchive;


        [SetUp]
        public void SetUp()
        {
            this.renderArchive = (Render)ArchiveFactory.Instance.Build(CacheFile.Render);
            this.cobjects = (CObjects) ArchiveFactory.Instance.Build(CacheFile.CObjects);
            this.cacheObjectFactory = CacheObjectFactory.Instance;
        }

        [Test]
        public void Parse_Should_Correctly_Identify_Data_For_CacheIndex_24000()
        {
            //var cacheIndex = this.cacheObjectFactory.Indexes.First(x => x.identity == 24000);
            //var treeOfLife = this.cacheObjectFactory.Create(cacheIndex);
            var treeOfLife = this.cobjects[24000];
            var length = treeOfLife.Item1.Count;
            Find_Where_The_RenderId_Is_Hiding(24000, treeOfLife.Item1);
        }


        [Test]
        public void Find_Where_The_RenderId_Is_Hiding(int cacheItemId, ArraySegment<byte> item)
        {
            int count = item.Count;
            using (var reader = item.CreateBinaryReaderUtf32())
            {
                for (int i = 25; i < count - 4; i++)
                {
                    reader.BaseStream.Position = i;

                    int id = reader.ReadInt32();

                    if (TestRange(id, 1000, 77000300))
                    {
                        if (Found(id))
                        {
                            Console.WriteLine("Found matching renderId at position {0}, id is {1}", i, id);
                        }
                    }
                }
            }
        }

        private bool Found(int id)
        {
            var renderId = renderArchive[id];
            if (renderId.CacheIndex1.identity > 0)
            {
                return true;
            }
            return false;
        }

        private bool TestRange(int numberToCheck, int bottom, int top)
        {
            return (numberToCheck > bottom && numberToCheck < top);
        }

    }
}