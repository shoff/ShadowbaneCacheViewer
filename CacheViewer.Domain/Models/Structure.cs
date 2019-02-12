// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace CacheViewer.Domain.Models
{
    using System;
    using Archive;
    using Data;
    using Exportable;
    using Extensions;
    using Factories;
    using NLog;

    public class Structure : ModelObject
    {
        private const int ValidRange = 5000;
        private readonly RenderInformationFactory renderInformationFactory = RenderInformationFactory.Instance;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //private readonly CollisionInfo collisionData = new CollisionInfo();
        //private readonly CollisionInfo collisionData1 = new CollisionInfo();
        //protected readonly List<CollisionInfo> collisionInfo = new List<CollisionInfo>();
        //private readonly List<RenderEntity> renderEntities = new List<RenderEntity>();
        //public readonly List<uint> renderIds = new List<uint>();

        public Structure(CacheIndex cacheIndex, ObjectType flag,
            string name, int offset, ArraySegment<byte> data, int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
            logger?.Debug($"Creating new Structure with id {cacheIndex.Identity}");
        }

        public int BytesOfZeroData { get; private set; }
        public bool BValue1 { get; private set; }
        public bool BValue2 { get; private set; }
        public bool BValue3 { get; private set; }
        public bool BWalkData { get; private set; }
        public uint InventoryTextureId { get; private set; }
        public uint IUnk { get; private set; }
        public uint MapTex { get; private set; }
        public uint NumberOfMeshes { get; private set; }
        public StructureValidationResult ValidationResult { get; private set; }
    
        private bool ValidRenderId(int id)
        {
            if (id == 0)
            {
                return false;
            }

            if (!RenderInformationFactory.Instance.IsValidRenderId(id))
            {
                return false;
            }

            var range = id > this.CacheIndex.Identity ?
                Math.Abs(id - this.CacheIndex.Identity) :
                Math.Abs(this.CacheIndex.Identity - id);

            return range <= ValidRange;
        }

        public override void Parse()
        {
            using (var reader = this.Data.CreateBinaryReaderUtf32())
            {
                var tnlc = reader.ReadInt32();
                var flag = (ObjectType)reader.ReadInt32();
                var nameLength = reader.ReadUInt32();
                var name = reader.ReadAsciiString(nameLength);

                if (string.IsNullOrWhiteSpace(this.Name))
                {
                    this.Name = name;
                }
                
                // should be 12 since the trailing byte may not be there if we are at the end of the file
                // once we have found a valid id then we will jump forward more than a byte at a time.

                while (reader.CanRead(12) && this.RenderIds.Count == 0)
                {
                    ValidationResult = reader.ValidateCobjectIdType4(this.CacheIndex.Identity);

                    if (ValidationResult.IsValid)
                    {
                        this.RenderIds.Add(ValidationResult.Id);
                        this.RenderCount = reader.StructureRenderCount(ValidationResult);
                    }
                    else
                    {
                        reader.BaseStream.Position = (ValidationResult.InitialOffset + 1);
                    }
                }

                while (reader.CanRead(12) && ValidationResult.IsValid &&
                    ValidationResult.NullTerminator == 0)
                {
                    ValidationResult = reader.ValidateCobjectIdType4(this.CacheIndex.Identity);
                    if (ValidationResult.IsValid)
                    {
                        this.RenderIds.Add(ValidationResult.Id);
                    }
                }
            }
        }

        public void ParseAndAssemble()
        {
            this.Parse();
            foreach (var render in this.RenderIds)
            {
                // TODO this doesn't handle duplicate ids
                var renderInformation = this.renderInformationFactory.Create(render, 0, true);
                this.Renders.Add(renderInformation);
            }
        }

        //public override void Parse()
        //{
        //    // I don't believe that CursorOffset + name is correct 
        //    //var ptr = this.CursorOffset; // this should be 37 + this.name.length * 2
        //    //Debug.Assert(ptr == 37 + (this.Name.Length * 2));
        //    var ptr = this.InnerOffset;
        //    this.IUnk = 0;
        //    //unknownData1 unkData1;
        //    var info = new CollisionInfo();
        //    try
        //    {
        //        using (var reader = this.Data.CreateBinaryReaderUtf32())
        //        {
        //            reader.BaseStream.Position = ptr;
        //            this.renderId = reader.ReadUInt32();

        //            while (!this.ValidRenderId((int) this.renderId) &&
        //                reader.BaseStream.Position + 1 <= reader.BaseStream.Length)
        //            {
        //                this.renderId = reader.ReadUInt32();
        //                if (this.renderId == 0)
        //                {
        //                    this.BytesOfZeroData++;
        //                    reader.BaseStream.Position -= 3; // go back 3 bytes and try to read again.
        //                }
        //                else if (RenderInformationFactory.Instance.IsValidRenderId((int) this.renderId))
        //                {
        //                    var range = (int) this.renderId > this.CacheIndex.Identity ?
        //                        Math.Abs((int) this.renderId - this.CacheIndex.Identity) :
        //                        Math.Abs(this.CacheIndex.Identity - (int) this.renderId);

        //                    if (range > ValidRange)
        //                    {
        //                        // almost certainly NOT a valid id
        //                        // invalid
        //                        this.renderId = 0;
        //                        reader.BaseStream.Position -= 3; // go back 3 bytes and try to read again.
        //                    }
        //                }
        //                else
        //                {
        //                    // invalid
        //                    this.renderId = 0;
        //                    reader.BaseStream.Position -= 3; // go back 3 bytes and try to read again.
        //                }
        //            }

        //            // TODO we should just pre-cache the render ids rather than reloading
        //            if (this.renderId == 0) // || !renderFactory.RenderArchive.Contains((int)this.renderId))
        //            {
        //                throw new ApplicationException($"No render Id found for {this.Name}");
        //            }

        //            // world texture id
        //            ptr += 4;
        //            var invTex = reader.ReadUInt32();
        //            // inventory texture id
        //            ptr += 4;
        //            this.MapTex = reader.ReadUInt32();
        //            // Get the minimap texture id
        //            ptr += 4;
        //            this.IUnk = reader.ReadUInt32();
        //            ptr += 4;
        //            this.IUnk = reader.ReadUInt32();
        //            ptr += 4;
        //            this.counter = reader.ReadUInt32();
        //            // Counter for number of records of unknown data
        //            ptr += 4;
        //            // range error check
        //            if (this.counter > 10000) // one million, nothing should have more than that
        //            {
        //                logger.Error(
        //                    "Counter value of {0} out of range when parsing structure of CacheIndex {1} in Structure line 84.",
        //                    this.counter, this.CacheIndex.Identity);
        //                return;
        //            }

        //            // Unknown data chunks
        //            // These chunks contain info like position, rotation, and scale - I think.
        //            /* Uncomment this to check out the data in the chunks
        //            for (unsigned int i=0; i<counter; i++) {
        //                memcpy(&unkData1, data + ptr, sizeof(unknownData1));
        //                ptr += sizeof(unknownData1);
        //            }
        //            */
        //            // ptr += sizeof(unknownData1) * counter;

        //            // skip over unknown data
        //            ptr += 108;
        //            reader.BaseStream.Position += 108;

        //            // 4 bytes that seem to contain bool info
        //            //memcpy(&bValue1, data+ptr, 1);
        //            this.BValue1 = reader.ReadBoolean();
        //            ptr++;

        //            //memcpy(&bValue2, data+ptr, 1);
        //            this.BValue2 = reader.ReadBoolean();
        //            ptr++;

        //            //memcpy(&bValue3, data+ptr, 1);
        //            this.BValue3 = reader.ReadBoolean();
        //            ptr++;

        //            //memcpy(&bWalkData, data+ptr, 1);
        //            this.BWalkData = reader.ReadBoolean();
        //            ptr++;

        //            // skip over more unknown data
        //            ptr += 7;
        //            reader.BaseStream.Position += 7;

        //            // range check and if statement - some type 5 objects don't have any of this data - must be a bool value somewhere !?
        //            // possible in the above 119 bytes
        //            if (this.BWalkData)
        //            {
        //                // Counter
        //                //memcpy(&counter, data + ptr, 4);
        //                this.counter = reader.ReadUInt32();
        //                ptr += 4;

        //                // range error check
        //                if (this.counter > 10000) // nothing should be more than that
        //                {
        //                    logger.Error(
        //                        "counter of {0} is out of range for walk data in Structure parsing for CacheIndex {1} in Structure line 133.",
        //                        this.counter, this.CacheIndex.Identity);
        //                    return;
        //                }

        //                // unknown data chunk(s)
        //                // These chunks contain information like collision detection, walkable areas, etc - and some other data im unsure of.
        //                for (uint i = 0; i < this.counter; i++)
        //                {
        //                    // Error handler
        //                    try
        //                    {
        //                        //memcpy(&collisionData.nVectors, data + ptr, 4); 
        //                        info.nVectors = reader.ReadUInt32();
        //                        ptr += 4;

        //                        //memcpy(&collisionData.bounds, data + ptr, (sizeof(Vec3D) * collisionData.nVectors));
        //                        //ptr += (sizeof(Vec3D) * collisionData.nVectors);
        //                        for (var x = 0; x < info.nVectors; x++)
        //                        {
        //                            info.bounds.Add(reader.ReadToVector3());
        //                        }

        //                        //memcpy(&collisionData.upVector, data + ptr, sizeof(Vec3D));
        //                        //ptr += sizeof(Vec3D);
        //                        info.upVector = reader.ReadToVector3();

        //                        //memcpy(&collisionData.order, data + ptr, sizeof(uint16) * 6);
        //                        //ptr += sizeof(uint16) * 6;
        //                        for (var y = 0; y < 6; y++)
        //                        {
        //                            info.order.Add(reader.ReadUInt16());
        //                        }

        //                        //memcpy(&collisionData.unknown, data + ptr, sizeof(Vec3D));
        //                        // ptr += sizeof(Vec3D);
        //                        info.unknown = reader.ReadToVector3();

        //                        // collisionInfo.push_back(collisionData);
        //                        this.collisionInfo.Add(info);
        //                    }
        //                    catch (Exception e1)
        //                    {
        //                        logger.Error(e1,
        //                            "Exception thrown while trying to create walk data for CacheIndex {0} in Structure line 172.",
        //                            this.CacheIndex.Identity);
        //                        throw;
        //                    }
        //                }

        //                // Counter for another data chunk similar to above
        //                //memcpy(&counter, data + ptr, 4);
        //                this.counter = reader.ReadUInt32();
        //                ptr += 4;

        //                // Error check
        //                if (this.counter < 1000)
        //                {
        //                    // anything not within this range is probably bad
        //                    var tempCounter = this.counter;

        //                    for (uint j = 0; j < tempCounter; j++)
        //                    {
        //                        try
        //                        {
        //                            // real counter to the number of chunks
        //                            //memcpy(&counter, data + ptr, 4); // Counter for the data chunk, identical to the above data chunk
        //                            this.counter = reader.ReadUInt32();
        //                            ptr += 4;

        //                            // range error check
        //                            if (this.counter > 10000) // nothing should be more than that
        //                            {
        //                                logger.Error(
        //                                    "counter of {0} is out of range for walk data in Structure parsing for CacheIndex {1} in Structure line 202.",
        //                                    this.counter, this.CacheIndex.Identity);
        //                                return;
        //                            }
        //                        }
        //                        catch (Exception e2)
        //                        {
        //                            logger.Error(e2,
        //                                "Exception occurred parsing structure for CacheIndex {0} in Structure line 209.",
        //                                this.CacheIndex.Identity);
        //                            return;
        //                        }

        //                        // unknown data chunk(s)
        //                        for (uint i = 0; i < this.counter; i++)
        //                        {
        //                            // Error handler
        //                            try
        //                            {
        //                                //memcpy(&collisionData.nVectors, data + ptr, 4); 
        //                                info.nVectors = reader.ReadUInt32();
        //                                ptr += 4;

        //                                //memcpy(&collisionData.bounds, data + ptr, (sizeof(Vec3D) * collisionData.nVectors));
        //                                //ptr += (sizeof(Vec3D) * collisionData.nVectors);
        //                                for (var x = 0; x < info.nVectors; x++)
        //                                {
        //                                    info.bounds.Add(reader.ReadToVector3());
        //                                }

        //                                //memcpy(&collisionData.upVector, data + ptr, sizeof(Vec3D));
        //                                //ptr += sizeof(Vec3D);
        //                                info.upVector = reader.ReadToVector3();

        //                                //memcpy(&collisionData.order, data + ptr, sizeof(uint16) * 6);
        //                                //ptr += sizeof(uint16) * 6;
        //                                for (var y = 0; y < 6; y++)
        //                                {
        //                                    info.order.Add(reader.ReadUInt16());
        //                                }

        //                                //memcpy(&collisionData.unknown, data + ptr, sizeof(Vec3D));
        //                                // ptr += sizeof(Vec3D);
        //                                info.unknown = reader.ReadToVector3();

        //                                // collisionInfo.push_back(collisionData);
        //                                this.collisionInfo.Add(info);
        //                            }
        //                            catch (Exception e3)
        //                            {
        //                                logger.Error(e3,
        //                                    "Exception thrown on second chunk of walk data for CacheIndex {0} in Structure line 247.",
        //                                    this.CacheIndex.Identity);
        //                                return;
        //                            }
        //                        }
        //                    }
        //                }

        //                // --

        //                // TODO:  This isn't null bytes,  its a counter of a new chunk of data
        //                //ptr += 4; // skip over null bytes at the end of data chunk
        //                // Counter for another data chunk
        //                // memcpy(&counter, data + ptr, 4);
        //                this.counter = reader.ReadUInt32();
        //                ptr += 4;

        //                for (uint i = 0; i < this.counter; i++)
        //                {
        //                    ptr += 456;
        //                }
        //            }

        //            // --

        //            // Number of meshes
        //            //memcpy(&counter, data + ptr, 4);
        //            this.counter = reader.ReadUInt32();
        //            this.NumberOfMeshes = this.counter;
        //            ptr += 4;

        //            // output some debug info
        //            //wxLogMessage(_T("Render Pass Count: %i"), counter);

        //            if (this.counter > 5000)
        //            {
        //                // Range check for invalid values
        //                logger.Error(
        //                    "Error: had render pass count of {0} on CacheIndex {1}  Exiting function  in Structure line 281.",
        //                    this.counter, this.CacheIndex.Identity);

        //                //wxLogMessage(_T("Error: had render pass count of %i.  Exiting function."), counter);
        //                return;
        //            }

        //            for (uint i = 0; i < this.counter; i++)
        //            {
        //                // Error handler
        //                try
        //                {
        //                    ptr += 9; // 9 null bytes ?
        //                    reader.BaseStream.Position += 9;

        //                    // memcpy(&renderID, data + ptr, 4);
        //                    this.renderId = reader.ReadUInt32();

        //                    ptr += 4;

        //                    //wxLogMessage(_T("Render ID: %i"), renderID);
        //                    // no point adding it to the stack, unless we actually have an id - basic error check
        //                    if (this.renderId > 0)
        //                    {
        //                        this.renderIds.Add(this.renderId);
        //                    }
        //                }
        //                catch (Exception e5)
        //                {
        //                    logger.Error(e5, "Error parsing structure for CacheIndex {0} in Structure line 309.",
        //                        this.CacheIndex.Identity);
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e6)
        //    {
        //        logger.Error(e6, "Error parsing structure for CacheIndex {0} in Structure line 317.",
        //            this.CacheIndex.Identity);

        //        //wxLogMessage(_T("Error: Failed to load Object Type 4"));
        //    }
        //}
    }
}