namespace CacheViewer.Domain.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Models;

    public class MotionEntity
    {
        [Key]
        public int MotionEntityId { get; set; }

        public long CacheIdentity { get; set; }
    }


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

        public static implicit operator SkeletonEntity(Skeleton skeleton)
        {
            var se = new SkeletonEntity();
            se.SkeletonText = skeleton.SkeletonText;
            se.MotionIdCounter = (int) skeleton.MotionCount;
            se.DistinctMotionCounter = skeleton.DistinctMotionIdCount;
            foreach (var mi in skeleton.MotionIds)
            {
                se.MotionEntities.Add(new MotionEntity
                {
                    CacheIdentity = (int) mi
                });
            }

            return se;
        }
    }
}