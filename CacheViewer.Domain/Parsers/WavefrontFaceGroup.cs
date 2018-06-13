namespace CacheViewer.Domain.Parsers
{
    using System.Collections.Generic;

    /// <summary>
    ///     Struct representing an Wavefront OBJ face group.
    /// </summary>
    /// <remarks>
    ///     Groups contain faces and subdivide a geometry into smaller objects.
    /// </remarks>
    public class WavefrontFaceGroup
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WavefrontFaceGroup" /> class.
        /// </summary>
        public WavefrontFaceGroup()
        {
            this.Faces = new List<WavefrontFace>();
        }

        // Name of the sub mesh
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the faces.
        /// </summary>
        /// <value>
        ///     The faces.
        /// </value>
        public List<WavefrontFace> Faces { get; set; }

        /// <summary>
        ///     Gets the triangle count.
        /// </summary>
        /// <value>
        ///     The triangle count.
        /// </value>
        public int TriangleCount
        {
            get
            {
                var count = 0;

                foreach (var face in this.Faces)
                {
                    count += face.TriangleCount;
                }

                return count;
            }
        }
    }
}