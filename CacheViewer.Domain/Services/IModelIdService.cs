namespace CacheViewer.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ModelIdServiceContract))]
    public interface IModelIdService
    {
        /// <summary>
        /// Finds the model identifier asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="itemNumber">The item number.</param>
        /// <returns></returns>
        IList<RenderListViewMeshItem> FindModelId(ArraySegment<byte> data, int itemNumber = 1);
    }

    [ContractClassFor(typeof(IModelIdService))]
    public abstract class ModelIdServiceContract : IModelIdService
    {
        public IList<RenderListViewMeshItem> FindModelId(ArraySegment<byte> data, int itemNumber = 1)
        {
            Contract.Requires<ArgumentNullException>(data != null);
            Contract.Ensures(Contract.Result<IList<RenderListViewMeshItem>>() != null);
            return default(IList<RenderListViewMeshItem>);
        }
    }
}