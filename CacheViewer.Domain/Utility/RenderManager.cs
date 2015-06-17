using System.Threading;
using SlimDX;
using SlimDX.DXGI;

namespace CacheViewer.Domain.Utility
{
    /// <summary>
    /// </summary>
    public class RenderManager
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static readonly RenderManager instance = new RenderManager();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static RenderManager Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="RenderManager"/> class from being created.
        /// </summary>
        private RenderManager() { }

        /// <summary>
        /// The render thread
        /// </summary>
        Thread renderThread;

        /// <summary>
        /// Renders the scene.
        /// </summary>
        public void RenderScene()
        {
            while (true)
            {
                DeviceManager dm = DeviceManager.Instance;
                dm.context.ClearRenderTargetView(dm.renderTarget, new Color4(0.25f, 0.75f, 0.25f));

                Scene.Instance.Render();

                dm.swapChain.Present(0, PresentFlags.None);
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init()
        {
            renderThread = new Thread(this.RenderScene);
            renderThread.Start();
        }

        /// <summary>
        /// Shuts down.
        /// </summary>
        public void ShutDown()
        {
            renderThread.Abort();
        }
    }
}