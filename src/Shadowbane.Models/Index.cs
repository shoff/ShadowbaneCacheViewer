namespace Shadowbane.Models
{
    public class Index
    {
        public Index(ushort position, ushort textureCoordinate, ushort normal)
        {
            this.Position = position;
            this.TextureCoordinate = textureCoordinate;
            this.Normal = normal;
        }

        public ushort Position { get; set; }
        public ushort Normal { get; set; }
        public ushort TextureCoordinate { get; set; }
    }
}