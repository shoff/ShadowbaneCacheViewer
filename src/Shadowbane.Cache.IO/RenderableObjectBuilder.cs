namespace Shadowbane.Cache.IO;

using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Exporter.File;
using Models;

public static class RenderableObjectBuilder
{
    private const int MAX_JOINT_NAME_SIZE = 100;
    private const int MAX_TEXTURE_COUNT = 20;
    private static readonly MeshBuilder meshBuilder = new();

    public static Renderable Build(CacheIndex cacheIndex, bool saveToFile = false)
    {
        return Build(cacheIndex.identity, saveToFile);
    }

    public static Renderable RecurseBuildAndExport(CacheIndex cacheIndex)
    {
        return Build(cacheIndex.identity, true, true, true);
    }

    public static Renderable RecurseBuild(CacheIndex cacheIndex)
    {
        return Build(cacheIndex.identity, false, false, true, false);
    }

    public static Renderable Build(uint identity, 
        bool saveToFile = false, 
        bool saveIndexedTextures = false,
        bool parseChildren = false,
        bool buildTexture = true)
    {
        var asset = ArchiveLoader.RenderArchive[identity];

        var renderInformation = new Renderable
        {
            Identity = identity,
            CacheIndex = ArchiveLoader.RenderArchive[identity].CacheIndex,
            ByteCount = asset.Asset.Length
        };

        if (saveToFile && !File.Exists($"{CacheLocation.RenderOutputFolder.FullName}{identity}-{asset.Order}"))
        {
            // for now let's save them all in a folder for analysis
            FileWriter.Writer.Write(asset.Asset.Span, CacheLocation.RenderOutputFolder.FullName, $"{identity}-{asset.Order}.sbri");
        }

        using var reader = asset.Asset.CreateBinaryReaderUtf32();
        renderInformation.RenderType = reader.ReadUInt32();
        reader.BaseStream.Position = 35;
        renderInformation.HasMesh = reader.ReadUInt32() == 1;
        renderInformation.Unknown[0] = reader.ReadUInt32();
        renderInformation.MeshId = reader.ReadUInt32();

        if (renderInformation.HasMesh && renderInformation.MeshId == 0 && saveToFile
            && !File.Exists($"{CacheLocation.RenderOutputFolder.FullName}missing_mesh\\{identity}-{asset.Order}"))
        {
            // for now let's save them all in a folder for analysis
            FileWriter.Writer.Write(asset.Asset.Span, CacheLocation.RenderOutputFolder.FullName + "missing_mesh", $"{identity}-{asset.Order}.sbri");
        }

        // build the mesh
        if (renderInformation.HasMesh && renderInformation.MeshId > 0)
        {
            var meshAsset = ArchiveLoader.MeshArchive[renderInformation.MeshId];
            var mesh = meshBuilder.Build(meshAsset.Asset, renderInformation.MeshId);
            if (mesh == null)
            {
                throw new InvalidMeshException($"Could not build mesh with identity {renderInformation.MeshId}");
            }
            renderInformation.Mesh = mesh;
        }

        Debug.Assert(reader.BaseStream.Position == 47);
        renderInformation.Unknown[1] = reader.ReadUInt16();
        renderInformation.LastOffset = reader.BaseStream.Position;

        // its seems that when the render type (nfi if that's what the first 4 bytes really are but let's go with it) is type 257
        // there is no joint name, that it claims to have a mesh but no mesh id,

        renderInformation.JointNameSize = reader.ReadUInt32();

        if (renderInformation.JointNameSize > MAX_JOINT_NAME_SIZE)
        {
            // no joint has a name that long
            renderInformation.Notes.Add(
                $" We read a joint name size of {renderInformation.JointNameSize} this can't be correct.");

#pragma warning disable S112 // General exceptions should never be thrown
            throw new ParseException($"Parsing {renderInformation.CacheIndex.identity} a JointMeshSize of " +
                                     $"{renderInformation.JointNameSize} was read. This is obviously incorrect.");
#pragma warning restore S112 // General exceptions should never be thrown
        }

        renderInformation.JointName = reader.AsciiString(renderInformation.JointNameSize); // should be 
        renderInformation.Scale = reader.SafeReadToVector3();
        if (renderInformation.HasMesh && renderInformation.Mesh != null)
            //if (renderInformation.Mesh != null)
        {
            renderInformation.Mesh.Scale = renderInformation.Scale;
        }

        renderInformation.LastOffset = reader.BaseStream.Position;
        // I think this is probably a bool or flag of some kind
        renderInformation.Unknown[2] = reader.SafeReadUInt32();
        renderInformation.LastOffset = reader.BaseStream.Position;
        // these need to go to mesh not here :)
        // object position ?
        renderInformation.Position = reader.SafeReadToVector3();
        if (renderInformation.HasMesh && renderInformation.Mesh != null)
            //if (renderInformation.Mesh != null)
        {
            renderInformation.Mesh.Position = renderInformation.Position;
        }

        renderInformation.LastOffset = reader.BaseStream.Position;
        renderInformation.ChildCount = reader.SafeReadInt32();
        renderInformation.LastOffset = reader.BaseStream.Position;

        for (int i = 0; i < renderInformation.ChildCount; i++)
        {
            // null bytes
            reader.SafeReadInt32();
            var childId = reader.SafeReadInt32();
            renderInformation.ChildRenderIdList.Add(childId);

            if (parseChildren)
            {
                renderInformation.Children.Add(
                    Build((uint) childId, saveToFile, saveIndexedTextures, true, buildTexture));
            }

        }

        if (reader.CanRead(1))
        {
            renderInformation.HasTexture = reader.ReadByte() == 1;
            renderInformation.LastOffset = reader.BaseStream.Position;
        }

        if (renderInformation.HasTexture && buildTexture)
        {
            // I don't believe this is correct.
            // lets save any render info where the count is greater than 1 but less than max
            renderInformation.TextureCount = reader.SafeReadUInt32();

            if (renderInformation.FirstInt != 257)
            {
                // TODO I should be doing something with this data!
                if (reader.CanRead(8))
                {
                    // seems to always be a 1 or 0
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                }
            }
            else
            {
                reader.SafeReadUInt32();
                reader.SafeReadUInt32();
            }

            if (renderInformation.TextureCount <= MAX_TEXTURE_COUNT && renderInformation.HasTexture)
            {
                if (renderInformation.TextureCount > 1 && saveToFile
                                                       && !File.Exists($"{CacheLocation.RenderOutputFolder.FullName}multi-texture\\{identity}-{asset.Order}"))
                {
                    // for now let's save them all in a folder for analysis
                    FileWriter.Writer.Write(asset.Asset.Span, CacheLocation.RenderOutputFolder.FullName + "multi-texture", $"{identity}-{asset.Order}.sbri");
                }

                for (int i = 0; i < renderInformation.TextureCount; i++)
                {
                    var textureId = reader.SafeReadUInt32();
                    if (textureId > 0)
                    {
                        renderInformation.Textures.Add(textureId);
                        var textureAsset = ArchiveLoader.TextureArchive[textureId];
                        if (textureAsset.IsValid)
                        {
                            var texture = new Texture(textureAsset.Asset, textureId);
                            if (texture.Image == null && texture.PixelFormat == PixelFormat.Indexed && saveIndexedTextures)
                            {
                                // probably an indexed image lets save it to look at the bytes
                                FileWriter.Writer.Write(textureAsset.Asset.Span, $"{CacheLocation.TextureFolder.FullName}\\indexed-images", $"{identity}.sbtex");

                            }

                            renderInformation.Mesh?.Textures?.Add(texture);
                        }
                        else
                        {
                            renderInformation.Notes.Add($"{textureId} failed validation check, not added.");
                        }
                    }

                    // WHY?
                    if (reader.CanRead(34))
                    {
                        reader.BaseStream.Position += 34;
                    }
                }
            }
        }

        renderInformation.IsValid = IsValid();

        bool IsValid()
        {
            bool result = true;
            var sb = new StringBuilder();
            sb.Append(renderInformation.Notes);
            if (renderInformation.HasMesh && renderInformation.Mesh == null)
            {
                sb.Append("This claims to have a mesh, but no mesh id was found or no mesh could be parsed.");
                sb.Append($"MeshId: {renderInformation.MeshId}");
                result = false;
            }

            if (renderInformation.HasTexture && renderInformation.Textures.Count == 0)
            {
                sb.Append("This claims to have a texture, but no texture id was found or no texture could be parsed.");
                sb.Append($"Texture Count: {renderInformation.TextureCount}");
                result = false;
            }
            return result;
        }

        return renderInformation;
    }
}