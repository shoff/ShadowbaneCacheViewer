namespace Shadowbane.Viewer;

using Shadowbane.Cache.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using ViewModels;

public partial class MainWindow : Window
{
    private readonly CacheObjectViewModelLoader loader;
    private readonly CacheObjectViewModel viewModel;
    private readonly RenderableBuilder renderableBuilder;

    public MainWindow()
    {
        InitializeComponent();
        this.viewModel = new CacheObjectViewModel();
        this.loader = new CacheObjectViewModelLoader(this.viewModel);
        this.CacheObjectTreeView.ItemsSource = this.viewModel.Items;
        this.renderableBuilder = new RenderableBuilder();
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

    private async void Load_Cache_Click(object sender, RoutedEventArgs e)
    {
        await this.loader.GetObjectsAsync(CancellationToken.None);
    }

    private void TreeViewItem_OnItemSelected(object sender, RoutedEventArgs e)
    {
        CacheObjectNameOnly? selectedObject =
            (CacheObjectNameOnly)((TreeViewItem)((TreeView)sender).SelectedItem).Tag;

        var renderId = selectedObject.RenderId;
        var renderable = this.renderableBuilder.Build(renderId);
        
        if (renderable?.HasMesh != null && renderable.HasMesh)
        {
            ModelVisual3D modelVisual3D =
                (ModelVisual3D)this.CacheItemViewPort.Children[0];
            GeometryModel3D content = (GeometryModel3D)modelVisual3D.Content;
            MeshGeometry3D meshGeometry = (MeshGeometry3D)content.Geometry;

            var positions = new Point3DCollection((int)renderable.Mesh!.VertexCount);
            foreach (var vertex in renderable.Mesh.Vertices)
            {
                positions.Add(new Point3D(vertex.X, vertex.Y, vertex.Z));
            }
            meshGeometry.Positions = positions;

            meshGeometry.TriangleIndices.Clear();
            foreach (var index in renderable.Mesh.Indices)
            {
                meshGeometry.TriangleIndices.Add(index.Position);
                meshGeometry.TriangleIndices.Add(index.Normal);
                meshGeometry.TriangleIndices.Add(index.TextureCoordinate);
            }
        }
    }
}