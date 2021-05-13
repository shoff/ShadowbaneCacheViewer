namespace Shadowbane.Cache.IO
{
    using System;
    using System.IO;
    using System.Linq;
    using Models;

    public class CacheObjectBuilder
    {
        public ICacheObject CreateAndParse(uint identity)
        {
            var asset = ArchiveLoader.ObjectArchive[identity];
            using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
            var flag = (ObjectType)reader.ReadInt32();
            return ToObject(flag, reader, asset);
        }

        private ICacheObject ToObject(ObjectType objectType, BinaryReader reader, CacheAsset asset) => objectType switch
        {
            ObjectType.Simple => SimpleType(reader, asset),
            ObjectType.Structure => Structure(reader, asset),
            ObjectType.Interactive => Interactive(reader, asset),
            ObjectType.Equipment => Equipment(reader, asset),
            ObjectType.Mobile => Mobile(reader, asset),
            ObjectType.Deed => Deed(reader, asset),
            ObjectType.Warrant => Warrant(reader, asset),
            ObjectType.Particle => Particle(reader, asset),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        private ICacheObject SimpleType(BinaryReader reader, CacheAsset asset)
        {
            var nameLength = reader.ReadUInt32();
            var name = reader.ReadAsciiString(nameLength);
            reader.BaseStream.Position += 25;
            var offset = (uint)reader.BaseStream.Position;
            var simple = new Simple(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
                .Parse();

            // sucks to have a bad render ids list but baby steps I guess
            foreach (var renderId in simple.RenderIds.Where(r=> !BadRenderIds.IsInList(r)))
            {
                var renderInformation = RenderableObjectBuilder.Build(renderId);
                simple.Renders.Add(renderInformation);
            }

            return simple;
        }
        
        private ICacheObject Structure(BinaryReader reader, CacheAsset asset)
        {
            var nameLength = reader.ReadUInt32();
            var name = reader.ReadAsciiString(nameLength);
            reader.BaseStream.Position += 25;

            var offset = (uint)reader.BaseStream.Position + 25;
            var structure =  new Structure(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
                .Parse();

            // sucks to have a bad render ids list but baby steps I guess
            foreach (var renderId in structure.RenderIds.Where(r => !BadRenderIds.IsInList(r)))
            {
                var renderInformation = RenderableObjectBuilder.Build(renderId);
                structure.Renders.Add(renderInformation);
            }

            return structure;
        }
        
        private ICacheObject Interactive(BinaryReader reader, CacheAsset asset)
        {
            var nameLength = reader.ReadUInt32();
            var name = reader.ReadAsciiString(nameLength);
            reader.BaseStream.Position += 25;
            var offset = (uint)reader.BaseStream.Position + 25;
            return new Interactive(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
                .Parse();
        }
        
        private ICacheObject Equipment(BinaryReader reader, CacheAsset asset)
        {
            var nameLength = reader.ReadUInt32();
            var name = reader.ReadAsciiString(nameLength);
            reader.BaseStream.Position += 25;
            var offset = (uint)reader.BaseStream.Position + 25;
            return new Equipment(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
                .Parse();
        }
        
        private ICacheObject Mobile(BinaryReader reader, CacheAsset asset)
        {
            var nameLength = reader.ReadUInt32();
            var name = reader.ReadAsciiString(nameLength);
            reader.BaseStream.Position += 25;

            var offset = (uint)reader.BaseStream.Position + 25;
            return new Mobile(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
                .Parse();
        }
       
        private ICacheObject Deed(BinaryReader reader, CacheAsset asset)
        {
            var nameLength = reader.ReadUInt32();
            var name = reader.ReadAsciiString(nameLength);
            reader.BaseStream.Position += 25;
            var offset = (uint)reader.BaseStream.Position + 25;
            return new Deed(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
                .Parse();
        }

        private ICacheObject Warrant(BinaryReader reader, CacheAsset asset)
        {
            var nameLength = reader.ReadUInt32();
            var name = reader.ReadAsciiString(nameLength);
            reader.BaseStream.Position += 25;
            // what are we doing with the offset here??
            // so I think this must be the bug? 
            var offset = (uint)reader.BaseStream.Position + 25;
            return new Warrant(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
                .Parse();
        }

        private ICacheObject Particle(BinaryReader reader, CacheAsset asset)
        {
            var nameLength = reader.ReadUInt32();
            var name = reader.ReadAsciiString(nameLength);
            reader.BaseStream.Position += 25;
            // what are we doing with the offset here??
            // so I think this must be the bug? 
            var offset = (uint)reader.BaseStream.Position + 25;
            return new Particle(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
                .Parse();
        }
    }
}