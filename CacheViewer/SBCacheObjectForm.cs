
namespace CacheViewer
{
    using System.ComponentModel;
    using ControlExtensions;
    using Controls;
    using System.Windows.Forms;
    using Code;
    using Domain.Models.Exportable;
    using OpenTK;
    using OpenTK.Graphics.ES10;

    public partial class SBCacheObjectForm : Form
    {
        private ICacheObject selectedObject;

        public SBCacheObjectForm()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.sbTreeControl1.OnCacheObjectSelected += CacheObjectSelected;
                this.sbTreeControl1.OnLoadingMessage += LoadingMessageReceived;
                this.sbTreeControl1.OnInvalidRenderId += InvalidRenderIdRead;
                this.sbTreeControl1.OnParseError += ParseErrorReceived;
                this.MessageLabel.SetText("");
            }
        }

        private void ParseErrorReceived(object sender, ParseErrorEventArgs e)
        {
            var message = $"Parse error created trying to parse {e.ParseError.Name} ";
            this.MessageLabel.SetText(message);
        }

        private void InvalidRenderIdRead(object sender, InvalidRenderIdEventArgs e)
        {
            var message = $"Unable to find valid render id where expected from cache object {e.CacheObject.Name} ";
            this.MessageLabel.SetText(message);
        }

        private void LoadingMessageReceived(object sender, LoadingMessageEventArgs e)
        {
            this.MessageLabel.SetText(e.Message);
        }

        private void CacheObjectSelected(object sender, CacheObjectSelectedEventArgs e)
        {
            var message = $"Cache object {e.CacheObject.Name} selected";
            this.selectedObject = e.CacheObject;
            this.MessageLabel.SetText(message);

            GL.Enable(All.Texture2D);
            GL.Enable(All.ColorMaterial);
            GL.ShadeModel(All.Smooth);
            GL.Hint(All.PerspectiveCorrectionHint, All.Nicest);
            GL.DepthFunc(All.Lequal);
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            GL.MatrixMode(All.Projection);
            GL.LoadIdentity();

            GL.Enable(All.Lighting);
            GL.Enable(All.Light0);
            GL.Enable(All.Normalize);

            var ambience = new []{0.3f, 0.3f, 0.3f, 1.0f};
            var diffuse = new[] {8.0f, 8.0f, 8.0f, 8.0f};
            var specular = new[] { 0.0f, 0.0f, 0.0f, 1.0f};
            var pos = new[] { 0.0f, 0.15f, 1.0f, 1.0f}; // light from behind


            GL.Light(All.Light0, All.Diffuse, diffuse);
            GL.Light(All.Light0, All.Ambient, ambience);
            GL.Light(All.Light0, All.Specular, specular);
            GL.Light(All.Light0, All.Position, pos);

            GL.Light(All.Light0, All.ConstantAttenuation, 0.1f);
            GL.Light(All.Light0, All.LinearAttenuation, 0.0f);
            GL.Light(All.Light0, All.QuadraticAttenuation, 0.0f);
            GL.Light(All.Light0, All.SpotCutoff, 180.0f);	// Makes the lighting directional

            GL.MatrixMode(All.Modelview);
            GL.LoadIdentity();
            GL.Ortho(0, 512, 0, 512, (float)-1.0, (float)1.0);
            GL.Disable(All.DepthTest);

            foreach (var render in this.selectedObject.Renders)
            {
                if (render.Mesh.Textures.Count > 0)
                {
                    GL.BindTexture(All.Texture2D, render.Mesh.Textures[0].TextureId);
                }
            }
        }

        private void databaseToolStripMenuItem1_Click(object sender, System.EventArgs e)
        {
            DatabaseForm dbf = new DatabaseForm();
            dbf.Show();
        }

        private void databaseToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

        }
    }
}
