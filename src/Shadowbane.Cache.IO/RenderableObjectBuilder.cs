namespace Shadowbane.Cache.IO
{
    using System;
    using Exporter.File;
    using Models;

    public static class RenderableObjectBuilder
    {
        private static string storeFolder = $"{AppDomain.CurrentDomain.BaseDirectory}/render-store";

        public static RenderInformation Create(CacheIndex cacheIndex)
        {
            var asset = ArchiveLoader.RenderArchive[cacheIndex.identity];

            var renderInformation = new RenderInformation
            {
                CacheIndex = cacheIndex,
                ByteCount = asset.Asset.Length,
                Unknown = new object[6]
            };

            // for now let's save them all in a folder for analysis
            FileWriter.Writer.Write(asset.Asset.Span, storeFolder, $"{asset.CacheIndex.identity}-{asset.Order}");
            if (asset.HasMultipleIdentityEntries)
            {
                var secondAsset = ArchiveLoader.RenderArchive.GetSecondIndex(cacheIndex.identity);
                FileWriter.Writer.Write(secondAsset.Asset.Span, storeFolder, $"{secondAsset.CacheIndex.identity}-{secondAsset.Order}");
            }

            using var reader = asset.Asset.CreateBinaryReaderUtf32(35);
            renderInformation.HasMesh = reader.ReadUInt32() == 1;
            renderInformation.Unknown[0] = reader.ReadUInt32();
            renderInformation.MeshId = reader.ReadInt32();
            renderInformation.Unknown[1] = reader.ReadUInt16();
            renderInformation.LastOffset = reader.BaseStream.Position;
            renderInformation.JointNameSize = reader.ReadUInt32();
            if (renderInformation.JointNameSize > 30)
            {
                // no joint has a name that long
                renderInformation.Notes +=
                    $" We read a joint name size of {renderInformation.JointNameSize} this can't be correct.";

#pragma warning disable S112 // General exceptions should never be thrown
                throw new ParseException($"Parsing {renderInformation.CacheIndex.identity} a JointMeshSize of " +
                    $"{renderInformation.JointNameSize} was read. This is obviously incorrect.");
#pragma warning restore S112 // General exceptions should never be thrown
            }

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

            // Texture count
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
            return renderInformation;
        }
    }
}