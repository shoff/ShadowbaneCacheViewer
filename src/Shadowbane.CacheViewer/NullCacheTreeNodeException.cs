namespace Shadowbane.CacheViewer;

internal class NullCacheTreeNodeException : Exception
{
    public NullCacheTreeNodeException(string message)
        : base(message) { }
}