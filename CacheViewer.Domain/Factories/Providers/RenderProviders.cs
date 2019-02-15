namespace CacheViewer.Domain.Factories.Providers
{
    using System;
    using System.IO;
    using Extensions;
    using Models;

    public static class RenderProviders
    {
        // THREEWAVE FIXEDY ?
        internal static int[] type5ContainingText = 
        {
            422983, 423023, 423024, 423042, 423043, 423080, 423100, 423140, 423160, 423162,
            423182, 423200, 
        };


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

        internal static int[] type4RenderInfos =
        {
            20, 500, 501, 1800,1801,1802, 1803, 1804, 1805,  1806, 1807, 1808, 1809, 1810, 1811,
            1812, 1813, 1814, 1815, 1816, 1817, 1818, 1819, 1820, 1821, 1822, 1823, 1824, 1825,
            1826, 1827, 1828, 1829, 1830, 1831, 1832, 1833, 1834, 1835, 1836, 1837, 1838, 1839,
            1840, 1841, 1842, 1843, 1844, 1845, 1846, 1847, 1848, 1849, 1850, 1851, 1852, 1853,
            1854, 1855, 1856, 1857, 1858, 1859, 1860, 1861, 1862, 2000, 2001, 2002, 2003, 2004,
            2005, 2006, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2500, 2501,
            2502, 2503, 2504, 2510, 2511, 2512, 2513, 2514, 2515, 2516, 2517, 2518, 2519, 2520,
            2521, 2522, 2523, 2524, 2525, 2526, 2527, 2528, 2529, 2530, 2531, 3000, 3001, 3002,
            3003, 3004, 3005, 3006, 3007, 3008, 3009, 3010, 3011, 3012, 3013, 3014, 3015, 3016,
            3017, 3018, 3019, 3020, 3021, 3022, 3023, 3024, 3025, 3026, 3027, 3028, 3029, 3030,
            3031, 3032, 3033, 3034, 3035, 3036, 3037, 3038, 3039, 3040, 3041, 3042, 3043, 3044,
            3045, 3046, 3047, 4000, 4001, 4002, 4003, 4004, 4005, 4006, 4007, 4008, 4009, 4010,
            4011, 4012, 4013, 4014, 4015, 4016, 4017, 4018, 4019, 4020, 4021, 4022, 4023, 4024,
            4025, 12000, 12001, 12002, 12003, 12004, 12005, 12006, 12007, 12008, 12009, 12010,
            12011, 12012, 12013, 12014, 12015, 12016, 12017, 12018, 12019, 12020, 12021, 12022,
            12023, 12024, 12025, 12026, 12027, 12028, 12029, 12030, 12031, 12032, 12033, 12034,
            12035, 12036, 12037, 12038, 12039, 12040, 12041, 12042, 12043, 12044, 12045, 12046,
            12047, 12048, 12049, 12050, 12051, 12052, 12053, 12054, 12055, 12056, 12057, 12058,
            12059, 12060, 12061, 12062, 12063, 12064, 12065, 12066, 12067, 12068, 12069, 12070,
            12071, 12072, 12073, 12074, 12075, 12076, 12077, 12078, 12079, 12080, 12081, 12082,
            12083, 12084, 12085, 12086, 12087, 12088, 12089, 12090,
            12091,12092
        };

        internal static int[] type3RenderInfos =
        {
          510, 520, 535, 541, 542, 543, 544, 20, 541
        };

        // THIS APPEARS TO WORK FOR RENDER TYPES 257 256
        public static void ParseTypeFour(this BinaryReader reader, RenderInformation renderInfo)
        {
            reader.BaseStream.Position = 0;

            renderInfo.FirstInt = reader.ReadUInt32();
            renderInfo.FirstUshort = reader.ReadUInt16();
            renderInfo.CreateDate = reader.ReadToDate();

            renderInfo.UnknownIntOne = reader.ReadUInt32();
            renderInfo.UnknownIntTwo = reader.ReadUInt32();
            renderInfo.UnknownCounterOrBool = reader.ReadUInt32();

            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadByte();

            renderInfo.HasMesh = reader.ReadUInt32() == 1;
            uint null5 = reader.ReadUInt32();
            renderInfo.MeshId = reader.ReadInt32();
            renderInfo.ValidMeshFound = renderInfo.MeshId != 0 && MeshFactory.Instance.HasMeshId(renderInfo.MeshId);
            ushort null1 = reader.ReadUInt16();
            renderInfo.JointNameSize = reader.ReadUInt32();
            if (renderInfo.JointNameSize > 0)
            {
                renderInfo.JointName = reader.ReadAsciiString(renderInfo.JointNameSize);
            }

            renderInfo.Scale = reader.ReadToVector3();

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
                var id = reader.ReadInt32();
                renderInfo.ChildRenderIdList.Add(id);
            }

            renderInfo.IUNK = reader.ReadUInt32();
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
                var id = reader.ReadInt32();
                renderInfo.Textures.Add(id);
                
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
            renderInfo.ChildCount = (int)renderInfo.TextureCount;

            // more crap
            reader.ReadUInt32();
            reader.ReadUInt32();

            for (int i = 0; i < renderInfo.TextureCount; i++)
            {
                var id = reader.ReadInt32();
                renderInfo.ChildRenderIdList.Add(id);
                renderInfo.Textures.Add(id);
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
                var id = reader.ReadInt32();
                renderInformation.Textures.Add(id);
                if (reader.CanRead(4))
                {
                    reader.ReadUInt32();
                }
            }
        }

        public static void ParseTypeOne(this BinaryReader reader, RenderInformation renderInfo)
        {
            reader.BaseStream.Position = 0;
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
            // end of name

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

            if (reader.CanRead(4))
            {
                // I think this is probably a bool or flag of some kind
                renderInfo.Unknown[2] = reader.ReadUInt32();
                renderInfo.LastOffset = reader.BaseStream.Position;
            }

            if (reader.CanRead(12))
            {
                // object position ?
                renderInfo.Position = reader.ReadToVector3();
                renderInfo.LastOffset = reader.BaseStream.Position;
            }

            if (reader.CanRead(4))
            {
                renderInfo.ChildCount = reader.ReadInt32();
                renderInfo.LastOffset = reader.BaseStream.Position;

                for (int i = 0; i < renderInfo.ChildCount; i++)
                {
                    if (reader.CanRead(8))
                    {
                        // null bytes
                        reader.ReadBytes(4);
                        var childId = reader.ReadUInt32();
                        renderInfo.ChildRenderIdList.Add((int)childId);
                        // TODO add render cache children for each one of these ids
                    }
                }
            }

            // Texture count
            if (reader.CanRead(1))
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

                if (renderInfo.FirstInt != 257)
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

                for (int i = 0; i < renderInfo.TextureCount; i++)
                {
                    var text = (int)reader.ReadInt32();
                    renderInfo.Textures.Add(text);
                    // renderInfo.ChildRenderIdList.Add(text);
                    if (reader.CanRead(34))
                    {
                        reader.BaseStream.Position += 34;
                    }
                }
            }
        }
    }
}