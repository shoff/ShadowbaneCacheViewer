using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CacheViewer.Domain.Archive;
using CacheViewer.Domain.Exceptions;
using CacheViewer.Domain.Extensions;
using CacheViewer.Domain.Models;
using SlimDX;

namespace CacheViewer.Domain.Factories
{
    public class MeshFactory : IModelFactory
    {
        private static MeshArchive meshArchive;
        private static readonly MeshFactory instance = new MeshFactory();

        private MeshFactory()
        {
            meshArchive = (MeshArchive)ArchiveFactory.Instance.Build(CacheFile.Mesh);
        }

        /// <summary>
        /// Creates the specified index identifier.
        /// </summary>
        /// <param name="indexId">The index identifier.</param>
        /// <returns></returns>
        public Mesh Create(int indexId)
        {
            CacheIndex cacheIndex = meshArchive.CacheIndices.FirstOrDefault(x => x.identity == indexId);
            if (cacheIndex.identity > 0)
            {
                return Create(cacheIndex);
            }
            throw new IndexNotFoundException(this.GetType(), indexId);
        }

        /// <summary>
        /// Creates the specified buffer.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <returns></returns>
        public Mesh Create(CacheIndex cacheIndex)
        {
            Mesh mesh = new Mesh
            {
                CacheIndex = cacheIndex
            };

            CacheAsset cacheAsset = meshArchive[cacheIndex.identity];
            using (BinaryReader reader = cacheAsset.Item1.CreateBinaryReaderUtf32())
            {
                mesh.Header = new MeshHeader
                {
                    null1 = reader.ReadUInt32(), // 4
                    unixUpdatedTimeStamp = reader.ReadUInt32(),  // 8
                    unk3 = reader.ReadUInt32(),  // 12
                    unixCreatedTimeStamp = reader.ReadUInt32(),  // 16
                    unk5 = reader.ReadUInt32(),  // 20
                    min = reader.ReadToVector3(), // 32
                    max = reader.ReadToVector3(), // 44
                    null2 = reader.ReadUInt16()   // 46
                };

                Debug.Assert(reader.BaseStream.Position == 46);

                mesh.VertexCount = reader.ReadUInt32();
                mesh.VertexBufferSize = mesh.VertexCount * sizeof(float) * 3;
                mesh.Vertices = new Vector3[mesh.VertexCount];

                for (int i = 0; i < mesh.VertexCount; i++)
                {
                    mesh.Vertices[i] = reader.ReadToVector3();
                }

                mesh.NormalsCount = reader.ReadUInt32();
                mesh.NormalsBufferSize = mesh.NormalsCount * sizeof(float) * 3;
                mesh.Normals = new Vector3[mesh.NormalsCount];

                for (int i = 0; i < mesh.NormalsCount; i++)
                {
                    mesh.Normals[i] = reader.ReadToVector3();
                }

                // should be the same as the verts, normals count.
                mesh.TextureCoordinatesCount = reader.ReadUInt32();

                mesh.TextureVectors = new Vector2[mesh.TextureCoordinatesCount];
                for (int i = 0; i < mesh.TextureCoordinatesCount; i++)
                {
                    mesh.TextureVectors[i] = reader.ReadToVector2();
                }

                //---------------- Indices ----------------
                // Experiment to see if the extra data at the bottom of mesh files is more indices
                // nIndices *= 3; // three indices per triangle maybe ?
                mesh.NumberOfIndices = reader.ReadUInt32();

                // if they aren't divisable by 3 then something is borked.
                if ((mesh.NumberOfIndices > 0) && (mesh.NumberOfIndices % 3 == 0))
                {
                    var dataLength = mesh.NumberOfIndices * 2;

                    if (reader.BaseStream.Position - dataLength < 0)
                    {
                        throw new OutOfDataException();
                    }

                    for (int i = 0; i < mesh.NumberOfIndices;i+=3)
                    {
                        int position = reader.ReadUInt16();
                        int textureCoordinate = reader.ReadUInt16();
                        int normal = reader.ReadUInt16();
                        mesh.Indices.Add(new Parsers.WavefrontVertex(position, textureCoordinate, normal));
                    }
                }
            }

            return mesh;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static MeshFactory Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets the indexes.
        /// </summary>
        /// <value>
        /// The indexes.
        /// </value>
        public CacheIndex[] Indexes
        {
            get { return meshArchive.CacheIndices.ToArray(); }
        }

        /// <summary>
        /// Gets the identity range.
        /// </summary>
        /// <value>
        /// The identity range.
        /// </value>
        public Tuple<int, int> IdentityRange
        {
            get { return new Tuple<int, int>(meshArchive.LowestId, meshArchive.HighestId); }
        }

        /// <summary>
        /// Gets the identity array.
        /// </summary>
        /// <value>
        /// The identity array.
        /// </value>
        public int[] IdentityArray
        {
            get { return meshArchive.IdentityArray; }
        }
    }
}