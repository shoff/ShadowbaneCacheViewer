using System;
using CacheViewer.Domain.Archive;

namespace CacheViewer.Domain.Models.Exportable
{
    public interface ICacheObject : IComparable<ICacheObject>
    {
        /// <summary>
        /// Gets the index of the cache.
        /// </summary>
        /// <value>
        /// The index of the cache.
        /// </value>
        CacheIndex CacheIndex { get; }

        /// <summary>
        /// Gets the render identifier.
        /// </summary>
        /// <value>
        /// The render identifier.
        /// </value>
        uint RenderId { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the flag.
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        ObjectType Flag { get; }

        /// <summary>
        /// Gets the cursor offset.
        /// </summary>
        /// <value>
        /// The cursor offset.
        /// </value>
        int CursorOffset { get; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        ArraySegment<byte> Data { get; }

        /// <summary>
        /// Parses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        void Parse(ArraySegment<byte> data);

        /// <summary>
        /// Gets the inner offset.
        /// </summary>
        /// <value>
        /// The inner offset.
        /// </value>
        int InnerOffset { get; }
    }
}