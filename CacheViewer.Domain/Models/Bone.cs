namespace CacheViewer.Domain.Models
{
    using System;
    using SlimDX;

    public class Bone
    {
        private readonly ArraySegment<byte> data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Bone" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Bone(ArraySegment<byte> data)
        {
            this.data = data;
            this.Id = 0;
            this.MeshId = 0;
            this.NumberOfChildren = 0;
            this.ParentId = 0;
            this.Children = 0;

            this.Direction = Vector3.Zero;
            this.Axis = Vector3.Zero;
            this.Length = 0.0f;

            this.Pos = Vector3.Zero;
            this.Rot = Quaternion.Identity;
            this.Scale = new Vector3(1, 1, 1);

            this.Mat = Matrix.Identity;
            this.RMat = Matrix.Identity;
            this.Setup = false;
            this.Flip = false;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="Bone" /> is setup.
        /// </summary>
        /// <value>
        ///     <c>true</c> if setup; otherwise, <c>false</c>.
        /// </value>
        public bool Setup { get; set; }

        /// <summary>
        ///     Gets or sets the r mat.
        /// </summary>
        /// <value>
        ///     The r mat.
        /// </value>
        public Matrix RMat { get; set; }

        /// <summary>
        ///     Gets or sets the mat.
        /// </summary>
        /// <value>
        ///     The mat.
        /// </value>
        public Matrix Mat { get; set; }

        /// <summary>
        ///     Gets or sets the scale.
        /// </summary>
        /// <value>
        ///     The scale.
        /// </value>
        public Vector3 Scale { get; set; }

        /// <summary>
        ///     Gets or sets the rot.
        /// </summary>
        /// <value>
        ///     The rot.
        /// </value>
        public Quaternion Rot { get; set; }

        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        /// <value>
        ///     The position.
        /// </value>
        public Vector3 Pos { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="Bone" /> is flip.
        /// </summary>
        /// <value>
        ///     <c>true</c> if flip; otherwise, <c>false</c>.
        /// </value>
        public bool Flip { get; set; }

        /// <summary>
        ///     Gets or sets the length.
        /// </summary>
        /// <value>
        ///     The length.
        /// </value>
        public float Length { get; set; }

        /// <summary>
        ///     Gets or sets the axis.
        /// </summary>
        /// <value>
        ///     The axis.
        /// </value>
        public Vector3 Axis { get; set; }

        /// <summary>
        ///     Gets or sets the direction.
        /// </summary>
        /// <value>
        ///     The direction.
        /// </value>
        public Vector3 Direction { get; set; }

        /// <summary>
        ///     Gets or sets the children.
        /// </summary>
        /// <value>
        ///     The children.
        /// </value>
        public int Children { get; set; }

        /// <summary>
        ///     Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        ///     The parent identifier.
        /// </value>
        public int ParentId { get; set; }

        /// <summary>
        ///     Gets or sets the number of children.
        /// </summary>
        /// <value>
        ///     The number of children.
        /// </value>
        public int NumberOfChildren { get; set; }

        /// <summary>
        ///     Gets or sets the mesh identifier.
        /// </summary>
        /// <value>
        ///     The mesh identifier.
        /// </value>
        public int MeshId { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public int Id { get; set; }
    }
}