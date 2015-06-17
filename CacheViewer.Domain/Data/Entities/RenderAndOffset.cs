namespace CacheViewer.Domain.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class RenderAndOffset
    {
        [Key]
        public int RenderAndOffsetId { get; set; }

        public int RenderId { get; set; }

        public long Offset { get; set; }

        public int  CacheIndexIdentity { get; set; }
    }
}