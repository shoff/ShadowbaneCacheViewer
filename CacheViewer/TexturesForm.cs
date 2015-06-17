using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using CacheViewer.Domain.Factories;

namespace CacheViewer
{
    public partial class TexturesForm : Form
    {
        private int nullCount;
        private const string ExportDirectory = @"D:\dev\CacheViewer_2014\Exports\Textures\";
        private readonly TextureFactory textureFactory;
        public TexturesForm()
        {
            InitializeComponent();
            this.textureFactory = TextureFactory.Instance;
        }

        private async void ExportButton_Click(object sender, System.EventArgs e)
        {
            await Export();
        }

        private async Task Export()
        {
            await Task.Run(() =>
            {
                foreach (var index in this.textureFactory.Indexes)
                {
                    var bitmap = this.textureFactory.TextureMap(index.identity);
                    if (bitmap != null)
                    {
                        bitmap.Save(Path.Combine(ExportDirectory, index.identity.ToString(CultureInfo.InvariantCulture) + ".png"),
                            ImageFormat.Png);

                        this.SetMessage(MessageLabel,
                            string.Format("Saved {0}", index.identity.ToString(CultureInfo.InvariantCulture) + ".png"));
                    }
                    else
                    {
                        nullCount++;
                        this.SetMessage(this.NullCountLabel, string.Format("Null count: {0}", nullCount));
                    }
                }
            });
        }

        private void SetMessage(Control control, string message)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => SetMessage(control, message)));
            }
            else
            {
                control.Text = message;
                control.Refresh();
            }
        }
    }
}
