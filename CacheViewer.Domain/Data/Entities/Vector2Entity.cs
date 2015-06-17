using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CacheViewer.Domain.Data.Entities
{
    [Table("Vector2Entities")]
    public class Vector2Entity
    {
        [Key]
        public int Vector2EntityId { get; set; }

        public string Vector2String { get; set; }
    }
}