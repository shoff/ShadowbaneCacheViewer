namespace Shadowbane.Cache;

using System;

[Serializable]
public sealed class HeaderFileSizeException : Exception
{
    public HeaderFileSizeException(string message)
        : base(message)
    {
    }
}