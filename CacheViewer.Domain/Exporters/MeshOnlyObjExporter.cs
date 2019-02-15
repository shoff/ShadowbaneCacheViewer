namespace CacheViewer.Domain.Exporters
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Archive;
    using Factories;
    using Models;
    using NLog;
    using Utility;

    /// <summary>
    /// </summary>
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    public class MeshOnlyObjExporter
    {
        private const string SbMeshId = "# MeshId: {0}\r\n";

        private const string UsesCentimeters =
            "# This file uses centimeters as units for non-parametric coordinates.\r\n";

        private const string DefaultGroup = "g default\r\n";
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
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public string ModelDirectory { get; set; } = FileLocations.Instance.GetExportFolder();
        private string name;
        private readonly Dictionary<string, string> indexMaterialDictionary = new Dictionary<string, string>();
        private readonly Textures texturesArchive = (Textures)ArchiveFactory.Instance.Build(CacheFile.Textures);

        public MeshOnlyObjExporter()
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

        public async Task<bool> ExportAsync(Mesh mesh, string modelName = null, string modelDirectory=null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(modelDirectory))
                {
                    this.ModelDirectory = modelDirectory;
                }
                var mainStringBuilder = new StringBuilder();
                var materialBuilder = new StringBuilder();
                // todo - not all objects seem to have names
                this.name = modelName ?? string.Join(string.Empty, "Mesh_", mesh.CacheIndex.Identity.ToString(CultureInfo.InvariantCulture));
                modelName = this.name;

                mainStringBuilder.AppendLine(MayaObjHeaderFactory.Instance.Create(this.name));
                mainStringBuilder.AppendFormat(SbMeshId, mesh.CacheIndex.Identity);
                mainStringBuilder.AppendFormat(MaterialLib, this.name);
                mainStringBuilder.Append($"{UsesCentimeters}\r\n");
                mainStringBuilder.Append(DefaultGroup);

                try
                {
                    var meshName = string.Join(string.Empty, "Mesh_", mesh.CacheIndex.Identity.ToString(CultureInfo.InvariantCulture));
                    this.indexMaterialDictionary.Add(meshName, string.Empty);
                    // do the textures first because this is the most likely place to have an exception thrown. Die young leave a beautiful corpse.
                    this.SaveTextures(mesh, meshName);

                    // now create the material entry for the mesh
                    // TODO handle extra maps?
                    this.AppendMaterial(this.indexMaterialDictionary[meshName], $"Mesh_{mesh.Id}", materialBuilder);

                    // v 
                    mainStringBuilder.Append(this.BuildVerts(mesh));

                    // vt
                    mainStringBuilder.Append(this.BuildTextures(mesh));

                    // vn
                    mainStringBuilder.Append(this.BuildNormals(mesh));

                    // now build the faces
                    this.BuildFaces(mesh, mainStringBuilder);

                    using (var fs = new FileStream(modelDirectory + "\\" + modelName + ".obj",
                        FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (var writer = new StreamWriter(fs))
                        {
                            await writer.WriteAsync(mainStringBuilder.ToString());
                        }
                    }

                    // save the material
                    var mtlFile = modelDirectory + "\\" + modelName + ".mtl";
                    if (File.Exists(mtlFile))
                    {
                        File.Delete(mtlFile);
                    }

                    //File.WriteAllText(mtlFile, material.ToString());
                    using (var fs1 = new FileStream(modelDirectory + "\\" + modelName + ".mtl",
                        FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (var writer = new StreamWriter(fs1))
                        {
                            await writer.WriteAsync(materialBuilder.ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    logger?.Error(e);
                }
            }
            catch (Exception e)
            {
                logger?.Error(e);
                return false;
            }

            return true;
        }

        private void AppendMaterial(string mapName, string materialName, StringBuilder sb)
        {
            sb.AppendFormat(MaterialName, materialName);
            sb.Append(MaterialWhite);
            sb.Append(MaterialDiffuse);
            sb.Append(MaterialSpecular);
            sb.Append(MaterialSpecualrNs);
            sb.Append(MaterialDefaultIllumination);
            sb.AppendFormat(MapTo, mapName.Substring(mapName.LastIndexOf('\\') + 1));
        }

        private string BuildVerts(Mesh mesh)
        {
            var sb = new StringBuilder();
            foreach (var v in mesh.Vertices)
            {
                sb.AppendFormat(Vertice, v[0].ToString("0.000000"), v[1].ToString("0.000000"), v[2].ToString("0.000000"));
            }

            return sb.ToString();
        }

        private string BuildTextures(Mesh mesh)
        {
            var sb = new StringBuilder();
            foreach (var t in mesh.TextureVectors)
            {
                sb.AppendFormat(Texture, t[0].ToString("0.000000"), t[1].ToString("0.000000"));
            }

            return sb.ToString();
        }

        private string BuildNormals(Mesh mesh)
        {
            var sb = new StringBuilder();
            foreach (var vn in mesh.Normals)
            {
                sb.AppendFormat(Normal, vn[0].ToString("0.000000"), vn[1].ToString("0.000000"), vn[2].ToString("0.000000"));
            }

            return sb.ToString();
        }

        //private void BuildFaces(Mesh mesh, StringBuilder sb)
        //{
        //    sb.AppendLine("s off"); // smoothing groups off
        //    sb.AppendLine($"g Mesh_{mesh.Id}");
        //    sb.AppendLine($"usemtl Mesh_{mesh.Id}");

        //    for (int i = 0; i < mesh.Indices.Count; i += 3)
        //    {
        //        var a = (ushort)(mesh.Indices[i].Position);
        //        var b = (ushort)(mesh.Indices[i].TextureCoordinate);
        //        var c = (ushort)(mesh.Indices[i].Normal);

        //        var d = (ushort)(mesh.Indices[i + 1].Position);
        //        var e = (ushort)(mesh.Indices[i + 1].TextureCoordinate);
        //        var f = (ushort)(mesh.Indices[i + 1].Normal);

        //        var g = (ushort)(mesh.Indices[i + 2].Position);
        //        var h = (ushort)(mesh.Indices[i + 2].TextureCoordinate);
        //        var j = (ushort)(mesh.Indices[i + 3].Normal);

        //        sb.Append("f " + a + @"/" + b + @"/" + c + " " + d + @"/" + e + @"/" + f + " " + g + @"/" + h + @"/" + j + "\r\n");
        //    }
        //}

        private void BuildFaces(Mesh mesh, StringBuilder sb)
        {
            int currentIndexCount = 0;

            currentIndexCount++;

            // TODO this does not spit out the faces the same as the exporter from Maya does
            sb.AppendLine("s off"); // smoothing groups off
            sb.AppendLine($"g Mesh_{mesh.Id}");
            sb.AppendLine($"usemtl Mesh_{mesh.Id}");

            foreach (var wavefrontVertex in mesh.Indices)
            {
                var a = (ushort)(wavefrontVertex.Position + currentIndexCount);
                var b = (ushort)(wavefrontVertex.TextureCoordinate + currentIndexCount);
                var c = (ushort)(wavefrontVertex.Normal + currentIndexCount);

               sb.Append("f " + a + @"/" + a + @"/" + a + " " + b + @"/" + b + @"/" + b + " " + c + @"/" + c + @"/" + c + "\r\n");
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
                        var textureFile = this.ModelDirectory + "\\" + mesh.Id.ToString(CultureInfo.InvariantCulture).Replace(" ", "_") + "_" + i + ".png";
                        logger?.Debug($"Save texture file {textureFile}");
                        bitmap.Save(textureFile, ImageFormat.Png);

                        // TODO figure this shit out right here
                        var usmtl = string.Format(UseMaterial, $"Mesh_{mesh.Id}");
                        if (this.indexMaterialDictionary[meshName] != string.Empty)
                        {
                            // we're on more than one texture here ... shit
                            this.indexMaterialDictionary.Add($"{meshName}_{i}", textureFile);
                        }
                        else
                        {
                            this.indexMaterialDictionary[meshName] = textureFile;
                        }

                        // this should go before each set of faces.
                        // sb.AppendFormat(UseMaterial, $"Mesh_{mesh.Id}");
                    }
                }
            }
        }

        private void CreateObject(
            Mesh mesh,
            StringBuilder mainStringBuilder,
            StringBuilder materialBuilder,
            string directory,
            string meshName = null)
        {
            var mapFiles = new List<string>();

            if (mesh.Textures.Any())
            {
                var archive = (Textures)ArchiveFactory.Instance.Build(CacheFile.Textures);

                for (var i = 0; i < mesh.Textures.Count(); i++)
                {
                    var texture = mesh.Textures[i];
                    var asset = archive[texture.TextureId];
                    using (var map = mesh.Textures[i].TextureMap(asset.Item1))
                    {
                        var mapName = directory + "\\" + mesh.Id.ToString(CultureInfo.InvariantCulture).Replace(" ", "_") + "_" + i + ".png";
                        mapFiles.Add(mapName);
                        map.Save(mapName, ImageFormat.Png);

                        mainStringBuilder.AppendFormat(UseMaterial, $"Mesh_{mesh.Id}");
                        mainStringBuilder.AppendLine("s off");
                    }
                }
            }


            if (mapFiles.Count > 0)
            {
                this.CreateMaterial(mapFiles[0], materialBuilder, meshName);
            }

            foreach (var v in mesh.Vertices)
            {
                mainStringBuilder.AppendFormat(Vertice, v[0].ToString("0.0#####"), v[1].ToString("0.0#####"),
                    v[2].ToString("0.0#####"));
            }

            foreach (var t in mesh.TextureVectors)
            {
                mainStringBuilder.AppendFormat(Texture, t[0].ToString("0.000000"), t[1].ToString("0.000000"));
            }

            foreach (var vn in mesh.Normals)
            {
                mainStringBuilder.AppendFormat(Normal, vn[0].ToString("0.000000"), vn[1].ToString("0.000000"),
                    vn[2].ToString("0.000000"));
            }

            // TODO this does not spit out the faces the same as the exporter from Maya does
            // g Mesh_124163:default1
            // usemtl initialShadingGroup
            mainStringBuilder.AppendLine($"g Mesh_{mesh.Id}:Mesh_{mesh.Id}");
            mainStringBuilder.AppendLine($"usemtl Mesh_{mesh.Id}");

            foreach (var wavefrontVertex in mesh.Indices)
            {
                var a = (ushort)(wavefrontVertex.Position + 1);
                var b = (ushort)(wavefrontVertex.TextureCoordinate + 1);
                var c = (ushort)(wavefrontVertex.Normal + 1);

                mainStringBuilder.Append("f " + a + @"/" + a + @"/" + a + " " + b + @"/" + b + @"/" + b + " " + c +
                    @"/" + c +
                    @"/" + c + "\r\n");
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

        private void CreateMaterial(string mapName, StringBuilder materialBuilder, string materialName = null)
        {
            materialBuilder.AppendFormat(MaterialName, materialName ?? this.name);
            materialBuilder.Append(MaterialWhite);
            materialBuilder.Append(MaterialDiffuse);
            materialBuilder.Append(MaterialSpecular);
            materialBuilder.Append(MaterialSpecualrNs);
            materialBuilder.Append(MaterialDefaultIllumination);
            materialBuilder.AppendFormat(MapTo, mapName.Substring(mapName.LastIndexOf('\\') + 1));
        }
    }
}