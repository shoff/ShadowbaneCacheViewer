namespace CacheViewer.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensions;
    using Factories;

    public static class FindRenderIdService
    {
        /// <summary>
        ///     Given a segment of data, this loops through each group of 4 bytes sequentially, looking for
        ///     matching Id's in the render index. Note, found Ids may or may not be related to the original
        ///     render object, some logic will be needed to determine false positives.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static IList<Tuple<int, int>> FindRenderIds(ArraySegment<byte> data)
        {
            var idList = new List<Tuple<int, int>>();

            // this just gets the length of the data 
            var count = data.Count;

            using (var reader = data.CreateBinaryReaderUtf32())
            {
                for (var offset = 25; offset < count - 4; offset++)
                {
                    reader.BaseStream.Position = offset;

                    // we're testing each possible int in this array 
                    // to see if there is a corresponding id in the 
                    // index array from the RenderArchive
                    var id = reader.ReadInt32();

                    // Only test the int if it falls within a range determined by the item itself.
                    // The identityRange is just the lowest id in the cache and the highest id, if it 
                    // falls outside of those bounds, it's obviously wrong.
                    if (id.TestRange(RenderInformationFactory.Instance.IdentityRange.Item1,
                        RenderInformationFactory.Instance.IdentityRange.Item2))
                    {
                        // simple query to look up the id
                        var found = RenderInformationFactory.Instance.IdentityArray.Where(x => x == id).Any();

                        if (found)
                        {
                            idList.Add(new Tuple<int, int>(id, offset));
                        }
                    }
                }
            }

            return idList;
        }
    }
}