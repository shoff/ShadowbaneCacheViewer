namespace CacheViewer.Domain.Utility
{
    using System.Threading;
    using SlimDX;
    using SlimDX.DXGI;

    /// <summary>
    /// </summary>
    public class RenderManager
    {
        /// <summary>
        ///     The instance
        /// </summary>
        private static readonly RenderManager instance = new RenderManager();

        /// <summary>
        ///     The render thread
        /// </summary>
        private Thread renderThread;

        /// <summary>
        ///     Prevents a default instance of the <see cref="RenderManager" /> class from being created.
        /// </summary>
        private RenderManager()
        {
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>
        ///     The instance.
        /// </value>
        public static RenderManager Instance => instance;

        /// <summary>
        ///     Renders the scene.
        /// </summary>
        public void RenderScene()
        {
            while (true)
            {
                var dm = DeviceManager.Instance;
                dm.context.ClearRenderTargetView(dm.renderTarget, new Color4(0.25f, 0.75f, 0.25f));

                Scene.Instance.Render();

                dm.swapChain.Present(0, PresentFlags.None);
            }
        }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public void Init()
        {
            this.renderThread = new Thread(this.RenderScene);
            this.renderThread.Start();
        }

        /// <summary>
        ///     Shuts down.
        /// </summary>
        public void ShutDown()
        {
            this.renderThread.Abort();
        }
    }
}