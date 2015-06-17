

namespace CacheViewer.Tests.Domain.Archive
{
    using System;
    using System.Linq;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Factories;
    using NUnit.Framework;

    
    [TestFixture]
    public class CObjectsTests
    {
        
        private readonly CObjects cobjects = (CObjects)ArchiveFactory.Instance.Build(CacheFile.CObjects);

        
        [Test]
        public void CachHeader_Should_Contain_Correct_Values()
        {
            Assert.AreEqual(212376, this.cobjects.CacheHeader.dataOffset);
            Assert.AreEqual(5211072, this.cobjects.CacheHeader.fileSize);
            Assert.AreEqual(10618, this.cobjects.CacheHeader.indexCount);
        }

        
        [Test]
        public void LoadIndexes_Should_Load_The_Corrent_Number()
        {

            var expected = this.cobjects.CacheHeader.indexCount;
            var actual = this.cobjects.CacheIndices.Count;
            Assert.AreEqual(expected, actual);
        }

        
        [Test]
        public void All_Indexes_Have_Unique_Identity()
        {
            this.cobjects.CacheOnIndexLoad = true;
            var actual = this.cobjects.CacheIndices.Distinct().Count();
            Assert.AreEqual(actual, this.cobjects.CacheHeader.indexCount);
        }

        
        [Test, Explicit]
        public async void SaveToFile_Should_Output_All_Assets()
        {
            this.cobjects.CacheOnIndexLoad = true;
            foreach (var item in this.cobjects.CacheIndices)
            {
                await this.cobjects.SaveToFile(item, AppDomain.CurrentDomain.BaseDirectory + "\\CObjects\\");
            }
        }

        // [Test]
        // public void Find_Where_The_RenderId_Is_Hiding()
        // {
        // int cacheItemId = 13752;
        // var item = this.cobjects[cacheItemId];
        // var cacheIndex = item.CacheIndex1;
        // long ptr = 69;
        // bool found = false;
        // int count = item.Item1.Count;

        // using (var reader = item.Item1.CreateBinaryReaderUtf32())
        // {
        // for (int i = 25; i < count - 4; i++)
        // {
        // reader.BaseStream.Position = i;

        // int id = reader.ReadInt32();

        // //if ((id > 200) && (id < 77000300))
        // if (TestRange(id, 1000, 77000300))
        // {
        // if (Found(id))
        // {
        // Console.WriteLine("Found matching renderId at position {0}, id is {1}", i, id);
        // //break;
        // }
        // }
        // }
        // }
        // }
        // found the following for 585000
        // Found matching renderId at position 721, id is 585002
        // Found matching renderId at position 734, id is 585003
        // Found matching renderId at position 747, id is 585004
        // Found matching renderId at position 760, id is 585005
        // Found matching renderId at position 773, id is 585009
        // Found matching renderId at position 786, id is 585010
        // Found matching renderId at position 799, id is 585011
        // Found matching renderId at position 812, id is 585014

        // private readonly DataContext dc = new DataContext();
        // private bool Found(int id)
        // {
        // var render = dc.RenderEntities.FirstOrDefault(x => x.CacheIndexIdentity == id);
        // if (render != null)
        // {
        // return true;
        // }
        // return false;
        // }

        
        private bool TestRange(int numberToCheck, int bottom, int top)
        {
            return numberToCheck > bottom && numberToCheck < top;
        }

    }
}