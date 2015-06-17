using System.Collections.Generic;
using CacheViewer.Domain.Models;

namespace CacheViewer.Domain.Utility
{
    /// <summary>
    /// </summary>
    public class Scene
    {
        /// <summary>
        ///     The instance
        /// </summary>
        private static readonly Scene instance = new Scene();

        /// <summary>
        ///     The render objects
        /// </summary>
        private readonly List<Renderable> renderObjects = new List<Renderable>();

        /// <summary>
        ///     Prevents a default instance of the <see cref="Scene" /> class from being created.
        /// </summary>
        private Scene()
        {
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>
        ///     The instance.
        /// </value>
        public static Scene Instance { get { return instance; } }

        /// <summary>
        ///     Adds the render object.
        /// </summary>
        /// <param name="renderObject">The render object.</param>
        public void AddRenderObject(Renderable renderObject)
        {
            lock (this.renderObjects)
            {
                this.renderObjects.Add(renderObject);
            }
        }

        /// <summary>
        ///     Removes the render object.
        /// </summary>
        /// <param name="renderObject">The render object.</param>
        public void RemoveRenderObject(Renderable renderObject)
        {
            lock (this.renderObjects)
            {
                if (this.renderObjects.Contains(renderObject))
                {
                    this.renderObjects.Remove(renderObject);
                }
            }
        }

        /// <summary>
        ///     Renders this instance.
        /// </summary>
        public void Render()
        {
            lock (this.renderObjects)
            {
                foreach (Renderable renderable in this.renderObjects)
                {
                    renderable.Render();
                }
            }
        }
    }
}