namespace Shadowbane.Viewer;

using System;
using System.IO;
using Shadowbane.Cache.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Models;
using SixLabors.ImageSharp;
using ViewModels;
using Point = System.Windows.Point;

public partial class MainWindow
{
    private CacheObjectViewModelLoader loader;
    private CacheObjectViewModel viewModel;
    private RenderableBuilder renderableBuilder;
    private MeshGeometry3D meshGeometry3D;
    private DiffuseMaterial material;
    private PerspectiveCamera camera;
    private Model3DGroup model3dGroup;
    private GeometryModel3D geometryModel3D;
    private ModelVisual3D modelVisual3d;

    public MainWindow()
    {
        InitializeComponent();
        InitializeResources();
    }

    private void InitializeResources()
    {
        this.Error.Content = "";
        this.viewModel = new CacheObjectViewModel();
        this.loader = new CacheObjectViewModelLoader(this.viewModel);
        this.CacheObjectTreeView.ItemsSource = this.viewModel.Items;
        this.renderableBuilder = new RenderableBuilder();
        this.model3dGroup = new Model3DGroup();
        this.geometryModel3D = new GeometryModel3D();
        this.modelVisual3d = new ModelVisual3D();

        this.camera = new PerspectiveCamera
        {
            // Specify where in the 3D scene the camera is.
            Position = new Point3D(-40, 40, 40),
            // Specify the direction that the camera is pointing.
            // LookDirection = new Vector3D(40, -40, 40),
            LookDirection = new Vector3D(40, -40, -40),
            // Define camera's horizontal field of view in degrees.
            UpDirection = new Vector3D(1, 0, 0),
            FieldOfView = 60
        };
        this.CacheItemViewPort.Camera = this.camera;

        // lighting
        DirectionalLight directionalLight = new DirectionalLight
        {
            Color = Colors.White,
            // Direction = new Vector3D(-0.61, -0.5, -0.61)
            Direction = new Vector3D(-1,-1,-3)
        };
        this.model3dGroup.Children.Add(directionalLight);
        this.meshGeometry3D = new MeshGeometry3D
        {
            Normals = new Vector3DCollection
            {
                new(0, 0, 1),
                new(0, 0, 1),
                new(0,0,1),
                new(0,0,1),
                new(0,0,1),
                new(0,0,1)
            },
            Positions =  new Point3DCollection
            {
                new(-0.5, -0.5, 0.5),
                new(0.5, -0.5, 0.5),
                new(0.5, 0.5, 0.5),
                new(0.5, 0.5, 0.5),
                new(-0.5, 0.5, 0.5),
                new(-0.5, -0.5, 0.5)
            },
            TextureCoordinates =  new PointCollection
            {
                new(0, 0),
                new(1, 0),
                new(1, 1),
                new(1, 1),
                new(0, 1),
                new(0, 0)
            },
            TriangleIndices = new Int32Collection
            {
                0,
                1,
                2,
                3,
                4,
                5
            }
        };

        LinearGradientBrush myHorizontalGradient = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0.5),
            EndPoint = new Point(1, 0.5)
        };

        myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
        myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
        myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Blue, 0.75));
        myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.LimeGreen, 1.0));
        this.geometryModel3D.Geometry = this.meshGeometry3D;
        this.geometryModel3D.Material = new DiffuseMaterial(myHorizontalGradient);
        RotateTransform3D myRotateTransform3D = new RotateTransform3D();
        AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D
        {
            Axis = new Vector3D(0,3,0),
            Angle = 40
        };
        myRotateTransform3D.Rotation = myAxisAngleRotation3d;
        this.geometryModel3D.Transform = myRotateTransform3D;
        this.model3dGroup.Children.Add(this.geometryModel3D);
        this.modelVisual3d.Content = this.model3dGroup;
        this.CacheItemViewPort.Children.Add(this.modelVisual3d);
    }

    private async void Load_Cache_Click(object sender, RoutedEventArgs e)
    {
        await this.loader.GetObjectsAsync(CancellationToken.None);
    }

    private void TreeViewItem_OnItemSelected(object sender, RoutedEventArgs e)
    {
        this.Error.Content = "";
        //ModelVisual3D modelVisual3D =
        //    (ModelVisual3D)this.CacheItemViewPort.Children[0];

        //this.geometryModel3D = (GeometryModel3D)modelVisual3D.Content;
        //this.meshGeometry3D = (MeshGeometry3D)this.geometryModel3D.Geometry;
        //this.material = (DiffuseMaterial)this.geometryModel3D.Material;

        CacheObjectNameOnly? selectedObject =
            (CacheObjectNameOnly)((TreeViewItem)((TreeView)sender).SelectedItem).Tag;

        try
        {
            var renderId = selectedObject.RenderId;

            var renderable = this.renderableBuilder.Build(renderId);

            if (renderable?.HasMesh != null && renderable.HasMesh)
            {
                this.meshGeometry3D.Positions.Clear();
                foreach (var vertex in renderable.Mesh.Vertices)
                {
                    this.meshGeometry3D.Positions.Add(new Point3D(vertex.X, vertex.Y, vertex.Z));
                }

                // meshGeometry3D.Positions = positions;
                this.meshGeometry3D.TriangleIndices.Clear();
                foreach (var index in renderable.Mesh.Indices)
                {
                    this.meshGeometry3D.TriangleIndices.Add(index.Position);
                    this.meshGeometry3D.TriangleIndices.Add(index.Normal);
                    this.meshGeometry3D.TriangleIndices.Add(index.TextureCoordinate);
                }

                this.meshGeometry3D.TextureCoordinates.Clear();
                if (renderable.HasTexture)
                {
                    // pretty sure there's only one texture per mesh currently
                    var texture = renderable.Mesh.Textures[0];
                    var path = $"{Texture.ImageSavePath}\\{texture.TextureId}.png";
                    if (!File.Exists(path))
                    {
                        texture.Image.SaveAsPng(path);
                    }

                    //this.material.Brush = new ImageBrush { ImageSource = new BitmapImage(new Uri($"file://{path}")) };
                    //ImageBrush brush = (ImageBrush)this.material.Brush;
                    //brush.ImageSource = new System.Windows.Image();
                }
            }
        }
        catch (Exception ex)
        {
            this.Error.Content = ex.Message;
        }
    }

    static ImageSource PngStreamToImageSource(Stream pngStream)
    {
        var decoder = new PngBitmapDecoder(pngStream,
            BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
        return decoder.Frames[0];
    }


    private void CacheItemViewPort_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.meshGeometry3D == null || this.geometryModel3D == null)
        {
            // can't really translate or rotate a null model :)
            return;
        }

    }

    private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
    {

    }
}