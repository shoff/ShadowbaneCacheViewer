using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CacheViewer.Controls;
using CacheViewer.Domain.Archive;
using CacheViewer.Domain.Exporters;
using CacheViewer.Domain.Factories;
using CacheViewer.Domain.Models;
using SlimDX;
using Timer = System.Threading.Timer;

namespace CacheViewer
{
    public partial class MeshForm : Form
    {
        private readonly MeshOnlyObjExporter meshExporter;
        private readonly MeshFactory meshFactory;
        private readonly CacheObjectFactory cacheObjectFactory;
        private readonly RenderFactory renderFactory;
        private readonly Timer clearTimer;

        public MeshForm()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.clearTimer = new Timer(ClearMessages, null, 10000, 20000);
                this.meshFactory = MeshFactory.Instance;
                this.meshExporter = MeshOnlyObjExporter.Instance;
                this.cacheObjectFactory = CacheObjectFactory.Instance;
                this.renderFactory = RenderFactory.Instance;
                //this.slimRenderControl1.BorderStyle = BorderStyle.FixedSingle;
            }

            this.MessageLabel.Text = Messages.LoadingArchive;
        }


        private void ClearMessages(object state)
        {
            if (this.MessageLabel.InvokeRequired)
            {
                this.MessageLabel.BeginInvoke(new MethodInvoker(() => ClearMessages(state)));
            }
            else
            {
                this.MessageLabel.Text = String.Empty;
            }
        }

        public void Render()
        {
            //this.slimRenderControl1.RenderFrame();
        }


        private void LoadModelButton_Click(object sender, EventArgs e)
        {
            int selectedrowindex = this.CacheIndexesDataGrid.SelectedCells[0].RowIndex;
            int id = (int)this.CacheIndexesDataGrid.Rows[selectedrowindex].Cells[1].Value;
            CacheIndex ci = this.meshFactory.Indexes.First(x => x.identity == id);
            var model = this.meshFactory.Create(ci);
            //this.slimControl11.SetVerts(model);
        }

        private async void MeshFormLoad(object sender, System.EventArgs e)
        {
            int i = 0;

            foreach (var cacheIndex in this.meshFactory.Indexes)
            {
                int n = this.CacheIndexesDataGrid.Rows.Add();
                this.CacheIndexesDataGrid.Rows[n].Cells[0].Value = i;
                this.CacheIndexesDataGrid.Rows[n].Cells[1].Value = cacheIndex.identity;
                this.CacheIndexesDataGrid.Rows[n].Cells[2].Value = cacheIndex.offset;
                this.CacheIndexesDataGrid.Rows[n].Cells[3].Value = cacheIndex.unCompressedSize;
                this.CacheIndexesDataGrid.Rows[n].Cells[4].Value = cacheIndex.compressedSize;
                i++;

                if (i > 500)
                {
                    break;
                }
            }
            this.MessageLabel.Text = String.Empty;
            this.LoadModelButton.Enabled = true;
        }

        private async void SaveSelectedButton_Click(object sender, EventArgs e)
        {
            int selectedrowindex = this.CacheIndexesDataGrid.SelectedCells[0].RowIndex;
            int id = (int)this.CacheIndexesDataGrid.Rows[selectedrowindex].Cells[1].Value;
            await this.ExportAsync(id);
            this.MessageLabel.Text = "Mesh successully exported.";
        }


        private async void SaveAllButton_Click(object sender, EventArgs e)
        {
            foreach (var index in this.meshFactory.Indexes)
            {
                await ExportAsync(index.identity);
                this.MessageLabel.Text = "Mesh successully exported.";
            }
        }

        private async Task ExportAsync(int id)
        {
            CacheIndex ci = this.meshFactory.Indexes.First(x => x.identity == id);
            var model = await Task.Run(() => this.meshFactory.Create(ci));
            await this.meshExporter.ExportAsync(model);
        }

        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedrowindex = this.CacheIndexesDataGrid.SelectedCells[0].RowIndex;
            int id = (int)this.CacheIndexesDataGrid.Rows[selectedrowindex].Cells[1].Value;
            await this.ExportAsync(id);
            this.MessageLabel.Text = "Mesh successully exported.";
        }

        private async void CacheIndexesDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            // get the row
            int selectedrowindex = this.CacheIndexesDataGrid.SelectedCells[0].RowIndex;

            if (selectedrowindex == 0)
            {
                // hack
                return;
            }

            // get the id
            int id = (int)this.CacheIndexesDataGrid.Rows[selectedrowindex].Cells[1].Value;

            // get the model index
            CacheIndex ci = this.meshFactory.Indexes.First(x => x.identity == id);

            // get the model
            Mesh mesh = await Task.Run(() => this.meshFactory.Create(ci));

            //List<Vertex> verts = new List<Vertex>();

            //// To convert from right hand to left hand, we must do a couple things. 
            //// First is invert the z-axis of the vertice's positions by multiplying it with -1.0f.
            //// We will also need to invert the v-axis of the texture coordinats by subtracting it from 1.0f.
            //// Finally we will need to convert the z-axis of the vertex normals by multiplying it by -1.0f. 
            //for (int i = 0; i < mesh.VertexCount; i++)
            //{
            //    //Vertex vertex = new Vertex()
            //    //{
            //    //    Position = new Vector4(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z * -1.0f, 0.05f),
            //    //    Color = new Color4(1.0f, 0.0f, 1.0f, 0.0f),
            //    //    TexCoord = new Vector2(mesh.TextureVectors[i].X, 1.0f - mesh.TextureVectors[i].Y)
            //    //};


            //    Vertex vertex = new Vertex()
            //    {
            //        Position = new Vector4(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z, 0.05f),
            //        Color = new Color4(1.0f, 0.0f, 1.0f, 0.0f),
            //        TexCoord = new Vector2(mesh.TextureVectors[i].X, mesh.TextureVectors[i].Y)
            //    };
            //    verts.Add(vertex);
            //}

            ////verts.Reverse();
            //this.slimRenderControl1.Verts = verts.ToArray();
            //this.MessageLabel.Text = "";
        }
    }
}
