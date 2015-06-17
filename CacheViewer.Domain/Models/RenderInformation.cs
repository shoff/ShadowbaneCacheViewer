using System.Collections.Generic;
using System.Globalization;
using System.Text;
using CacheViewer.Domain.Archive;
using SlimDX;


namespace CacheViewer.Domain.Models
{
    public class RenderInformation
    {
        private readonly List<RenderInformation> children = new List<RenderInformation>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderInformation"/> class.
        /// </summary>
        public RenderInformation()
        {
            this.ChildRenderIdList = new List<int>();
        }

        /// <summary>
        ///   Gets or sets the byte count.
        /// </summary>
        /// <value>The byte count.</value>
        public int ByteCount { get; set; }

        /// <summary>
        ///   Gets or sets the order.
        /// </summary>
        /// <value>
        ///   The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance has mesh.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has mesh; otherwise, <c>false</c>.
        /// </value>
        public bool HasMesh { get; set; }

        /// <summary>
        /// Gets or sets the mesh.
        /// </summary>
        /// <value>
        /// The mesh.
        /// </value>
        public Mesh Mesh { get; set; }

        /// <summary>
        ///   Gets or sets the unknown.
        /// </summary>
        /// <value>
        ///   The unknown.
        /// </value>
        public object[] Unknown { get; set; }

        /// <summary>
        ///   Gets or sets the mesh id.
        /// </summary>
        /// <value>
        ///   The mesh id.
        /// </value>
        public int MeshId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [valid mesh found].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [valid mesh found]; otherwise, <c>false</c>.
        /// </value>
        public bool ValidMeshFound { get; set; }

        /// <summary>
        ///   Gets or sets the name of the joint.
        /// </summary>
        /// <value>
        ///   The name of the joint.
        /// </value>
        public string JointName { get; set; }

        /// <summary>
        ///   Gets or sets the scale.
        /// </summary>
        /// <value>
        ///   The scale.
        /// </value>
        public Vector3 Scale { get; set; }

        /// <summary>
        ///   Gets or sets the position.
        /// </summary>
        /// <value>
        ///   The position.
        /// </value>
        public Vector3 Position { get; set; }

        /// <summary>
        ///   Gets or sets the render count.
        /// </summary>
        /// <value>
        ///   The render count.
        /// </value>
        public int RenderCount { get; set; }

        /// <summary>
        ///   Gets or sets the child render ids.
        /// </summary>
        /// <value>
        ///   The child render ids.
        /// </value>
        public CacheIndex[] ChildRenderIds { get; set; }

        /// <summary>
        ///   Gets or sets the texture id.
        /// </summary>
        /// <value>
        ///   The texture id.
        /// </value>
        public int TextureId { get; set; }

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        public Texture Texture { get; set; }

        /// <summary>
        ///   Gets or sets the identity.
        /// </summary>
        /// <value>The identity.</value>
        public CacheIndex CacheIndex { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the shared identifier.
        /// </summary>
        /// <value>
        /// The shared identifier.
        /// </value>
        public RenderInformation SharedId { get; set; }

        /// <summary>
        /// Gets or sets the child render identifier list.
        /// </summary>
        /// <value>
        /// The child render identifier list.
        /// </value>
        public List<int> ChildRenderIdList { get; set; }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public List<RenderInformation> Children
        {
            get { return this.children; }
        }

        /// <summary>
        /// Gets or sets the binary asset.
        /// </summary>
        /// <value>
        /// The binary asset.
        /// </value>
        public CacheAsset BinaryAsset { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("MeshId: {0} ", MeshId.ToString(CultureInfo.InvariantCulture));
            sb.AppendFormat(" Joint name: {0}", JointName);
            sb.AppendFormat(" Notes: {0}", Notes);
            return sb.ToString();
        }
    }
}