﻿namespace Shadowbane.Cache.IO;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Exporter.File;
using Models;
using Serilog;
using PixelFormat = Models.PixelFormat;

public class RenderableBuilder : IRenderableBuilder
{
    private const int MAX_JOINT_NAME_SIZE = 100;
    private const int MAX_TEXTURE_COUNT = 20;
    private static readonly MeshBuilder meshBuilder = new();

    public Renderable? Build(CacheIndex cacheIndex, bool saveToFile = false)
    {
        return Build(cacheIndex.identity, saveToFile);
    }

    public Renderable? RecurseBuildAndExport(CacheIndex cacheIndex)
    {
        return Build(cacheIndex.identity, true, true, true);
    }

    public Renderable? RecurseBuild(CacheIndex cacheIndex)
    {
        return Build(cacheIndex.identity, false, false, true, false);
    }

    // ReSharper disable once CognitiveComplexity
    public Renderable? Build(uint identity, 
        bool saveToFile = false, 
        bool saveIndexedTextures = false,
        bool parseChildren = false,
        bool buildTexture = true)
    {
        var renderableAsset = ArchiveLoader.RenderArchive[identity];

        if (renderableAsset == null)
        {
            return null;
        }

        var renderable = new Renderable
        {
            Identity = identity,
            CacheIndex = ArchiveLoader.RenderArchive.CacheIndices.First(c => c.identity == identity),
            ByteCount = renderableAsset.Asset.Length,
            Data = renderableAsset.Asset
        };


        using var reader = renderableAsset.Asset.CreateBinaryReaderUtf32();
        renderable.RenderType = reader.ReadUInt32();
        reader.BaseStream.Position = 35;
        renderable.HasMesh = reader.ReadUInt32() == 1;
        renderable.Unknown[0] = reader.ReadUInt32();
        renderable.MeshId = reader.ReadUInt32();

        if (renderable is { HasMesh: true, MeshId: 0 } && saveToFile 
            && !File.Exists($"{CacheLocation.RenderOutputFolder.FullName}missing_mesh\\{identity}-{renderableAsset.Order}"))
        {
            // for now let's save them all in a folder for analysis
            FileWriter.Writer.Write(renderableAsset.Asset.Span, CacheLocation.RenderOutputFolder.FullName + "missing_mesh", $"{identity}-{renderableAsset.Order}.sbri");
        }

        // build the mesh
        if (renderable is { HasMesh: true, MeshId: > 0 } 
            && IdLookup.IsValidMeshId(renderable.MeshId))
        {
            var meshAsset = ArchiveLoader.MeshArchive[renderable.MeshId];
            var mesh = meshBuilder.Build(meshAsset!.Asset, renderable.MeshId);
           
            if (mesh == null)
            {
                throw new InvalidMeshException($"Could not build mesh with identity {renderable.MeshId}");
            }
            
            renderable.Mesh = mesh;
        }
        else if (renderable.HasMesh)
        {
            throw new InvalidMeshException($"{renderable.Identity} claims to have a mesh but the mesh id {renderable.MeshId} is not valid");
        }

        Debug.Assert(reader.BaseStream.Position == 47);
        renderable.Unknown[1] = reader.ReadUInt16();
        renderable.LastOffset = reader.BaseStream.Position;

        // its seems that when the render type (nfi if that's what the first 4 bytes really are but let's go with it) is type 257
        // there is no joint name, that it claims to have a mesh but no mesh id,

        renderable.JointNameSize = reader.ReadUInt32();

        if (renderable.JointNameSize > MAX_JOINT_NAME_SIZE)
        {
            // no joint has a name that long
            renderable.Notes.Add(
                $" We read a joint name size of {renderable.JointNameSize} this can't be correct.");

#pragma warning disable S112 // General exceptions should never be thrown
            throw new ParseException($"Parsing {renderable.CacheIndex.identity} a JointMeshSize of " +
                                     $"{renderable.JointNameSize} was read. This is obviously incorrect.");
#pragma warning restore S112 // General exceptions should never be thrown
        }

        renderable.JointName = reader.AsciiString(renderable.JointNameSize); // should be 
        renderable.Scale = reader.SafeReadToVector3();
        if (renderable is { HasMesh: true, Mesh: { } })
            //if (renderInformation.Mesh != null)
        {
            renderable.Mesh.Scale = renderable.Scale;
        }

        renderable.LastOffset = reader.BaseStream.Position;
        // I think this is probably a bool or flag of some kind
        renderable.Unknown[2] = reader.SafeReadUInt32();
        renderable.LastOffset = reader.BaseStream.Position;
        // these need to go to mesh not here :)
        // object position ?
        renderable.Position = reader.SafeReadToVector3();
        if (renderable is { HasMesh: true, Mesh: { } })
            //if (renderInformation.Mesh != null)
        {
            renderable.Mesh.Position = renderable.Position;
        }

        renderable.LastOffset = reader.BaseStream.Position;
        renderable.ChildCount = reader.SafeReadInt32();
        renderable.LastOffset = reader.BaseStream.Position;

        for (var i = 0; i < renderable.ChildCount; i++)
        {
            // null bytes
            reader.SafeReadInt32();
            var childId = reader.SafeReadInt32();
            renderable.ChildRenderIdList.Add(childId);

            if (!parseChildren)
            {
                continue;
            }

            var child = Build((uint)childId, saveToFile, saveIndexedTextures, true, buildTexture);
            if (child != null)
            {
                renderable.Children.Add(child);
            }
            else
            {
                Log.Error($"null renderable returned from render builder for render id {childId}");
            }

        }

        if (reader.CanRead(1))
        {
            renderable.HasTexture = reader.ReadByte() == 1;
            renderable.LastOffset = reader.BaseStream.Position;
        }

        if (renderable.HasTexture && buildTexture)
        {
            // I don't believe this is correct.
            // lets save any render info where the count is greater than 1 but less than max
            renderable.TextureCount = reader.SafeReadUInt32();

            if (renderable.FirstInt != 257)
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

            if (renderable is { TextureCount: <= MAX_TEXTURE_COUNT, HasTexture: true })
            {
                if (renderable.TextureCount > 1 && saveToFile
                                                       && !File.Exists($"{CacheLocation.RenderOutputFolder.FullName}multi-texture\\{identity}-{renderableAsset.Order}"))
                {
                    // for now let's save them all in a folder for analysis
                    FileWriter.Writer.Write(renderableAsset.Asset.Span, CacheLocation.RenderOutputFolder.FullName + "multi-texture", $"{identity}-{renderableAsset.Order}.sbri");
                }

                for (int i = 0; i < renderable.TextureCount; i++)
                {
                    var textureId = reader.SafeReadUInt32();
                    if (textureId > 0)
                    {
                        renderable.Textures.Add(textureId);
                        CacheAsset? textureAsset = ArchiveLoader.TextureArchive[textureId];
                        
                        if (textureAsset is { IsValid: true })
                        {
                            try
                            {
                                var texture = new Texture(textureAsset.Asset, textureId);
                                if (texture.Image == null && texture.PixelFormat == PixelFormat.Indexed &&
                                    saveIndexedTextures)
                                {
                                    // probably an indexed image lets save it to look at the bytes
                                    FileWriter.Writer.Write(textureAsset.Asset.Span,
                                        $"{CacheLocation.TextureFolder.FullName}\\indexed-images", $"{identity}.sbtex");
                                }

                                renderable.Mesh?.Textures?.Add(texture);
                            }
                            catch (Exception e)
                            {
                                Log.Error(e,
                                    $"Unable to create texture for {textureId}! on renderable {identity}!");
                                continue;
                            }
                        }
                        else
                        {
                            renderable.Notes.Add($"{textureId} failed validation check, not added.");
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

        renderable.IsValid = IsValid();

        bool IsValid()
        {
            bool result = true;
            var sb = new StringBuilder();
            sb.Append(renderable.Notes);
            if (renderable is { HasMesh: true, Mesh: null })
            {
                sb.Append("This claims to have a mesh, but no mesh id was found or no mesh could be parsed.");
                sb.Append($"MeshId: {renderable.MeshId}");
                result = false;
            }

            if (renderable is { HasTexture: true, Textures.Count: 0 })
            {
                sb.Append("This claims to have a texture, but no texture id was found or no texture could be parsed.");
                sb.Append($"Texture Count: {renderable.TextureCount}");
                result = false;
            }
            return result;
        }

        return renderable;
    }
}