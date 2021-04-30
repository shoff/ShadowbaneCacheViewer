namespace Shadowbane.Cache
{
    using System.Text;

    public struct CacheIndex
    {
        public uint Junk1;
        public uint Identity;
        public uint Offset;
        public uint UnCompressedSize;
        public uint CompressedSize;
 
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("CacheIndex Object\r\n");
            sb.AppendFormat("identity {0}\r\n", this.Identity);
            sb.AppendFormat("junk1 {0}\r\n", this.Junk1);
            sb.AppendFormat("offset {0}\r\n", this.Offset);
            sb.AppendFormat("unCompressedSize {0}\r\n", this.UnCompressedSize);
            sb.AppendFormat("compressedSize {0}\r\n", this.CompressedSize);
            return sb.ToString();
        }
    }
}