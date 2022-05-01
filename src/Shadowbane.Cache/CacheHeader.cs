namespace Shadowbane.Cache;

using System.Runtime.InteropServices;
using System.Text;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct CacheHeader
{
    public uint indexCount;
    public uint dataOffset; // File offset to where the data chunks begin
    public uint fileSize;   // total size of the file
    public uint junk1;      // 0xFFFF ffff
        
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendFormat("\r\nIndexCount: {0}", this.indexCount);
        sb.AppendFormat("\r\nDataOffset: {0}", this.dataOffset);
        sb.AppendFormat("\r\nFileSize: {0}", this.fileSize);
        sb.AppendFormat("\r\nJunk1: {0}", this.junk1);
        return sb.ToString();
    }
}