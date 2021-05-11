namespace Shadowbane.Models
{
    using System;
    using System.Collections.Generic;
    using Cache;

    public interface ICacheObject : IComparable<ICacheObject>
    {
        uint Identity { get; }
        uint RenderId { get; }
        string Name { get; }
        ObjectType Flag { get; }
        uint CursorOffset { get; }
        ReadOnlyMemory<byte> Data { get; }
        uint InnerOffset { get; }
        uint RenderCount { get; set; }
        ICollection<uint> RenderIds { get; }
        ICollection<RenderInformation> Renders { get; }
        ICacheObject Parse();
    }
}