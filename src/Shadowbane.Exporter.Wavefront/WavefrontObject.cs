namespace Shadowbane.Exporter.Wavefront;

using System.Collections.Generic;
using System.Numerics;

public class WavefrontObject
{
    public List<Vector3> Positions { get; } = new();
    public List<Vector2> TextureCoordinates { get; } = new();
    public List<Vector3> Normals { get; } = new();
    public List<WavefrontFaceGroup> Groups { get; } = new();
}