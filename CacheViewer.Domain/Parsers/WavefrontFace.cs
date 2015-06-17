using System.Collections.Generic;

namespace CacheViewer.Domain.Parsers
{
    /// <summary>
    /// A struct representing a Wavefront OBJ geometry face.
    /// </summary>
    ///<remarks>
    /// A face is described through a list of OBJ vertices.
    /// It can consist of three or more vertices an can therefore be split up
    /// into one or more triangles.
    /// </remarks>
    public class WavefrontFace
    {
        public List<WavefrontVertex> Vertices { get; set; }

        /// <summary>
        /// Gets the triangle count.
        /// </summary>
        /// <value>
        /// The triangle count.
        /// </value>
        public int TriangleCount
        {
            get { return this.Vertices.Count - 2; }
        }

        /// <summary>
        /// Gets the vertex count.
        /// </summary>
        /// <value>
        /// The vertex count.
        /// </value>
        public int VertexCount
        {
            get { return this.Vertices.Count; }
        }
    }
}