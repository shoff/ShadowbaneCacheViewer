namespace CacheViewer.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RenderChildren")]
    public class RenderChild
    {
        [Key]
        public int DatabaseRenderId { get; set; }
        [ForeignKey("Parent")]
        public int ParentId { get; set; }
        public virtual RenderEntity Parent { get; set; }
        [ForeignKey("Child")]
        public int ChildRenderId { get; set; }
        public virtual RenderEntity Child { get; set; }
    }
}