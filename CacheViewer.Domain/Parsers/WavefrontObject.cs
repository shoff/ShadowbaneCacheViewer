using System.Collections.Generic;
using SlimDX;

namespace CacheViewer.Domain.Parsers
{
    /// <summary>
    /// Class representing a Wavefront OBJ 3D mesh.
    /// </summary>
    public class WavefrontObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WavefrontObject"/> class.
        /// </summary>
        public WavefrontObject()
        {
            this.Groups = new List<WavefrontFaceGroup>();
            this.Positions = new List<Vector3>();
            this.Texcoords = new List<Vector2>();
            this.Normals = new List<Vector3>();
        }
        
        /// <summary>
        /// Gets the positions.
        /// </summary>
        /// <value>
        /// The positions.
        /// </value>
        public List<Vector3> Positions { get; private set; }

        /// <summary>
        /// Gets the texcoords.
        /// </summary>
        /// <value>
        /// The texcoords.
        /// </value>
        public List<Vector2> Texcoords { get; private set; }

        /// <summary>
        /// Gets the normals.
        /// </summary>
        /// <value>
        /// The normals.
        /// </value>
        public List<Vector3> Normals { get; private set; }

        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <value>
        /// The groups.
        /// </value>
        public List<WavefrontFaceGroup> Groups { get; private set; }
    }
}