namespace Shadowbane.Cache
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualBasic.CompilerServices;

    public interface ICacheObject : IComparable<ICacheObject>
    {
        CacheIndex CacheIndex { get; }
        uint RenderId { get; }
        string Name { get; }
        ObjectType Flag { get; }
        int CursorOffset { get; }
        ArraySegment<byte> Data { get; }
        int InnerOffset { get; }
        int RenderCount { get; set; }
        ICollection<int> RenderIds { get; set; }
        ICollection<RenderInformation> Renders { get; set; }
        void Parse();
    }
}