namespace CacheViewer.Domain.Factories.Providers
{
    using System;
    using System.IO;
    using Extensions;
    using Models;

    public static class RenderProviders
    {

        internal static int[] type2RenderInfos =
        {
            110109, 122100, 20196, 24035, 24038, 24046, 26520, 42026, 50223, 64115, 64140,
            64199, 110135, 142120, 144048, 144094, 144129, 144550, 200130, 242100, 402012,
            406929, 406931, 425107, 425291, 425479, 425752, 425904, 426150, 426293, 451050,
            451100, 451150, 451200, 451250, 451300, 460304, 460476, 460476, 460476, 460517,
            461500, 461600, 461700, 462300, 462350, 482000, 482003, 482006, 482010, 482100,
            482500, 482600, 482700, 482800, 482900, 524031, 524035, 524035, 524035, 542019,
            542025, 542028, 542031, 542034, 562024, 562061, 562095, 562137, 562141, 562154,
            562165, 562176, 562181, 564683, 565102, 565105, 565105, 565105, 565106, 565106,
            565106, 584100, 584280, 584512, 584654, 584882, 590064, 590476, 590897, 591271,
            591683, 592118, 592472, 592878, 593274, 593672, 594091, 594526, 594861, 595290,
            595719, 596059, 596475, 596905, 600075, 600895, 601291, 601731, 602137, 602550,
            602909, 605029, 605429, 605829, 606241, 606641, 607041, 607476, 607876, 608304,
            608704, 609103, 609557, 622245, 622368, 622666, 622743, 622795, 623160, 701586,
            702875, 702950, 703225, 704630, 800003, 804089, 904000, 904005, 904010, 904015,
            1200000, 1200010, 1300040, 1300251, 1300464, 1301046, 1301255, 1301451,1302039,
            1302243, 1302454, 1303044, 1303243, 1303447, 1304044, 1304248, 1304450, 1305044,
            1305238, 1305430, 1320070, 1320275, 1320476, 1321057, 1321266, 1321308, 1321509,
            1322075, 1322291, 1322313, 1322489, 1323076, 1323283, 1323351, 1323523, 1324084,
            1324305, 1324312, 1324312, 1324312, 1324508, 1325066, 1325290, 1325489, 1326092,
            1326739, 1327283, 1327304, 1327504, 1327704, 1612200, 5001100, 5030130, 5030270,
            5030350, 5030360, 5030390, 5030410, 5030440, 5030450, 5030630, 5060111, 5060537,
            5060911, 7000260, 77000100, 50223, 62006, 62019, 142002, 142008, 142011, 142018,
            142024, 162000, 242001, 402000
        };

        internal static int[] type3RenderInfos =
        {
            510, 520, 535, 541, 542, 543
        };

        public static void Parse541(this BinaryReader reader, RenderInformation renderInfo)
        {
            reader.BaseStream.Position = 0;
            //int renderType;
            int renderType = reader.ReadInt32();
            //ushort renderType1;
            ushort renderType1 = reader.ReadUInt16();
            //time_t unk;
            var unk = reader.ReadToDate();
            //uint renderType2;
            uint renderType2 = reader.ReadUInt32();
            //uint null1;
            uint null1 = reader.ReadUInt32();

            //uint some_counter_or_bool;
            uint some_counter_or_bool = reader.ReadUInt32();

            //uint null2; // padding
            uint null2 = reader.ReadUInt32();

            //uint null3; // padding
            uint null3 = reader.ReadUInt32();

            //uint null4; // padding
            uint null4 = reader.ReadUInt32();

            //byte somebool;
            byte somebool = reader.ReadByte();

            //uint possibly_has_mesh_bool;
            uint possibly_has_mesh_bool = reader.ReadUInt32();

            //uint null5; // padding
            uint null5 = reader.ReadUInt32();

            //uint meshid;
            uint meshid = reader.ReadUInt32();

            ushort null21 = reader.ReadUInt16();

            //ushort null1<hidden= true >;      // null short
            //int jointNameSize;              // can be 0
            //wchar_t name[jointNameSize] < name = "Joint Name" >;

            //reader.BaseStream.Position = 0;
            //reader.ReadInt32(); // 4
            //reader.ReadInt16(); // 6 
            //reader.ReadInt32(); // 10

            //reader.ReadInt32();
            //reader.ReadInt32();
            //reader.ReadInt32();
            //reader.ReadInt32();
            //reader.ReadInt32();
            //reader.ReadInt32(); // 34

            //reader.ReadByte(); // 35

            //renderInfo.HasMesh = reader.ReadUInt32() == 1; // 39

            //// null
            //reader.ReadUInt32(); // 43
            //renderInfo.MeshId = reader.ReadInt32(); // 47

            //renderInfo.JointNameSize = reader.ReadUInt32(); // should be 0 // 51

            //// Debug.Assert(renderInfo.JointNameSize == 0);
            //if (renderInfo.JointNameSize > 0)
            //{
            //    throw new ApplicationException($"{renderInfo.CacheIndex.Identity} should not have a joint name!");
            //}

            //reader.ReadBytes(2);
            reader.BaseStream.Position = 53;

            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            var z = reader.ReadSingle();
            renderInfo.Scale = new Geometry.Vector3(x, y, z);
            // renderInfo.Scale = reader.ReadToVector3();
            uint[] crap =
            {
                reader.ReadUInt32(),
                reader.ReadUInt32(),
                reader.ReadUInt32(),
                reader.ReadUInt32(),
                reader.ReadUInt32(),
                reader.ReadUInt32()
            };
            reader.ReadByte();
            uint someCounter = reader.ReadUInt32();
            for (int i = 0; i < someCounter; i++)
            {
                reader.ReadUInt32();
            }

            reader.ReadUInt32();
            renderInfo.ModifiedDate = reader.ReadToDate();
            // more crap
            reader.ReadUInt32();
            reader.ReadUInt32();

            renderInfo.TextureCount = reader.ReadUInt32();
            // more crap
            reader.ReadUInt32();
            reader.ReadUInt32();

            for (int i = 0; i < renderInfo.TextureCount; i++)
            {
                renderInfo.Textures.Add(reader.ReadInt32());
                if (reader.CanRead(34))
                {
                    reader.ReadBytes(34);
                }
            }
        }
        
        public static void ParseTypeThree(this BinaryReader reader, RenderInformation renderInfo)
        {
            reader.BaseStream.Position = 0;
            renderInfo.FirstInt = reader.ReadUInt32();
            renderInfo.FirstUshort = reader.ReadUInt16();
            renderInfo.CreateDate = reader.ReadToDate();
            renderInfo.UnknownIntOne = reader.ReadUInt32();
            renderInfo.UnknownIntTwo = reader.ReadUInt32();
            renderInfo.UnknownCounterOrBool = reader.ReadUInt32();

            // three nulls
            reader.ReadInt32();
            reader.ReadInt32();
            reader.ReadInt32();

            renderInfo.B34 = reader.ReadByte();
            renderInfo.HasMesh = reader.ReadUInt32() == 1;
            
            // null
            reader.ReadUInt32();
            renderInfo.MeshId = reader.ReadInt32();

            reader.ReadUInt16(); // null short

            renderInfo.JointNameSize = reader.ReadUInt32(); // should be 0

            // Debug.Assert(renderInfo.JointNameSize == 0);
            if (renderInfo.JointNameSize > 0)
            {
                throw new ApplicationException($"{renderInfo.CacheIndex.Identity} should not have a joint name!");
            }
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            var z = reader.ReadSingle();
            renderInfo.Scale = new Geometry.Vector3(x, y, z);
            // renderInfo.Scale = reader.ReadToVector3();
            uint[] crap =
            {
                reader.ReadUInt32(),
                reader.ReadUInt32(),
                reader.ReadUInt32(),
                reader.ReadUInt32(),
                reader.ReadUInt32(),
                reader.ReadUInt32()
            };
            reader.ReadByte();
            uint someCounter = reader.ReadUInt32();
            for (int i = 0; i < someCounter; i++)
            {
                reader.ReadUInt32();
            }

            reader.ReadUInt32();
            renderInfo.ModifiedDate = reader.ReadToDate();
            // more crap
            reader.ReadUInt32();
            reader.ReadUInt32();

            renderInfo.TextureCount = reader.ReadUInt32();
            // more crap
            reader.ReadUInt32();
            reader.ReadUInt32();

            for (int i = 0; i < renderInfo.TextureCount; i++)
            {
                renderInfo.Textures.Add(reader.ReadInt32());
                if (reader.CanRead(34))
                {
                    reader.ReadBytes(34);
                }
            }
        }

        public static void ParseTypeTwo(this BinaryReader reader, RenderInformation renderInformation)
        {
            reader.BaseStream.Position = 0;

            renderInformation.FirstInt = reader.ReadUInt32();
            renderInformation.FirstUshort = reader.ReadUInt16();
            renderInformation.CreateDate = reader.ReadToDate();

            renderInformation.UnknownIntOne = reader.ReadUInt32();
            renderInformation.UnknownIntTwo = reader.ReadUInt32();

            renderInformation.UnknownCounterOrBool = reader.ReadUInt32();

            reader.ReadUInt32(); // null
            reader.ReadUInt32(); // null
            reader.ReadUInt32(); // null

            renderInformation.B34 = reader.ReadByte();
            renderInformation.HasMesh = reader.ReadUInt32() == 1; // in type 2s this should always be false?
            
            renderInformation.Position = reader.ReadToVector3();
            renderInformation.HasTexture = reader.ReadUInt32() == 1;

            reader.ReadUInt32(); // null
            reader.ReadUInt32(); // null
            reader.ReadUInt32(); // null

            renderInformation.TextureCount = reader.ReadUInt32();

            // read null int
            reader.ReadUInt32(); // null
            
            for (int i = 0; i < renderInformation.TextureCount; i++)
            {
                renderInformation.Textures.Add(reader.ReadInt32());
                if (reader.CanRead(4))
                {
                    reader.ReadUInt32();
                }
            }
        }

        public static void ParseTypeOne(this BinaryReader reader, RenderInformation renderInfo)
        {
            // 1/25/2019 - this is NOT a bool 
            // for instance render id 1856 has the following first four bytes 01 01 00 00 (257 uint)
            // see if this has a joint
            renderInfo.FirstInt = reader.ReadUInt32();

            // previously I thought this was always a null short, but it is not always null actually.
            reader.ReadUInt16();

            renderInfo.CreateDate = reader.ReadToDate();

            renderInfo.B11 = reader.ReadByte();
            reader.BaseStream.Position = 34;
            renderInfo.B34 = reader.ReadByte();
            renderInfo.HasMesh = reader.ReadUInt32() == 1;

            renderInfo.Unknown[0] = reader.ReadUInt32();
            renderInfo.MeshId = reader.ReadInt32();
            renderInfo.Unknown[1] = reader.ReadUInt16();
            renderInfo.LastOffset = reader.BaseStream.Position;
            renderInfo.JointNameSize = reader.ReadUInt32();

            if (renderInfo.JointNameSize > 30)
            {
                // no joint has a name that long
                renderInfo.Notes +=
                    $" We read a joint name size of {renderInfo.JointNameSize} this can't be correct.";

                throw new ApplicationException($"Parsing {renderInfo.CacheIndex.Identity} a JointMeshSize of " +
                    $"{renderInfo.JointNameSize} was read. This is obviously incorrect.");
            }

            if (reader.BaseStream.Position + renderInfo.JointNameSize <= renderInfo.ByteCount)
            {
                renderInfo.JointName = reader.ReadAsciiString(renderInfo.JointNameSize);
                renderInfo.LastOffset = reader.BaseStream.Position;
            }

            if (reader.BaseStream.Position + 12 <= renderInfo.ByteCount)
            {
                // object scale ?
                renderInfo.Scale = reader.ReadToVector3();
                renderInfo.LastOffset = reader.BaseStream.Position;
            }

            if (reader.BaseStream.Position + 4 <= renderInfo.ByteCount)
            {
                // I think this is probably a bool or flag of some kind
                renderInfo.Unknown[2] = reader.ReadUInt32();
                renderInfo.LastOffset = reader.BaseStream.Position;
            }

            if (reader.BaseStream.Position + 12 <= renderInfo.ByteCount)
            {
                // object position ?
                renderInfo.Position = reader.ReadToVector3();
                renderInfo.LastOffset = reader.BaseStream.Position;
            }

            if (reader.BaseStream.Position + 4 <= renderInfo.ByteCount)
            {
                renderInfo.ChildCount = reader.ReadInt32();
                renderInfo.LastOffset = reader.BaseStream.Position;
            }

            if (reader.BaseStream.Position + 1 <= renderInfo.ByteCount)
            {
                var ht = reader.ReadByte();
                renderInfo.HasTexture = ht == 1;
                renderInfo.LastOffset = reader.BaseStream.Position;
            }

            if (renderInfo.HasTexture)
            {
                if (reader.CanRead(4))
                {
                    renderInfo.TextureCount = reader.ReadUInt32();
                }

                if (reader.CanRead(8))
                {
                    // seems to always be a 1 or 0
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                }

                for (int i = 0; i < renderInfo.TextureCount; i++)
                {
                    //renderInfo.Textures[i] = (int) reader.ReadUInt32();
                    var text = (int)reader.ReadInt32();
                    renderInfo.Textures.Add(text);
                    if (reader.CanRead(34))
                    {
                        reader.BaseStream.Position += 34;
                    }
                }
            }
        }
    }
}