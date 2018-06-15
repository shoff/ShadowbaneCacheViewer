namespace CacheViewer
{
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using CacheViewer.Domain.Data;

    /// <summary>
    /// </summary>
    internal static class Program
    {
        // TODO look into http://arraysegments.codeplex.com/

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataContext>());

            var status = CheckForCacheFiles();
            if (!status.AllFound)
            {
                OpenFileDialog ofd = new OpenFileDialog();
            }
            //CreateDb();

            var mainForm = ConfigurationManager.AppSettings["MainFormType"];

            if (mainForm == "CacheViewForm")
            {
                Application.Run(new CacheViewForm());
            }
            else
            {
                Application.Run(new MainForm());
            }


            // Application.Run(new CacheViewForm());

            // Application.Run(new SlimForm());

            // Application.Run(new MainForm());
        }

        private static CacheStatus CheckForCacheFiles()
        {
            CacheStatus cacheStatus = new CacheStatus();
            if (!Directory.Exists(ConfigurationManager.AppSettings["CacheFolder"]))
            {
                cacheStatus.CacheFolderFound = false;
                return cacheStatus;
            }

            cacheStatus.CacheFolderFound = true;
            cacheStatus.AllFound = true;

            var files = Directory.GetFiles(ConfigurationManager.AppSettings["CacheFolder"], "*.cache");
            if (files.Length == 0)
            {
                return cacheStatus;
            }
            cacheStatus.CObjectsFound = files.Contains("CObjects.cache");
            if (!cacheStatus.CObjectsFound)
            {
                cacheStatus.AllFound = false;
            }
            cacheStatus.CZoneFound = files.Contains("CZone.cache");
            if (!cacheStatus.CZoneFound)
            {
                cacheStatus.AllFound = false;
            }
            cacheStatus.DungeonFound = files.Contains("Dungeon.cache");
            if (!cacheStatus.DungeonFound)
            {
                cacheStatus.AllFound = false;
            }
            cacheStatus.MeshFound = files.Contains("Mesh.cache");
            if (!cacheStatus.MeshFound)
            {
                cacheStatus.AllFound = false;
            }
            cacheStatus.MotionFound = files.Contains("Motion.cache");
            if (!cacheStatus.MotionFound)
            {
                cacheStatus.AllFound = false;
            }
            cacheStatus.PaletteFound = files.Contains("Palette.cache");
            if (!cacheStatus.PaletteFound)
            {
                cacheStatus.AllFound = false;
            }
            cacheStatus.RenderFound = files.Contains("Render.cache");
            if (!cacheStatus.RenderFound)
            {
                cacheStatus.AllFound = false;
            }
            cacheStatus.SkeletonFound = files.Contains("Skeleton.cache");
            if (!cacheStatus.SkeletonFound)
            {
                cacheStatus.AllFound = false;
            }
            cacheStatus.SoundFound = files.Contains("Sound.cache");
            if (!cacheStatus.SoundFound)
            {
                cacheStatus.AllFound = false;
            }
            cacheStatus.TerrainAlphaFound = files.Contains("TerrainAlpha.cache");
            if (!cacheStatus.TerrainAlphaFound)
            {
                cacheStatus.AllFound = false;
            }
            cacheStatus.TexturesFound = files.Contains("Textures.cache");
            if (!cacheStatus.TexturesFound)
            {
                cacheStatus.AllFound = false;
            }
            cacheStatus.TileFound = files.Contains("Tile.cache");
            if (!cacheStatus.TileFound)
            {
                cacheStatus.AllFound = false;
            }
            cacheStatus.VisualFound = files.Contains("Visual.cache");
            if (!cacheStatus.VisualFound)
            {
                cacheStatus.AllFound = false;
            }
            return cacheStatus;
        }

        private static void CreateDb()
        {
            
        }
    }

    internal class CacheStatus
    {
        public bool AllFound
        {
            get;
            set;
        }
        public bool CacheFolderFound
        {
            get;
            set;
        }
        public bool CObjectsFound
        {
            get;
            set;
        }
        public bool CZoneFound
        {
            get;
            set;
        }
        public bool DungeonFound
        {
            get;
            set;
        }
        public bool MeshFound
        {
            get;
            set;
        }
        public bool MotionFound
        {
            get;
            set;
        }
        public bool PaletteFound
        {
            get;
            set;
        }
        public bool RenderFound
        {
            get;
            set;
        }
        public bool SkeletonFound
        {
            get;
            set;
        }
        public bool SoundFound
        {
            get;
            set;
        }
        public bool TerrainAlphaFound
        {
            get;
            set;
        }
        public bool TexturesFound
        {
            get;
            set;
        }
        public bool TileFound
        {
            get;
            set;
        }
        public bool VisualFound
        {
            get;
            set;
        }
    }
}