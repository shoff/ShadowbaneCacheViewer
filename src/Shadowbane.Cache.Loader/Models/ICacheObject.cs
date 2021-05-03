namespace Shadowbane.Cache.IO.Models
{
    using System;
    using System.Collections.Generic;

    public interface ICacheObject : IComparable<ICacheObject>
    {
        CacheIndex CacheIndex { get; }
        uint RenderId { get; }
        string Name { get; }
        ObjectType Flag { get; }
        uint CursorOffset { get; }
        ReadOnlyMemory<byte> Data { get; }
        uint InnerOffset { get; }
        uint RenderCount { get; set; }
        ICollection<uint> RenderIds { get; set; }
        ICollection<RenderInformation> Renders { get; set; }
        void Parse();
    }
}