using System;

namespace CacheViewer.Domain.Exporters
{
    public class MayaObjHeaderFactory
    {
        private const string SbCacheId = "# Shadowbane cacheObject id: {0}\r\n";
        private const string SbCreated = "# created on: {0}\r\n";
        
        /// <summary>
        /// Creates the specified identity.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns></returns>
        public string Create(int identity)
        {
            string id = string.Format(SbCacheId, identity);
            string created = string.Format(SbCreated, DateTime.Now);
            var createdAll = string.Join(String.Empty, new[] {id, created});
            return createdAll;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static MayaObjHeaderFactory Instance
        {
            get { return new MayaObjHeaderFactory(); }
        }
    }
}