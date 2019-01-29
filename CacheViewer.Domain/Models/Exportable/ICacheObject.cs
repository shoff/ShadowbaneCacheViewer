namespace CacheViewer.Domain.Models.Exportable
{
    using System;
    using Archive;
    using Data;

    public interface ICacheObject : IComparable<ICacheObject>
    {
        CacheIndex CacheIndex { get; }
        
        uint RenderId { get; }
        
        string Name { get; }
        
        ObjectType Flag { get; }
        
        int CursorOffset { get; }
        
        ArraySegment<byte> Data { get; }
        
        int InnerOffset { get; }
        
        void Parse(ArraySegment<byte> data);
    }
}