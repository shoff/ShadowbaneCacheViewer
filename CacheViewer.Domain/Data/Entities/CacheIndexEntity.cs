namespace CacheViewer.Domain.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using Archive;

    public class CacheIndexEntity
    {
        [Key]
        public int CacheIndexEntityId { get; set; }

        public int Offset { get; set; }
        
        public int UnCompressedSize { get; set; }
        
        public int CompressedSize { get; set; }

        public CacheFile File { get; set; }
    }
}