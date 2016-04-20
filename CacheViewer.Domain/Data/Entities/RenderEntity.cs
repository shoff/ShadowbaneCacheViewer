﻿

namespace CacheViewer.Domain.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RenderEntities")]   
    public class RenderEntity
    {
        public RenderEntity()
        {
            this.Children = new List<RenderChild>();
        }

        [Key]
        public int RenderEntityId { get; set; }

        public int CacheIndexIdentity { get; set; }
        
        public int ByteCount { get; set; }

        public int Order { get; set; }

        public bool HasMesh { get; set; }
        
        public int MeshId { get; set; }

        public string JointName { get; set; }

        [MaxLength(64)]
        public string Scale { get; set; }

        [MaxLength(64)]
        public string Position { get; set; }

        public int RenderCount { get; set; }
        
        public int CompressedSize { get; set; }

        public int UncompressedSize { get; set; }

        public int FileOffset { get; set; }

        public int? TextureId { get; set; }

        public string Notes { get; set; }

        public List<RenderChild> Children { get; set; }
    }
}