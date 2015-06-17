using System;
using System.Collections.Generic;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;

namespace CacheViewer.Controls.Viewer
{
    public class Scene : IDisposable
    {
        private List<int> alphas;
        private List<DisplayMesh> models;
        private List<int> opaques;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class.
        /// </summary>
        /// <param name="effectName">Name of the effect.</param>
        /// <param name="inTechniqueName">Name of the in technique.</param>
        /// <param name="dir">The dir.</param>
        /// <param name="sceneFile">The scene file.</param>
        /// <param name="inNormalLoad">The in normal load.</param>
        /// <param name="sceneName">Name of the scene.</param>
        public Scene(string effectName, string inTechniqueName, string dir, string sceneFile,
            ObjLoader.NormalLoad inNormalLoad, string sceneName = "Unnamed")
        {
            var effectToCompile = ShaderBytecode.CompileFromFile(effectName, "Textured", "fx_5_0", ShaderFlags.None,
                EffectFlags.None);
            this.LoadScene(dir, sceneFile, new Effect(ViewerBase.Instance.ViewDevice, effectToCompile), inTechniqueName,
                inNormalLoad);
            this.SceneName = sceneName;
        }

        /// <summary>
        /// Gets or sets the name of the scene.
        /// </summary>
        /// <value>
        /// The name of the scene.
        /// </value>
        public string SceneName { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (DisplayMesh m in this.models)
            {
                m.Dispose();
            }
        }

        private void LoadScene(string inDir, string inEscenaFile, Effect inEfecto, string inTechnique,
            ObjLoader.NormalLoad inNormalLoad)
        {
            this.models = ObjLoader.FromFile(inDir, inEscenaFile, inEfecto, inTechnique, inNormalLoad);
            this.alphas = new List<int>();
            this.opaques = new List<int>();
            for (int i = 0; i < this.models.Count; i++)
            {
                if (this.models[i].IsOpaque())
                {
                    this.opaques.Add(i);
                }
                else
                {
                    this.alphas.Add(i);
                }
            }
        }

        /// <summary>
        /// Draws the specified in world.
        /// </summary>
        /// <param name="inWorld">The in world.</param>
        /// <param name="inViewProjection">The in view projection.</param>
        /// <returns></returns>
        public long Draw(Matrix inWorld, Matrix inViewProjection)
        {
            long drawed = 0;
            foreach (int i in this.opaques)
            {
                drawed += this.models[i].Draw(inWorld, inViewProjection);
            }
            ViewerBase.Instance.ViewDevice.ImmediateContext.OutputMerger.BlendState =
                States.Instance.blendEnabledSourceAlphaInverseSourceAlpha;
            foreach (int i in this.alphas)
            {
                drawed += this.models[i].Draw(inWorld, inViewProjection);
            }
            ViewerBase.Instance.ViewDevice.ImmediateContext.OutputMerger.BlendState = States.Instance.blendDisabled;
            return drawed;
        }
    }
}