namespace Shadowbane.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Cache;
    using Geometry;

    public class RenderInformation
    {
        public uint IUNK { get; set; }
        public uint FirstInt { get; set; }
        public ushort FirstUshort { get; set; }
        public DateTime? CreateDate { get; set; }
        public uint UnknownIntOne { get; set; }
        public uint UnknownIntTwo { get; set; }
        public uint UnknownCounterOrBool { get; set; }
        public int ByteCount { get; set; }
        public bool HasMesh { get; set; }
        public Mesh Mesh { get; set; }
        public object[] Unknown { get; set; }
        public int MeshId { get; set; }
        public bool ValidMeshFound { get; set; }
        public string JointName { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Position { get; set; }
        public int RenderCount { get; set; }
        public CacheIndex[] ChildRenderIds { get; set; }
        public List<int> Textures { get; set; } = new List<int>();
        public bool HasTexture { get; set; }
        public uint TextureCount { get; set; }
        public Texture Texture { get; set; }
        public Vector2 TextureVector { get; set; }
        public CacheIndex CacheIndex { get; set; }
        public string Notes { get; set; }
        public RenderInformation SharedId { get; set; }
        public int ChildCount { get; set; }
        public List<int> ChildRenderIdList { get; set; } = new List<int>();
        public List<RenderInformation> Children { get; } = new List<RenderInformation>();
        public CacheAsset BinaryAsset { get; set; }
        public byte B34 { get; set; }
        public byte B11 { get; set; }
        public long LastOffset { get; set; }
        public long UnreadByteCount { get; set; }
        public uint JointNameSize { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"# RenderIdentity: {this.CacheIndex.identity}");
            sb.AppendLine($"# MeshId: {this.MeshId.ToString(CultureInfo.InvariantCulture)}");
            sb.AppendLine($"# Joint name: {this.JointName ?? "Not a joint"}");
            sb.AppendLine($"# Notes: {this.Notes}");
            sb.AppendLine($"# Children: {this.RenderCount}");
            sb.AppendLine($"# Scale: {this.Scale.ToString()}");
            sb.AppendLine($"# Position: {this.Position.ToString()}");
            sb.AppendLine($"# TextureCount: {this.TextureCount}");

            return sb.ToString();
        }
    }
}