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

        public async Task<bool> ExportAsync(Mesh mesh, string meshName = null)
        {
            try
            {
                var mainStringBuilder = new StringBuilder();
                var materialBuilder = new StringBuilder();
                mainStringBuilder.Append(UsesCentimeters);

                // todo - not all objects seem to have names
                this.name = meshName ?? string.Join(string.Empty, "Mesh_",
                    mesh.CacheIndex.Identity.ToString(CultureInfo.InvariantCulture));

                mainStringBuilder.Append(MayaObjHeaderFactory.Instance.Create(mesh.CacheIndex.Identity));
                mainStringBuilder.AppendFormat(SbMeshId, mesh.CacheIndex.Identity);
                mainStringBuilder.AppendFormat(MaterialLib, this.name);
                mainStringBuilder.Append(DefaultGroup);

                // save the obj
                this.CreateObject(mesh, mainStringBuilder, materialBuilder, this.ModelDirectory);

                using (
                    var fs = new FileStream(
                        this.ModelDirectory + "\\" + this.name + ".obj",
                        FileMode.Create,
                        FileAccess.ReadWrite,
                        FileShare.ReadWrite))
                {
                    using (var writer = new StreamWriter(fs))
                    {
                        await writer.WriteAsync(mainStringBuilder.ToString());
                    }
                }

                // save the material
                var mtlFile = this.ModelDirectory + "\\" + this.name + ".mtl";
                if (File.Exists(mtlFile))
                {
                    File.Delete(mtlFile);
                }

                using (
                    var fs1 = new FileStream(
                        this.ModelDirectory + "\\" + this.name + ".mtl",
                        FileMode.Create,
                        FileAccess.ReadWrite,
                        FileShare.ReadWrite))
                {
                    using (var writer = new StreamWriter(fs1))
                    {
                        await writer.WriteAsync(materialBuilder.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                return false;
            }

            return true;
        }

        public async Task CreateCombinedObjectAsync(List<Mesh> meshModels, string modelName)
        {
            StringBuilder complex = new StringBuilder();
            complex.Append(MayaObjHeaderFactory.Instance.Create(modelName));
            complex.AppendFormat(MaterialLib, modelName);

            StringBuilder material = new StringBuilder();
            foreach (var mesh in meshModels)
            {
                try
                {
                    var mainStringBuilder = new StringBuilder();
                    var materialBuilder = new StringBuilder();
                    mainStringBuilder.Append(UsesCentimeters);

                    // todo - not all objects seem to have names
                    var meshName = string.Join(string.Empty, "Mesh_", mesh.CacheIndex.Identity.ToString(CultureInfo.InvariantCulture));
                    //mainStringBuilder.AppendFormat(SbMeshId, mesh.CacheIndex.Identity);
                    //mainStringBuilder.Append(DefaultGroup);

                    // fill the stringbuilders
                    this.CreateObject(mesh, mainStringBuilder, materialBuilder, this.ModelDirectory, meshName);
                    complex.Append(mainStringBuilder);
                    material.Append(materialBuilder);
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }

                using (
                    var fs = new FileStream(
                        this.ModelDirectory + "\\" + modelName + ".obj",
                        FileMode.Create,
                        FileAccess.ReadWrite,
                        FileShare.ReadWrite))
                {
                    using (var writer = new StreamWriter(fs))
                    {
                        await writer.WriteAsync(complex.ToString());
                    }
                }

                // save the material
                var mtlFile = this.ModelDirectory + "\\" + modelName + ".mtl";
                if (File.Exists(mtlFile))
                {
                    File.Delete(mtlFile);
                }

                //File.WriteAllText(mtlFile, material.ToString());
                using (var fs1 = new FileStream(
                    this.ModelDirectory + "\\" + modelName + ".mtl",
                    FileMode.Create,
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite))
                {
                    using (var writer = new StreamWriter(fs1))
                    {
                        await writer.WriteAsync(material.ToString());
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
                var archive = (Textures) ArchiveFactory.Instance.Build(CacheFile.Textures);

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
                var a = (ushort) (wavefrontVertex.Position + 1);
                var b = (ushort) (wavefrontVertex.TextureCoordinate + 1);
                var c = (ushort) (wavefrontVertex.Normal + 1);

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
            materialBuilder.AppendFormat(MaterialName, materialName??this.name);
            materialBuilder.Append(MaterialWhite);
            materialBuilder.Append(MaterialDiffuse);
            materialBuilder.Append(MaterialSpecular);
            materialBuilder.Append(MaterialSpecualrNs);
            materialBuilder.Append(MaterialDefaultIllumination);
            materialBuilder.AppendFormat(MapTo, mapName.Substring(mapName.LastIndexOf('\\') + 1));
        }
    }
}