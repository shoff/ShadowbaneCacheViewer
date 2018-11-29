namespace CacheViewer.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// </summary>
    [Table("MeshEntities")]
    public class MeshEntity
    {
        [Key]
        public int MeshEntityId { get; set; }

        public int CacheIndexIdentity { get; set; }

        public int CompressedSize { get; set; }

        public int UncompressedSize { get; set; }

        public int FileOffset { get; set; }

        public int VertexCount { get; set; }

        public int NormalsCount { get; set; }

        public int TexturesCount { get; set; }

        public int Id { get; set; }

        public string Vertices { get; set; }

        public string Normals { get; set; }

        public string TextureVectors { get; set; }

        public virtual ICollection<RenderTexture> RenderTextures { get; set; } = new List<RenderTexture>();

        public virtual ICollection<TextureEntity> Textures { get; set; } = new List<TextureEntity>();
    }
}