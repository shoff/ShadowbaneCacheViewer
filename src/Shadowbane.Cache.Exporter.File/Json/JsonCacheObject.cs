namespace Shadowbane.Cache.Exporter.File.Json;

using System.Collections.Generic;

public class JsonCacheObject
{
    public uint Identity { get; set; }
    public ICollection<uint> RenderIds { get; set; } = new HashSet<uint>();
    public ICollection<JsonRenderInformation> Renders { get; set; } = new HashSet<JsonRenderInformation>();
    public int UnParsedBytes { get; set; }
    public uint RenderId { get; set; }
    public string Name { get; set; }
    public ObjectType Flag { get; set; }
    public uint CursorOffset { get; set; }
    public string Data { get; set; }
    public uint InnerOffset { get; set; }
    public uint RenderCount { get; set; }
}