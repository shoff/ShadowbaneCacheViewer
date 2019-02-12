namespace CacheViewer.Tests.Domain.Models
{
    using System;
    using CacheViewer.Domain;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Factories;
    using Xunit;

    public class InteractiveTests
    {
        private CacheObjectFactory cacheObjectFactory;
        private readonly CObjects cobjects;
        private readonly Render renderArchive;

        public InteractiveTests()
        {
            this.renderArchive = (Render) ArchiveFactory.Instance.Build(CacheFile.Render);
            this.cobjects = (CObjects) ArchiveFactory.Instance.Build(CacheFile.CObjects);
            this.cacheObjectFactory = CacheObjectFactory.Instance;
        }

        private void Find_Where_The_RenderId_Is_Hiding(int cacheItemId, ArraySegment<byte> item)
        {
            var count = item.Count;
            using (var reader = item.CreateBinaryReaderUtf32())
            {
                for (var i = 25; i < count - 4; i++)
                {
                    reader.BaseStream.Position = i;

                    var id = reader.ReadInt32();

                    if (this.TestRange(id) && this.renderArchive.Contains(id))
                    {
                        if (this.Found(id))
                        {
                            Console.WriteLine(DomainMessages.MatchingRenderIdFound, i, id);
                        }
                    }
                }
            }
        }

        private bool Found(int id)
        {
            var renderId = this.renderArchive[id];
            if (renderId.CacheIndex1.Identity > 0)
            {
                return true;
            }

            return false;
        }

        private bool TestRange(int numberToCheck)
        {
            return numberToCheck >= this.renderArchive.LowestId
                && numberToCheck <= this.renderArchive.HighestId;
        }

        [Fact]
        public void Parse_Should_Correctly_Identify_Data_For_CacheIndex_24000()
        {
            //var cacheIndex = this.cacheObjectFactory.Indexes.First(x => x.identity == 24000);
            //var treeOfLife = this.cacheObjectFactory.CreateAndParse(cacheIndex);
            var treeOfLife = this.cobjects[24000];
            var length = treeOfLife.Item1.Count;
            this.Find_Where_The_RenderId_Is_Hiding(24000, treeOfLife.Item1);
        }
    }
}