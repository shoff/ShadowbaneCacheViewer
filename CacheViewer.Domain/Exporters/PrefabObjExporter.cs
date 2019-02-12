namespace CacheViewer.Domain.Exporters
{
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Archive;
    using Exceptions;
    using Factories;
    using Models;
    using NLog;
    using Utility;

    /// <summary>
    /// Attempts to create a wavefront obj from multiple meshes/textures.
    /// </summary>
    public class PrefabObjExporter : IPrefabObjExporter
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private const string UsesCentimeters = "# This file uses centimeters as units for non-parametric coordinates.\r\n";
        private const string MaterialLib = "mtllib {0}.mtl\r\n";
        private const string Vertice = "v {0} {1} {2}\r\n";
        private const string Normal = "vn {0} {1} {2}\r\n";
        private const string Texture = "vt {0} {1}\r\n";
        private const string MaterialName = "newmtl {0}\r\n";
        private const string UseMaterial = "usemtl {0}\r\n";
        private const string MaterialWhite = "Ka 1.000 1.000 1.000\r\n";
        private const string MaterialDiffuse = "Kd 1.000 1.000 1.000\r\n";
        private const string MaterialSpecular = "Ks 0.000 0.000 0.000\r\n ";
        private const string MaterialSpecualrNs = "Ns 10.000\r\n";
        private const string MaterialDefaultIllumination = "illum 4\r\n";
        private const string MapTo = "map_Ka {0}\r\nmap_Kd {0}\r\nmap_Ks {0}\r\n";
        
        private readonly Textures texturesArchive = (Textures)ArchiveFactory.Instance.Build(CacheFile.Textures);
        private readonly Dictionary<string, string> indexMaterialDictionary = new Dictionary<string, string>();
        private readonly StringBuilder prefab = new StringBuilder();
        private readonly StringBuilder material = new StringBuilder();

        public PrefabObjExporter()
        {
            if (!string.IsNullOrEmpty(this.ModelDirectory))
            {
                if (!Directory.Exists(this.ModelDirectory))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    Directory.CreateDirectory(this.ModelDirectory);
                }
            }
        }

        public static MeshOnlyObjExporter Instance => new MeshOnlyObjExporter();

        public async Task CreatePrefabIndividualFiles(List<Mesh> meshModels, string modelName)
        {
            if (meshModels == null || meshModels.Count == 0)
            {
                logger?.Error(
                    $"An empty or null mesh collection passed to CreatePrefabAsync for model {modelName ?? "no-name"} aborting creation.");
                return;
            }

            if (string.IsNullOrWhiteSpace(modelName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(modelName));
            }

            if (string.IsNullOrWhiteSpace(this.ModelDirectory))
            {
                throw new ModelDirectoryNotSetException(modelName);
            }

            foreach (var mesh in meshModels)
            {
                if (mesh == null)
                {
                    continue;
                }
                var cindex = MeshFactory.Instance.Indexes.FirstOrDefault(c => c.Identity == mesh.CacheIndex.Identity);
                var m = MeshFactory.Instance.Create(cindex);

                foreach (var rt in mesh.Textures)
                {
                    var tex = TextureFactory.Instance.Build(rt.TextureId);
                    m.Textures.Add(tex);
                }

                await Instance.ExportAsync(m, $"{modelName}_{m.Id}", this.ModelDirectory);
            }
        }

        public async Task CreatePrefabAsync(List<Mesh> meshModels, string modelName)
        {
            if (meshModels == null || meshModels.Count == 0)
            {
                logger?.Error(
                    $"An empty or null mesh collection passed to CreatePrefabAsync for model {modelName ?? "no-name"} aborting creation.");
                return;
            }

            if (string.IsNullOrWhiteSpace(modelName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(modelName));
            }

            if (string.IsNullOrWhiteSpace(this.ModelDirectory))
            {
                throw new ModelDirectoryNotSetException(modelName);
            }

            // Builds the .obj header/information header
            this.prefab.AppendLine(MayaObjHeaderFactory.Instance.Create(modelName));
            this.prefab.AppendFormat(MaterialLib, modelName);
            this.prefab.Append(UsesCentimeters); // TODO validate this flag

            var vertexBuilder = new StringBuilder();
            var normalsBuilder = new StringBuilder();
            var texturesBuilder = new StringBuilder();

            foreach (var mesh in meshModels)
            {
                try
                {
                    var meshName = string.Join(string.Empty, "Mesh_", mesh.CacheIndex.Identity.ToString(CultureInfo.InvariantCulture));
                    this.indexMaterialDictionary.Add(meshName, string.Empty);
                    // do the textures first because this is the most likely place to have an exception thrown. Die young leave a beautiful corpse.
                    this.SaveTextures(mesh, meshName);

                    // now create the material entry for the mesh
                    // TODO handle extra maps?
                    this.AppendMaterial(this.indexMaterialDictionary[meshName], $"Mesh_{mesh.Id}");

                    // v 
                    vertexBuilder.AppendLine(this.BuildVerts(mesh));

                    // vt
                    texturesBuilder.AppendLine(this.BuildTextures(mesh));
                    
                    // vn
                    normalsBuilder.AppendLine(this.BuildNormals(mesh));
                }
                catch (Exception e)
                {
                    logger?.Error(e);
                }
            }
            
            // combine them all
            this.prefab.AppendLine(vertexBuilder.ToString());
            this.prefab.AppendLine(texturesBuilder.ToString());
            this.prefab.AppendLine(normalsBuilder.ToString());

            // now build the faces
            this.BuildFaces(meshModels, this.prefab);

            using (var fs = new FileStream(this.ModelDirectory + "\\" + modelName + ".obj",
                    FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (var writer = new StreamWriter(fs))
                {
                    await writer.WriteAsync(this.prefab.ToString());
                }
            }

            // save the material
            var mtlFile = this.ModelDirectory + "\\" + modelName + ".mtl";
            if (File.Exists(mtlFile))
            {
                File.Delete(mtlFile);
            }

            //File.WriteAllText(mtlFile, material.ToString());
            using (var fs1 = new FileStream(this.ModelDirectory + "\\" + modelName + ".mtl",
                FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (var writer = new StreamWriter(fs1))
                {
                    await writer.WriteAsync(this.material.ToString());
                }
            }
        }

        public string ModelDirectory { get; set; } = FileLocations.Instance.GetExportFolder();
        
        private string BuildVerts(Mesh mesh)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var v in mesh.Vertices)
            {
                sb.AppendFormat(Vertice, v[0].ToString("0.0#####"), v[1].ToString("0.0#####"), v[2].ToString("0.0#####"));
            }

            return sb.ToString();
        }

        private string BuildTextures(Mesh mesh)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var t in mesh.TextureVectors)
            {
                sb.AppendFormat(Texture, t[0].ToString("0.000000"), t[1].ToString("0.000000"));
            }

            return sb.ToString();
        }

        private string BuildNormals(Mesh mesh)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var vn in mesh.Normals)
            {
                sb.AppendFormat(Normal, vn[0].ToString("0.000000"), vn[1].ToString("0.000000"), vn[2].ToString("0.000000"));
            }

            return sb.ToString();
        }

        private void BuildFaces(List<Mesh> meshes, StringBuilder sb)
        {
            int currentIndexCount = 0;

            foreach (var mesh in meshes)
            {
                currentIndexCount++;

                // TODO this does not spit out the faces the same as the exporter from Maya does
                // g Mesh_124163:default1
                // usemtl initialShadingGroup
                sb.AppendLine($"g Mesh_{mesh.Id}:Mesh_{mesh.Id}");
                sb.AppendLine($"usemtl Mesh_{mesh.Id}");

                foreach (var wavefrontVertex in mesh.Indices)
                {
                    var a = (ushort)(wavefrontVertex.Position + currentIndexCount);
                    var b = (ushort)(wavefrontVertex.TextureCoordinate + currentIndexCount);
                    var c = (ushort)(wavefrontVertex.Normal + currentIndexCount);

                    sb.Append("f " + a + @"/" + a + @"/" + a + " " + b + @"/" + b + @"/" + b + " " + c + @"/" + c + @"/" + c + "\r\n");
                }
            }
        }
        
        private void SaveTextures(Mesh mesh, string meshName)
        {
            if (mesh.Textures.Any())
            {
                for (var i = 0; i < mesh.Textures.Count; i++)
                {
                    // Debug.Assert(mesh.Textures.Count == 1);
                    var texture = mesh.Textures[i];
                    var asset = this.texturesArchive[texture.TextureId];

                    using (var bitmap = mesh.Textures[i].TextureMap(asset.Item1))
                    {
                        var textureDirectory = this.ModelDirectory + "\\" + mesh.Id.ToString(CultureInfo.InvariantCulture).Replace(" ", "_") + "_" + i + ".png";
                        logger?.Info($"TextureDirectory: {textureDirectory}");
                        bitmap.Save(textureDirectory, ImageFormat.Png);

                        // TODO figure this shit out right here
                        var usmtl = string.Format(UseMaterial, $"Mesh_{mesh.Id}");
                        if (this.indexMaterialDictionary[meshName] != string.Empty)
                        {
                            // we're on more than one texture here ... shit
                            this.indexMaterialDictionary.Add($"{meshName}_{i}", textureDirectory);
                        }
                        else
                        {
                            this.indexMaterialDictionary[meshName] = textureDirectory;
                        }

                        // this should go before each set of faces.
                        // sb.AppendFormat(UseMaterial, $"Mesh_{mesh.Id}");
                    }
                }
            }
        }

        // Multiple illumination models are available, per material. These are enumerated as follows:
        // 0. Color on and Ambient off
        // 1. Color on and Ambient on
        // 2. Highlight on
        // 3. Reflection on and Ray trace on
        // 4. Transparency: Glass on, Reflection: Ray trace on
        // 5. Reflection: Fresnel on and Ray trace on
        // 6. Transparency: Refraction on, Reflection: Fresnel off and Ray trace on
        // 7. Transparency: Refraction on, Reflection: Fresnel on and Ray trace on
        // 8. Reflection on and Ray trace off
        // 9. Transparency: Glass on, Reflection: Ray trace off
        // 10. Casts shadows onto invisible surfaces

        // example
        // newmtl Textured
        // Ka 1.000 1.000 1.000
        // Kd 1.000 1.000 1.000
        // Ks 0.000 0.000 0.000
        // d 1.0
        // illum 2
        // map_Ka lenna.tga           # the ambient texture map
        // map_Kd lenna.tga           # the diffuse texture map (most of the time, it will
        // # be the same as the ambient texture map)
        // map_Ks lenna.tga           # the specular texture map
        // map_d lenna_alpha.tga      # the alpha texture map
        // map_bump lenna_bump.tga    # the bump map
        // bump lenna_bump.tga        # some implementations use 'bump' instead of 'map_Bump'

        private void AppendMaterial(string mapName, string materialName)
        {
            this.material.AppendFormat(MaterialName, materialName);
            this.material.Append(MaterialWhite);
            this.material.Append(MaterialDiffuse);
            this.material.Append(MaterialSpecular);
            this.material.Append(MaterialSpecualrNs);
            this.material.Append(MaterialDefaultIllumination);
            this.material.AppendFormat(MapTo, mapName.Substring(mapName.LastIndexOf('\\') + 1));
        }
    }
}