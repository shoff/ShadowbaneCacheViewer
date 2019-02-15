namespace CacheViewer.Domain.Factories
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Archive;
    using Exceptions;
    using Extensions;
    using Models;
    using NLog;
    using Parsers;

    public class MeshFactory : IModelFactory
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private static MeshArchive meshArchive;

        private MeshFactory()
        {
            meshArchive = (MeshArchive) ArchiveFactory.Instance.Build(CacheFile.Mesh);
        }

        public async Task SaveToFile(CacheIndex index, string path)
        {
            await meshArchive.SaveToFileAsync(index, path);
        }

        public static MeshFactory Instance { get; } = new MeshFactory();

        public CacheIndex[] Indexes => meshArchive.CacheIndices.ToArray();

        public Tuple<int, int> IdentityRange => new Tuple<int, int>(meshArchive.LowestId, meshArchive.HighestId);

        public int[] IdentityArray => meshArchive.IdentityArray;

        public Mesh Create(CacheIndex cacheIndex)
        {
            var mesh = new Mesh
            {
                CacheIndex = cacheIndex,
                Id = cacheIndex.Identity
            };

            var cacheAsset = meshArchive[cacheIndex.Identity];

            using (var reader = cacheAsset.Item1.CreateBinaryReaderUtf32())
            {
                mesh.Header = new MeshHeader
                {
                    null1 = reader.ReadUInt32(), // 4
                    unixUpdatedTimeStamp = reader.ReadUInt32(), // 8
                    unk3 = reader.ReadUInt32(), // 12
                    unixCreatedTimeStamp = reader.ReadUInt32(), // 16
                    unk5 = reader.ReadUInt32(), // 20
                    min = reader.ReadToVector3(), // 32
                    max = reader.ReadToVector3(), // 44
                    null2 = reader.ReadUInt16() // 46
                };

                Debug.Assert(reader.BaseStream.Position == 46);

                mesh.VertexCount = reader.ReadUInt32();
                mesh.VerticesOffset = (ulong) reader.BaseStream.Position;
                mesh.VertexBufferSize = mesh.VertexCount * sizeof(float) * 3;

                for (var i = 0; i < mesh.VertexCount; i++)
                {
                    mesh.Vertices.Add(reader.ReadToVector3());
                }

                mesh.NormalsCount = reader.ReadUInt32();
                mesh.NormalsBufferSize = mesh.NormalsCount * sizeof(float) * 3;
                mesh.NormalsOffset = (ulong) reader.BaseStream.Position;

                for (var i = 0; i < mesh.NormalsCount; i++)
                {
                    mesh.Normals.Add(reader.ReadToVector3());
                }

                // should be the same as the vertices, normals count.
                mesh.TextureCoordinatesCount = reader.ReadUInt32();
                mesh.TextureOffset = (ulong) reader.BaseStream.Position;
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
                        int position = reader.ReadUInt16();
                        int textureCoordinate = reader.ReadUInt16();
                        int normal = reader.ReadUInt16();
                        mesh.Indices.Add(new Index(position, textureCoordinate, normal));
                    }
                }
                else
                {
                    logger?.Warn($"{mesh.Id} has {mesh.NumberOfIndices} which are not divisible by 3");
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
                            int position = reader.ReadUInt16();
                            int textureCoordinate = reader.ReadUInt16();
                            int normal = reader.ReadUInt16();
                            mesh.Indices.Add(new Index(position, textureCoordinate, normal));
                        }
                    }
                    else
                    {
                        logger?.Error($"{mesh.Id} is unparsable, indeces and vert counts don't match.");
                    }
                }
            }

            logger?.Info(mesh.GetMeshInformation());
            return mesh;
        }

        public Mesh Create(int indexId)
        {
            var cacheIndex = meshArchive.CacheIndices.FirstOrDefault(x => x.Identity == indexId);
            if (cacheIndex.Identity > 0)
            {
                return this.Create(cacheIndex);
            }

            throw new IndexNotFoundException(this.GetType(), indexId);
        }

        public bool HasMeshId(int id)
        {
            return meshArchive.Contains(id);
        }
    }
}