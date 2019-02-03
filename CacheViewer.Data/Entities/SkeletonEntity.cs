namespace CacheViewer.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SkeletonEntity
    {
        public SkeletonEntity()
        {
            this.MotionEntities = new List<MotionEntity>();
        }

        [Key]
        public int SkeletonEntityId { get; set; }

        public string SkeletonText { get; set; }

        public int MotionIdCounter { get; set; }

        public int DistinctMotionCounter { get; set; }

        public ICollection<MotionEntity> MotionEntities { get; set; }

    }
}