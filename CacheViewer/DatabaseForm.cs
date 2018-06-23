
namespace CacheViewer
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using ControlExtensions;
    using Domain.Services;
    using Domain.Services.Prefabs;
    using NLog;

    public partial class DatabaseForm : Form
    {
        private int validRange = 5000;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DatabaseForm()
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
                this.ClearDataButton.SetEnabled(false);
                await service.SaveToDatabaseAsync();
                this.SaveTexturesButton.SetEnabled(true);
                this.ClearDataButton.SetEnabled(true);
                service.TexturesSaved -= this.UpdateLabel;
            });
            this.TextureSaveLabel.SetText("Textures saved to database");
            this.SaveMeshesButton.Enabled = true;
            this.SaveMeshesButton.Focus();
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
                this.ClearDataButton.SetEnabled(false);
                await service.SaveToDatabaseAsync();
                this.ClearDataButton.SetEnabled(true);
                this.SaveMeshesButton.SetEnabled(true);
                service.MeshesSaved -= this.UpdateMeshesLabel;
            });

            this.SaveMeshesLabel.SetText("Meshes saved to database");
            this.RenderButton.Enabled = true;
            this.RenderButton.Focus();
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
                this.ClearDataButton.SetEnabled(false);
                await service.SaveToDatabaseAsync(this.validRange);
                this.ClearDataButton.SetEnabled(true);
                this.SaveCacheButton.SetEnabled(true);
                service.CacheObjectsSaved -= this.UpdateCachesLabel;
            });
            this.SaveCacheLabel.SetText("Cache objects saved to database");
            this.AssociateRenderButton.Enabled = true;
            this.AssociateRenderButton.Focus();
        }

        private void UpdateCachesLabel(object sender, CacheObjectSaveEventArgs e)
        {
            this.SaveCacheLabel.SetText($"CacheObjects Count: {e.CacheObjectsCount} - RenderAndOffsets Count: {e.RenderOffsetsCount}");
        }

        private bool renderHasBeenSaved;
        private async void RenderButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                RenderInfoDatabaseService service = new RenderInfoDatabaseService();
                service.RendersSaved += this.UpdateRendersLabel;
                this.RenderButton.SetEnabled(false);
                this.ClearDataButton.SetEnabled(false);
                await service.SaveToDatabaseAsync();
                this.ClearDataButton.SetEnabled(true);
                this.RenderButton.SetEnabled(true);
                service.RendersSaved -= this.UpdateRendersLabel;
            });
            this.RenderLabel.SetText("Render objects saved to database");
            this.renderHasBeenSaved = true;
            this.SaveCacheButton.Enabled = true;
            this.SaveCacheButton.Focus();
        }

        private void UpdateRendersLabel(object sender, RenderInfoSaveEventArgs e)
        {
            this.RenderLabel.SetText($"Count: {e.Count}");
        }

        private async  void AssociateRenderButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                AssociateRenderOffsetsDatabaseService service = new AssociateRenderOffsetsDatabaseService();
                service.RenderOffsetsSaved += this.UpdateAssociateLabel;
                this.AssociateRenderButton.SetEnabled(false);
                this.ClearDataButton.SetEnabled(false);
                await service.SaveToDatabaseAsync();
                this.ClearDataButton.SetEnabled(true);
                this.AssociateRenderButton.SetEnabled(true);
                service.RenderOffsetsSaved -= this.UpdateAssociateLabel;
            });
            this.AssociateRenderLabel.SetText("Render objects saved to database");
            this.LizardManTempleButton.SetEnabled(true);
            this.CreateElvenChurchButton.SetEnabled(true);
            this.RangerBlindButton.SetEnabled(true);
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
                this.ClearDataButton.SetEnabled(false);
                await service.SaveAllAsync();
                this.ClearDataButton.SetEnabled(true);
                this.CreateElvenChurchButton.SetEnabled(true);
            });
            this.CreateChurchLabel.SetText("Finished saving");
        }

        private async void RangerBlindButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                this.RangerBlindLabel.SetText("Creating structure");
                RangerBlindService service = new RangerBlindService();
                this.RangerBlindButton.SetEnabled(false);
                this.ClearDataButton.SetEnabled(false);
                await service.SaveAllAsync();
                this.ClearDataButton.SetEnabled(true);
                this.RangerBlindButton.SetEnabled(true);
            });
            this.RangerBlindLabel.SetText("Finished saving");
        }

        private async void LizardManTempleButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                this.LizardManTempleLabel.SetText("Creating structure");
                LizardManTempleService service = new LizardManTempleService();
                this.LizardManTempleButton.SetEnabled(false);
                this.ClearDataButton.SetEnabled(false);
                await service.SaveAllAsync();
                this.ClearDataButton.SetEnabled(true);
                this.LizardManTempleButton.SetEnabled(true);
            });
            this.LizardManTempleLabel.SetText("Finished saving");
        }

        private void RangeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(this.RangeTextBox.Text, out this.validRange))
            {
                this.validRange = 0;
                this.ValidRangeValidationLabel.Text =
                    $"{this.RangeTextBox.Text ?? "Null"} is not a valid unsigned integer value.";
                this.RangeTextBox.Text = "0";
                this.SaveCacheButton.Enabled = false;
            }
            else
            {
                if (this.renderHasBeenSaved)
                {
                    this.SaveCacheButton.Enabled = true;
                }
                this.ValidRangeValidationLabel.Text = "";
            }
        }

        private void DatabaseForm_Load(object sender, EventArgs e)
        {
        }

        private async void ClearDataButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                this.ClearDatabaseLabel.SetText("Clearing data from database.");
                ClearDatabaseService service = new ClearDatabaseService();
                this.AssociateRenderButton.SetEnabled(false);
                this.SaveTexturesButton.SetEnabled(false);
                this.RenderButton.SetEnabled(false);
                this.SaveMeshesButton.SetEnabled(false);
                this.SaveCacheButton.SetEnabled(false);
                this.ClearDataButton.SetEnabled(false);
                this.LizardManTempleButton.SetEnabled(false);
                this.CreateElvenChurchButton.SetEnabled(false);
                this.RangerBlindButton.SetEnabled(false);

                await service.ClearDataAsync();
                this.ClearDataButton.SetEnabled(true);
                this.SaveTexturesButton.SetEnabled(true);
            });
            this.ClearDatabaseLabel.SetText("");
            this.SaveTexturesButton.Focus();
        }
    }
}
