namespace CacheViewer.Domain.Factories
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Archive;
    using Exceptions;
    using Extensions;
    using Models;
    using Parsers;

    public class MeshFactory : IModelFactory
    {
        internal static MeshArchive MeshArchive { get; private set; }

        private MeshFactory()
        {
            MeshArchive = (MeshArchive) ArchiveFactory.Instance.Build(CacheFile.Mesh);
        }

        /// <summary>Gets the instance.</summary>
        public static MeshFactory Instance => new MeshFactory();

        /// <summary>Gets the indexes.</summary>
        public CacheIndex[] Indexes => MeshArchive.CacheIndices.ToArray();

        /// <summary>Gets the identity range.</summary>
        public Tuple<int, int> IdentityRange => new Tuple<int, int>(MeshArchive.LowestId, MeshArchive.HighestId);

        /// <summary>Gets the identity array.</summary>
        public int[] IdentityArray => MeshArchive.IdentityArray;

        /// <summary>Creates the specified buffer.</summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <returns></returns>
        /// <exception cref="IndexNotFoundException">Condition. </exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="OutOfDataException">Condition.</exception>
        public Mesh Create(CacheIndex cacheIndex)
        {
            var mesh = new Mesh
            {
                CacheIndex = cacheIndex,
                Id = cacheIndex.Identity
            };
            var cacheAsset = MeshArchive[cacheIndex.Identity];
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
                mesh.VertexBufferSize = mesh.VertexCount * sizeof(float) * 3;

                //mesh.Vertices = new Vector3[mesh.VertexCount];

                for (var i = 0; i < mesh.VertexCount; i++)
                {
                    mesh.Vertices.Add(reader.ReadToVector3());
                }

                mesh.NormalsCount = reader.ReadUInt32();
                mesh.NormalsBufferSize = mesh.NormalsCount * sizeof(float) * 3;

                //mesh.Normals = new Vector3[mesh.NormalsCount];

                for (var i = 0; i < mesh.NormalsCount; i++)
                {
                    mesh.Normals.Add(reader.ReadToVector3());
                }

                // should be the same as the vertices, normals count.
                mesh.TextureCoordinatesCount = reader.ReadUInt32();

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
                        mesh.Indices.Add(new WavefrontVertex(position, textureCoordinate, normal));
                    }
                }
            }

            return mesh;
        }


        /// <summary>Creates the specified index identifier.</summary>
        /// <param name="indexId">The index identifier.</param>
        /// <returns></returns>
        /// <exception cref="IndexNotFoundException">When cache does not contain item with matching indexId.</exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="OutOfDataException">Condition.</exception>
        public Mesh Create(int indexId)
        {
            var cacheIndex = MeshArchive.CacheIndices.FirstOrDefault(x => x.Identity == indexId);
            if (cacheIndex.Identity > 0)
            {
                return this.Create(cacheIndex);
            }

            throw new IndexNotFoundException(this.GetType(), indexId);
        }
    }
}