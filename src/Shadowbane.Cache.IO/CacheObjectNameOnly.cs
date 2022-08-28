using System;
using System.Collections.Generic;

namespace Shadowbane.Cache.IO;

public record CacheObjectNameOnly : ICacheObject
{
    public int CompareTo(ICacheObject? other)
    {
        return ((CacheObjectNameOnly)other).CompareTo(this);
    }

    public uint Identity { get; set; }
    public uint RenderId { get; set; }
    public string Name { get; set; }
    public ObjectType Flag { get; set; }
    public uint CursorOffset { get; } = 0;
    public ReadOnlyMemory<byte> Data { get; } = null;
    public uint InnerOffset { get; } = 0;
    public uint RenderCount { get; set; }
    public ICollection<uint> RenderIds { get; } = new List<uint>();
    public ICollection<IRenderable> Renders { get; } = new List<IRenderable>();
    public ICollection<uint> InvalidRenderIds { get; } = new List<uint>();
    public ICacheObject Parse()
    {
        return new CacheObjectNameOnly(this);
    }
}