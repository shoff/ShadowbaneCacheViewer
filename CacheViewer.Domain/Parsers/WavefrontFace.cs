namespace CacheViewer.Domain.Parsers
{
    using System.Collections.Generic;

    /// <summary>
    ///     A struct representing a Wavefront OBJ geometry face.
    /// </summary>
    /// <remarks>
    ///     A face is described through a list of OBJ vertices.
    ///     It can consist of three or more vertices an can therefore be split up
    ///     into one or more triangles.
    /// </remarks>
    public class WavefrontFace
    {
        public List<Index> Vertices { get; set; }

        /// <summary>
        ///     Gets the triangle count.
        /// </summary>
        /// <value>
        ///     The triangle count.
        /// </value>
        public int TriangleCount => this.Vertices.Count - 2;

    }
}