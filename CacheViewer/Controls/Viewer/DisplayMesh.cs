using System;
using System.Collections.Generic;
using CacheViewer.Domain.Exporters;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Buffer = SlimDX.Direct3D11.Buffer;
using Device = SlimDX.Direct3D11.Device;

namespace CacheViewer.Controls.Viewer
{
    public class DisplayMesh : IDisposable, IDisplayMesh
    {
        private readonly Device device;
        private readonly VertexBufferBinding[] nullBinding = new VertexBufferBinding[3];
        private readonly Texture2D texture;
        private VertexBufferBinding[] binding;
        private Effect effect;
        private InputElement[] elements;
        private int indexCount;
        private Buffer indices;
        private InputLayout layout;
        private Buffer normals;
        public string techniqueActual;
        private Buffer texCoords;
        private ShaderResourceView textureView;
        private Buffer vertices;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMesh" /> class.
        /// </summary>
        /// <param name="effectName">Name of the effect.</param>
        /// <param name="techniqueName">Name of the technique.</param>
        /// <param name="meshFile">The mesh file.</param>
        /// <param name="textureName">Name of the texture.</param>
        /// <param name="device">The device.</param>
        /// <param name="defaultName">The default name.</param>
        public DisplayMesh(string effectName, string techniqueName, string meshFile, 
            string textureName, Device device=null, string defaultName = "Unnamed")
        {
            this.device = device ?? ViewerBase.Instance.ViewDevice;
            this.LoadEffect(effectName, techniqueName);
            this.texture = Texture2D.FromFile(device, textureName);
            this.AsignarTextura();
            this.MeshName = defaultName;
            this.LoadMesh(meshFile);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMesh" /> class.
        /// </summary>
        /// <param name="effectName">Name of the effect.</param>
        /// <param name="techniqueName">Name of the technique.</param>
        /// <param name="inMesh">The in mesh.</param>
        /// <param name="inTexture">The in texture.</param>
        /// <param name="device">The device.</param>
        /// <param name="defaultName">The default name.</param>
        public DisplayMesh(string effectName, string techniqueName, MeshData inMesh, 
            Texture2D inTexture, Device device=null, string defaultName = "Unnamed")
        {
            //var ViewDevice = ViewerBase.Instance.ViewDevice;
            this.LoadEffect(effectName, techniqueName);
            this.texture = inTexture;
            this.device = device ?? ViewerBase.Instance.ViewDevice;
            this.AsignarTextura();
            this.MeshName = defaultName;
            this.LoadMesh(inMesh);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMesh" /> class.
        /// </summary>
        /// <param name="efecto">The efecto.</param>
        /// <param name="techniqueName">Name of the technique.</param>
        /// <param name="inMesh">The in mesh.</param>
        /// <param name="inTexture">The in texture.</param>
        /// <param name="device">The device.</param>
        /// <param name="defaultName">The default name.</param>
        public DisplayMesh(Effect efecto, string techniqueName, MeshData inMesh, 
            Texture2D inTexture, Device device=null, string defaultName = "Unnamed")
        {
            //var ViewDevice = ViewerBase.Instance.ViewDevice;
            this.LoadEffect(efecto, techniqueName);
            this.texture = inTexture;
            this.device = device ?? ViewerBase.Instance.ViewDevice;
            this.AsignarTextura();
            this.MeshName = defaultName;
            this.LoadMesh(inMesh);
        }

        private string MeshName { get; set; }

        public void Dispose()
        {
            this.indices.Dispose();
            this.normals.Dispose();
            this.vertices.Dispose();
            this.texCoords.Dispose();
            this.texture.Dispose();
            this.effect.Dispose();
            this.textureView.Dispose();
            this.layout.Dispose();
        }

        private void LoadEffect(string inEffectPath, string inTechnique)
        {
            // Effect associated with the model
            var efectoCompilado = ShaderBytecode.CompileFromFile(inEffectPath, null, "fx_5_0", ShaderFlags.None,
                EffectFlags.None);
            this.effect = new Effect(this.device, efectoCompilado);
            this.techniqueActual = inTechnique;
        }

        private void LoadEffect(Effect inEffect, string inTechnique)
        {
            // effects
            this.effect = inEffect.Clone();
            this.techniqueActual = inTechnique;
        }

        private void AsignarTextura()
        {
            this.textureView = new ShaderResourceView(this.device, this.texture);
            this.effect.GetVariableByName("texColorMap").AsResource().SetResource(this.textureView);
        }

        private void LoadInputBindings(List<string> inSemantics, List<Format> inFormats, 
            List<Buffer> inBuffers, List<int> inStrides)
        {
            this.elements = new InputElement[inSemantics.Count];
            this.binding = new VertexBufferBinding[inSemantics.Count];
            for (int i = 0; i < inSemantics.Count; i++)
            {
                this.elements[i] = new InputElement(inSemantics[i], 0, inFormats[i], i);
                this.binding[i] = new VertexBufferBinding(inBuffers[i], inStrides[i], 0);
            }
            this.layout = new InputLayout(device,
                this.effect.GetTechniqueByName(this.techniqueActual).GetPassByIndex(0).Description.Signature,
                this.elements);
        }

        private void LoadMesh(MeshData inMesh)
        {
            List<string> semantics = new List<string>();
            List<Format> formats = new List<Format>();
            List<Buffer> buffers = new List<Buffer>();
            List<int> strides = new List<int>();

           
            using (var data = new DataStream(inMesh.Indices.ToArray(), true, false))
            {
                this.indices = new Buffer(device, data, inMesh.Indices.Count * 4, ResourceUsage.Default,
                    BindFlags.IndexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 4);
                this.indexCount = inMesh.Indices.Count;
            }

            using (var data = new DataStream(inMesh.Positions.ToArray(), true, false))
            {
                this.vertices = new Buffer(device, data, inMesh.Positions.Count * 4 * 3, ResourceUsage.Default,
                    BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 12);
                semantics.Add("POSITION");
                formats.Add(Format.R32G32B32_Float);
                buffers.Add(this.vertices);
                strides.Add(12);
            }

            if (inMesh.Normals.Count != 0)
            {
                using (var data = new DataStream(inMesh.Normals.ToArray(), true, false))
                {
                    this.normals = new Buffer(device, data, inMesh.Normals.Count * 4 * 3, ResourceUsage.Default,
                        BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 12);
                    semantics.Add("NORMAL");
                    formats.Add(Format.R32G32B32_Float);
                    buffers.Add(this.normals);
                    strides.Add(12);
                }
            }

            if (inMesh.TextureCoordinates.Count != 0)
            {
                using (var data = new DataStream(inMesh.TextureCoordinates.ToArray(), true, false))
                {
                    this.texCoords = new Buffer(device, data, inMesh.TextureCoordinates.Count * 4 * 2, ResourceUsage.Default,
                        BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 8);
                    semantics.Add("TEXCOORD");
                    formats.Add(Format.R32G32_Float);
                    buffers.Add(this.texCoords);
                    strides.Add(8);
                }
            }

            this.LoadInputBindings(semantics, formats, buffers, strides);
        }

        private void LoadMesh(string meshName)
        {
            MeshData meshData;
            //if (meshName.EndsWith("smd", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    meshData = SmdLoader.FromFile(meshName);
            //}
            //else
            //{
             //   throw new SystemException("File format not supported (smd only)");
            //}
            //LoadMesh(meshData);
        }

        public long Draw(Matrix inWorld, Matrix inViewProjection)
        {
            var deviceContext = this.device.ImmediateContext;
            deviceContext.InputAssembler.InputLayout = this.layout;
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            deviceContext.InputAssembler.SetIndexBuffer(this.indices, Format.R32_UInt, 0);
            deviceContext.InputAssembler.SetVertexBuffers(0, this.binding);
            this.effect.GetConstantBufferByName("PerRenderBuffer").GetMemberByName("matWorld").AsMatrix().SetMatrix(
                inWorld);
            this.effect.GetConstantBufferByName("PerRenderBuffer").GetMemberByName("matViewProjection").AsMatrix()
                .SetMatrix(inViewProjection);
            this.effect.GetTechniqueByName(this.techniqueActual).GetPassByIndex(0).Apply(deviceContext);
            deviceContext.DrawIndexed(this.indexCount, 0, 0);
            deviceContext.InputAssembler.SetIndexBuffer(null, Format.Unknown, 0);
            deviceContext.InputAssembler.SetVertexBuffers(0, this.nullBinding);
            return this.indexCount;
        }

        public bool IsOpaque()
        {
            return this.textureView.Description.Format != Format.R8G8B8A8_UNorm;
        }
    }
}