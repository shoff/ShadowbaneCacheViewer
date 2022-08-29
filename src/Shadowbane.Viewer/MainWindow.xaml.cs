namespace Shadowbane.Viewer;

using System.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        //this.CacheObjectTreeControl.ValidObjectParsed += SetValidText;
        //this.CacheObjectTreeControl.InvalidObjectParsed += SetInvalidText;
    }

    private string SetInvalidText(string arg)
    {
        this.Dispatcher.Invoke(() => { this.ValidCacheObjects.Content = arg; });
        return arg;
    }

    private string SetValidText(string arg)
    {
        this.Dispatcher.Invoke(() => { this.InvalidCacheCount.Content = arg; });
        return arg;
    }
}
