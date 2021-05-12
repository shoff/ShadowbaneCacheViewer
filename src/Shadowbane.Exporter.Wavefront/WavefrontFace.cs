namespace Shadowbane.Exporter.Wavefront
{
    using System.Collections.Generic;


    public class WavefrontFace
    {
        public ICollection<Index> Vertices { get; set; } = new HashSet<Index>();

        // why did I subtract 2?
        public int TriangleCount => this.Vertices.Count - 2;

    }
}