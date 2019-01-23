namespace CacheViewer.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class CObjects
    {
        [Key]
        public int CObjectsId { get; set; }
        public int Identity { get; set; }
        public int Junk1 { get; set; }
        public int Offset { get; set; }
        public int UnCompressedSize { get; set; }
        public int CompressedSize { get; set; }

        // not really part of the index
        public int Order { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}