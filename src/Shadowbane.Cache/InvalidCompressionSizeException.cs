namespace Shadowbane.Cache;

using System;
using System.Runtime.Serialization;

[Serializable]
public sealed class InvalidCompressionSizeException : Exception
{
    public InvalidCompressionSizeException(string message)
        : base(message)
    {
    }

    private InvalidCompressionSizeException(
        SerializationInfo serializationInfo,
        StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }
        
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }
}