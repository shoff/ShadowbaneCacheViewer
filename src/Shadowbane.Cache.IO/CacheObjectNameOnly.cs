using System;
using System.Collections.Generic;

namespace Shadowbane.Cache.IO;

public record CacheObjectNameOnly : ICacheObject
{
    private readonly object syncRoot = new();
    public int CompareTo(ICacheObject? other)
    {
        if (other == null || this.Flag > other.Flag)
        {
            return 1;
        }

        if (other.Flag == this.Flag)
        {
            return string.Compare(this.Name, other.Name, StringComparison.Ordinal);
        }

        return -1;
    }

    public uint Identity { get; set; }
    public uint RenderId { get; set; }
    public string Name { get; set; }
    public ObjectType Flag { get; set; }
    public uint CursorOffset { get; } = 0;
    public ReadOnlyMemory<byte> Data { get; } = null;
    public uint RenderCount { get; set; }
    public ICollection<uint> RenderIds { get; } = new List<uint>();
    public ICollection<IRenderable> Renders { get; } = new List<IRenderable>();
    public IDictionary<uint, uint> InvalidRenderIds { get; } = new Dictionary<uint, uint>();
    public void Parse()
    {
    }
    public void RecordInvalidRenderId(uint renderId)
    {
        lock (this.syncRoot)
        {
            if (this.InvalidRenderIds.ContainsKey(renderId))
            {
                this.InvalidRenderIds[renderId]++;
            }
            else
            {
                this.InvalidRenderIds.Add(renderId, 1);
            }
        }
    }
}