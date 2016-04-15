namespace CacheViewer.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Archive;
    using Extensions;
    using Exportable;
    using NLog;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    [SuppressMessage("ReSharper", "UnusedVariable")]
    public class Structure : ModelObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly CollisionInfo collisionData = new CollisionInfo();
        private readonly CollisionInfo collisionData1 = new CollisionInfo();
        protected readonly List<CollisionInfo> collisionInfo = new List<CollisionInfo>();
        public readonly List<uint> renderIds = new List<uint>();
        private bool bValue1;
        private bool bValue2;
        private bool bValue3;
        private bool bWalkData;

        private uint inventoryTextureId;
        private uint mapTex;
        private uint numberOfMeshes;
        private uint iUnk;
        private uint counter;
        private uint renderId;


        /// <summary>
        /// Initializes a new instance of the <see cref="Structure"/> class.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <param name="flag">The flag.</param>
        /// <param name="name">The name.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="data">The data.</param>
        /// <param name="innerOffset">The inner offset.</param>
        public Structure(CacheIndex cacheIndex, ObjectType flag, string name, int offset, ArraySegment<byte> data, int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

        /// <summary>Parses the specified buffer.</summary>
        /// <param name="data">The buffer.</param>
        public override void Parse(ArraySegment<byte> data)
        {
            int ptr = this.CursorOffset;
            iUnk = 0;
            //unknownData1 unkData1;
            CollisionInfo info = new CollisionInfo();
            try
            {
                BinaryReader reader = data.CreateBinaryReaderUtf32();
                reader.BaseStream.Position = ptr;
                renderId = reader.ReadUInt32();
                // world texture id
                ptr += 4;
                var invTex = reader.ReadUInt32();
                // inventory texture id
                ptr += 4;
                mapTex = reader.ReadUInt32();
                // Get the minimap texture id
                ptr += 4;
                iUnk = reader.ReadUInt32();
                ptr += 4;
                iUnk = reader.ReadUInt32();
                ptr += 4;
                counter = reader.ReadUInt32();
                // Counter for number of records of unknown data
                ptr += 4;
                // range error check
                if (counter > 10000) // one million, nothing should have more than that
                {
                    logger.Error("Counter value of {0} out of range when parsing structure of CacheIndex {1} in Structure line 84.",
                        counter, this.CacheIndex.Identity);
                    return;
                }

                // Unknown data chunks
                // These chunks contain info like position, rotation, and scale - I think.
                /* Uncomment this to check out the data in the chunks
                for (unsigned int i=0; i<counter; i++) {
                    memcpy(&unkData1, data + ptr, sizeof(unknownData1));
                    ptr += sizeof(unknownData1);
                }
                */
                // ptr += sizeof(unknownData1) * counter;

                // skip over unknown data
                ptr += 108;
                reader.BaseStream.Position += 108;

                // 4 bytes that seem to contain bool info
                //memcpy(&bValue1, data+ptr, 1);
                bValue1 = reader.ReadBoolean();
                ptr++;

                //memcpy(&bValue2, data+ptr, 1);
                bValue2 = reader.ReadBoolean();
                ptr++;

                //memcpy(&bValue3, data+ptr, 1);
                bValue3 = reader.ReadBoolean();
                ptr++;

                //memcpy(&bWalkData, data+ptr, 1);
                bWalkData = reader.ReadBoolean();
                ptr++;

                // skip over more unknown data
                ptr += 7;

                // range check and if statement - some type 5 objects don't have any of this data - must be a bool value somewhere !?
                // possible in the above 119 bytes
                if (bWalkData)
                {
                    // Counter
                    //memcpy(&counter, data + ptr, 4);
                    counter = reader.ReadUInt32();
                    ptr += 4;

                    // range error check
                    if (counter > 10000) // nothing should be more than that
                    {
                        logger.Error(
                            "counter of {0} is out of range for walk data in Structure parsing for CacheIndex {1} in Structure line 133.",
                            counter, this.CacheIndex.Identity);
                        return;
                    }

                    // unknown data chunk(s)
                    // These chunks contain information like collision detection, walkable areas, etc - and some other data im unsure of.
                    for (uint i = 0; i < counter; i++)
                    {
                        // Error handler
                        try
                        {
                            //memcpy(&collisionData.nVectors, data + ptr, 4); 
                            info.nVectors = reader.ReadUInt32();
                            ptr += 4;

                            //memcpy(&collisionData.bounds, data + ptr, (sizeof(Vec3D) * collisionData.nVectors));
                            //ptr += (sizeof(Vec3D) * collisionData.nVectors);
                            for (int x = 0; x < info.nVectors; x++)
                            {
                                info.bounds.Add(reader.ReadToVector3());
                            }

                            //memcpy(&collisionData.upVector, data + ptr, sizeof(Vec3D));
                            //ptr += sizeof(Vec3D);
                            info.upVector = reader.ReadToVector3();

                            //memcpy(&collisionData.order, data + ptr, sizeof(uint16) * 6);
                            //ptr += sizeof(uint16) * 6;
                            for (int y = 0; y < 6; y++)
                            {
                                info.order.Add(reader.ReadUInt16());
                            }

                            //memcpy(&collisionData.unknown, data + ptr, sizeof(Vec3D));
                            // ptr += sizeof(Vec3D);
                            info.unknown = reader.ReadToVector3();

                            // collisionInfo.push_back(collisionData);
                            collisionInfo.Add(info);
                        }
                        catch (Exception e1)
                        {
                            logger.Error(e1, "Exception thrown while trying to create walk data for CacheIndex {0} in Structure line 172.",this.CacheIndex.Identity);
                            throw;
                        }
                    }

                    // Counter for another data chunk similar to above
                    //memcpy(&counter, data + ptr, 4);
                    counter = reader.ReadUInt32();
                    ptr += 4;

                    // Error check
                    if (counter < 1000)
                    {
                        // anything not within this range is probably bad
                        uint tempCounter = counter;

                        for (uint j = 0; j < tempCounter; j++)
                        {
                            try
                            {
                                // real counter to the number of chunks
                                //memcpy(&counter, data + ptr, 4); // Counter for the data chunk, identical to the above data chunk
                                counter = reader.ReadUInt32();
                                ptr += 4;

                                // range error check
                                if (counter > 10000) // nothing should be more than that
                                {
                                    logger.Error(
                                        "counter of {0} is out of range for walk data in Structure parsing for CacheIndex {1} in Structure line 202.",
                                        counter, this.CacheIndex.Identity);
                                    return;
                                }
                            }
                            catch (Exception e2)
                            {
                                logger.Error(e2, "Exception occurred parsing structure for CacheIndex {0} in Structure line 209.",this.CacheIndex.Identity);
                                return;
                            }

                            // unknown data chunk(s)
                            for (uint i = 0; i < counter; i++)
                            {
                                // Error handler
                                try
                                {
                                    //memcpy(&collisionData.nVectors, data + ptr, 4); 
                                    info.nVectors = reader.ReadUInt32();
                                    ptr += 4;

                                    //memcpy(&collisionData.bounds, data + ptr, (sizeof(Vec3D) * collisionData.nVectors));
                                    //ptr += (sizeof(Vec3D) * collisionData.nVectors);
                                    for (int x = 0; x < info.nVectors; x++)
                                    {
                                        info.bounds.Add(reader.ReadToVector3());
                                    }

                                    //memcpy(&collisionData.upVector, data + ptr, sizeof(Vec3D));
                                    //ptr += sizeof(Vec3D);
                                    info.upVector = reader.ReadToVector3();

                                    //memcpy(&collisionData.order, data + ptr, sizeof(uint16) * 6);
                                    //ptr += sizeof(uint16) * 6;
                                    for (int y = 0; y < 6; y++)
                                    {
                                        info.order.Add(reader.ReadUInt16());
                                    }

                                    //memcpy(&collisionData.unknown, data + ptr, sizeof(Vec3D));
                                    // ptr += sizeof(Vec3D);
                                    info.unknown = reader.ReadToVector3();

                                    // collisionInfo.push_back(collisionData);
                                    collisionInfo.Add(info);
                                }
                                catch (Exception e3)
                                {
                                    logger.Error(e3,
                                        "Exception thrown on second chunk of walk data for CacheIndex {0} in Structure line 247.",this.CacheIndex.Identity);
                                    return;
                                }
                            }
                        }
                    }

                    // --

                    // TODO:  This isn't null bytes,  its a counter of a new chunk of data
                    //ptr += 4; // skip over null bytes at the end of data chunk
                    // Counter for another data chunk
                    // memcpy(&counter, data + ptr, 4);
                    counter = reader.ReadUInt32();
                    ptr += 4;

                    for (uint i = 0; i < counter; i++)
                    {
                        ptr += 456;
                    }
                }

                // --

                // Number of meshes
                //memcpy(&counter, data + ptr, 4);
                counter = reader.ReadUInt32();
                ptr += 4;

                // output some debug info
                //wxLogMessage(_T("Render Pass Count: %i"), counter);

                if (counter > 5000)
                {
                    // Range check for invalid values
                    logger.Error("Error: had render pass count of {0} on CacheIndex {1}  Exiting function  in Structure line 281.",
                        counter, this.CacheIndex.Identity);

                    //wxLogMessage(_T("Error: had render pass count of %i.  Exiting function."), counter);
                    return;
                }

                for (uint i = 0; i < counter; i++)
                {
                    // Error handler
                    try
                    {
                        ptr += 9; // 9 null bytes ?
                        reader.BaseStream.Position += 9;

                        // memcpy(&renderID, data + ptr, 4);
                        renderId = reader.ReadUInt32();

                        ptr += 4;

                        //wxLogMessage(_T("Render ID: %i"), renderID);

                        // no point adding it to the stack, unless we actually have an id - basic error check
                        if (renderId > 0)
                        {
                            renderIds.Add(renderId);
                        }
                    }
                    catch (Exception e5)
                    {
                        logger.Error(e5, "Error parsing structure for CacheIndex {0} in Structure line 309.",this.CacheIndex.Identity);
                        break;
                    }
                }
            }
            catch (Exception e6)
            {
                logger.Error(e6, "Error parsing structure for CacheIndex {0} in Structure line 317.",this.CacheIndex.Identity);

                //wxLogMessage(_T("Error: Failed to load Object Type 4"));
            }
        }

        /// <summary>
        /// Parses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <exception cref="System.ApplicationException">
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">data;the number of meshes exceeded 5000.</exception>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        public void Parsssse(ArraySegment<byte> data)
        {
            using (BinaryReader reader = data.CreateBinaryReaderUtf32())
            {
                reader.BaseStream.Position = this.CursorOffset;
                this.RenderId = reader.ReadUInt32();
                this.inventoryTextureId = reader.ReadUInt32();
                this.mapTex = reader.ReadUInt32();

                // unknown data
                reader.ReadUInt32();
                reader.ReadUInt32();

                // Counter for number of records of unknown data
                uint unknownDataCounter = reader.ReadUInt32();

                // range error check
                if (unknownDataCounter > 10000) // nothing should have more than that
                {
                    throw new ApplicationException();
                }

                // Unknown data chunks
                // These chunks contain info like position, rotation, and scale - I think.
                /* Uncomment this to check out the data in the chunks
                for (uint i=0; i<counter; i++) {
                    memcpy(&unkData1, data + ptr, sizeof(unknownData1));
                    ptr += sizeof(unknownData1);
                }
                */
                reader.BaseStream.Position += (48 * unknownDataCounter);
                reader.BaseStream.Position += 108;
                this.bValue1 = reader.ReadBoolean();
                this.bValue2 = reader.ReadBoolean();
                this.bValue3 = reader.ReadBoolean();
                this.bWalkData = reader.ReadBoolean();

                // skip over more unknown data
                // ptr += 7;
                reader.BaseStream.Position += 7;

                // range check and if statement - some type 5 objects don't have any of 
                // this data - must be a bool value somewhere !?
                // possible in the above 119 bytes
                if (this.bWalkData)
                {
                    unknownDataCounter = reader.ReadUInt32();

                    // range error check
                    if (unknownDataCounter > 10000) // nothing should have more than that
                    {
                        throw new ApplicationException();
                    }

                    // unknown data chunk(s)
                    // These chunks contain information like colision detection, 
                    // walkable areas, etc - and some other data im unsure of.
                    for (uint i = 0; i < unknownDataCounter; i++)
                    {
                        collisionData.nVectors = reader.ReadUInt32();
                        for (int n = 0; n < collisionData.nVectors; n++)
                        {
                            collisionData.bounds.Add(reader.ReadToVector3());
                        }

                        collisionData.upVector = reader.ReadToVector3();

                        for (int nn = 0; nn < 6; nn++)
                        {
                            collisionData.order[nn] = reader.ReadUInt16();
                        }

                        collisionData.unknown = reader.ReadToVector3();
                        this.collisionInfo.Add(collisionData);
                    }

                    unknownDataCounter = reader.ReadUInt32();

                    // Error check
                    if (unknownDataCounter < 1000)
                    {
                        // anything not within this range is probably bad
                        uint tempCounter = unknownDataCounter;
                        for (uint j = 0; j < tempCounter; j++)
                        {
                            unknownDataCounter = reader.ReadUInt32();

                            // range error check
                            if (unknownDataCounter > 10000) // nothing should be more than that
                            {
                                return;
                            }

                            // unknown data chunk(s)
                            for (uint i = 0; i < unknownDataCounter; i++)
                            {
                                for (int n = 0; n < collisionData1.nVectors; n++)
                                {
                                    collisionData1.bounds.Add(reader.ReadToVector3());
                                }
                                collisionData1.upVector = reader.ReadToVector3();

                                for (int nn = 0; nn < 6; nn++)
                                {
                                    collisionData1.order[nn] = reader.ReadUInt16();
                                }
                                collisionData1.unknown = reader.ReadToVector3();
                                this.collisionInfo.Add(collisionData1);
                            }
                        }
                    }

                    // --

                    // TODO:  This isn't null bytes,  its a counter of a new chunk of data
                    // skip over null bytes at the end of data chunk
                    // Counter for another data chunk
                    unknownDataCounter = reader.ReadUInt32();
                    for (uint i = 0; i < unknownDataCounter; i++)
                    {
                        reader.BaseStream.Position += 456;
                    }
                }

                // --
                this.numberOfMeshes = reader.ReadUInt32();

                if (this.numberOfMeshes > 5000)
                {
                    throw new ArgumentOutOfRangeException(DomainMessages.The_number_of_meshes_exceeded_5000);
                }

                for (uint i = 0; i < this.numberOfMeshes; i++)
                {
                    // Error handler
                    reader.BaseStream.Position += 9;
                    var id = reader.ReadUInt32();
                    // no point adding it to the stack, unless we actually have an id - basic error check
                    if (id > 0)
                    {
                        throw new NotImplementedException();

                        //var cii = new CacheIndexIdentity
                        //{
                        //    FileId = 7,
                        //    Id = (int)id
                        //};

                        //this.renderInfo.Add(this.renderRepository.BuildRenderInfo(cii));
                    }
                }
            }
        }
    }
}