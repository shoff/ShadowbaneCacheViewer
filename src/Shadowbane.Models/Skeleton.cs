namespace Shadowbane.Models
{
    using System;
    using System.Collections.Generic;
    using Cache;

    public class Skeleton
    {
        public Skeleton(ReadOnlyMemory<byte> data, int id)
        {
            this.MotionIds = new HashSet<long>();
            this.SkeletonId = id;

            using (var reader = data.CreateBinaryReaderUtf32())
            {
                var textLength = reader.ReadUInt32();
                this.SkeletonText = reader.ReadAsciiString(textLength);
                this.MotionCount = reader.ReadUInt32();
                reader.BaseStream.Position += 20; // empty ints

                for (var i = 0; i < this.MotionCount; i++)
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

        public int DistinctMotionIdCount { get; set; }
        public uint MotionCount { get; set; }
        public string SkeletonText { get; set; }
        public int SkeletonId { get; set; }
        public ICollection<long> MotionIds { get; set; }
    }
}