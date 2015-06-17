using System;
using System.Collections.Generic;
using System.IO;
using CacheViewer.Domain.Archive;
using CacheViewer.Domain.Extensions;
using CacheViewer.Domain.Models.Exportable;
// ReSharper disable UnusedVariable
// ReSharper disable RedundantAssignment

namespace CacheViewer.Domain.Models
{
    using System.Diagnostics.Contracts;
    using NLog;

    public class Interactive : AnimationObject
    {
        //private bool bValue1;
        //private uint mapTex;
        //private bool bValue2;
        //private bool bValue3;
        private bool bWalkData;
        //private readonly CollisionInfo collisionData = new CollisionInfo();
        //private readonly CollisionInfo collisionData1 = new CollisionInfo();
        // ReSharper disable once InconsistentNaming
        protected readonly List<CollisionInfo> collisionInfo = new List<CollisionInfo>();
        // ReSharper disable once InconsistentNaming
        public readonly List<uint> renderIds = new List<uint>();

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
            Contract.Requires<ArgumentNullException>(data != null);
            Contract.Requires<ArgumentException>(data.Count > 0);

            int ptr = this.CursorOffset;
            // ReSharper disable once NotAccessedVariable
            uint iUnk = 0;
            //unknownData1 unkData1;
            CollisionInfo collisionData = new CollisionInfo();

            // Error handler
            try
            {
                using (BinaryReader reader = data.CreateBinaryReaderUtf32())
                {

                    reader.BaseStream.Position = ptr;

                    // ReSharper disable once RedundantAssignment
                    var renderId = reader.ReadUInt32();

                    //memcpy(&renderID, data + ptr, 4); // world texture id
                    ptr += 4;

                    //wxLogMessage(_T("Render ID: %i"), renderID);
                    var invTex = reader.ReadUInt32();

                    //memcpy(&invTex, data + ptr, 4); // inventory texture id
                    ptr += 4;

                    //wxLogMessage(_T("Inventory Texture ID: %i"), invTex);
                    var mapTex = reader.ReadUInt32();

                    //memcpy(&mapTex, data + ptr, 4); // Get the minimap texture id
                    ptr += 4;

                    //wxLogMessage(_T("Minimap Texture ID: %i"), mapTex);
                    iUnk = reader.ReadUInt32();

                    //memcpy(&iUnk, data + ptr, 4);
                    ptr += 4;

                    //wxLogMessage(_T("Unknown ID: %i"), iUnk);

                    iUnk = reader.ReadUInt32();

                    // memcpy(&iUnk, data + ptr, 4); 
                    ptr += 4;

                    //wxLogMessage(_T("Unknown ID: %i"), iUnk);
                    var counter = reader.ReadUInt32();

                    // Counter for number of records of unknown data
                    //memcpy(&counter, data + ptr, 4);
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
                    // ptr += sizeof(unknownData1) * counter;


                    // skip over unknown data
                    ptr += 108;
                    reader.BaseStream.Position += 108;

                    // 4 bytes that seem to contain bool info
                    //memcpy(&bValue1, data+ptr, 1);
                    var bValue1 = reader.ReadBoolean();
                    ptr++;

                    //memcpy(&bValue2, data+ptr, 1);
                    var bValue2 = reader.ReadBoolean();
                    ptr++;

                    //memcpy(&bValue3, data+ptr, 1);
                    var bValue3 = reader.ReadBoolean();
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
                            return;
                        }

                        // unknown data chunk(s)
                        // These chunks contain information like collision detection, walkable areas, etc - and some other data I'm unsure of.
                        for (uint i = 0; i < counter; i++)
                        {
                            // Error handler
                            try
                            {
                                //memcpy(&collisionData.nVectors, data + ptr, 4); 
                                collisionData.nVectors = reader.ReadUInt32();
                                ptr += 4;

                                //memcpy(&collisionData.bounds, data + ptr, (sizeof(Vec3D) * collisionData.nVectors));
                                //ptr += (sizeof(Vec3D) * collisionData.nVectors);
                                for (int x = 0; x < collisionData.nVectors; x++)
                                {
                                    collisionData.bounds.Add(reader.ReadToVector3());
                                }

                                //memcpy(&collisionData.upVector, data + ptr, sizeof(Vec3D));
                                //ptr += sizeof(Vec3D);
                                collisionData.upVector = reader.ReadToVector3();

                                //memcpy(&collisionData.order, data + ptr, sizeof(uint16) * 6);
                                //ptr += sizeof(uint16) * 6;
                                for (int y = 0; y < 6; y++)
                                {
                                    collisionData.order.Add(reader.ReadUInt16());
                                }

                                //memcpy(&collisionData.unknown, data + ptr, sizeof(Vec3D));
                                // ptr += sizeof(Vec3D);
                                collisionData.unknown = reader.ReadToVector3();

                                // collisionInfo.push_back(collisionData);
                                collisionInfo.Add(collisionData);
                            }
                            catch (Exception e1)
                            {
                                logger.Error(
                                    string.Format(
                                        "Exception thrown parsing CacheIndex {0}, in Interactive exception e1.",
                                        this.CacheIndex.identity),
                                    e1);
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
                                    // memcpy(&counter, data + ptr, 4); // Counter for the data chunk, identical to the above data chunk
                                    counter = reader.ReadUInt32();
                                    ptr += 4;

                                    // range error check
                                    if (counter > 10000) // nothing should be more than that
                                    {
                                        logger.Error("counter of {0} is out of range for CacheIndex {1} in Interactive line 209.",counter,this.CacheIndex.identity);
                                        return;
                                    }
                                }
                                catch (Exception e2)
                                {
                                    logger.Error(string.Format("Exception thrown parsing CacheIndex {0}, in Interactive exception e2.",this.CacheIndex.identity),e2);
                                    return;
                                }

                                // unknown data chunk(s)
                                for (uint i = 0; i < counter; i++)
                                {
                                    // Error handler
                                    try
                                    {

                                        //memcpy(&collisionData.nVectors, data + ptr, 4); 
                                        collisionData.nVectors = reader.ReadUInt32();
                                        ptr += 4;

                                        //memcpy(&collisionData.bounds, data + ptr, (sizeof(Vec3D) * collisionData.nVectors));
                                        //ptr += (sizeof(Vec3D) * collisionData.nVectors);
                                        for (int x = 0; x < collisionData.nVectors; x++)
                                        {
                                            collisionData.bounds.Add(reader.ReadToVector3());
                                        }

                                        //memcpy(&collisionData.upVector, data + ptr, sizeof(Vec3D));
                                        //ptr += sizeof(Vec3D);
                                        collisionData.upVector = reader.ReadToVector3();

                                        //memcpy(&collisionData.order, data + ptr, sizeof(uint16) * 6);
                                        //ptr += sizeof(uint16) * 6;
                                        for (int y = 0; y < 6; y++)
                                        {
                                            collisionData.order.Add(reader.ReadUInt16());
                                        }

                                        //memcpy(&collisionData.unknown, data + ptr, sizeof(Vec3D));
                                        // ptr += sizeof(Vec3D);
                                        collisionData.unknown = reader.ReadToVector3();

                                        // collisionInfo.push_back(collisionData);
                                        collisionInfo.Add(collisionData);


                                    }
                                    catch (Exception e3)
                                    {
                                        logger.Error(
                                            string.Format(
                                                "Exception thrown parsing CacheIndex {0}, in Interactive exception e3.",
                                                this.CacheIndex.identity),
                                            e3);
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
                        logger.Error("counter of {0} is out of range for walk data in Structure parsing for CacheIndex {1} in Interactive line 276.",counter,this.CacheIndex.identity);
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
                            // no point adding it to the stack, unless we actually have an id - basic error check
                            if (renderId > 0)
                            {
                                renderIds.Add(renderId);
                            }
                        }
                        catch (Exception e4)
                        {
                            logger.Error(string.Format("Exception thrown parsing CacheIndex {0}, in Interactive exception e4.",this.CacheIndex.identity), e4);
                            break;
                        }
                    }
                }
            }
            catch(Exception e5)
            {
                logger.Error(string.Format("Exception thrown parsing CacheIndex {0}, in Interactive exception e5.", this.CacheIndex.identity), e5);
                //wxLogMessage(_T("Error: Failed to load Object Type 4"));
            }
        }
    }
}
// ReSharper restore UnusedVariable
// ReSharper restore RedundantAssignment