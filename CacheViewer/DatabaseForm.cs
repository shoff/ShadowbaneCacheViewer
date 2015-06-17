
using System.IO;

namespace CacheViewer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Data;
    using CacheViewer.Domain.Data.Entities;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Models;
    using CacheViewer.Domain.Models.Exportable;
    using CacheViewer.Domain.Services;
    using NLog;

    public partial class DatabaseForm : Form
    {
        private readonly List<CacheObject> mobiles;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private CacheObjectFactory cacheObjectFactory;
        private readonly DataContext dataContext;

        /// <summary>
        /// </summary>
        /// <param name="mobiles">
        /// </param>
        public DatabaseForm(List<CacheObject> mobiles)
        {
            this.mobiles = mobiles;
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                logger.Debug("DatabaseForm created.");
                this.cacheObjectFactory = CacheObjectFactory.Instance;
                this.dataContext = new DataContext();
                this.dataContext.Configuration.AutoDetectChangesEnabled = false;
                this.dataContext.ValidateOnSave = false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void SaveAllToDatabaseButtonClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void SaveMobilesButtonClick(object sender, EventArgs e)
        {
            foreach (var mobile in this.mobiles)
            {
                mobile.Parse(mobile.Data);
                MobileEntity me = (Mobile)mobile;
                this.dataContext.MobileEntities.Add(me);
                this.dataContext.SaveChanges();
            }
            this.SaveMobilesLabel.Text = "done";
        }

        private void SaveSkelButtonClick(object sender, EventArgs e)
        {
            var motionArchive = ArchiveFactory.Instance.Build(CacheFile.Motion, true);
            var archive = ArchiveFactory.Instance.Build(CacheFile.Skeleton, true);

            var directory = AppDomain.CurrentDomain.BaseDirectory + "\\Skeleton";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var motionDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Motion";
            if (!Directory.Exists(motionDirectory))
            {
                Directory.CreateDirectory(motionDirectory);
            }

            foreach (var motion in motionArchive.CacheIndices)
            {
                ArraySegment<byte> buffer = motionArchive[motion.identity].Item1;

                // await SaveBinaryData(directory + "\\cobject.cache", item.Data);
                FileWriter.Writer.Write(buffer, motionDirectory + "\\motion_" + motion.identity + ".cache");

                this.dataContext.MotionEntities.Add(new MotionEntity
                {
                    CacheIdentity = motion.identity
                });
                this.dataContext.SaveChanges();
            }
            this.SkeletonLabel.Text = "Saved all motion files";

            foreach (var skel in archive.CacheIndices)
            {
                ArraySegment<byte> buffer = archive[skel.identity].Item1;

                // await SaveBinaryData(directory + "\\cobject.cache", item.Data);
                FileWriter.Writer.Write(buffer, directory + "\\skeleton_" + skel.identity + ".cache");

                this.dataContext.SkeletonEntities.Add(new Skeleton(buffer, skel.identity));
                this.dataContext.SaveChanges();
            }

            this.SkeletonLabel.Text = "Saved all skeletons.";
        }


        private async Task SaveBinaryData(string fileName, ArraySegment<byte> data)
        {

            if (data.Count > 0)
            {
                await FileWriter.Writer.WriteAsync(data, fileName);

                //if (asset.Item2.Count > 0)
                //{
                //    await
                //        FileWriter.Writer.WriteAsync(
                //            asset.Item2,
                //            Path.Combine(
                //                path,
                //                this.saveName + asset.CacheIndex2.identity.ToString(CultureInfo.InvariantCulture) + "_1.cache"));
                //}
            }
        }
    }
}
