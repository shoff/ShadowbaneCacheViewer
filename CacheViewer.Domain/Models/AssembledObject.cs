namespace CacheViewer.Domain.Models
{
    public class AssembledObject
    {
        public string Name { get; set; }
        public string ObjectType { get; set; }
        public int CacheId { get; set; }
        public int RenderId { get; set; }
        public string JointName { get; set; }
        public int MeshId { get; set; }
        public int CompressedSize { get; set; }
        public int UncompressedSize { get; set; }
        public int FileOffset { get; set; }
        public int VertexCount { get; set; }
        public int NormalsCount { get; set; }
    }
}