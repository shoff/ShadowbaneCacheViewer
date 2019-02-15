namespace CacheViewer.Domain.Parsers
{
    using System.Collections.Generic;
    using Geometry;

    /// <summary>
    ///     Class representing a Wavefront OBJ 3D mesh.
    /// </summary>
    public class WavefrontObject
    {
        public WavefrontObject()
        {
            this.Groups = new List<WavefrontFaceGroup>();
            this.Positions = new List<Vector3>();
            this.Texcoords = new List<Vector2>();
            this.Normals = new List<Vector3>();
        }
        public List<Vector3> Positions { get; }
        public List<Vector2> Texcoords { get; }
        public List<Vector3> Normals { get; }
        public List<WavefrontFaceGroup> Groups { get; }
    }
}