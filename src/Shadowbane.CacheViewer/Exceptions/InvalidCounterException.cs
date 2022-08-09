namespace Shadowbane.CacheViewer.Exceptions;

using System;
using Cache;

[Serializable]
public class InvalidCounterException : ApplicationException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InvalidCounterException" /> class.
    /// </summary>
    /// <param name="cacheIndex">Index of the cache.</param>
    public InvalidCounterException(CacheIndex cacheIndex)
        : base(string.Format("Invalid counter parsed for CacheInded {0}", cacheIndex.identity))
    {
    }
}