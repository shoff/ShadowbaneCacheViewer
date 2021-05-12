namespace Shadowbane.Cache.Exporter.File.Json
{
    using System;
    using System.Collections.Generic;
    using Models;

    public class JsonCacheObject
    {
        public uint Identity { get; set; }
        public ICollection<uint> RenderIds { get; set; } = new HashSet<uint>();
        public ICollection<JsonRenderInformation> Renders { get; set; } = new HashSet<JsonRenderInformation>();
        public int UnParsedBytes { get; set; }
        public uint RenderId { get; set; }
        public string Name { get; set; }
        public ObjectType Flag { get; set; }
        public uint CursorOffset { get; set; }
        public string Data { get; set; }
        public uint InnerOffset { get; set; }
        public uint RenderCount { get; set; }
    }
    public class JsonVector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
    public class JsonVector2
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
    public class JsonRenderInformation
    {
        public uint IUNK { get; set; }
        public uint RenderType { get; set; }
        public uint FirstInt { get; set; }
        public ushort FirstUshort { get; set; }
        public DateTime? CreateDate { get; set; }
        public uint UnknownIntOne { get; set; }
        public uint UnknownIntTwo { get; set; }
        public uint UnknownCounterOrBool { get; set; }
        public int ByteCount { get; set; }
        public bool HasMesh { get; set; }
        public JsonMesh Mesh { get; set; }
        public object[] Unknown { get; set; }
        public int MeshId { get; set; }
        public bool ValidMeshFound { get; set; }
        public string JointName { get; set; }
        public JsonVector3 Scale { get; set; }
        public JsonVector3 Position { get; set; }
        public int RenderCount { get; set; }
        public CacheIndex[] ChildRenderIds { get; set; }
        public bool HasTexture { get; set; }
        public uint TextureCount { get; set; }
        public ICollection<JsonTexture> Textures { get; set; } = new HashSet<JsonTexture>();
        public JsonVector2 TextureVector { get; set; }
        public CacheIndex CacheIndex { get; set; }
        public string Notes { get; set; }
        public JsonRenderInformation Render { get; set; }
        public int ChildCount { get; set; }
        public List<int> ChildRenderIdList { get; set; } = new List<int>();
        public List<JsonRenderInformation> Children { get; set; } = new List<JsonRenderInformation>();
        public CacheAsset BinaryAsset { get; set; }
        public byte B34 { get; set; }
        public byte B11 { get; set; }
        public long LastOffset { get; set; }
        public long UnreadByteCount { get; set; }
        public uint JointNameSize { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsValid { get; set; } = true;
        public uint Identity { get; set; }
    }
    public class JsonTexture
    {
        public uint TextureId { get; set; }
        public CacheIndex CacheIndexIdentity { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
    }
    public class JsonIndex
    {
        public ushort Position { get; set; }
        public ushort Normal { get; set; }
        public ushort TextureCoordinate { get; set; }
    }
    public class JsonMesh
    {
        public uint Identity { get; set; }
        public uint VertexCount { get; set; }
        public uint VertexBufferSize { get; set; }
        public MeshHeader Header { get; set; }
        public IList<JsonTexture> Textures { get; set; } = new List<JsonTexture>();
        public ICollection<JsonVector3> Vertices { get; set; } = new List<JsonVector3>();
        public ulong VerticesOffset { get; set; }
        public ICollection<JsonVector3> Normals { get; set; } = new List<JsonVector3>();
        public ICollection<JsonVector2> TextureVectors { get; set; } = new List<JsonVector2>();
        public ulong IndicesOffset { get; set; }
        public IList<JsonIndex> Indices { get; set; } = new List<JsonIndex>();
        public ulong NormalsOffset { get; set; }
        public uint NormalsCount { get; set; }
        public uint NormalsBufferSize { get; set; }
        public ulong TextureOffset { get; set; }
        public uint TextureCoordinatesCount { get; set; }
        public byte[] UnknownData { get; set; }
        public long OffsetToUnknownData { get; set; }
        public int NumberOfIndices { get; set; }
        public JsonVector3 Scale { get; set; }
        public JsonVector3 Position { get; set; }
        public JsonVector3[] Bounds { get; set; }
        public float MeshSize { get; set; }
    }

    public static class ReadOnlyMemoryExtensions
    {
        public static string ToHexString(this ReadOnlyMemory<byte> segment, string prefix)
        {
            char[] lookup = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            //if (segment.Span == null)
            //{
            //    return "";
            //}

            int i = 0, p = prefix.Length, l = segment.Span.Length;
            char[] c = new char[l * 2 + p];
            byte d;
            for (; i < p; ++i)
            {
                c[i] = prefix[i];
            }

            i = -1;
            --l;
            --p;
            while (i < l)
            {
                d = segment.Span[++i];
                c[++p] = lookup[d >> 4];
                c[++p] = lookup[d & 0xF];
            }
            return new string(c, 0, c.Length);
        }
    }
}