using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.Windows;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;

namespace CacheViewer.Controls.Viewer
{
    public class ViewerBase
    {
        private readonly DisplayMesh displayMesh;
        //private readonly Scene altairScene;
        //private readonly Scene autoScene;
        //private readonly Scene teamFortress2Scene;
        private readonly long frequency;
        //private readonly DisplayMesh cubeModel;
        //private readonly DisplayMesh modEsfera;
        private readonly Point windowSize = new Point(1280, 720);
        private CameraComponent camara;
        private DeviceContext deviceContext;
        private RenderTargetView renderTargetBackBuffer;
        private DepthStencilView renderTargetDepthStencil;
        private SwapChain swapChain;
        private long tickAnterior;

        public ViewerBase()
        {
        }

        public ViewerBase(DisplayMesh displayMesh)
        {
            this.displayMesh = displayMesh;

            // TODO remove this
            this.ResourcesDir = Environment.CurrentDirectory + "\\Resources\\";

            // TODO change this
            this.RenderForm = new RenderForm("Framework")
            {
                ClientSize = new Size(this.windowSize)
            };

            QueryPerformanceFrequency(out this.frequency);

            this.CrearDeviceSwapChainContext();

            this.CrearBackBufferRenderTargets();

            this.SetearContextStates();

            Vector3 cameraPosition = new Vector3(0f, 0f, 50f);

            this.InitializeCamera(cameraPosition, cameraPosition - Vector3.UnitZ, 10f, 15f, 0.1f,
                Camera.Behavior.Spectator, 1f, 1000f);

            //// TODO this needs to be renmoved and opened based upon the selected object.
            //this.cubeModel = new DisplayMesh(this.ResourcesDir + "fxVarios.fx", "ShowNormals", this.ResourcesDir + "box.smd",
            //    this.ResourcesDir + "cubo.jpg", this.ViewDevice);

            //this.modEsfera = new DisplayMesh(this.ResourcesDir + "fxVarios.fx", "ShowNormals", this.ResourcesDir + "esfera.smd",
            //    this.ResourcesDir + "jupiter.jpg", this.ViewDevice);

            //this.teamFortress2Scene = new Scene(this.ResourcesDir + "fxVarios.fx", "ShowNormals",
            //    this.ResourcesDir + "Mapas\\gullywash", "gullywash.obj", ObjLoader.NormalLoad.Load);

            //this.autoScene = new Scene(this.ResourcesDir + "fxVarios.fx", "ShowNormals", this.ResourcesDir + "Auto",
            //    "rapide.obj", ObjLoader.NormalLoad.Load);

            //this.altairScene = new Scene(this.ResourcesDir + "fxVarios.fx", "ShowNormals", this.ResourcesDir + "Altair",
            //    "altair.obj", ObjLoader.NormalLoad.GenerateVertex);

            this.OverrideEvents();

            QueryPerformanceCounter(out this.tickAnterior);
        }

        /// <summary>
        ///     Gets or sets the render form.
        /// </summary>
        /// <value>
        ///     The render form.
        /// </value>
        public RenderForm RenderForm { get; set; }

        /// <summary>
        ///     Gets the resources dir.
        /// </summary>
        /// <value>
        ///     The resources dir.
        /// </value>
        public string ResourcesDir { get; private set; }

        /// <summary>
        ///     Gets the view device.
        /// </summary>
        /// <value>
        ///     The view device.
        /// </value>
        public Device ViewDevice { get; private set; }

        /// <summary>
        ///     Singleton instance,
        /// </summary>
        /// <value>
        ///     The instance.
        /// </value>
        public static ViewerBase Instance
        {
            get { return null; }
        }

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lFrequency);

        private void OverrideEvents()
        {
            // prevent DXGI handling of alt+enter, which doesn't work properly with Winforms
            using (var factory = this.swapChain.GetParent<Factory>())
            {
                factory.SetWindowAssociation(this.RenderForm.Handle, WindowAssociationFlags.IgnoreAltEnter);
            }


            // handle alt+enter ourselves
            this.RenderForm.KeyDown += (o, e) =>
            {
                if (e.Alt && e.KeyCode == Keys.Enter)
                {
                    this.swapChain.IsFullScreen = !this.swapChain.IsFullScreen;
                }
            };
        }

        public void Update(float elapsed)
        {
            this.camara.Update(elapsed);
        }

        public void Render(float elapsed)
        {
            long verticesDrawed = 0;
            // clear the render target to a soothing blue
            this.deviceContext.ClearRenderTargetView(this.renderTargetBackBuffer, new Color4(0.5f, 0.5f, 1.0f));
            this.deviceContext.ClearDepthStencilView(this.renderTargetDepthStencil, DepthStencilClearFlags.Depth, 1, 0);

            verticesDrawed +=
                this.displayMesh.Draw(
                    Matrix.Scaling(5f, 5f, 5f)*Matrix.RotationX((float) Math.PI/2)*Matrix.Translation(-10f, 10f, 0f),
                    this.camara.ViewProjectionMatrix);
            //verticesDrawed +=
            //    this.modEsfera.Draw(
            //        Matrix.Scaling(0.1f, 0.1f, 0.1f) * Matrix.Translation(new Vector3(20f, 0f, 0f)) *
            //        Matrix.Translation(0f, 10f, 0f), this.camara.ViewProjectionMatrix);

            //verticesDrawed +=
            //    this.teamFortress2Scene.Draw(
            //        Matrix.Scaling(0.1f, 0.1f, 0.1f) * Matrix.RotationX((float)-Math.PI / 2) *
            //        Matrix.Translation(0f, -20f, -20f), this.camara.ViewProjectionMatrix);

            //verticesDrawed += this.autoScene.Draw(Matrix.Scaling(5f, 5f, 5f), this.camara.ViewProjectionMatrix);

            //verticesDrawed += this.altairScene.Draw(Matrix.Scaling(0.1f, 0.1f, 0.1f) * Matrix.Translation(0f, 15f, 0f),
            //    this.camara.ViewProjectionMatrix);


            this.swapChain.Present(0, PresentFlags.None);
        }

        public float ElapsedTime()
        {
            long tickActual;
            QueryPerformanceCounter(out tickActual);
            float elapsed = (float) (tickActual - this.tickAnterior)/this.frequency;
            this.tickAnterior = tickActual;
            return elapsed;
        }

        public void Dispose()
        {
            this.renderTargetBackBuffer.Dispose();
            this.renderTargetDepthStencil.Dispose();
            this.displayMesh.Dispose();

            //this.modEsfera.Dispose();
            //this.teamFortress2Scene.Dispose();
            //this.autoScene.Dispose();
            //this.altairScene.Dispose();

            this.swapChain.Dispose();
            this.ViewDevice.Dispose();
        }

        private void CrearDeviceSwapChainContext()
        {
            var description = new SwapChainDescription
            {
                BufferCount = 1,
                Usage = Usage.RenderTargetOutput,
                OutputHandle = this.RenderForm.Handle,
                IsWindowed = true,
                ModeDescription =
                    new ModeDescription(this.RenderForm.ClientSize.Width, this.RenderForm.ClientSize.Height,
                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                SampleDescription = new SampleDescription(1, 0),
                Flags = SwapChainFlags.AllowModeSwitch,
                SwapEffect = SwapEffect.Discard
            };
            var dev = this.ViewDevice;
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.Debug, description, out dev,
                out this.swapChain);
            this.ViewDevice = dev;
            this.deviceContext = this.ViewDevice.ImmediateContext;
        }

        private void CrearBackBufferRenderTargets()
        {
            // create a view of our render target, which is the backbuffer of the swap chain we just created
            using (var resource = Resource.FromSwapChain<Texture2D>(this.swapChain, 0))
            {
                this.renderTargetBackBuffer = new RenderTargetView(this.ViewDevice, resource);
            }


            Format depthFormat = Format.D32_Float;
            Texture2DDescription depthBufferDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = depthFormat,
                Height = this.RenderForm.ClientSize.Height,
                Width = this.RenderForm.ClientSize.Width,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            };

            var DepthBuffer = new Texture2D(this.ViewDevice, depthBufferDesc);

            DepthStencilViewDescription dsViewDesc = new DepthStencilViewDescription
            {
                ArraySize = 0,
                Format = depthFormat,
                Dimension = DepthStencilViewDimension.Texture2D,
                MipSlice = 0,
                Flags = 0,
                FirstArraySlice = 0
            };

            this.renderTargetDepthStencil = new DepthStencilView(this.ViewDevice, DepthBuffer, dsViewDesc);
            this.deviceContext.OutputMerger.SetTargets(this.renderTargetDepthStencil, this.renderTargetBackBuffer);
        }

        private void SetearContextStates()
        {
            this.deviceContext.OutputMerger.DepthStencilState = States.Instance.depthEnabledStencilDisabled;
            this.deviceContext.Rasterizer.State = States.Instance.cullBackFillSolid;
            this.deviceContext.OutputMerger.BlendState = States.Instance.blendDisabled;
            // setting a viewport is required if you want to actually see anything
            this.deviceContext.Rasterizer.SetViewports(new Viewport(0.0f, 0.0f, this.RenderForm.ClientSize.Width,
                this.RenderForm.ClientSize.Height));
        }

        private void InitializeCamera(Vector3 inPosition, Vector3 inTarget, float inAcceleration, float inVelocity,
            float inRotationSpeed, Camera.Behavior inBehavior, float inNear, float inFar)
        {
            this.camara = new CameraComponent(this.RenderForm);
            this.camara.Perspective(90.0f, (float) this.RenderForm.ClientSize.Width/this.RenderForm.ClientSize.Height,
                inNear, inFar);
            this.camara.ClickAndDragMouseRotation = true;
            this.camara.Position = inPosition;
            this.camara.LookAt(inTarget);
            this.camara.Acceleration = new Vector3(inAcceleration, inAcceleration, inAcceleration);
            this.camara.Velocity = new Vector3(inVelocity, inVelocity, inVelocity);
            this.camara.RotationSpeed = inRotationSpeed;
            this.camara.CurrentBehavior = inBehavior;
        }
    }
}