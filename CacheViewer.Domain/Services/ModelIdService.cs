namespace CacheViewer.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensions;
    using Factories;

    /// <summary>
    /// </summary>
    public class ModelIdService : IModelIdService
    {
        /// <summary>
        ///     Returns a tuple containing the found ID and the offset it was found
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="itemNumber">The item number.</param>
        /// <returns></returns>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        public IList<RenderListViewMeshItem> FindModelId(ArraySegment<byte> data, int itemNumber = 1)
        {
            var idList = new List<RenderListViewMeshItem>();
            var count = data.Count;

            using (var reader = data.CreateBinaryReaderUtf32())
            {
                // we start at 25 because all RenderInformation cache items
                // have the same 25 starting information which is NOT child ids.
                for (var offset = 25; offset < count - 4; offset++)
                {
                    reader.BaseStream.Position = offset;

                    var id = reader.ReadInt32();

                    if (id.TestRange(MeshFactory.Instance.IdentityRange.Item1,
                        MeshFactory.Instance.IdentityRange.Item2))
                    {
                        // only set result to true.
                        var found = MeshFactory.Instance.IdentityArray.Where(x => x == id).Any();

                        if (found)
                        {
                            idList.Add(new RenderListViewMeshItem
                                {Id = id.ToString(), Offset = offset.ToString(), ItemNumber = itemNumber});
                        }
                    }
                }
            }

            return idList;
        }
    }
}