using System;
using System.Collections;
using System.Data.Common;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;
using System.Linq;

namespace CacheViewer.Domain.Data
{
    /// <summary>
    /// Basic methods that all <see cref="CacheViewer.Domain.Data.DataContext"/> objects share.
    /// </summary>
    public interface IDataContext : IQueryableDataContext
    {
    }
}