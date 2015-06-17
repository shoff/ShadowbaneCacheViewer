using SlimDX.Direct3D11;

namespace CacheViewer.Controls.Viewer
{
    public class States
    {
        private static States instance;

        public States()
        {
            var device = ViewerBase.Instance.ViewDevice;
            DepthStencilStateDescription dsStateDesc = new DepthStencilStateDescription
            {
                IsDepthEnabled = false,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
            };
            this.depthDisabledStencilDisabled = DepthStencilState.FromDescription(device, dsStateDesc);
            dsStateDesc.IsDepthEnabled = true;
            this.depthEnabledStencilDisabled = DepthStencilState.FromDescription(device, dsStateDesc);
            dsStateDesc.DepthWriteMask = DepthWriteMask.Zero;
            this.depthEnabledWriteDisabledStencilDisabled = DepthStencilState.FromDescription(device, dsStateDesc);

            RasterizerStateDescription rasStateDesc = new RasterizerStateDescription
            {
                CullMode = CullMode.None,
                //No tiene nada que ver con near y far, es para evitar Z-fighting
                DepthBias = 1,
                DepthBiasClamp = 10.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterclockwise = true,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false
            };
            this.cullNoneFillSolid = RasterizerState.FromDescription(device, rasStateDesc);
            rasStateDesc.FillMode = FillMode.Wireframe;
            this.cullNoneFillWireframe = RasterizerState.FromDescription(device, rasStateDesc);
            rasStateDesc.CullMode = CullMode.Back;
            this.cullBackFillWireframe = RasterizerState.FromDescription(device, rasStateDesc);
            rasStateDesc.FillMode = FillMode.Solid;
            this.cullBackFillSolid = RasterizerState.FromDescription(device, rasStateDesc);
            rasStateDesc.CullMode = CullMode.Front;
            this.cullFrontFillSolid = RasterizerState.FromDescription(device, rasStateDesc);
            rasStateDesc.FillMode = FillMode.Wireframe;
            this.cullFrontFillWireframe = RasterizerState.FromDescription(device, rasStateDesc);

            BlendStateDescription blendStateDesc = new BlendStateDescription
            {
                IndependentBlendEnable = false,
                AlphaToCoverageEnable = false,
            };

            blendStateDesc.RenderTargets[0] = new RenderTargetBlendDescription();
            blendStateDesc.RenderTargets[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
            this.blendDisabled = BlendState.FromDescription(device, blendStateDesc);
            blendStateDesc.RenderTargets[0].BlendEnable = true;
            blendStateDesc.RenderTargets[0].BlendOperation = BlendOperation.Add;
            blendStateDesc.RenderTargets[0].BlendOperationAlpha = BlendOperation.Add;
            blendStateDesc.RenderTargets[0].DestinationBlend = BlendOption.DestinationAlpha;
            blendStateDesc.RenderTargets[0].DestinationBlendAlpha = BlendOption.DestinationAlpha;
            blendStateDesc.RenderTargets[0].SourceBlend = BlendOption.SourceAlpha;
            blendStateDesc.RenderTargets[0].SourceBlendAlpha = BlendOption.SourceAlpha;
            this.blendEnabledSourceAlphaDestinationAlpha = BlendState.FromDescription(device, blendStateDesc);
            blendStateDesc.RenderTargets[0].DestinationBlend = BlendOption.InverseSourceAlpha;
            blendStateDesc.RenderTargets[0].DestinationBlendAlpha = BlendOption.InverseSourceAlpha;
            blendStateDesc.RenderTargets[0].SourceBlend = BlendOption.SourceAlpha;
            blendStateDesc.RenderTargets[0].SourceBlendAlpha = BlendOption.SourceAlpha;
            this.blendEnabledSourceAlphaInverseSourceAlpha = BlendState.FromDescription(device, blendStateDesc);
            blendStateDesc.RenderTargets[0].DestinationBlend = BlendOption.One;
            blendStateDesc.RenderTargets[0].DestinationBlendAlpha = BlendOption.One;
            blendStateDesc.RenderTargets[0].SourceBlend = BlendOption.One;
            blendStateDesc.RenderTargets[0].SourceBlendAlpha = BlendOption.One;
            this.blendEnabledOneOne = BlendState.FromDescription(device, blendStateDesc);
        }

        public DepthStencilState depthEnabledStencilDisabled { get; private set; }
        public DepthStencilState depthDisabledStencilDisabled { get; private set; }
        public DepthStencilState depthEnabledWriteDisabledStencilDisabled { get; private set; }
        public RasterizerState cullNoneFillSolid { get; private set; }
        public RasterizerState cullNoneFillWireframe { get; private set; }
        public RasterizerState cullBackFillSolid { get; private set; }
        public RasterizerState cullBackFillWireframe { get; private set; }
        public RasterizerState cullFrontFillSolid { get; private set; }
        public RasterizerState cullFrontFillWireframe { get; private set; }
        public BlendState blendDisabled { get; private set; }
        public BlendState blendEnabledSourceAlphaInverseSourceAlpha { get; private set; }
        public BlendState blendEnabledSourceAlphaDestinationAlpha { get; private set; }
        public BlendState blendEnabledOneOne { get; private set; }

        public static States Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new States();
                }
                return instance;
            }
        }
    }
}