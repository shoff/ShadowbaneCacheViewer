#nullable enable
namespace Shadowbane.Models;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Cache;

public class Renderable : IRenderable
{
    public uint RenderType { get; set; }
    public uint FirstInt { get; set; }
    public int ByteCount { get; set; }
    public bool HasMesh { get; set; }
    public IMesh? Mesh { get; set; }
    public uint[] Unknown { get; } = new uint[6];
    public uint MeshId { get; set; }
    public string? JointName { get; set; }
    public Vector3 Scale { get; set; }
    public Vector3 Position { get; set; }
    public bool HasTexture { get; set; }
    public uint TextureCount { get; set; }
    public Vector2 TextureVector { get; set; }
    public CacheIndex CacheIndex { get; set; }
    public int ChildCount { get; set; }
    public long LastOffset { get; set; }
    public long UnreadByteCount { get; set; }
    public uint JointNameSize { get; set; }
    public bool IsValid { get; set; } = true;
    public uint Identity { get; set; }
    public ICollection<uint> Textures { get; } = new HashSet<uint>();
    public List<int> ChildRenderIdList { get; set; } = new();
    public List<IRenderable> Children { get; } = new();
    public ICollection<string> Notes { get; set; } = new HashSet<string>();
    public ReadOnlyMemory<byte> Data { get; set; }
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"{Identity},{RenderType},{ChildCount},{LastOffset},");

        if (HasMesh)
        {
            sb.Append($"{MeshId},");
        }
        else
        {
            sb.Append(",");
        }

        sb.Append($"{TextureCount},");

        if (HasTexture && Textures.Count > 0)
        {
                
            foreach (var texture in Textures)
            {
                sb.Append($"{texture} ");
            }
            sb.Append(",");
        }
        else
        {
            sb.Append(",");
        }

        if (!string.IsNullOrWhiteSpace(JointName))
        {
            sb.Append($"{JointName}");
        }
        else
        {
            sb.Append(",");
        }
        return sb.ToString();
    }
}