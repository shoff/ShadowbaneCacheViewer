using SlimDX;

namespace CacheViewer.Controls.Viewer
{
    public interface IDisplayMesh
    {
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Draws the specified in world.
        /// </summary>
        /// <param name="inWorld">The in world.</param>
        /// <param name="inViewProjection">The in view projection.</param>
        /// <returns></returns>
        long Draw(Matrix inWorld, Matrix inViewProjection);

        /// <summary>
        /// Determines whether this instance is opaque.
        /// </summary>
        /// <returns></returns>
        bool IsOpaque();
    }
}