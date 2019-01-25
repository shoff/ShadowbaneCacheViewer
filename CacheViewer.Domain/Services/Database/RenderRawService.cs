namespace CacheViewer.Domain.Services.Database
{
    using System.Threading.Tasks;
    using Archive;
    using Data;
    using Extensions;

    public class RenderRawService
    {
        private readonly Render renderArchive;

        public RenderRawService()
        {
            this.renderArchive = new Render();
            this.renderArchive.LoadCacheHeader();
            this.renderArchive.LoadIndexes();
        }

        public async Task SaveRendersToDbAsync()
        {
            using (var context = new SbCacheViewerContext())
            {
                foreach (var c in this.renderArchive.CacheIndices)
                {
                    context.Renders.Add(
                        new Data.Entities.RenderRaw
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
                    await context.SaveChangesAsync();
                }
            }
        }

        public byte[] GetData(CacheIndex index)
        {
            using (var reader = this.renderArchive.Data.CreateBinaryReaderUtf32())
            {
                // ReSharper disable ExceptionNotDocumented
                reader.BaseStream.Position = index.Offset;
                var buffer = reader.ReadBytes((int)index.CompressedSize);
                var item1 = this.renderArchive.Unzip(index.UnCompressedSize, buffer);
                return item1.Array;
            }
        }

    }
}