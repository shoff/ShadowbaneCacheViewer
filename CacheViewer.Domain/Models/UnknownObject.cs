﻿namespace CacheViewer.Domain.Models
{
    using System;
    using Archive;
    using Exportable;

    public class UnknownObject : CacheObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UnknownObject" /> class.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <param name="flag">The flag.</param>
        /// <param name="name">The name.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="data">The data.</param>
        /// <param name="innerOffset">The inner offset.</param>
        public UnknownObject(CacheIndex cacheIndex, ObjectType flag, string name, int offset, ArraySegment<byte> data,
            int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

        /// <summary>
        ///     Parses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Parse(ArraySegment<byte> data)
        {
        }
    }
}