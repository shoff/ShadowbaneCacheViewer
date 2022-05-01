namespace Shadowbane.Cache.IO;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

// TODO this is simply a brainstorming placeholder, do not use.
public class CacheWriter
{
    public void WriteNewHeader(CacheArchive cacheArchive, CacheHeader cacheHeader)
    {
        // DO NOT USE THIS PLACE HOLDER ONLY 
        using var stream = new FileStream(cacheArchive.Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        using var writer = new BinaryWriter(stream, Encoding.UTF32);
        writer.Write(cacheHeader.indexCount);
        writer.Write(cacheHeader.dataOffset);
        writer.Write(cacheHeader.fileSize);
        writer.Write(cacheHeader.junk1);
    }

    public void WriteNewIndex(CacheArchive cacheArchive, IEnumerable<CacheIndex> cacheIndices)
    {
        // DO NOT USE THIS PLACE HOLDER ONLY 
        using var stream = new FileStream(cacheArchive.Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        using var writer = new BinaryWriter(stream, Encoding.UTF32);
        writer.BaseStream.Position = cacheArchive.DataOffset;
        var enumerable = cacheIndices as CacheIndex[] ?? cacheIndices.ToArray();
        for (int i = 0; i < enumerable.Count(); i++)
        {
            writer.Write(enumerable[i].junk1);
            writer.Write(enumerable[i].identity);
            writer.Write(enumerable[i].offset);
            writer.Write(enumerable[i].unCompressedSize);
            writer.Write(enumerable[i].compressedSize);
        }
    }
}