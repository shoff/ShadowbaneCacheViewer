 // ReSharper disable UnusedVariable
// ReSharper disable RedundantAssignment

namespace CacheViewer.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Models.Exportable;
    using NLog;

    public class Interactive : AnimationObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // ReSharper disable once InconsistentNaming
        protected readonly List<CollisionInfo> collisionInfo = new List<CollisionInfo>();

        // ReSharper disable once InconsistentNaming
        public readonly List<uint> renderIds = new List<uint>();
        private bool bWalkData;

        /// <summary>
        /// Initializes a new instance of the <see cref="Interactive"/> class.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <param name="flag">The flag.</param>
        /// <param name="name">The name.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="data">The data.</param>
        /// <param name="innerOffset">The inner offset.</param>
        public Interactive(CacheIndex cacheIndex, ObjectType flag, string name, int offset, ArraySegment<byte> data, int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

        /// <summary>
        /// Parses the specified buffer.
        /// </summary>
        /// <param name="data">The buffer.</param>
        public override void Parse(ArraySegment<byte> data)
        {
            int ptr = this.CursorOffset;

            // ReSharper disable once NotAccessedVariable
            uint iUnk = 0;

            //unknownData1 unkData1;
            CollisionInfo collisionData = new CollisionInfo();

            try
            {
                using (BinaryReader reader = data.CreateBinaryReaderUtf32())
                {
                    reader.BaseStream.Position = ptr;

                    // ReSharper disable once RedundantAssignment
                    var renderId = reader.ReadUInt32();
                    ptr += 4;
                    var invTex = reader.ReadUInt32();
                    ptr += 4;
                    var mapTex = reader.ReadUInt32();
                    ptr += 4;
                    iUnk = reader.ReadUInt32();
                    ptr += 4;
                    iUnk = reader.ReadUInt32();
                    ptr += 4;
                    var counter = reader.ReadUInt32();
                    // Counter for number of records of unknown data
                    ptr += 4;

                    // range error check
                    if (counter > 10000) // one million, nothing should have more than that
                    {
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
                    // skip over unknown data
                    ptr += 108;
                    reader.BaseStream.Position += 108;
                    var bValue1 = reader.ReadBoolean();
                    ptr++;

                    //memcpy(&bValue2, data+ptr, 1);
                    var bValue2 = reader.ReadBoolean();
                    ptr++;
                    var bValue3 = reader.ReadBoolean();
                    ptr++;
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
                            return;
                        }

                        // unknown data chunk(s)
                        // These chunks contain information like collision detection, walkable areas, etc - and some other data I'm unsure of.
                        for (uint i = 0; i < counter; i++)
                        {
                            try
                            {
                                collisionData.nVectors = reader.ReadUInt32();
                                ptr += 4;
                                for (int x = 0; x < collisionData.nVectors; x++)
                                {
                                    collisionData.bounds.Add(reader.ReadToVector3());
                                }

                                collisionData.upVector = reader.ReadToVector3();
                                for (int y = 0; y < 6; y++)
                                {
                                    collisionData.order.Add(reader.ReadUInt16());
                                }
                                collisionData.unknown = reader.ReadToVector3();
                                collisionInfo.Add(collisionData);
                            }
                            catch (Exception e1)
                            {
                                logger.Error(e1, "Exception thrown parsing CacheIndex {0}, in Interactive exception e1.",this.CacheIndex.Identity);
                                throw;
                            }
                        }

                        // Counter for another data chunk similar to above
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
                                    // Counter for the data chunk, identical to the above data chunk
                                    counter = reader.ReadUInt32();
                                    ptr += 4;

                                    // range error check
                                    if (counter > 10000) // nothing should be more than that
                                    {
                                        logger.Error("counter of {0} is out of range for CacheIndex {1} in Interactive line 209.",
                                            counter, this.CacheIndex.Identity);
                                        return;
                                    }
                                }
                                catch (Exception e2)
                                {
                                    logger.Error(e2, "Exception thrown parsing CacheIndex {0}, in Interactive exception e2.", this.CacheIndex.Identity);
                                    return;
                                }

                                // unknown data chunk(s)
                                for (uint i = 0; i < counter; i++)
                                {
                                    try
                                    {
                                        collisionData.nVectors = reader.ReadUInt32();
                                        ptr += 4;
                                        for (int x = 0; x < collisionData.nVectors; x++)
                                        {
                                            collisionData.bounds.Add(reader.ReadToVector3());
                                        }

                                        collisionData.upVector = reader.ReadToVector3();

                                        for (int y = 0; y < 6; y++)
                                        {
                                            collisionData.order.Add(reader.ReadUInt16());
                                        }
                                        collisionData.unknown = reader.ReadToVector3();
                                        collisionInfo.Add(collisionData);
                                    }
                                    catch (Exception e3)
                                    {
                                        logger.Error(e3, "Exception thrown parsing CacheIndex {0}, in Interactive exception e3.",this.CacheIndex.Identity);
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

                    // Num of meshes
                    counter = reader.ReadUInt32();
                    ptr += 4;
                    if (counter > 5000)
                    {
                        // Range check for invalid values
                        logger.Error(
                            "counter of {0} is out of range for walk data in Structure parsing for CacheIndex {1} in Interactive line 276.",
                            counter, this.CacheIndex.Identity);
                        return;
                    }

                    for (uint i = 0; i < counter; i++)
                    {
                        try
                        {
                            ptr += 9; // 9 null bytes ?
                            reader.BaseStream.Position += 9;

                            renderId = reader.ReadUInt32();
                            ptr += 4;
                            // no point adding it to the stack, unless we actually have an id - basic error check
                            if (renderId > 0)
                            {
                                renderIds.Add(renderId);
                            }
                        }
                        catch (Exception e4)
                        {
                            logger.Error(e4, "Exception thrown parsing CacheIndex {0}, in Interactive exception e4.",this.CacheIndex.Identity);
                            break;
                        }
                    }
                }
            }
            catch (Exception e5)
            {
                logger.Error(e5, "Exception thrown parsing CacheIndex {0}, in Interactive exception e5.", this.CacheIndex.Identity);
            }
        }
    }
}

// ReSharper restore UnusedVariable
// ReSharper restore RedundantAssignment