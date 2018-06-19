namespace CacheViewer.Domain.Models.Exportable
{
    using System;
    using Archive;
    using CacheViewer.Data;

    public abstract class AnimationObject : ModelObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AnimationObject" /> class.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <param name="flag">The flag.</param>
        /// <param name="name">The name.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="data">The data.</param>
        /// <param name="innerOffset">The inner offset.</param>
        protected AnimationObject(CacheIndex cacheIndex, ObjectType flag, string name, int offset,
            ArraySegment<byte> data, int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

        /// <summary>
        ///     Gets or sets the skeleton.
        /// </summary>
        /// <value>
        ///     The skeleton.
        /// </value>
        public ICacheObject Skeleton { get; set; }
    }
}