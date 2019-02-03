namespace CacheViewer.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class MotionEntity
    {
        [Key]
        public int MotionEntityId { get; set; }

        public long CacheIdentity { get; set; }
    }
}