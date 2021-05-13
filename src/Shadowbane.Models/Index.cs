namespace Shadowbane.Models
{
    public class Index
    {
        public Index(ushort position, ushort textureCoordinate, ushort normal) =>
            (this.Position, this.TextureCoordinate, this.Normal) = (position, textureCoordinate, normal);

        public ushort Position { get; }
        public ushort Normal { get; }
        public ushort TextureCoordinate { get; }
    }
}