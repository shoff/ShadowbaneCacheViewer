using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using CacheViewer.Domain.Exporters;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Buffer = SlimDX.Direct3D11.Buffer;
using Device = SlimDX.Direct3D11.Device;


namespace CacheViewer.Controls
{

    // To convert from right hand to left hand, we must do a couple things. 
    // First is invert the z-axis of the vertice's positions by multiplying it with -1.0f.
    // We will also need to invert the v-axis of the texture coordinats by subtracting it from 1.0f.
    // Finally we will need to convert the z-axis of the vertex normals by multiplying it by -1.0f. 
    // I have put a boolean variable in the parameters of the load model function, where if it is set to true, 
    // we will do the conversion, and if it is set to false we will not do the conversion.
    public partial class SlimRenderControl : UserControl
    {
        enum GraphicsMode
        {
            SolidBlue = 0,
            PerVertexColoring,
            Textured
        }

        private readonly Color4 clearColor;
        private Device device; // The Direct3D device.
        private DeviceContext deviceContext; // This is just a convenience member.  It holds the context for the Direct3D device.
        private RenderTargetView renderTargetView; // Our render target.
        private SwapChain swapChain; // Our swap chain.
        private Viewport viewport; // The viewport.
        private InputLayout inputLayout;  // Tells Direct3D about the vertex format we are using.
        private VertexShader vertexShader; // This is the vertex shader.
        private ShaderSignature vShaderSignature; // The vertex shader signature.
        private PixelShader cbChangePixelShader; // This is the pixel shader.

        private Buffer cbChangesOnResize;
        private Buffer cbChangesPerFrame;
        private Buffer cbChangesPerObject;
        private Buffer vertexBuffer; // This will hold our geometry.

        // new
        private Buffer indices;
        private Buffer normals;
        private Buffer vertices;
        private Buffer texCoords;
        private int indexCount;

        private DataStream dataStream;
        private Matrix viewMatrix;  // This is our view matrix.
        private Matrix projectionMatrix;  // The projection matrix.
        private Matrix cubeWorldMatrix;   // The world matrix for the cube.  This controls the current position and rotation of the cube.
        private Matrix cubeRotationMatrix;  // This matrix controls the rotation of our cube.
        private Texture2D depthStencilTexture;     // Holds the depth stencil texture.
        private DepthStencilView depthStencilView; // The depth stencil view object.
        private ShaderResourceView cubeTexture;        // Holds the texture for our cube.
        private SamplerState cubeTexSamplerState;      // The sampler state we will use with our cube texture.

        private readonly Vector3 cameraPosition = new Vector3(0, 2, -5); // The position of our camera in 3D space.
        private float cubeRotation = 0.005f; // The current rotation amount for the cube on the Y axis.
        private float moveSpeed = 0.01f;     // Sets the speed you move around at.
        private GraphicsMode graphicsMode = GraphicsMode.SolidBlue;

        public SlimRenderControl()
        {
            this.InitializeComponent();

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.clearColor = new Color4(1.0f, 0.0f, 0.0f, 0.0f);

                this.InitializeSlimDx();
            }
        }

        private void InitializeSlimDx()
        {
            if (!this.InitD3D())
            {
                return;
            }

            this.InitShaders();

            this.SetupModel();
        }

        private bool InitD3D()
        {
            // Setup the configuration for the SwapChain.
            var swapChainDesc = new SwapChainDescription()
            {
                BufferCount = 2, // 2 back buffers (a.k.a. Triple Buffering).
                Usage = Usage.RenderTargetOutput,
                OutputHandle = FormObject.Handle,
                IsWindowed = true,
                ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                SampleDescription = new SampleDescription(1, 0),
                Flags = SwapChainFlags.AllowModeSwitch,
                SwapEffect = SwapEffect.Discard
            };


            // Create the SwapChain and check for errors.
            if (Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.Debug, new[] { FeatureLevel.Level_11_0 },
                swapChainDesc, out this.device, out this.swapChain).IsFailure)
            {
                // An error has occurred.  Initialization of the Direct3D device has failed for some reason.
                return false;
            }
            
            // Create a view of our render target, which is the backbuffer of the swap chain we just created
            using (var resource = SlimDX.Direct3D11.Resource.FromSwapChain<Texture2D>(this.swapChain, 0))
            {
                this.renderTargetView = new RenderTargetView(this.device, resource);
            };
            
            // Get the device context and store it in our deviceContext member variable.
            this.deviceContext = this.device.ImmediateContext;
            
            // Setting a viewport is required if you want to actually see anything
            this.viewport = new Viewport(0.0f, 0.0f, FormObject.Width, FormObject.Height, 0.0f, 1.0f);
            this.deviceContext.Rasterizer.SetViewports(this.viewport);
            this.deviceContext.OutputMerger.SetTargets(this.renderTargetView);
            
            // Prevent DXGI handling of Alt+Enter since it does not work properly with Winforms
            using (var factory = this.swapChain.GetParent<Factory>())
            {
                factory.SetWindowAssociation(FormObject.Handle,
                                             WindowAssociationFlags.IgnoreAltEnter);
            };

            // Create our projection matrix.
            this.projectionMatrix = Matrix.PerspectiveFovLH(1.570796f,
                (float)FormObject.Width / (float)FormObject.Height, 0.5f, 100.0f);

            // Create our view matrix.
            this.viewMatrix = Matrix.LookAtLH(this.cameraPosition, new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            return true;
        }

        public void InitShaders()
        {
            // Load and compile the vertex shader
            string vsCompileError = "Vertex Shader Compile Error!!!";
            using (var bytecode = ShaderBytecode.CompileFromFile("Effects.fx", "Vertex_Shader", "vs_4_0",
                ShaderFlags.Debug, EffectFlags.None, null, null, out vsCompileError))
            {
                this.vShaderSignature = ShaderSignature.GetInputSignature(bytecode);
                this.vertexShader = new VertexShader(this.device, bytecode);
            }
            
            // Load and compile the pixel shader
            string pixelShaderName = "";
            if (this.graphicsMode == GraphicsMode.SolidBlue)
            {
                pixelShaderName = "Pixel_Shader_Blue";
            }
            else if (this.graphicsMode == GraphicsMode.PerVertexColoring)
            {
                pixelShaderName = "Pixel_Shader_Color";
            }
            else if (this.graphicsMode == GraphicsMode.Textured)
            {
                pixelShaderName = "Pixel_Shader_Texture";
            }

            string psCompileError = "Pixel Shader Compile Error!!!";
            using (var bytecode = ShaderBytecode.CompileFromFile("Effects.fx", pixelShaderName,
                "ps_4_0", ShaderFlags.Debug, EffectFlags.None, null, null, out psCompileError))
            {
                this.cbChangePixelShader = new PixelShader(this.device, bytecode);
            }
            
            // Set the shaders.
            this.deviceContext.VertexShader.Set(this.vertexShader);
            this.deviceContext.PixelShader.Set(this.cbChangePixelShader);
        }

        // called in SetupModel
        public void InitScene()
        {
            //// Create our projection matrix.
            //this.projectionMatrix = Matrix.PerspectiveFovLH(1.570796f, 
            //    (float)FormObject.Width / (float)FormObject.Height, 0.5f, 100.0f);

            //// Create our view matrix.
            //this.viewMatrix = Matrix.LookAtLH(this.cameraPosition, new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            // Create the vertices of our cube.
            var vertexData = Verts;

            // Create a DataStream object that we will use to put the vertices into the vertex buffer.
            using (DataStream innerDataStream = new DataStream(40 * vertexData.Length, true, true))
            {
                innerDataStream.Position = 0;
                foreach (Vertex v in vertexData)
                {
                    innerDataStream.Write(v);
                }
                innerDataStream.Position = 0;
                
                // Create a description for the vertex buffer.
                BufferDescription bd = new BufferDescription();
                bd.Usage = ResourceUsage.Default;
                bd.SizeInBytes = 40 * vertexData.Length;
                bd.BindFlags = BindFlags.VertexBuffer;
                bd.CpuAccessFlags = CpuAccessFlags.None;
                bd.OptionFlags = ResourceOptionFlags.None;
                bd.StructureByteStride = 40;

                // Create the vertex buffer.
                this.vertexBuffer = new Buffer(this.device, innerDataStream, bd);
            }

            // Define the vertex format.
            // This tells Direct3D what information we are storing for each vertex, and how it is stored.
            InputElement[] inputElements = new InputElement[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
                new InputElement("COLOR",    0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float,       InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0)
            };

            // Create the InputLayout using the vertex format we just created.
            this.inputLayout = new InputLayout(this.device, this.vShaderSignature, inputElements);
            
            // Setup the InputAssembler stage of the Direct3D 11 graphics pipeline.
            this.deviceContext.InputAssembler.InputLayout = this.inputLayout;
            this.deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(this.vertexBuffer, 40, 0));
            // Set the Primitive Topology.
            this.deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            
            // Load the cube texture.
            this.cubeTexture = ShaderResourceView.FromFile(this.device, Application.StartupPath + "\\Brick.png");

            // Create a SamplerDescription
            SamplerDescription sd = new SamplerDescription();
            sd.Filter = Filter.MinMagMipLinear;
            sd.AddressU = TextureAddressMode.Wrap;
            sd.AddressV = TextureAddressMode.Wrap;
            sd.AddressW = TextureAddressMode.Wrap;
            sd.ComparisonFunction = Comparison.Never;
            sd.MinimumLod = 0;
            sd.MaximumLod = float.MaxValue;

            // Create our SamplerState
            this.cubeTexSamplerState = SamplerState.FromDescription(this.device, sd);

        }

        public Vertex[] Verts
        {
            get { return vertexData; }
            set
            {
                this.vertexData = value;
                this.SetupModel();
            }
        }

        private void SetupModel()
        {
            this.InitScene();

            this.InitDepthStencil();

            this.InitConstantBuffers();
        }

        // called in SetupModel second
        public void InitDepthStencil()
        {
            // Create the depth stencil texture description
            Texture2DDescription depthStencilTextureDesc = new Texture2DDescription
            {
                Width = this.FormObject.ClientSize.Width,
                Height = this.FormObject.ClientSize.Height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.D24_UNorm_S8_UInt,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            // Create the Depth Stencil View description
            DepthStencilViewDescription depthStencilViewDesc = new DepthStencilViewDescription
            {
                Format = depthStencilTextureDesc.Format,
                Dimension = DepthStencilViewDimension.Texture2D,
                MipSlice = 0
            };

            // Create the depth stencil texture.
            this.depthStencilTexture = new Texture2D(this.device, depthStencilTextureDesc);

            // Create the DepthStencilView object.
            this.depthStencilView = new DepthStencilView(this.device, this.depthStencilTexture, depthStencilViewDesc);

            // Make the DepthStencilView active.
            this.deviceContext.OutputMerger.SetTargets(this.depthStencilView, this.renderTargetView);
        }

        // called in SetupModel last
        public void InitConstantBuffers()
        {
            // Create a buffer description.
            BufferDescription bd = new BufferDescription
            {
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.None, 
                SizeInBytes = 64
            };

            this.cbChangesOnResize = new Buffer(this.device, bd);  // Create the changes on resize buffer.
            this.cbChangesPerFrame = new Buffer(this.device, bd);  // Create the changes per frame buffer.
            this.cbChangesPerObject = new Buffer(this.device, bd); // Create the changes per object buffer.
            
            // Send the Projection matrix into the changes on resize constant buffer.
            this.dataStream = new DataStream(64, true, true)
            {
                Position = 0
            };

            this.dataStream.Write(Matrix.Transpose(this.projectionMatrix));
            this.dataStream.Position = 0;
            this.device.ImmediateContext.UpdateSubresource(new DataBox(0, 0, this.dataStream), this.cbChangesOnResize, 0);
            
            // Send the View matrix into the changes per frame buffer.
            this.dataStream.Position = 0;
            this.dataStream.Write(Matrix.Transpose(this.viewMatrix));
            this.dataStream.Position = 0;
            this.device.ImmediateContext.UpdateSubresource(new DataBox(0, 0, this.dataStream), this.cbChangesPerFrame, 0);

            // Tell the VertexShader to use our constant buffers.
            this.deviceContext.VertexShader.SetConstantBuffer(this.cbChangesOnResize, 0);
            this.deviceContext.VertexShader.SetConstantBuffer(this.cbChangesPerFrame, 1);
            this.deviceContext.VertexShader.SetConstantBuffer(this.cbChangesPerObject, 2);

        }

        public void RenderFrame()
        {
            this.UpdateScene();

            // clear the render target to a soothing blue
            // this causes a nasty flicker when used as a control on a form
            // this.deviceContext.ClearRenderTargetView(this.renderTargetView, new Color4(0.5f, 0.5f, 1.0f));
            // this.deviceContext.ClearRenderTargetView(this.renderTargetView, this.clearColor);

            //this.deviceContext.Draw(3, 0);
            this.swapChain.Present(0, PresentFlags.None);
            
            // Clear the screen before we draw the next frame.
            this.deviceContext.ClearRenderTargetView(this.renderTargetView, this.clearColor);
            this.deviceContext.ClearDepthStencilView(this.depthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
            this.deviceContext.PixelShader.SetShaderResource(this.cubeTexture, 0);
            this.deviceContext.PixelShader.SetSampler(this.cubeTexSamplerState, 0);

            // Send the cube's world matrix to the changes per object constant buffer.
            this.dataStream.Position = 0;
            this.dataStream.Write(Matrix.Transpose(this.cubeWorldMatrix));
            this.dataStream.Position = 0;
            this.device.ImmediateContext.UpdateSubresource(new DataBox(0, 0, this.dataStream), this.cbChangesPerObject, 0);

            // Draw the triangle that we created in our vertex buffer.
            this.deviceContext.Draw(36, 0);

            // Present the frame we just rendered to the user.
            this.swapChain.Present(0, PresentFlags.None);
        }

        public void UpdateScene()
        {
            if (this.Disposing)
            {
                return;
            }

            // Keep the cube rotating by increasing its rotation amount
            this.cubeRotation += 0.00025f;
            if (this.cubeRotation > 6.28f) // 2 times PI
            {
                this.cubeRotation = 0.0f;
            }

            // Update the view matrix.
            this.viewMatrix = Matrix.LookAtLH(this.cameraPosition, new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            // Send the updated view matrix into its constant buffer.
            this.dataStream.Position = 0;
            var transpose = Matrix.Transpose(this.viewMatrix);
            this.dataStream.Write(transpose);
            this.dataStream.Position = 0;
            this.device.ImmediateContext.UpdateSubresource(new DataBox(0, 0, this.dataStream),
                this.cbChangesPerFrame, 0);

            // Update the cube's rotation matrix.
            this.cubeRotationMatrix = Matrix.RotationAxis(new Vector3(0.0f, 1.0f, 0.0f), this.cubeRotation);

            // Update the cube's world matrix with the new translation and rotation matrices.
            this.cubeWorldMatrix = this.cubeRotationMatrix;
        }

        public void InitScene(MeshData meshData)
        {
            // Create the vertices of our cube.
            var vertexData = Verts;

            // Create a DataStream object that we will use to put the vertices into the vertex buffer.
            using (DataStream innerDataStream = new DataStream(40 * vertexData.Length, true, true))
            {
                innerDataStream.Position = 0;
                foreach (Vertex v in vertexData)
                {
                    innerDataStream.Write(v);
                }
                innerDataStream.Position = 0;

                // Create a description for the vertex buffer.
                BufferDescription bd = new BufferDescription();
                bd.Usage = ResourceUsage.Default;
                bd.SizeInBytes = 40 * vertexData.Length;
                bd.BindFlags = BindFlags.VertexBuffer;
                bd.CpuAccessFlags = CpuAccessFlags.None;
                bd.OptionFlags = ResourceOptionFlags.None;
                bd.StructureByteStride = 40;

                // Create the vertex buffer.
                this.vertexBuffer = new Buffer(this.device, innerDataStream, bd);
            }

            // Define the vertex format.
            // This tells Direct3D what information we are storing for each vertex, and how it is stored.
            InputElement[] inputElements = new InputElement[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
                new InputElement("COLOR",    0, Format.R32G32B32A32_Float, InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float,       InputElement.AppendAligned, 0, InputClassification.PerVertexData, 0)
            };

            // Create the InputLayout using the vertex format we just created.
            this.inputLayout = new InputLayout(this.device, this.vShaderSignature, inputElements);

            // Setup the InputAssembler stage of the Direct3D 11 graphics pipeline.
            this.deviceContext.InputAssembler.InputLayout = this.inputLayout;
            this.deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(this.vertexBuffer, 40, 0));
            // Set the Primitive Topology.
            this.deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            // Load the cube texture.
            this.cubeTexture = ShaderResourceView.FromFile(this.device, Application.StartupPath + "\\Brick.png");

            // Create a SamplerDescription
            SamplerDescription sd = new SamplerDescription();
            sd.Filter = Filter.MinMagMipLinear;
            sd.AddressU = TextureAddressMode.Wrap;
            sd.AddressV = TextureAddressMode.Wrap;
            sd.AddressW = TextureAddressMode.Wrap;
            sd.ComparisonFunction = Comparison.Never;
            sd.MinimumLod = 0;
            sd.MaximumLod = float.MaxValue;

            // Create our SamplerState
            this.cubeTexSamplerState = SamplerState.FromDescription(this.device, sd);

        }

        private void LoadInputBindings(List<string> inSemantics, List<Format> inFormats, List<Buffer> inBuffers, List<int> inStrides)
        {

            InputElement[] elements = new InputElement[inSemantics.Count];
           
            var binding = new VertexBufferBinding[inSemantics.Count];

            for (int i = 0; i < inSemantics.Count; i++)
            {
                elements[i] = new InputElement(inSemantics[i], 0, inFormats[i], i);
                binding[i] = new VertexBufferBinding(inBuffers[i], inStrides[i], 0);
            }

            this.inputLayout = new InputLayout(this.device, this.vShaderSignature, elements);

        }

        private void LoadMesh(MeshData inMesh)
        {
            List<string> semantics = new List<string>();
            List<Format> formats = new List<Format>();
            List<Buffer> buffers = new List<Buffer>();
            List<int> strides = new List<int>();

            using (var data = new DataStream(inMesh.Indices.ToArray(), true, false))
            {
                this.indices = new Buffer(device, data, inMesh.Indices.Count * 4,
                    ResourceUsage.Default, BindFlags.IndexBuffer, CpuAccessFlags.None,
                    ResourceOptionFlags.None, 4);
                this.indexCount = inMesh.Indices.Count;
            }

            using (var data = new DataStream(inMesh.Positions.ToArray(), true, false))
            {
                this.vertices = new Buffer(device, data, inMesh.Positions.Count * 4 * 3,
                    ResourceUsage.Default, BindFlags.VertexBuffer, CpuAccessFlags.None,
                    ResourceOptionFlags.None, 12);
                semantics.Add("POSITION");
                formats.Add(Format.R32G32B32_Float);
                buffers.Add(vertices);
                strides.Add(12);
            }

            if (inMesh.Normals.Count != 0)
            {
                using (var data = new DataStream(inMesh.Normals.ToArray(), true, false))
                {
                    this.normals = new Buffer(device, data, inMesh.Normals.Count * 4 * 3,
                        ResourceUsage.Default, BindFlags.VertexBuffer, CpuAccessFlags.None,
                        ResourceOptionFlags.None, 12);
                    semantics.Add("NORMAL");
                    formats.Add(Format.R32G32B32_Float);
                    buffers.Add(normals);
                    strides.Add(12);
                }
            }

            if (inMesh.TextureCoordinates.Count != 0)
            {
                using (var data = new DataStream(inMesh.TextureCoordinates.ToArray(), true, false))
                {
                    this.texCoords = new Buffer(device, data, inMesh.TextureCoordinates.Count * 4 * 2,
                        ResourceUsage.Default, BindFlags.VertexBuffer, CpuAccessFlags.None,
                        ResourceOptionFlags.None, 8);
                    semantics.Add("TEXCOORD");
                    formats.Add(Format.R32G32_Float);
                    buffers.Add(texCoords);
                    strides.Add(8);
                }
            }

            LoadInputBindings(semantics, formats, buffers, strides);
        }

        #region Initial VertexData
        private Vertex[] vertexData =
            {
                // Bottom face of the cube.
                new Vertex()
                {
                    Position = new Vector4(-1.0f, -1.0f, 1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(0, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), 
                    Color = new Color4(1.0f, 1.0f, 0.0f, 0.0f),
                    TexCoord = new Vector2(0, 1)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, -1.0f, 1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 0.0f, 1.0f),
                    TexCoord = new Vector2(1, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, -1.0f, -1.0f, 1.0f),
                    Color = new Color4(1.0f, 1.0f, 0.0f, 0.0f),
                    TexCoord = new Vector2(0, 1)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, -1.0f, -1.0f, 1.0f), 
                    Color = new Color4(1.0f, 1.0f, 1.0f, 1.0f),
                    TexCoord = new Vector2(1, 1)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, -1.0f, 1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 0.0f, 1.0f),
                    TexCoord = new Vector2(1, 0)
                },


                // Front face of the cube.
                new Vertex()
                {
                    Position = new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), 
                    Color = new Color4(1.0f, 1.0f, 0.0f, 0.0f),
                    TexCoord = new Vector2(0, 1)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, 1.0f, -1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 0.0f, 1.0f),
                    TexCoord = new Vector2(0, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, -1.0f, -1.0f, 1.0f), 
                    Color = new Color4(1.0f, 1.0f, 1.0f, 1.0f),
                    TexCoord = new Vector2(1, 1)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, 1.0f, -1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 0.0f, 1.0f),
                    TexCoord = new Vector2(0, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, 1.0f, -1.0f, 1.0f),
                    Color = new Color4(0.0f, 0.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(1, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, -1.0f, -1.0f, 1.0f), 
                    Color = new Color4(1.0f, 1.0f, 1.0f, 1.0f),
                    TexCoord = new Vector2(1, 1)
                },


                // Right face of the cube.
                new Vertex()
                {
                    Position = new Vector4(1.0f, -1.0f, -1.0f, 1.0f),
                    Color = new Color4(1.0f, 1.0f, 1.0f, 1.0f),
                    TexCoord = new Vector2(0, 1)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, 1.0f, -1.0f, 1.0f),
                    Color = new Color4(1.0f, 0.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(0, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, -1.0f, 1.0f, 1.0f),
                    Color = new Color4(1.0f, 0.0f, 0.0f, 1.0f),
                    TexCoord = new Vector2(1, 1)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, 1.0f, -1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(0, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 
                    Color = new Color4(1.0f, 1.0f, 0.0f, 0.0f),
                    TexCoord = new Vector2(1, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, -1.0f, 1.0f, 1.0f),
                    Color = new Color4(1.0f, 0.0f, 0.0f, 1.0f),
                    TexCoord = new Vector2(1, 1)
                },


                // Back face of the cube.
                new Vertex()
                {
                    Position = new Vector4(1.0f, -1.0f, 1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 0.0f, 1.0f),
                    TexCoord = new Vector2(0, 1)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 
                    Color = new Color4(1.0f, 1.0f, 0.0f, 0.0f),
                    TexCoord = new Vector2(0, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, -1.0f, 1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(1, 1)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                    Color = new Color4(1.0f, 1.0f, 0.0f, 0.0f),
                    TexCoord = new Vector2(0, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, 1.0f, 1.0f, 1.0f), 
                    Color = new Color4(1.0f, 1.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(1, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, -1.0f, 1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(1, 1)
                },


                // Left face of the cube.
                new Vertex()
                {
                    Position = new Vector4(-1.0f, -1.0f, 1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(0, 1)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, 1.0f, 1.0f, 1.0f),
                    Color = new Color4(1.0f, 1.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(0, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, -1.0f, -1.0f, 1.0f),
                    Color = new Color4(1.0f, 1.0f, 0.0f, 0.0f),
                    TexCoord = new Vector2(1, 1)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, 1.0f, 1.0f, 1.0f), 
                    Color = new Color4(1.0f, 1.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(0, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, 1.0f, -1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 0.0f, 1.0f),
                    TexCoord = new Vector2(1, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, -1.0f, -1.0f, 1.0f),
                    Color = new Color4(1.0f, 1.0f, 0.0f, 0.0f),
                    TexCoord = new Vector2(1, 1)
                },


                // Top face of the cube.
                new Vertex()
                {
                    Position = new Vector4(-1.0f, 1.0f, -1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 0.0f, 1.0f),
                    TexCoord = new Vector2(0, 1)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, 1.0f, 1.0f, 1.0f), 
                    Color = new Color4(1.0f, 1.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(0, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, 1.0f, -1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(1, 1)
                },
                new Vertex()
                {
                    Position = new Vector4(-1.0f, 1.0f, 1.0f, 1.0f), 
                    Color = new Color4(1.0f, 1.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(0, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                    Color = new Color4(1.0f, 1.0f, 0.0f, 0.0f),
                    TexCoord = new Vector2(1, 0)
                },
                new Vertex()
                {
                    Position = new Vector4(1.0f, 1.0f, -1.0f, 1.0f), 
                    Color = new Color4(1.0f, 0.0f, 1.0f, 0.0f),
                    TexCoord = new Vector2(1, 1)
                },
            };

        #endregion
    }

    public struct Vertex
    {
        public Vector4 Position;   // The position of the vertex in 3D space.
        public Color4 Color;       // The color to use for this vertex when we are not using textured mode.
        public Vector2 TexCoord;   // The textures coordinates for this vertex.  We need these when we are using textured mode.
    }
}
