using System.Runtime.InteropServices;
using System.Text;

namespace CacheViewer.Domain.Archive
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CacheHeader
    {
        public uint indexCount;
        public uint dataOffset;	// File offset to where the data chunks begin
        public uint fileSize;	    // total size of the file
        public uint junk1;		// 0xFFFF ffff
        public long indexOffset;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("\r\nIndexCount: {0}", indexCount);
            sb.AppendFormat("\r\nDataOffset: {0}", dataOffset);
            sb.AppendFormat("\r\nFileSize: {0}", fileSize);
            sb.AppendFormat("\r\nJunk1: {0}", junk1);
            return sb.ToString();
        }
    };
}