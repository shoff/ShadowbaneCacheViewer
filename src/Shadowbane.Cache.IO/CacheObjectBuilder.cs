using System.Threading.Tasks;

namespace Shadowbane.Cache.IO;

using System;
using System.IO;
using System.Linq;
using Models;
using Serilog;

public class CacheObjectBuilder : ICacheObjectBuilder
{
    private readonly IRenderableBuilder? renderableObjectBuilder;

    public CacheObjectBuilder(IRenderableBuilder? renderableBuilder = null)
    {
        this.renderableObjectBuilder = renderableBuilder ?? new RenderableBuilder();
    }

    public ICacheObject? NameOnly(uint identity)
    {
        var asset = ArchiveLoader.ObjectArchive[identity];
        if (asset == null)
        {
            Log.Error($"Unable to create cache object for identity {identity}");
            return null;
        }

        using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
        var flag = (ObjectType)reader.ReadInt32();
        var nameLength = reader.ReadUInt32();
        var name = reader.AsciiString(nameLength);
        reader.BaseStream.Position += 25;
        var renderId = reader.ReadUInt32();

        return new CacheObjectNameOnly()
        {
            Identity = identity,
            Name = name,
            Flag = flag,
            RenderId = renderId
        };
    }

    public ICacheObject? CreateAndParse(uint identity)
    {
        var asset = ArchiveLoader.ObjectArchive[identity];
        if (asset == null)
        {
            Log.Error($"Unable to create cache object for identity {identity}");
            return null;
        }
        
        using var reader = asset.Asset.CreateBinaryReaderUtf32(4);
        var flag = (ObjectType)reader.ReadInt32();
        return ToObject(flag, reader, asset);
    }

    private ICacheObject? ToObject(ObjectType objectType, BinaryReader reader, CacheAsset asset) 
        => objectType switch
    {
        ObjectType.Sun => Sun(reader, asset),
        ObjectType.Simple => SimpleType(reader, asset),
        ObjectType.Structure => Structure(reader, asset),
        ObjectType.Interactive => Interactive(reader, asset),
        ObjectType.Equipment => Equipment(reader, asset),
        ObjectType.Mobile => Mobile(reader, asset),
        ObjectType.Deed => Deed(reader, asset),
        ObjectType.Unknown => Unknown(reader, asset),
        ObjectType.Warrant => Warrant(reader, asset),
        ObjectType.Particle => Particle(reader, asset),
        _ => null
    };
    
    private ICacheObject Sun(BinaryReader reader, CacheAsset asset)
    {
        var nameLength = reader.ReadUInt32();
        var name = reader.AsciiString(nameLength);
        reader.BaseStream.Position += 25;
        var offset = (uint)reader.BaseStream.Position;
        var simple = new Simple(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
            .Parse();

        // sucks to have a bad render ids list but baby steps I guess
        foreach (var renderId in simple.RenderIds.Where(r => !BadRenderIds.IsInList(r)))
        {
            var renderInformation = this.renderableObjectBuilder?.Build(renderId);

            if (renderInformation == null)
            {
                Log.Error($"render builder unable to build renderable {renderId}!");
                continue;
            }

            simple.Renders.Add(renderInformation);
        }

        return simple;
    }

    private ICacheObject SimpleType(BinaryReader reader, CacheAsset asset)
    {
        var nameLength = reader.ReadUInt32();
        var name = reader.AsciiString(nameLength);
        reader.BaseStream.Position += 25;
        var offset = (uint)reader.BaseStream.Position;
        var simple = new Simple(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
            .Parse();

        // sucks to have a bad render ids list but baby steps I guess
        foreach (var renderId in simple.RenderIds.Where(r=> !BadRenderIds.IsInList(r)))
        {
            var renderInformation = this.renderableObjectBuilder?.Build(renderId);

            if (renderInformation == null)
            {
                Log.Error($"render builder unable to build renderable {renderId}!");
                continue;
            }
            
            simple.Renders.Add(renderInformation);
        }

        return simple;
    }
        
    private ICacheObject Structure(BinaryReader reader, CacheAsset asset)
    {
        var nameLength = reader.ReadUInt32();
        var name = reader.AsciiString(nameLength);
        reader.BaseStream.Position += 25;

        var offset = (uint)reader.BaseStream.Position + 25;
        var structure =  new Structure(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
            .Parse();

        // sucks to have a bad render ids list but baby steps I guess
        foreach (var renderId in structure.RenderIds.Where(r => !BadRenderIds.IsInList(r)))
        {
            try
            {
                var renderInformation = this.renderableObjectBuilder?.Build(renderId);
                if (renderInformation == null)
                {
                    // not sure wtf to do or why this is suddenly not correct!
                    Log.Error($"Could not create renderable for {renderId}!");
                    continue;
                }
                structure.Renders.Add(renderInformation);
            }
            catch (Exception)
            {
                structure.InvalidRenderIds.Add(renderId);
            }
        }

        return structure;
    }
        
    private ICacheObject Interactive(BinaryReader reader, CacheAsset asset)
    {
        var nameLength = reader.ReadUInt32();
        var name = reader.AsciiString(nameLength);
        reader.BaseStream.Position += 25;
        var offset = (uint)reader.BaseStream.Position + 25;
        var interactive = new Interactive(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
            .Parse();

        // sucks to have a bad render ids list but baby steps I guess
        foreach (var renderId in interactive.RenderIds.Where(r => !BadRenderIds.IsInList(r)))
        {
            var renderInformation = this.renderableObjectBuilder?.Build(renderId);
            if (renderInformation == null)
            {
                // not sure wtf to do or why this is suddenly not correct!
                Log.Error($"Could not create renderable for {renderId}!");
                continue;
            }
            interactive.Renders.Add(renderInformation);
        }

        return interactive;
    }
        
    private ICacheObject Equipment(BinaryReader reader, CacheAsset asset)
    {
        var nameLength = reader.ReadUInt32();
        var name = reader.AsciiString(nameLength);
        reader.BaseStream.Position += 25;
        var offset = (uint)reader.BaseStream.Position + 25;
        var equipment = new Equipment(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
            .Parse();

        // sucks to have a bad render ids list but baby steps I guess
        foreach (var renderId in equipment.RenderIds.Where(r => !BadRenderIds.IsInList(r)))
        {
            var renderInformation = this.renderableObjectBuilder?.Build(renderId);
            if (renderInformation == null)
            {
                // not sure wtf to do or why this is suddenly not correct!
                Log.Error($"Could not create renderable for {renderId}!");
                continue;
            }
            equipment.Renders.Add(renderInformation);
        }

        return equipment;
    }

    private ICacheObject Unknown(BinaryReader reader, CacheAsset asset)
    {
        var nameLength = reader.ReadUInt32();
        var name = reader.AsciiString(nameLength);
        reader.BaseStream.Position += 25;
        var offset = (uint)reader.BaseStream.Position;
        var simple = new Simple(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
            .Parse();

        // sucks to have a bad render ids list but baby steps I guess
        foreach (var renderId in simple.RenderIds.Where(r => !BadRenderIds.IsInList(r)))
        {
            var renderInformation = this.renderableObjectBuilder?.Build(renderId);

            if (renderInformation == null)
            {
                Log.Error($"render builder unable to build renderable {renderId}!");
                continue;
            }

            simple.Renders.Add(renderInformation);
        }

        return simple;
    }
    
    private ICacheObject Mobile(BinaryReader reader, CacheAsset asset)
    {
        var nameLength = reader.ReadUInt32();
        var name = reader.AsciiString(nameLength);
        reader.BaseStream.Position += 25;

        var offset = (uint)reader.BaseStream.Position + 25;
        var mobile =  new Mobile(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
            .Parse();

        // sucks to have a bad render ids list but baby steps I guess
        foreach (uint renderId in mobile.RenderIds.Where(r => !BadRenderIds.IsInList(r)))
        {
            var renderInformation = this.renderableObjectBuilder?.Build(renderId);
            if (renderInformation == null)
            {
                // not sure wtf to do or why this is suddenly not correct!
                Log.Error($"Could not create renderable for {renderId}!");
                continue;
            }
            mobile.Renders.Add(renderInformation);
        }

        return mobile;
    }
       
    private ICacheObject Deed(BinaryReader reader, CacheAsset asset)
    {
        var nameLength = reader.ReadUInt32();
        var name = reader.AsciiString(nameLength);
        reader.BaseStream.Position += 25;
        var offset = (uint)reader.BaseStream.Position + 25;
        return new Deed(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
            .Parse();
    }

    private ICacheObject Warrant(BinaryReader reader, CacheAsset asset)
    {
        var nameLength = reader.ReadUInt32();
        var name = reader.AsciiString(nameLength);
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
        var name = reader.AsciiString(nameLength);
        reader.BaseStream.Position += 25;
        // what are we doing with the offset here??
        // so I think this must be the bug? 
        var offset = (uint)reader.BaseStream.Position + 25;
        return new Particle(asset.CacheIndex.identity, name, offset, asset.Asset, offset)
            .Parse();
    }

}