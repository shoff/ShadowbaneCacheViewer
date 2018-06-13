namespace CacheViewer.Domain.Models.Exportable
{
    using System;
    using Archive;

    public abstract class ModelObject : CacheObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ModelObject" /> class.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <param name="flag">The flag.</param>
        /// <param name="name">The name.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="data">The data.</param>
        /// <param name="innerOffset">The inner offset.</param>
        protected ModelObject(CacheIndex cacheIndex, ObjectType flag, string name, int offset, ArraySegment<byte> data,
            int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }
    }
}