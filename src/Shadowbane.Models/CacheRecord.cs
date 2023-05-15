namespace Shadowbane.Models;

using System;
using System.Collections.Generic;
using Cache;

public abstract record CacheRecord(uint Identity, ObjectType Flag, string Name, uint CursorOffset, ReadOnlyMemory<byte> Data)
    : ICacheRecord
{
    public ICollection<uint> RenderIds { get; } = new HashSet<uint>();
    public ICollection<IRenderable> Renders { get; } = new HashSet<IRenderable>();
    public IDictionary<uint, uint> InvalidRenderIds => new Dictionary<uint, uint>();
    public uint RenderId { get; protected set; }
    public string Name { get; protected set; } = Name;
    public CacheIndex CacheIndex { get; set; }
    public uint RenderCount { get; set; }
    public abstract void Parse();

    public void RecordInvalidRenderId(uint renderId)
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
}