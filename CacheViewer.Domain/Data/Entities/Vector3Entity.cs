using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CacheViewer.Domain.Data.Entities
{
    [Table("Vector3Entities")]
    public class Vector3Entity
    {
        [Key]
        public int Vector3EntityId { get; set; }

        public string Vector3String { get; set; }
    }
}