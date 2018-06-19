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
        /// <summary>
        /// </summary>
        [Key]
        public int MeshEntityId { get; set; }

        /// <summary>
        /// </summary>
        public int CacheIndexIdentity { get; set; }

        /// <summary>
        /// </summary>
        public int CompressedSize { get; set; }

        /// <summary>
        /// </summary>
        public int UncompressedSize { get; set; }

        /// <summary>
        /// </summary>
        public int FileOffset { get; set; }

        /// <summary>
        /// </summary>
        public int VertexCount { get; set; }

        /// <summary>
        /// </summary>
        public int NormalsCount { get; set; }

        /// <summary>
        /// </summary>
        public int TexturesCount { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        public string Vertices { get; set; }

        /// <summary>
        /// </summary>
        public string Normals { get; set; }

        /// <summary>
        /// </summary>
        public string TextureVectors { get; set; }

        public virtual ICollection<RenderTexture> RenderTextures { get; set; } = new List<RenderTexture>();

        public virtual ICollection<TextureEntity> Textures { get; set; } = new List<TextureEntity>();
    }
}