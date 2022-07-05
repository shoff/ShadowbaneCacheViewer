namespace Shadowbane.Models;

using System;
using Cache;

public abstract record ModelObject : CacheObject
{
    protected ModelObject(
        uint identity, 
        ObjectType flag, 
        string name, 
        uint offset, 
        ReadOnlyMemory<byte> data,
        uint innerOffset)
        : base(identity, flag, name, offset, data, innerOffset)
    {
    }
}