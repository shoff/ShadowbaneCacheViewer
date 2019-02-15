namespace CacheViewer.Domain.Models
{
    using Geometry;

    public struct MeshHeader
    {

        //50 byte header
        // 46th byte of the header is the vertex count
        // following the header (immediately after the vertex count
        // is VertexCount * 4 (float) * 3 (Vector)
        // At the end of this chunk is another count (which always seems to be the same as the one in the header)

        public uint null1; // 4
        public double unixUpdatedTimeStamp; // 8
        public double unk3; // 
        public double unixCreatedTimeStamp;
        public double unk5;
        public Vector3 min;
        public Vector3 max;
        public ushort null2;
    }
}