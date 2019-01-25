namespace CacheViewer.Domain.Services.Database
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Archive;
    using Data;
    using EntityFramework.BulkInsert.Extensions;
    using Extensions;
    using CObjectArchive = CacheViewer.Domain.Archive.CObjects;

    public class RawCobjectEventArgs : EventArgs
    {
        public int Count { get; set; }
    }

    public class CObjectsRawService
    {
        private readonly CObjectArchive cobjects;
        public event EventHandler<RawCobjectEventArgs> RawCobjectsSaved;
        public CObjectsRawService()
        {
            this.cobjects = new CObjectArchive();
            this.cobjects.LoadCacheHeader();
            this.cobjects.LoadIndexes();
        }

        public async Task SaveCObjectsToDbAsync()
        {
            int count = 0;
            List<Data.Entities.CObjects> entities = new List<Data.Entities.CObjects>();

            foreach (var c in cobjects.CacheIndices)
            {
                entities.Add(
                    new Data.Entities.CObjects
                    {
                        CompressedSize = (int)c.CompressedSize,
                        Data = this.GetData(c),
                        Identity = (int)c.Identity,
                        Junk1 = (int)c.Junk1,
                        Name = c.Name,
                        Offset = (int)c.Offset,
                        UnCompressedSize = (int)c.UnCompressedSize,
                        Order = c.Order
                    });
                count++;

                if (count == 20)
                {
                    count = 0;
                    var eventArgs = new RawCobjectEventArgs
                    {
                        Count = entities.Count
                    };
                    this.RawCobjectsSaved.Raise(this, eventArgs);
                }
            }

            using (var context = new SbCacheViewerContext())
            {
                context.ExecuteCommand("delete from dbo.CObjects");
                context.ExecuteCommand("DBCC CHECKIDENT ('CObjects', RESEED, 1)");
                await context.BulkInsertAsync(entities);
            }

        }

        public byte[] GetData(CacheIndex index)
        {
            using (var reader = this.cobjects.Data.CreateBinaryReaderUtf32())
            {
                // ReSharper disable ExceptionNotDocumented
                reader.BaseStream.Position = index.Offset;
                var buffer = reader.ReadBytes((int)index.CompressedSize);
                var item1 = this.cobjects.Unzip(index.UnCompressedSize, buffer);
                return item1.Array;
            }
        }

    }
}