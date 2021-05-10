#pragma warning disable IDE0032 // Use auto property
namespace Shadowbane.Models
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;
    using Geometry;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MeshHeader
    {
        //50 byte header
        // 46th byte of the header is the vertex count
        // following the header (immediately after the vertex count
        // is VertexCount * 4 (float) * 3 (Vector)
        // At the end of this chunk is another count (which always seems to be the same as the one in the header)
        public uint null1; // 4
        public uint unixUpdatedTimeStamp; // 8
        public uint unk3; // 
        public uint unixCreatedTimeStamp;
        public uint unk5;
        public float minx;
        public float miny;
        public float minz;
        public float maxx;
        public float maxy;
        public float maxz;
        public ushort null2;
    }

    public class Mesh
    {
        private float meshSize;

        public Mesh(uint identity)
        {
            this.Identity = identity;
            this.Bounds = new Vector3[16];
        }
        public uint Identity { get; }
        public uint VertexCount { get; set; }
        public uint VertexBufferSize { get; set; }
        public MeshHeader Header { get; set; }
        public ICollection<Texture> Textures { get; } = new List<Texture>();
        public ICollection<Vector3> Vertices { get; private set; } = new List<Vector3>();
        public ulong VerticesOffset { get; set; }
        public ICollection<Vector3> Normals { get; } = new List<Vector3>();
        public ICollection<Vector2> TextureVectors { get; } = new List<Vector2>();
        public ulong IndicesOffset { get; set; }
        public ICollection<Models.Index> Indices { get; } = new List<Models.Index>();
        public ulong NormalsOffset { get; set; }
        public uint NormalsCount { get; set; }
        public uint NormalsBufferSize { get; set; }
        public ulong TextureOffset { get; set; }
        public uint TextureCoordinatesCount { get; set; }
        public byte[] UnknownData { get; set; }
        public long OffsetToUnknownData { get; set; }
        public int NumberOfIndices { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Position { get; set; }
        public Vector3[] Bounds { get; }
        public float MeshSize => this.meshSize;

        //public void ApplyPosition()
        //{
        //    List<Vector3> positioned = new List<Vector3>();
        //    foreach (var vector in this.Vertices)
        //    {
        //        var v = Vector3.Add(vector, this.Position);
        //        positioned.Add(v);
        //    }
        //    this.Vertices = positioned;
        //}

        public void ApplyPosition()
        {
            List<Vector3> positioned = new List<Vector3>();

            var translationMatrix = Matrix4.CreateTranslation(this.Position);
            foreach (var v in this.Vertices)
            {
                var v4 = new Vector4(v.X, v.Y, v.Z, 1);
                Vector4.Transform(ref translationMatrix, ref v4, out var vprime);
                positioned.Add(new Vector3(vprime.X, vprime.Y, vprime.Z));
            }
            this.Vertices = positioned;
        }

        public void ApplyScale()
        {
            // do nothing for now
        }

        public void SetBounds()
        {
            this.SetBounds(new Vector3(this.Header.minx, this.Header.miny, this.Header.minz), 
                new Vector3(this.Header.maxx, this.Header.maxy, this.Header.maxz));
        }

        public void SetBounds(Vector3 min, Vector3 max)
        {
            Vector3 size = max - min;
            this.meshSize = (size.X + size.Y + size.Z) / 3;

            // front face
            this.Bounds[0] = max;                        // Top left
            this.Bounds[1] = new Vector3(min.X, max.Y, max.Z); // Top right
            this.Bounds[2] = new Vector3(min.X, min.Y, max.Z); // Bottom right
            this.Bounds[3] = new Vector3(max.Z, min.Y, max.Z); // bottom left

            // right face
            this.Bounds[4] = new Vector3(min.X, max.Y, max.Z); // top left
            this.Bounds[5] = new Vector3(min.X, max.Y, min.Z); // top right
            this.Bounds[6] = min;                        // bottm right
            this.Bounds[7] = new Vector3(min.X, min.Y, max.Z); // bottom left

            // back face
            this.Bounds[8] = new Vector3(min.X, max.Y, min.Z);     // Top left
            this.Bounds[9] = new Vector3(max.X, max.Y, min.Z);     // top right
            this.Bounds[10] = new Vector3(max.X, min.Y, min.Z);    // bottom right
            this.Bounds[11] = min;                           // bottom left

            // left face
            this.Bounds[12] = new Vector3(max.X, max.Y, min.Z);    // Top left
            this.Bounds[13] = max;                           // Top right
            this.Bounds[14] = new Vector3(max.X, min.Y, max.Z);    // bottom right
            this.Bounds[15] = new Vector3(max.X, min.Y, min.Z);    // bottom left

        }

        // for reference
        // void Model::DrawBounds()
        // {
        //    // Set to render as lines
        //    glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

        //    glColor3f(0.0f, 0.2f, 0.8f);

        //    glBegin(GL_QUADS);
        //    // Draw the boundry lines
        //    for (unsigned int i = 0; i < 16; i++)
        //    glVertex3fv(bounds[i]);

        //    glEnd();

        //    glColor3f(1.0f, 1.0f, 1.0f);

        //    // finished drawing, set back to default
        //    glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
        // }

        public string GetMeshInformation()
        {
            var sb = new StringBuilder();
            sb.Append($"Vertex count {this.VertexCount} - Vertex offset {this.VerticesOffset}\r\n");
            sb.Append($"Normals count {this.NormalsCount} - Normals offset {this.NormalsOffset}\r\n");
            sb.Append($"Indices count {this.Indices.Count} - Indices offset {this.IndicesOffset}\r\n");
            sb.Append($"Texture count {this.TextureCoordinatesCount} - Texture offset {this.TextureOffset}");
            return sb.ToString();
        }
    }
}

#pragma warning restore IDE0032 // Use auto property
