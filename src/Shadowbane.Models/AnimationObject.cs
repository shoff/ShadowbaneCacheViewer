namespace Shadowbane.Models;

using System;
using Cache;

public abstract class AnimationObject : ModelObject
{
    protected AnimationObject(uint identity, ObjectType flag, string name, uint offset,
        ReadOnlyMemory<byte> data, uint innerOffset)
        : base(identity, flag, name, offset, data, innerOffset)
    {
    }

    public ICacheObject Skeleton { get; set; }
}