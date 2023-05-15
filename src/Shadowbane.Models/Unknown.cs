namespace Shadowbane.Models;

using System;
using Cache;

public record Unknown(uint Identity, ObjectType Flag, string Name, uint CursorOffset, ReadOnlyMemory<byte> Data) 
    : ModelRecord(Identity, Flag, Name, CursorOffset, Data)
{
    private const int VALID_RANGE = 50000;
    
    public override void Parse()
    {
        using var reader = this.Data.CreateBinaryReaderUtf32();
        // this is unknown at the time so let's see if there are just a bunch of render ids in here.
        while (reader.CanRead(4) && this.RenderId == 0)
        {
            var off = reader.BaseStream.Position;

            var id = reader.ReadUInt32();
            var valid = Validate(id, Identity);

            if (valid)
            {
                this.RenderId = id;
                this.RenderIds.Add(id);
            }
            else
            {
                // rewind 3 bytes and read the next
                this.RecordInvalidRenderId(id);
                off++;
                reader.BaseStream.Position = off;
            }
        }

        // multiple render ids? wtf was I thinking with this?
        while (reader.CanRead(4))
        {
            var id = reader.ReadUInt32();
            var valid = Validate(id, Identity);
            if (valid)
            {
                if (!this.RenderIds.Contains(id))
                {
                    this.RenderIds.Add(id);
                }
            }
            else
            {
                this.RecordInvalidRenderId(id);
            }
        }
    }

    private bool Validate(uint id, uint identity)
    {
        if (id <= 0 || id > IdLookup.HighestObjectId)
        {
            return false;
        }

        if (!IdLookup.IsValidObjectId(id) || id < 100)
        {
            return false;
        }

        var range = (int)(id > identity ? Math.Abs(id - identity) : Math.Abs(identity - id));

        if (Math.Abs(range) > VALID_RANGE)
        {
            return false;
        }
        return true;
    }

}