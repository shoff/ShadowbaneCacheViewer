

namespace CacheViewer.Domain.Models
{
    using System;
    using System.IO;
    using ArraySegments;
    using System.Collections.Generic;
    using CacheViewer.Domain.Extensions;

    // TODO this is currently being used in the data context and really should not be as it is used to 
    // TODO parse binary data as such it include properties that are not available to entity framework
    // TODO such as unsigned int.
    public class Skeleton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Skeleton"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="id">The identifier.</param>
        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public Skeleton(ArraySegment<byte> data, int id)
        {
            this.MotionIds = new HashSet<long>();
            this.SkeletonId = id;

            using (BinaryReader reader = data.CreateBinaryReader())
            {
                uint textLength = reader.ReadUInt32();
                this.SkeletonText = reader.ReadAsciiString(textLength);
                this.MotionCount = reader.ReadUInt32();
                reader.BaseStream.Position += 20; // empty ints

                for (int i = 0; i < this.MotionCount; i++)
                {
                    long x = reader.ReadUInt32();
                    if (x > 0)
                    {
                        this.MotionIds.Add(x);
                    }
                }
            }
            this.DistinctMotionIdCount = this.MotionIds.Count;
        }

        /// <summary>
        /// Gets or sets the distinct motion identifier count.
        /// </summary>
        /// <value>
        /// The distinct motion identifier count.
        /// </value>
        public int DistinctMotionIdCount { get; set; }

        /// <summary>
        /// Gets or sets the motion count.
        /// </summary>
        /// <value>
        /// The motion count.
        /// </value>
        public uint MotionCount { get; set; }

        /// <summary>
        /// Gets or sets the skeleton text.
        /// </summary>
        /// <value>
        /// The skeleton text.
        /// </value>
        public string SkeletonText { get; set; }

        /// <summary>
        /// Gets or sets the skeleton identifier.
        /// </summary>
        /// <value>
        /// The skeleton identifier.
        /// </value>
        public int SkeletonId { get; set; }

        /// <summary>
        /// Gets or sets the motion ids.
        /// </summary>
        /// <value>
        /// The motion ids.
        /// </value>
        public ICollection<long> MotionIds { get; set; }
    }
}