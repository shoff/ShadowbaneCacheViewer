// ReSharper disable InconsistentNaming
namespace CacheViewer.Domain.Archive
{
    using System;
    using System.Text;

    public struct CacheIndex : IComparable<CacheIndex>, IEquatable<CacheIndex>
    {
        public int Index;
        public int Identity;
        public uint Junk1;
        public uint Offset;
        public uint UnCompressedSize;
        public uint CompressedSize;

        // not really part of the index
        public int Order;
        public string Name;
        public uint Flag; // this is ALWAYS 0

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("CacheIndex Object\r\n");
            sb.Append($"index {this.Index}\r\n");
            sb.AppendFormat("identity {0}\r\n", this.Identity);
            sb.AppendFormat("junk1 {0}\r\n", this.Junk1);
            sb.AppendFormat("offset {0}\r\n", this.Offset);
            sb.AppendFormat("unCompressedSize {0}\r\n", this.UnCompressedSize);
            sb.AppendFormat("compressedSize {0}\r\n", this.CompressedSize);
            sb.AppendFormat("order {0}\r\n", this.Order);
            sb.AppendFormat("name {0}\r\n", this.Name ?? "no name");
            sb.AppendFormat("flag {0}\r\n", this.Flag);
            return sb.ToString();
        }

        /// <summary>
        ///     Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public int CompareTo(CacheIndex other)
        {
            if (this.Flag == other.Flag)
            {
                return 0;
            }

            if (this.Flag > other.Flag)
            {
                return 1;
            }

            return -1;
        }

        /// <summary>
        ///     Determines if a <see cref="CacheIndex" /> item is the same.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(CacheIndex other)
        {
            if (other.Identity != this.Identity)
            {
                return false;
            }

            if (other.Offset != this.Offset)
            {
                return false;
            }

            if (other.Flag != this.Flag)
            {
                return false;
            }

            if (other.Order != this.Order)
            {
                return false;
            }

            return true;
        }
    }
}