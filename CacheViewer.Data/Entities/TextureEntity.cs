namespace CacheViewer.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TextureEntity
    {
        [Key]
        public int TextureEntityId { get; set; }
        public int TextureId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public virtual ICollection<MeshEntity> Meshes { get; set; } = new List<MeshEntity>();
    }
}