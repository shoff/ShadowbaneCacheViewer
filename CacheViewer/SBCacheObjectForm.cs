
namespace CacheViewer
{
    using System.ComponentModel;
    using ControlExtensions;
    using Controls;
    using System.Windows.Forms;
    using Code;

    public partial class SBCacheObjectForm : Form
    {
        private InsideMover insideMover1;

        public SBCacheObjectForm()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.insideMover1 = new InsideMover(this.panelContainer1);
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
            this.panelContainer1.Controls.Clear();
            var cobjectView = new CObjectControl(e.CacheObject);
            this.insideMover1.ControlToMove = cobjectView;
            this.panelContainer1.Add(cobjectView);
            this.panelContainer1.Controls.Add(cobjectView);
            this.MessageLabel.SetText(message);
        }

        private void databaseToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            DatabaseForm dbf = new DatabaseForm();
            dbf.Show();
        }
    }
}
