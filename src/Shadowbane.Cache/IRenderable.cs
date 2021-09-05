#nullable enable
namespace Shadowbane.Cache
{
    using System.Collections.Generic;
    using Geometry;

    public interface IRenderable
    {
        uint RenderType { get; set; }
        uint FirstInt { get; set; }
        int ByteCount { get; set; }
        bool HasMesh { get; set; }
        Mesh? Mesh { get; set; }
        object[] Unknown { get; }
        uint MeshId { get; set; }
        string? JointName { get; set; }
        Vector3 Scale { get; set; }
        Vector3 Position { get; set; }
        bool HasTexture { get; set; }
        uint TextureCount { get; set; }
        ICollection<uint> Textures { get; }
        Vector2 TextureVector { get; set; }
        CacheIndex CacheIndex { get; set; }
        ICollection<string> Notes { get; set; }
        int ChildCount { get; set; }
        List<int> ChildRenderIdList { get; set; }
        List<IRenderable> Children { get; }
        long LastOffset { get; set; }
        long UnreadByteCount { get; set; }
        uint JointNameSize { get; set; }
        bool IsValid { get; set; }
        uint Identity { get; set; }
        string ToString();
    }
}