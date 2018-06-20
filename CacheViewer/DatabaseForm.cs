
namespace CacheViewer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using ControlExtensions;
    using Domain.Models.Exportable;
    using Domain.Services;
    using Domain.Services.Prefabs;
    using NLog;

    public partial class DatabaseForm : Form
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static bool rendersSaved;
        private static bool cacheSaved;

        /// <summary>
        /// </summary>
        /// <param name="mobiles">
        /// </param>
        public DatabaseForm(List<CacheObject> mobiles)
        {
            this.InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                logger.Debug("DatabaseForm created.");
            }
        }


        private async void SaveTexturesButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                TextureDatabaseService service = new TextureDatabaseService();
                service.TexturesSaved += this.UpdateLabel;
                this.SaveTexturesButton.SetEnabled(false);
                await service.SaveToDatabase();
                this.SaveTexturesButton.SetEnabled(true);
                service.TexturesSaved -= this.UpdateLabel;
            });
            this.TextureSaveLabel.SetText("Textures saved to database");
        }

        private void UpdateLabel(object sender, TextureSaveEventArgs e)
        {
            this.TextureSaveLabel.SetText($"Count: {e.Count}");
        }

        private async void SaveMeshesButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                MeshDatabaseService service = new MeshDatabaseService();
                service.MeshesSaved += this.UpdateMeshesLabel;
                this.SaveMeshesButton.SetEnabled(false);
                await service.SaveToDatabase();
                this.SaveMeshesButton.SetEnabled(true);
                service.MeshesSaved -= this.UpdateMeshesLabel;
            });

            this.SaveMeshesLabel.SetText("Meshes saved to database");
        }

        private void UpdateMeshesLabel(object sender, MeshSaveEventArgs e)
        {
            this.SaveMeshesLabel.SetText($"Count: {e.Count}");
        }

        private async void SaveCacheButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                CacheObjectsDatabaseService service = new CacheObjectsDatabaseService();
                service.CacheObjectsSaved += this.UpdateCachesLabel;
                this.SaveCacheButton.SetEnabled(false);
                await service.SaveToDatabase();
                this.SaveCacheButton.SetEnabled(true);
                service.CacheObjectsSaved -= this.UpdateCachesLabel;
            });
            this.SaveCacheLabel.SetText("Cache objects saved to database");
            this.SetStatus("cache", true);
        }

        private void UpdateCachesLabel(object sender, CacheObjectSaveEventArgs e)
        {
            this.SaveCacheLabel.SetText($"CacheObjects Count: {e.CacheObjectsCount} - RenderAndOffsets Count: {e.RenderOffsetsCount}");
        }



        private async void RenderButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                RenderInfoDatabaseService service = new RenderInfoDatabaseService();
                service.RendersSaved += this.UpdateRendersLabel;
                this.RenderButton.SetEnabled(false);
                await service.SaveToDatabase();
                this.RenderButton.SetEnabled(true);
                service.RendersSaved -= this.UpdateRendersLabel;
            });
            this.RenderLabel.SetText("Render objects saved to database");
            this.SetStatus("render", true);
        }

        private void UpdateRendersLabel(object sender, RenderInfoSaveEventArgs e)
        {
            this.RenderLabel.SetText($"Count: {e.Count}");
        }

        private void SetStatus(string name, bool value)
        {
            if (name == "cache")
            {
                cacheSaved = value;
            }

            if (name == "render")
            {
                rendersSaved = value;
            }

            this.AssociateRenderButton.Enabled = cacheSaved && rendersSaved;
        }

        private async  void AssociateRenderButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                AssociateRenderOffsetsDatabaseService service = new AssociateRenderOffsetsDatabaseService();
                service.RenderOffsetsSaved += this.UpdateAssociateLabel;
                this.AssociateRenderButton.SetEnabled(false);
                await service.SaveToDatabase();
                this.AssociateRenderButton.SetEnabled(true);
                service.RenderOffsetsSaved -= this.UpdateAssociateLabel;
            });
            this.AssociateRenderLabel.SetText("Render objects saved to database");
        }

        private void UpdateAssociateLabel(object sender, RenderOffsetEventArgs e)
        {
            this.AssociateRenderLabel.SetText($"Count: {e.Count}");
        }

        private async void CreateElvenChurchButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                this.CreateChurchLabel.SetText("Creating structure");
                ElvenChurchService service = new ElvenChurchService();
                this.CreateElvenChurchButton.SetEnabled(false);
                await service.SaveAll();
                this.CreateElvenChurchButton.SetEnabled(true);
            });
            this.CreateChurchLabel.SetText("Finished saving");
        }
    }
}
