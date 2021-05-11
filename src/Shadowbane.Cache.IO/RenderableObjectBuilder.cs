namespace Shadowbane.Cache.IO
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Exporter.File;
    using Models;

    public static class RenderableObjectBuilder
    {
        private const int MAX_JOINT_NAME_SIZE = 100;
        private static string storeFolder = $"{AppDomain.CurrentDomain.BaseDirectory}/render-store";

        public static RenderInformation Create(CacheIndex cacheIndex, bool saveToFile = false)
        {
            return Create(cacheIndex.identity, saveToFile);
        }

        public static RenderInformation Create(uint identity, bool saveToFile = false)
        {
            var asset = ArchiveLoader.RenderArchive[identity];
            var renderInformation = new RenderInformation
            {
                Identity = identity,
                CacheIndex = ArchiveLoader.RenderArchive[identity].CacheIndex,
                ByteCount = asset.Asset.Length,
                Unknown = new object[6]
            };

            if (saveToFile && !File.Exists($"{identity}-{asset.Order}"))
            {
                // for now let's save them all in a folder for analysis
                FileWriter.Writer.Write(asset.Asset.Span, storeFolder, $"{identity}-{asset.Order}");
            }

            using var reader = asset.Asset.CreateBinaryReaderUtf32(35);
            renderInformation.HasMesh = reader.ReadUInt32() == 1;
            Debug.Assert(reader.BaseStream.Position == 39);
            
            renderInformation.Unknown[0] = reader.ReadUInt32();
            renderInformation.MeshId = reader.ReadInt32();
            Debug.Assert(reader.BaseStream.Position == 47);

            renderInformation.Unknown[1] = reader.ReadUInt16();
            renderInformation.LastOffset = reader.BaseStream.Position;

            renderInformation.JointNameSize = reader.ReadUInt32();

            if (renderInformation.JointNameSize > MAX_JOINT_NAME_SIZE)
            {
                // no joint has a name that long
                renderInformation.Notes +=
                    $" We read a joint name size of {renderInformation.JointNameSize} this can't be correct.";

#pragma warning disable S112 // General exceptions should never be thrown
                throw new ParseException($"Parsing {renderInformation.CacheIndex.identity} a JointMeshSize of " +
                    $"{renderInformation.JointNameSize} was read. This is obviously incorrect.");
#pragma warning restore S112 // General exceptions should never be thrown
            }

            // TODO should not key off the arbitrariness of the byte stream length!
            if (reader.BaseStream.Position + renderInformation.JointNameSize <= renderInformation.ByteCount)
            {
                renderInformation.JointName = reader.ReadAsciiString(renderInformation.JointNameSize);
                renderInformation.LastOffset = reader.BaseStream.Position;
            }

            if (reader.BaseStream.Position + 12 <= renderInformation.ByteCount)
            {
                // object scale ?
                renderInformation.Scale = reader.ReadToVector3();
                renderInformation.LastOffset = reader.BaseStream.Position;
            }

            if (reader.CanRead(4))
            {
                // I think this is probably a bool or flag of some kind
                renderInformation.Unknown[2] = reader.ReadUInt32();
                renderInformation.LastOffset = reader.BaseStream.Position;
            }

            if (reader.CanRead(12))
            {
                // object position ?
                renderInformation.Position = reader.ReadToVector3();
                renderInformation.LastOffset = reader.BaseStream.Position;
            }

            if (reader.CanRead(4))
            {
                renderInformation.ChildCount = reader.ReadInt32();
                renderInformation.LastOffset = reader.BaseStream.Position;

                for (int i = 0; i < renderInformation.ChildCount; i++)
                {
                    if (reader.CanRead(8))
                    {
                        // null bytes
                        reader.ReadBytes(4);
                        var childId = reader.ReadUInt32();
                        renderInformation.ChildRenderIdList.Add((int)childId);
                        // TODO add render cache children for each one of these ids
                    }
                }
            }

            if (reader.CanRead(1))
            {
                renderInformation.HasTexture = reader.ReadByte() == 1;
                renderInformation.LastOffset = reader.BaseStream.Position;
            }

            if (renderInformation.HasTexture)
            {
                if (reader.CanRead(4))
                {
                    renderInformation.TextureCount = reader.ReadUInt32();
                }

                if (renderInformation.FirstInt != 257)
                {
                    if (reader.CanRead(8))
                    {
                        // seems to always be a 1 or 0
                        reader.ReadUInt32();
                        reader.ReadUInt32();
                    }
                }
                else
                {
                    if (reader.CanRead(4))
                    {
                        reader.ReadUInt32();
                    }
                }

                for (int i = 0; i < renderInformation.TextureCount; i++)
                {
                    var text = (int)reader.ReadInt32();
                    renderInformation.Textures.Add(text);
                    // renderInformation.ChildRenderIdList.Add(text);
                    if (reader.CanRead(34))
                    {
                        reader.BaseStream.Position += 34;
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

                if (renderInformation.HasTexture && renderInformation.Texture == null)
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
}