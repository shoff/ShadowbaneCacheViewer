﻿namespace Shadowbane.Cache
{
    using System.IO;

    public static class CacheLocation
    {
        public static DirectoryInfo CacheFolder => new DirectoryInfo(@"C:\dev\Shadowbane\cache");
    }
}