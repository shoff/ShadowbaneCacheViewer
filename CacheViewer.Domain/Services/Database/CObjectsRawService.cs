namespace CacheViewer.Domain.Services.Database
{
    using System;
    using System.Threading.Tasks;
    using Archive;
    using Data;
    using Extensions;
    using CObjectArchive = CacheViewer.Domain.Archive.CObjects;

    public class CObjectsRawService
    {
        private CObjectArchive cobjects;

        public CObjectsRawService()
        {
            this.cobjects = new CObjectArchive();
            this.cobjects.LoadCacheHeader();
            this.cobjects.LoadIndexes();
        }

        public async Task SaveCObjectsToDbAsync()
        {
            using (var context = new DataContext())
            {
                foreach (var c in cobjects.CacheIndices)
                {
                    context.CObjects.Add(
                        new Data.Entities.CObjects
                        {
                            CompressedSize = (int) c.CompressedSize,
                            Data = this.GetData(c),
                            Identity = (int) c.Identity,
                            Junk1 = (int) c.Junk1,
                            Name = c.Name,
                            Offset = (int) c.Offset,
                            UnCompressedSize = (int) c.UnCompressedSize,
                            Order = c.Order
                        });
                  await  context.SaveChangesAsync();
                }
            }
        }

        public byte[] GetData(CacheIndex index)
        {
            using (var reader = this.cobjects.Data.CreateBinaryReaderUtf32())
            {
                // ReSharper disable ExceptionNotDocumented
                reader.BaseStream.Position = index.Offset;
                var buffer = reader.ReadBytes((int) index.CompressedSize);
                var item1 = this.cobjects.Unzip(index.UnCompressedSize, buffer);
                return item1.Array;
            }
        }

    }
}