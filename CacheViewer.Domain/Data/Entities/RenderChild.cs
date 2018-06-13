namespace CacheViewer.Domain.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RenderChildren")]
    public class RenderChild
    {
        [Key]
        public int RenderChildId { get; set; }

        // this is the ParentId
        public int RenderId { get; set; }
    }
}