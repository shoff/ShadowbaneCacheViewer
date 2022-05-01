namespace Shadowbane.Cache.Exporter.File.Json;

public class JsonTexture
{
    public uint TextureId { get; set; }
    public CacheIndex CacheIndexIdentity { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Depth { get; set; }
}