namespace Shadowbane.Models;

using System;
using System.Collections.Generic;
using Cache;

public abstract class ModelObject : CacheObject
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

    public ICollection<Exception> Errors { get; } = new List<Exception>();
}