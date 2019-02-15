namespace CacheViewer.Domain.Exporters
{
    using System;

    public class MayaObjHeaderFactory
    {
        private const string SbCacheId = "# Shadowbane cacheObject id: {0}\r\n";
        private const string SbCacheName = "# Shadowbane cacheObject: {0}\r\n";
        private const string SbCreated = "# created on: {0}\r\n";

        public static MayaObjHeaderFactory Instance => new MayaObjHeaderFactory();

        public string Create(int identity)
        {
            var id = string.Format(SbCacheId, identity);
            var created = string.Format(SbCreated, DateTime.Now);
            var createdAll = string.Join(string.Empty, id, created);
            return createdAll;
        }

        public string Create(string name)
        {
            var id = string.Format(SbCacheName, name);
            var created = string.Format(SbCreated, DateTime.Now);
            var createdAll = string.Join(string.Empty, id, created);
            return createdAll;
        }
    }
}