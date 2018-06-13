namespace CacheViewer.Domain.Services
{
    using System;
    using System.Collections.Generic;

    public interface IModelIdService
    {
        /// <summary>
        ///     Finds the model identifier asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="itemNumber">The item number.</param>
        /// <returns></returns>
        IList<RenderListViewMeshItem> FindModelId(ArraySegment<byte> data, int itemNumber = 1);
    }
}