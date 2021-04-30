namespace Shadowbane.Exporter.Wavefront
{
    using System.Collections.Generic;


    public class WavefrontFace
    {
        public ICollection<Index> Vertices { get; set; } = new HashSet<Index>();
        public int TriangleCount => this.Vertices.Count - 2;

    }
}