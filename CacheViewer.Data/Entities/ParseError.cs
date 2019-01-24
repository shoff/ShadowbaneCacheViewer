namespace CacheViewer.Data.Entities
{
    public class ParseError
    {
        public long ParseErrorId { get; set; }
        public int CursorOffset { get; set; }
        public ObjectType ObjectType { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public int InnerOffset { get; set; }
        public int RenderId { get; set; }
        public int CacheIndexIdentity { get; set; }
        public int CacheIndexOffset { get; set; }
    }
}