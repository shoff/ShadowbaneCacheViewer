namespace Shadowbane.Models
{
    using System;
    using Cache;

    public class Equipment : ModelObject
    {
        // ReSharper disable NotAccessedField.Local
        private uint inventoryTextureId;
        private uint mapTex;

        public Equipment(uint identity, string name, uint offset, ReadOnlyMemory<byte> data,
            uint innerOffset)
            : base(identity, ObjectType.Equipment, name, offset, data, innerOffset)
        {
        }

        public override ICacheObject Parse()
        {
            using var reader = this.Data.CreateBinaryReaderUtf32(this.CursorOffset);
            // ReSharper disable once NotAccessedVariable
            uint iUnk = 0;
            reader.BaseStream.Position = this.CursorOffset;

            this.RenderId = reader.ReadUInt32();
//#if DEBUG
//            long tempOffset = reader.BaseStream.Position;
//            this.ValidateRenderId(reader);
//            reader.BaseStream.Position = tempOffset;
//#endif
            // skip null bytes.
            reader.BaseStream.Position += 24;

            this.inventoryTextureId = reader.ReadUInt32();

            //memcpy(&invTex, data + ptr, 4); // inventory texture id
            //ptr += 4;
            //wxLogMessage(_T("Inventory Texture ID: %i"), invTex);
            this.mapTex = reader.ReadUInt32();

            //memcpy(&mapTex, data + ptr, 4); // Get the minimap texture id
            //ptr += 4;
            //wxLogMessage(_T("Minimap Texture ID: %i"), mapTex);
            // ReSharper disable once RedundantAssignment
            iUnk = reader.ReadUInt32();

            //memcpy(&iUnk, data + ptr, 4);
            //ptr += 4;
            //wxLogMessage(_T("Unknown ID: %i"), iUnk);
            // ReSharper disable once RedundantAssignment
            iUnk = reader.ReadUInt32();

            //memcpy(&iUnk, data + ptr, 4); 
            //ptr += 4;
            //wxLogMessage(_T("Unknown ID: %i"), iUnk);

            if (this.RenderId == 0)
            {
                //logger.Error("No RenderId found for Equipment CacheIndex {0}", this.CacheIndex.Identity);
            }

            //var cii = new CacheIndexIdentity
            //{
            //    FileId = 7,
            //    Id = renderId
            //};

            //var ri = this.renderRepository.BuildRenderInfo(cii);
            //this.renderInfo.Add(ri);
            return this;
        }

        //private void ValidateRenderId(BinaryReader reader)
        //{
        //    if (this.RenderId == 0 || this.RenderId > 1000000)
        //    {

        //        // how many variations do we want to try?
        //        // probably should be sticking these in the DB so we
        //        // can figure out what the pattern is here on invalid offsets
        //        // let's try 10
        //        int i = 0;
        //        reader.BaseStream.Position = this.CursorOffset - 4; // back up 4? 
        //        while (i <= 10)
        //        {
        //            var tempRenderId = reader.ReadUInt32();
        //            if (ArchiveLoader.RenderArchive[this.RenderId] == null)
        //            {
        //                // hmmm
        //            }

        //            i++;
        //        }
        //    }
        //}
    }
}