namespace Shadowbane.Exporter.MongoDb.Entities;
using MongoDB.Bson.Serialization.Attributes;


[BsonIgnoreExtraElements]
public class GameObject : BaseEntity
{
    public uint Identity { get; set; }
    public string Name { get; set; }
    public Renderable Render { get; set; }
}

public class Renderable 
{
    public uint Id { get; set; }
    public uint RenderType { get; set; }
    public uint FirstInt { get; set; }
    public int ByteCount { get; set; }
    public bool HasMesh { get; set; }
    public uint MeshId { get; set; }
    public string? JointName { get; set; }
    public Vector3Data? Scale { get; set; }
    public Vector3Data? Position { get; set; }
    public bool HasTexture { get; set; }
    public uint TextureCount { get; set; }
    public Vector2Data? TextureVector { get; set; }
    public int ChildCount { get; set; }
    public long LastOffset { get; set; }
    public long UnreadByteCount { get; set; }
    public uint JointNameSize { get; set; }
    public bool IsValid { get; set; }
    public ICollection<uint> Textures { get; set; } = new HashSet<uint>();
    public ICollection<string> Notes { get; set; } = new List<string>();
    public ICollection<uint> Unknown { get; set; } = new HashSet<uint>();
    public ICollection<int> ChildRenderIdList { get; set; } = new HashSet<int>();
    public ICollection<Renderable> Children { get; set; } = new List<Renderable>();
}

public record Vector3Data(float X, float Y, float Z); 
public record Vector2Data(float X, float Y);