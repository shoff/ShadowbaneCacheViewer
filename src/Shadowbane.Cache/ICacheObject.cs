namespace Shadowbane.Cache;

using System;
using System.Collections.Generic;

public interface ICacheObject : IComparable<ICacheObject>
{
    uint Identity { get; }
    uint RenderId { get; }
    string Name { get; }
    ObjectType Flag { get; }
    uint CursorOffset { get; }
    ReadOnlyMemory<byte> Data { get; }
    uint RenderCount { get; }
    ICollection<uint> RenderIds { get; }
    ICollection<IRenderable> Renders { get; }
    IDictionary<uint, uint> InvalidRenderIds { get; }
    void Parse();
    void RecordInvalidRenderId(uint renderId);
}