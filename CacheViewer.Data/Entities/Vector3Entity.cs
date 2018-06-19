namespace CacheViewer.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Vector3Entities")]
    public class Vector3Entity
    {
        [Key]
        public int Vector3EntityId { get; set; }

        public string Vector3String { get; set; }
    }
}