﻿namespace CacheViewer.Domain.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using CacheViewer.Domain.Models;

    [Table("CacheObjectEntities")]
    public class CacheObjectEntity
    {
        [Key]
        public int CacheObjectEntityId { get; set; }

        public int CacheIndexIdentity { get; set; }

        public int CompressedSize { get; set; }

        public int UncompressedSize { get; set; }

        public int FileOffset { get; set; }

        public int RenderKey { get; set; }

        public string Name { get; set; }

        public ObjectType ObjectType { get; set; }

        [MaxLength(11)]
        public string ObjectTypeDescription { get; set; }

        public virtual ICollection<RenderAndOffset> RenderAndOffsets { get; set; } = new HashSet<RenderAndOffset>();
    }
}