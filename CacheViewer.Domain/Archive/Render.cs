namespace CacheViewer.Domain.Archive
{
    using System;

    [CacheFile(CacheFile.Render)]
    internal sealed class Render : CacheArchive
    {
        public Render()
            : base("Render.cache")
        {
        }

        public ArraySegment<byte> Data => this.bufferData;

        public ArraySegment<byte> Unzip(uint uncompressedSize, byte[] file)
        {
            return this.Decompress(uncompressedSize, file);
        }
    }
}