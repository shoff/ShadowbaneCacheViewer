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

public partial class TestWindow
{
    private CacheObjectViewModelLoader loader;
    private CacheObjectViewModel viewModel;
    private RenderableBuilder renderableBuilder;
    private PerspectiveCamera camera;
    private DirectionalLight directionalLight;
    private AmbientLight ambientLight;
    public TestWindow()
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

        this.camera = new PerspectiveCamera
        {
            // Specify where in the 3D scene the camera is.
            Position = new Point3D(-40, 40, 40),
            // Specify the direction that the camera is pointing.
            // LookDirection = new Vector3D(40, -40, 40),
            LookDirection = new Vector3D(40, -40, -40),
            NearPlaneDistance = 0.001,
            FarPlaneDistance = 5000,
            // Define camera's horizontal field of view in degrees.
            UpDirection = new Vector3D(1, 0, 0),
            FieldOfView = 80
        };
        this.CacheItemViewPort.Camera = this.camera;

        // lighting
        this.ambientLight = new AmbientLight
        {
            Color = Colors.WhiteSmoke
        };

        this.directionalLight = new DirectionalLight
        {
            Color = Colors.White,
            // Direction = new Vector3D(-0.61, -0.5, -0.61)
            Direction = new Vector3D(-1, -1, -3)
        };
        SetupModelGroup(this.DefaultGeometry());
    }

    private void SetupModelGroup(GeometryModel3D geometryModel3D)
    {
        var model3dGroup = new Model3DGroup();
        var modelVisual3d = new ModelVisual3D();

        model3dGroup.Children.Add(this.directionalLight);
        model3dGroup.Children.Add(this.ambientLight);
        model3dGroup.Children.Add(geometryModel3D);
        modelVisual3d.Content = model3dGroup;
        this.CacheItemViewPort.Children.Add(modelVisual3d);
    }

    private async void Load_Cache_Click(object sender, RoutedEventArgs e)
    {
        await this.loader.GetObjectsAsync(CancellationToken.None);
    }

    private void TreeViewItem_OnItemSelected(object sender, RoutedEventArgs e)
    {
        this.Error.Content = "";
        CacheObjectNameOnly? selectedObject =
            (CacheObjectNameOnly)((TreeViewItem)((TreeView)sender).SelectedItem).Tag;

        this.CacheItemViewPort.Children.Clear();

        try
        {
            // TODO here add each render id in renderable
            var renderId = selectedObject.RenderId;
            var renderable = this.renderableBuilder.Build(renderId);
            SetupModelGroup(AddRenderableToModelGroup(renderable));
        }
        catch (Exception ex)
        {
            this.Error.Content = ex.Message;
        }
    }

    private GeometryModel3D AddRenderableToModelGroup(Renderable? renderable)
    {
        if (renderable?.HasMesh == false || renderable?.Mesh?.Vertices == null)
        {
            return DefaultGeometry();
        }
        // create geom
        var geometry = new MeshGeometry3D();

        // verts
        var positions = new Point3DCollection();
        foreach (var vertex in renderable.Mesh.Vertices)
        {
            positions.Add(new Point3D(vertex.X, vertex.Y, -vertex.Z));
        }
        geometry.Positions = positions;

        // indices
        foreach (var index in renderable.Mesh.Indices)
        {
            geometry.TriangleIndices.Add(index.Position);
            geometry.TriangleIndices.Add(index.Normal);
            geometry.TriangleIndices.Add(index.TextureCoordinate);
        }

        // normals
        var normals = new Vector3DCollection();
        foreach (var normal in renderable.Mesh.Normals)
        {
            normals.Add(new Vector3D(normal.X, normal.Y, normal.Z));
        }
        geometry.Normals = normals;

        var material = new DiffuseMaterial();

        if (renderable.HasTexture)
        {
            // pretty sure there's only one texture per mesh currently
            var texture = renderable.Mesh.Textures[0];
            var path = $"{Texture.ImageSavePath}\\{texture.TextureId}.png";
            if (!File.Exists(path))
            {
                texture.Image.SaveAsPng(path);
            }

            var pointCollection = new PointCollection();
            foreach (var textureIndex in renderable.Mesh.TextureVectors)
            {
                pointCollection.Add(new Point(textureIndex.X, -textureIndex.Y));
            }

            geometry.TextureCoordinates = pointCollection;
            material.Brush = new ImageBrush(new BitmapImage(new Uri($"file://{path}")));
            //var brush= new Brush()
            //ImageSource imageSource = new BitmapImage(new Uri($"file://{path}"));
            //var material = new DiffuseMaterial(

            //this.material.Brush = new ImageBrush { ImageSource = new BitmapImage(new Uri($"file://{path}")) };
            //ImageBrush brush = (ImageBrush)this.material.Brush;
            //brush.ImageSource = new System.Windows.Image();
        }
        else
        {
            material.Brush = DefaultBrush();
        }

        var geometryModel3D = new GeometryModel3D
        {
            Geometry = geometry,
            // material 
            Material = material,
            Transform = new ScaleTransform3D
            {
                ScaleX = -1,
                ScaleY = 1,
                ScaleZ = 1
            }
            // transform
            //Transform = new RotateTransform3D
            //{
            //    Rotation = new AxisAngleRotation3D
            //    {
            //        Axis = new Vector3D(0, 3, 0),
            //        Angle = 40
            //    }
            //}
        };
        return geometryModel3D;

    }

    private LinearGradientBrush DefaultBrush()
    {
        LinearGradientBrush gradientBrush = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0.5),
            EndPoint = new Point(1, 0.5)
        };

        gradientBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
        gradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
        gradientBrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.75));
        gradientBrush.GradientStops.Add(new GradientStop(Colors.SkyBlue, 1.0));
        return gradientBrush;
    }

    private GeometryModel3D DefaultGeometry()
    {
        var geometryModel3D = new GeometryModel3D
        {
            Geometry = new MeshGeometry3D
            {
                // normals
                Normals = new Vector3DCollection
                {
                    new(0, 0, 1),
                    new(0, 0, 1),
                    new(0,0,1),
                    new(0,0,1),
                    new(0,0,1),
                    new(0,0,1)
                },
                // verts
                Positions = new Point3DCollection
                {
                    new(-0.5, -0.5, 0.5),
                    new(0.5, -0.5, 0.5),
                    new(0.5, 0.5, 0.5),
                    new(0.5, 0.5, 0.5),
                    new(-0.5, 0.5, 0.5),
                    new(-0.5, -0.5, 0.5)
                },
                // texture
                TextureCoordinates = new PointCollection { new(0, 0), new(1, 0), new(1, 1), new(1, 1), new(0, 1), new(0, 0) },
                // indices
                TriangleIndices = new Int32Collection { 0, 1, 2, 3, 4, 5 }
            },
            // material 
            Material = new DiffuseMaterial(DefaultBrush()),

            // transform
            Transform = new RotateTransform3D
            {
                Rotation = new AxisAngleRotation3D
                {
                    Axis = new Vector3D(0, 3, 0),
                    Angle = 40
                }
            }
        };

        return geometryModel3D;
    }

    private void CacheItemViewPort_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {

    }

    private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
    {
    }
}