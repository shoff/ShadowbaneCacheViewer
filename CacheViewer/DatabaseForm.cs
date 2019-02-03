
namespace CacheViewer
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using ControlExtensions;
    using Domain.Services.Database;
    using Domain.Services.Prefabs;
    using Models;
    using NLog;

    public partial class DatabaseForm : Form
    {
        private int validRange = 5000;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly DatabaseState databaseState;

        public DatabaseForm()
        {
            this.InitializeComponent();
            this.databaseState = new DatabaseState();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                logger.Debug("DatabaseForm created.");
            }
            this.databaseState.StateChanged += SetState;
        }
        
        private async void SaveTexturesButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                this.SetCheckboxState(false);
                var service = new TextureDatabaseService();
                service.TexturesSaved += this.UpdateLabel;
                await service.SaveToDatabaseAsync();
                service.TexturesSaved -= this.UpdateLabel;
            });
            this.SetCheckboxState(true);
            this.TextureSaveLabel.SetText("Textures saved to database");
            this.databaseState.TexturesSaved = true;
        }

        private void UpdateLabel(object sender, TextureSaveEventArgs e)
        {
            this.TextureSaveLabel.SetText($"Count: {e.Count}");
        }

        private async void SaveMeshesButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                this.SetCheckboxState(false);
                var service = new MeshDatabaseService();
                service.MeshesSaved += this.UpdateMeshesLabel;
                await service.SaveToDatabaseAsync();
                service.MeshesSaved -= this.UpdateMeshesLabel;
            });

            this.databaseState.MeshesSaved = true;
            this.SetCheckboxState(true);
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
                this.SetCheckboxState(false);
                var service = new CacheObjectsDatabaseService();
                service.CacheObjectsSaved += this.UpdateCachesLabel;
                await service.SaveToDatabaseAsync(this.validRange);
                service.CacheObjectsSaved -= this.UpdateCachesLabel;
            });
            this.SaveCacheLabel.SetText("Cache objects saved to database");
            this.databaseState.CObjectSaved = true;
            this.SetCheckboxState(true);
        }

        private void UpdateCachesLabel(object sender, CacheObjectSaveEventArgs e)
        {
            this.SaveCacheLabel.SetText($"CacheObjects Count: {e.CacheObjectsCount} - RenderAndOffsets Count: {e.RenderOffsetsCount}");
        }

        private async void RenderButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                this.SetCheckboxState(false);
                var service = new RenderInfoDatabaseService();
                service.RendersSaved += this.UpdateRendersLabel;
                await service.SaveToDatabaseAsync();
                service.RendersSaved -= this.UpdateRendersLabel;
            });
            this.databaseState.RenderSaved = true;
            this.SetCheckboxState(true);
            this.RenderLabel.SetText("Render objects saved to database");
        }

        private void UpdateRendersLabel(object sender, RenderInfoSaveEventArgs e)
        {
            this.RenderLabel.SetText($"Count: {e.Count}");
        }

        private async void AssociateRenderButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                this.SetCheckboxState(false);
                var service = new AssociateRenderOffsetsDatabaseService();
                service.RenderOffsetsSaved += this.UpdateAssociateLabel;
                await service.AssociateRenderAndOffsets();
                service.RenderOffsetsSaved -= this.UpdateAssociateLabel;
            });
            this.AssociateRenderLabel.SetText("Render objects association completed");
            this.databaseState.RenderOffsetSaved = true;
            this.SetCheckboxState(true);
        }

        private void UpdateAssociateLabel(object sender, RenderOffsetEventArgs e)
        {
            this.AssociateRenderLabel.SetText($"Count: {e.Count}");
        }

        private async void AssociateTexturesButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                this.SetCheckboxState(false);
                var service = new AssociateTexturesDatabaseService();
                service.TextureMeshSaved += this.UpdateAssociateTexturesLabel;
                await service.AssociateTextures();
                service.TextureMeshSaved -= this.UpdateAssociateTexturesLabel;
            });
            this.databaseState.TexturesAssociated = true;
            this.AssociateTexturesLabel.SetText("Texture objects association completed");
            this.SetCheckboxState(true);
        }

        private void UpdateAssociateTexturesLabel(object sender, TextureMeshEventArgs e)
        {
            this.AssociateTexturesLabel.SetText($"Count: {e.Count}");
        }

        private async void CreateElvenChurchButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                this.SetCheckboxState(false);
                this.CreateChurchLabel.SetText("Creating structure");
                var service = new ElvenChurchService();
                await service.SaveAllAsync();
            });
            this.CreateChurchLabel.SetText("Finished saving");
            this.SetCheckboxState(true);
        }

        private async void RangerBlindButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                this.SetCheckboxState(false);
                this.RangerBlindLabel.SetText("Creating structure");
                var service = new RangerBlindService();
                await service.SaveAllAsync();
            });
            this.RangerBlindLabel.SetText("Finished saving");
            this.SetCheckboxState(true);
        }

        private async void LizardManTempleButton_Click(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                this.SetCheckboxState(false);
                this.LizardManTempleLabel.SetText("Creating structure");
                var service = new LizardManTempleService();
                await service.SaveAllAsync();
            });
            this.LizardManTempleLabel.SetText("Finished saving");
            this.SetCheckboxState(true);
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
                if (this.databaseState.RenderSaved)
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
                var service = new ClearDatabaseService();
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

        private void SetState(object sender, EventArgs e)
        {
            this.SaveMeshesButton.Enabled = this.databaseState.TexturesSaved || this.SaveMeshesCheckbox.Checked;
            this.RenderButton.Enabled = this.databaseState.MeshesSaved || this.RenderInfoCheckbox.Checked;
            this.SaveCacheButton.Enabled = this.databaseState.RenderSaved || this.CObjectsCheckbox.Checked;
            this.AssociateRenderButton.Enabled = this.databaseState.CObjectSaved || this.RenderCheckbox.Checked;
            this.AssociateTexturesButton.Enabled = this.databaseState.TexturesAssociated || this.TexturesCheckbox.Checked;
        }

        private void SaveMeshesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (SaveMeshesCheckbox.Checked && !SaveMeshesButton.Enabled)
            {
                SaveMeshesButton.Enabled = true;
            }
            else if (!SaveMeshesCheckbox.Checked && !databaseState.TexturesSaved)
            {
                SaveMeshesButton.Enabled = false;
            }
        }

        private void RenderInfoCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (RenderInfoCheckbox.Checked && !RenderButton.Enabled)
            {
                RenderButton.Enabled = true;
            }
            else if (!RenderInfoCheckbox.Checked && !databaseState.MeshesSaved)
            {
                RenderButton.Enabled = false;
            }
        }

        private void CObjectsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (CObjectsCheckbox.Checked && !SaveCacheButton.Enabled)
            {
                SaveCacheButton.Enabled = true;
            }
            else if (!CObjectsCheckbox.Checked && !databaseState.RenderSaved)
            {
                SaveCacheButton.Enabled = false;
            }
        }

        private void RenderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (RenderCheckbox.Checked && !AssociateRenderButton.Enabled)
            {
                AssociateRenderButton.Enabled = true;
            }
            else if (!RenderCheckbox.Checked && !databaseState.CObjectSaved)
            {
                AssociateRenderButton.Enabled = false;
            }
        }

        private void TexturesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (TexturesCheckbox.Checked && !AssociateTexturesButton.Enabled)
            {
                AssociateTexturesButton.Enabled = true;
            }
            else if (!TexturesCheckbox.Checked && !databaseState.RenderOffsetSaved)
            {
                AssociateTexturesButton.Enabled = false;
            }
        }

        private void SetCheckboxState(bool enabled)
        {
            if (enabled)
            {
                this.SaveTexturesButton.SetEnabled(true);
                this.AssociateRenderButton.SetEnabled(TexturesCheckbox.Checked || this.databaseState.RenderOffsetSaved);
                this.RenderButton.SetEnabled(RenderCheckbox.Checked || this.databaseState.MeshesSaved);
                this.SaveMeshesButton.SetEnabled(SaveMeshesCheckbox.Checked || this.databaseState.TexturesSaved);
                this.SaveCacheButton.SetEnabled(CObjectsCheckbox.Checked || this.databaseState.RenderSaved);
                this.AssociateRenderButton.SetEnabled(TexturesCheckbox.Checked || this.databaseState.CObjectSaved);
                this.AssociateTexturesButton.SetEnabled(TexturesCheckbox.Checked || this.databaseState.RenderOffsetSaved);

                this.LizardManTempleButton.SetEnabled(true);
                this.CreateElvenChurchButton.SetEnabled(true);
                this.RangerBlindButton.SetEnabled(true);
                this.ClearDataButton.SetEnabled(true);
            }
            else
            {
                this.AssociateRenderButton.SetEnabled(false);
                this.SaveTexturesButton.SetEnabled(false);
                this.RenderButton.SetEnabled(false);
                this.SaveMeshesButton.SetEnabled(false);
                this.SaveCacheButton.SetEnabled(false);
                this.AssociateRenderButton.SetEnabled(false);
                this.AssociateTexturesButton.SetEnabled(false);

                this.ClearDataButton.SetEnabled(false);
                this.LizardManTempleButton.SetEnabled(false);
                this.CreateElvenChurchButton.SetEnabled(false);
                this.RangerBlindButton.SetEnabled(false);
            }

            SaveMeshesCheckbox.SetEnabled(enabled);
            RenderInfoCheckbox.SetEnabled(enabled);
            CObjectsCheckbox.SetEnabled(enabled);
            RenderCheckbox.SetEnabled(enabled);
            TexturesCheckbox.SetEnabled(enabled);
        }
    }
}
