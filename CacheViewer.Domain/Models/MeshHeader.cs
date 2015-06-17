
using SlimDX;


namespace CacheViewer.Domain.Models
{
    public struct MeshHeader
    {
        public uint null1;          // 4
        public double unixUpdatedTimeStamp;   // 8
        public double unk3;  // 
        public double unixCreatedTimeStamp;
        public double unk5;
        public Vector3 min;
        public Vector3 max;
        public ushort null2;
    };
}