namespace Shadowbane.Models;

using System;
using System.Collections.Generic;
using Cache;

public abstract record ModelRecord(uint Identity, ObjectType Flag, string Name, uint CursorOffset, ReadOnlyMemory<byte> Data)
    : ICacheRecord
{
    private readonly object syncRoot = new();
    // sort by object type then name
    public int CompareTo(ICacheRecord? other)
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

    public CacheIndex CacheIndex { get; set; }
    public uint RenderId { get; set; }

    public uint RenderCount => (uint)this.RenderIds.Count;

    public ICollection<uint> RenderIds { get; } = new List<uint>();
    public ICollection<IRenderable> Renders { get; } = new List<IRenderable>();
    public IDictionary<uint, uint> InvalidRenderIds { get; } = new Dictionary<uint, uint>();
    public abstract void Parse();
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