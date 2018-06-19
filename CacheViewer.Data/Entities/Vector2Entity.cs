namespace CacheViewer.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Vector2Entities")]
    public class Vector2Entity
    {
        [Key]
        public int Vector2EntityId { get; set; }

        public string Vector2String { get; set; }
    }
}