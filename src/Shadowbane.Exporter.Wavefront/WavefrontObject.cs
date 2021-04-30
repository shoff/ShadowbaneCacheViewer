namespace Shadowbane.Exporter.Wavefront
{
    using System.Collections.Generic;
    using Geometry;

    public class WavefrontObject
    {
        public List<Vector3> Positions { get; } = new List<Vector3>();
        public List<Vector2> TextureCoordinates { get; } = new List<Vector2>();
        public List<Vector3> Normals { get; } = new List<Vector3>();
        public List<WavefrontFaceGroup> Groups { get; } = new List<WavefrontFaceGroup>();
    }
}