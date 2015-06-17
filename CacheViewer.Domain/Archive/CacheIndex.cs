using System;
using System.Text;

namespace CacheViewer.Domain.Archive
{
    public struct CacheIndex : IComparable<CacheIndex>, IEquatable<CacheIndex>
    {
        public int identity;
        public uint junk1;
        public uint offset;
        public uint unCompressedSize;
        public uint compressedSize;

        // not really part of the index
        public int order;
        public string name;
        public uint flag;       // this is ALWAYS 0

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CacheIndex Object\r\n");
            sb.AppendFormat("identity {0}\r\n", identity);
            sb.AppendFormat("junk1 {0}\r\n", junk1);
            sb.AppendFormat("offset {0}\r\n", offset);
            sb.AppendFormat("unCompressedSize {0}\r\n", unCompressedSize);
            sb.AppendFormat("compressedSize {0}\r\n", compressedSize);
            sb.AppendFormat("order {0}\r\n", order);
            sb.AppendFormat("name {0}\r\n", name ?? "no name");
            sb.AppendFormat("flag {0}\r\n", flag);
            return sb.ToString();
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public int CompareTo(CacheIndex other)
        {
            if (this.flag == other.flag)
            {
                return 0;
            }
            if (this.flag > other.flag)
            {
                return 1;
            }
            return -1;
        }

        /// <summary>
        /// Determines if a <see cref="CacheIndex"/> item is the same.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(CacheIndex other)
        {
            if (other.identity != this.identity)
            {
                return false;
            }

            if (other.offset != this.offset)
            {
                return false;
            }

            if (other.flag != this.flag)
            {
                return false;
            }
            if (other.order != this.order)
            {
                return false;
            }
            return true;
        }
    };
}