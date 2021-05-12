namespace Shadowbane.Exporter.Wavefront
{
    using System;

    public class MayaObjHeaderFactory
    {
        private const string SB_CACHE_ID = "# Shadowbane cacheObject id: {0}\r\n";
        private const string SB_CACHE_NAME = "# Shadowbane cacheObject: {0}\r\n";
        private const string SB_CREATED = "# created on: {0}\r\n";

        public static MayaObjHeaderFactory Instance => new MayaObjHeaderFactory();

        public string Create(int identity)
        {
            var id = string.Format(SB_CACHE_ID, identity);
            var created = string.Format(SB_CREATED, DateTime.Now);
            var createdAll = string.Join(string.Empty, id, created);
            return createdAll;
        }

        public string Create(string name)
        {
            var id = string.Format(SB_CACHE_NAME, name);
            var created = string.Format(SB_CREATED, DateTime.Now);
            var createdAll = string.Join(string.Empty, id, created);
            return createdAll;
        }
    }
}