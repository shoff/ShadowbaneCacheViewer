using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CacheViewer.Domain.Data.Entities
{
    [Table("RenderChildren")]
    public class RenderChild
    {
        [Key]
        public int RenderChildId { get; set; }

        // this is the ParentId
        public int RenderId { get; set; }
    }

}