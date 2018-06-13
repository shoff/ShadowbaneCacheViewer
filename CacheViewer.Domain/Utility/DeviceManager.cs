namespace CacheViewer.Domain.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Forms;
    using SlimDX;
    using SlimDX.Direct3D11;
    using SlimDX.DXGI;
    using Device = SlimDX.Direct3D11.Device;
    using Resource = SlimDX.Direct3D11.Resource;

    /// <summary>
    /// </summary>
    public class DeviceManager
    {
        /// <summary>
        ///     The context
        /// </summary>
        public DeviceContext context;

        /// <summary>
        ///     The device
        /// </summary>
        public Device device;

        /// <summary>
        ///     The render target
        /// </summary>
        public RenderTargetView renderTarget;

        /// <summary>
        ///     The swap chain
        /// </summary>
        public SwapChain swapChain;

        /// <summary>
        ///     The viewport
        /// </summary>
        public Viewport viewport;

        /// <summary>
        ///     Prevents a default instance of the <see cref="DeviceManager" /> class from being created.
        /// </summary>
        private DeviceManager()
        {
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>
        ///     The instance.
        /// </value>
        public static DeviceManager Instance { get; } = new DeviceManager();


        /// <summary>
        ///     Gets the caps.
        /// </summary>
        public void GetCaps()
        {
            var fac = new Factory1();
            var numAdapters = fac.GetAdapterCount1();
            var adapts = new List<Adapter1>();
            for (var i = 0; i < numAdapters; i++)
            {
                adapts.Add(fac.GetAdapter1(i));
            }

            var outp = adapts[0].GetOutput(0);

            var formats = new List<Format>();
            foreach (Format format in Enum.GetValues(typeof(Format)))
            {
                formats.Add(format);
            }

            var ll = new List<ReadOnlyCollection<ModeDescription>>();

            for (var i = 0; i < formats.Count - 1; i++)
            {
                ReadOnlyCollection<ModeDescription> mdl;
                mdl = outp.GetDisplayModeList(formats[i], DisplayModeEnumerationFlags.Interlaced);
                ll.Add(mdl);
                if (mdl != null)
                {
                    Console.WriteLine(formats[i].ToString());
                }
            }
        }

        /// <summary>
        ///     Creates the device and swap chain.
        /// </summary>
        /// <param name="form">The form.</param>
        public void CreateDeviceAndSwapChain(Control form)
        {
            var description = new SwapChainDescription
            {
                BufferCount = 1,
                Usage = Usage.RenderTargetOutput,
                OutputHandle = form.Handle,
                IsWindowed = true,
                ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                SampleDescription = new SampleDescription(1, 0),
                Flags = SwapChainFlags.AllowModeSwitch,
                SwapEffect = SwapEffect.Discard
            };
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, description, out this.device,
                out this.swapChain);

            // create a view of our render target, which is the backbuffer of the swap chain we just created
            using (var resource = Resource.FromSwapChain<Texture2D>(this.swapChain, 0))
            {
                this.renderTarget = new RenderTargetView(this.device, resource);
            }

            // setting a viewport is required if you want to actually see anything
            this.context = this.device.ImmediateContext;
            this.viewport = new Viewport(0.0f, 0.0f, form.ClientSize.Width, form.ClientSize.Height);
            this.context.OutputMerger.SetTargets(this.renderTarget);
            this.context.Rasterizer.SetViewports(this.viewport);


            // prevent DXGI handling of alt+enter, which doesn't work properly with Winforms
            using (var factory = this.swapChain.GetParent<Factory>())
            {
                factory.SetWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAltEnter);
            }

            // handle alt+enter ourselves
            form.KeyDown += (o, e) =>
            {
                if (e.Alt && e.KeyCode == Keys.Enter)
                {
                    this.swapChain.IsFullScreen = !this.swapChain.IsFullScreen;
                }
            };
        }

        /// <summary>
        ///     Shuts down.
        /// </summary>
        public void ShutDown()
        {
            this.renderTarget.Dispose();
            this.swapChain.Dispose();
            this.device.Dispose();
        }
    }
}