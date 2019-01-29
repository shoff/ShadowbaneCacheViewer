
namespace CacheViewer
{
    using ControlExtensions;
    using Controls;
    using System.Windows.Forms;

    public partial class SBCacheObjectForm : Form
    {
        public SBCacheObjectForm()
        {
            InitializeComponent();
            this.sbTreeControl1.OnCacheObjectSelected += CacheObjectSelected;
            this.sbTreeControl1.OnLoadingMessage += LoadingMessageReceived;
            this.sbTreeControl1.OnInvalidRenderId += InvalidRenderIdRead;
            this.sbTreeControl1.OnParseError += ParseErrorReceived;
            this.MessageLabel.SetText("");
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
            this.MessageLabel.SetText(message);
        }
    }
}
