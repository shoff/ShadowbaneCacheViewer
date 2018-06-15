namespace CacheViewer.Domain.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class RenderAndOffset
    {
        [Key]
        public int RenderAndOffsetId { get; set; }

        public int RenderId { get; set; }

        public long OffSet { get; set; }

    }


}