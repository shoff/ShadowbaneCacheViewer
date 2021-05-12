namespace Shadowbane.Cache
{
    using System.Runtime.InteropServices;
    using System.Text;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct CacheIndex
    {
        public uint junk1;
        public uint identity;
        public uint offset;
        public uint unCompressedSize;
        public uint compressedSize;

        public bool IsValid()
        {
            return this.unCompressedSize > 0 && this.identity > 0 && this.compressedSize > 0;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("CacheIndex Object\r\n");
            sb.AppendFormat("identity {0}\r\n", this.identity);
            sb.AppendFormat("junk1 {0}\r\n", this.junk1);
            sb.AppendFormat("offset {0}\r\n", this.offset);
            sb.AppendFormat("unCompressedSize {0}\r\n", this.unCompressedSize);
            sb.AppendFormat("compressedSize {0}\r\n", this.compressedSize);
            return sb.ToString();
        }
    }
}