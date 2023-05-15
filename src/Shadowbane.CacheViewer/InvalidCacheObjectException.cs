namespace Shadowbane.CacheViewer;

internal class InvalidCacheObjectException : Exception
{
    public InvalidCacheObjectException(uint ciIdentity)
        :base($"Invalid cache object index {ciIdentity}")
    {
        
    }
}