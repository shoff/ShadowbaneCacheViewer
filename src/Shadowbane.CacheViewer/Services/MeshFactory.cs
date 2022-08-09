namespace Shadowbane.CacheViewer.Services;

using System.Diagnostics;
using Cache;
using Cache.Exporter.File;
using Cache.IO;
using Exceptions;
using Models;
using Serilog;
using IndexNotFoundException = Cache.IndexNotFoundException;

public class MeshFactory : IMeshFactory
{
    private readonly ILogger logger;

    public MeshFactory(ILogger logger)
    {
        this.logger = logger;
    }

    public async Task SaveToFile(CacheIndex index, string name, string path)
    {
        await ArchiveLoader.MeshArchive.SaveToFileAsync(index, name, path);
    }

    public Tuple<uint, uint> IdentityRange => new(ArchiveLoader.MeshArchive.LowestId, ArchiveLoader.MeshArchive.HighestId);

    public Mesh Create(CacheIndex cacheIndex)
    {
        var mesh = new Mesh(cacheIndex.identity);

        var cacheAsset = ArchiveLoader.MeshArchive[cacheIndex.identity];

        using (var reader = cacheAsset?.Asset.CreateBinaryReaderUtf32())
        {
            mesh.Header = new MeshHeader
            {
                null1 = reader.ReadUInt32(), // 4
                unixUpdatedTimeStamp = reader.ReadUInt32(), // 8
                unk3 = reader.ReadUInt32(), // 12
                unixCreatedTimeStamp = reader.ReadUInt32(), // 16
                unk5 = reader.ReadUInt32(), // 20
                minx = reader.ReadSingle(),
                miny = reader.ReadSingle(),
                minz = reader.ReadSingle(),
                maxx = reader.ReadSingle(),
                maxy = reader.ReadSingle(),
                maxz = reader.ReadSingle(),
                null2 = reader.ReadUInt16() // 46
            };

            Debug.Assert(reader.BaseStream.Position == 46);

            mesh.VertexCount = reader.ReadUInt32();
            mesh.VerticesOffset = (ulong)reader.BaseStream.Position;
            mesh.VertexBufferSize = mesh.VertexCount * sizeof(float) * 3;

            for (var i = 0; i < mesh.VertexCount; i++)
            {
                mesh.Vertices.Add(reader.ReadToVector3());
            }

            mesh.NormalsCount = reader.ReadUInt32();
            mesh.NormalsBufferSize = mesh.NormalsCount * sizeof(float) * 3;
            mesh.NormalsOffset = (ulong)reader.BaseStream.Position;

            for (var i = 0; i < mesh.NormalsCount; i++)
            {
                mesh.Normals.Add(reader.ReadToVector3());
            }

            // should be the same as the vertices, normals count.
            mesh.TextureCoordinatesCount = reader.ReadUInt32();
            mesh.TextureOffset = (ulong)reader.BaseStream.Position;
            for (var i = 0; i < mesh.TextureCoordinatesCount; i++)
            {
                mesh.TextureVectors.Add(reader.ReadToVector2());
            }

            //---------------- Indices ----------------
            // Experiment to see if the extra data at the bottom of mesh files is more indices
            // nIndices *= 3; // three indices per triangle maybe ?
            mesh.NumberOfIndices = reader.ReadUInt32();

            // if they aren't dividable by 3 then something is borked.
            if (mesh.NumberOfIndices > 0 && mesh.NumberOfIndices % 3 == 0)
            {
                mesh.IndicesOffset = (ulong)reader.BaseStream.Position;
                var dataLength = mesh.NumberOfIndices * 2;

                if (reader.BaseStream.Position - dataLength < 0)
                {
                    throw new OutOfDataException();
                }
                
                for (var i = 0; i < mesh.NumberOfIndices; i += 3)
                {
                    ushort position = reader.ReadUInt16();
                    ushort textureCoordinate = reader.ReadUInt16();
                    ushort normal = reader.ReadUInt16();
                    mesh.Indices.Add(new Shadowbane.Cache.Index(position, textureCoordinate, normal));
                }
            }
            else
            {
                logger?.Warning($"{mesh.Identity} has {mesh.NumberOfIndices} which are not divisible by 3");
                // experimenting  
                reader.BaseStream.Position += mesh.NumberOfIndices * 12; // i think in these cases they are a second skin?

                // try it again 
                mesh.NumberOfIndices = reader.ReadUInt32();

                // if they aren't dividable by 3 then something is borked.
                if (mesh.NumberOfIndices > 0 && mesh.NumberOfIndices % 3 == 0)
                {
                    mesh.IndicesOffset = (ulong)reader.BaseStream.Position;
                    var dataLength = mesh.NumberOfIndices * 2;

                    if (reader.BaseStream.Position - dataLength < 0)
                    {
                        throw new OutOfDataException();
                    }

                    for (var i = 0; i < mesh.NumberOfIndices; i += 3)
                    {
                        ushort position = reader.ReadUInt16();
                        ushort textureCoordinate = reader.ReadUInt16();
                        ushort normal = reader.ReadUInt16();
                        mesh.Indices.Add(new Cache.Index(position, textureCoordinate, normal));
                    }
                }
                else
                {
                    logger?.Error($"{mesh.Identity} is unparsable, indeces and vert counts don't match.");
                }
            }
        }

        logger?.Information(mesh.GetMeshInformation());
        return mesh;
    }

    public Mesh Create(uint indexId)
    {
        var cacheIndex = ArchiveLoader.MeshArchive.CacheIndices.FirstOrDefault(x => x.identity == indexId);
        if (cacheIndex.identity > 0)
        {
            return this.Create(cacheIndex);
        }

        throw new IndexNotFoundException(this.GetType(), indexId);
    }

    public bool HasMeshId(uint id)
    {
        return ArchiveLoader.MeshArchive.CacheIndices.Any(c => c.identity == id);
    }
}