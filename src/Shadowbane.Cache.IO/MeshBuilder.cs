namespace Shadowbane.Cache.IO
{
    using System;
    using System.Buffers.Text;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Geometry;
    using Models;

    public static class MeshBuilder
    {
        public static Mesh Create(uint identity)
        {
            var cacheIndex = ArchiveLoader.MeshArchive[identity];
            var buffer = cacheIndex.Asset.Span;
            var mesh = new Mesh();
            var headerSize = Marshal.SizeOf<MeshHeader>();
            mesh.Header = buffer.Slice(0, headerSize).ByteArrayToStructure<MeshHeader>();
            mesh.SetBounds();
            
            //// YUCK!
            //using var reader = new ReadOnlyMemory<byte>
            //    (buffer.Slice(46).ToArray()).CreateBinaryReaderUtf32(0);
            var ptr = 46;
            Utf8Parser.TryParse(buffer.Slice(ptr), out int nVerts, out var _);
            ptr += 4;

            var floatSize = Marshal.SizeOf<float>();
            var vBufSize = nVerts * floatSize * 3;
            mesh.Vertices = new List<Vector3>(nVerts);
            var preVerts = ptr;

            for (int i = 0; i < nVerts; i++)
            {
                Utf8Parser.TryParse(buffer.Slice(ptr), out float x, out var _);
                ptr += floatSize;
                Utf8Parser.TryParse(buffer.Slice(ptr), out float y, out var _);
                ptr += floatSize;
                Utf8Parser.TryParse(buffer.Slice(ptr), out float z, out var _);
                ptr += floatSize;
                mesh.Vertices.Add(new Vector3(x, y, z));
            }

            Debug.Assert(preVerts + vBufSize == ptr);

            // normals
            Utf8Parser.TryParse(buffer.Slice(ptr), out int nNormals, out var _);
            ptr += 4;
            
            var normalsBufSize = nNormals * floatSize * 3;
            mesh.Normals = new List<Vector3>(normalsBufSize);
            var preNormals = ptr;

            for (int i = 0; i < normalsBufSize; i++)
            {
                Utf8Parser.TryParse(buffer.Slice(ptr), out float x, out var _);
                ptr += floatSize;
                Utf8Parser.TryParse(buffer.Slice(ptr), out float y, out var _);
                ptr += floatSize;
                Utf8Parser.TryParse(buffer.Slice(ptr), out float z, out var _);
                ptr += floatSize;
                mesh.Normals.Add(new Vector3(x, y, z));
            }
            Debug.Assert(preNormals + normalsBufSize == ptr);

            // textures
            Utf8Parser.TryParse(buffer.Slice(ptr), out int nTextures, out var _);
            ptr += 4;

            var tBufSize = nNormals * floatSize * 2;
            var preTextures = ptr;
            mesh.TextureVectors = new List<Vector2>();

            for (int i = 0; i < tBufSize; i++)
            {
                Utf8Parser.TryParse(buffer.Slice(ptr), out float x, out var _);
                ptr += floatSize;
                Utf8Parser.TryParse(buffer.Slice(ptr), out float y, out var _);
                ptr += floatSize;
                mesh.TextureVectors.Add(new Vector2(x, y));
            }

            Debug.Assert(preTextures + tBufSize == ptr);

            // indices
            Utf8Parser.TryParse(buffer.Slice(ptr), out uint nIndices, out var _);
            ptr += 4;

            mesh.NumberOfIndices = nIndices;
            mesh.Indices = new List<Models.Index>();

            var uint16Size = Marshal.SizeOf<UInt16>();
            for (var i = 0; i < mesh.NumberOfIndices; i += 3)
            {
                Utf8Parser.TryParse(buffer.Slice(ptr), out UInt16 position, out var _);
                ptr += uint16Size;
                Utf8Parser.TryParse(buffer.Slice(ptr), out UInt16 textureCoordinates, out var _);
                ptr += uint16Size;
                Utf8Parser.TryParse(buffer.Slice(ptr), out UInt16 normal, out var _);
                ptr += uint16Size;
                mesh.Indices.Add(new Models.Index(position, textureCoordinates, normal));
            }

            return mesh;
        }


    }
}