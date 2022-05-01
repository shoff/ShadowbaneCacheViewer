namespace Shadowbane.Cache.Exporter.File.Json;

using System;
using System.Collections.Generic;

public class JsonRenderInformation
{
    public uint IUNK { get; set; }
    public uint RenderType { get; set; }
    public uint FirstInt { get; set; }
    public ushort FirstUshort { get; set; }
    public DateTime? CreateDate { get; set; }
    public uint UnknownIntOne { get; set; }
    public uint UnknownIntTwo { get; set; }
    public uint UnknownCounterOrBool { get; set; }
    public int ByteCount { get; set; }
    public bool HasMesh { get; set; }
    public JsonMesh Mesh { get; set; }
    public object[] Unknown { get; set; }
    public int MeshId { get; set; }
    public bool ValidMeshFound { get; set; }
    public string JointName { get; set; }
    public JsonVector3 Scale { get; set; }
    public JsonVector3 Position { get; set; }
    public int RenderCount { get; set; }
    public CacheIndex[] ChildRenderIds { get; set; }
    public bool HasTexture { get; set; }
    public uint TextureCount { get; set; }
    public JsonVector2 TextureVector { get; set; }
    public CacheIndex CacheIndex { get; set; }
    public string Notes { get; set; }
    public JsonRenderInformation Render { get; set; }
    public int ChildCount { get; set; }
    public CacheAsset BinaryAsset { get; set; }
    public byte B34 { get; set; }
    public byte B11 { get; set; }
    public long LastOffset { get; set; }
    public long UnreadByteCount { get; set; }
    public uint JointNameSize { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsValid { get; set; } = true;
    public uint Identity { get; set; }
    public List<JsonRenderInformation> Children { get; set; } = new();
    public ICollection<JsonTexture> Textures { get; set; } = new HashSet<JsonTexture>();
    public List<int> ChildRenderIdList { get; set; } = new();
}