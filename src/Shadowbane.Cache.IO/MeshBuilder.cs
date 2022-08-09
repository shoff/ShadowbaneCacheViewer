namespace Shadowbane.Cache.IO;

using System;
using System.Runtime.InteropServices;
using Exporter.File;
using Models;
using Index = Cache.Index;

public class MeshBuilder
{
    private const int MAX_VERTS = 1000000;
    private const int MAX_NORMALS = 1000000;
    private const int MAX_TEXTURES = 1000000;
    private const int MAX_INDICES = 1000000;

    public Mesh SaveRawMeshData(ReadOnlyMemory<byte> buffer, uint identity)
    {
        var asset = ArchiveLoader.MeshArchive[identity];
        FileWriter.Writer.Write(asset.Asset.Span, CacheLocation.MeshOutputFolder.FullName, $"{identity}.sbmesh");
        return this.Build(buffer, identity);
    }

    public Mesh Build(ReadOnlyMemory<byte> buffer, uint identity)
    {
        var mesh = new Mesh(identity);
        var headerSize = Marshal.SizeOf<MeshHeader>();
        mesh.Header = buffer.Slice(0, headerSize).Span.ByteArrayToStructure<MeshHeader>();
        mesh.SetBounds();

        using var reader = buffer.CreateBinaryReaderUtf32(46);

        // verts
        var numberOfVerts = reader.ReadInt32();
        if (numberOfVerts > MAX_VERTS)
        {
            throw new InvalidMeshException($"{numberOfVerts} exceeds the sane MAX_VERTS {MAX_VERTS} count.");
        }
        for (int i = 0; i < numberOfVerts; i++)
        {
            mesh.Vertices.Add(reader.ToVector3());
        }

        // normals
        var numberOfNormals = reader.ReadInt32();
        if (numberOfNormals > MAX_NORMALS)
        {
            throw new InvalidMeshException($"{numberOfNormals} exceeds the sane MAX_NORMALS {MAX_NORMALS} count.");
        }
        for (int i = 0; i < numberOfNormals; i++)
        {
            mesh.Normals.Add(reader.ToVector3());
        }
            
        // textures
        var numberOfTextures = reader.ReadInt32();
        if (numberOfTextures > MAX_TEXTURES)
        {
            throw new InvalidMeshException($"{numberOfTextures} exceeds the sane MAX_TEXTURES {MAX_TEXTURES} count.");
        }
        for (int i = 0; i < numberOfTextures; i++)
        {
            mesh.TextureVectors.Add(reader.ToVector2());
        }

        // indices
        var numberOfIndices = reader.ReadUInt32();
        mesh.NumberOfIndices = numberOfIndices;
        if (numberOfIndices > MAX_INDICES)
        {
            throw new InvalidMeshException($"{numberOfIndices} exceeds the sane MAX_INDICES {MAX_INDICES} count.");
        }

        for (var i = 0; i < numberOfIndices; i += 3)
        {
            mesh.Indices.Add(new Index(reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16()));
        }
        return mesh;
    }
}