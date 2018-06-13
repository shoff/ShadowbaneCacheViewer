﻿namespace CacheViewer.Domain.Exporters
{
    using System;

    public class MayaObjHeaderFactory
    {
        private const string SbCacheId = "# Shadowbane cacheObject id: {0}\r\n";
        private const string SbCreated = "# created on: {0}\r\n";

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>
        ///     The instance.
        /// </value>
        public static MayaObjHeaderFactory Instance => new MayaObjHeaderFactory();

        /// <summary>
        ///     Creates the specified identity.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns></returns>
        public string Create(int identity)
        {
            var id = string.Format(SbCacheId, identity);
            var created = string.Format(SbCreated, DateTime.Now);
            var createdAll = string.Join(string.Empty, id, created);
            return createdAll;
        }
    }
}